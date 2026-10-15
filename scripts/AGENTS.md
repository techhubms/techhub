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

## Scope

`scripts/` is a flat directory of PowerShell scripts plus the `TechHubRunner.psm1` module. Tests for them live in `tests/powershell/`.

> **Note**: All content processing (RSS feeds AND weekly roundup generation) is handled
> entirely by the C# background services in `TechHub.Api` — no PowerShell scripts are involved
> in content or roundup processing.

## Key Scripts

See [docs/repository-structure.md](../docs/repository-structure.md) for the complete list. The groups below describe how they relate.

### Development

- **TechHubRunner.psm1**: Exports `Run`, `Stop-Servers`, `Stop-OrphanedTestProcesses`, and `Start-McpInspector`. Auto-imported in every session (see [docs/running-and-testing.md](../docs/running-and-testing.md)).
- **Restore-Database.ps1**: Dumps the production database and restores it locally (`-Target local`) or only dumps to `.tmp/` (`-Target disk`).
- **Invoke-Sql.ps1**: Runs one SQL query against the local PostgreSQL container. Extend it for specific local-query use cases.
- **Setup-UserSecrets.ps1**: Fills .NET user secrets for local development (localhost Entra ID app, AI, and newsletter settings) from Azure.
- **Generate-DevCertificate.ps1**, **Normalize-Images.ps1**: HTTPS dev certificate and image normalization helpers.
- **Fetch-VideoTranscripts.ps1**: Backfills video transcripts with yt-dlp through the admin API. Defaults to the local API (`https://localhost:5001`).

### Infrastructure and deployment

- **Deploy-Infrastructure.ps1**: Phase 1 Bicep deployment (validate/whatif/deploy). Deploys networking, identity, Key Vault, PostgreSQL, ACS, monitoring. `-LinkEmailDomain` links the verified ACS email domain. **No image tag needed.**
- **Deploy-Applications.ps1**: Phase 2 Bicep deployment (API + Web App Service sites, declarative). Requires `-ImageTag`. Used when infra/scripts changed.
- **Build-Images.ps1**: Docker build + push to ghcr.io. Supports `-SkipPush` for local testing. Used by CI build-and-push jobs.
- **Deploy-Application.ps1**: `az webapp config set` with health checks, smoke tests, and auto-rollback. Requires `-Tag`. Used for **fast code-only deploys** (skips full Bicep evaluation).
- **Wait-ForLiveVersion.ps1**: Post-deploy gate. Polls the Web `/version` endpoint for the expected tag, then runs smoke tests. Called by `Deploy-Application.ps1` and `Deploy-PrPreview.ps1`.
- **Add-AppInsightsAnnotation.ps1**: Creates an Application Insights release annotation. Called by `cd.yml` after a production deploy.
- **Deploy-PrPreview.ps1**: PR preview environment lifecycle (deploy/teardown). Creates PITR clone of prod PostgreSQL in prod RG. Uses ghcr.io images + `pr-applications.bicep` (shared PR App Service Plan).
- **Teardown-PrEnvironments.ps1**: Finds all active PR previews (sites and PostgreSQL servers) and tears them down via `Deploy-PrPreview.ps1`. Supports `-DryRun`. Used by the nightly workflow.

### Identity, secrets, and certificates

- **Manage-EntraId.ps1**: Creates or rotates the Entra ID app registration (localhost, staging, production). See [docs/admin-authentication.md](../docs/admin-authentication.md).
- **Assign-AdminUsers.ps1**: Idempotently assigns the admin users to the Entra ID enterprise app. The user list is in the script.
- **Setup-OidcAuthentication.ps1**: Configures GitHub Actions OIDC federated credentials and the repository variables.
- **Sync-KeyVaultSecrets.ps1**: Writes application secrets from environment variables to Key Vault. Called by `Deploy-Infrastructure.ps1`.
- **Renew-WildcardCertificates.ps1**: Renews the Let's Encrypt wildcard certificates and redeploys `wildcardCert.bicep`. See [docs/wildcard-certificates.md](../docs/wildcard-certificates.md).

### Legacy (one-off)

- **Migrate-KeyVaultSecrets.ps1**: One-time copy of secrets from `kv-techhub-shared` to `kv-techhub-prod`. Only relevant to the old shared-resource-group setup.

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
