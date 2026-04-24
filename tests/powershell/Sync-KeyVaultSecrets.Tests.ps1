Describe "Sync-KeyVaultSecrets" {
    BeforeAll {
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Sync-KeyVaultSecrets.ps1"
    }

    BeforeEach {
        . $PSScriptRoot/Initialize-BeforeEach.ps1
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

        It "Should validate Environment to staging and prod" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "Environment" }
            $validateSet = $param.Attributes | Where-Object { $_.TypeName.Name -eq "ValidateSet" }
            $validateSet | Should -Not -BeNullOrEmpty
            $values = $validateSet.PositionalArguments | ForEach-Object { $_.Value }
            $values | Should -Contain "staging"
            $values | Should -Contain "prod"
        }

        It "Should have a KeyVaultName parameter defaulting to kv-techhub-shared" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "KeyVaultName" }
            $param | Should -Not -BeNullOrEmpty
            $param.DefaultValue.Value | Should -Be "kv-techhub-shared"
        }

        It "Should have an optional PostgresHost parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "PostgresHost" }
            $param | Should -Not -BeNullOrEmpty
        }
    }

    Context "Key Vault firewall IP management" {
        It "Should call Invoke-RestMethod to detect the current IP" {
            $content = Get-Content $scriptPath -Raw
            $content | Should -Match "Invoke-RestMethod"
        }

        It "Should reference checkip.amazonaws.com for IP detection" {
            $content = Get-Content $scriptPath -Raw
            $content | Should -Match "checkip\.amazonaws\.com"
        }

        It "Should add the current IP to the Key Vault firewall before writing secrets" {
            $content = Get-Content $scriptPath -Raw
            $content | Should -Match "az keyvault network-rule add"
        }

        It "Should remove the current IP from the Key Vault firewall in a finally block" {
            $content = Get-Content $scriptPath -Raw
            # Verify both 'finally' and 'network-rule remove' are present
            $content | Should -Match "finally"
            $content | Should -Match "az keyvault network-rule remove"
        }

        It "Should only remove the IP if it was added by this script" {
            $content = Get-Content $scriptPath -Raw
            # The removal is guarded by $ipWasAdded
            $content | Should -Match '\$ipWasAdded'
        }

        It "Should not throw if IP removal fails in the finally block" {
            $content = Get-Content $scriptPath -Raw
            # Uses Write-Warning instead of throw for cleanup failures
            $addSection = $content -replace '(?s).*finally\s*\{', ''
            $addSection | Should -Match "Write-Warning"
        }
    }

    Context "Secret writing" {
        It "Should write the database connection string secret" {
            $content = Get-Content $scriptPath -Raw
            $content | Should -Match "db-connection-string"
        }

        It "Should write the AI API key secret" {
            $content = Get-Content $scriptPath -Raw
            $content | Should -Match "ai-api-key"
        }

        It "Should write the AAD client secret" {
            $content = Get-Content $scriptPath -Raw
            $content | Should -Match "aad-client-secret"
        }

        It "Should use a temp file to write secret values (prevents exposure in process args)" {
            $content = Get-Content $scriptPath -Raw
            $content | Should -Match "GetTempFileName"
            $content | Should -Match "--file"
        }
    }
}
