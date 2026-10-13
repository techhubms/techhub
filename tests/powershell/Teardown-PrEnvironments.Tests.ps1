Describe "Teardown-PrEnvironments" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $realScript = Join-Path $PSScriptRoot "../../scripts/Teardown-PrEnvironments.ps1"

        # Stub Deploy-PrPreview.ps1: records "<args>" and fails for PR numbers listed in stub-fail.txt.
        $deployStub = @'
$log = Join-Path $PSScriptRoot 'stub-calls.log'
Add-Content -Path $log -Value ("Deploy-PrPreview $($args -join ' ')")
$failFile = Join-Path $PSScriptRoot 'stub-fail.txt'
if ((Test-Path $failFile) -and ((Get-Content $failFile) -contains [string]$args[1])) { exit 1 }
exit 0
'@
    }

    BeforeEach {
        $sandbox = New-ScriptSandbox -Scripts @('Teardown-PrEnvironments.ps1') -StubScripts @{ 'Deploy-PrPreview.ps1' = $deployStub }
        $scriptPath = Join-Path $sandbox 'scripts/Teardown-PrEnvironments.ps1'
        $cli = New-CliFake
        Mock az { Invoke-CliFake -Fake $cli -Command 'az' -Arguments $args }
        Add-CliRule $cli 'az' '^az account show' '{"name":"Test Subscription"}'
    }

    AfterEach {
        Remove-ScriptSandbox $sandbox
    }

    Context "Parameters" {
        It "Has an optional DryRun switch and no other parameters" {
            $ast = [System.Management.Automation.Language.Parser]::ParseFile($realScript, [ref]$null, [ref]$null)
            @($ast.ParamBlock.Parameters).Count | Should -Be 1
            (Get-ScriptParameter $realScript 'DryRun').StaticType.Name | Should -Be 'SwitchParameter'
        }
    }

    Context "Prerequisites" {
        It "Exits 1 without scanning when Azure CLI is not logged in" {
            $cli.Rules.Clear()
            Add-CliRule $cli 'az' '^az account show' 'ERROR: Please run az login' -ExitCode 1

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Not logged in'
            Get-CliCalls $cli '^az webapp list' | Should -BeNullOrEmpty
            Get-StubScriptCalls $sandbox | Should -BeNullOrEmpty
        }
    }

    Context "Discovery" {
        It "Only scans the production resource group for PR-named resources" {
            Invoke-ScriptUnderTest $scriptPath @{ DryRun = $true } | Out-Null

            $webList = @(Get-CliCalls $cli '^az webapp list')
            $webList | Should -HaveCount 1
            $webList[0] | Should -Match '--resource-group rg-techhub-prod'
            $webList[0] | Should -Match "starts_with\(name, 'app-techhub-api-pr-'\)"
            $pgList = @(Get-CliCalls $cli '^az postgres flexible-server list')
            $pgList | Should -HaveCount 1
            $pgList[0] | Should -Match "starts_with\(name, 'psql-techhub-pr-'\)"
        }

        It "Exits 0 and tears nothing down when no PR environments exist" {
            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'nothing to do'
            Get-StubScriptCalls $sandbox | Should -BeNullOrEmpty
        }

        It "Finds orphaned databases that have no matching App Service site" {
            Add-CliRule $cli 'az' '^az postgres flexible-server list' "psql-techhub-pr-77`npsql-techhub-pr-5"

            $result = Invoke-ScriptUnderTest $scriptPath @{ DryRun = $true }

            $result.Output | Should -Match 'Found 2 active PR environment\(s\): 5, 77'
        }

        It "De-duplicates PR numbers discovered from both sites and servers and sorts them" {
            Add-CliRule $cli 'az' '^az webapp list' "app-techhub-api-pr-12`napp-techhub-api-pr-3"
            Add-CliRule $cli 'az' '^az postgres flexible-server list' "psql-techhub-pr-12`npsql-techhub-pr-40"

            $result = Invoke-ScriptUnderTest $scriptPath @{ DryRun = $true }

            $result.Output | Should -Match 'Found 3 active PR environment\(s\): 3, 12, 40'
        }

        It "Ignores resources whose names do not carry a PR number" {
            Add-CliRule $cli 'az' '^az webapp list' "app-techhub-api-prod`napp-techhub-api-pr-"

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.Output | Should -Match 'nothing to do'
        }

        It "Treats a failing az list command as no results" {
            Add-CliRule $cli 'az' '^az webapp list' 'app-techhub-api-pr-9' -ExitCode 1

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 0
            Get-StubScriptCalls $sandbox | Should -BeNullOrEmpty
        }
    }

    Context "DryRun" {
        It "Lists environments but never calls Deploy-PrPreview.ps1 or az delete" {
            Add-CliRule $cli 'az' '^az webapp list' 'app-techhub-api-pr-42'

            $result = Invoke-ScriptUnderTest $scriptPath @{ DryRun = $true }

            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'Dry run — would teardown PR\(s\): 42'
            Get-StubScriptCalls $sandbox | Should -BeNullOrEmpty
            Get-CliCalls $cli ' delete' | Should -BeNullOrEmpty
        }
    }

    Context "Teardown" {
        It "Calls Deploy-PrPreview.ps1 -Action teardown once per discovered PR, in ascending order" {
            Add-CliRule $cli 'az' '^az webapp list' "app-techhub-api-pr-42`napp-techhub-api-pr-7"

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 0
            $calls = @(Get-StubScriptCalls $sandbox)
            $calls | Should -HaveCount 2
            $calls[0] | Should -Match '^Deploy-PrPreview .*7 .*teardown'
            $calls[1] | Should -Match '^Deploy-PrPreview .*42 .*teardown'
            $calls | ForEach-Object { $_ | Should -Not -Match 'deploy$' }
            $result.Output | Should -Match 'All 2 PR environment\(s\) torn down successfully'
        }

        It "Never passes a production resource name to the teardown script" {
            Add-CliRule $cli 'az' '^az webapp list' "app-techhub-api-prod`napp-techhub-api-pr-42"
            Add-CliRule $cli 'az' '^az postgres flexible-server list' 'psql-techhub-prod'

            Invoke-ScriptUnderTest $scriptPath | Out-Null

            $calls = @(Get-StubScriptCalls $sandbox)
            $calls | Should -HaveCount 1
            $calls[0] | Should -Not -Match 'prod'
        }

        It "Keeps going after a failed teardown, reports the failed PR and exits 1" {
            Add-CliRule $cli 'az' '^az webapp list' "app-techhub-api-pr-1`napp-techhub-api-pr-2`napp-techhub-api-pr-3"
            Set-Content (Join-Path $sandbox 'scripts/stub-fail.txt') '2'

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            @(Get-StubScriptCalls $sandbox) | Should -HaveCount 3
            $result.Output | Should -Match 'Failed PR\(s\): 2'
        }
    }
}
