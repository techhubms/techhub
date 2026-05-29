#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Builds and pushes TechHub Docker images to GitHub Container Registry.

.DESCRIPTION
    Builds Docker images for the API and Web applications and pushes them to ghcr.io.
    Used by the CI/CD build-and-push job. Can also be run locally for manual image builds.

.PARAMETER Tag
    Docker image tag. Defaults to UTC datetime (yyyyMMddHHmmss) for human-readable versioning.

.PARAMETER GithubRegistryUsername
    GitHub organization username for ghcr.io. Defaults to 'techhubms'.

.PARAMETER GithubRegistryAuthUsername
    GitHub username of the PAT owner used to authenticate with ghcr.io. Required when GHCR_PAT
    is used locally and GITHUB_ACTOR is not set. In CI, GITHUB_ACTOR is used automatically.

.PARAMETER SkipPush
    Skip pushing images to ghcr.io. Useful for local-only testing with docker compose.

.EXAMPLE
    ./scripts/Build-Images.ps1
    Build with datetime tag and push to ghcr.io.

.EXAMPLE
    ./scripts/Build-Images.ps1 -Tag "20260501120000"
    Build with specific tag and push.

.EXAMPLE
    ./scripts/Build-Images.ps1 -SkipPush
    Build images locally without pushing (for docker compose testing).
#>

param(
    [Parameter(Mandatory = $false)]
    [string]$Tag,

    [Parameter(Mandatory = $false)]
    [string]$GithubRegistryUsername = 'techhubms',

    [Parameter(Mandatory = $false)]
    [string]$GithubRegistryAuthUsername = '',

    [switch]$SkipPush
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# ============================================================================
# CONFIGURATION
# ============================================================================

$workspaceRoot = if (Test-Path (Join-Path $PSScriptRoot "../src")) {
    (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
} else {
    $PSScriptRoot
}

$isCI = $null -ne $env:GITHUB_ACTIONS

if (-not $Tag) {
    $Tag = Get-Date -Format 'yyyyMMddHHmmss'
}

$registryServer = "ghcr.io"
$apiImage = "$registryServer/$GithubRegistryUsername/techhub-api"
$webImage = "$registryServer/$GithubRegistryUsername/techhub-web"

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

function Invoke-DockerPush {
    param(
        [string]$ImageRef,
        [int]$MaxAttempts = 3,
        [int]$RetryDelaySecs = 15
    )
    for ($attempt = 1; $attempt -le $MaxAttempts; $attempt++) {
        docker push $ImageRef
        if ($LASTEXITCODE -eq 0) { return $true }
        if ($attempt -lt $MaxAttempts) {
            Write-Warn "Push failed (attempt $attempt/$MaxAttempts) — retrying in ${RetryDelaySecs}s (known GHCR transient issue)"
            Start-Sleep -Seconds $RetryDelaySecs
        }
    }
    return $false
}

# ============================================================================
# BANNER
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  TechHub Image Build" -ForegroundColor White
Write-Host "  Tag                 : $Tag" -ForegroundColor Gray
Write-Host "  Registry            : $registryServer" -ForegroundColor Gray
Write-Host "  Source              : $(if ($isCI) { 'CI (GitHub Actions)' } else { 'Local' })" -ForegroundColor Gray
Write-Host "  Steps               : Build $(if ($SkipPush) { '-' } else { 'Push' })" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan

# ============================================================================
# AUTHENTICATE
# ============================================================================

if (-not $SkipPush) {
    $ghcrToken = if ($env:GITHUB_TOKEN) { $env:GITHUB_TOKEN } else { $env:GHCR_PAT }
    if (-not $ghcrToken) {
        Write-Fail "Neither GITHUB_TOKEN nor GHCR_PAT is set. Cannot authenticate with ghcr.io."
        Write-Detail "In CI: GITHUB_TOKEN is available automatically."
        Write-Detail "Locally: set GHCR_PAT to a GitHub PAT with write:packages scope."
        exit 1
    }

    Write-Step "Authenticating with GitHub Container Registry"
    $loginUsername = if ($env:GITHUB_ACTOR) {
        $env:GITHUB_ACTOR
    }
    elseif (-not [string]::IsNullOrWhiteSpace($GithubRegistryAuthUsername)) {
        $GithubRegistryAuthUsername
    }
    else {
        Write-Fail "Cannot determine ghcr.io login username: GITHUB_ACTOR is not set and -GithubRegistryAuthUsername was not provided."
        Write-Detail "Locally: set GHCR_PAT and pass -GithubRegistryAuthUsername <your-github-username>."
        exit 1
    }
    $ghcrToken | docker login ghcr.io --username $loginUsername --password-stdin
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to authenticate with ghcr.io"
        exit 1
    }
    Write-Ok "Authenticated with $registryServer"
}

# ============================================================================
# BUILD
# ============================================================================

Write-Step "Building Docker images (tag: $Tag)"

Push-Location $workspaceRoot
try {
    Write-Detail "Building API image..."
    docker build -f src/TechHub.Api/Dockerfile `
        -t "$($apiImage):$Tag" `
        .
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to build API image"
        exit 1
    }
    Write-Ok "API image: $($apiImage):$Tag"

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

# ============================================================================
# PUSH
# ============================================================================

if (-not $SkipPush) {
    Write-Step "Pushing images to ghcr.io"

    Write-Detail "Pushing API image..."
    if (-not (Invoke-DockerPush "$($apiImage):$Tag")) {
        Write-Fail "Failed to push API image after 3 attempts (tag: $Tag)"
        exit 1
    }
    Write-Ok "API image pushed"

    Write-Detail "Pushing Web image..."
    if (-not (Invoke-DockerPush "$($webImage):$Tag")) {
        Write-Fail "Failed to push Web image after 3 attempts (tag: $Tag)"
        exit 1
    }
    Write-Ok "Web image pushed"
}
else {
    Write-Step "Skipping push"
}

# ============================================================================
# SUMMARY
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  Image Build Complete" -ForegroundColor Green
Write-Host "  Tag                  : $Tag" -ForegroundColor Gray
Write-Host "  API image            : $($apiImage):$Tag" -ForegroundColor Gray
Write-Host "  Web image            : $($webImage):$Tag" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""

# Output for CI consumption
if ($isCI) {
    "image-tag=$Tag" >> $env:GITHUB_OUTPUT
}
