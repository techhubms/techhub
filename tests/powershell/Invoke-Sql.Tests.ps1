Describe "Invoke-Sql" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Invoke-Sql.ps1"
        $envNames = @('PGPASSWORD', 'PAGER')
    }

    BeforeEach {
        $savedEnv = Save-EnvironmentVariable $envNames
        $env:PGPASSWORD = $null
        $env:PAGER = 'less'
        $cli = New-CliFake
        $seen = [pscustomobject]@{ Password = $null; Pager = 'unset' }
        Mock psql { $seen.Password = $env:PGPASSWORD; $seen.Pager = $env:PAGER; Invoke-CliFake -Fake $cli -Command 'psql' -Arguments $args }
    }

    AfterEach {
        Restore-EnvironmentVariable $savedEnv
    }

    It "Requires a Query parameter" {
        Test-ParameterMandatory (Get-ScriptParameter $scriptPath 'Query') | Should -BeTrue
    }

    It "Runs the query against the local techhub database only" {
        Add-CliRule $cli 'psql' '^psql' 'a|b'

        $result = Invoke-ScriptUnderTest $scriptPath @{ Query = 'SELECT 1' }

        $result.Output | Should -Match 'a\|b'
        $call = @(Get-CliCalls $cli '^psql')
        $call | Should -HaveCount 1
        $call[0] | Should -Be 'psql -h localhost -p 5432 -d techhub -U techhub --no-psqlrc -A -F | -c SELECT 1'
    }

    It "Passes the query as a single argument (no shell splitting)" {
        Invoke-ScriptUnderTest $scriptPath @{ Query = "SELECT * FROM content_items WHERE title = 'a b'; DROP TABLE x" } | Out-Null

        Should -Invoke psql -Times 1 -Exactly -ParameterFilter { $args[-1] -eq "SELECT * FROM content_items WHERE title = 'a b'; DROP TABLE x" }
    }

    It "Authenticates with the local dev password and disables the pager" {
        Invoke-ScriptUnderTest $scriptPath @{ Query = 'SELECT 1' } | Out-Null

        $seen.Password | Should -Be 'localdev'
        $seen.Pager | Should -BeNullOrEmpty
    }

    It "Never targets a remote host" {
        Invoke-ScriptUnderTest $scriptPath @{ Query = 'SELECT 1' } | Out-Null

        @(Get-CliCalls $cli '^psql')[0] | Should -Not -Match 'azure|\.com|\.net'
        @(Get-CliCalls $cli '^psql')[0] | Should -Match '-h localhost'
    }
}
