#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Tears down all active PR preview environments in the staging resource group.

.DESCRIPTION
    Scans rg-techhub-staging for active PR preview environments by listing:
    - Container Apps matching ca-techhub-api-pr-{N}
    - PostgreSQL Flexible Servers matching psql-techhub-pr-{N}

    The dual-scan catches orphaned databases that have no matching Container App
    (e.g. from a partial teardown failure when a PR was closed).

    For each discovered PR number, calls Deploy-PrPreview.ps1 -Action teardown.

    Can be run locally (requires Azure CLI login) or from GitHub Actions.

.PARAMETER DryRun
    List discovered environments without deleting them.

.PARAMETER RegistryName
    Azure Container Registry name (without .azurecr.io). Defaults to 'crtechhubms'.

.EXAMPLE
    ./scripts/Teardown-PrEnvironments.ps1
    Tear down all active PR environments.

.EXAMPLE
    ./scripts/Teardown-PrEnvironments.ps1 -DryRun
    List active PR environments without removing them.
#>

param(
    [switch]$DryRun,

    [string]$RegistryName = 'crtechhubms'
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$stagingRG = 'rg-techhub-staging'

$scriptDir = $PSScriptRoot
$deployScript = Join-Path $scriptDir 'Deploy-PrPreview.ps1'

# ============================================================================
# HELPERS
# ============================================================================

function Write-Step {
    param([string]$Message)
    Write-Host ''
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

Write-Host ''
Write-Host '===============================================================' -ForegroundColor DarkCyan
Write-Host '  TechHub PR Environment Teardown' -ForegroundColor White
Write-Host "  Resource Group : $stagingRG" -ForegroundColor Gray
if ($DryRun) {
    Write-Host '  Mode           : DRY RUN (no changes)' -ForegroundColor Yellow
}
else {
    Write-Host '  Mode           : LIVE (environments will be deleted)' -ForegroundColor Red
}
Write-Host '===============================================================' -ForegroundColor DarkCyan

# ============================================================================
# PREREQUISITES
# ============================================================================

Write-Step 'Validating prerequisites'

$account = az account show -o json 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Not logged in to Azure CLI. Run 'az login' first."
    exit 1
}
$accountInfo = $account | ConvertFrom-Json
Write-Ok "Azure CLI authenticated (subscription: $($accountInfo.name))"

# ============================================================================
# DISCOVER
# ============================================================================

Write-Step "Scanning $stagingRG for active PR environments"

$prNumbers = [System.Collections.Generic.SortedSet[int]]::new()

# Discover from Container Apps (ca-techhub-api-pr-{N})
$caApps = az containerapp list `
    --resource-group $stagingRG `
    --query "[?starts_with(name, 'ca-techhub-api-pr-')].name" -o tsv 2>$null

if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($caApps)) {
    foreach ($app in ($caApps -split "`n")) {
        if ($app -match 'ca-techhub-api-pr-(\d+)') {
            [void]$prNumbers.Add([int]$Matches[1])
        }
    }
}
Write-Detail "Container Apps found  : $($prNumbers.Count) PR(s)"

# Discover from PostgreSQL servers (psql-techhub-pr-{N})
# Catches orphaned databases with no matching Container App
$pgServers = az postgres flexible-server list `
    --resource-group $stagingRG `
    --query "[?starts_with(name, 'psql-techhub-pr-')].name" -o tsv 2>$null

if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($pgServers)) {
    foreach ($server in ($pgServers -split "`n")) {
        if ($server -match 'psql-techhub-pr-(\d+)') {
            [void]$prNumbers.Add([int]$Matches[1])
        }
    }
}
Write-Detail "After Postgres scan    : $($prNumbers.Count) unique PR(s)"

if ($prNumbers.Count -eq 0) {
    Write-Host ''
    Write-Ok "No active PR environments found in $stagingRG — nothing to do."
    exit 0
}

$prList = $prNumbers -join ', '
Write-Ok "Found $($prNumbers.Count) active PR environment(s): $prList"

if ($DryRun) {
    Write-Host ''
    Write-Host "Dry run — would teardown PR(s): $prList" -ForegroundColor Yellow
    exit 0
}

# ============================================================================
# TEARDOWN
# ============================================================================

$failures = [System.Collections.Generic.List[int]]::new()

foreach ($prNum in $prNumbers) {
    Write-Host ''
    Write-Host "━━━ Tearing down PR #$prNum ━━━" -ForegroundColor Cyan
    & $deployScript -PrNumber $prNum -Action teardown -RegistryName $RegistryName
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to teardown PR #$prNum"
        $failures.Add($prNum)
    }
}

Write-Host ''

if ($failures.Count -gt 0) {
    Write-Fail "Teardown completed with errors. Failed PR(s): $($failures -join ', ')"
    exit 1
}

Write-Ok "All $($prNumbers.Count) PR environment(s) torn down successfully."
