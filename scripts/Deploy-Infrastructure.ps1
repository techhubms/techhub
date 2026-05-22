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

$infraTemplateFile = Join-Path $workspaceRoot "infra/infrastructure.bicep"
$infraParamsFile   = Join-Path $workspaceRoot "infra/parameters/prod-infrastructure.bicepparam"
$appsTemplateFile  = Join-Path $workspaceRoot "infra/applications.bicep"
$appsParamsFile    = Join-Path $workspaceRoot "infra/parameters/prod-applications.bicepparam"
$resourceGroup     = "rg-techhub-prod"

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
Write-Host "  Resource Group : $resourceGroup" -ForegroundColor Gray
Write-Host "  Mode        : $Mode" -ForegroundColor Gray
Write-Host "  Location    : $Location" -ForegroundColor Gray
  Write-Host "  Phase 1     : infra/infrastructure.bicep" -ForegroundColor Gray
  Write-Host "  Phase 2     : infra/applications.bicep" -ForegroundColor Gray
if ($ImageTag) {
    Write-Host "  Image Tag   : $ImageTag" -ForegroundColor Gray
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
foreach ($f in @($infraTemplateFile, $infraParamsFile, $appsTemplateFile, $appsParamsFile)) {
    if (-not (Test-Path $f)) {
        Write-Fail "File not found: $f"
        exit 1
    }
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

# AZURE_AD_CLIENT_SECRET — optional; used by Sync-KeyVaultSecrets to update the
# techhub-prod-aad-client-secret in Key Vault. If not set, the existing Key Vault
# secret is reused (deploy succeeds). If it has never been written, Sync-KeyVaultSecrets
# will fail with a clear error — set AZURE_AD_CLIENT_SECRET to write it for the first time.
if (-not $env:AZURE_AD_CLIENT_SECRET) {
    if ($Mode -eq 'deploy') {
        Write-Warn "AZURE_AD_CLIENT_SECRET is not set — existing Key Vault secret will be reused (run will fail if the secret has never been written)"
    }
    [Environment]::SetEnvironmentVariable('AZURE_AD_CLIENT_SECRET', "")
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
    Write-Step "Validating Bicep templates"

    foreach ($pair in @(
        @{ Template = $infraTemplateFile; Params = $infraParamsFile; Label = 'infrastructure' }
        @{ Template = $appsTemplateFile;  Params = $appsParamsFile;  Label = 'applications'  }
    )) {
        $validationErrors = Test-AzDeployment `
            -Location $Location `
            -TemplateFile $pair.Template `
            -TemplateParameterFile $pair.Params `
            -SkipTemplateParameterPrompt
        if ($validationErrors) {
            $validationErrors | ForEach-Object { Write-Fail $_.Message }
            Write-Fail "$($pair.Label) template validation failed"
            exit 1
        }
        Write-Ok "$($pair.Label) template validation passed"
    }
}

# Step 2: What-If (skipped in deploy mode — adds ~1-2 min overhead with no benefit in CI)
if ($Mode -eq 'whatif') {
    foreach ($pair in @(
        @{ Template = $infraTemplateFile; Params = $infraParamsFile; Label = 'infrastructure' }
        @{ Template = $appsTemplateFile;  Params = $appsParamsFile;  Label = 'applications'  }
    )) {
        Write-Step "Running What-If: $($pair.Label)"
        try {
            New-AzDeployment `
                -Location $Location `
                -TemplateFile $pair.Template `
                -TemplateParameterFile $pair.Params `
                -SkipTemplateParameterPrompt `
                -WhatIf
        } catch {
            Write-Fail "What-If failed for $($pair.Label): $_"
            exit 1
        }
        Write-Ok "$($pair.Label) What-If completed"
    }
}

# Step 3: Deploy (two-phase to handle fresh environments where the Key Vault does not yet exist)
if ($Mode -eq 'deploy') {
    # Phase 1: infrastructure.bicep — VNet, Key Vault, PostgreSQL, identity, Container Apps Environment.
    # Deploys before secrets are synced so the Key Vault exists when Sync-KeyVaultSecrets.ps1 runs.
    Write-Step "Phase 1: Deploying base infrastructure"

    # New-AzDeployment -Verbose streams one line per resource as it completes.
    $savedVerbose = $VerbosePreference
    $VerbosePreference = 'Continue'
    try {
        New-AzDeployment `
            -Name "$deploymentName-infra" `
            -Location $Location `
            -TemplateFile $infraTemplateFile `
            -TemplateParameterFile $infraParamsFile `
            -SkipTemplateParameterPrompt
    } catch {
        Write-Fail "Phase 1 infrastructure deployment failed: $_"
        exit 1
    } finally {
        $VerbosePreference = $savedVerbose
    }
    Write-Ok "Base infrastructure deployed successfully"

    # Clean up any stale PostgreSQL firewall rules left over from previous infrastructure iterations.
    # Bicep uses incremental deployments — renaming a rule resource creates the new rule but leaves
    # the old one as an orphan. The rules below were replaced by 'allow-nat-gateway' in PR #425.
    Write-Step "Cleaning up stale PostgreSQL firewall rules"
    $postgresServer = "psql-techhub-prod"
    $staleRules = @("allow-container-apps-subnet", "allow-container-apps-static-ip")
    foreach ($ruleName in $staleRules) {
        try {
            $rule = Get-AzPostgreSqlFlexibleServerFirewallRule `
                -ResourceGroupName $resourceGroup `
                -ServerName $postgresServer `
                -Name $ruleName `
                -ErrorAction SilentlyContinue
            if ($rule) {
                Remove-AzPostgreSqlFlexibleServerFirewallRule `
                    -ResourceGroupName $resourceGroup `
                    -ServerName $postgresServer `
                    -Name $ruleName
                Write-Ok "Deleted stale rule: $ruleName"
            } else {
                Write-Detail "Rule not present (already clean): $ruleName"
            }
        } catch {
            Write-Warn "Could not check/remove rule '$ruleName': $_"
        }
    }

    # Sync secrets now that the Key Vault exists.
    # Container Apps reference secrets via keyVaultUrl; they are fetched when the
    # revision starts, so writing them before Phase 2 ensures they are available
    # as soon as the revision begins pulling the image.
    # Note: Database connection string and AI API key are no longer stored as KV secrets —
    # the app uses managed identity token auth (Entra) for both PostgreSQL and AI Foundry.
    Write-Step "Syncing application secrets to Key Vault"
    $syncScript = Join-Path $PSScriptRoot 'Sync-KeyVaultSecrets.ps1'
    try {
        & $syncScript
    } catch {
        Write-Fail "Secret sync failed — aborting deployment to prevent a crash-looping revision."
        Write-Fail $_
        exit 1
    }
    Write-Ok "Secrets synced"

    # Phase 2: applications.bicep — Container Apps with secrets already in Key Vault.
    Write-Step "Phase 2: Deploying Container Apps"

    $savedVerbose = $VerbosePreference
    $VerbosePreference = 'Continue'
    try {
        New-AzDeployment `
            -Name "$deploymentName-apps" `
            -Location $Location `
            -TemplateFile $appsTemplateFile `
            -TemplateParameterFile $appsParamsFile `
            -SkipTemplateParameterPrompt
    } catch {
        Write-Fail "Phase 2 Container Apps deployment failed: $_"
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
Write-Host "  Infrastructure $($Mode.ToUpper()) Complete" -ForegroundColor Green
Write-Host "  Resource Group  : $resourceGroup" -ForegroundColor Gray
if ($Mode -eq 'deploy') {
    Write-Host "  Deployments     : $deploymentName-infra, $deploymentName-apps" -ForegroundColor Gray
} else {
    Write-Host "  Deployment      : $deploymentName" -ForegroundColor Gray
}
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""
