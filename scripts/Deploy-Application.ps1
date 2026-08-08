#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Deploys TechHub containers to the production Azure App Service Plan (imperative image swap).

.DESCRIPTION
    Updates the API and Web sites (Web App for Containers) with a previously-pushed image tag.
    Performs health checks, runs smoke tests, and auto-rolls back on failure.

    Images must already exist in ghcr.io (built by Build-Images.ps1 or CI).

.PARAMETER Tag
    Docker image tag to deploy. Must already exist in ghcr.io.

.PARAMETER GithubRegistryUsername
    GitHub organization username for ghcr.io. Defaults to 'techhubms'.

.PARAMETER SkipSmokeTests
    Skip running smoke tests after deployment.

.EXAMPLE
    ./scripts/Deploy-Application.ps1 -Tag "20260501120000"
    Deploy the given image tag to the production App Service sites.

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
$apiAppName = "app-techhub-api-prod"
$webAppName = "app-techhub-web-prod"
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
Write-Host "  API Web App         : $apiAppName" -ForegroundColor Gray
Write-Host "  Web App             : $webAppName" -ForegroundColor Gray
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

# Save current production linuxFxVersion (DOCKER|<image>) for rollback
$previousApiFxVersion = az webapp config show `
    --name $apiAppName `
    --resource-group $resourceGroup `
    --query "linuxFxVersion" `
    --only-show-errors `
    -o tsv
$previousWebFxVersion = az webapp config show `
    --name $webAppName `
    --resource-group $resourceGroup `
    --query "linuxFxVersion" `
    --only-show-errors `
    -o tsv

if ($previousApiFxVersion) {
    Write-Detail "Previous API image: $previousApiFxVersion"
    Write-Detail "Previous Web image: $previousWebFxVersion"
}

Write-Step "Deploying to production (tag: $Tag)"

# Update API site — changing linuxFxVersion swaps the container image and restarts the site
Write-Detail "Deploying API..."
az webapp config set `
    --name $apiAppName `
    --resource-group $resourceGroup `
    --linux-fx-version "DOCKER|$($apiImage):$Tag" | Out-Null
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Failed to deploy API"
    exit 1
}
Write-Ok "API deployed"

# Wait for API to become healthy before deploying Web
Write-Detail "Waiting for API to become healthy..."
$apiFqdn = az webapp show `
    --name $apiAppName `
    --resource-group $resourceGroup `
    --query defaultHostName `
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
    Write-Warning "Could not get API hostname, skipping health check"
}

# Update Web site
Write-Detail "Deploying Web..."
az webapp config set `
    --name $webAppName `
    --resource-group $resourceGroup `
    --linux-fx-version "DOCKER|$($webImage):$Tag" | Out-Null
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Failed to deploy Web"
    exit 1
}
az webapp config appsettings set `
    --name $webAppName `
    --resource-group $resourceGroup `
    --settings "DEPLOY_IMAGE_TAG=$Tag" | Out-Null
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Failed to set DEPLOY_IMAGE_TAG app setting"
    exit 1
}
Write-Ok "Web deployed"

$webFqdn = az webapp show `
    --name $webAppName `
    --resource-group $resourceGroup `
    --query defaultHostName `
    -o tsv 2>$null

if ([string]::IsNullOrWhiteSpace($webFqdn)) {
    Write-Fail "Could not retrieve Web App hostname"
    exit 1
}

# Delegate version wait and smoke tests to the shared script
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$waitArgs = @('-WebFqdn', $webFqdn, '-Tag', $Tag)
if ($SkipSmokeTests) { $waitArgs += '-SkipSmokeTests' }
& (Join-Path $scriptDir 'Wait-ForLiveVersion.ps1') @waitArgs
if ($LASTEXITCODE -ne 0) {
    # Rollback on failure
    if ($previousApiFxVersion -and $previousWebFxVersion) {
        Write-Step "Rolling back to previous version"
        az webapp config set `
            --name $apiAppName `
            --resource-group $resourceGroup `
            --linux-fx-version $previousApiFxVersion | Out-Null
        az webapp config set `
            --name $webAppName `
            --resource-group $resourceGroup `
            --linux-fx-version $previousWebFxVersion | Out-Null
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
