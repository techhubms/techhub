Describe "Cleanup-ContainerRegistry" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Cleanup-ContainerRegistry.ps1"

        function New-ManifestJson {
            # Spec: @{ Digest; Tags (array or $null); AgeDays }
            param([hashtable[]]$Manifests)
            $items = foreach ($m in $Manifests) {
                [ordered]@{
                    digest         = $m.Digest
                    tags           = $m.Tags
                    lastUpdateTime = (Get-Date -AsUTC).AddDays(-$m.AgeDays).ToString('o')
                }
            }
            ConvertTo-Json -InputObject @($items) -Depth 5 -Compress
        }
    }

    BeforeEach {
        $cli = New-CliFake
        Mock az { Invoke-CliFake -Fake $cli -Command 'az' -Arguments $args }
    }

    Context "Parameters" {
        It "Defaults to registry crtechhubms, all repositories and 7 days" {
            (Get-ScriptParameter $scriptPath 'RegistryName').DefaultValue.Value | Should -Be 'crtechhubms'
            (Get-ScriptParameter $scriptPath 'Repository').DefaultValue.Value | Should -Be 'all'
            (Get-ScriptParameter $scriptPath 'AgeDays').DefaultValue.Value | Should -Be 7
        }

        It "Has a DryRun switch" {
            (Get-ScriptParameter $scriptPath 'DryRun').StaticType.Name | Should -Be 'SwitchParameter'
        }
    }

    Context "Repository selection" {
        It "Enumerates all repositories when Repository is 'all'" {
            Add-CliRule $cli 'az' '^az acr repository list' '["techhub-api","techhub-web"]'
            Add-CliRule $cli 'az' '^az acr manifest list-metadata' '[]'

            $result = Invoke-ScriptUnderTest $scriptPath @{ DryRun = $true }

            $result.ExitCode | Should -Be 0
            @(Get-CliCalls $cli '^az acr repository list --name crtechhubms') | Should -HaveCount 1
            @(Get-CliCalls $cli '--name techhub-api ') | Should -HaveCount 1
            @(Get-CliCalls $cli '--name techhub-web ') | Should -HaveCount 1
        }

        It "Scans only the named repository without listing the registry" {
            Add-CliRule $cli 'az' '^az acr manifest list-metadata' '[]'

            Invoke-ScriptUnderTest $scriptPath @{ Repository = 'techhub-api'; DryRun = $true } | Out-Null

            Get-CliCalls $cli '^az acr repository list' | Should -BeNullOrEmpty
            @(Get-CliCalls $cli '^az acr manifest list-metadata') | Should -HaveCount 1
        }

        It "Throws when the repository list cannot be retrieved" {
            Add-CliRule $cli 'az' '^az acr repository list' '' -ExitCode 1

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.Error | Should -Not -BeNullOrEmpty
            $result.Error.Exception.Message | Should -Match "Failed to list repositories on 'crtechhubms'"
            Get-CliCalls $cli ' delete' | Should -BeNullOrEmpty
        }

        It "Targets the registry passed in RegistryName" {
            Add-CliRule $cli 'az' '^az acr manifest list-metadata' '[]'

            Invoke-ScriptUnderTest $scriptPath @{ RegistryName = 'myreg'; Repository = 'r'; DryRun = $true } | Out-Null

            @(Get-CliCalls $cli '^az acr manifest list-metadata --registry myreg --name r') | Should -HaveCount 1
        }
    }

    Context "Deletion rules" {
        BeforeEach {
            $manifests = New-ManifestJson @(
                @{ Digest = 'sha256:old-untagged'; Tags = $null; AgeDays = 30 }
                @{ Digest = 'sha256:old-empty-tags'; Tags = @(); AgeDays = 10 }
                @{ Digest = 'sha256:recent-untagged'; Tags = $null; AgeDays = 1 }
                @{ Digest = 'sha256:old-tagged'; Tags = @('latest'); AgeDays = 90 }
            )
            Add-CliRule $cli 'az' '^az acr manifest list-metadata' $manifests
        }

        It "Deletes only untagged manifests older than the cutoff" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ Repository = 'techhub-api' }

            $result.ExitCode | Should -Be 0
            $deletes = @(Get-CliCalls $cli '^az acr repository delete')
            $deletes | Should -HaveCount 2
            ($deletes -join "`n") | Should -Match 'techhub-api@sha256:old-untagged'
            ($deletes -join "`n") | Should -Match 'techhub-api@sha256:old-empty-tags'
            ($deletes -join "`n") | Should -Not -Match 'recent-untagged'
            ($deletes -join "`n") | Should -Not -Match 'old-tagged'
            $result.Output | Should -Match 'Deleted 2 manifest'
        }

        It "Never deletes a tagged manifest regardless of its age" {
            Invoke-ScriptUnderTest $scriptPath @{ Repository = 'techhub-api'; AgeDays = 0 } | Out-Null

            @(Get-CliCalls $cli '^az acr repository delete') -join "`n" | Should -Not -Match 'old-tagged'
        }

        It "Honours a custom AgeDays cutoff" {
            Invoke-ScriptUnderTest $scriptPath @{ Repository = 'techhub-api'; AgeDays = 20 } | Out-Null

            $deletes = @(Get-CliCalls $cli '^az acr repository delete')
            $deletes | Should -HaveCount 1
            $deletes[0] | Should -Match 'old-untagged'
        }

        It "Passes --yes so the delete is non-interactive" {
            Invoke-ScriptUnderTest $scriptPath @{ Repository = 'techhub-api' } | Out-Null

            foreach ($call in @(Get-CliCalls $cli '^az acr repository delete')) {
                $call | Should -Match '--yes'
                $call | Should -Match '--name crtechhubms'
            }
        }

        It "Deletes nothing and reports candidates when DryRun is set" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ Repository = 'techhub-api'; DryRun = $true }

            $result.ExitCode | Should -Be 0
            Get-CliCalls $cli '^az acr repository delete' | Should -BeNullOrEmpty
            $result.Output | Should -Match '\[dry-run\] Would delete techhub-api@sha256:old-untagged'
            $result.Output | Should -Match 'Dry run complete'
        }

        It "Does not count a failed delete and continues with the next manifest" {
            Add-CliRule $cli 'az' 'repository delete .*old-untagged' '' -ExitCode 1

            $result = Invoke-ScriptUnderTest $scriptPath @{ Repository = 'techhub-api' }

            @(Get-CliCalls $cli '^az acr repository delete') | Should -HaveCount 2
            $result.Output | Should -Match 'Failed to delete techhub-api@sha256:old-untagged'
            $result.Output | Should -Match 'Deleted 1 manifest'
        }
    }

    Context "Failure handling" {
        It "Skips a repository whose manifests cannot be listed and continues with the others" {
            Add-CliRule $cli 'az' '^az acr repository list' '["broken","good"]'
            Add-CliRule $cli 'az' 'list-metadata --registry crtechhubms --name broken' '' -ExitCode 1
            Add-CliRule $cli 'az' 'list-metadata --registry crtechhubms --name good' (New-ManifestJson @(@{ Digest = 'sha256:g1'; Tags = $null; AgeDays = 20 }))

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match "Failed to enumerate manifests for 'broken'"
            $deletes = @(Get-CliCalls $cli '^az acr repository delete')
            $deletes | Should -HaveCount 1
            $deletes[0] | Should -Match 'good@sha256:g1'
        }

        It "Is idempotent: a registry with no candidates deletes nothing" {
            Add-CliRule $cli 'az' '^az acr manifest list-metadata' (New-ManifestJson @(@{ Digest = 'sha256:t'; Tags = @('x'); AgeDays = 50 }))

            $result = Invoke-ScriptUnderTest $scriptPath @{ Repository = 'r' }

            Get-CliCalls $cli '^az acr repository delete' | Should -BeNullOrEmpty
            $result.Output | Should -Match 'Deleted 0 manifest'
        }
    }
}
