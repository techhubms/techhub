# PowerShell Test Suite

> **RULE**: Follow [scripts/AGENTS.md](../../scripts/AGENTS.md) for PowerShell patterns and [Root AGENTS.md](../../AGENTS.md) for workflow.

Pester tests for PowerShell scripts. Scripts are run end to end in a child scope with every external tool (`az`, `gh`, `docker`, `git`, `curl`, ...) and Az cmdlet mocked, so no real service is touched.

## What Is Covered

Every script in `scripts/` has a test file named after it (`TechHubRunner.psm1` is exercised through the `Run` workflow itself).

| Test file | Script |
|---|---|
| `Add-AppInsightsAnnotation.Tests.ps1` | `Add-AppInsightsAnnotation.ps1` |
| `Assign-AdminUsers.Tests.ps1` | `Assign-AdminUsers.ps1` |
| `Build-Images.Tests.ps1` | `Build-Images.ps1` |
| `Cleanup-ContainerRegistry.Tests.ps1` | `Cleanup-ContainerRegistry.ps1` |
| `Deploy-Application.Tests.ps1` | `Deploy-Application.ps1` |
| `Deploy-Applications.Tests.ps1` | `Deploy-Applications.ps1` |
| `Deploy-Infrastructure.Tests.ps1` | `Deploy-Infrastructure.ps1` |
| `Deploy-PrPreview.Tests.ps1` | `Deploy-PrPreview.ps1` |
| `Fetch-VideoTranscripts.Tests.ps1` | `Fetch-VideoTranscripts.ps1` |
| `Generate-DevCertificate.Tests.ps1` | `Generate-DevCertificate.ps1` |
| `Invoke-Sql.Tests.ps1` | `Invoke-Sql.ps1` |
| `Manage-EntraId.Tests.ps1` | `Manage-EntraId.ps1` |
| `Migrate-KeyVaultSecrets.Tests.ps1` | `Migrate-KeyVaultSecrets.ps1` |
| `Normalize-Images.Tests.ps1` | `Normalize-Images.ps1` |
| `Renew-WildcardCertificates.Tests.ps1` | `Renew-WildcardCertificates.ps1` |
| `Restore-Database.Tests.ps1` | `Restore-Database.ps1` |
| `Setup-OidcAuthentication.Tests.ps1` | `Setup-OidcAuthentication.ps1` |
| `Setup-UserSecrets.Tests.ps1` | `Setup-UserSecrets.ps1` |
| `Sync-KeyVaultSecrets.Tests.ps1` | `Sync-KeyVaultSecrets.ps1` |
| `Teardown-PrEnvironments.Tests.ps1` | `Teardown-PrEnvironments.ps1` |
| `Wait-ForLiveVersion.Tests.ps1` | `Wait-ForLiveVersion.ps1` |

> **Note**: PowerShell content-processing and roundup-generation tests have been removed.
> All content processing and roundup generation is now handled by C# background services in `TechHub.Api`.

## Structure

- Test files: `*.Tests.ps1` (named to match the script being tested)
- `TestHelpers.ps1` — Shared helpers, dot-sourced in `BeforeAll`: stubs for external commands and Az cmdlets, a recording CLI fake (`New-CliFake`, `Add-CliRule`, `Invoke-CliFake`, `Get-CliCalls`), a script sandbox (`New-ScriptSandbox`, stub sibling scripts) and `Invoke-ScriptUnderTest` (captures exit code, all output streams and the terminating error)
- `Initialize-BeforeAll.ps1` — Minimal setup (disables progress bars)
- `Initialize-BeforeEach.ps1` — Mocks HTTP calls (`Invoke-WebRequest`, `Invoke-RestMethod`)

## Test Pattern

```powershell
Describe "My-Script" {
    BeforeAll {
        . $PSScriptRoot/TestHelpers.ps1
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/My-Script.ps1"
    }

    BeforeEach {
        $cli = New-CliFake
        Mock az { Invoke-CliFake -Fake $cli -Command 'az' -Arguments $args }
    }

    It "Exits 1 when az is not logged in" {
        Add-CliRule $cli -Command 'az' -Match '^az account show' -ExitCode 1

        $result = Invoke-ScriptUnderTest $scriptPath @{ Name = 'x' }

        $result.ExitCode | Should -Be 1
        $result.Output | Should -Match 'not logged in'
    }
}
```

## Key Rules

- Test real implementation — never duplicate production logic
- Mock external dependencies (`az`, `gh`, `docker`, `Invoke-RestMethod`, ...) with Pester `Mock`; never call a real service
- No skipped or pending tests — if a test exposes a script bug, fix the script (minimally) and keep the test
- Wrap helper results in `@(...)` before `Should -HaveCount`/indexing (a single result is not an array)
- Share state between a test and its mocks through a state object (`$state = [pscustomobject]@{ ... }`), not `$script:` variables
- When mocking `Get-Command`, call the real cmdlet for everything you do not override
- Use `$PSScriptRoot` for paths, never hardcode
- Run a single file with `Invoke-Pester -Path tests/powershell/<File>.Tests.ps1 -Output Detailed`; the whole suite runs via `Run -TestProject powershell`
