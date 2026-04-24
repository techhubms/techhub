#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Builds, pushes, and deploys TechHub application containers.

.DESCRIPTION
    Builds Docker images for the API and Web applications, pushes them to Azure Container Registry,
    and deploys them to Azure Container Apps. Supports staging and production environments.

    When run locally, images are tagged with a UTC datetime (yyyyMMddHHmmss) by default for
    human-readable versioning. The same format is used in GitHub Actions CI.

.PARAMETER Environment
    Target environment: staging or production.

.PARAMETER Tag
    Docker image tag. Defaults to UTC datetime (yyyyMMddHHmmss) for human-readable versioning.

.PARAMETER RegistryName
    Azure Container Registry name (without .azurecr.io). Defaults to 'crtechhubms'.

.PARAMETER SkipBuild
    Skip building Docker images (use existing images in the registry).

.PARAMETER SkipPush
    Skip pushing images to ACR. Useful for local-only testing with docker compose.

.PARAMETER SkipDeploy
    Skip deploying to Container Apps (build and push only).

.PARAMETER SkipSmokeTests
    Skip running smoke tests after deployment.

.EXAMPLE
    ./scripts/Deploy-Application.ps1 -Environment staging
    Build with 'dev' tag, push, and deploy to staging.

.EXAMPLE
    ./scripts/Deploy-Application.ps1 -Environment staging -Tag "v1.0.0"
    Build with specific tag, push, and deploy to staging.

.EXAMPLE
    ./scripts/Deploy-Application.ps1 -Environment staging -SkipDeploy
    Build and push images only (no container app update).

.EXAMPLE
    ./scripts/Deploy-Application.ps1 -Environment staging -SkipBuild -SkipPush
    Deploy previously pushed images to staging (build and push happened earlier).

.EXAMPLE
    ./scripts/Deploy-Application.ps1 -Environment production -Tag "abc123def" -SkipBuild -SkipPush
    Deploy a specific previously-built tag to production.
#>

param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('staging', 'production')]
    [string]$Environment,

    [Parameter(Mandatory = $false)]
    [string]$Tag,

    [Parameter(Mandatory = $false)]
    [string]$RegistryName = 'crtechhubms',

    [switch]$SkipBuild,
    [switch]$SkipPush,
    [switch]$SkipDeploy,
    [switch]$SkipSmokeTests
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# ============================================================================
# CONFIGURATION
# ============================================================================

# Resolve workspace root (support running from scripts/ or repo root)
$workspaceRoot = if (Test-Path (Join-Path $PSScriptRoot "../src")) {
    (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
} else {
    $PSScriptRoot
}

# Detect CI vs local
$isCI = $null -ne $env:GITHUB_ACTIONS

# Resolve default tag: datetime-based for human-readable versioning
if (-not $Tag) {
    $Tag = Get-Date -Format 'yyyyMMddHHmmss'
}

# Environment suffix mapping (staging -> staging, production -> prod)
$envSuffix = if ($Environment -eq 'production') { 'prod' } else { $Environment }

# Resource names
$registryServer = "$($RegistryName).azurecr.io"
$apiImage = "$registryServer/techhub-api"
$webImage = "$registryServer/techhub-web"
$apiAppName = "ca-techhub-api-$envSuffix"
$webAppName = "ca-techhub-web-$envSuffix"
$resourceGroup = "rg-techhub-$envSuffix"

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
Write-Host "  Environment         : $Environment" -ForegroundColor Gray
Write-Host "  Tag                 : $Tag" -ForegroundColor Gray
Write-Host "  Registry            : $registryServer" -ForegroundColor Gray
Write-Host "  Source              : $(if ($isCI) { 'CI (GitHub Actions)' } else { 'Local' })" -ForegroundColor Gray
Write-Host "  Steps               : $(if ($SkipBuild) { '-' } else { 'Build' }) $(if ($SkipPush) { '-' } else { 'Push' }) $(if ($SkipDeploy) { '-' } else { 'Deploy' })" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan

# ============================================================================
# PREREQUISITES
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

# ACR login (needed for push; also needed for build if pulling base images through ACR)
if (-not $SkipBuild -or -not $SkipPush) {
    Write-Step "Authenticating with Azure Container Registry"
    az acr login --name $RegistryName
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to authenticate with ACR '$RegistryName'"
        exit 1
    }
    Write-Ok "Authenticated with $registryServer"
}

# ============================================================================
# BUILD
# ============================================================================

if (-not $SkipBuild) {
    Write-Step "Building Docker images (tag: $Tag)"

    Push-Location $workspaceRoot
    try {
        # Build API image
        Write-Detail "Building API image..."
        docker build -f src/TechHub.Api/Dockerfile `
            -t "$($apiImage):$Tag" `
            .
        if ($LASTEXITCODE -ne 0) {
            Write-Fail "Failed to build API image"
            exit 1
        }
        Write-Ok "API image: $($apiImage):$Tag"

        # Build Web image
        Write-Detail "Building Web image..."
        docker build -f src/TechHub.Web/Dockerfile `
            -t "$($webImage):$Tag" `
            .
        if ($LASTEXITCODE -ne 0) {
            Write-Fail "Failed to build Web image"
            exit 1
        }
        Write-Ok "Web image: $($webImage):$Tag"
    }
    finally {
        Pop-Location
    }
}
else {
    Write-Step "Skipping build (using existing images)"
}

# ============================================================================
# PUSH
# ============================================================================

if (-not $SkipPush) {
    Write-Step "Pushing images to ACR"

    # --- Temporarily add this machine's IP to the ACR firewall ---
    # ACR uses Standard SKU (Premium is required for private endpoints — not cost-effective).
    # Instead we restrict to admin IPs via networkRuleSet and add/remove the runner IP on the fly.
    # Try multiple providers so a transient outage of one doesn't break the entire deployment.
    $acrCurrentIp = $null
    foreach ($provider in @('https://checkip.amazonaws.com', 'https://api.ipify.org', 'https://icanhazip.com')) {
        try {
            $response = (Invoke-RestMethod -Uri $provider -UseBasicParsing -TimeoutSec 10).Trim()
            if ($response -match '^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$') {
                $acrCurrentIp = $response
                break
            }
            Write-Warning "IP provider '$provider' returned unexpected response: '$response'"
        }
        catch {
            Write-Warning "IP provider '$provider' failed: $($_.Exception.Message)"
        }
    }
    if (-not $acrCurrentIp) {
        Write-Fail "Failed to detect outbound IP from any provider. Cannot add ACR firewall rule."
        exit 1
    }
    $acrIpCidr = "$acrCurrentIp/32"

    $acrExistingRules = az acr network-rule list `
        --name $RegistryName `
        --query 'ipRules[].value' `
        --output tsv 2>$null
    # Normalize and exact-match to avoid false positives (e.g. 1.2.3.4 matching 1.2.3.40/32).
    $acrExistingRuleValues = @()
    if ($acrExistingRules) {
        $acrExistingRuleValues = @(($acrExistingRules -split "`n") |
            ForEach-Object { $_.Trim() } |
            Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
    }
    $acrRuleAlreadyPresent = $acrExistingRuleValues -contains $acrCurrentIp -or $acrExistingRuleValues -contains $acrIpCidr

    $acrIpWasAdded = $false
    # The firewall rule add is inside the try block so the finally cleanup always runs if the
    # add succeeds but a subsequent push step fails.
    try {
        if (-not $acrRuleAlreadyPresent) {
            Write-Host "   Adding current IP $acrCurrentIp to ACR '$($RegistryName)' firewall..." -ForegroundColor Cyan
            az acr network-rule add `
                --name $RegistryName `
                --ip-address $acrIpCidr `
                --output none
            if ($LASTEXITCODE -ne 0) { throw "Failed to add IP $acrCurrentIp to ACR '$($RegistryName)' firewall." }
            $acrIpWasAdded = $true
            # Poll until the rule takes effect instead of a fixed sleep.
            Write-Host "   Waiting for firewall rule to propagate..." -ForegroundColor Gray
            $acrMaxWaitSecs = 60
            $acrElapsed = 0
            $acrPropagated = $false
            while ($acrElapsed -lt $acrMaxWaitSecs -and -not $acrPropagated) {
                Start-Sleep -Seconds 5
                $acrElapsed += 5
                az acr repository list --name $RegistryName --output none 2>$null
                if ($LASTEXITCODE -eq 0) {
                    $acrPropagated = $true
                    Write-Host "   Firewall rule active after ${acrElapsed}s." -ForegroundColor Gray
                }
                else {
                    Write-Host "   Still propagating (${acrElapsed}s / ${acrMaxWaitSecs}s)..." -ForegroundColor Gray
                }
            }
            if (-not $acrPropagated) {
                Write-Warning "Firewall rule may not have propagated after ${acrMaxWaitSecs}s — proceeding anyway."
            }
        }
        else {
            Write-Host "   IP $acrCurrentIp is already permitted by ACR '$($RegistryName)' firewall." -ForegroundColor Gray
        }

        # Push API
        Write-Detail "Pushing API image..."
        docker push "$($apiImage):$Tag"
        if ($LASTEXITCODE -ne 0) { throw "Failed to push API image (tag: $Tag)" }
        Write-Ok "API image pushed"

        # Push Web
        Write-Detail "Pushing Web image..."
        docker push "$($webImage):$Tag"
        if ($LASTEXITCODE -ne 0) { throw "Failed to push Web image (tag: $Tag)" }
        Write-Ok "Web image pushed"
    }
    catch {
        Write-Fail $_.Exception.Message
        exit 1
    }
    finally {
        if ($acrIpWasAdded) {
            Write-Host "   Removing current IP $acrCurrentIp from ACR '$($RegistryName)' firewall..." -ForegroundColor Cyan
            az acr network-rule remove `
                --name $RegistryName `
                --ip-address $acrIpCidr `
                --output none
            if ($LASTEXITCODE -ne 0) {
                Write-Warning "Failed to remove IP $acrCurrentIp from ACR firewall. Remove manually: az acr network-rule remove --name $RegistryName --ip-address $acrIpCidr"
            }
        }
    }
}
else {
    Write-Step "Skipping push"
}

# ============================================================================
# DEPLOY
# ============================================================================

if (-not $SkipDeploy) {
    # Production safety: validate staging health first
    if ($Environment -eq 'production') {
        Write-Step "Validating staging health before production deployment"

        $stagingWebFqdn = az containerapp show `
            --name ca-techhub-web-staging `
            --resource-group rg-techhub-staging `
            --query properties.configuration.ingress.fqdn `
            -o tsv 2>$null

        if ($stagingWebFqdn) {
            $response = try {
                Invoke-WebRequest -Uri "https://$stagingWebFqdn/health" -TimeoutSec 15 -UseBasicParsing
            }
            catch { $null }

            if ($response -and $response.StatusCode -eq 200) {
                Write-Ok "Staging health check passed"
            }
            else {
                Write-Fail "Staging health check failed. Fix staging before deploying to production."
                exit 1
            }
        }
        else {
            Write-Warn "Could not retrieve staging URL. Proceeding without staging validation."
        }

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
    }

    Write-Step "Deploying to $Environment (tag: $Tag)"

    # Update API container app (revision suffix matches Bicep convention: api-{tag})
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
        --revision-suffix "web-$Tag"
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to deploy Web"
        exit 1
    }
    Write-Ok "Web deployed"

    # Wait for stabilization
    $waitSeconds = if ($Environment -eq 'production') { 60 } else { 30 }
    Write-Detail "Waiting $waitSeconds seconds for deployment to stabilize before running smoke tests..."
    Start-Sleep -Seconds $waitSeconds

    # ============================================================================
    # SMOKE TESTS
    # ============================================================================

    if (-not $SkipSmokeTests) {
        Write-Step "Running smoke tests"

        $webFqdn = az containerapp show `
            --name $webAppName `
            --resource-group $resourceGroup `
            --query properties.configuration.ingress.fqdn `
            -o tsv 2>$null

        $smokeTestsPassed = $true

        if ($webFqdn) {
            # Test Web health endpoint
            $healthResponse = try {
                Invoke-WebRequest -Uri "https://$webFqdn/health" -TimeoutSec 30 -UseBasicParsing
            }
            catch { $null }

            if ($healthResponse -and $healthResponse.StatusCode -eq 200) {
                Write-Ok "Web health check passed (https://$webFqdn/health)"
            }
            else {
                Write-Fail "Web health check failed"
                $smokeTestsPassed = $false
            }

            # Test Web homepage
            $homepageResponse = try {
                Invoke-WebRequest -Uri "https://$webFqdn" -TimeoutSec 30 -UseBasicParsing
            }
            catch { $null }

            if ($homepageResponse -and $homepageResponse.StatusCode -eq 200) {
                Write-Ok "Web homepage accessible (https://$webFqdn)"
            }
            else {
                Write-Fail "Web homepage not accessible"
                $smokeTestsPassed = $false
            }
        }
        else {
            Write-Warn "Could not retrieve web URL for smoke tests"
            $smokeTestsPassed = $false
        }

        # Rollback on failure (production only)
        if (-not $smokeTestsPassed) {
            if ($Environment -eq 'production' -and $previousApiImage -and $previousWebImage) {
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
    }
}
else {
    Write-Step "Skipping deployment"
}

# ============================================================================
# SUMMARY
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  Application Deployment Complete" -ForegroundColor Green
Write-Host "  Environment          : $Environment" -ForegroundColor Gray
Write-Host "  Tag                  : $Tag" -ForegroundColor Gray
Write-Host "  API image            : $($apiImage):$Tag" -ForegroundColor Gray
Write-Host "  Web image            : $($webImage):$Tag" -ForegroundColor Gray

if (-not $SkipDeploy) {
    $webFqdn = az containerapp show `
        --name $webAppName `
        --resource-group $resourceGroup `
        --query properties.configuration.ingress.fqdn `
        -o tsv 2>$null
    if ($webFqdn) {
        Write-Host "  Web URL     : https://$webFqdn" -ForegroundColor Gray
    }
}

Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""
