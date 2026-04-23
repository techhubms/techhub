# PowerShell Test Suite

> **RULE**: Follow [scripts/AGENTS.md](../../scripts/AGENTS.md) for PowerShell patterns and [Root AGENTS.md](../../AGENTS.md) for workflow.

Pester v5 tests for PowerShell scripts.

## What Is Covered

- **`Manage-EntraId.Tests.ps1`** — Structural and parameter validation for `Manage-EntraId.ps1`

> **Note**: PowerShell content-processing and roundup-generation tests have been removed.
> All content processing and roundup generation is now handled by C# background services in `TechHub.Api`.

## Structure

- Test files: `*.Tests.ps1` (named to match the script being tested)
- `Initialize-BeforeAll.ps1` — Minimal setup (disables progress bars)
- `Initialize-BeforeEach.ps1` — Mocks HTTP calls (`Invoke-WebRequest`, `Invoke-RestMethod`)

## Test Pattern

```powershell
Describe "My-Script" {
    BeforeAll {
        $scriptPath = Join-Path $PSScriptRoot "../../scripts/My-Script.ps1"
    }

    Context "Script structure" {
        It "Should exist" {
            Test-Path $scriptPath | Should -BeTrue
        }
    }
}
```

## Key Rules

- Test real implementation — never duplicate production logic
- Mock external dependencies (`Invoke-WebRequest`, etc.) with Pester `Mock`
- Use `Should -Throw` for error handling tests
- Use `$PSScriptRoot` for paths, never hardcode
