Describe "Add-AppInsightsAnnotation" {
    BeforeAll {
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Add-AppInsightsAnnotation.ps1"
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

        It "Should have a mandatory SubscriptionId parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "SubscriptionId" }
            $param | Should -Not -BeNullOrEmpty
            $mandatory = $param.Attributes | Where-Object {
                $_.TypeName.Name -eq "Parameter" -and
                ($_.NamedArguments | Where-Object { $_.ArgumentName -eq "Mandatory" -and $_.Argument.Extent.Text -eq '$true' })
            }
            $mandatory | Should -Not -BeNullOrEmpty
        }

        It "Should have a mandatory ResourceGroupName parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "ResourceGroupName" }
            $param | Should -Not -BeNullOrEmpty
            $mandatory = $param.Attributes | Where-Object {
                $_.TypeName.Name -eq "Parameter" -and
                ($_.NamedArguments | Where-Object { $_.ArgumentName -eq "Mandatory" -and $_.Argument.Extent.Text -eq '$true' })
            }
            $mandatory | Should -Not -BeNullOrEmpty
        }

        It "Should have a mandatory AppInsightsName parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "AppInsightsName" }
            $param | Should -Not -BeNullOrEmpty
            $mandatory = $param.Attributes | Where-Object {
                $_.TypeName.Name -eq "Parameter" -and
                ($_.NamedArguments | Where-Object { $_.ArgumentName -eq "Mandatory" -and $_.Argument.Extent.Text -eq '$true' })
            }
            $mandatory | Should -Not -BeNullOrEmpty
        }

        It "Should have a mandatory AnnotationId parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "AnnotationId" }
            $param | Should -Not -BeNullOrEmpty
            $mandatory = $param.Attributes | Where-Object {
                $_.TypeName.Name -eq "Parameter" -and
                ($_.NamedArguments | Where-Object { $_.ArgumentName -eq "Mandatory" -and $_.Argument.Extent.Text -eq '$true' })
            }
            $mandatory | Should -Not -BeNullOrEmpty
        }

        It "Should have a mandatory ReleaseName parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "ReleaseName" }
            $param | Should -Not -BeNullOrEmpty
            $mandatory = $param.Attributes | Where-Object {
                $_.TypeName.Name -eq "Parameter" -and
                ($_.NamedArguments | Where-Object { $_.ArgumentName -eq "Mandatory" -and $_.Argument.Extent.Text -eq '$true' })
            }
            $mandatory | Should -Not -BeNullOrEmpty
        }

        It "Should have a mandatory Commit parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "Commit" }
            $param | Should -Not -BeNullOrEmpty
            $mandatory = $param.Attributes | Where-Object {
                $_.TypeName.Name -eq "Parameter" -and
                ($_.NamedArguments | Where-Object { $_.ArgumentName -eq "Mandatory" -and $_.Argument.Extent.Text -eq '$true' })
            }
            $mandatory | Should -Not -BeNullOrEmpty
        }

        It "Should have a mandatory DeployedBy parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "DeployedBy" }
            $param | Should -Not -BeNullOrEmpty
            $mandatory = $param.Attributes | Where-Object {
                $_.TypeName.Name -eq "Parameter" -and
                ($_.NamedArguments | Where-Object { $_.ArgumentName -eq "Mandatory" -and $_.Argument.Extent.Text -eq '$true' })
            }
            $mandatory | Should -Not -BeNullOrEmpty
        }

        It "Should have a mandatory RunUrl parameter" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "RunUrl" }
            $param | Should -Not -BeNullOrEmpty
            $mandatory = $param.Attributes | Where-Object {
                $_.TypeName.Name -eq "Parameter" -and
                ($_.NamedArguments | Where-Object { $_.ArgumentName -eq "Mandatory" -and $_.Argument.Extent.Text -eq '$true' })
            }
            $mandatory | Should -Not -BeNullOrEmpty
        }

        It "Should have an optional WorkspaceDirectory parameter with a default value" {
            $param = $paramBlock.Parameters | Where-Object { $_.Name.VariablePath.UserPath -eq "WorkspaceDirectory" }
            $param | Should -Not -BeNullOrEmpty
            $param.DefaultValue | Should -Not -BeNullOrEmpty
        }
    }

    Context "Implementation" {
        It "Should build the Azure management API URL with all required path components" {
            $content = Get-Content $scriptPath -Raw
            $content | Should -Match '\$SubscriptionId'
            $content | Should -Match '\$ResourceGroupName'
            $content | Should -Match '\$AppInsightsName'
            $content | Should -Match 'management\.azure\.com'
            $content | Should -Match 'Annotations'
        }

        It "Should include Category field in the annotation body" {
            # The 2015-05-01 API requires a non-null Category field
            $content = Get-Content $scriptPath -Raw
            $content | Should -Match 'Category'
        }

        It "Should handle az rest failure via exit code check" {
            $content = Get-Content $scriptPath -Raw
            $content | Should -Match '\$LASTEXITCODE'
        }
    }
}
