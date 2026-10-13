Describe "Assign-AdminUsers" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Assign-AdminUsers.ps1"
        $users = @('reinier.vanmaanen@xebia.com', 'rob.bos@xebia.com')
    }

    BeforeEach {
        $cli = New-CliFake
        $bodies = [System.Collections.Generic.List[string]]::new()
        $bodyFiles = [System.Collections.Generic.List[string]]::new()
        Mock az { Invoke-CliFake -Fake $cli -Command 'az' -Arguments $args }

        Add-CliRule $cli 'az' '^az account show' ''
        Add-CliRule $cli 'az' '^az ad sp list' '{"id":"sp-object-1","appId":"app-1"}'
        Add-CliRule $cli 'az' '^az rest --method GET' '[]'
        Add-CliRule $cli 'az' '^az ad user show --id reinier' 'user-id-1'
        Add-CliRule $cli 'az' '^az ad user show --id rob' 'user-id-2'
        Add-CliRule $cli 'az' '^az rest --method POST' '' -Action {
            param($a)
            $file = "$($a | Where-Object { "$_" -like '@*' } | Select-Object -First 1)".TrimStart('@')
            $bodyFiles.Add($file)
            $bodies.Add((Get-Content $file -Raw))
        }
    }

    Context "Parameters" {
        It "Requires Environment and restricts it to localhost, staging and production" {
            $p = Get-ScriptParameter $scriptPath 'Environment'
            Test-ParameterMandatory $p | Should -BeTrue
            Get-ParameterValidateSet $p | Should -Be @('localhost', 'staging', 'production')
        }

        It "Rejects an unknown environment" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ Environment = 'qa' }

            $result.Error | Should -Not -BeNullOrEmpty
            @(Get-CliCalls $cli '^az') | Should -BeNullOrEmpty
        }
    }

    Context "App lookup" {
        It "Looks up the enterprise app by the display name of the chosen environment" -ForEach @(
            @{ Environment = 'localhost'; DisplayName = 'TechHub Local Dev' }
            @{ Environment = 'staging'; DisplayName = 'TechHub Staging' }
            @{ Environment = 'production'; DisplayName = 'TechHub Production' }
        ) {
            Invoke-ScriptUnderTest $scriptPath @{ Environment = $Environment } | Out-Null

            @(Get-CliCalls $cli '^az ad sp list')[0] | Should -Match "--display-name $DisplayName "
        }

        It "Exits 1 when not logged in to Azure CLI" {
            Add-CliRule $cli 'az' '^az account show' 'ERROR' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath @{ Environment = 'production' }

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^az ad sp list') | Should -BeNullOrEmpty
        }

        It "Exits 1 when the service principal lookup fails" {
            Add-CliRule $cli 'az' '^az ad sp list' '' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath @{ Environment = 'production' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match "Could not find enterprise app 'TechHub Production'"
        }

        It "Exits 1 when the lookup returns an object without an id" {
            Add-CliRule $cli 'az' '^az ad sp list' '{"id":null,"appId":null}' -First

            $result = Invoke-ScriptUnderTest $scriptPath @{ Environment = 'production' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'No enterprise app found'
        }

        It "Exits 1 when existing assignments cannot be fetched" {
            Add-CliRule $cli 'az' '^az rest --method GET' '' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath @{ Environment = 'production' }

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^az rest --method POST') | Should -BeNullOrEmpty
        }
    }

    Context "Assignment" {
        It "Assigns every unassigned admin user through Microsoft Graph" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ Environment = 'production' }

            $result.ExitCode | Should -Be 0
            $posts = @(Get-CliCalls $cli '^az rest --method POST')
            $posts | Should -HaveCount 2
            $posts[0] | Should -Match 'https://graph.microsoft.com/v1.0/servicePrincipals/sp-object-1/appRoleAssignments'
            $result.Output | Should -Match 'Assigned: 2, Already assigned: 0, Failed: 0'
        }

        It "Posts the user, the service principal and the default access role" {
            Invoke-ScriptUnderTest $scriptPath @{ Environment = 'production' } | Out-Null

            $first = $bodies[0] | ConvertFrom-Json
            $first.principalId | Should -Be 'user-id-1'
            $first.resourceId | Should -Be 'sp-object-1'
            $first.appRoleId | Should -Be '00000000-0000-0000-0000-000000000000'
            ($bodies[1] | ConvertFrom-Json).principalId | Should -Be 'user-id-2'
        }

        It "Removes the temporary request body files" {
            Invoke-ScriptUnderTest $scriptPath @{ Environment = 'production' } | Out-Null

            $bodyFiles | Should -HaveCount 2
            foreach ($file in $bodyFiles) { Test-Path $file | Should -BeFalse }
        }

        It "Is idempotent: skips users that already hold an assignment" {
            Add-CliRule $cli 'az' '^az rest --method GET' '["user-id-1"]' -First

            $result = Invoke-ScriptUnderTest $scriptPath @{ Environment = 'production' }

            $result.ExitCode | Should -Be 0
            @(Get-CliCalls $cli '^az rest --method POST') | Should -HaveCount 1
            $result.Output | Should -Match 'Already assigned: reinier.vanmaanen@xebia.com'
            $result.Output | Should -Match 'Assigned: 1, Already assigned: 1, Failed: 0'
        }

        It "Makes no changes when everyone is already assigned" {
            Add-CliRule $cli 'az' '^az rest --method GET' '["user-id-1","user-id-2"]' -First

            $result = Invoke-ScriptUnderTest $scriptPath @{ Environment = 'production' }

            @(Get-CliCalls $cli '^az rest --method POST') | Should -BeNullOrEmpty
            $result.Output | Should -Match 'Assigned: 0, Already assigned: 2, Failed: 0'
        }

        It "Counts unknown directory users as failed and keeps processing the rest" {
            Add-CliRule $cli 'az' '^az ad user show --id reinier' 'not found' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath @{ Environment = 'production' }

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'User not found in directory: reinier.vanmaanen@xebia.com'
            @(Get-CliCalls $cli '^az rest --method POST') | Should -HaveCount 1
            $result.Output | Should -Match 'Assigned: 1, Already assigned: 0, Failed: 1'
        }

        It "Counts a failed POST as failed without aborting" {
            Add-CliRule $cli 'az' '^az rest --method POST' '' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath @{ Environment = 'production' }

            $result.Error | Should -BeNullOrEmpty
            $result.Output | Should -Match 'Failed to assign: reinier.vanmaanen@xebia.com'
            $result.Output | Should -Match 'Assigned: 0, Already assigned: 0, Failed: 2'
        }

        It "Looks up exactly the configured admin users" {
            Invoke-ScriptUnderTest $scriptPath @{ Environment = 'production' } | Out-Null

            $lookups = @(Get-CliCalls $cli '^az ad user show')
            $lookups | Should -HaveCount $users.Count
            foreach ($u in $users) { ($lookups -join "`n") | Should -Match ([regex]::Escape($u)) }
        }
    }
}
