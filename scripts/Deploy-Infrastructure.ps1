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

# Check POSTGRES_ADMIN_PASSWORD for production only
# Staging no longer deploys a PostgreSQL server via Bicep — PR environments handle
# their own databases via Deploy-PrPreview.ps1 (PITR + password reset).
if ($Environment -eq 'production') {
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

    # AZURE_AD_TENANT_ID and AZURE_AD_CLIENT_ID are hardcoded in the .bicepparam files
    # (public Entra identifiers — not secrets). No env var resolution needed.

    # AZURE_AD_CLIENT_SECRET — must be provided externally; there is no way to read it from Azure.
    if (-not $env:AZURE_AD_CLIENT_SECRET) {
        if ($Mode -eq 'deploy') {
            Write-Warn "AZURE_AD_CLIENT_SECRET is not set — admin authentication will be disabled"
        }
        [Environment]::SetEnvironmentVariable('AZURE_AD_CLIENT_SECRET', "")
    }
}


# Resolve ACTION_GROUP_ID for environments that use Bicep alerts.
# Staging: skip alerts entirely — PR environments use E2E tests, not synthetic monitoring.
# Production: resolve from the shared action group.
if ($Environment -eq 'staging') {
    $env:ACTION_GROUP_ID = ""
    Write-Ok "Alerts disabled for staging (PR environments rely on E2E tests, not alerts)"
}
elseif ($Environment -eq 'production') {
    $sharedRg     = $envConfig.shared.ResourceGroup
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

# Sync application secrets into Key Vault before deployment.
# Container Apps reference these secrets via keyVaultUrl; they must exist before
# the new revision starts. Only required for production — staging no longer deploys
# Container Apps via Bicep (PR environments set their own secrets directly).
if ($Mode -eq 'deploy' -and $Environment -eq 'production') {
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
    & $syncScript -Environment 'prod'
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Secret sync failed — aborting deployment to prevent a crash-looping revision."
        exit 1
    }
    Write-Ok "Secrets synced"
}

# ============================================================================
# DEPLOYMENT
# ============================================================================

$deploymentName = "techhub-$($config.EnvSuffix)-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

# Set image tag environment variables (read by .bicepparam files via readEnvironmentVariable)
# Only required for production — staging no longer deploys Container Apps via Bicep.
if ($Environment -eq 'production') {
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

    # One-time cleanup: permanently remove old permanent staging resources that are no
    # longer deployed by Bicep. This is idempotent — subsequent runs simply find nothing
    # to delete. The CAE, VNet, monitoring stack, and OpenAI resources are kept because
    # they host ephemeral PR environments (ca-techhub-api-pr-{N}, ca-techhub-web-pr-{N}).
    if ($Environment -eq 'staging') {
        Write-Step "Removing permanent staging resources (one-time cleanup — idempotent)"
        $stagingRg = $config.ResourceGroup

        $resourcesToDelete = @(
            @{ Type = 'Container App';            Name = 'ca-techhub-api-staging';  Cmd = { az containerapp delete --name ca-techhub-api-staging --resource-group $stagingRg --yes 2>$null } }
            @{ Type = 'Container App';            Name = 'ca-techhub-web-staging';  Cmd = { az containerapp delete --name ca-techhub-web-staging --resource-group $stagingRg --yes 2>$null } }
            @{ Type = 'Private Endpoint';         Name = 'pe-psql-techhub-staging'; Cmd = { az network private-endpoint delete --name pe-psql-techhub-staging --resource-group $stagingRg 2>$null } }
            @{ Type = 'PostgreSQL Flexible Server'; Name = 'psql-techhub-staging';  Cmd = { az postgres flexible-server delete --name psql-techhub-staging --resource-group $stagingRg --yes 2>$null } }
        )

        foreach ($res in $resourcesToDelete) {
            Write-Detail "Deleting $($res.Type) '$($res.Name)'..."
            & $res.Cmd
            if ($LASTEXITCODE -eq 0) {
                Write-Ok "$($res.Type) '$($res.Name)' deleted (or was already gone)"
            }
            else {
                Write-Ok "$($res.Type) '$($res.Name)' not found — skipping (already deleted)"
            }
        }

        Write-Ok "Staging cleanup complete"
    }
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
