Describe "Setup-UserSecrets" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Setup-UserSecrets.ps1"
        $aiSettings = '[{"name":"AiCategorization__Endpoint","value":"https://ai.example.net"},{"name":"AiCategorization__DeploymentName","value":"gpt-test"},{"name":"Other","value":"x"}]'
    }

    BeforeEach {
        $cli = New-CliFake
        $state = [pscustomobject]@{ IpHandler = { '198.51.100.20' } }
        Mock Start-Sleep { }
        Mock az { Invoke-CliFake -Fake $cli -Command 'az' -Arguments $args }
        Mock dotnet { Invoke-CliFake -Fake $cli -Command 'dotnet' -Arguments $args }
        Mock Invoke-RestMethod { & $state.IpHandler }

        Add-CliRule $cli 'az' '^az ad app list' '[{"appId":"client-123"}]'
        Add-CliRule $cli 'az' '^az account show --query tenantId' 'tenant-456'
        Add-CliRule $cli 'az' '^az ad app credential reset' '{"password":"super-secret-pw"}'
        Add-CliRule $cli 'az' '^az webapp config appsettings list' $aiSettings
        Add-CliRule $cli 'az' 'secret show .*--name techhub-prod-newsletter-acs-endpoint' 'https://acs.example.net'
        Add-CliRule $cli 'az' 'secret show .*--name techhub-prod-acs-sender-address' 'noreply@example.net'
        Add-CliRule $cli 'az' 'secret show .*--name techhub-prod-newsletter-unsubscribe-secret' 'unsub-hmac-secret'
    }

    Context "Parameters" {
        It "Has safe defaults" {
            (Get-ScriptParameter $scriptPath 'KeyVaultName').DefaultValue.Value | Should -Be 'kv-techhub-prod'
            (Get-ScriptParameter $scriptPath 'AppDisplayName').DefaultValue.Value | Should -Be 'TechHub Local Dev'
            (Get-ScriptParameter $scriptPath 'SecretExpiryDays').DefaultValue.Value | Should -Be 90
            (Get-ScriptParameter $scriptPath 'Force').StaticType.Name | Should -Be 'SwitchParameter'
        }

        It "Limits SecretExpiryDays to 1..730" {
            $range = (Get-ScriptParameter $scriptPath 'SecretExpiryDays').Attributes | Where-Object { $_.TypeName.Name -eq 'ValidateRange' }
            @($range.PositionalArguments | ForEach-Object { $_.Value }) | Should -Be @(1, 730)
        }

        It "Rejects an out-of-range SecretExpiryDays before calling Azure" {
            (Invoke-ScriptUnderTest $scriptPath @{ SecretExpiryDays = 0 }).Error | Should -Not -BeNullOrEmpty
            (Invoke-ScriptUnderTest $scriptPath @{ SecretExpiryDays = 731 }).Error | Should -Not -BeNullOrEmpty
            @(Get-CliCalls $cli '^az') | Should -BeNullOrEmpty
        }
    }

    Context "App registration lookup" {
        It "Exits 1 when the lookup fails" {
            Add-CliRule $cli 'az' '^az ad app list' 'denied' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^dotnet') | Should -BeNullOrEmpty
        }

        It "Exits 1 and points at Manage-EntraId.ps1 when the app registration does not exist" {
            Add-CliRule $cli 'az' '^az ad app list' '[]' -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Manage-EntraId.ps1 -Environment localhost'
            @(Get-CliCalls $cli '^dotnet user-secrets set') | Should -BeNullOrEmpty
        }

        It "Searches by the AppDisplayName parameter" {
            Invoke-ScriptUnderTest $scriptPath @{ AppDisplayName = 'My Local App' } | Out-Null

            @(Get-CliCalls $cli '^az ad app list')[0] | Should -Match '--display-name My Local App '
        }
    }

    Context "Client secret" {
        It "Creates an appended client secret that expires after SecretExpiryDays" {
            Invoke-ScriptUnderTest $scriptPath @{ SecretExpiryDays = 30 } | Out-Null

            $reset = @(Get-CliCalls $cli '^az ad app credential reset')
            $reset | Should -HaveCount 1
            $reset[0] | Should -Match '--id client-123 --append'
            $reset[0] -match '--end-date (\S+)' | Should -BeTrue
            ([datetime]::Parse($Matches[1]).ToUniversalTime() - [datetime]::UtcNow).TotalDays | Should -BeGreaterThan 29
            ([datetime]::Parse($Matches[1]).ToUniversalTime() - [datetime]::UtcNow).TotalDays | Should -BeLessThan 31
        }

        It "Skips creation when the Web project already has a client secret" {
            Add-CliRule $cli 'dotnet' 'user-secrets list --project .*TechHub\.Web' 'AzureAd:ClientSecret = existing'

            $result = Invoke-ScriptUnderTest $scriptPath

            @(Get-CliCalls $cli '^az ad app credential reset') | Should -BeNullOrEmpty
            $result.Output | Should -Match 'already set in user secrets'
        }

        It "Rotates the client secret with -Force even when one exists" {
            Add-CliRule $cli 'dotnet' 'user-secrets list --project .*TechHub\.Web' 'AzureAd:ClientSecret = existing'

            Invoke-ScriptUnderTest $scriptPath @{ Force = $true } | Out-Null

            @(Get-CliCalls $cli '^az ad app credential reset') | Should -HaveCount 1
        }

        It "Exits 1 when the secret cannot be created" {
            Add-CliRule $cli 'az' '^az ad app credential reset' 'denied' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^dotnet user-secrets set') | Should -BeNullOrEmpty
        }

        It "Never prints secret values" {
            $result = Invoke-ScriptUnderTest $scriptPath

            $result.Output | Should -Not -Match 'super-secret-pw'
            $result.Output | Should -Not -Match 'unsub-hmac-secret'
        }
    }

    Context "Key Vault firewall" {
        It "Opens the firewall for the current IP before reading secrets and closes it afterwards" {
            Invoke-ScriptUnderTest $scriptPath | Out-Null

            $add = $cli.Calls.FindIndex({ param($c) $c -match 'network-rule add --name kv-techhub-prod --ip-address 198\.51\.100\.20' })
            $show = $cli.Calls.FindIndex({ param($c) $c -match 'secret show' })
            $remove = $cli.Calls.FindIndex({ param($c) $c -match 'network-rule remove --name kv-techhub-prod --ip-address 198\.51\.100\.20' })
            $add | Should -BeGreaterOrEqual 0
            $add | Should -BeLessThan $show
            $remove | Should -BeGreaterThan $show
        }

        It "Still reads secrets (without a firewall change) when the public IP cannot be determined" {
            $state.IpHandler = { throw 'offline' }

            $result = Invoke-ScriptUnderTest $scriptPath

            @(Get-CliCalls $cli '^az keyvault network-rule') | Should -BeNullOrEmpty
            @(Get-CliCalls $cli '^az keyvault secret show') | Should -HaveCount 3
            $result.Output | Should -Match 'Could not determine public IP'
        }

        It "Removes the firewall rule even when a secret lookup fails" {
            Add-CliRule $cli 'az' 'secret show .*--name techhub-prod-newsletter-acs-endpoint' 'forbidden' -ExitCode 1 -First

            Invoke-ScriptUnderTest $scriptPath | Out-Null

            @(Get-CliCalls $cli '^az keyvault network-rule remove') | Should -HaveCount 1
        }

        It "Uses the KeyVaultName parameter" {
            Invoke-ScriptUnderTest $scriptPath @{ KeyVaultName = 'kv-other' } | Out-Null

            foreach ($call in @(Get-CliCalls $cli '^az keyvault')) { $call | Should -Match 'kv-other' }
        }
    }

    Context "User secrets" {
        It "Stores localhost Entra ID, production AI and newsletter values in the API project" {
            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 0
            $sets = @(Get-CliCalls $cli '^dotnet user-secrets set') -join "`n"
            foreach ($expected in @(
                    'AzureAd:TenantId tenant-456 --project .*src/TechHub.Api'
                    'AzureAd:ClientId client-123 --project .*src/TechHub.Api'
                    'AiCategorization:Endpoint https://ai.example.net --project .*src/TechHub.Api'
                    'AiCategorization:DeploymentName gpt-test --project .*src/TechHub.Api'
                    'Newsletter:Endpoint https://acs.example.net --project .*src/TechHub.Api'
                    'Newsletter:SenderAddress noreply@example.net --project .*src/TechHub.Api'
                    'Newsletter:UnsubscribeSecret unsub-hmac-secret --project .*src/TechHub.Api'
                )) {
                $sets | Should -Match ([regex]::new("dotnet user-secrets set $expected"))
            }
        }

        It "Stores Entra ID values, the new client secret and the API scope in the Web project" {
            Invoke-ScriptUnderTest $scriptPath | Out-Null

            $sets = @(Get-CliCalls $cli '^dotnet user-secrets set') -join "`n"
            $sets | Should -Match 'AzureAd:ClientId client-123 --project .*src/TechHub.Web'
            $sets | Should -Match 'AzureAd:ClientSecret super-secret-pw --project .*src/TechHub.Web'
            $sets | Should -Match 'AzureAd:Scopes api://client-123/Admin.Access --project .*src/TechHub.Web'
        }

        It "Never stores an AI API key (DefaultAzureCredential is used)" {
            Invoke-ScriptUnderTest $scriptPath | Out-Null

            @(Get-CliCalls $cli '^dotnet user-secrets set') -join "`n" | Should -Not -Match 'ApiKey'
        }

        It "Skips secrets that are already set unless -Force is used" {
            Add-CliRule $cli 'dotnet' 'user-secrets list --project .*TechHub\.Api' 'AzureAd:ClientId = old-client'

            $result = Invoke-ScriptUnderTest $scriptPath

            @(Get-CliCalls $cli '^dotnet user-secrets set AzureAd:ClientId .*TechHub\.Api') | Should -BeNullOrEmpty
            $result.Output | Should -Match '\[SKIP\] AzureAd:ClientId'

            $cli.Calls.Clear()
            Invoke-ScriptUnderTest $scriptPath @{ Force = $true } | Out-Null
            @(Get-CliCalls $cli '^dotnet user-secrets set AzureAd:ClientId client-123 .*TechHub\.Api') | Should -HaveCount 1
        }

        It "Skips values that could not be fetched instead of storing empty secrets" {
            Add-CliRule $cli 'az' 'secret show .*--name techhub-prod-newsletter-acs-endpoint' 'forbidden' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath

            @(Get-CliCalls $cli '^dotnet user-secrets set Newsletter:Endpoint') | Should -BeNullOrEmpty
            $result.Output | Should -Match 'Newsletter:Endpoint .*value is empty, skipping'
        }

        It "Exits 1 when the production app settings cannot be read" {
            Add-CliRule $cli 'az' '^az webapp config appsettings list' 'denied' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^dotnet user-secrets set') | Should -BeNullOrEmpty
        }
    }
}
