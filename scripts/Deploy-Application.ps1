#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Deploys TechHub containers to Azure Container Apps (imperative image swap).

.DESCRIPTION
    Updates Azure Container Apps with a previously-pushed image tag. Performs health checks,
    verifies sticky sessions, runs smoke tests, and auto-rolls back on failure.

    Images must already exist in ghcr.io (built by Build-Images.ps1 or CI).

.PARAMETER Tag
    Docker image tag to deploy. Must already exist in ghcr.io.

.PARAMETER GithubRegistryUsername
    GitHub organization username for ghcr.io. Defaults to 'techhubms'.

.PARAMETER SkipSmokeTests
    Skip running smoke tests after deployment.

.EXAMPLE
    ./scripts/Deploy-Application.ps1 -Tag "20260501120000"
    Deploy the given image tag to production Container Apps.

.EXAMPLE
    ./scripts/Deploy-Application.ps1 -Tag "20260501120000" -SkipSmokeTests
    Deploy without running post-deployment smoke tests.
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$Tag,

    [Parameter(Mandatory = $false)]
    [string]$GithubRegistryUsername = 'techhubms',

    [switch]$SkipSmokeTests
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# ============================================================================
# CONFIGURATION
# ============================================================================

$registryServer = "ghcr.io"
$apiImage = "$registryServer/$GithubRegistryUsername/techhub-api"
$webImage = "$registryServer/$GithubRegistryUsername/techhub-web"
$apiAppName = "ca-techhub-api-prod"
$webAppName = "ca-techhub-web-prod"
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
Write-Host "  TechHub Application Deployment" -ForegroundColor White
Write-Host "  Tag                 : $Tag" -ForegroundColor Gray
Write-Host "  Registry            : $registryServer" -ForegroundColor Gray
Write-Host "  API Container App   : $apiAppName" -ForegroundColor Gray
Write-Host "  Web Container App   : $webAppName" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan

# ============================================================================
# PREREQUISITES
# ============================================================================

Write-Step "Validating prerequisites"

$account = az account show -o json 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Not logged in to Azure CLI. Run 'az login' first."
    exit 1
}
$accountInfo = $account | ConvertFrom-Json
Write-Ok "Azure CLI authenticated (subscription: $($accountInfo.name))"

# ============================================================================
# DEPLOY
# ============================================================================

# Save current production images for rollback
$previousApiImage = az containerapp show `
    --name $apiAppName `
    --resource-group $resourceGroup `
    --query "properties.template.containers[0].image" `
    -o tsv 2>$null
$previousWebImage = az containerapp show `
    --name $webAppName `
    --resource-group $resourceGroup `
    --query "properties.template.containers[0].image" `
    -o tsv 2>$null

if ($previousApiImage) {
    Write-Detail "Previous API image: $previousApiImage"
    Write-Detail "Previous Web image: $previousWebImage"
}

Write-Step "Deploying to production (tag: $Tag)"

# Update API container app
Write-Detail "Deploying API..."
az containerapp update `
    --name $apiAppName `
    --resource-group $resourceGroup `
    --image "$($apiImage):$Tag" `
    --revision-suffix "api-$Tag"
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Failed to deploy API"
    exit 1
}
Write-Ok "API deployed"

# Wait for API to become healthy before deploying Web
Write-Detail "Waiting for API to become healthy..."
$apiFqdn = az containerapp show `
    --name $apiAppName `
    --resource-group $resourceGroup `
    --query properties.configuration.ingress.fqdn `
    -o tsv 2>$null

if ($apiFqdn) {
    $maxRetries = 30
    $retryCount = 0
    $apiHealthy = $false

    while ($retryCount -lt $maxRetries -and -not $apiHealthy) {
        $retryCount++
        $healthResponse = try {
            Invoke-WebRequest -Uri "https://$apiFqdn/health" -TimeoutSec 10 -UseBasicParsing
        } catch { $null }

        if ($healthResponse -and $healthResponse.StatusCode -eq 200) {
            $apiHealthy = $true
            Write-Ok "API health check passed (attempt $retryCount/$maxRetries)"
        } else {
            Write-Detail "API not healthy yet (attempt $retryCount/$maxRetries), waiting 5 seconds..."
            Start-Sleep -Seconds 5
        }
    }

    if (-not $apiHealthy) {
        Write-Fail "API failed to become healthy after $maxRetries attempts"
        exit 1
    }
} else {
    Write-Warning "Could not get API FQDN, skipping health check"
}

# Update Web container app
Write-Detail "Deploying Web..."
az containerapp update `
    --name $webAppName `
    --resource-group $resourceGroup `
    --image "$($webImage):$Tag" `
    --revision-suffix "web-$Tag" `
    --set-env-vars "DEPLOY_IMAGE_TAG=$Tag"
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Failed to deploy Web"
    exit 1
}
Write-Ok "Web deployed"

$webFqdn = az containerapp show `
    --name $webAppName `
    --resource-group $resourceGroup `
    --query properties.configuration.ingress.fqdn `
    -o tsv 2>$null

if ([string]::IsNullOrWhiteSpace($webFqdn)) {
    Write-Fail "Could not retrieve Web Container App FQDN"
    exit 1
}

# Verify sticky sessions are still enabled after the update
Write-Step "Verifying sticky sessions on Web Container App"
$stickySetTimeoutSeconds = 90
$stickySetRetryDelaySeconds = 5
$stickySetDeadline = (Get-Date).AddSeconds($stickySetTimeoutSeconds)
$stickySessionsEnabled = $false
while ((Get-Date) -lt $stickySetDeadline) {
    $stickySetOutput = az containerapp ingress sticky-sessions set `
        --name $webAppName `
        --resource-group $resourceGroup `
        --affinity sticky 2>&1
    if ($LASTEXITCODE -eq 0) {
        $stickySessionsEnabled = $true
        break
    }

    $stickySetOutputText = ($stickySetOutput | Out-String).Trim()
    if ($stickySetOutputText -match 'ContainerAppOperationInProgress') {
        Write-Warn "Container App operation still in progress — retrying in $($stickySetRetryDelaySeconds)s"
        Start-Sleep -Seconds $stickySetRetryDelaySeconds
        continue
    }

    Write-Fail "Failed to set sticky sessions (exit code $LASTEXITCODE)"
    if (-not [string]::IsNullOrWhiteSpace($stickySetOutputText)) {
        Write-Detail $stickySetOutputText
    }
    exit 1
}
if (-not $stickySessionsEnabled) {
    Write-Fail "Failed to set sticky sessions within $($stickySetTimeoutSeconds)s"
    exit 1
}

# Poll until sticky sessions are confirmed active
$stickyConfirmDeadline = (Get-Date).AddSeconds(60)
$stickyConfirmed = $false
while ((Get-Date) -lt $stickyConfirmDeadline) {
    $affinity = az containerapp show `
        --name $webAppName `
        --resource-group $resourceGroup `
        --query "properties.configuration.ingress.stickySessions.affinity" `
        -o tsv 2>$null
    if ($affinity -eq 'sticky') {
        $stickyConfirmed = $true
        break
    }
    Start-Sleep -Seconds 5
}
if (-not $stickyConfirmed) {
    Write-Fail "Sticky sessions did not propagate within 60s — Blazor SignalR will not work correctly"
    exit 1
}
Write-Ok "Sticky sessions confirmed active (affinity=sticky)"

# Delegate version wait and smoke tests to the shared script
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$waitArgs = @('-WebFqdn', $webFqdn, '-Tag', $Tag)
if ($SkipSmokeTests) { $waitArgs += '-SkipSmokeTests' }
& (Join-Path $scriptDir 'Wait-ForLiveVersion.ps1') @waitArgs
if ($LASTEXITCODE -ne 0) {
    # Rollback on failure
    if ($previousApiImage -and $previousWebImage) {
        Write-Step "Rolling back to previous version"
        az containerapp update `
            --name $apiAppName `
            --resource-group $resourceGroup `
            --image $previousApiImage | Out-Null
        az containerapp update `
            --name $webAppName `
            --resource-group $resourceGroup `
            --image $previousWebImage | Out-Null
        Write-Warn "Rollback complete. Previous images restored."
    }
    exit 1
}

# ============================================================================
# SUMMARY
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  Application Deployment Complete" -ForegroundColor Green
Write-Host "  Tag                  : $Tag" -ForegroundColor Gray
Write-Host "  API image            : $($apiImage):$Tag" -ForegroundColor Gray
Write-Host "  Web image            : $($webImage):$Tag" -ForegroundColor Gray
Write-Host "  Web URL              : https://$webFqdn" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""
