Describe "Deploy-Infrastructure" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $realScript = Join-Path $PSScriptRoot "../../scripts/Deploy-Infrastructure.ps1"
        $envNames = @('ADMIN_IP_ADDRESSES', 'POSTGRES_ADMIN_PASSWORD', 'NEWSLETTER_ACS_ENDPOINT')
        $paramsText = "using '../infrastructure.bicep'`nparam linkEmailDomain = false`n"

        function Start-InfraDeploy {
            param([hashtable]$Parameters = @{}, [switch]$OmitTemplates, [string]$SyncStub = (New-RecordingStubScript))
            $files = if ($OmitTemplates) { @{} } else { @{
                    'infra/infrastructure.bicep'                       = '// bicep'
                    'infra/parameters/prod-infrastructure.bicepparam' = $paramsText
                } }
            $root = New-ScriptSandbox -Scripts @('Deploy-Infrastructure.ps1') -StubScripts @{ 'Sync-KeyVaultSecrets.ps1' = $SyncStub } -Files $files
            $sandboxes.Add($root)
            $result = Invoke-ScriptUnderTest (Join-Path $root 'scripts/Deploy-Infrastructure.ps1') $Parameters
            $result | Add-Member -NotePropertyName Sandbox -NotePropertyValue $root -PassThru
        }
    }

    BeforeEach {
        $savedEnv = Save-EnvironmentVariable $envNames
        $env:ADMIN_IP_ADDRESSES = '1.2.3.4'
        $env:POSTGRES_ADMIN_PASSWORD = 'not-a-real-password'
        $env:NEWSLETTER_ACS_ENDPOINT = $null

        $sandboxes = [System.Collections.Generic.List[string]]::new()
        $cli = New-CliFake
        $az = [pscustomobject]@{
            ContainerApps = @()
            Env           = $null
            EmailDomain   = $null
            DeployParams  = $null
            ParamsFileAtDeploy = $null
            States        = [System.Collections.Generic.Queue[string]]::new()
            Removed       = [System.Collections.Generic.List[string]]::new()
            ValidationErrors = $null
            DeployBehavior = { [pscustomobject]@{ Outputs = @{ acsEndpoint = [pscustomobject]@{ Value = 'https://acs.example.net' } } } }
        }

        Mock Start-Sleep { }
        Mock Get-AzContext { [pscustomobject]@{ Subscription = [pscustomobject]@{ Name = 'Test Subscription' } } }
        Mock Test-AzDeployment { $az.ValidationErrors }
        Mock New-AzDeployment {
            $az.DeployParams = @{ Name = $Name; Location = $Location; TemplateFile = $TemplateFile; TemplateParameterFile = $TemplateParameterFile }
            if ($TemplateParameterFile -and (Test-Path $TemplateParameterFile)) { $az.ParamsFileAtDeploy = Get-Content $TemplateParameterFile -Raw }
            if (-not $WhatIfPreference) { & $az.DeployBehavior }
        }
        Mock Get-AzResource {
            switch ($ResourceType) {
                'Microsoft.App/containerApps' { $az.ContainerApps }
                'Microsoft.App/managedEnvironments' { $az.Env }
                'Microsoft.Communication/emailServices/domains' { $az.EmailDomain }
            }
        }
        Mock Remove-AzResource { $az.Removed.Add($ResourceId) }
        Mock Get-AzPostgreSqlFlexibleServerFirewallRule { $null }
        Mock Remove-AzPostgreSqlFlexibleServerFirewallRule { }
        Mock Get-AzNatGateway { $null }
        Mock Remove-AzNatGateway { }
        Mock Get-AzPublicIpAddress { $null }
        Mock Remove-AzPublicIpAddress { }
        Mock Get-AzDeployment { }
        Mock Get-AzDeploymentOperation { @() }
        Mock Get-AzResourceGroupDeploymentOperation { @() }
        Mock gh { Invoke-CliFake -Fake $cli -Command 'gh' -Arguments $args }
        Mock az { Invoke-CliFake -Fake $cli -Command 'az' -Arguments $args }
    }

    AfterEach {
        Restore-EnvironmentVariable $savedEnv
        foreach ($root in $sandboxes) { Remove-ScriptSandbox $root }
    }

    Context "Parameters" {
        It "Defaults Mode to whatif (safe default) and restricts it to validate, whatif and deploy" {
            $mode = Get-ScriptParameter $realScript 'Mode'
            $mode.DefaultValue.Value | Should -Be 'whatif'
            Get-ParameterValidateSet $mode | Should -Be @('validate', 'whatif', 'deploy')
        }

        It "Defaults Location to swedencentral and has a LinkEmailDomain switch" {
            (Get-ScriptParameter $realScript 'Location').DefaultValue.Value | Should -Be 'swedencentral'
            (Get-ScriptParameter $realScript 'LinkEmailDomain').StaticType.Name | Should -Be 'SwitchParameter'
        }

        It "Rejects an unknown Mode before doing any work" {
            $result = Start-InfraDeploy @{ Mode = 'destroy' }

            $result.Error | Should -Not -BeNullOrEmpty
            $result.Error.Exception.Message | Should -Match 'destroy'
            Should -Invoke Get-AzContext -Times 0 -Exactly
        }
    }

    Context "Pre-flight checks" {
        It "Exits 1 when Azure PowerShell is not logged in" {
            Mock Get-AzContext { $null }

            $result = Start-InfraDeploy

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Not logged in to Azure PowerShell'
            Should -Invoke Test-AzDeployment -Times 0 -Exactly
        }

        It "Exits 1 when the Bicep template or parameter file is missing" {
            $result = Start-InfraDeploy -OmitTemplates

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'File not found'
            Should -Invoke Test-AzDeployment -Times 0 -Exactly
        }

        It "Reads ADMIN_IP_ADDRESSES from the GitHub variable when not set" {
            $env:ADMIN_IP_ADDRESSES = $null
            Add-CliRule $cli 'gh' '^gh variable get ADMIN_IP_ADDRESSES --repo techhubms/techhub' ' 9.9.9.9,8.8.8.8 '

            $result = Start-InfraDeploy @{ Mode = 'validate' }

            $result.ExitCode | Should -Be 0
            $env:ADMIN_IP_ADDRESSES | Should -Be '9.9.9.9,8.8.8.8'
        }

        It "Uses a placeholder admin IP in validate/whatif mode when none can be found" {
            $env:ADMIN_IP_ADDRESSES = $null

            $result = Start-InfraDeploy @{ Mode = 'whatif' }

            $result.ExitCode | Should -Be 0
            $env:ADMIN_IP_ADDRESSES | Should -Be '0.0.0.0'
            $result.Output | Should -Match 'using placeholder'
        }

        It "Refuses to deploy without ADMIN_IP_ADDRESSES" {
            $env:ADMIN_IP_ADDRESSES = $null

            $result = Start-InfraDeploy @{ Mode = 'deploy' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'ADMIN_IP_ADDRESSES is not set'
            Should -Invoke New-AzDeployment -Times 0 -Exactly
        }

        It "Falls back to the Key Vault secret for POSTGRES_ADMIN_PASSWORD" {
            $env:POSTGRES_ADMIN_PASSWORD = $null
            Add-CliRule $cli 'az' '^az keyvault secret show --vault-name kv-techhub-prod --name techhub-prod-postgres-admin-password' 'kv-password'

            $result = Start-InfraDeploy @{ Mode = 'validate' }

            $result.ExitCode | Should -Be 0
            $env:POSTGRES_ADMIN_PASSWORD | Should -Be 'kv-password'
        }

        It "Exits 1 when POSTGRES_ADMIN_PASSWORD is not set and Key Vault does not provide it" {
            $env:POSTGRES_ADMIN_PASSWORD = $null
            Add-CliRule $cli 'az' '^az keyvault secret show' '' -ExitCode 1

            $result = Start-InfraDeploy @{ Mode = 'whatif' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'POSTGRES_ADMIN_PASSWORD is not set'
            Should -Invoke Test-AzDeployment -Times 0 -Exactly
        }

        It "Never prints the password" {
            $env:POSTGRES_ADMIN_PASSWORD = 'super-secret-value'

            $result = Start-InfraDeploy @{ Mode = 'validate' }

            $result.Output | Should -Not -Match 'super-secret-value'
        }
    }

    Context "Validate mode" {
        It "Validates the template and makes no changes" {
            $result = Start-InfraDeploy @{ Mode = 'validate' }

            $result.ExitCode | Should -Be 0
            Should -Invoke Test-AzDeployment -Times 1 -Exactly -ParameterFilter { $Location -eq 'swedencentral' -and $TemplateFile -like '*infra/infrastructure.bicep' }
            Should -Invoke New-AzDeployment -Times 0 -Exactly
            Should -Invoke Remove-AzResource -Times 0 -Exactly
        }

        It "Exits 1 and prints each validation message when the template is invalid" {
            $az.ValidationErrors = @([pscustomobject]@{ Message = 'bad sku' }, [pscustomobject]@{ Message = 'bad name' })

            $result = Start-InfraDeploy @{ Mode = 'validate' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'bad sku'
            $result.Output | Should -Match 'bad name'
            $result.Output | Should -Match 'template validation failed'
        }
    }

    Context "WhatIf mode" {
        It "Runs New-AzDeployment with -WhatIf and never deletes or deploys" {
            $result = Start-InfraDeploy

            $result.ExitCode | Should -Be 0
            Should -Invoke Test-AzDeployment -Times 1 -Exactly
            Should -Invoke New-AzDeployment -Times 1 -Exactly -ParameterFilter { $WhatIf }
            Should -Invoke Remove-AzResource -Times 0 -Exactly
            Should -Invoke Remove-AzNatGateway -Times 0 -Exactly
            $result.Output | Should -Match 'What-If completed'
        }

        It "Exits 1 when What-If fails" {
            $az.DeployBehavior = { throw 'arm exploded' }
            Mock New-AzDeployment { throw 'arm exploded' }

            $result = Start-InfraDeploy

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'What-If failed'
        }
    }

    Context "Deploy mode" {
        It "Deploys to the production template with a timestamped deployment name" {
            $result = Start-InfraDeploy @{ Mode = 'deploy' }

            $result.ExitCode | Should -Be 0
            Should -Invoke New-AzDeployment -Times 1 -Exactly -ParameterFilter {
                $Name -match '^techhub-prod-infra-\d{8}-\d{6}$' -and $Location -eq 'swedencentral' -and -not $WhatIf
            }
            $result.Output | Should -Match 'Base infrastructure deployed successfully'
        }

        It "Deletes the decommissioned Container Apps before the environment and before deploying" {
            $az.ContainerApps = @(
                [pscustomobject]@{ ResourceId = '/subs/x/containerApps/api'; Name = 'api' }
                [pscustomobject]@{ ResourceId = '/subs/x/containerApps/web'; Name = 'web' }
            )
            $az.Env = [pscustomobject]@{ ResourceId = '/subs/x/managedEnvironments/cae-techhub-prod' }

            $result = Start-InfraDeploy @{ Mode = 'deploy' }

            $result.ExitCode | Should -Be 0
            $az.Removed | Should -Be @('/subs/x/containerApps/api', '/subs/x/containerApps/web', '/subs/x/managedEnvironments/cae-techhub-prod')
        }

        It "Treats already-removed Container Apps resources as success" {
            $result = Start-InfraDeploy @{ Mode = 'deploy' }

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'No Container Apps present'
            $result.Output | Should -Match 'Container Apps Environment not present'
            $az.Removed | Should -BeNullOrEmpty
        }

        It "Removes stale PostgreSQL firewall rules only when they exist" {
            Mock Get-AzPostgreSqlFlexibleServerFirewallRule { if ($Name -eq 'allow-nat-gateway') { [pscustomobject]@{ Name = $Name } } }

            $result = Start-InfraDeploy @{ Mode = 'deploy' }

            $result.ExitCode | Should -Be 0
            Should -Invoke Remove-AzPostgreSqlFlexibleServerFirewallRule -Times 1 -Exactly -ParameterFilter {
                $Name -eq 'allow-nat-gateway' -and $ServerName -eq 'psql-techhub-prod' -and $ResourceGroupName -eq 'rg-techhub-prod'
            }
        }

        It "Removes the decommissioned NAT Gateway and its public IP when present" {
            Mock Get-AzNatGateway { [pscustomobject]@{ Name = 'natgw-techhub-prod' } }
            Mock Get-AzPublicIpAddress { [pscustomobject]@{ Name = 'pip-nat-techhub-prod' } }

            Start-InfraDeploy @{ Mode = 'deploy' } | Out-Null

            Should -Invoke Remove-AzNatGateway -Times 1 -Exactly -ParameterFilter { $Name -eq 'natgw-techhub-prod' }
            Should -Invoke Remove-AzPublicIpAddress -Times 1 -Exactly -ParameterFilter { $Name -eq 'pip-nat-techhub-prod' }
        }

        It "Exports the ACS endpoint and then runs Sync-KeyVaultSecrets.ps1" {
            $result = Start-InfraDeploy @{ Mode = 'deploy' }

            $result.ExitCode | Should -Be 0
            $env:NEWSLETTER_ACS_ENDPOINT | Should -Be 'https://acs.example.net'
            @(Get-StubScriptCalls $result.Sandbox) | Should -HaveCount 1
            @(Get-StubScriptCalls $result.Sandbox)[0] | Should -Match '^Sync-KeyVaultSecrets.ps1'
        }

        It "Exits 1 when the secret sync fails" {
            $stub = "throw 'kv unavailable'"

            $result = Start-InfraDeploy @{ Mode = 'deploy' } -SyncStub $stub

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Secret sync failed'
        }

        It "Does not sync secrets in whatif or validate mode" {
            $result = Start-InfraDeploy @{ Mode = 'whatif' }

            @(Get-StubScriptCalls $result.Sandbox) | Should -BeNullOrEmpty
        }

        It "Exits 1 and lists failed operations when the deployment fails" {
            Mock New-AzDeployment { throw 'The template deployment failed' }
            Mock Get-AzDeploymentOperation { [pscustomobject]@{ ProvisioningState = 'Failed'; TargetResource = '/subs/x/resourceGroups/rg/providers/Microsoft.Sql/servers/s'; StatusCode = 'Conflict'; StatusMessage = 'boom' } }

            $result = Start-InfraDeploy @{ Mode = 'deploy' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Infrastructure deployment failed'
            $result.Output | Should -Match 'Resource: /subs/x/resourceGroups/rg/providers/Microsoft.Sql/servers/s'
            @(Get-StubScriptCalls $result.Sandbox) | Should -BeNullOrEmpty
        }
    }

    Context "Deployment reattach after a dropped connection" {
        It "Continues when the existing deployment finishes with Succeeded" {
            Mock New-AzDeployment { throw 'An error occurred while sending the request.' }
            'Running', 'Succeeded' | ForEach-Object { $az.States.Enqueue($_) }
            Mock Get-AzDeployment { [pscustomobject]@{ ProvisioningState = $az.States.Dequeue(); Outputs = @{ acsEndpoint = [pscustomobject]@{ Value = 'https://acs.reattached.net' } } } }

            $result = Start-InfraDeploy @{ Mode = 'deploy' }

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match "Reattached — deployment 'techhub-prod-infra-\d{8}-\d{6}' completed successfully"
            $env:NEWSLETTER_ACS_ENDPOINT | Should -Be 'https://acs.reattached.net'
            @(Get-StubScriptCalls $result.Sandbox) | Should -HaveCount 1
            Should -Invoke Get-AzDeployment -Times 2 -Exactly
        }

        It "Exits 1 when the reattached deployment ends Failed" {
            Mock New-AzDeployment { throw 'An error occurred while sending the request.' }
            Mock Get-AzDeployment { [pscustomobject]@{ ProvisioningState = 'Failed' } }

            $result = Start-InfraDeploy @{ Mode = 'deploy' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Infrastructure deployment failed: Failed'
        }

        It "Gives up after 80 reattach attempts" {
            Mock New-AzDeployment { throw 'An error occurred while sending the request.' }
            Mock Get-AzDeployment { [pscustomobject]@{ ProvisioningState = 'Running' } }

            $result = Start-InfraDeploy @{ Mode = 'deploy' }

            $result.ExitCode | Should -Be 1
            Should -Invoke Get-AzDeployment -Times 80 -Exactly
        }
    }

    Context "LinkEmailDomain and email domain handling" {
        It "Leaves the parameter file untouched by default" {
            Start-InfraDeploy @{ Mode = 'deploy' } | Out-Null

            $az.DeployParams.TemplateParameterFile | Should -Match 'prod-infrastructure\.bicepparam$'
            $az.ParamsFileAtDeploy | Should -Match 'param linkEmailDomain = false'
        }

        It "Deploys with an override file that links the domain when -LinkEmailDomain is set, then removes it" {
            $result = Start-InfraDeploy @{ Mode = 'deploy'; LinkEmailDomain = $true }

            $az.DeployParams.TemplateParameterFile | Should -Match 'prod-infrastructure-override\.bicepparam$'
            $az.ParamsFileAtDeploy | Should -Match 'param linkEmailDomain = true'
            Test-Path (Join-Path $result.Sandbox 'infra/parameters/prod-infrastructure-override.bicepparam') | Should -BeFalse
        }

        It "Does not re-create an existing email domain (a PUT would reset DNS verification)" {
            $az.EmailDomain = [pscustomobject]@{ Name = 'mail.hub.ms' }

            $result = Start-InfraDeploy @{ Mode = 'deploy' }

            $az.ParamsFileAtDeploy | Should -Match 'param createEmailDomain = false'
            Test-Path (Join-Path $result.Sandbox 'infra/parameters/prod-infrastructure-override.bicepparam') | Should -BeFalse
        }

        It "Removes the override file even when the deployment fails" {
            $az.EmailDomain = [pscustomobject]@{ Name = 'mail.hub.ms' }
            Mock New-AzDeployment { throw 'The template deployment failed' }

            $result = Start-InfraDeploy @{ Mode = 'deploy' }

            $result.ExitCode | Should -Be 1
            Test-Path (Join-Path $result.Sandbox 'infra/parameters/prod-infrastructure-override.bicepparam') | Should -BeFalse
        }
    }
}
