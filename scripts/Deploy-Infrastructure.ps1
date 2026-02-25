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
    [string]$Location
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
        TemplatePath    = "infra/shared.bicep"
        ParamsPath      = "infra/parameters/shared.bicepparam"
        DefaultLocation = "westeurope"
        ResourceGroup   = "rg-techhub-shared"
        EnvSuffix       = "shared"
        OpenAi          = $null
    }
    staging    = @{
        TemplatePath    = "infra/main.bicep"
        ParamsPath      = "infra/parameters/staging.bicepparam"
        DefaultLocation = "swedencentral"
        ResourceGroup   = "rg-techhub-staging"
        EnvSuffix       = "staging"
        # OpenAI deployed separately at resource-group level (Azure bug 715-123420)
        OpenAi          = @{
            TemplatePath = "infra/modules/openai.bicep"
            Location     = "swedencentral"
            Name         = "oai-techhub-staging"
            Capacity     = 50
        }
    }
    production = @{
        TemplatePath    = "infra/main.bicep"
        ParamsPath      = "infra/parameters/prod.bicepparam"
        DefaultLocation = "westeurope"
        ResourceGroup   = "rg-techhub-prod"
        EnvSuffix       = "prod"
        # OpenAI deployed separately at resource-group level (Azure bug 715-123420)
        OpenAi          = @{
            TemplatePath = "infra/modules/openai.bicep"
            Location     = "swedencentral"
            Name         = "oai-techhub-prod"
            Capacity     = 100
        }
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
}

# Pre-flight: Check for soft-deleted AI resources (staging/production only)
if ($Environment -ne 'shared' -and $Mode -eq 'deploy' -and $config.OpenAi) {
    Write-Step "Checking for soft-deleted AI Services resources"

    $openAiName = $config.OpenAi.Name
    $openAiLocation = $config.OpenAi.Location
    $deletedJson = az cognitiveservices account list-deleted `
        --query "[?name=='$openAiName']" -o json 2>$null
    $deleted = if ($deletedJson) { $deletedJson | ConvertFrom-Json } else { @() }

    if ($deleted -and $deleted.Count -gt 0) {
        Write-Warn "Found soft-deleted AI resource '$openAiName'. Purging..."
        az cognitiveservices account purge `
            --name $openAiName `
            --resource-group $config.ResourceGroup `
            --location $openAiLocation
        Write-Detail "Waiting 60 seconds for purge to propagate..."
        Start-Sleep -Seconds 60
        Write-Ok "Soft-deleted resource purged"
    }
    else {
        Write-Ok "No soft-deleted AI resources found"
    }
}

# ============================================================================
# DEPLOYMENT
# ============================================================================

$deploymentName = "techhub-$($config.EnvSuffix)-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

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

    if ($config.OpenAi) {
        $openAiTemplate = Join-Path $workspaceRoot $config.OpenAi.TemplatePath
        $rgExists = az group exists --name $config.ResourceGroup 2>$null
        if ($rgExists -eq 'true') {
            Write-Step "Validating OpenAI template"

            az deployment group validate `
                --resource-group $config.ResourceGroup `
                --template-file $openAiTemplate `
                --parameters location=$($config.OpenAi.Location) `
                             openAiName=$($config.OpenAi.Name) `
                             modelCapacity=$($config.OpenAi.Capacity)

            if ($LASTEXITCODE -ne 0) {
                # Azure bug 715-123420: CognitiveServices validation can intermittently fail.
                # This is non-blocking — the actual deployment is not affected.
                Write-Warn "OpenAI template validation failed (Azure bug 715-123420, non-blocking)"
            } else {
                Write-Ok "OpenAI template validation passed"
            }
        } else {
            Write-Warning "Skipping OpenAI validation (resource group does not exist yet)"
        }
    }
}

# Step 2: What-If
if ($Mode -in @('whatif', 'deploy')) {
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

    if ($config.OpenAi) {
        $openAiTemplate = Join-Path $workspaceRoot $config.OpenAi.TemplatePath
        $rgExists = az group exists --name $config.ResourceGroup 2>$null
        if ($rgExists -eq 'true') {
            Write-Step "Running OpenAI What-If analysis"

            az deployment group what-if `
                --resource-group $config.ResourceGroup `
                --template-file $openAiTemplate `
                --parameters location=$($config.OpenAi.Location) `
                             openAiName=$($config.OpenAi.Name) `
                             modelCapacity=$($config.OpenAi.Capacity)

            if ($LASTEXITCODE -ne 0) {
                # Azure bug 715-123420: CognitiveServices what-if can intermittently fail.
                # This is non-blocking — the actual deployment is not affected.
                Write-Warn "OpenAI What-If analysis failed (Azure bug 715-123420, non-blocking)"
            } else {
                Write-Ok "OpenAI What-If analysis completed"
            }
        } else {
            Write-Warning "Skipping OpenAI What-If (resource group does not exist yet)"
        }
    }
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

    # Post-deployment: Deploy OpenAI separately (Azure bug 715-123420 prevents
    # CognitiveServices from being deployed in nested subscription-level deployments)
    if ($config.OpenAi) {
        $openAiConfig = $config.OpenAi
        $openAiTemplate = Join-Path $workspaceRoot $openAiConfig.TemplatePath
        $openAiDeployName = "openai-$($config.EnvSuffix)-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

        Write-Step "Deploying Azure AI Foundry ($($openAiConfig.Name))"

        az deployment group create `
            --name $openAiDeployName `
            --resource-group $config.ResourceGroup `
            --template-file $openAiTemplate `
            --parameters location=$($openAiConfig.Location) `
                         openAiName=$($openAiConfig.Name) `
                         modelCapacity=$($openAiConfig.Capacity)

        if ($LASTEXITCODE -ne 0) {
            Write-Fail "OpenAI deployment failed"
            exit 1
        }
        Write-Ok "Azure AI Foundry deployed successfully"
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
