#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Deploys TechHub App Service sites to Azure (Phase 2).

.DESCRIPTION
    Deploys the API + Web App Service sites using Bicep templates, with the specified image tag.
    The ACS endpoint is resolved automatically from Key Vault at deploy time
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
    Preview what changes would be made to the App Service sites.

.EXAMPLE
    ./scripts/Deploy-Applications.ps1 -Mode deploy -ImageTag "20260501120000"
    Deploy the App Service sites with the given image tag.

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

# Wildcard certificates required by applications.bicep's hostNameBindings (must match
# infra/parameters/prod-applications.bicepparam's wildcardCertNames).
$keyVaultName = "kv-techhub-prod"
$appServicePlanName = "asp-techhub-prod"
$wildcardCertNames = @('wildcard-hub-ms', 'wildcard-xebia-ms')

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
    # applications.bicep's hostNameBindings requires these Microsoft.Web/certificates to already
    # exist (see docs/wildcard-certificates.md) — wildcardCert.bicep is intentionally NOT part of
    # the regular deploy cycle (App Service certificates don't auto-refresh from Key Vault), so it
    # normally only gets (re)deployed by Renew-WildcardCertificates.ps1 after a renewal. But on a
    # freshly (re)built environment with a new App Service Plan, the certificates won't exist yet —
    # import them here from whatever PFX is already in Key Vault so the deploy self-heals instead
    # of hard-failing on ResourceNotFound.
    Write-Step "Checking wildcard certificates exist"
    $missingCertNames = @($wildcardCertNames | Where-Object {
        -not (Get-AzResource -ResourceGroupName $resourceGroup -ResourceType 'Microsoft.Web/certificates' -Name $_ -ErrorAction SilentlyContinue)
    })
    if ($missingCertNames.Count -eq 0) {
        Write-Ok "All wildcard certificates present"
    } else {
        $appServicePlan = Get-AzResource -ResourceGroupName $resourceGroup -ResourceType 'Microsoft.Web/serverfarms' -Name $appServicePlanName -ErrorAction Stop
        $keyVault = Get-AzResource -ResourceGroupName $resourceGroup -ResourceType 'Microsoft.KeyVault/vaults' -Name $keyVaultName -ErrorAction Stop

        # The first-party "Microsoft Azure App Service" service principal must hold Key Vault
        # Certificate User + Key Vault Secrets User on this Key Vault to read the PFX secret (see
        # the prerequisite note in infra/modules/wildcardCert.bicep). This is normally a one-time
        # manual grant, but a freshly rebuilt Key Vault won't have it yet — assign it here so the
        # import below doesn't fail with "the service does not have access to ... Key Vault".
        # Both -ApplicationId and Get-AzADServicePrincipal need Microsoft Graph to resolve the
        # principal, which the deploy pipeline's identity cannot read ('PrincipalId' cannot be
        # null / Graph permission errors). -ObjectId + -ObjectType instead assigns the role
        # directly against ARM without any Graph lookup. The object ID below is this specific
        # tenant's instance of that service principal (appId abfa0a7c-a6b6-4736-8310-5855508787cd,
        # display name "Microsoft.Azure.WebSites") — re-resolve via `az ad sp show --id
        # abfa0a7c-a6b6-4736-8310-5855508787cd` (from an account with Graph read access) if this
        # Key Vault is ever moved to a different tenant.
        Write-Detail "Ensuring App Service certificate provider has Key Vault access"
        $appServiceCertProviderObjectId = 'c3b57f5b-db8e-4ede-bead-4f11bef97e1c'
        $requiredRoleIds = @(
            'db79e9a7-68ee-4b58-9aeb-b90e7c24fcba' # Key Vault Certificate User
            '4633458b-17de-408a-b874-0445c86b69e6' # Key Vault Secrets User
        )
        foreach ($roleId in $requiredRoleIds) {
            try {
                New-AzRoleAssignment -ObjectId $appServiceCertProviderObjectId -ObjectType ServicePrincipal -RoleDefinitionId $roleId -Scope $keyVault.ResourceId -ErrorAction Stop | Out-Null
            } catch {
                if ($_.Exception.Message -notmatch 'already exists|RoleAssignmentExists') {
                    throw
                }
            }
        }


        foreach ($certName in $missingCertNames) {
            Write-Detail "Importing missing certificate: $certName"
            New-AzResourceGroupDeployment `
                -ResourceGroupName $resourceGroup `
                -TemplateFile (Join-Path $workspaceRoot "infra/modules/wildcardCert.bicep") `
                -location $appServicePlan.Location `
                -appServicePlanId $appServicePlan.ResourceId `
                -certResourceName $certName `
                -keyVaultResourceId $keyVault.ResourceId `
                -keyVaultSecretName $certName `
                -ErrorAction Stop | Out-Null
            Write-Ok "Imported certificate: $certName"
        }
    }

    Write-Step "Deploying App Service sites"

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
        Write-Fail "App Service sites deployment failed: $_"
        exit 1
    } finally {
        $VerbosePreference = $savedVerbose
    }
    Write-Ok "App Service sites deployed successfully"
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
