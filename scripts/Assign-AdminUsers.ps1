#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Assigns users to the TechHub enterprise app so they can access admin pages.

.DESCRIPTION
    Idempotent script that ensures every user in the list is assigned to the
    TechHub Entra ID enterprise app. Skips users that are already assigned.
    Requires Azure CLI (`az`) logged in with permissions to read users and
    manage app role assignments.

.PARAMETER Environment
    Target environment whose enterprise app to update.
    Determines the app registration display name to look up.

.EXAMPLE
    ./scripts/Assign-AdminUsers.ps1 -Environment production
#>

param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('localhost', 'staging', 'production')]
    [string]$Environment
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# ============================================================================
# ADMIN USERS — add or remove email addresses here
# ============================================================================

$AdminUsers = @(
    'reinier.vanmaanen@xebia.com'
    'rob.bos@xebia.com'
)

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

# ============================================================================
# RESOLVE APP DISPLAY NAME
# ============================================================================

$displayName = switch ($Environment) {
    'localhost'  { 'TechHub Local Dev' }
    'staging'    { 'TechHub Staging' }
    'production' { 'TechHub Production' }
}

# ============================================================================
# PRE-FLIGHT
# ============================================================================

Write-Step "Validating prerequisites"

$null = az account show -o none 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Not logged in to Azure CLI. Run 'az login' first."
    exit 1
}
Write-Ok "Azure CLI authenticated"

# ============================================================================
# FIND SERVICE PRINCIPAL
# ============================================================================

Write-Step "Looking up service principal for '$displayName'"

$spJson = az ad sp list --display-name $displayName --query "[0].{id:id, appId:appId}" -o json 2>&1
if ($LASTEXITCODE -ne 0 -or -not $spJson) {
    Write-Fail "Could not find enterprise app '$displayName'. Run Manage-EntraId.ps1 first."
    exit 1
}

$sp = $spJson | ConvertFrom-Json
if (-not $sp.id) {
    Write-Fail "No enterprise app found with display name '$displayName'."
    exit 1
}

$spObjectId = $sp.id
Write-Ok "Found service principal (objectId: $spObjectId)"

# ============================================================================
# GET EXISTING ASSIGNMENTS
# ============================================================================

Write-Step "Fetching existing role assignments"

$existingJson = az rest `
    --method GET `
    --uri "https://graph.microsoft.com/v1.0/servicePrincipals/$spObjectId/appRoleAssignedTo" `
    --query "value[].principalId" `
    -o json 2>&1

if ($LASTEXITCODE -ne 0) {
    Write-Fail "Could not fetch existing assignments."
    exit 1
}

$existingPrincipalIds = @($existingJson | ConvertFrom-Json)
Write-Ok "Found $($existingPrincipalIds.Count) existing assignment(s)"

# ============================================================================
# ASSIGN USERS
# ============================================================================

Write-Step "Processing $($AdminUsers.Count) user(s)"

$assigned = 0
$skipped = 0
$failed = 0

foreach ($email in $AdminUsers) {
    $userJson = az ad user show --id $email --query "id" -o tsv 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Warn "User not found in directory: $email"
        $failed++
        continue
    }

    $userId = $userJson.Trim()

    if ($existingPrincipalIds -contains $userId) {
        Write-Ok "Already assigned: $email"
        $skipped++
        continue
    }

    $body = @{
        principalId = $userId
        resourceId  = $spObjectId
        appRoleId   = "00000000-0000-0000-0000-000000000000"
    } | ConvertTo-Json -Compress

    $bodyFile = Join-Path ([System.IO.Path]::GetTempPath()) "techhub-assign-$([guid]::NewGuid().ToString('N')).json"
    try {
        $body | Set-Content -Path $bodyFile -Encoding utf8

        az rest `
            --method POST `
            --uri "https://graph.microsoft.com/v1.0/servicePrincipals/$spObjectId/appRoleAssignments" `
            --headers "Content-Type=application/json" `
            --body "@$bodyFile" 2>&1 | Out-Null

        if ($LASTEXITCODE -ne 0) {
            Write-Warn "Failed to assign: $email"
            $failed++
        }
        else {
            Write-Ok "Assigned: $email"
            $assigned++
        }
    }
    finally {
        if (Test-Path $bodyFile) {
            Remove-Item $bodyFile -Force
        }
    }
}

# ============================================================================
# SUMMARY
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  Done! Assigned: $assigned, Already assigned: $skipped, Failed: $failed" -ForegroundColor $(if ($failed -gt 0) { 'Yellow' } else { 'Green' })
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""
