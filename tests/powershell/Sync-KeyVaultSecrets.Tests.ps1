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

        It "Should use fallback IP providers if the primary one fails" {
            $content = Get-Content $scriptPath -Raw
            # Primary and fallback providers must all be referenced
            $content | Should -Match "checkip\.amazonaws\.com"
            $content | Should -Match "api\.ipify\.org"
            $content | Should -Match "icanhazip\.com"
        }

        It "Should validate the IP response is a valid IPv4 address" {
            $content = Get-Content $scriptPath -Raw
            $content | Should -Match '\\d\{1,3\}'
        }

        It "Should remove the current IP from the Key Vault firewall in a finally block" {
            $content = Get-Content $scriptPath -Raw
            $tokens = $null
            $errors = $null
            $ast = [System.Management.Automation.Language.Parser]::ParseInput($content, [ref]$tokens, [ref]$errors)
            $errors.Count | Should -Be 0
            # Find the specific TryStatementAst whose TryBlock contains both the firewall rule add
            # and Set-KvSecret calls, and whose Finally contains the firewall rule remove.
            # Using AST is more precise than grepping for 'finally' — the script already has a
            # finally block in Set-KvSecret for temp-file cleanup, which would cause a false pass.
            $firewallCleanupTry = $ast.FindAll({
                param($node)
                if ($node -isnot [System.Management.Automation.Language.TryStatementAst]) { return $false }
                # TryStatementAst.Body is the try block; TryBlock is not a valid property name.
                if ($null -eq $node.Finally -or $null -eq $node.Body) { return $false }
                $tryText = $node.Body.Extent.Text
                $finallyText = $node.Finally.Extent.Text
                return $tryText -match 'az keyvault network-rule add' -and
                    $tryText -match 'Set-KvSecret' -and
                    $finallyText -match 'az keyvault network-rule remove'
            }, $true)
            @($firewallCleanupTry).Count | Should -BeGreaterThan 0
        }

        It "Should only remove the IP if it was added by this script" {
            $content = Get-Content $scriptPath -Raw
            # The removal should be guarded by an explicit if ($ipWasAdded) check
            $guardMatch = [regex]::Match($content, 'if\s*\(\s*\$ipWasAdded\s*\)')
            $guardMatch.Success | Should -BeTrue
            # And the removal command must appear after the guard
            $removeIndex = $content.IndexOf('az keyvault network-rule remove')
            $removeIndex | Should -BeGreaterThan $guardMatch.Index
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
