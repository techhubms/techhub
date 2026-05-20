#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Copies secrets from the shared Key Vault to the production Key Vault.

.DESCRIPTION
    One-time migration script for transitioning from the shared resource group architecture
    (kv-techhub-shared) to the single production resource group architecture (kv-techhub-prod).

    Secrets migrated:
        techhub-prod-aad-client-secret        → techhub-prod-aad-client-secret (same name)

    Additionally copies the wildcard certificate secrets:
        wildcard-hub-ms    → wildcard-hub-ms
        wildcard-xebia-ms  → wildcard-xebia-ms

    Secrets no longer migrated (replaced by managed identity / RBAC):
        techhub-prod-db-connection-string   — removed; app uses Entra token auth
        techhub-prod-ai-api-key             — removed; app uses Cognitive Services OpenAI User RBAC role

    Run this script ONCE after creating kv-techhub-prod and BEFORE deploying Container Apps
    that reference the production Key Vault. See the deployment order in the cost-reduction
    implementation plan.

    Requirements:
        - The caller must have Key Vault Secrets User on kv-techhub-shared (to read)
        - The caller must have Key Vault Secrets Officer on kv-techhub-prod (to write)
        - Both Key Vaults must be accessible (caller IP must be in both KV firewall rules)

.PARAMETER SourceKeyVaultName
    Source Key Vault name. Defaults to 'kv-techhub-shared'.

.PARAMETER TargetKeyVaultName
    Target Key Vault name. Defaults to 'kv-techhub-prod'.

.EXAMPLE
    ./scripts/Migrate-KeyVaultSecrets.ps1
    Copy secrets from kv-techhub-shared to kv-techhub-prod.

.EXAMPLE
    ./scripts/Migrate-KeyVaultSecrets.ps1 -SourceKeyVaultName my-old-kv -TargetKeyVaultName my-new-kv
    Copy secrets between custom vault names.
#>

param(
    [Parameter(Mandatory = $false)]
    [string]$SourceKeyVaultName = 'kv-techhub-shared',

    [Parameter(Mandatory = $false)]
    [string]$TargetKeyVaultName = 'kv-techhub-prod'
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# ============================================================================
# HELPERS
# ============================================================================

function Write-Step {
    param([string]$Message)
    Write-Host ""
    Write-Host "=> $Message" -ForegroundColor Cyan
}

function Write-Ok {
    param([string]$Message)
    Write-Host "   [OK] $Message" -ForegroundColor Green
}

function Write-Warn {
    param([string]$Message)
    Write-Host "   [WARN] $Message" -ForegroundColor Yellow
}

function Write-Fail {
    param([string]$Message)
    Write-Host "   [FAIL] $Message" -ForegroundColor Red
}

function Write-Detail {
    param([string]$Message)
    Write-Host "   $Message" -ForegroundColor Gray
}

function Copy-KvSecret {
    <#
    .SYNOPSIS
        Copies a single secret from source Key Vault to target Key Vault.
        Skips if the source secret does not exist.
        Overwrites the target secret if it already exists.
    #>
    param(
        [string]$SourceName,
        [string]$TargetName
    )

    if (-not $TargetName) { $TargetName = $SourceName }

    # Read from source vault
    $value = az keyvault secret show `
        --vault-name $SourceKeyVaultName `
        --name $SourceName `
        --query value -o tsv 2>$null

    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($value)) {
        Write-Warn "$SourceName not found in '$SourceKeyVaultName' — skipping"
        return
    }

    # Write to target vault via temp file (avoids secret in process args / shell history)
    $tmpFile = [System.IO.Path]::GetTempFileName()
    try {
        [System.IO.File]::WriteAllText($tmpFile, $value)
        az keyvault secret set `
            --vault-name $TargetKeyVaultName `
            --name $TargetName `
            --file $tmpFile `
            --output none
    }
    finally {
        Remove-Item $tmpFile -Force -ErrorAction SilentlyContinue
    }

    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to write '$TargetName' to '$TargetKeyVaultName'"
        exit 1
    }

    Write-Ok "Copied: $SourceName → $TargetName"
}

# ============================================================================
# BANNER
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  TechHub Key Vault Secret Migration" -ForegroundColor White
Write-Host "  Source : $SourceKeyVaultName" -ForegroundColor Gray
Write-Host "  Target : $TargetKeyVaultName" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan

# ============================================================================
# PREREQUISITES
# ============================================================================

Write-Step "Validating prerequisites"

$account = az account show -o json 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Not logged in to Azure CLI. Run 'az login' first."
    exit 1
}
$accountInfo = $account | ConvertFrom-Json
Write-Ok "Azure CLI authenticated (subscription: $($accountInfo.name))"

# Verify source vault is accessible
az keyvault secret list --vault-name $SourceKeyVaultName --query '[]' --output none 2>&1 | Out-Null
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Cannot access source Key Vault '$SourceKeyVaultName'. Check firewall rules and RBAC."
    exit 1
}
Write-Ok "Source Key Vault accessible: $SourceKeyVaultName"

# Verify target vault is accessible
az keyvault secret list --vault-name $TargetKeyVaultName --query '[]' --output none 2>&1 | Out-Null
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Cannot access target Key Vault '$TargetKeyVaultName'. Check firewall rules and RBAC."
    exit 1
}
Write-Ok "Target Key Vault accessible: $TargetKeyVaultName"

# ============================================================================
# MIGRATE APPLICATION SECRETS
# ============================================================================

Write-Step "Migrating application secrets"

# Production secrets (already have correct names in old shared KV)
Copy-KvSecret -SourceName 'techhub-prod-aad-client-secret' -TargetName 'techhub-prod-aad-client-secret'

# ============================================================================
# MIGRATE WILDCARD CERTIFICATES
# ============================================================================

Write-Step "Migrating wildcard certificate secrets"

Copy-KvSecret -SourceName 'wildcard-hub-ms' -TargetName 'wildcard-hub-ms'
Copy-KvSecret -SourceName 'wildcard-xebia-ms' -TargetName 'wildcard-xebia-ms'

# ============================================================================
# SUMMARY
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  Migration Complete" -ForegroundColor Green
Write-Host "  Source : $SourceKeyVaultName" -ForegroundColor Gray
Write-Host "  Target : $TargetKeyVaultName" -ForegroundColor Gray
Write-Host ""
Write-Host "  Next steps:" -ForegroundColor Gray
Write-Host "  1. Store the GitHub PAT in the target Key Vault:" -ForegroundColor Gray
Write-Host "     az keyvault secret set --vault-name $($TargetKeyVaultName) ``" -ForegroundColor Gray
Write-Host "       --name techhub-github-registry-token --value <PAT>" -ForegroundColor Gray
Write-Host "  2. Deploy production infrastructure: ./scripts/Deploy-Infrastructure.ps1 -Mode deploy" -ForegroundColor Gray
Write-Host "  3. Verify production health: https://tech.hub.ms/" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""
