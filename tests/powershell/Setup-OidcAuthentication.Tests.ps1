Describe "Setup-OidcAuthentication" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Setup-OidcAuthentication.ps1"
        $defaultAppId = '91fe5f39-bee3-48b1-b976-e8ba4e41d84e'
    }

    BeforeEach {
        $cli = New-CliFake
        $created = [System.Collections.Generic.List[object]]::new()
        Mock az { Invoke-CliFake -Fake $cli -Command 'az' -Arguments $args }
        Mock gh { Invoke-CliFake -Fake $cli -Command 'gh' -Arguments $args }

        Add-CliRule $cli 'az' '^az account show' '{"tenantId":"tenant-1","id":"sub-1","name":"Test Subscription"}'
        Add-CliRule $cli 'gh' '^gh auth status' 'Logged in'
        Add-CliRule $cli 'az' '^az ad app show' '{"displayName":"sp-techhubms","appId":"91fe5f39-bee3-48b1-b976-e8ba4e41d84e"}'
        Add-CliRule $cli 'az' '^az ad app federated-credential list' '[]'
        Add-CliRule $cli 'az' '^az ad app federated-credential create' '' -Action {
            param($a)
            $file = "$($a[([array]::IndexOf(@($a | ForEach-Object { "$_" }), '--parameters') + 1)])"
            $created.Add((Get-Content $file -Raw | ConvertFrom-Json))
        }
    }

    Context "Parameters" {
        It "Defaults to the TechHub service principal and repository" {
            (Get-ScriptParameter $scriptPath 'AppId').DefaultValue.Value | Should -Be $defaultAppId
            (Get-ScriptParameter $scriptPath 'GitHubRepo').DefaultValue.Value | Should -Be 'techhubms/techhub'
            Test-ParameterMandatory (Get-ScriptParameter $scriptPath 'AppId') | Should -BeFalse
        }
    }

    Context "Prerequisites" {
        It "Exits 1 when not logged in to Azure" {
            Add-CliRule $cli 'az' '^az account show' 'ERROR' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^gh') | Should -BeNullOrEmpty
        }

        It "Exits 1 when not logged in to GitHub" {
            Add-CliRule $cli 'gh' '^gh auth status' 'not logged in' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^az ad app') | Should -BeNullOrEmpty
        }

        It "Exits 1 when the app registration does not exist" {
            Add-CliRule $cli 'az' '^az ad app show' 'not found' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match "App registration '$defaultAppId' not found"
            @(Get-CliCalls $cli '^az ad app federated-credential create') | Should -BeNullOrEmpty
        }
    }

    Context "Federated credentials" {
        It "Creates the four GitHub Actions credentials against the GitHub OIDC issuer" {
            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 0
            $created | Should -HaveCount 4
            $created.name | Should -Be @('techhub-environment-staging', 'techhub-environment-production', 'techhub-ref-main', 'techhub-pull-request')
            foreach ($cred in $created) {
                $cred.issuer | Should -Be 'https://token.actions.githubusercontent.com'
                $cred.audiences | Should -Be @('api://AzureADTokenExchange')
            }
        }

        It "Binds each credential to the exact repository subject" {
            Invoke-ScriptUnderTest $scriptPath | Out-Null

            $created.subject | Should -Be @(
                'repo:techhubms/techhub:environment:staging'
                'repo:techhubms/techhub:environment:production'
                'repo:techhubms/techhub:ref:refs/heads/main'
                'repo:techhubms/techhub:pull_request'
            )
        }

        It "Uses the GitHubRepo parameter for subjects and variables" {
            Invoke-ScriptUnderTest $scriptPath @{ GitHubRepo = 'someone/else' } | Out-Null

            $created[0].subject | Should -Be 'repo:someone/else:environment:staging'
            foreach ($call in @(Get-CliCalls $cli '^gh variable set')) { $call | Should -Match '--repo someone/else$' }
        }

        It "Targets the AppId parameter" {
            Invoke-ScriptUnderTest $scriptPath @{ AppId = 'custom-app-id' } | Out-Null

            foreach ($call in @(Get-CliCalls $cli '^az ad app (show|federated-credential)')) { $call | Should -Match '--id custom-app-id' }
        }

        It "Is idempotent: creates only credentials that do not exist yet" {
            Add-CliRule $cli 'az' '^az ad app federated-credential list' '[{"name":"techhub-ref-main"},{"name":"techhub-pull-request"}]' -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 0
            $created.name | Should -Be @('techhub-environment-staging', 'techhub-environment-production')
            $result.Output | Should -Match '\[SKIP\] techhub-ref-main'
        }

        It "Exits 1 when the existing credentials cannot be listed" {
            Add-CliRule $cli 'az' '^az ad app federated-credential list' 'denied' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^gh variable set') | Should -BeNullOrEmpty
        }

        It "Exits 1 without setting variables when a credential cannot be created" {
            Add-CliRule $cli 'az' '^az ad app federated-credential create' 'conflict' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match "Failed to create 'techhub-environment-staging'"
            @(Get-CliCalls $cli '^gh variable set') | Should -BeNullOrEmpty
        }
    }

    Context "GitHub repository variables" {
        It "Stores only identifiers (client, tenant and subscription id) as variables, never a secret" {
            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 0
            $sets = @(Get-CliCalls $cli '^gh variable set')
            $sets | Should -HaveCount 3
            $sets -join "`n" | Should -Match "gh variable set AZURE_CLIENT_ID --body $defaultAppId --repo techhubms/techhub"
            $sets -join "`n" | Should -Match 'gh variable set AZURE_TENANT_ID --body tenant-1'
            $sets -join "`n" | Should -Match 'gh variable set AZURE_SUBSCRIPTION_ID --body sub-1'
            @(Get-CliCalls $cli '^gh secret') | Should -BeNullOrEmpty
        }

        It "Exits 1 when a variable cannot be set" {
            Add-CliRule $cli 'gh' '^gh variable set' 'forbidden' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Failed to set AZURE_'
        }
    }
}
