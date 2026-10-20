Describe "Deploy-Applications" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $realScript = Join-Path $PSScriptRoot "../../scripts/Deploy-Applications.ps1"
        $envNames = @('API_IMAGE_TAG', 'WEB_IMAGE_TAG')

        function Start-AppsDeploy {
            param([hashtable]$Parameters = @{}, [switch]$OmitTemplates)
            $files = if ($OmitTemplates) { @{} } else { @{
                    'infra/applications.bicep'                       = '// bicep'
                    'infra/parameters/prod-applications.bicepparam' = "using '../applications.bicep'"
                    'infra/modules/wildcardCert.bicep'               = '// bicep'
                } }
            $root = New-ScriptSandbox -Scripts @('Deploy-Applications.ps1') -Files $files
            $sandboxes.Add($root)
            Invoke-ScriptUnderTest (Join-Path $root 'scripts/Deploy-Applications.ps1') $Parameters
        }
    }

    BeforeEach {
        $savedEnv = Save-EnvironmentVariable $envNames
        $env:API_IMAGE_TAG = $null
        $env:WEB_IMAGE_TAG = $null
        $sandboxes = [System.Collections.Generic.List[string]]::new()
        $az = [pscustomobject]@{
            ExistingCerts     = @('wildcard-hub-ms', 'wildcard-xebia-ms')
            ValidationErrors  = $null
            TagAtDeploy       = $null
            ImportedCerts     = [System.Collections.Generic.List[string]]::new()
            ImportAttempts    = 0
            ImportBehavior    = { }
            RoleBehavior      = { }
            Roles             = [System.Collections.Generic.List[string]]::new()
        }

        Mock Start-Sleep { }
        Mock Get-AzContext { [pscustomobject]@{ Subscription = [pscustomobject]@{ Name = 'Test Subscription' } } }
        Mock Test-AzDeployment { $az.ValidationErrors }
        Mock New-AzDeployment { $az.TagAtDeploy = $env:API_IMAGE_TAG }
        Mock Get-AzResource {
            switch ($ResourceType) {
                'Microsoft.Web/certificates' { if ($az.ExistingCerts -contains $Name) { [pscustomobject]@{ Name = $Name } } }
                'Microsoft.Web/serverfarms' { [pscustomobject]@{ ResourceId = '/subs/x/serverfarms/asp'; Location = 'swedencentral' } }
                'Microsoft.KeyVault/vaults' { [pscustomobject]@{ ResourceId = '/subs/x/vaults/kv' } }
            }
        }
        Mock New-AzRoleAssignment { $az.Roles.Add("$RoleDefinitionId|$Scope|$ObjectType"); & $az.RoleBehavior }
        Mock New-AzResourceGroupDeployment {
            $az.ImportAttempts++
            & $az.ImportBehavior
            $az.ImportedCerts.Add($certResourceName)
        }
    }

    AfterEach {
        Restore-EnvironmentVariable $savedEnv
        foreach ($root in $sandboxes) { Remove-ScriptSandbox $root }
    }

    Context "Bicep publishing access" {
        It "Denies all SCM traffic independently of application ingress for <Module>" -ForEach @(
            @{ Module = 'api' }
            @{ Module = 'web' }
        ) {
            $template = Get-Content (Join-Path $PSScriptRoot "../../infra/modules/$Module.bicep") -Raw

            $template | Should -Match "scmIpSecurityRestrictionsUseMain:\s*false"
            $template | Should -Match "scmIpSecurityRestrictions:\s*\[\s*\]"
            $template | Should -Match "scmIpSecurityRestrictionsDefaultAction:\s*'Deny'"
            $template | Should -Match "ftpsState:\s*'Disabled'"
        }

        It "Disables both basic publishing policies for <Module>" -ForEach @(
            @{ Module = 'api' }
            @{ Module = 'web' }
        ) {
            $template = Get-Content (Join-Path $PSScriptRoot "../../infra/modules/$Module.bicep") -Raw

            foreach ($policy in @('scm', 'ftp')) {
                $template | Should -Match "resource \w+ 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2023-12-01' = \{\s+parent: $Module\s+name: '$policy'\s+properties: \{\s+allow: false"
            }
        }

        It "Applies the same hardened modules to production and PR previews" {
            foreach ($file in @('applications', 'pr-applications')) {
                $template = Get-Content (Join-Path $PSScriptRoot "../../infra/$file.bicep") -Raw

                $template | Should -Match "module apiApp './modules/api.bicep'"
                $template | Should -Match "module webApp './modules/web.bicep'"
            }
        }
    }

    Context "Parameters" {
        It "Defaults Mode to whatif and restricts it to validate, whatif and deploy" {
            $mode = Get-ScriptParameter $realScript 'Mode'
            $mode.DefaultValue.Value | Should -Be 'whatif'
            Get-ParameterValidateSet $mode | Should -Be @('validate', 'whatif', 'deploy')
        }

        It "Has an optional ImageTag and defaults Location to swedencentral" {
            (Test-ParameterMandatory (Get-ScriptParameter $realScript 'ImageTag')) | Should -BeFalse
            (Get-ScriptParameter $realScript 'Location').DefaultValue.Value | Should -Be 'swedencentral'
        }

        It "Rejects an unknown Mode" {
            $result = Start-AppsDeploy @{ Mode = 'apply' }

            $result.Error | Should -Not -BeNullOrEmpty
            Should -Invoke Get-AzContext -Times 0 -Exactly
        }
    }

    Context "Pre-flight checks" {
        It "Exits 1 when Azure PowerShell is not logged in" {
            Mock Get-AzContext { $null }

            $result = Start-AppsDeploy

            $result.ExitCode | Should -Be 1
            Should -Invoke Test-AzDeployment -Times 0 -Exactly
        }

        It "Exits 1 when the template files are missing" {
            $result = Start-AppsDeploy -OmitTemplates

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'File not found'
        }

        It "Refuses to deploy without an ImageTag" {
            $result = Start-AppsDeploy @{ Mode = 'deploy' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'ImageTag is required for deploy mode'
            Should -Invoke New-AzDeployment -Times 0 -Exactly
        }

        It "Uses a placeholder tag in whatif/validate mode" {
            $result = Start-AppsDeploy @{ Mode = 'validate' }

            $result.ExitCode | Should -Be 0
            $env:API_IMAGE_TAG | Should -Be '00000000000000'
            $env:WEB_IMAGE_TAG | Should -Be '00000000000000'
        }

        It "Exports the tag for the .bicepparam files before deploying" {
            Start-AppsDeploy @{ Mode = 'deploy'; ImageTag = '20260501120000' } | Out-Null

            $az.TagAtDeploy | Should -Be '20260501120000'
            $env:WEB_IMAGE_TAG | Should -Be '20260501120000'
        }
    }

    Context "Validate and WhatIf" {
        It "Validate mode only validates" {
            $result = Start-AppsDeploy @{ Mode = 'validate' }

            $result.ExitCode | Should -Be 0
            Should -Invoke Test-AzDeployment -Times 1 -Exactly -ParameterFilter { $TemplateFile -like '*infra/applications.bicep' }
            Should -Invoke New-AzDeployment -Times 0 -Exactly
        }

        It "Exits 1 with the validation messages when the template is invalid" {
            $az.ValidationErrors = @([pscustomobject]@{ Message = 'bad parameter' })

            $result = Start-AppsDeploy @{ Mode = 'validate' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'bad parameter'
        }

        It "WhatIf mode previews with -WhatIf and checks no certificates" {
            $result = Start-AppsDeploy

            $result.ExitCode | Should -Be 0
            Should -Invoke New-AzDeployment -Times 1 -Exactly -ParameterFilter { $WhatIf }
            Should -Invoke Get-AzResource -Times 0 -Exactly
            Should -Invoke New-AzRoleAssignment -Times 0 -Exactly
        }

        It "Exits 1 when What-If fails" {
            Mock New-AzDeployment { throw 'arm exploded' }

            $result = Start-AppsDeploy

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'What-If failed'
        }
    }

    Context "Deploy mode" {
        It "Deploys the sites with a timestamped name when all wildcard certificates already exist" {
            $result = Start-AppsDeploy @{ Mode = 'deploy'; ImageTag = '20260501120000' }

            $result.ExitCode | Should -Be 0
            Should -Invoke New-AzDeployment -Times 1 -Exactly -ParameterFilter { $Name -match '^techhub-prod-apps-\d{8}-\d{6}$' -and -not $WhatIf }
            Should -Invoke New-AzRoleAssignment -Times 0 -Exactly
            Should -Invoke New-AzResourceGroupDeployment -Times 0 -Exactly
            $result.Output | Should -Match 'All wildcard certificates present'
        }

        It "Exits 1 when the deployment fails" {
            Mock New-AzDeployment { throw 'The template deployment failed' }

            $result = Start-AppsDeploy @{ Mode = 'deploy'; ImageTag = 't1' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'App Service sites deployment failed'
        }

        It "Imports only the missing certificates after granting Key Vault roles to the App Service provider" {
            $az.ExistingCerts = @('wildcard-hub-ms')

            $result = Start-AppsDeploy @{ Mode = 'deploy'; ImageTag = 't1' }

            $result.ExitCode | Should -Be 0
            $az.Roles | Should -HaveCount 2
            $az.Roles | Should -Contain 'db79e9a7-68ee-4b58-9aeb-b90e7c24fcba|/subs/x/vaults/kv|ServicePrincipal'
            $az.Roles | Should -Contain '4633458b-17de-408a-b874-0445c86b69e6|/subs/x/vaults/kv|ServicePrincipal'
            $az.ImportedCerts | Should -Be @('wildcard-xebia-ms')
            Should -Invoke New-AzResourceGroupDeployment -Times 1 -Exactly -ParameterFilter {
                $ResourceGroupName -eq 'rg-techhub-prod' -and $keyVaultSecretName -eq 'wildcard-xebia-ms' -and
                $appServicePlanId -eq '/subs/x/serverfarms/asp' -and $location -eq 'swedencentral'
            }
        }

        It "Imports both certificates on a freshly rebuilt environment" {
            $az.ExistingCerts = @()

            Start-AppsDeploy @{ Mode = 'deploy'; ImageTag = 't1' } | Out-Null

            $az.ImportedCerts | Should -Be @('wildcard-hub-ms', 'wildcard-xebia-ms')
        }

        It "Ignores role assignments that already exist" {
            $az.ExistingCerts = @()
            $az.RoleBehavior = { throw 'RoleAssignmentExists: already exists' }

            $result = Start-AppsDeploy @{ Mode = 'deploy'; ImageTag = 't1' }

            $result.ExitCode | Should -Be 0
            $az.ImportedCerts | Should -HaveCount 2
        }

        It "Fails (without deploying the sites) when the role assignment fails for another reason" {
            $az.ExistingCerts = @()
            $az.RoleBehavior = { throw 'Authorization failed' }

            $result = Start-AppsDeploy @{ Mode = 'deploy'; ImageTag = 't1' }

            $result.Error | Should -Not -BeNullOrEmpty
            $result.Error.Exception.Message | Should -Match 'Authorization failed'
            Should -Invoke New-AzDeployment -Times 0 -Exactly
        }

        It "Retries the import while Key Vault access has not propagated" {
            $az.ExistingCerts = @('wildcard-hub-ms')
            $az.ImportBehavior = { if ($az.ImportAttempts -lt 3) { throw 'The service does not have access to the Key Vault' } }

            $result = Start-AppsDeploy @{ Mode = 'deploy'; ImageTag = 't1' }

            $result.ExitCode | Should -Be 0
            $az.ImportAttempts | Should -Be 3
            $az.ImportedCerts | Should -Be @('wildcard-xebia-ms')
            Should -Invoke Start-Sleep -Times 2 -Exactly -ParameterFilter { $Seconds -eq 20 }
        }

        It "Gives up after 6 attempts and does not deploy the sites" {
            $az.ExistingCerts = @('wildcard-hub-ms')
            $az.ImportBehavior = { throw 'The service does not have access to the Key Vault' }

            $result = Start-AppsDeploy @{ Mode = 'deploy'; ImageTag = 't1' }

            $result.Error | Should -Not -BeNullOrEmpty
            $az.ImportAttempts | Should -Be 6
            Should -Invoke New-AzDeployment -Times 0 -Exactly
        }

        It "Does not retry unrelated import errors" {
            $az.ExistingCerts = @('wildcard-hub-ms')
            $az.ImportBehavior = { throw 'Quota exceeded' }

            $result = Start-AppsDeploy @{ Mode = 'deploy'; ImageTag = 't1' }

            $result.Error | Should -Not -BeNullOrEmpty
            $az.ImportAttempts | Should -Be 1
        }
    }
}
