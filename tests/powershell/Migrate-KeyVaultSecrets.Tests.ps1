Describe "Migrate-KeyVaultSecrets" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Migrate-KeyVaultSecrets.ps1"
    }

    BeforeEach {
        $cli = New-CliFake
        $written = [System.Collections.Generic.List[hashtable]]::new()
        Mock az { Invoke-CliFake -Fake $cli -Command 'az' -Arguments $args }

        Add-CliRule $cli 'az' '^az account show' '{"name":"Test Subscription"}'
        Add-CliRule $cli 'az' 'secret show --vault-name kv-techhub-shared --name techhub-prod-aad-client-secret' 'aad-secret-value'
        Add-CliRule $cli 'az' 'secret show --vault-name kv-techhub-shared --name wildcard-hub-ms' 'hub-pfx-base64'
        Add-CliRule $cli 'az' 'secret show --vault-name kv-techhub-shared --name wildcard-xebia-ms' 'xebia-pfx-base64'
        # Capture what is written (file content at call time) and where the temp file lives.
        Add-CliRule $cli 'az' '^az keyvault secret set' '' -Action {
            param($a)
            $strings = @($a | ForEach-Object { "$_" })
            $file = $strings[[array]::IndexOf($strings, '--file') + 1]
            $written.Add(@{
                    Vault = $strings[[array]::IndexOf($strings, '--vault-name') + 1]
                    Name  = $strings[[array]::IndexOf($strings, '--name') + 1]
                    File  = $file
                    Value = [System.IO.File]::ReadAllText($file)
                })
        }
    }

    Context "Parameters" {
        It "Defaults to copying from kv-techhub-shared to kv-techhub-prod" {
            (Get-ScriptParameter $scriptPath 'SourceKeyVaultName').DefaultValue.Value | Should -Be 'kv-techhub-shared'
            (Get-ScriptParameter $scriptPath 'TargetKeyVaultName').DefaultValue.Value | Should -Be 'kv-techhub-prod'
        }
    }

    Context "Prerequisites" {
        It "Exits 1 when not logged in" {
            Add-CliRule $cli 'az' '^az account show' 'ERROR' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^az keyvault') | Should -BeNullOrEmpty
        }

        It "Exits 1 before copying anything when the source vault is not accessible" {
            Add-CliRule $cli 'az' 'secret list --vault-name kv-techhub-shared' 'Forbidden' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match "Cannot access source Key Vault 'kv-techhub-shared'"
            @(Get-CliCalls $cli 'secret set') | Should -BeNullOrEmpty
        }

        It "Exits 1 before copying anything when the target vault is not accessible" {
            Add-CliRule $cli 'az' 'secret list --vault-name kv-techhub-prod' 'Forbidden' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match "Cannot access target Key Vault 'kv-techhub-prod'"
            @(Get-CliCalls $cli 'secret set') | Should -BeNullOrEmpty
        }
    }

    Context "Copying secrets" {
        It "Copies the AAD client secret and both wildcard certificates with unchanged names" {
            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 0
            $written | Should -HaveCount 3
            $written.Name | Should -Be @('techhub-prod-aad-client-secret', 'wildcard-hub-ms', 'wildcard-xebia-ms')
            $written.Value | Should -Be @('aad-secret-value', 'hub-pfx-base64', 'xebia-pfx-base64')
            foreach ($w in $written) { $w.Vault | Should -Be 'kv-techhub-prod' }
        }

        It "Reads from the source vault and writes only to the target vault" {
            Invoke-ScriptUnderTest $scriptPath | Out-Null

            foreach ($call in @(Get-CliCalls $cli 'secret show')) { $call | Should -Match '--vault-name kv-techhub-shared' }
            foreach ($call in @(Get-CliCalls $cli 'secret set')) { $call | Should -Match '--vault-name kv-techhub-prod' }
        }

        It "Uses the custom vault names" {
            Add-CliRule $cli 'az' 'secret show --vault-name old-kv --name techhub-prod-aad-client-secret' 'v'
            Add-CliRule $cli 'az' 'secret list' ''

            Invoke-ScriptUnderTest $scriptPath @{ SourceKeyVaultName = 'old-kv'; TargetKeyVaultName = 'new-kv' } | Out-Null

            $written[0].Vault | Should -Be 'new-kv'
        }

        It "Passes the value through a temp file, never as a command-line argument, and deletes the file" {
            Invoke-ScriptUnderTest $scriptPath | Out-Null

            foreach ($call in @(Get-CliCalls $cli 'secret set')) {
                $call | Should -Not -Match '--value'
                $call | Should -Not -Match 'aad-secret-value|hub-pfx-base64|xebia-pfx-base64'
            }
            foreach ($w in $written) { Test-Path $w.File | Should -BeFalse }
        }

        It "Skips secrets missing from the source vault with a warning and keeps going" {
            Add-CliRule $cli 'az' 'secret show .*--name wildcard-hub-ms' '' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match "wildcard-hub-ms not found in 'kv-techhub-shared' — skipping"
            $written.Name | Should -Be @('techhub-prod-aad-client-secret', 'wildcard-xebia-ms')
        }

        It "Treats an empty source value as missing" {
            Add-CliRule $cli 'az' 'secret show .*--name techhub-prod-aad-client-secret' '  ' -First

            Invoke-ScriptUnderTest $scriptPath | Out-Null

            $written.Name | Should -Not -Contain 'techhub-prod-aad-client-secret'
        }

        It "Exits 1 when writing to the target vault fails" {
            Add-CliRule $cli 'az' '^az keyvault secret set' '' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match "Failed to write 'techhub-prod-aad-client-secret' to 'kv-techhub-prod'"
        }

        It "Never prints secret values" {
            $result = Invoke-ScriptUnderTest $scriptPath

            $result.Output | Should -Not -Match 'aad-secret-value|hub-pfx-base64|xebia-pfx-base64'
        }
    }
}
