Describe "Renew-WildcardCertificates" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $realGetCommand = Get-Command -Name Get-Command -CommandType Cmdlet
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Renew-WildcardCertificates.ps1"

        function Get-ArgValue {
            # Value following a flag in an argument array (e.g. --config-dir <value>).
            param([object[]]$Arguments, [string]$Flag)
            $i = [array]::IndexOf(@($Arguments | ForEach-Object { "$_" }), $Flag)
            if ($i -ge 0) { "$($Arguments[$i + 1])" }
        }

        function Get-AzCallIndex {
            param([string]$Match)
            $cli.Calls.FindIndex({ param($c) $c -match $Match })
        }
    }

    BeforeEach {
        $cli = New-CliFake
        $state = [pscustomobject]@{
            IniContents = [System.Collections.Generic.List[string]]::new()
            IpHandler   = { '203.0.113.7' }
            IpUris      = [System.Collections.Generic.List[string]]::new()
            HasCertbot  = $true
            HasOpenssl  = $true
        }

        Mock Start-Sleep { }
        Mock az { Invoke-CliFake -Fake $cli -Command 'az' -Arguments $args }
        Mock certbot { Invoke-CliFake -Fake $cli -Command 'certbot' -Arguments $args }
        Mock openssl { Invoke-CliFake -Fake $cli -Command 'openssl' -Arguments $args }
        Mock Get-Command {
            if ($Name -eq 'certbot' -and -not $state.HasCertbot) { return $null }
            if ($Name -eq 'openssl' -and -not $state.HasOpenssl) { return $null }
            & $realGetCommand @PesterBoundParameters
        }
        Mock Invoke-RestMethod { $state.IpUris.Add([string]$Uri); & $state.IpHandler }

        Add-CliRule $cli 'az' '^az account show' '{"id":"sub-1111","name":"Test Subscription"}'
        Add-CliRule $cli 'az' '^az appservice plan show .*--query id' '/subs/x/serverfarms/asp'
        Add-CliRule $cli 'az' '^az appservice plan show .*--query location' 'swedencentral'
        Add-CliRule $cli 'az' '^az keyvault show' '/subs/x/vaults/kv-techhub-prod'
        Add-CliRule $cli 'az' '^az keyvault network-rule list' ''
        Add-CliRule $cli 'az' '^az keyvault secret list' ''
        Add-CliRule $cli 'certbot' '^certbot plugins' 'dns-azure: Obtain certificates using Azure DNS'
        # certbot "issues" certificates by creating the files the script converts afterwards.
        Add-CliRule $cli 'certbot' '^certbot certonly' '' -Action {
            param($a)
            $state.IniContents.Add((Get-Content (Get-ArgValue $a '--dns-azure-credentials') -Raw))
            if ($a -notcontains '--dry-run') {
                $dir = Join-Path (Get-ArgValue $a '--config-dir') "live/$(Get-ArgValue $a '--cert-name')"
                New-Item -ItemType Directory -Force -Path $dir | Out-Null
                Set-Content (Join-Path $dir 'fullchain.pem') 'chain'
                Set-Content (Join-Path $dir 'privkey.pem') 'key'
            }
        }
        Add-CliRule $cli 'openssl' '^openssl pkcs12' '' -Action {
            param($a)
            [System.IO.File]::WriteAllBytes((Get-ArgValue $a '-out'), [byte[]](1, 2, 3, 4))
        }
    }

    Context "Parameters" {
        It "Requires Email and has safe defaults" {
            Test-ParameterMandatory (Get-ScriptParameter $scriptPath 'Email') | Should -BeTrue
            (Get-ScriptParameter $scriptPath 'KeyVaultName').DefaultValue.Value | Should -Be 'kv-techhub-prod'
            (Get-ScriptParameter $scriptPath 'ResourceGroup').DefaultValue.Value | Should -Be 'rg-techhub-prod'
            (Get-ScriptParameter $scriptPath 'AcmeDnsZone').DefaultValue.Value | Should -Be 'acme.hub.ms'
            (Get-ScriptParameter $scriptPath 'AppServicePlanName').DefaultValue.Value | Should -Be 'asp-techhub-prod'
        }

        It "Has DryRun and Force switches" {
            (Get-ScriptParameter $scriptPath 'DryRun').StaticType.Name | Should -Be 'SwitchParameter'
            (Get-ScriptParameter $scriptPath 'Force').StaticType.Name | Should -Be 'SwitchParameter'
        }
    }

    Context "Pre-flight checks" {
        It "Exits 1 when Azure CLI is not logged in" {
            $cli.Rules.Clear()
            Add-CliRule $cli 'az' '^az account show' 'ERROR' -ExitCode 1

            $result = Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c' }

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^certbot') | Should -BeNullOrEmpty
        }

        It "Exits 1 when certbot is not installed" {
            $state.HasCertbot = $false

            $result = Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'certbot not found'
        }

        It "Exits 1 when the certbot-dns-azure plugin is missing" {
            Add-CliRule $cli 'certbot' '^certbot plugins' 'nginx, apache' -First

            $result = Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'certbot-dns-azure plugin not found'
            @(Get-CliCalls $cli '^certbot certonly') | Should -BeNullOrEmpty
        }

        It "Exits 1 when the ACME DNS zone does not exist" {
            Add-CliRule $cli 'az' '^az network dns zone show' 'ERROR' -ExitCode 1

            $result = Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match "DNS zone 'acme.hub.ms' not found"
            @(Get-CliCalls $cli '^certbot certonly') | Should -BeNullOrEmpty
        }
    }

    Context "Dry run" {
        It "Runs certbot in --dry-run for both wildcard domains and touches neither Key Vault nor openssl" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ Email = 'admin@example.com'; DryRun = $true }

            $result.ExitCode | Should -Be 0
            $certbot = @(Get-CliCalls $cli '^certbot certonly')
            $certbot | Should -HaveCount 2
            $certbot[0] | Should -Match '-d \*\.hub\.ms -d hub\.ms .*--cert-name wildcard-hub-ms'
            $certbot[1] | Should -Match '-d \*\.xebia\.ms -d xebia\.ms .*--cert-name wildcard-xebia-ms'
            foreach ($call in $certbot) {
                $call | Should -Match '--dry-run'
                $call | Should -Match '--email admin@example\.com --agree-tos --non-interactive'
                $call | Should -Not -Match '--force-renewal'
            }
            @(Get-CliCalls $cli '^openssl') | Should -BeNullOrEmpty
            @(Get-CliCalls $cli '^az (keyvault|deployment)') | Should -BeNullOrEmpty
            Should -Invoke Invoke-RestMethod -Times 0 -Exactly
            $result.Output | Should -Match 'Dry run completed successfully'
        }

        It "Maps each domain to a TXT alias in the ACME zone using the signed-in subscription" {
            Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c'; DryRun = $true } | Out-Null

            $ini = $state.IniContents[0]
            $ini | Should -Match 'dns_azure_use_cli_credentials = true'
            $prefix = '/subscriptions/sub-1111/resourceGroups/rg-techhub-prod/providers/Microsoft.Network/dnsZones/acme.hub.ms'
            $ini | Should -Match ([regex]::Escape("dns_azure_zone1 = hub.ms:$prefix/TXT/hub-ms"))
            $ini | Should -Match ([regex]::Escape("dns_azure_zone2 = xebia.ms:$prefix/TXT/xebia-ms"))
        }

        It "Adds --force-renewal when Force is set" {
            Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c'; DryRun = $true; Force = $true } | Out-Null

            foreach ($call in @(Get-CliCalls $cli '^certbot certonly')) { $call | Should -Match '--force-renewal' }
        }

        It "Keeps certbot state in a temp directory that is removed afterwards" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c'; DryRun = $true }

            $configDir = Get-ArgValue ((Get-CliCalls $cli '^certbot certonly')[0] -split ' ') '--config-dir'
            $configDir | Should -Match 'certbot-[0-9a-f]{8}'
            Test-Path (Split-Path $configDir) | Should -BeFalse
            $result.Output | Should -Match 'Cleaned up temporary config'
        }

        It "Exits 1 and stops at the first domain when certbot fails" {
            Add-CliRule $cli 'certbot' '^certbot certonly' '' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c' }

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^certbot certonly') | Should -HaveCount 1
            $result.Output | Should -Match 'certbot failed for \*\.hub\.ms'
        }
    }

    Context "Issuing and importing certificates" {
        It "Opens the Key Vault firewall for the detected IP, imports both PFX secrets and removes the rule" {
            $result = Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c' }

            $result.ExitCode | Should -Be 0
            @(Get-CliCalls $cli '^certbot certonly') | ForEach-Object { $_ | Should -Not -Match '--dry-run' }
            $add = @(Get-CliCalls $cli '^az keyvault network-rule add')
            $add | Should -HaveCount 1
            $add[0] | Should -Match '--name kv-techhub-prod --ip-address 203\.0\.113\.7/32'
            $secrets = @(Get-CliCalls $cli '^az keyvault secret set')
            $secrets | Should -HaveCount 2
            $secrets[0] | Should -Match '--name wildcard-hub-ms --value AQIDBA== --content-type application/x-pkcs12'
            $secrets[1] | Should -Match '--name wildcard-xebia-ms'
            $remove = @(Get-CliCalls $cli '^az keyvault network-rule remove')
            $remove | Should -HaveCount 1
            $remove[0] | Should -Match '--ip-address 203\.0\.113\.7/32'
            (Get-AzCallIndex 'network-rule add') | Should -BeLessThan (Get-AzCallIndex 'secret set')
            (Get-AzCallIndex 'network-rule remove') | Should -BeGreaterThan (Get-AzCallIndex 'secret set --vault-name kv-techhub-prod --name wildcard-xebia-ms')
        }

        It "Redeploys wildcardCert.bicep for each certificate so App Service picks up the new PFX" {
            Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c' } | Out-Null

            $deploys = @(Get-CliCalls $cli '^az deployment group create')
            $deploys | Should -HaveCount 2
            $deploys[0] | Should -Match '--resource-group rg-techhub-prod'
            $deploys[0] | Should -Match 'wildcardCert\.bicep'
            $deploys[0] | Should -Match 'certResourceName=wildcard-hub-ms keyVaultResourceId=/subs/x/vaults/kv-techhub-prod keyVaultSecretName=wildcard-hub-ms'
            $deploys[0] | Should -Match 'appServicePlanId=/subs/x/serverfarms/asp'
            $deploys[1] | Should -Match 'certResourceName=wildcard-xebia-ms'
        }

        It "Uses the App Service plan resource group when it differs from ResourceGroup" {
            Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c'; AppServicePlanResourceGroup = 'rg-other' } | Out-Null

            @(Get-CliCalls $cli '^az appservice plan show')[0] | Should -Match '--resource-group rg-other'
            @(Get-CliCalls $cli '^az deployment group create')[0] | Should -Match '--resource-group rg-other'
        }

        It "Does not touch the firewall when the IP is already permitted" {
            Add-CliRule $cli 'az' '^az keyvault network-rule list' @('10.0.0.1', '203.0.113.7/32') -First

            $result = Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c' }

            $result.ExitCode | Should -Be 0
            @(Get-CliCalls $cli '^az keyvault network-rule (add|remove)') | Should -BeNullOrEmpty
            @(Get-CliCalls $cli '^az keyvault secret set') | Should -HaveCount 2
        }

        It "Falls back to the next IP provider and ignores invalid responses" {
            $state.IpHandler = {
                switch ($state.IpUris.Count) {
                    1 { throw 'provider down' }
                    2 { '<html>not an ip</html>' }
                    default { '198.51.100.9' }
                }
            }

            Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c' } | Out-Null

            @($state.IpUris | ForEach-Object { $_.TrimEnd('/') }) | Should -Be @('https://checkip.amazonaws.com', 'https://api.ipify.org', 'https://icanhazip.com')
            @(Get-CliCalls $cli '^az keyvault network-rule add')[0] | Should -Match '198\.51\.100\.9/32'
        }

        It "Exits 1 without writing secrets when no provider returns an IP" {
            $state.IpHandler = { throw 'offline' }

            $result = Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Failed to detect outbound IP'
            @(Get-CliCalls $cli '^az keyvault secret set') | Should -BeNullOrEmpty
        }

        It "Removes the firewall rule even when writing a secret fails" {
            Add-CliRule $cli 'az' '^az keyvault secret set' '' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Key Vault write failed for wildcard-hub-ms'
            @(Get-CliCalls $cli '^az keyvault network-rule remove') | Should -HaveCount 1
        }

        It "Only warns (exit 0) when the App Service certificate update fails" {
            Add-CliRule $cli 'az' '^az deployment group create' '' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c' }

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'App Service certificate update failed for wildcard-hub-ms'
        }

        It "Exits 1 when PFX conversion fails" {
            Add-CliRule $cli 'openssl' '^openssl pkcs12' '' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'PFX conversion failed'
            @(Get-CliCalls $cli '^az keyvault secret set') | Should -BeNullOrEmpty
        }

        It "Exits 1 when OpenSSL is not installed" {
            $state.HasOpenssl = $false

            $result = Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'OpenSSL not found'
        }

        It "Exits 1 when the App Service plan or Key Vault cannot be resolved" {
            Add-CliRule $cli 'az' '^az appservice plan show .*--query id' 'ERROR' -ExitCode 1 -First

            $result = Invoke-ScriptUnderTest $scriptPath @{ Email = 'a@b.c' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match "Could not resolve App Service Plan 'asp-techhub-prod'"
            @(Get-CliCalls $cli '^az keyvault network-rule add') | Should -BeNullOrEmpty
        }
    }
}
