#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Deploys TechHub Azure infrastructure for a given environment.

.DESCRIPTION
    Deploys infrastructure using Bicep templates. Supports shared, staging, and production environments
    with validate, what-if, and deploy modes. Includes pre-flight checks for known issues
    (e.g., soft-deleted AI resources) and post-deployment steps (ACR pull role assignments).

    Can be run locally or from GitHub Actions workflows.

.PARAMETER Environment
    Target environment: shared, staging, or production.

.PARAMETER Mode
    Deployment mode: validate (syntax check), whatif (preview changes), or deploy (apply changes).
    Defaults to 'whatif' for safety.

.PARAMETER Location
    Azure region for deployment metadata. Defaults to the environment's primary region.

.PARAMETER ImageTag
    Docker image tag to deploy. When provided, overrides apiImageTag and webImageTag
    parameters in Bicep. Required for staging/production environments in deploy mode.

.EXAMPLE
    ./scripts/Deploy-Infrastructure.ps1 -Environment staging -Mode whatif
    Preview what changes would be made to staging infrastructure.

.EXAMPLE
    ./scripts/Deploy-Infrastructure.ps1 -Environment shared -Mode deploy
    Deploy shared resources (container registry).

.EXAMPLE
    ./scripts/Deploy-Infrastructure.ps1 -Environment production -Mode deploy
    Deploy production infrastructure.

.EXAMPLE
    ./scripts/Deploy-Infrastructure.ps1 -Environment staging -Mode validate
    Validate staging Bicep template without making any changes.
#>

param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('shared', 'staging', 'production')]
    [string]$Environment,

    [Parameter(Mandatory = $false)]
    [ValidateSet('validate', 'whatif', 'deploy')]
    [string]$Mode = 'whatif',

    [Parameter(Mandatory = $false)]
    [string]$Location,

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

# Environment configuration
$envConfig = @{
    shared     = @{
        TemplatePath     = "infra/shared.bicep"
        ParamsPath       = "infra/parameters/shared.bicepparam"
        DefaultLocation  = "westeurope"
        ResourceGroup    = "rg-techhub-shared"
        EnvSuffix        = "shared"
        ActionGroupName  = "ag-techhub-ops"
    }
    staging    = @{
        TemplatePath     = "infra/main.bicep"
        ParamsPath       = "infra/parameters/staging.bicepparam"
        DefaultLocation  = "swedencentral"
        ResourceGroup    = "rg-techhub-staging"
        EnvSuffix        = "staging"
    }
    production = @{
        TemplatePath     = "infra/main.bicep"
        ParamsPath       = "infra/parameters/prod.bicepparam"
        DefaultLocation  = "swedencentral"
        ResourceGroup    = "rg-techhub-prod"
        EnvSuffix        = "prod"
    }
}

$config = $envConfig[$Environment]
$templateFile = Join-Path $workspaceRoot $config.TemplatePath
$paramsFile = Join-Path $workspaceRoot $config.ParamsPath
$deployLocation = if ($Location) { $Location } else { $config.DefaultLocation }

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
Write-Host "  Environment : $Environment" -ForegroundColor Gray
Write-Host "  Mode        : $Mode" -ForegroundColor Gray
Write-Host "  Location    : $deployLocation" -ForegroundColor Gray
Write-Host "  Template    : $($config.TemplatePath)" -ForegroundColor Gray
Write-Host "  Parameters  : $($config.ParamsPath)" -ForegroundColor Gray
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

# ADMIN_IP_ADDRESSES — used by all environments (shared, staging, production) to configure
# PostgreSQL and Key Vault firewall rules. Must be set externally; no safe default exists.
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

# Check POSTGRES_ADMIN_PASSWORD for environments that need it
if ($Environment -ne 'shared') {
    if (-not $env:POSTGRES_ADMIN_PASSWORD) {
        if ($Mode -eq 'deploy') {
            Write-Fail "Environment variable POSTGRES_ADMIN_PASSWORD is not set."
            Write-Detail "Set it with: `$env:POSTGRES_ADMIN_PASSWORD = '<password>'"
            exit 1
        }
        else {
            # For validate/whatif, use a placeholder — the actual value doesn't matter
            $env:POSTGRES_ADMIN_PASSWORD = "placeholder-for-$Mode"
            Write-Warn "POSTGRES_ADMIN_PASSWORD not set — using placeholder (acceptable for $Mode mode)"
        }
    }
    else {
        Write-Ok "POSTGRES_ADMIN_PASSWORD is set"
    }

    # AZURE_AD_TENANT_ID — derive from the authenticated Azure CLI session; no GitHub secret needed.
    if (-not $env:AZURE_AD_TENANT_ID) {
        $env:AZURE_AD_TENANT_ID = $accountInfo.tenantId
        Write-Ok "AZURE_AD_TENANT_ID resolved from Azure CLI session: $($env:AZURE_AD_TENANT_ID)"
    }
    else {
        Write-Ok "AZURE_AD_TENANT_ID already set"
    }

    # AZURE_AD_CLIENT_ID — look up the app registration by its deterministic display name.
    # The name convention matches what Manage-EntraId.ps1 creates; no GitHub secret needed.
    if (-not $env:AZURE_AD_CLIENT_ID) {
        $appDisplayName = if ($Environment -eq 'production') { 'TechHub Production' } else { 'TechHub Staging' }
        $clientId = az ad app list --display-name $appDisplayName --query '[0].appId' -o tsv 2>$null
        if ($LASTEXITCODE -eq 0 -and $clientId) {
            $env:AZURE_AD_CLIENT_ID = $clientId.Trim()
            Write-Ok "AZURE_AD_CLIENT_ID resolved from Entra (app: '$appDisplayName'): $($env:AZURE_AD_CLIENT_ID)"
        }
        else {
            $env:AZURE_AD_CLIENT_ID = ""
            Write-Warn "App registration '$appDisplayName' not found — admin authentication will be disabled"
            Write-Detail "Run 'Manage-EntraId.ps1 -Environment $Environment' to create it."
        }
    }
    else {
        Write-Ok "AZURE_AD_CLIENT_ID already set"
    }

    # AZURE_AD_CLIENT_SECRET — must be provided externally; there is no way to read it from Azure.
    if (-not $env:AZURE_AD_CLIENT_SECRET) {
        if ($Mode -eq 'deploy') {
            Write-Warn "AZURE_AD_CLIENT_SECRET is not set — admin authentication will be disabled"
        }
        [Environment]::SetEnvironmentVariable('AZURE_AD_CLIENT_SECRET', "")
    }

    $adConfigured = -not [string]::IsNullOrEmpty($env:AZURE_AD_CLIENT_ID)
    if ($adConfigured) {
        Write-Ok "Azure AD configured"
    }
    else {
        Write-Warn "Azure AD not configured — admin authentication will be disabled"
    }

    # Resolve the shared action group resource ID automatically so callers don't need to
    # set ACTION_GROUP_ID manually. Names are taken from the shared envConfig entry so a
    # rename in config is the only change needed — no hardcoded strings here.
    if (-not $env:ACTION_GROUP_ID) {
        $sharedRg  = $envConfig.shared.ResourceGroup
        $sharedAgName = $envConfig.shared.ActionGroupName
        $agJson = az monitor action-group show `
            --resource-group $sharedRg `
            --name $sharedAgName `
            --query id --output tsv 2>$null
        if ($LASTEXITCODE -eq 0 -and $agJson) {
            $env:ACTION_GROUP_ID = $agJson.Trim()
            Write-Ok "Action group resolved: $($env:ACTION_GROUP_ID)"
        }
        else {
            # Shared infra not yet deployed or action group not created — alerts will be skipped.
            $env:ACTION_GROUP_ID = ""
            Write-Warn "Action group '$sharedAgName' not found in '$sharedRg' — operational alerts will be disabled for this deployment"
            Write-Detail "Deploy shared infrastructure first to enable alerts."
        }
    }
    else {
        Write-Ok "ACTION_GROUP_ID already set: $($env:ACTION_GROUP_ID)"
    }

    # Sync application secrets into Key Vault before deployment.
    # Container Apps reference these secrets via keyVaultUrl; they must exist before
    # the new revision starts. POSTGRES_ADMIN_PASSWORD and AZURE_AD_CLIENT_SECRET must
    # be provided externally (GitHub secrets). AI_API_KEY is read from Azure directly
    # so it does not need to be a GitHub secret.
    if ($Mode -eq 'deploy') {
        # AI_API_KEY — read from the Azure AI Foundry account directly; no GitHub secret needed.
        if (-not $env:AI_API_KEY) {
            $aiName = "oai-techhub-$($config.EnvSuffix)"
            $aiRg   = $config.ResourceGroup
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
        # Map Deploy-Infrastructure.ps1 environment names to the sync script's convention
        $syncEnv = if ($Environment -eq 'production') { 'prod' } else { $Environment }
        & $syncScript -Environment $syncEnv
        if ($LASTEXITCODE -ne 0) {
            Write-Fail "Secret sync failed — aborting deployment to prevent a crash-looping revision."
            exit 1
        }
        Write-Ok "Secrets synced"
    }
}

# ============================================================================
# DEPLOYMENT
# ============================================================================

$deploymentName = "techhub-$($config.EnvSuffix)-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

# Set image tag environment variables (read by .bicepparam files via readEnvironmentVariable)
if ($Environment -ne 'shared') {
    if (-not $ImageTag) {
        if ($Mode -eq 'deploy') {
            Write-Fail "ImageTag is required for $Environment deployments."
            Write-Detail "Provide -ImageTag with a yyyyMMddHHmmss datetime tag."
            exit 1
        }
        else {
            # For validate/whatif, use a placeholder — the actual value doesn't matter
            $ImageTag = "00000000000000"
            Write-Warn "ImageTag not set — using placeholder (acceptable for $Mode mode)"
        }
    }
    $env:API_IMAGE_TAG = $ImageTag
    $env:WEB_IMAGE_TAG = $ImageTag
    Write-Step "Image tag: $ImageTag"
}

# Step 1: Validate
if ($Mode -in @('validate', 'whatif', 'deploy')) {
    Write-Step "Validating Bicep template"

    az deployment sub validate `
        --location $deployLocation `
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
        --location $deployLocation `
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
        --location $deployLocation `
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
Write-Host "  Environment  : $Environment" -ForegroundColor Gray
Write-Host "  Deployment   : $deploymentName" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""
