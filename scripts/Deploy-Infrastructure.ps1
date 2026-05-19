#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Deploys TechHub Azure infrastructure for production.

.DESCRIPTION
    Deploys infrastructure using Bicep templates. Supports validate, what-if, and deploy modes.
    Includes pre-flight checks for known issues (e.g., soft-deleted AI resources) and
    post-deployment steps (secret syncing).

    Can be run locally or from GitHub Actions workflows.

.PARAMETER Mode
    Deployment mode: validate (syntax check), whatif (preview changes), or deploy (apply changes).
    Defaults to 'whatif' for safety.

.PARAMETER Location
    Azure region for deployment metadata. Defaults to 'swedencentral'.

.PARAMETER ImageTag
    Docker image tag to deploy. Required in deploy mode.

.EXAMPLE
    ./scripts/Deploy-Infrastructure.ps1 -Mode whatif
    Preview what changes would be made to production infrastructure.

.EXAMPLE
    ./scripts/Deploy-Infrastructure.ps1 -Mode deploy -ImageTag "20260501120000"
    Deploy production infrastructure with the given image tag.

.EXAMPLE
    ./scripts/Deploy-Infrastructure.ps1 -Mode validate
    Validate the production Bicep template without making any changes.
#>

param(
    [Parameter(Mandatory = $false)]
    [ValidateSet('validate', 'whatif', 'deploy')]
    [string]$Mode = 'whatif',

    [Parameter(Mandatory = $false)]
    [string]$Location = 'swedencentral',

    [Parameter(Mandatory = $false)]
    [string]$ImageTag
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# ============================================================================
# CONFIGURATION
# ============================================================================

# Resolve workspace root (support running from scripts/ or repo root)
$workspaceRoot = if (Test-Path (Join-Path $PSScriptRoot "../infra")) {
    (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
} else {
    $PSScriptRoot
}

$templateFile = Join-Path $workspaceRoot "infra/main.bicep"
$paramsFile   = Join-Path $workspaceRoot "infra/parameters/prod.bicepparam"
$resourceGroup = "rg-techhub-prod"

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

# ============================================================================
# BANNER
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  TechHub Infrastructure Deployment" -ForegroundColor White
Write-Host "  Environment : production" -ForegroundColor Gray
Write-Host "  Mode        : $Mode" -ForegroundColor Gray
Write-Host "  Location    : $Location" -ForegroundColor Gray
Write-Host "  Template    : infra/main.bicep" -ForegroundColor Gray
Write-Host "  Parameters  : infra/parameters/prod.bicepparam" -ForegroundColor Gray
if ($ImageTag) {
    Write-Host "  Image Tag   : $ImageTag" -ForegroundColor Gray
}
Write-Host "===============================================================" -ForegroundColor DarkCyan

# ============================================================================
# PRE-FLIGHT CHECKS
# ============================================================================

Write-Step "Validating prerequisites"

# Check Azure CLI login
$account = az account show -o json 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Not logged in to Azure CLI. Run 'az login' first."
    exit 1
}
$accountInfo = $account | ConvertFrom-Json
Write-Ok "Azure CLI authenticated (subscription: $($accountInfo.name))"

# Check template files exist
if (-not (Test-Path $templateFile)) {
    Write-Fail "Template file not found: $templateFile"
    exit 1
}
if (-not (Test-Path $paramsFile)) {
    Write-Fail "Parameters file not found: $paramsFile"
    exit 1
}
Write-Ok "Template files found"

# ADMIN_IP_ADDRESSES — required for PostgreSQL and Key Vault firewall rules.
if (-not $env:ADMIN_IP_ADDRESSES) {
    if ($Mode -eq 'deploy') {
        Write-Fail "Environment variable ADMIN_IP_ADDRESSES is not set."
        Write-Detail "Set it with: `$env:ADMIN_IP_ADDRESSES = '<ip1>,<ip2>'"
        exit 1
    }
    else {
        # For validate/whatif, use a non-routable placeholder — Bicep only checks the type
        $env:ADMIN_IP_ADDRESSES = "0.0.0.0"
        Write-Warn "ADMIN_IP_ADDRESSES not set — using placeholder (acceptable for $Mode mode)"
    }
}
else {
    Write-Ok "ADMIN_IP_ADDRESSES is set"
}

# POSTGRES_ADMIN_PASSWORD — required for production
if (-not $env:POSTGRES_ADMIN_PASSWORD) {
    if ($Mode -eq 'deploy') {
        Write-Fail "Environment variable POSTGRES_ADMIN_PASSWORD is not set."
        Write-Detail "Set it with: `$env:POSTGRES_ADMIN_PASSWORD = '<password>'"
        exit 1
    }
    else {
        $env:POSTGRES_ADMIN_PASSWORD = "placeholder-for-$Mode"
        Write-Warn "POSTGRES_ADMIN_PASSWORD not set — using placeholder (acceptable for $Mode mode)"
    }
}
else {
    Write-Ok "POSTGRES_ADMIN_PASSWORD is set"
}

# AZURE_AD_CLIENT_SECRET — optional, admin authentication disabled if not set
if (-not $env:AZURE_AD_CLIENT_SECRET) {
    if ($Mode -eq 'deploy') {
        Write-Warn "AZURE_AD_CLIENT_SECRET is not set — admin authentication will be disabled"
    }
    [Environment]::SetEnvironmentVariable('AZURE_AD_CLIENT_SECRET', "")
}

# Sync application secrets into Key Vault before deployment.
# Container Apps reference these secrets via keyVaultUrl; they must exist before
# the new revision starts.
if ($Mode -eq 'deploy') {
    # AI_API_KEY — read from the Azure AI Foundry account directly; no GitHub secret needed.
    if (-not $env:AI_API_KEY) {
        $aiName = "oai-techhub-prod"
        $aiRg   = $resourceGroup
        $aiKey  = az cognitiveservices account keys list `
            --name $aiName `
            --resource-group $aiRg `
            --query key1 -o tsv 2>$null
        if ($LASTEXITCODE -eq 0 -and $aiKey) {
            $env:AI_API_KEY = $aiKey.Trim()
            Write-Ok "AI_API_KEY resolved from Azure Cognitive Services '$aiName'"
        }
        else {
            Write-Warn "Could not read AI Foundry key from '$aiName' — AI categorization may be unavailable"
        }
    }
    else {
        Write-Ok "AI_API_KEY already set"
    }

    Write-Step "Syncing application secrets to Key Vault"
    $syncScript = Join-Path $PSScriptRoot 'Sync-KeyVaultSecrets.ps1'
    & $syncScript
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Secret sync failed — aborting deployment to prevent a crash-looping revision."
        exit 1
    }
    Write-Ok "Secrets synced"
}

# ============================================================================
# DEPLOYMENT
# ============================================================================

$deploymentName = "techhub-prod-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

# Set image tag environment variables (read by .bicepparam files via readEnvironmentVariable)
if (-not $ImageTag) {
    if ($Mode -eq 'deploy') {
        Write-Fail "ImageTag is required for deploy mode."
        Write-Detail "Provide -ImageTag with a yyyyMMddHHmmss datetime tag."
        exit 1
    }
    else {
        $ImageTag = "00000000000000"
        Write-Warn "ImageTag not set — using placeholder (acceptable for $Mode mode)"
    }
}
$env:API_IMAGE_TAG = $ImageTag
$env:WEB_IMAGE_TAG = $ImageTag
Write-Step "Image tag: $ImageTag"

# Step 1: Validate
if ($Mode -in @('validate', 'whatif', 'deploy')) {
    Write-Step "Validating Bicep template"

    az deployment sub validate `
        --location $Location `
        --template-file $templateFile `
        --parameters $paramsFile

    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Template validation failed"
        exit 1
    }
    Write-Ok "Template validation passed"
}

# Step 2: What-If (skipped in deploy mode — adds ~1-2 min overhead with no benefit in CI)
if ($Mode -eq 'whatif') {
    Write-Step "Running What-If analysis"

    az deployment sub what-if `
        --location $Location `
        --template-file $templateFile `
        --parameters $paramsFile

    if ($LASTEXITCODE -ne 0) {
        Write-Fail "What-If analysis failed"
        exit 1
    }
    Write-Ok "What-If analysis completed"
}

# Step 3: Deploy
if ($Mode -eq 'deploy') {
    Write-Step "Deploying infrastructure ($deploymentName)"

    az deployment sub create `
        --name $deploymentName `
        --location $Location `
        --template-file $templateFile `
        --parameters $paramsFile

    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Deployment failed"
        exit 1
    }
    Write-Ok "Infrastructure deployed successfully"
}

# ============================================================================
# SUMMARY
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  Infrastructure $($Mode.ToUpper()) Complete" -ForegroundColor Green
Write-Host "  Environment  : production" -ForegroundColor Gray
Write-Host "  Deployment   : $deploymentName" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""
