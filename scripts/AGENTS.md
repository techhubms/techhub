# PowerShell Development Agent

> **AI CONTEXT**: This is a **LEAF** context file for the `scripts/` directory. It complements the [Root AGENTS.md](../AGENTS.md).

## Critical PowerShell Rules

### ✅ Always Do

- **Always use backticks for escaping** (`` ` ``), NEVER backslashes
- **Always use subexpressions `$(...)` for complex interpolations** (dotted notation, array access, type casting)
- **Always test ALL script changes** — Run `Run -TestProject powershell`
- **Always set error handling** — `$ErrorActionPreference = "Stop"` + `Set-StrictMode -Version Latest`
- **Always handle two execution contexts** — Script directory vs workspace root
- **Always document parameters** with `[Parameter()]` attributes

### ⚠️ Ask First

- Adding new PowerShell modules

### 🚫 Never Do

- **Never use backslashes for escaping** — Use backticks
- **Never escape dollar signs** — Use subexpressions `$(...)`
- **Never use dotted notation without subexpression** — `"$object.property"` is WRONG, use `"$($object.property)"`
- **Never install dependencies in scripts** — Use `.devcontainer/post-create.sh`
- **Never add functions only for tests** — Test real implementation
- **Never leave scripts without error handling** — Always use try/catch
- **Never assume execution context** — Support both script dir and workspace root
- **Never skip testing after changes**

## PowerShell Syntax Rules

```powershell
# ✅ Correct
Write-Host "Value is $variable"
Write-Host "Value is `"$variable`""
$variable = "a $($object.with.dottednotation) value"
$variable = "a $($object['key']) value"

# ❌ Wrong
Write-Host "Value is `$variable"      # Shows literal $variable
$variable = "a $object.property value" # Only interpolates $object
```

## Script Standards

```powershell
param(
    [Parameter(Mandatory = $true)]
    [string]$RequiredParameter,

    [Parameter(Mandatory = $false)]
    [string]$WorkspaceDirectory = $PSScriptRoot
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest
```

**Error Handling**: Use try/catch with proper error reporting.

## Directory Structure

```text
scripts/
└── data/                    # rss-feeds.json (seed file for initial DB population)
```

> **Note**: All content processing (RSS feeds AND weekly roundup generation) is now handled
> entirely by the C# background services in `TechHub.Api` — no PowerShell scripts are involved
> in content or roundup processing.

## Key Scripts

### Infrastructure

- **Deploy-Infrastructure.ps1**: Phase 1 Bicep deployment (validate/whatif/deploy). Deploys networking, identity, Key Vault, PostgreSQL, ACS, monitoring. **No image tag needed.**
- **Deploy-Applications.ps1**: Phase 2 Bicep deployment (Container Apps declarative). Requires `-ImageTag`. Used when infra/scripts changed.
- **Build-Images.ps1**: Docker build + push to ghcr.io. Supports `-SkipPush` for local testing. Used by CI build-and-push jobs.
- **Deploy-Application.ps1**: `az containerapp update` with health checks, smoke tests, and auto-rollback. Requires `-Tag`. Used for **fast code-only deploys** (skips full Bicep evaluation).
- **Deploy-PrPreview.ps1**: PR preview environment lifecycle (deploy/teardown). Creates PITR clone of prod PostgreSQL in prod RG. Uses ghcr.io images + `pr-applications.bicep`.
- **Migrate-KeyVaultSecrets.ps1**: One-time migration script to copy secrets from `kv-techhub-shared` to `kv-techhub-prod`.

### CD Pipeline Deployment Strategy

The CD workflow detects whether `infra/` or `scripts/` changed since the last successful production deployment:

| What changed | Deployment path | Duration |
|---|---|---|
| Infrastructure/scripts | `Deploy-Infrastructure.ps1` → `Deploy-Applications.ps1` (full Bicep) | ~10 min |
| Code only (C#, tests, docs) | `Deploy-Application.ps1 -Tag <tag>` (fast image swap) | ~1 min |

## Testing

See [tests/powershell/AGENTS.md](../tests/powershell/AGENTS.md) for Pester v5 testing patterns.

```powershell
Run -TestProject powershell               # All PowerShell tests
Run -TestProject powershell -TestName "RSS" # Filtered by name
```
