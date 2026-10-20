Describe "Deploy-PrPreview" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $realScript = Join-Path $PSScriptRoot "../../scripts/Deploy-PrPreview.ps1"
        $envNames = @('ADMIN_IP_ADDRESSES')

        function Start-PrPreview {
            param([hashtable]$Parameters, [int]$WaitExitCode = 0)
            $root = New-ScriptSandbox -Scripts @('Deploy-PrPreview.ps1') -StubScripts @{
                'Wait-ForLiveVersion.ps1' = (New-RecordingStubScript -ExitCode $WaitExitCode)
            }
            $sandboxes.Add($root)
            $result = Invoke-ScriptUnderTest (Join-Path $root 'scripts/Deploy-PrPreview.ps1') $Parameters
            $result | Add-Member -NotePropertyName Sandbox -NotePropertyValue $root -PassThru
        }

        function Get-AzCallIndex {
            param([string]$Match)
            $cli.Calls.FindIndex({ param($c) $c -match $Match })
        }

        $bicepOutput = '{"properties":{"outputs":{"webFqdn":{"value":"app-techhub-web-pr-42.azurewebsites.net"}}}}'
    }

    BeforeEach {
        $savedEnv = Save-EnvironmentVariable $envNames
        $env:ADMIN_IP_ADDRESSES = '1.1.1.1,2.2.2.2'
        $sandboxes = [System.Collections.Generic.List[string]]::new()
        $cli = New-CliFake

        Mock Start-Sleep { }
        Mock az { Invoke-CliFake -Fake $cli -Command 'az' -Arguments $args }
        Mock gh { Invoke-CliFake -Fake $cli -Command 'gh' -Arguments $args }

        # Default state: logged in, and no PR resources exist yet.
        Add-CliRule $cli 'az' '^az account show' '{"name":"Test Subscription"}'
        Add-CliRule $cli 'az' '^az postgres flexible-server show --name psql-techhub-prod ' '/subs/x/flexibleServers/psql-techhub-prod'
        Add-CliRule $cli 'az' '^az postgres flexible-server show --name psql-techhub-pr-42 ' '/subs/x/flexibleServers/psql-techhub-pr-42'
        Add-CliRule $cli 'az' '^az identity show --name id-techhub-pr' 'principal-1234'
        Add-CliRule $cli 'az' '^az postgres flexible-server microsoft-entra-admin list' '[]'
        Add-CliRule $cli 'az' '^az deployment group create' $bicepOutput
        Add-CliRule $cli 'az' '^az webapp show -n ' '{"id":"/subs/x/sites/preview","state":"Running","defaultHostName":"preview.azurewebsites.net","linuxFxVersion":"DOCKER|ghcr.io/owner/image:t1"}'
        Add-CliRule $cli 'az' '^az monitor activity-log list' '[]'
    }

    AfterEach {
        Restore-EnvironmentVariable $savedEnv
        foreach ($root in $sandboxes) { Remove-ScriptSandbox $root }
    }

    Context "Parameters" {
        It "Requires PrNumber (int) and Action" {
            $pr = Get-ScriptParameter $realScript 'PrNumber'
            Test-ParameterMandatory $pr | Should -BeTrue
            $pr.StaticType.Name | Should -Be 'Int32'
            Test-ParameterMandatory (Get-ScriptParameter $realScript 'Action') | Should -BeTrue
        }

        It "Restricts Action to deploy and teardown" {
            Get-ParameterValidateSet (Get-ScriptParameter $realScript 'Action') | Should -Be @('deploy', 'teardown')
        }

        It "Defaults the registry owner to techhubms and the auth user to the owner" {
            (Get-ScriptParameter $realScript 'GithubRegistryUsername').DefaultValue.Value | Should -Be 'techhubms'
            (Get-ScriptParameter $realScript 'GithubRegistryAuthUsername').DefaultValue.Extent.Text | Should -Be '$GithubRegistryUsername'
        }

        It "Rejects an unknown Action and a non-numeric PrNumber" {
            (Start-PrPreview @{ PrNumber = 1; Action = 'destroy' }).Error | Should -Not -BeNullOrEmpty
            (Start-PrPreview @{ PrNumber = 'abc'; Action = 'teardown' }).Error | Should -Not -BeNullOrEmpty
            @(Get-CliCalls $cli '^az') | Should -BeNullOrEmpty
        }
    }

    Context "Prerequisites" {
        It "Exits 1 when Azure CLI is not logged in" {
            $cli.Rules.Clear()
            Add-CliRule $cli 'az' '^az account show' 'ERROR' -ExitCode 1

            $result = Start-PrPreview @{ PrNumber = 42; Action = 'teardown' }

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^az (webapp|postgres|appservice|network)') | Should -BeNullOrEmpty
        }

        It "Requires a Tag for the deploy action" {
            $result = Start-PrPreview @{ PrNumber = 42; Action = 'deploy' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Tag is required for the deploy action'
            @(Get-CliCalls $cli '^az postgres') | Should -BeNullOrEmpty
        }

        It "Requires ADMIN_IP_ADDRESSES for deploy and falls back to the GitHub variable" {
            $env:ADMIN_IP_ADDRESSES = $null

            $failed = Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' }
            $failed.ExitCode | Should -Be 1
            $failed.Output | Should -Match 'ADMIN_IP_ADDRESSES is not set'

            Add-CliRule $cli 'gh' '^gh variable get ADMIN_IP_ADDRESSES --repo techhubms/techhub' '3.3.3.3'
            Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' } | Out-Null
            $env:ADMIN_IP_ADDRESSES | Should -Be '3.3.3.3'
        }
    }

    Context "Bicep scheduled job configuration" {
        It "Disables scheduled content processing, roundup generation, and newsletter sends for PR previews" {
            $prTemplate = Get-Content (Join-Path $PSScriptRoot "../../infra/pr-applications.bicep") -Raw
            $applicationTemplate = Get-Content (Join-Path $PSScriptRoot "../../infra/applications.bicep") -Raw
            $apiModule = Get-Content (Join-Path $PSScriptRoot "../../infra/modules/api.bicep") -Raw

            $prTemplate | Should -Match "contentProcessorEnabled:\s*false"
            $prTemplate | Should -Match "roundupGeneratorEnabled:\s*false"
            $prTemplate | Should -Match "newsletterScheduledSendEnabled:\s*false"
            $applicationTemplate | Should -Match "contentProcessorEnabled:\s*true"
            $applicationTemplate | Should -Match "roundupGeneratorEnabled:\s*true"
            $applicationTemplate | Should -Match "newsletterScheduledSendEnabled:\s*true"
            $apiModule | Should -Match "name: 'ContentProcessor__Enabled'\s+value: contentProcessorEnabled"
            $apiModule | Should -Match "name: 'RoundupGenerator__Enabled'\s+value: roundupGeneratorEnabled"
            $apiModule | Should -Match "name: 'Newsletter__ScheduledSendEnabled'\s+value: newsletterScheduledSendEnabled"
        }
    }

    Context "Teardown" {
        BeforeEach {
            Add-CliRule $cli 'az' "webapp list .*name=='app-techhub-web-pr-42'" 'app-techhub-web-pr-42'
            Add-CliRule $cli 'az' "webapp list .*name=='app-techhub-api-pr-42'" 'app-techhub-api-pr-42'
            Add-CliRule $cli 'az' 'appservice plan show --name asp-techhub-pr .*--query name' 'asp-techhub-pr'
            Add-CliRule $cli 'az' 'appservice plan show --name asp-techhub-pr .*--query numberOfSites' '0'
            Add-CliRule $cli 'az' '^az network private-endpoint show --name pe-psql-techhub-pr-42' 'pe-psql-techhub-pr-42'
            Add-CliRule $cli 'az' "postgres flexible-server list .*name=='psql-techhub-pr-42'" 'psql-techhub-pr-42'
        }

        It "Deletes Web, then API, the empty plan, the private endpoint and finally the PostgreSQL server" {
            $result = Start-PrPreview @{ PrNumber = 42; Action = 'teardown' }

            $result.ExitCode | Should -Be 0
            $deletes = @(Get-CliCalls $cli ' delete ')
            $deletes | Should -HaveCount 5
            $deletes[0] | Should -Match '^az webapp delete --name app-techhub-web-pr-42 --resource-group rg-techhub-prod'
            $deletes[1] | Should -Match '^az webapp delete --name app-techhub-api-pr-42 --resource-group rg-techhub-prod'
            $deletes[2] | Should -Match '^az appservice plan delete --name asp-techhub-pr .*--yes'
            $deletes[3] | Should -Match '^az network private-endpoint delete --name pe-psql-techhub-pr-42'
            $deletes[4] | Should -Match '^az postgres flexible-server delete --name psql-techhub-pr-42 .*--yes'
            $result.Output | Should -Match 'removed successfully'
        }

        It "Only ever deletes resources named after the PR, never production resources" {
            Start-PrPreview @{ PrNumber = 42; Action = 'teardown' } | Out-Null

            foreach ($call in @(Get-CliCalls $cli ' delete ')) {
                $call | Should -Not -Match 'app-techhub-(api|web)-prod|psql-techhub-prod|asp-techhub-prod|id-techhub-prod|kv-techhub'
                $call | Should -Match '(-pr-42|asp-techhub-pr)'
            }
        }

        It "Derives resource names from the PR number" {
            Add-CliRule $cli 'az' "webapp list .*name=='app-techhub-web-pr-7'" 'app-techhub-web-pr-7'

            Start-PrPreview @{ PrNumber = 7; Action = 'teardown' } | Out-Null

            @(Get-CliCalls $cli ' delete ') -join "`n" | Should -Not -Match 'pr-42'
            @(Get-CliCalls $cli '^az webapp delete') | Should -HaveCount 1
            @(Get-CliCalls $cli '^az webapp delete')[0] | Should -Match 'app-techhub-web-pr-7 '
        }

        It "Keeps the shared plan while other PR sites still use it" {
            Add-CliRule $cli 'az' 'appservice plan show --name asp-techhub-pr .*--query numberOfSites' '2' -First

            $result = Start-PrPreview @{ PrNumber = 42; Action = 'teardown' }

            $result.ExitCode | Should -Be 0
            @(Get-CliCalls $cli '^az appservice plan delete') | Should -BeNullOrEmpty
            $result.Output | Should -Match 'still has 2 site\(s\)'
        }

        It "Does not delete the plan when its site count cannot be read" {
            Add-CliRule $cli 'az' 'appservice plan show --name asp-techhub-pr .*--query numberOfSites' '' -ExitCode 1 -First

            Start-PrPreview @{ PrNumber = 42; Action = 'teardown' } | Out-Null

            @(Get-CliCalls $cli '^az appservice plan delete') | Should -BeNullOrEmpty
        }

        It "Succeeds with 'Nothing to remove' when nothing exists" {
            $cli.Rules.Clear()
            Add-CliRule $cli 'az' '^az account show' '{"name":"s"}'

            $result = Start-PrPreview @{ PrNumber = 99; Action = 'teardown' }

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'Nothing to remove for PR #99'
            @(Get-CliCalls $cli ' delete ') | Should -BeNullOrEmpty
        }

        It "Exits 1 and stops when a site delete fails" {
            Add-CliRule $cli 'az' '^az webapp delete --name app-techhub-web-pr-42' '' -ExitCode 1 -First

            $result = Start-PrPreview @{ PrNumber = 42; Action = 'teardown' }

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^az postgres flexible-server delete') | Should -BeNullOrEmpty
        }

        It "Continues with a warning when the private endpoint or database delete fails" {
            Add-CliRule $cli 'az' '^az network private-endpoint delete' '' -ExitCode 1 -First
            Add-CliRule $cli 'az' '^az postgres flexible-server delete' '' -ExitCode 1 -First

            $result = Start-PrPreview @{ PrNumber = 42; Action = 'teardown' }

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'Failed to delete private endpoint'
            $result.Output | Should -Match 'Failed to delete psql-techhub-pr-42'
        }

        It "Never provisions anything during teardown" {
            Start-PrPreview @{ PrNumber = 42; Action = 'teardown' } | Out-Null

            @(Get-CliCalls $cli '^az (deployment|postgres flexible-server restore|appservice plan create|network private-endpoint create)') | Should -BeNullOrEmpty
        }
    }

    Context "Deploy" {
        It "Restores the PR database via PITR from production at 03:00 UTC" {
            $result = Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 'pr-42-20260101120000' }

            $result.ExitCode | Should -Be 0
            $restore = @(Get-CliCalls $cli '^az postgres flexible-server restore')
            $restore | Should -HaveCount 1
            $restore[0] | Should -Match '--name psql-techhub-pr-42'
            $restore[0] | Should -Match '--source-server /subs/x/flexibleServers/psql-techhub-prod'
            $restore[0] | Should -Match '--restore-time \d{4}-\d{2}-\d{2}T03:00:00Z$'
        }

        It "Reuses an existing PR database instead of restoring again" {
            Add-CliRule $cli 'az' "postgres flexible-server list .*name=='psql-techhub-pr-42'" 'psql-techhub-pr-42' -First

            $result = Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' }

            $result.ExitCode | Should -Be 0
            @(Get-CliCalls $cli '^az postgres flexible-server restore') | Should -BeNullOrEmpty
            $result.Output | Should -Match 'already exists — reusing psql-techhub-pr-42'
        }

        It "Exits 1 when the production server to clone cannot be found" {
            Add-CliRule $cli 'az' '^az postgres flexible-server show --name psql-techhub-prod ' '' -ExitCode 1 -First

            $result = Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' }

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^az postgres flexible-server restore') | Should -BeNullOrEmpty
        }

        It "Exits 1 when the PITR restore fails" {
            Add-CliRule $cli 'az' '^az postgres flexible-server restore' '' -ExitCode 1 -First

            $result = Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' }

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^az deployment group create') | Should -BeNullOrEmpty
        }

        It "Switches the PR database to Entra-only auth and registers the shared PR identity as admin" {
            Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' } | Out-Null

            $update = @(Get-CliCalls $cli '^az postgres flexible-server update')
            $update | Should -HaveCount 1
            $update[0] | Should -Match '--microsoft-entra-auth Enabled --password-auth Disabled'
            $admin = @(Get-CliCalls $cli 'microsoft-entra-admin create')
            $admin | Should -HaveCount 1
            $admin[0] | Should -Match '--object-id principal-1234 --type ServicePrincipal'
        }

        It "Does not register the Entra admin again when it already exists" {
            Add-CliRule $cli 'az' '^az postgres flexible-server microsoft-entra-admin list' '[{"objectId":"principal-1234"}]' -First

            $result = Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' }

            $result.ExitCode | Should -Be 0
            @(Get-CliCalls $cli 'microsoft-entra-admin create') | Should -BeNullOrEmpty
        }

        It "Exits 1 when the shared PR identity is missing" {
            Add-CliRule $cli 'az' '^az identity show' '' -ExitCode 1 -First

            $result = Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' }

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match "Shared PR managed identity 'id-techhub-pr' not found"
        }

        It "Opens the firewall only for each admin IP and removes stale admin rules first" {
            Add-CliRule $cli 'az' '^az postgres flexible-server firewall-rule list .*allow-admin-ip-' "allow-admin-ip-0`nallow-admin-ip-5" -First

            Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' } | Out-Null

            $deletes = @(Get-CliCalls $cli '^az postgres flexible-server firewall-rule delete')
            $deletes | Should -HaveCount 2
            $creates = @(Get-CliCalls $cli '^az postgres flexible-server firewall-rule create')
            $creates | Should -HaveCount 2
            $creates[0] | Should -Match '--name allow-admin-ip-0 --start-ip-address 1.1.1.1 --end-ip-address 1.1.1.1'
            $creates[1] | Should -Match '--name allow-admin-ip-1 --start-ip-address 2.2.2.2 --end-ip-address 2.2.2.2'
            $creates -join "`n" | Should -Not -Match '0\.0\.0\.0'
            (Get-AzCallIndex 'firewall-rule delete') | Should -BeLessThan (Get-AzCallIndex 'firewall-rule create')
        }

        It "Creates the private endpoint and DNS zone group when missing, skips them when present" {
            Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' } | Out-Null
            @(Get-CliCalls $cli '^az network private-endpoint create') | Should -HaveCount 1
            @(Get-CliCalls $cli '^az network private-endpoint create')[0] | Should -Match '--group-id postgresqlServer'
            @(Get-CliCalls $cli 'dns-zone-group create') | Should -HaveCount 1

            $cli.Calls.Clear()
            Add-CliRule $cli 'az' '^az network private-endpoint show --name pe-psql-techhub-pr-42' 'pe-psql-techhub-pr-42' -First
            Add-CliRule $cli 'az' 'dns-zone-group show' 'default' -First
            Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' } | Out-Null
            @(Get-CliCalls $cli '^az network private-endpoint create') | Should -BeNullOrEmpty
            @(Get-CliCalls $cli 'dns-zone-group create') | Should -BeNullOrEmpty
        }

        It "Creates the shared PR App Service plan only when it does not exist" {
            Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' } | Out-Null
            @(Get-CliCalls $cli '^az appservice plan create') | Should -HaveCount 1
            @(Get-CliCalls $cli '^az appservice plan create')[0] | Should -Match '--name asp-techhub-pr .*--sku B1'

            $cli.Calls.Clear()
            Add-CliRule $cli 'az' 'appservice plan show --name asp-techhub-pr .*--query name' 'asp-techhub-pr' -First
            Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' } | Out-Null
            @(Get-CliCalls $cli '^az appservice plan create') | Should -BeNullOrEmpty
        }

        It "Deploys pr-applications.bicep with the PR number, tag and registry parameters" {
            Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 'pr-42-20260101120000'; GithubRegistryUsername = 'owner'; GithubRegistryAuthUsername = 'authuser' } | Out-Null

            $deploy = @(Get-CliCalls $cli '^az deployment group create')
            $deploy | Should -HaveCount 1
            $deploy[0] | Should -Match '--resource-group rg-techhub-prod'
            $deploy[0] | Should -Match 'pr-applications\.bicep'
            $deploy[0] | Should -Match 'prNumber=42 imageTag=pr-42-20260101120000 githubRegistryUsername=owner githubRegistryAuthUsername=authuser'
        }

        It "Defaults the registry auth user to the registry owner" {
            Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1'; GithubRegistryUsername = 'owner' } | Out-Null

            @(Get-CliCalls $cli '^az deployment group create')[0] | Should -Match 'githubRegistryAuthUsername=owner'
        }

        It "Provisions in order: restore, entra auth, private endpoint, plan, bicep" {
            Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' } | Out-Null

            $order = 'flexible-server restore', 'flexible-server update', 'private-endpoint create', 'appservice plan create', 'deployment group create' |
                ForEach-Object { Get-AzCallIndex $_ }
            $order | Should -Be @($order | Sort-Object)
            ($order | Where-Object { $_ -lt 0 }) | Should -BeNullOrEmpty
        }

        It "Hands the Web FQDN and tag to Wait-ForLiveVersion.ps1" {
            $result = Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 'pr-42-20260101120000' }

            $result.ExitCode | Should -Be 0
            $calls = @(Get-StubScriptCalls $result.Sandbox)
            $calls | Should -HaveCount 1
            $calls[0] | Should -Match 'app-techhub-web-pr-42\.azurewebsites\.net'
            $calls[0] | Should -Match 'pr-42-20260101120000'
        }

        It "Exits 1 when the Bicep deployment fails or returns no FQDN" {
            Add-CliRule $cli 'az' '^az deployment group create' '' -ExitCode 1 -First
            (Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' }).ExitCode | Should -Be 1

            $cli.Rules.RemoveAt(0)
            Add-CliRule $cli 'az' '^az deployment group create' '{"properties":{"outputs":{"webFqdn":{"value":""}}}}' -First
            (Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' }).ExitCode | Should -Be 1
        }

        It "Dumps ARM diagnostics without contacting SCM and exits 1 when the version gate fails" {
            $result = Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' } -WaitExitCode 1

            $result.ExitCode | Should -Be 1
            @(Get-CliCalls $cli '^az webapp show -n app-techhub-web-pr-42') | Should -HaveCount 1
            @(Get-CliCalls $cli '^az webapp show -n app-techhub-api-pr-42') | Should -HaveCount 1
            @(Get-CliCalls $cli '^az monitor activity-log list --resource-id /subs/x/sites/preview --offset 1h') | Should -HaveCount 2
            @(Get-CliCalls $cli '^az webapp log') | Should -BeNullOrEmpty
            $result.Output | Should -Match 'SCM access is denied'
        }

        It "Reports diagnostic failures without masking the failed version gate" {
            Add-CliRule $cli 'az' '^az monitor activity-log list' 'activity log unavailable' -ExitCode 1 -First

            $result = Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' } -WaitExitCode 1

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Could not fetch activity log'
            @(Get-CliCalls $cli '^az webapp show -n ') | Should -HaveCount 2
        }

        It "Reports site status failures without contacting SCM or masking the failed version gate" {
            Add-CliRule $cli 'az' '^az webapp show -n ' 'site status unavailable' -ExitCode 1 -First

            $result = Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' } -WaitExitCode 1

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Could not fetch site status'
            @(Get-CliCalls $cli '^az monitor activity-log list') | Should -BeNullOrEmpty
            @(Get-CliCalls $cli '^az webapp log') | Should -BeNullOrEmpty
        }

        It "Never touches production app names" {
            Start-PrPreview @{ PrNumber = 42; Action = 'deploy'; Tag = 't1' } | Out-Null

            @(Get-CliCalls $cli '^az webapp') -join "`n" | Should -Not -Match 'app-techhub-(api|web)-prod'
            @(Get-CliCalls $cli ' (delete|update|restart|stop) ') -join "`n" | Should -Not -Match 'psql-techhub-prod|app-techhub-(api|web)-prod|asp-techhub-prod'
        }
    }
}
