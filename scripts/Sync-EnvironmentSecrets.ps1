#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Syncs environment-specific secrets from Azure to GitHub environment secrets.

.DESCRIPTION
    Reads secrets from Azure and sets them as GitHub environment secrets for
    staging and/or production. Container Apps reference secrets via Key Vault
    URL references, so this script writes new values to Key Vault directly;
    no redeploy is needed — restarting a revision is sufficient.

    - AI Foundry keys:  Read from Azure Cognitive Services, then:
                        1. Written to Key Vault as 'techhub-<env>-ai-api-key'
                        (No GitHub secret needed — Deploy-Infrastructure.ps1 reads the key
                        from Azure directly at deploy time.)
    - Postgres password: Prompted interactively, then synced everywhere:
                        1. Resets the Azure PostgreSQL Flexible Server admin password (optional)
                        2. Writes the full connection string to Key Vault as
                           'techhub-<env>-db-connection-string'
                        3. Sets the GitHub environment secret POSTGRES_ADMIN_PASSWORD
                        4. Restarts the API Container App revision so it re-reads the KV secret

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
    Sync a known Postgres password to Key Vault, GitHub, and restart Container App
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

# Write a secret to Key Vault using a temp file so the value never appears in
# process arguments (avoids exposure via /proc/<pid>/cmdline and shell history).
function Set-KvSecretFromValue {
    param(
        [Parameter(Mandatory = $true)][string]$VaultName,
        [Parameter(Mandatory = $true)][string]$SecretName,
        [Parameter(Mandatory = $true)][string]$Value
    )
    $tmpFile = [System.IO.Path]::GetTempFileName()
    try {
        [System.IO.File]::WriteAllText($tmpFile, $Value)
        az keyvault secret set `
            --vault-name $VaultName `
            --name $SecretName `
            --file $tmpFile `
            --output none
        return $LASTEXITCODE -eq 0
    }
    finally {
        Remove-Item $tmpFile -Force -ErrorAction SilentlyContinue
    }
}

# ============================================================================
# ENVIRONMENT CONFIG
# ============================================================================

$envConfig = @{
    staging    = @{
        ResourceGroup  = 'rg-techhub-staging'
        AiName         = 'oai-techhub-staging'
        EnvSuffix      = 'staging'
        # PostgreSQL and Container Apps are no longer permanent staging resources.
        # psql-techhub-staging and ca-techhub-api-staging were deleted as part of the
        # migration to ephemeral PR environments. Postgres password sync is skipped for staging.
        PostgresServer = $null
        ApiAppName     = $null
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
gh auth status 2>&1 | Out-Null
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
            # Write to Key Vault so the Container App picks it up on next revision start
            $kvAiKey = "techhub-$($config.EnvSuffix)-ai-api-key"
            Write-Detail "Writing '$kvAiKey' to Key Vault '$KeyVaultName'..."
            $kvOk = Set-KvSecretFromValue -VaultName $KeyVaultName -SecretName $kvAiKey -Value $key
            if ($kvOk) {
                Write-Ok "Key Vault secret '$kvAiKey' updated"
                $totalSet++
            }
            else {
                Write-Warn "Could not write '$kvAiKey' to Key Vault '$KeyVaultName'."
            }
        }
    }

    # ── Postgres admin password ────────────────────────────────────────────
    if (-not $SkipPostgresPassword) {
        Write-Step "Postgres admin password for '$env'"

        $pgServer = $config.PostgresServer
        $rgName   = $config.ResourceGroup
        $apiApp   = $config.ApiAppName

        if (-not $pgServer) {
            Write-Ok "Postgres password sync skipped for '$env' — no permanent PostgreSQL server (PR environments use ephemeral databases)"
        }
        else {

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
                    Write-Detail "Continuing with Key Vault / GitHub sync..."
                }
            }
            else {
                Write-Detail "Skipping server password reset (-SkipServerPasswordReset)"
            }

            # Fetch the server FQDN to build the connection string
            Write-Detail "Retrieving PostgreSQL server FQDN for '$pgServer'..."
            $pgFqdn = az postgres flexible-server show `
                --name $pgServer `
                --resource-group $rgName `
                --query fullyQualifiedDomainName `
                -o tsv 2>&1

            if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($pgFqdn)) {
                Write-Fail "Could not retrieve FQDN for '$pgServer'"
                Write-Detail "Run 'az postgres flexible-server show --name $pgServer --resource-group $rgName' to troubleshoot."
                Write-Detail "Continuing with GitHub secret sync. Key Vault and Container App will not be updated."
            }
            else {
                $connStr = "Host=$pgFqdn;Database=techhub;Username=techhubadmin;Password=$password;SSL Mode=Require"

                # 2. Write the full connection string to Key Vault.
                # The API Container App references this secret via keyVaultUrl — no redeploy needed,
                # only a revision restart to pick up the new value.
                $kvConnStr = "techhub-$($config.EnvSuffix)-db-connection-string"
                Write-Detail "Writing '$kvConnStr' to Key Vault '$KeyVaultName'..."
                $kvOk = Set-KvSecretFromValue -VaultName $KeyVaultName -SecretName $kvConnStr -Value $connStr
                if ($kvOk) {
                    Write-Ok "Key Vault secret '$kvConnStr' updated"
                    $totalSet++

                    # 3. Restart the Container App revision so it re-reads the new KV secret value.
                    # We create a new revision (not just restart) because Container Apps resolves
                    # KV references at revision start, not dynamically.
                    $revSuffix = "pwupdate-$(Get-Date -Format 'yyyyMMddHHmmss')"
                    Write-Detail "Creating new revision on '$apiApp' (suffix: $revSuffix)..."
                    az containerapp update `
                        --name $apiApp `
                        --resource-group $rgName `
                        --revision-suffix $revSuffix 2>&1 | Out-Null
                    if ($LASTEXITCODE -eq 0) {
                        Write-Ok "New revision '$revSuffix' started — '$apiApp' will use the new connection string"
                    }
                    else {
                        Write-Warn "Could not create new revision for '$apiApp'."
                        Write-Detail "The KV secret was updated. Trigger a redeploy or create a revision manually to apply it."
                    }
                }
                else {
                    Write-Warn "Could not write '$kvConnStr' to Key Vault '$KeyVaultName'."
                    Write-Detail "GitHub secret will still be updated. The new value will reach Key Vault on the next deploy."
                }
            }

            # Always update the GitHub secret so the next automated deploy uses the new password
            gh secret set POSTGRES_ADMIN_PASSWORD --env $env --body $password --repo $GitHubRepo
            if ($LASTEXITCODE -eq 0) {
                Write-Ok "POSTGRES_ADMIN_PASSWORD set in GitHub environment '$env'"
                $totalSet++
            }
            else {
                Write-Fail "Failed to set POSTGRES_ADMIN_PASSWORD in GitHub environment '$env'"
            }
        }
        } # end else ($pgServer check)
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
