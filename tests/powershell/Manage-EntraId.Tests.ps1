Describe "Manage-EntraId" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Manage-EntraId.ps1"
    }

    Context "Script structure" {
        It "Should exist as a PowerShell script" {
            Test-Path $scriptPath | Should -BeTrue
        }

        It "Should have a valid script block" {
            $content = Get-Content $scriptPath -Raw
            $errors = $null
            [System.Management.Automation.Language.Parser]::ParseInput($content, [ref]$null, [ref]$errors)
            $errors.Count | Should -Be 0
        }

        It "Should set ErrorActionPreference to Stop" {
            $content = Get-Content $scriptPath -Raw
            $content | Should -Match '\$ErrorActionPreference\s*=\s*"Stop"'
        }

        It "Should set StrictMode" {
            $content = Get-Content $scriptPath -Raw
            $content | Should -Match 'Set-StrictMode\s+-Version\s+Latest'
        }
    }

    Context "Parameters" {
        BeforeAll {
            $ast = [System.Management.Automation.Language.Parser]::ParseFile(
                $scriptPath, [ref]$null, [ref]$null
            )
            $paramBlock = $ast.ParamBlock
        }

        It "Should have a mandatory Environment parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "Environment" }
            $param | Should -Not -BeNullOrEmpty
            $mandatory = $param.Attributes | Where-Object {
                $_.TypeName.Name -eq "Parameter" -and
                ($_.NamedArguments | Where-Object { $_.ArgumentName -eq "Mandatory" -and $_.Argument.Extent.Text -eq '$true' })
            }
            $mandatory | Should -Not -BeNullOrEmpty
        }

        It "Should validate Environment to localhost, staging, and production" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "Environment" }
            $validateSet = $param.Attributes | Where-Object { $_.TypeName.Name -eq "ValidateSet" }
            $validateSet | Should -Not -BeNullOrEmpty
            $values = $validateSet.PositionalArguments | ForEach-Object { $_.Value }
            $values | Should -Contain "localhost"
            $values | Should -Contain "staging"
            $values | Should -Contain "production"
        }

        It "Should have an optional DisplayName parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "DisplayName" }
            $param | Should -Not -BeNullOrEmpty
        }

        It "Should have a WebPort parameter defaulting to 5003" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "WebPort" }
            $param | Should -Not -BeNullOrEmpty
            $param.DefaultValue.Value | Should -Be 5003
        }

        It "Should have a SecretExpiryDays parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "SecretExpiryDays" }
            $param | Should -Not -BeNullOrEmpty
        }

        It "Should have a SkipUserAssignment switch parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "SkipUserAssignment" }
            $param | Should -Not -BeNullOrEmpty
            $param.StaticType.Name | Should -Be "SwitchParameter"
        }

        It "Should have a RemoveExpired switch parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "RemoveExpired" }
            $param | Should -Not -BeNullOrEmpty
            $param.StaticType.Name | Should -Be "SwitchParameter"
        }

        It "Should have a GitHubSecretName parameter defaulting to AZURE_AD_CLIENT_SECRET" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "GitHubSecretName" }
            $param | Should -Not -BeNullOrEmpty
            $param.DefaultValue.Value | Should -Be "AZURE_AD_CLIENT_SECRET"
        }

        It "Should have an optional GitHubRepo parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "GitHubRepo" }
            $param | Should -Not -BeNullOrEmpty
        }
    }

    Context "Idempotent app registration" {
        BeforeAll {
            $content = Get-Content $scriptPath -Raw
        }

        It "Should check for existing app registrations before creating" {
            $content | Should -Match 'az ad app list'
        }

        It "Should reuse existing app registration when found" {
            # The script should look up existing apps and skip creation
            $content | Should -Match 'Found existing app registration'
        }

        It "Should only create a new app registration when none exists" {
            $content | Should -Match 'No existing app registration found'
            $content | Should -Match 'az ad app create'
        }
    }

    Context "Secret management" {
        BeforeAll {
            $content = Get-Content $scriptPath -Raw
        }

        It "Should always use append mode for secrets (no invalidation)" {
            $content | Should -Match '--append'
        }

        It "Should list existing credentials" {
            $content | Should -Match 'az ad app credential list'
        }

        It "Should create secrets with a descriptive display name including date" {
            $content | Should -Match "Get-Date.*'yyyy-MM-dd'"
        }

        It "Should note that previous secrets remain valid" {
            $content | Should -Match 'Previous secrets remain valid'
        }

        It "Should support removing only expired secrets" {
            $content | Should -Match 'az ad app credential delete'
        }
    }

    Context "Redirect URIs per environment" {
        BeforeAll {
            $content = Get-Content $scriptPath -Raw
        }

        It "Should configure localhost redirect URI with port" {
            $content | Should -Match 'localhost.*signin-oidc'
        }

        It "Should configure staging redirect URI" {
            $content | Should -Match 'staging-tech\.hub\.ms/signin-oidc'
        }

        It "Should configure production redirect URIs for both domains" {
            $content | Should -Match 'tech\.hub\.ms/signin-oidc'
            $content | Should -Match 'tech\.xebia\.ms/signin-oidc'
        }
    }

    Context "Security requirements" {
        BeforeAll {
            $content = Get-Content $scriptPath -Raw
        }

        It "Should check Azure CLI authentication before proceeding" {
            $content | Should -Match 'az account show'
        }

        It "Should check GitHub CLI for staging/production" {
            $content | Should -Match 'gh auth status'
        }

        It "Should create service principal with assignment required" {
            $content | Should -Match 'appRoleAssignmentRequired.*true'
        }

        It "Should use single-tenant audience" {
            $content | Should -Match 'AzureADMyOrg'
        }

        It "Should expose an Admin.Access API scope" {
            $content | Should -Match 'Admin\.Access'
        }

        It "Should clean up temporary files used for JSON payloads" {
            $content | Should -Match 'Remove-Item.*-Force'
        }
    }

    Context "Secret delivery" {
        BeforeAll {
            $content = Get-Content $scriptPath -Raw
        }

        It "Should refer to Setup-UserSecrets.ps1 for localhost" {
            $content | Should -Match 'Setup-UserSecrets\.ps1'
        }

        It "Should push secrets to GitHub for staging/production" {
            $content | Should -Match 'gh.*secret.*set'
        }

        It "Should use environment-scoped GitHub secrets" {
            $content | Should -Match '--env'
        }

        BeforeEach {
            $cli = New-CliFake
            Mock az { Invoke-CliFake -Fake $cli -Command 'az' -Arguments $args }
            Mock gh { Invoke-CliFake -Fake $cli -Command 'gh' -Arguments $args }

            Add-CliRule $cli 'az' '^az account show' '{"tenantId":"tenant-123","id":"subscription-123"}'
            Add-CliRule $cli 'az' '^az ad app list' '[{"appId":"client-123","id":"object-123"}]'
            Add-CliRule $cli 'az' '^az ad app credential list' '[]'
            Add-CliRule $cli 'az' '^az ad app credential reset' '{"password":"test-client-secret"}'
        }

        It "Updates only the client secret for <Environment>" -ForEach @(
            @{ Environment = 'staging' }
            @{ Environment = 'production' }
        ) {
            $result = Invoke-ScriptUnderTest $scriptPath @{ Environment = $Environment; GitHubRepo = 'techhubms/techhub' }

            $result.Error | Should -BeNullOrEmpty
            $result.ExitCode | Should -Be 0
            $writes = @(Get-CliCalls $cli '^gh secret set')
            $writes | Should -HaveCount 1
            $writes[0] | Should -Be "gh secret set AZURE_AD_CLIENT_SECRET --repo techhubms/techhub --body test-client-secret --env $Environment"
        }

        It "Preserves the custom client secret name" {
            $result = Invoke-ScriptUnderTest $scriptPath @{
                Environment = 'production'
                GitHubRepo = 'techhubms/techhub'
                GitHubSecretName = 'CUSTOM_CLIENT_SECRET'
            }

            $result.Error | Should -BeNullOrEmpty
            $result.ExitCode | Should -Be 0
            $writes = @(Get-CliCalls $cli '^gh secret set')
            $writes | Should -HaveCount 1
            $writes[0] | Should -Be 'gh secret set CUSTOM_CLIENT_SECRET --repo techhubms/techhub --body test-client-secret --env production'
        }

        It "Reports failure when the client secret cannot be updated" {
            Add-CliRule $cli 'gh' '^gh secret set AZURE_AD_CLIENT_SECRET' 'denied' -ExitCode 1

            $result = Invoke-ScriptUnderTest $scriptPath @{ Environment = 'production'; GitHubRepo = 'techhubms/techhub' }

            $result.Error | Should -BeNullOrEmpty
            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match "Failed to set GitHub secret 'AZURE_AD_CLIENT_SECRET'"
            @(Get-CliCalls $cli '^gh secret set') | Should -HaveCount 1
        }

        It "Does not write GitHub secrets for localhost" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ Environment = 'localhost' }

            $result.Error | Should -BeNullOrEmpty
            $result.ExitCode | Should -Be 0
            @(Get-CliCalls $cli '^gh') | Should -BeNullOrEmpty
        }
    }
}
