Describe "Setup-EntraIdDev" {
    BeforeAll {
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Setup-EntraIdDev.ps1"
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

        It "Should have a DisplayName parameter with default value" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "DisplayName" }
            $param | Should -Not -BeNullOrEmpty
            $param.DefaultValue.Value | Should -Be "TechHub Local Dev"
        }

        It "Should have a WebPort parameter defaulting to 5003" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "WebPort" }
            $param | Should -Not -BeNullOrEmpty
            $param.DefaultValue.Value | Should -Be 5003
        }

        It "Should have a SecretExpiryDays parameter defaulting to 90" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "SecretExpiryDays" }
            $param | Should -Not -BeNullOrEmpty
            $param.DefaultValue.Value | Should -Be 90
        }

        It "Should have a SkipUserAssignment switch parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "SkipUserAssignment" }
            $param | Should -Not -BeNullOrEmpty
            $param.StaticType.Name | Should -Be "SwitchParameter"
        }
    }

    Context "Security requirements" {
        BeforeAll {
            $content = Get-Content $scriptPath -Raw
        }

        It "Should check Azure CLI authentication before proceeding" {
            $content | Should -Match 'az account show'
        }

        It "Should check for existing app registrations to prevent duplicates" {
            $content | Should -Match 'az ad app list'
        }

        It "Should create a service principal with assignment required" {
            $content | Should -Match 'appRoleAssignmentRequired.*true'
        }

        It "Should configure redirect URIs for localhost" {
            $content | Should -Match 'localhost.*signin-oidc'
        }

        It "Should expose an Admin.Access API scope" {
            $content | Should -Match 'Admin\.Access'
        }

        It "Should clean up temporary files used for JSON payloads" {
            $content | Should -Match 'Remove-Item.*-Force'
        }

        It "Should use single-tenant audience" {
            $content | Should -Match 'AzureADMyOrg'
        }
    }

    Context "Output format" {
        BeforeAll {
            $content = Get-Content $scriptPath -Raw
        }

        It "Should output TenantId" {
            $content | Should -Match 'TenantId'
        }

        It "Should output ClientId" {
            $content | Should -Match 'ClientId'
        }

        It "Should output ClientSecret" {
            $content | Should -Match 'ClientSecret'
        }

        It "Should output Scopes" {
            $content | Should -Match 'Scopes'
        }

        It "Should provide user-secrets setup instructions" {
            $content | Should -Match 'dotnet user-secrets set'
        }

        It "Should provide environment variable instructions" {
            $content | Should -Match 'AzureAd__TenantId'
        }
    }
}
