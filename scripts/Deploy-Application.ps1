#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Builds, pushes, and deploys TechHub application containers.

.DESCRIPTION
    Builds Docker images for the API and Web applications, pushes them to GitHub Container
    Registry (ghcr.io), and deploys them to Azure Container Apps (production).

    When run locally, images are tagged with a UTC datetime (yyyyMMddHHmmss) by default for
    human-readable versioning. The same format is used in GitHub Actions CI.

.PARAMETER Tag
    Docker image tag. Defaults to UTC datetime (yyyyMMddHHmmss) for human-readable versioning.

.PARAMETER GithubRegistryUsername
    GitHub organization username for ghcr.io. Defaults to 'techhubms'.

.PARAMETER SkipBuild
    Skip building Docker images (use existing images in the registry).

.PARAMETER SkipPush
    Skip pushing images to ghcr.io. Useful for local-only testing with docker compose.

.PARAMETER SkipDeploy
    Skip deploying to Container Apps (build and push only).

.PARAMETER SkipSmokeTests
    Skip running smoke tests after deployment.

.EXAMPLE
    ./scripts/Deploy-Application.ps1
    Build with datetime tag, push to ghcr.io, and deploy to production.

.EXAMPLE
    ./scripts/Deploy-Application.ps1 -Tag "20260501120000"
    Build with specific tag, push, and deploy to production.

.EXAMPLE
    ./scripts/Deploy-Application.ps1 -SkipDeploy
    Build and push images only (no container app update).

.EXAMPLE
    ./scripts/Deploy-Application.ps1 -SkipBuild -SkipPush
    Deploy previously pushed images to production (build and push happened earlier).
#>

param(
    [Parameter(Mandatory = $false)]
    [string]$Tag,

    [Parameter(Mandatory = $false)]
    [string]$GithubRegistryUsername = 'techhubms',

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

# Resource names
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
Write-Host "  Source              : $(if ($isCI) { 'CI (GitHub Actions)' } else { 'Local' })" -ForegroundColor Gray
Write-Host "  Steps               : $(if ($SkipBuild) { '-' } else { 'Build' }) $(if ($SkipPush) { '-' } else { 'Push' }) $(if ($SkipDeploy) { '-' } else { 'Deploy' })" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan

# ============================================================================
# PREREQUISITES
# ============================================================================

Write-Step "Validating prerequisites"

if (-not $SkipPush) {
    # Authenticate with GitHub Container Registry
    # In CI: use GITHUB_TOKEN. Locally: use GHCR_PAT or GITHUB_TOKEN.
    $ghcrToken = if ($env:GITHUB_TOKEN) { $env:GITHUB_TOKEN } else { $env:GHCR_PAT }
    if (-not $ghcrToken) {
        Write-Fail "Neither GITHUB_TOKEN nor GHCR_PAT is set. Cannot authenticate with ghcr.io."
        Write-Detail "In CI: GITHUB_TOKEN is available automatically."
        Write-Detail "Locally: set GHCR_PAT to a GitHub PAT with write:packages scope."
        exit 1
    }

    Write-Step "Authenticating with GitHub Container Registry"
    # When GITHUB_TOKEN is used (CI), Docker username must match the workflow actor (token owner).
    # When GHCR_PAT is used (local), the PAT owner is always the person who created the token,
    # so use GITHUB_ACTOR if set, otherwise fall back to GithubRegistryUsername.
    $loginUsername = if ($env:GITHUB_ACTOR) { $env:GITHUB_ACTOR } else { $GithubRegistryUsername }
    $ghcrToken | docker login ghcr.io --username $loginUsername --password-stdin
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to authenticate with ghcr.io"
        exit 1
    }
    Write-Ok "Authenticated with $registryServer"
}

if (-not $SkipDeploy) {
    # Check Azure CLI login
    $account = az account show -o json 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Not logged in to Azure CLI. Run 'az login' first."
        exit 1
    }
    $accountInfo = $account | ConvertFrom-Json
    Write-Ok "Azure CLI authenticated (subscription: $($accountInfo.name))"
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
    Write-Step "Pushing images to ghcr.io"

    # Push API
    Write-Detail "Pushing API image..."
    docker push "$($apiImage):$Tag"
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to push API image (tag: $Tag)"
        exit 1
    }
    Write-Ok "API image pushed"

    # Push Web
    Write-Detail "Pushing Web image..."
    docker push "$($webImage):$Tag"
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to push Web image (tag: $Tag)"
        exit 1
    }
    Write-Ok "Web image pushed"
}
else {
    Write-Step "Skipping push"
}

# ============================================================================
# DEPLOY
# ============================================================================

if (-not $SkipDeploy) {
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

    # Verify sticky sessions are still enabled after the update.
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

    # Poll until sticky sessions are confirmed active in the resource API.
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

    # Delegate version wait and smoke tests to the shared script.
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
Write-Host "  Tag                  : $Tag" -ForegroundColor Gray
Write-Host "  API image            : $($apiImage):$Tag" -ForegroundColor Gray
Write-Host "  Web image            : $($webImage):$Tag" -ForegroundColor Gray

if (-not $SkipDeploy) {
    if ($webFqdn) {
        Write-Host "  Web URL     : https://$webFqdn" -ForegroundColor Gray
    }
}

Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""
