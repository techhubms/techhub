Describe "Restore-Database" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $script:scriptPath = Join-Path $PSScriptRoot "../../scripts/Restore-Database.ps1"
        $script:realGetCommand = Get-Command -Name Get-Command -CommandType Cmdlet
        function pg_dump { }
        function pg_restore { }
        function lsof { }
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
            $script:paramBlock = $ast.ParamBlock
        }

        It "Should have a mandatory Target parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "Target" }
            $param | Should -Not -BeNullOrEmpty
            $mandatory = $param.Attributes | Where-Object {
                $_.TypeName.Name -eq "Parameter" -and
                ($_.NamedArguments | Where-Object { $_.ArgumentName -eq "Mandatory" -and $_.Argument.Extent.Text -eq '$true' })
            }
            $mandatory | Should -Not -BeNullOrEmpty
        }

        It "Should validate Target to local only (staging removed — permanent staging DB deleted)" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "Target" }
            $validateSet = $param.Attributes | Where-Object { $_.TypeName.Name -eq "ValidateSet" }
            $validateSet | Should -Not -BeNullOrEmpty
            $values = $validateSet.PositionalArguments | ForEach-Object { $_.Value }
            $values | Should -Contain "local"
            $values | Should -Not -Contain "staging"
        }

        It "Should have an optional ProductionConnectionString parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "ProductionConnectionString" }
            $param | Should -Not -BeNullOrEmpty
        }

        It "Should have an optional TargetConnectionString parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "TargetConnectionString" }
            $param | Should -Not -BeNullOrEmpty
        }

        It "Should have a SkipDump switch parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "SkipDump" }
            $param | Should -Not -BeNullOrEmpty
            $param.StaticType.Name | Should -Be "SwitchParameter"
        }

        It "Should have a SkipRestore switch parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "SkipRestore" }
            $param | Should -Not -BeNullOrEmpty
            $param.StaticType.Name | Should -Be "SwitchParameter"
        }

        It "Should have a TablesOnly switch parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "TablesOnly" }
            $param | Should -Not -BeNullOrEmpty
            $param.StaticType.Name | Should -Be "SwitchParameter"
        }
    }

    Context "Staging target removed" {
        BeforeAll {
            $script:content = Get-Content $scriptPath -Raw
        }

        It "Should not reference psql-techhub-staging (permanent staging DB was deleted)" {
            $content | Should -Not -Match 'psql-techhub-staging'
        }

        It "Should not reference ca-techhub-api-staging or ca-techhub-web-staging (permanent staging apps deleted)" {
            $content | Should -Not -Match 'ca-techhub-api-staging'
            $content | Should -Not -Match 'ca-techhub-web-staging'
        }

        It "Should still drop and recreate database for local target" {
            $content | Should -Match '\$dropAndRecreate\s*=\s*\$true'
        }
    }

    Context "Local restore behavior" {
        BeforeAll {
            $script:content = Get-Content $scriptPath -Raw
        }

        It "Should stop local docker compose services before restore" {
            $content | Should -Match 'docker compose stop api web'
        }

        It "Should reset local database password after restore" {
            $content | Should -Match "ALTER ROLE.*WITH PASSWORD"
        }

        It "Should leave the restored production data unaltered (no RSS feed disabling)" {
            $content | Should -Not -Match 'UPDATE\s+rss_feed_configs'
        }
    }

    Context "Reusing existing dumps" {
        BeforeEach {
            $sandbox = [System.IO.Path]::GetFullPath(
                (Join-Path $PSScriptRoot "../../.tmp/restore-test-$([guid]::NewGuid().ToString('N'))"))
            New-Item -ItemType Directory -Path "$sandbox/scripts", "$sandbox/src", "$sandbox/.tmp" -Force | Out-Null
            Copy-Item $scriptPath "$sandbox/scripts/Restore-Database.ps1"
            $script:sandboxScript = Join-Path $sandbox "scripts/Restore-Database.ps1"
            $cli = New-CliFake
            $script:savedEnv = Save-EnvironmentVariable -Name @('PGHOST', 'PGPORT', 'PGDATABASE', 'PGUSER', 'PGPASSWORD', 'PGSSLMODE')
            Mock Get-Command {
                if ($Name -in @('pg_dump', 'pg_restore')) {
                    return [pscustomobject]@{ Source = "fake-$Name" }
                }
                & $realGetCommand @PesterBoundParameters
            }
            Mock az { Invoke-CliFake -Fake $cli -Command 'az' -Arguments $args }
            Mock pg_dump { Invoke-CliFake -Fake $cli -Command 'pg_dump' -Arguments $args }
            Mock pg_restore { Invoke-CliFake -Fake $cli -Command 'pg_restore' -Arguments $args }
            Mock psql { Invoke-CliFake -Fake $cli -Command 'psql' -Arguments $args }
            Mock docker { Invoke-CliFake -Fake $cli -Command 'docker' -Arguments $args }
            Mock lsof { }
            Mock Invoke-RestMethod { throw "Production network must not be contacted" }
            Add-CliRule $cli -Command 'docker' -Match '^docker compose ps' -Output 'techhub-postgres'
        }

        AfterEach {
            Restore-EnvironmentVariable -Saved $savedEnv
            Remove-Item -LiteralPath $sandbox -Recurse -Force
        }

        It "Restores the newest saved dump without contacting production" {
            $old = Join-Path $sandbox '.tmp/db-restore-20260101.dump'
            $latest = Join-Path $sandbox '.tmp/db-backup-20260201.dump'
            Set-Content $old 'old dump'
            Set-Content $latest 'new dump'
            (Get-Item $old).LastWriteTimeUtc = [datetime]'2026-01-01'
            (Get-Item $latest).LastWriteTimeUtc = [datetime]'2026-02-01'
            Set-Content "$sandbox/.tmp/unrelated.dump" 'not a database backup'
            New-Item -ItemType Directory "$sandbox/.tmp/db-backup-directory.dump" | Out-Null

            $result = Invoke-ScriptUnderTest $sandboxScript @{ Target = 'local'; UseExistingDump = $true }

            $result.Error | Should -BeNullOrEmpty
            $result.ExitCode | Should -Be 0
            $result.Output | Should -Match 'Using existing dump:'
            @(Get-CliCalls $cli '^pg_restore') | Should -HaveCount 1
            @(Get-CliCalls $cli '^pg_restore')[0] | Should -BeLike "*$latest"
            @(Get-CliCalls $cli '^(az|pg_dump)') | Should -BeNullOrEmpty
            Should -Invoke Invoke-RestMethod -Times 0 -Exactly
        }

        It "Fails explicitly when no saved dump exists instead of downloading one" {
            $result = Invoke-ScriptUnderTest $sandboxScript @{ Target = 'local'; UseExistingDump = $true }

            $result.Error.Exception.Message | Should -Match 'No existing database dump found'
            @(Get-CliCalls $cli '^(az|pg_dump|pg_restore|docker|psql)') | Should -BeNullOrEmpty
        }

        It "Breaks equal modification-time ties consistently by filename" {
            $first = Join-Path $sandbox '.tmp/db-restore-a.dump'
            $second = Join-Path $sandbox '.tmp/db-restore-b.dump'
            foreach ($dump in @($second, $first)) {
                Set-Content $dump 'dump'
                (Get-Item $dump).LastWriteTimeUtc = [datetime]'2026-01-01'
            }

            $result = Invoke-ScriptUnderTest $sandboxScript @{
                Target = 'local'; UseExistingDump = $true; SkipRestore = $true
            }

            $result.Error | Should -BeNullOrEmpty
            $result.ExitCode | Should -Be 0
            $result.Output | Should -BeLike "*Using existing dump: $first*"
            @(Get-CliCalls $cli '^(az|pg_dump|pg_restore|docker|psql)') | Should -BeNullOrEmpty
        }

        It "Rejects an explicit OutputPath with automatic dump selection" {
            $result = Invoke-ScriptUnderTest $sandboxScript @{
                Target = 'local'; UseExistingDump = $true; OutputPath = "$sandbox/.tmp/custom.dump"
            }

            $result.Error.Exception.Message | Should -Match 'UseExistingDump cannot be combined with OutputPath'
            @(Get-CliCalls $cli '^(az|pg_dump|pg_restore|docker|psql)') | Should -BeNullOrEmpty
        }

        It "Can reuse the only saved dump from the script directory" {
            $dump = Join-Path $sandbox '.tmp/db-restore-single.dump'
            Set-Content $dump 'dump'
            Push-Location "$sandbox/scripts"
            try {
                $result = Invoke-ScriptUnderTest $sandboxScript @{ Target = 'disk'; UseExistingDump = $true }
            }
            finally {
                Pop-Location
            }

            $result.Error | Should -BeNullOrEmpty
            $result.ExitCode | Should -Be 0
            $result.Output | Should -BeLike "*Using existing dump: $dump*"
            @(Get-CliCalls $cli '^(az|pg_dump|pg_restore|docker|psql)') | Should -BeNullOrEmpty
        }

        It "Keeps explicit SkipDump and OutputPath behavior" {
            $dump = Join-Path $sandbox '.tmp/custom.dump'
            Set-Content $dump 'dump'

            $result = Invoke-ScriptUnderTest $sandboxScript @{
                Target = 'local'; SkipDump = $true; OutputPath = $dump; SkipRestore = $true
            }

            $result.Error | Should -BeNullOrEmpty
            $result.ExitCode | Should -Be 0
            $result.Output | Should -BeLike "*Using existing dump: $dump*"
            @(Get-CliCalls $cli '^(az|pg_dump|pg_restore)') | Should -BeNullOrEmpty
        }
    }
}
