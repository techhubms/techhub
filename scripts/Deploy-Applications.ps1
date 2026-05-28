#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Deploys TechHub Container Apps to Azure (Phase 2).

.DESCRIPTION
    Deploys Container Apps using Bicep templates. Deploys API + Web container apps with the
    specified image tag. The ACS endpoint is resolved automatically from Key Vault at deploy time
    (stored there by Deploy-Infrastructure.ps1).

    Phase 1 (infrastructure) must be deployed first — this script references existing resources
    created by infrastructure.bicep.

    Can be run locally or from GitHub Actions workflows.

.PARAMETER Mode
    Deployment mode: validate (syntax check), whatif (preview changes), or deploy (apply changes).
    Defaults to 'whatif' for safety.

.PARAMETER Location
    Azure region for deployment metadata. Defaults to 'swedencentral'.

.PARAMETER ImageTag
    Docker image tag to deploy. Required for deploy mode (yyyyMMddHHmmss format).

.EXAMPLE
    ./scripts/Deploy-Applications.ps1 -Mode whatif -ImageTag "20260501120000"
    Preview what changes would be made to Container Apps.

.EXAMPLE
    ./scripts/Deploy-Applications.ps1 -Mode deploy -ImageTag "20260501120000"
    Deploy Container Apps with the given image tag.

.EXAMPLE
    ./scripts/Deploy-Applications.ps1 -Mode validate -ImageTag "20260501120000"
    Validate the Bicep template without making any changes.
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

$appsTemplateFile = Join-Path $workspaceRoot "infra/applications.bicep"
$appsParamsFile   = Join-Path $workspaceRoot "infra/parameters/prod-applications.bicepparam"
$resourceGroup    = "rg-techhub-prod"

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
Write-Host "  TechHub Application Deployment (Phase 2 - Bicep)" -ForegroundColor White
Write-Host "  Resource Group : $resourceGroup" -ForegroundColor Gray
Write-Host "  Mode           : $Mode" -ForegroundColor Gray
Write-Host "  Location       : $Location" -ForegroundColor Gray
Write-Host "  Template       : infra/applications.bicep" -ForegroundColor Gray
if ($ImageTag) {
    Write-Host "  Image Tag      : $ImageTag" -ForegroundColor Gray
}
Write-Host "===============================================================" -ForegroundColor DarkCyan

# ============================================================================
# PRE-FLIGHT CHECKS
# ============================================================================

Write-Step "Validating prerequisites"

# Check Azure PowerShell login
$context = Get-AzContext -ErrorAction SilentlyContinue
if (-not $context) {
    Write-Fail "Not logged in to Azure PowerShell. Run 'Connect-AzAccount' first."
    exit 1
}
Write-Ok "Azure PowerShell authenticated (subscription: $($context.Subscription.Name))"

# Check template files exist
foreach ($f in @($appsTemplateFile, $appsParamsFile)) {
    if (-not (Test-Path $f)) {
        Write-Fail "File not found: $f"
        exit 1
    }
}
Write-Ok "Template files found"

# ImageTag is required for deploy mode
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

# Set image tag environment variables (read by .bicepparam via readEnvironmentVariable)
$env:API_IMAGE_TAG = $ImageTag
$env:WEB_IMAGE_TAG = $ImageTag
Write-Ok "Image tag: $ImageTag"

# ============================================================================
# DEPLOYMENT
# ============================================================================

$deploymentName = "techhub-prod-apps-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

# Step 1: Validate
if ($Mode -in @('validate', 'whatif', 'deploy')) {
    Write-Step "Validating Bicep template"

    $validationErrors = Test-AzDeployment `
        -Location $Location `
        -TemplateFile $appsTemplateFile `
        -TemplateParameterFile $appsParamsFile `
        -SkipTemplateParameterPrompt
    if ($validationErrors) {
        $validationErrors | ForEach-Object { Write-Fail $_.Message }
        Write-Fail "Applications template validation failed"
        exit 1
    }
    Write-Ok "Applications template validation passed"
}

# Step 2: What-If
if ($Mode -eq 'whatif') {
    Write-Step "Running What-If: applications"
    try {
        New-AzDeployment `
            -Location $Location `
            -TemplateFile $appsTemplateFile `
            -TemplateParameterFile $appsParamsFile `
            -SkipTemplateParameterPrompt `
            -WhatIf
    } catch {
        Write-Fail "What-If failed: $_"
        exit 1
    }
    Write-Ok "Applications What-If completed"
}

# Step 3: Deploy
if ($Mode -eq 'deploy') {
    Write-Step "Deploying Container Apps"

    $savedVerbose = $VerbosePreference
    $VerbosePreference = 'Continue'
    try {
        New-AzDeployment `
            -Name $deploymentName `
            -Location $Location `
            -TemplateFile $appsTemplateFile `
            -TemplateParameterFile $appsParamsFile `
            -SkipTemplateParameterPrompt
    } catch {
        Write-Fail "Container Apps deployment failed: $_"
        exit 1
    } finally {
        $VerbosePreference = $savedVerbose
    }
    Write-Ok "Container Apps deployed successfully"
}

# ============================================================================
# SUMMARY
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  Application Deployment $($Mode.ToUpper()) Complete" -ForegroundColor Green
Write-Host "  Resource Group  : $resourceGroup" -ForegroundColor Gray
Write-Host "  Deployment      : $deploymentName" -ForegroundColor Gray
Write-Host "  Image Tag       : $ImageTag" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""
