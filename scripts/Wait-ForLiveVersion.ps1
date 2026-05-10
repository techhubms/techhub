#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Waits until a deployed web Container App is serving a specific version, then runs smoke tests.

.DESCRIPTION
    Polls the /version endpoint on the web FQDN until it reports the expected image tag, then
    optionally runs basic smoke tests (health check + homepage). Used by both Deploy-Application.ps1
    and Deploy-PrPreview.ps1 to provide a single, tested readiness gate after each deployment.

    For PR environments (minReplicas=0) the /version poll also acts as the warmup trigger: the
    web Container App only starts serving once its Kestrel startup completes (which is blocked
    until the API responds to the section-cache pre-load). So if /version returns the expected
    tag, the full Web → API → DB chain is confirmed healthy.

.PARAMETER WebFqdn
    The FQDN of the web Container App, without scheme (e.g. ca-techhub-web-pr-42.domain.io).

.PARAMETER Tag
    The image tag to wait for (e.g. 20260509120000 or pr-406-20260509120000).

.PARAMETER MaxWaitSeconds
    Maximum time in seconds to wait for the new version to appear. Defaults to 300 (5 minutes).

.PARAMETER SkipSmokeTests
    When set, skip the smoke tests after version confirmation.

.EXAMPLE
    ./scripts/Wait-ForLiveVersion.ps1 -WebFqdn "myapp.region.azurecontainerapps.io" -Tag "20260509120000"

.EXAMPLE
    ./scripts/Wait-ForLiveVersion.ps1 -WebFqdn $webFqdn -Tag $Tag -SkipSmokeTests
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$WebFqdn,

    [Parameter(Mandatory = $true)]
    [string]$Tag,

    [Parameter(Mandatory = $false)]
    [int]$MaxWaitSeconds = 300,

    [Parameter(Mandatory = $false)]
    [switch]$SkipSmokeTests
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

function Write-Step  { param([string]$Message); Write-Host ""; Write-Host "=> $Message" -ForegroundColor Cyan }
function Write-Ok    { param([string]$Message); Write-Host "   [OK] $Message" -ForegroundColor Green }
function Write-Warn  { param([string]$Message); Write-Host "   [WARN] $Message" -ForegroundColor Yellow }
function Write-Fail  { param([string]$Message); Write-Host "   [FAIL] $Message" -ForegroundColor Red }
function Write-Detail{ param([string]$Message); Write-Host "   $Message" -ForegroundColor Gray }

# ============================================================================
# VERSION WAIT
# ============================================================================

Write-Step "Waiting for version $Tag to become live at https://$WebFqdn/version"

$retryDelaySeconds = 5
$maxAttempts       = [Math]::Ceiling($MaxWaitSeconds / $retryDelaySeconds)
$attempt           = 0
$versionLive       = $false

while ($attempt -lt $maxAttempts) {
    $attempt++
    try {
        $response = Invoke-WebRequest `
            -Uri     "https://$WebFqdn/version?t=$Tag" `
            -TimeoutSec 10 `
            -UseBasicParsing `
            -Headers @{ "Cache-Control" = "no-cache" } `
            -ErrorAction Stop

        if ($response.StatusCode -eq 200) {
            $json = $response.Content | ConvertFrom-Json
            if ($json.tag -eq $Tag) {
                $versionLive = $true
                Write-Ok "Version $Tag is live after $($attempt * $retryDelaySeconds)s"
                break
            }
            Write-Detail "Still on '$($json.tag)' (attempt $attempt/$maxAttempts) — waiting ${retryDelaySeconds}s..."
        }
    }
    catch {
        Write-Detail "Not yet responding (attempt $attempt/$maxAttempts) — waiting ${retryDelaySeconds}s..."
    }
    Start-Sleep -Seconds $retryDelaySeconds
}

if (-not $versionLive) {
    Write-Fail "Version $Tag did not become live within ${MaxWaitSeconds}s"
    exit 1
}

if ($SkipSmokeTests) {
    Write-Ok "Smoke tests skipped"
    exit 0
}

# ============================================================================
# SMOKE TESTS
# ============================================================================

Write-Step "Running smoke tests against https://$WebFqdn"

$smokeTestsPassed = $true

# Health check — validates Kestrel and ASP.NET Core health middleware
$healthResponse = try {
    Invoke-WebRequest -Uri "https://$WebFqdn/health" -TimeoutSec 30 -UseBasicParsing
}
catch { $null }

if ($healthResponse -and $healthResponse.StatusCode -eq 200) {
    Write-Ok "Health check passed"
}
else {
    Write-Fail "Health check failed"
    $smokeTestsPassed = $false
}

# Homepage — validates the full Web → API → DB chain (SSR page load calls the API)
$homepageResponse = try {
    Invoke-WebRequest -Uri "https://$WebFqdn/" -TimeoutSec 30 -UseBasicParsing
}
catch { $null }

if ($homepageResponse -and $homepageResponse.StatusCode -eq 200) {
    Write-Ok "Homepage accessible — full Web → API → DB chain confirmed"
}
else {
    Write-Fail "Homepage not accessible"
    $smokeTestsPassed = $false
}

if (-not $smokeTestsPassed) {
    exit 1
}

Write-Ok "All smoke tests passed"
exit 0
