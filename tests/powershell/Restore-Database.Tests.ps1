Describe "Restore-Database" {
    BeforeAll {
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Restore-Database.ps1"
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
            $content = Get-Content $scriptPath -Raw
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
            $content = Get-Content $scriptPath -Raw
        }

        It "Should stop local docker compose services before restore" {
            $content | Should -Match 'docker compose stop api web'
        }

        It "Should reset local database password after restore" {
            $content | Should -Match "ALTER ROLE.*WITH PASSWORD"
        }
    }
}
