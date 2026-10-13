Describe "Deploy-Application" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $realScript = Join-Path $PSScriptRoot "../../scripts/Deploy-Application.ps1"

        function New-HttpStatusException {
            param([System.Net.HttpStatusCode]$Status)
            [Microsoft.PowerShell.Commands.HttpResponseException]::new(
                "status $([int]$Status)", [System.Net.Http.HttpResponseMessage]::new($Status))
        }
    }

    BeforeEach {
        $sandboxes = [System.Collections.Generic.List[string]]::new()
        $cli = New-CliFake
        Add-CliRule $cli 'az' '^az account show' '{"name":"Test Subscription"}'
        Add-CliRule $cli 'az' '^az webapp config show --name app-techhub-api-prod' 'DOCKER|ghcr.io/techhubms/techhub-api:previous'
        Add-CliRule $cli 'az' '^az webapp config show --name app-techhub-web-prod' 'DOCKER|ghcr.io/techhubms/techhub-web:previous'
        Add-CliRule $cli 'az' '^az webapp show --name app-techhub-api-prod' 'api.example.net'
        Add-CliRule $cli 'az' '^az webapp show --name app-techhub-web-prod' 'web.example.net'

        $health = [pscustomobject]@{ Uris = [System.Collections.Generic.List[string]]::new(); Handler = { [pscustomobject]@{ StatusCode = 200 } } }
        Mock az { Invoke-CliFake -Fake $cli -Command 'az' -Arguments $args }
        Mock Start-Sleep { }
        Mock Invoke-WebRequest { $health.Uris.Add([string]$Uri); & $health.Handler }

        function Start-Deploy {
            param([int]$WaitExitCode = 0, [hashtable]$Parameters = @{ Tag = '20260501120000' })
            $root = New-ScriptSandbox -Scripts @('Deploy-Application.ps1') -StubScripts @{
                'Wait-ForLiveVersion.ps1' = (New-RecordingStubScript -ExitCode $WaitExitCode)
            }
            $sandboxes.Add($root)
            $result = Invoke-ScriptUnderTest (Join-Path $root 'scripts/Deploy-Application.ps1') $Parameters
            $result | Add-Member -NotePropertyName Sandbox -NotePropertyValue $root -PassThru
        }
    }

    AfterEach {
        foreach ($root in $sandboxes) { Remove-ScriptSandbox $root }
    }

    Context "Parameters" {
        It "Requires Tag" {
            (Test-ParameterMandatory (Get-ScriptParameter $realScript 'Tag')) | Should -BeTrue
        }

        It "Defaults the registry owner to techhubms and has a SkipSmokeTests switch" {
            (Get-ScriptParameter $realScript 'GithubRegistryUsername').DefaultValue.Value | Should -Be 'techhubms'
            (Get-ScriptParameter $realScript 'SkipSmokeTests').StaticType.Name | Should -Be 'SwitchParameter'
        }
    }

    Context "Prerequisites" {
        It "Exits 1 without touching any web app when not logged in to Azure CLI" {
            $cli.Rules.Clear()
            Add-CliRule $cli 'az' '^az account show' 'ERROR' -ExitCode 1

            $result = Start-Deploy

            $result.ExitCode | Should -Be 1
            Get-CliCalls $cli '^az webapp' | Should -BeNullOrEmpty
        }
    }

    Context "Successful deployment" {
        It "Swaps the API image first and then the Web image to the requested tag in the production resource group" {
            $result = Start-Deploy

            $result.ExitCode | Should -Be 0
            $sets = @(Get-CliCalls $cli '^az webapp config set')
            $sets | Should -HaveCount 2
            $sets[0] | Should -Match '--name app-techhub-api-prod --resource-group rg-techhub-prod --linux-fx-version DOCKER\|ghcr.io/techhubms/techhub-api:20260501120000$'
            $sets[1] | Should -Match '--name app-techhub-web-prod --resource-group rg-techhub-prod --linux-fx-version DOCKER\|ghcr.io/techhubms/techhub-web:20260501120000$'
        }

        It "Uses the registry owner passed in GithubRegistryUsername" {
            Start-Deploy -Parameters @{ Tag = 't1'; GithubRegistryUsername = 'someoneelse' } | Out-Null

            $sets = @(Get-CliCalls $cli '^az webapp config set') -join "`n"
            $sets | Should -Match 'ghcr.io/someoneelse/techhub-api:t1'
            $sets | Should -Match 'ghcr.io/someoneelse/techhub-web:t1'
        }

        It "Records the tag in the Web DEPLOY_IMAGE_TAG app setting" {
            Start-Deploy | Out-Null

            $settings = @(Get-CliCalls $cli '^az webapp config appsettings set')
            $settings | Should -HaveCount 1
            $settings[0] | Should -Match '--name app-techhub-web-prod'
            $settings[0] | Should -Match '--settings DEPLOY_IMAGE_TAG=20260501120000'
        }

        It "Never touches PR preview or non-production app names" {
            Start-Deploy | Out-Null

            foreach ($call in @(Get-CliCalls $cli '^az webapp')) {
                $call | Should -Not -Match '-pr-'
                $call | Should -Match 'app-techhub-(api|web)-prod'
            }
        }

        It "Health-checks the API before deploying Web" {
            Start-Deploy | Out-Null

            $health.Uris | Should -Contain 'https://api.example.net/health'
            $apiSetIndex = $cli.Calls.FindIndex({ param($c) $c -match 'config set --name app-techhub-api-prod' })
            $webSetIndex = $cli.Calls.FindIndex({ param($c) $c -match 'config set --name app-techhub-web-prod' })
            $apiSetIndex | Should -BeLessThan $webSetIndex
        }

        It "Delegates to Wait-ForLiveVersion.ps1 with the Web FQDN and tag" {
            $result = Start-Deploy

            $calls = @(Get-StubScriptCalls $result.Sandbox)
            $calls | Should -HaveCount 1
            $calls[0] | Should -Match 'Wait-ForLiveVersion.ps1 .*web.example.net'
            $calls[0] | Should -Match '20260501120000'
            $calls[0] | Should -Not -Match 'SkipSmokeTests'
        }

        It "Passes -SkipSmokeTests through to Wait-ForLiveVersion.ps1" {
            $result = Start-Deploy -Parameters @{ Tag = 't1'; SkipSmokeTests = $true }

            @(Get-StubScriptCalls $result.Sandbox)[0] | Should -Match '-SkipSmokeTests'
        }

        It "Does not roll back when the version gate passes" {
            Start-Deploy | Out-Null

            @(Get-CliCalls $cli '^az webapp config set') | Should -HaveCount 2
        }
    }

    Context "API health gate" {
        It "Continues when the API answers 403 (blocked by VNet access restrictions)" {
            $health.Handler = { throw (New-HttpStatusException ([System.Net.HttpStatusCode]::Forbidden)) }

            $result = Start-Deploy

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'returned 403'
            @(Get-CliCalls $cli '^az webapp config set') | Should -HaveCount 2
        }

        It "Retries 30 times, then fails with exit 1 without deploying Web when the API stays unhealthy" {
            $health.Handler = { throw (New-HttpStatusException ([System.Net.HttpStatusCode]::ServiceUnavailable)) }

            $result = Start-Deploy

            $result.ExitCode | Should -Be 1
            $health.Uris.Count | Should -Be 30
            $result.Output | Should -Match 'failed to become healthy after 30 attempts'
            @(Get-CliCalls $cli '^az webapp config set') | Should -HaveCount 1
            Get-StubScriptCalls $result.Sandbox | Should -BeNullOrEmpty
        }

        It "Skips the health check with a warning when the API hostname cannot be resolved" {
            Add-CliRule $cli 'az' '^az webapp show --name app-techhub-api-prod' '' -First

            $result = Start-Deploy

            $result.ExitCode | Should -Be 0
            $health.Uris | Should -BeNullOrEmpty
        }
    }

    Context "Failures" {
        It "Exits 1 and does not deploy Web when the API image swap fails" {
            Add-CliRule $cli 'az' '^az webapp config set --name app-techhub-api-prod' '' -ExitCode 1

            $result = Start-Deploy

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Failed to deploy API'
            @(Get-CliCalls $cli '^az webapp config set --name app-techhub-web-prod') | Should -BeNullOrEmpty
        }

        It "Exits 1 when the DEPLOY_IMAGE_TAG setting cannot be written" {
            Add-CliRule $cli 'az' '^az webapp config appsettings set' '' -ExitCode 1

            $result = Start-Deploy

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'DEPLOY_IMAGE_TAG'
            Get-StubScriptCalls $result.Sandbox | Should -BeNullOrEmpty
        }

        It "Exits 1 when the Web hostname cannot be retrieved" {
            $cli.Rules.Clear()
            Add-CliRule $cli 'az' '^az account show' '{"name":"s"}'
            Add-CliRule $cli 'az' '^az webapp show --name app-techhub-api-prod' 'api.example.net'
            Add-CliRule $cli 'az' '^az webapp show --name app-techhub-web-prod' ''

            $result = Start-Deploy

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Could not retrieve Web app hostname'
        }
    }

    Context "Rollback" {
        It "Restores the previous API and Web images and exits 1 when the version gate fails" {
            $result = Start-Deploy -WaitExitCode 1

            $result.ExitCode | Should -Be 1
            $sets = @(Get-CliCalls $cli '^az webapp config set')
            $sets | Should -HaveCount 4
            $sets[2] | Should -Match '--name app-techhub-api-prod .*--linux-fx-version DOCKER\|ghcr.io/techhubms/techhub-api:previous$'
            $sets[3] | Should -Match '--name app-techhub-web-prod .*--linux-fx-version DOCKER\|ghcr.io/techhubms/techhub-web:previous$'
            $result.Output | Should -Match 'Rollback complete'
        }

        It "Cannot roll back when no previous image was recorded, but still exits 1" {
            Add-CliRule $cli 'az' '^az webapp config show' '' -First

            $result = Start-Deploy -WaitExitCode 1

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^az webapp config set') | Should -HaveCount 2
            $result.Output | Should -Not -Match 'Rollback complete'
        }
    }
}
