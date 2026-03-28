#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Syncs environment-specific secrets from Azure to GitHub environment secrets.

.DESCRIPTION
    Reads secrets from Azure and sets them as GitHub environment secrets for
    staging and/or production. Handles:

    - AI Foundry keys:      Read from Azure Cognitive Services (oai-techhub-staging / oai-techhub-prod)
    - Postgres passwords:   Prompted interactively (cannot be read from Azure)

    Requires:
    - Azure CLI authenticated (`az login`)
    - GitHub CLI authenticated (`gh auth login`)
    - Read access to Azure Cognitive Services keys
    - Admin access to the GitHub repository

.PARAMETER Environment
    Target environment(s). Defaults to both staging and production.

.PARAMETER SkipAiKey
    Skip syncing the AI Foundry key (useful if you only want to set the Postgres password).

.PARAMETER SkipPostgresPassword
    Skip setting the Postgres admin password (useful if you only want to sync the AI key).

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
    Set only Postgres passwords (prompted interactively).
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

function Write-Detail {
    param([string]$Message)
    Write-Host "   $Message" -ForegroundColor Gray
}

# ============================================================================
# ENVIRONMENT CONFIG
# ============================================================================

$envConfig = @{
    staging    = @{
        ResourceGroup = 'rg-techhub-staging'
        AiName        = 'oai-techhub-staging'
    }
    production = @{
        ResourceGroup = 'rg-techhub-prod'
        AiName        = 'oai-techhub-prod'
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
Write-Host "  Postgres password: $(if ($SkipPostgresPassword) { 'skip' } else { 'prompt' })" -ForegroundColor Gray
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
        Write-Detail "This password cannot be read from Azure — enter it manually."

        $securePassword = Read-Host -Prompt "   Enter Postgres admin password for $env" -AsSecureString
        $password = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(
            [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($securePassword)
        )

        if ([string]::IsNullOrWhiteSpace($password)) {
            Write-Fail "Empty password — skipping POSTGRES_ADMIN_PASSWORD for '$env'"
        }
        else {
            gh secret set POSTGRES_ADMIN_PASSWORD --env $env --body $password --repo $GitHubRepo
            if ($LASTEXITCODE -eq 0) {
                Write-Ok "POSTGRES_ADMIN_PASSWORD set for '$env'"
                $totalSet++
            }
            else {
                Write-Fail "Failed to set POSTGRES_ADMIN_PASSWORD for '$env'"
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
    Write-Host "  Once verified, you can remove these old repository secrets:" -ForegroundColor Gray
    Write-Host "    - POSTGRES_ACC_PW" -ForegroundColor DarkGray
    Write-Host "    - POSTGRES_PROD_PW" -ForegroundColor DarkGray
    Write-Host "    - AZURE_AI_KEY (repository-level)" -ForegroundColor DarkGray
    Write-Host ""
}
