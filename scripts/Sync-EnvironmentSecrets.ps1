#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Syncs environment-specific secrets from Azure to GitHub environment secrets.

.DESCRIPTION
    Reads secrets from Azure and sets them as GitHub environment secrets for
    staging and/or production. Handles:

    - AI Foundry keys:      Read from Azure Cognitive Services (oai-techhub-staging / oai-techhub-prod)
    - Postgres passwords:   Prompted interactively, then synced everywhere:
                            1. Resets the Azure PostgreSQL Flexible Server admin password
                            2. Stores the password in Azure Key Vault (kv-techhub-shared)
                            3. Sets the GitHub environment secret POSTGRES_ADMIN_PASSWORD
                            4. Updates the 'db-connection-string' secret on the API Container App
                            5. Forces a new revision so the running app picks up the new password

    Requires:
    - Azure CLI authenticated (`az login`)
    - GitHub CLI authenticated (`gh auth login`)
    - Read access to Azure Cognitive Services keys
    - Contributor access to the PostgreSQL Flexible Server and Container Apps
    - Key Vault Secrets Officer role on kv-techhub-shared
    - Admin access to the GitHub repository

.PARAMETER Environment
    Target environment(s). Defaults to both staging and production.

.PARAMETER SkipAiKey
    Skip syncing the AI Foundry key (useful if you only want to set the Postgres password).

.PARAMETER SkipPostgresPassword
    Skip the Postgres password sync (useful if you only want to sync the AI key).

.PARAMETER SkipServerPasswordReset
    Skip resetting the password on the Azure PostgreSQL Flexible Server itself.
    Use this when the server already has the correct password and you only need
    to propagate it to Key Vault, GitHub, and the Container Apps.

.PARAMETER KeyVaultName
    Azure Key Vault to store the Postgres password in. Defaults to 'kv-techhub-shared'.

.PARAMETER GitHubRepo
    GitHub repository in 'owner/repo' format. Defaults to the current repo.

.EXAMPLE
    ./scripts/Sync-EnvironmentSecrets.ps1
    Sync all secrets for both staging and production.

.EXAMPLE
    ./scripts/Sync-EnvironmentSecrets.ps1 -Environment staging
    Sync secrets for staging only.

.EXAMPLE
    ./scripts/Sync-EnvironmentSecrets.ps1 -SkipPostgresPassword
    Sync only AI Foundry keys (no password prompt).

.EXAMPLE
    ./scripts/Sync-EnvironmentSecrets.ps1 -SkipAiKey
    Reset and sync the Postgres password everywhere (no AI key sync).

.EXAMPLE
    ./scripts/Sync-EnvironmentSecrets.ps1 -SkipAiKey -SkipServerPasswordReset
    Sync a known Postgres password to Key Vault, GitHub, and Container Apps
    without touching the PostgreSQL server itself.
#>

param(
    [Parameter(Mandatory = $false)]
    [ValidateSet('staging', 'production')]
    [string[]]$Environment = @('staging', 'production'),

    [Parameter(Mandatory = $false)]
    [switch]$SkipAiKey,

    [Parameter(Mandatory = $false)]
    [switch]$SkipPostgresPassword,

    [Parameter(Mandatory = $false)]
    [switch]$SkipServerPasswordReset,

    [Parameter(Mandatory = $false)]
    [string]$KeyVaultName = 'kv-techhub-shared',

    [Parameter(Mandatory = $false)]
    [string]$GitHubRepo
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

function Write-Fail {
    param([string]$Message)
    Write-Host "   [FAIL] $Message" -ForegroundColor Red
}

function Write-Warn {
    param([string]$Message)
    Write-Host "   [WARN] $Message" -ForegroundColor Yellow
}

function Write-Detail {
    param([string]$Message)
    Write-Host "   $Message" -ForegroundColor Gray
}

# ============================================================================
# ENVIRONMENT CONFIG
# ============================================================================

$envConfig = @{
    staging    = @{
        ResourceGroup  = 'rg-techhub-staging'
        AiName         = 'oai-techhub-staging'
        EnvSuffix      = 'staging'
        PostgresServer = 'psql-techhub-staging'
        ApiAppName     = 'ca-techhub-api-staging'
    }
    production = @{
        ResourceGroup  = 'rg-techhub-prod'
        AiName         = 'oai-techhub-prod'
        EnvSuffix      = 'prod'
        PostgresServer = 'psql-techhub-prod'
        ApiAppName     = 'ca-techhub-api-prod'
    }
}

# ============================================================================
# BANNER
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  TechHub Environment Secret Sync" -ForegroundColor White
Write-Host "  Environments     : $($Environment -join ', ')" -ForegroundColor Gray
Write-Host "  AI Foundry key   : $(if ($SkipAiKey) { 'skip' } else { 'sync from Azure' })" -ForegroundColor Gray
Write-Host "  Postgres password: $(if ($SkipPostgresPassword) { 'skip' } else { 'prompt + sync everywhere' })" -ForegroundColor Gray
Write-Host "  Reset PG server  : $(if ($SkipPostgresPassword -or $SkipServerPasswordReset) { 'skip' } else { 'yes — will reset Azure PostgreSQL admin password' })" -ForegroundColor Gray
Write-Host "  Key Vault        : $KeyVaultName" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan

# ============================================================================
# PRE-FLIGHT CHECKS
# ============================================================================

Write-Step "Validating prerequisites"

# Azure CLI
$account = az account show -o json 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Azure CLI not authenticated. Run 'az login' first."
    exit 1
}
$tenantId = ($account | ConvertFrom-Json).tenantId
Write-Ok "Azure CLI authenticated (tenant: $tenantId)"

# GitHub CLI
$ghStatus = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Fail "GitHub CLI not authenticated. Run 'gh auth login' first."
    exit 1
}
Write-Ok "GitHub CLI authenticated"

# Resolve repo
if (-not $GitHubRepo) {
    $GitHubRepo = gh repo view --json nameWithOwner --jq '.nameWithOwner' 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Could not determine GitHub repository. Use -GitHubRepo 'owner/repo'."
        exit 1
    }
}
Write-Ok "Repository: $GitHubRepo"

# ============================================================================
# SYNC SECRETS PER ENVIRONMENT
# ============================================================================

$totalSet = 0

foreach ($env in $Environment) {
    $config = $envConfig[$env]

    Write-Host ""
    Write-Host "───────────────────────────────────────────────────────────────" -ForegroundColor DarkGray
    Write-Host "  Environment: $env" -ForegroundColor White
    Write-Host "───────────────────────────────────────────────────────────────" -ForegroundColor DarkGray

    # ── AI Foundry key ─────────────────────────────────────────────────────
    if (-not $SkipAiKey) {
        Write-Step "Reading AI Foundry key from '$($config.AiName)'"

        $key = az cognitiveservices account keys list `
            --name $config.AiName `
            --resource-group $config.ResourceGroup `
            --query key1 -o tsv 2>&1

        if ($LASTEXITCODE -ne 0) {
            Write-Fail "Could not read AI Foundry key from '$($config.AiName)'"
            Write-Detail "$key"
        }
        else {
            gh secret set AZURE_AI_KEY --env $env --body $key --repo $GitHubRepo
            if ($LASTEXITCODE -eq 0) {
                Write-Ok "AZURE_AI_KEY set for '$env'"
                $totalSet++
            }
            else {
                Write-Fail "Failed to set AZURE_AI_KEY for '$env'"
            }
        }
    }

    # ── Postgres admin password ────────────────────────────────────────────
    if (-not $SkipPostgresPassword) {
        Write-Step "Postgres admin password for '$env'"

        $pgServer = $config.PostgresServer
        $rgName   = $config.ResourceGroup
        $apiApp   = $config.ApiAppName
        $kvSecret = "postgres-admin-password-$($config.EnvSuffix)"

        $securePassword = Read-Host -Prompt "   Enter new Postgres admin password for $env" -AsSecureString
        $password = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(
            [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($securePassword)
        )

        if ([string]::IsNullOrWhiteSpace($password)) {
            Write-Fail "Empty password — skipping all Postgres steps for '$env'"
        }
        else {
            # 1. Reset the Azure PostgreSQL Flexible Server admin password
            if (-not $SkipServerPasswordReset) {
                Write-Detail "Resetting Azure PostgreSQL admin password on '$pgServer'..."
                $pgUpdateOutput = az postgres flexible-server update `
                    --name $pgServer `
                    --resource-group $rgName `
                    --admin-password $password 2>&1
                if ($LASTEXITCODE -eq 0) {
                    Write-Ok "Azure PostgreSQL admin password reset on '$pgServer'"
                }
                else {
                    Write-Fail "Failed to reset Azure PostgreSQL admin password on '$pgServer'"
                    Write-Detail "$pgUpdateOutput"
                    Write-Detail "Continuing with Key Vault / GitHub / Container App sync..."
                }
            }
            else {
                Write-Detail "Skipping server password reset (-SkipServerPasswordReset)"
            }

            # 2. Store in Azure Key Vault
            Write-Detail "Storing password in Key Vault '$KeyVaultName' as '$kvSecret'..."
            $kvOutput = az keyvault secret set `
                --vault-name $KeyVaultName `
                --name $kvSecret `
                --value $password `
                -o none 2>&1
            if ($LASTEXITCODE -eq 0) {
                Write-Ok "Password stored in Key Vault as '$kvSecret'"
                $totalSet++
            }
            else {
                Write-Warn "Could not store password in Key Vault '$KeyVaultName'."
                Write-Detail "$kvOutput"
                Write-Detail "Skipping Key Vault storage. GitHub secret and Container App will still be updated."
            }

            # 3. Set GitHub environment secret
            gh secret set POSTGRES_ADMIN_PASSWORD --env $env --body $password --repo $GitHubRepo
            if ($LASTEXITCODE -eq 0) {
                Write-Ok "POSTGRES_ADMIN_PASSWORD set in GitHub environment '$env'"
                $totalSet++
            }
            else {
                Write-Fail "Failed to set POSTGRES_ADMIN_PASSWORD in GitHub environment '$env'"
            }

            # 4. Update the Container App 'db-connection-string' secret and force a new revision
            Write-Detail "Retrieving PostgreSQL server FQDN for '$pgServer'..."
            $pgFqdn = az postgres flexible-server show `
                --name $pgServer `
                --resource-group $rgName `
                --query fullyQualifiedDomainName `
                -o tsv 2>&1

            if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($pgFqdn)) {
                $connStr = "Host=$pgFqdn;Database=techhub;Username=techhubadmin;Password=$password;SSL Mode=Require"

                Write-Detail "Updating 'db-connection-string' secret on Container App '$apiApp'..."
                az containerapp secret set `
                    --name $apiApp `
                    --resource-group $rgName `
                    --secrets "db-connection-string=$connStr" 2>&1 | Out-Null
                if ($LASTEXITCODE -eq 0) {
                    Write-Ok "Secret 'db-connection-string' updated on '$apiApp'"

                    # Force a new revision so the running app picks up the new secret value
                    $revSuffix = "pwupdate-$(Get-Date -Format 'yyyyMMddHHmmss')"
                    Write-Detail "Creating new revision on '$apiApp' (suffix: $revSuffix)..."
                    az containerapp update `
                        --name $apiApp `
                        --resource-group $rgName `
                        --revision-suffix $revSuffix 2>&1 | Out-Null
                    if ($LASTEXITCODE -eq 0) {
                        Write-Ok "New revision '$revSuffix' deployed — '$apiApp' now uses the new password"
                    }
                    else {
                        Write-Fail "Failed to create new revision for '$apiApp'"
                        Write-Detail "The secret was updated but the app needs a manual restart or redeploy to pick it up."
                    }
                }
                else {
                    Write-Fail "Failed to update 'db-connection-string' secret on '$apiApp'"
                    Write-Detail "The password was changed on the server. Redeploy the API to restore connectivity."
                }
            }
            else {
                Write-Fail "Could not retrieve FQDN for '$pgServer' — skipping Container App update"
                Write-Detail "Run 'az postgres flexible-server show --name $pgServer --resource-group $rgName' to troubleshoot."
            }
        }
    }
}

# ============================================================================
# SUMMARY
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  Done — $totalSet secret(s) updated" -ForegroundColor White
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""

if ($totalSet -gt 0) {
    Write-Host "  Next steps:" -ForegroundColor Gray
    Write-Host "    - Verify the running Container Apps are healthy (check /health and /alive)" -ForegroundColor DarkGray
    Write-Host "    - Future deployments will automatically use the updated GitHub secret" -ForegroundColor DarkGray
    Write-Host "    - The new password is also stored in Key Vault '$KeyVaultName' for reference" -ForegroundColor DarkGray
    Write-Host ""
}
