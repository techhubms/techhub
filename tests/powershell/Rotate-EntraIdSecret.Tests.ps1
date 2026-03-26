Describe "Rotate-EntraIdSecret" {
    BeforeAll {
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Rotate-EntraIdSecret.ps1"
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

        It "Should have a mandatory ClientId parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "ClientId" }
            $param | Should -Not -BeNullOrEmpty
            $mandatory = $param.Attributes | Where-Object {
                $_.TypeName.Name -eq "Parameter" -and
                ($_.NamedArguments | Where-Object { $_.ArgumentName -eq "Mandatory" -and $_.Argument.Extent.Text -eq '$true' })
            }
            $mandatory | Should -Not -BeNullOrEmpty
        }

        It "Should have a SecretExpiryDays parameter defaulting to 180" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "SecretExpiryDays" }
            $param | Should -Not -BeNullOrEmpty
            $param.DefaultValue.Value | Should -Be 180
        }

        It "Should have a GitHubSecretName parameter defaulting to AZURE_AD_CLIENT_SECRET" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "GitHubSecretName" }
            $param | Should -Not -BeNullOrEmpty
            $param.DefaultValue.Value | Should -Be "AZURE_AD_CLIENT_SECRET"
        }

        It "Should have an Environment parameter with staging and production values" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "Environment" }
            $param | Should -Not -BeNullOrEmpty
            $validateSet = $param.Attributes | Where-Object { $_.TypeName.Name -eq "ValidateSet" }
            $validateSet | Should -Not -BeNullOrEmpty
            $values = $validateSet.PositionalArguments | ForEach-Object { $_.Value }
            $values | Should -Contain "staging"
            $values | Should -Contain "production"
        }

        It "Should have a RemoveExpired switch parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "RemoveExpired" }
            $param | Should -Not -BeNullOrEmpty
            $param.StaticType.Name | Should -Be "SwitchParameter"
        }

        It "Should have a SkipGitHub switch parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "SkipGitHub" }
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

        It "Should verify app registration exists before rotating" {
            $content | Should -Match 'az ad app show'
        }

        It "Should check GitHub CLI authentication when updating secrets" {
            $content | Should -Match 'gh auth status'
        }

        It "Should use append mode to avoid invalidating existing secrets" {
            $content | Should -Match '--append'
        }

        It "Should support removing expired secrets" {
            $content | Should -Match 'az ad app credential delete'
        }

        It "Should support environment-scoped GitHub secrets" {
            $content | Should -Match '--env'
        }
    }

    Context "Secret management" {
        BeforeAll {
            $content = Get-Content $scriptPath -Raw
        }

        It "Should list existing credentials before creating new ones" {
            $content | Should -Match 'az ad app credential list'
        }

        It "Should create secrets with a descriptive display name including date" {
            $content | Should -Match "Get-Date.*'yyyy-MM-dd'"
        }

        It "Should support using gh secret set for GitHub integration" {
            $content | Should -Match 'gh.*secret.*set'
        }
    }
}
