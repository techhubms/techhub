Describe "Generate-DevCertificate" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/Generate-DevCertificate.ps1"
        $realTestPath = Get-Command -Name Test-Path -CommandType Cmdlet
        $expectedDir = Join-Path $HOME ".aspnet" "https"
        $expectedCert = Join-Path $expectedDir "aspnetapp.pfx"
    }

    BeforeEach {
        $cli = New-CliFake
        $state = [pscustomobject]@{
            DirExists  = $true
            CertExists = $false
            Answer     = 'n'
            Prompts    = [System.Collections.Generic.List[string]]::new()
            Created    = [System.Collections.Generic.List[string]]::new()
            Removed    = [System.Collections.Generic.List[string]]::new()
        }
        # Nothing touches the real ~/.aspnet directory: file-system cmdlets are mocked for those paths.
        Mock Test-Path {
            if ("$Path" -eq $expectedCert) { return $state.CertExists }
            if ("$Path" -eq $expectedDir) { return $state.DirExists }
            & $realTestPath @PesterBoundParameters
        }
        Mock New-Item { $state.Created.Add("$Path") }
        Mock Remove-Item { $state.Removed.Add("$Path") }
        Mock Read-Host { $state.Prompts.Add([string]$Prompt); $state.Answer }
        Mock dotnet { Invoke-CliFake -Fake $cli -Command 'dotnet' -Arguments $args }
    }

    Context "Fresh machine" {
        It "Generates and trusts the certificate in ~/.aspnet/https without prompting" {
            $result = Invoke-ScriptUnderTest $scriptPath

            $result.Error | Should -BeNullOrEmpty
            @(Get-CliCalls $cli '^dotnet') | Should -Be @("dotnet dev-certs https -ep $expectedCert -p devpass --trust")
            Should -Invoke Read-Host -Times 0 -Exactly
            $result.Output | Should -Match 'Certificate generated and trusted'
        }

        It "Creates the target directory when it does not exist" {
            $state.DirExists = $false

            Invoke-ScriptUnderTest $scriptPath | Out-Null

            $state.Created | Should -Contain $expectedDir
        }

        It "Does not recreate an existing directory" {
            Invoke-ScriptUnderTest $scriptPath | Out-Null

            $state.Created | Should -BeNullOrEmpty
        }

        It "Exits 1 when dotnet dev-certs fails" {
            Add-CliRule $cli 'dotnet' '^dotnet dev-certs' '' -ExitCode 1

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 1
            $result.Output | Should -Match 'Failed to generate certificate'
            $result.Output | Should -Not -Match 'Certificate generated and trusted'
        }
    }

    Context "Existing certificate" {
        BeforeEach { $state.CertExists = $true }

        It "Keeps the existing certificate unless the user answers y" {
            $state.Answer = 'n'

            $result = Invoke-ScriptUnderTest $scriptPath

            $result.ExitCode | Should -Be 0
            $state.Prompts | Should -HaveCount 1
            $state.Prompts[0] | Should -Match 'Regenerate\? \(y/N\)'
            @(Get-CliCalls $cli '^dotnet') | Should -BeNullOrEmpty
            $state.Removed | Should -BeNullOrEmpty
            $result.Output | Should -Match 'Using existing certificate'
        }

        It "Treats an empty answer as no (default N)" {
            $state.Answer = ''

            Invoke-ScriptUnderTest $scriptPath | Out-Null

            @(Get-CliCalls $cli '^dotnet') | Should -BeNullOrEmpty
        }

        It "Removes the old certificate and regenerates it when the user answers y or Y" -ForEach @(@{ Answer = 'y' }, @{ Answer = 'Y' }) {
            $state.Answer = $Answer

            $result = Invoke-ScriptUnderTest $scriptPath

            $state.Removed | Should -Contain $expectedCert
            @(Get-CliCalls $cli '^dotnet dev-certs https -ep') | Should -HaveCount 1
            $result.Output | Should -Match 'Certificate generated and trusted'
        }
    }
}
