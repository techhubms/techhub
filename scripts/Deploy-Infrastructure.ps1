#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Deploys TechHub Azure infrastructure (Phase 1 only).

.DESCRIPTION
    Deploys base platform infrastructure using Bicep templates: networking, identity, Key Vault,
    PostgreSQL, Azure AI Foundry, Azure Communication Services, monitoring, and governance.

    Does NOT deploy Container Apps — use Deploy-Applications.ps1 for that.

    Can be run locally or from GitHub Actions workflows.

.PARAMETER Mode
    Deployment mode: validate (syntax check), whatif (preview changes), or deploy (apply changes).
    Defaults to 'whatif' for safety.

.PARAMETER Location
    Azure region for deployment metadata. Defaults to 'swedencentral'.

.EXAMPLE
    ./scripts/Deploy-Infrastructure.ps1 -Mode whatif
    Preview what changes would be made to production infrastructure.

.EXAMPLE
    ./scripts/Deploy-Infrastructure.ps1 -Mode deploy
    Deploy production infrastructure.

.EXAMPLE
    ./scripts/Deploy-Infrastructure.ps1 -Mode validate
    Validate the production Bicep template without making any changes.
#>

param(
    [Parameter(Mandatory = $false)]
    [ValidateSet('validate', 'whatif', 'deploy')]
    [string]$Mode = 'whatif',

    [Parameter(Mandatory = $false)]
    [string]$Location = 'swedencentral'
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
Write-Host "  TechHub Infrastructure Deployment (Phase 1)" -ForegroundColor White
Write-Host "  Resource Group : $resourceGroup" -ForegroundColor Gray
Write-Host "  Mode           : $Mode" -ForegroundColor Gray
Write-Host "  Location       : $Location" -ForegroundColor Gray
Write-Host "  Template       : infra/infrastructure.bicep" -ForegroundColor Gray
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
foreach ($f in @($infraTemplateFile, $infraParamsFile)) {
    if (-not (Test-Path $f)) {
        Write-Fail "File not found: $f"
        exit 1
    }
}
Write-Ok "Template files found"

# ADMIN_IP_ADDRESSES — required for PostgreSQL and Key Vault firewall rules.
# If not set, reads from the ADMIN_IP_ADDRESSES GitHub Actions variable.
if (-not $env:ADMIN_IP_ADDRESSES) {
    try {
        $ghIps = (gh variable get ADMIN_IP_ADDRESSES --repo techhubms/techhub 2>$null)
        if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($ghIps)) {
            $env:ADMIN_IP_ADDRESSES = $ghIps.Trim()
            Write-Ok "ADMIN_IP_ADDRESSES read from GitHub variable: $($env:ADMIN_IP_ADDRESSES)"
        }
    } catch {}
}

if (-not $env:ADMIN_IP_ADDRESSES) {
    if ($Mode -eq 'deploy') {
        Write-Fail "ADMIN_IP_ADDRESSES is not set and could not be read from GitHub variable."
        Write-Detail "Set it with: `$env:ADMIN_IP_ADDRESSES = '<ip1>,<ip2>'"
        exit 1
    }
    $env:ADMIN_IP_ADDRESSES = "0.0.0.0"
    Write-Warn "ADMIN_IP_ADDRESSES not set — using placeholder (acceptable for $Mode mode)"
}
else {
    Write-Ok "ADMIN_IP_ADDRESSES is set: $($env:ADMIN_IP_ADDRESSES)"
}

# POSTGRES_ADMIN_PASSWORD — required for production.
# If not set, reads from Key Vault secret 'techhub-prod-postgres-admin-password'.
if (-not $env:POSTGRES_ADMIN_PASSWORD) {
    Write-Detail "POSTGRES_ADMIN_PASSWORD not set — trying Key Vault..."
    try {
        $kvPassword = (az keyvault secret show `
            --vault-name kv-techhub-prod `
            --name techhub-prod-postgres-admin-password `
            --query value -o tsv 2>$null)
        if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($kvPassword)) {
            $env:POSTGRES_ADMIN_PASSWORD = $kvPassword
            Write-Ok "POSTGRES_ADMIN_PASSWORD read from Key Vault"
        }
    } catch {}
}

if (-not $env:POSTGRES_ADMIN_PASSWORD) {
    Write-Fail "POSTGRES_ADMIN_PASSWORD is not set and could not be read from Key Vault."
    Write-Detail "Set it with: `$env:POSTGRES_ADMIN_PASSWORD = '<password>'"
    Write-Detail "Or ensure kv-techhub-prod is accessible and contains 'techhub-prod-postgres-admin-password'."
    exit 1
}
else {
    Write-Ok "POSTGRES_ADMIN_PASSWORD is set"
}

# ============================================================================
# DEPLOYMENT
# ============================================================================

$deploymentName = "techhub-prod-infra-$(Get-Date -Format 'yyyyMMdd-HHmmss')"

# Step 1: Validate
if ($Mode -in @('validate', 'whatif', 'deploy')) {
    Write-Step "Validating Bicep template"

    $validationErrors = Test-AzDeployment `
        -Location $Location `
        -TemplateFile $infraTemplateFile `
        -TemplateParameterFile $infraParamsFile `
        -SkipTemplateParameterPrompt
    if ($validationErrors) {
        $validationErrors | ForEach-Object { Write-Fail $_.Message }
        Write-Fail "Infrastructure template validation failed"
        exit 1
    }
    Write-Ok "Infrastructure template validation passed"
}

# Step 2: What-If
if ($Mode -eq 'whatif') {
    Write-Step "Running What-If: infrastructure"
    try {
        New-AzDeployment `
            -Location $Location `
            -TemplateFile $infraTemplateFile `
            -TemplateParameterFile $infraParamsFile `
            -SkipTemplateParameterPrompt `
            -WhatIf
    } catch {
        Write-Fail "What-If failed: $_"
        exit 1
    }
    Write-Ok "Infrastructure What-If completed"
}

# Step 3: Deploy
if ($Mode -eq 'deploy') {
    Write-Step "Deploying base infrastructure"

    $savedVerbose = $VerbosePreference
    $VerbosePreference = 'Continue'
    try {
        New-AzDeployment `
            -Name $deploymentName `
            -Location $Location `
            -TemplateFile $infraTemplateFile `
            -TemplateParameterFile $infraParamsFile `
            -SkipTemplateParameterPrompt `
            -OutVariable infraResult | Out-Null
    } catch {
        Write-Fail "Infrastructure deployment failed: $_"
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

    # Store the ACS endpoint in Key Vault so Deploy-Applications.ps1 can reference it
    # without needing to read infrastructure deployment outputs.
    $acsEndpoint = $infraResult[0].Outputs["acsEndpoint"].Value
    Write-Detail "ACS Endpoint: $acsEndpoint"
    $env:NEWSLETTER_ACS_ENDPOINT = $acsEndpoint

    # Sync secrets now that the Key Vault exists.
    Write-Step "Syncing application secrets to Key Vault"
    $syncScript = Join-Path $PSScriptRoot 'Sync-KeyVaultSecrets.ps1'
    try {
        & $syncScript
    } catch {
        Write-Fail "Secret sync failed: $_"
        exit 1
    }
    Write-Ok "Secrets synced"
}

# ============================================================================
# SUMMARY
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  Infrastructure $($Mode.ToUpper()) Complete" -ForegroundColor Green
Write-Host "  Resource Group  : $resourceGroup" -ForegroundColor Gray
Write-Host "  Deployment      : $deploymentName" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""
