#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Rotates the Entra ID client secret for TechHub and updates GitHub Actions secrets.

.DESCRIPTION
    Creates a new client secret on the TechHub app registration and updates
    GitHub Actions repository secrets. Optionally removes expired secrets.

    Designed for staging and production environments where secrets should be
    rotated regularly. Can be run manually or scheduled (e.g., via cron or
    GitHub Actions workflow_dispatch).

    Prerequisites:
    - Azure CLI authenticated with permissions to manage the app registration
    - GitHub CLI authenticated with repo admin permissions (for updating secrets)

.PARAMETER ClientId
    The Application (client) ID of the Entra app registration.

.PARAMETER SecretExpiryDays
    Number of days until the new client secret expires. Defaults to 180.

.PARAMETER GitHubSecretName
    Name of the GitHub Actions secret to update. Defaults to 'AZURE_AD_CLIENT_SECRET'.

.PARAMETER GitHubRepo
    GitHub repository in 'owner/repo' format. Defaults to the current repo
    detected by the GitHub CLI.

.PARAMETER Environment
    GitHub Actions environment name to scope the secret to (e.g., 'staging', 'production').
    When specified, creates an environment-scoped secret instead of a repository secret.

.PARAMETER RemoveExpired
    Remove all expired client secrets from the app registration after rotation.

.PARAMETER SkipGitHub
    Skip updating GitHub Actions secrets. Only creates the new secret and
    outputs it to the console.

.EXAMPLE
    ./scripts/Rotate-EntraIdSecret.ps1 -ClientId "12345678-..." -Environment staging
    Rotate secret and update the 'staging' environment secret in GitHub.

.EXAMPLE
    ./scripts/Rotate-EntraIdSecret.ps1 -ClientId "12345678-..." -Environment production -RemoveExpired
    Rotate secret for production and clean up expired secrets.

.EXAMPLE
    ./scripts/Rotate-EntraIdSecret.ps1 -ClientId "12345678-..." -SkipGitHub
    Create a new secret without updating GitHub. Useful for manual rotation.
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$ClientId,

    [Parameter(Mandatory = $false)]
    [ValidateRange(1, 730)]
    [int]$SecretExpiryDays = 180,

    [Parameter(Mandatory = $false)]
    [string]$GitHubSecretName = 'AZURE_AD_CLIENT_SECRET',

    [Parameter(Mandatory = $false)]
    [string]$GitHubRepo,

    [Parameter(Mandatory = $false)]
    [ValidateSet('staging', 'production')]
    [string]$Environment,

    [Parameter(Mandatory = $false)]
    [switch]$RemoveExpired,

    [Parameter(Mandatory = $false)]
    [switch]$SkipGitHub
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

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
Write-Host "  TechHub Entra ID Secret Rotation" -ForegroundColor White
Write-Host "  Client ID        : $ClientId" -ForegroundColor Gray
Write-Host "  Secret Expiry    : $SecretExpiryDays days" -ForegroundColor Gray
Write-Host "  GitHub Secret    : $GitHubSecretName" -ForegroundColor Gray
if ($Environment) {
    Write-Host "  Environment      : $Environment" -ForegroundColor Gray
}
Write-Host "  Remove Expired   : $RemoveExpired" -ForegroundColor Gray
Write-Host "  Skip GitHub      : $SkipGitHub" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan

# ============================================================================
# PRE-FLIGHT CHECKS
# ============================================================================

Write-Step "Validating prerequisites"

# Check Azure CLI
$account = az account show -o json 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Not logged in to Azure CLI. Run 'az login' first."
    exit 1
}
$accountInfo = $account | ConvertFrom-Json
Write-Ok "Azure CLI authenticated (tenant: $($accountInfo.tenantId))"

# Verify the app registration exists
$appJson = az ad app show --id $ClientId -o json 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Fail "App registration not found (clientId: $ClientId)"
    exit 1
}
$app = $appJson | ConvertFrom-Json
Write-Ok "App registration found: $($app.displayName)"

# Check GitHub CLI (unless skipping)
if (-not $SkipGitHub) {
    $null = gh auth status 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Not logged in to GitHub CLI. Run 'gh auth login' first."
        exit 1
    }
    Write-Ok "GitHub CLI authenticated"

    # Determine repo
    if (-not $GitHubRepo) {
        $GitHubRepo = gh repo view --json nameWithOwner --jq '.nameWithOwner' 2>&1
        if ($LASTEXITCODE -ne 0) {
            Write-Fail "Could not determine GitHub repository. Use -GitHubRepo parameter."
            exit 1
        }
    }
    Write-Ok "GitHub repo: $GitHubRepo"
}

# ============================================================================
# STEP 1: LIST CURRENT SECRETS
# ============================================================================

Write-Step "Checking existing secrets"

$credentialsJson = az ad app credential list --id $ClientId -o json 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Warn "Could not list existing credentials"
    $credentials = @()
}
else {
    $credentials = $credentialsJson | ConvertFrom-Json
}

$now = Get-Date
$activeCount = ($credentials | Where-Object { [datetime]$_.endDateTime -gt $now }).Count
$expiredCount = ($credentials | Where-Object { [datetime]$_.endDateTime -le $now }).Count
Write-Detail "Active secrets: $activeCount, Expired: $expiredCount"

# ============================================================================
# STEP 2: CREATE NEW SECRET
# ============================================================================

Write-Step "Creating new client secret (expires in $SecretExpiryDays days)"

$endDate = $now.AddDays($SecretExpiryDays).ToString("yyyy-MM-ddTHH:mm:ssZ")
$secretDisplayName = "TechHub $(if ($Environment) { $Environment } else { 'rotated' }) $(Get-Date -Format 'yyyy-MM-dd')"

$secretJson = az ad app credential reset `
    --id $ClientId `
    --append `
    --display-name $secretDisplayName `
    --end-date $endDate `
    --query "{password:password}" `
    -o json 2>&1

if ($LASTEXITCODE -ne 0) {
    Write-Fail "Failed to create client secret"
    Write-Host $secretJson -ForegroundColor Red
    exit 1
}

$newSecret = ($secretJson | ConvertFrom-Json).password
Write-Ok "New secret created: '$secretDisplayName' (expires: $(Get-Date $endDate -Format 'yyyy-MM-dd'))"

# ============================================================================
# STEP 3: UPDATE GITHUB ACTIONS SECRET
# ============================================================================

if (-not $SkipGitHub) {
    Write-Step "Updating GitHub Actions secret '$GitHubSecretName'"

    $ghArgs = @("secret", "set", $GitHubSecretName, "--repo", $GitHubRepo, "--body", $newSecret)
    if ($Environment) {
        $ghArgs += @("--env", $Environment)
    }

    $ghResult = & gh @ghArgs 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to update GitHub secret"
        Write-Host $ghResult -ForegroundColor Red
        Write-Host ""
        Write-Host "   New secret value (store manually):" -ForegroundColor Yellow
        Write-Host "   $newSecret" -ForegroundColor White
        exit 1
    }

    $scopeLabel = if ($Environment) { "environment '$Environment'" } else { "repository" }
    Write-Ok "GitHub $scopeLabel secret '$GitHubSecretName' updated"
}

# ============================================================================
# STEP 4: REMOVE EXPIRED SECRETS
# ============================================================================

if ($RemoveExpired -and $expiredCount -gt 0) {
    Write-Step "Removing $expiredCount expired secret(s)"

    $expiredCreds = $credentials | Where-Object { [datetime]$_.endDateTime -le $now }
    foreach ($cred in $expiredCreds) {
        az ad app credential delete --id $ClientId --key-id $cred.keyId 2>&1 | Out-Null
        if ($LASTEXITCODE -eq 0) {
            Write-Ok "Removed expired secret: $($cred.displayName) (expired: $(Get-Date $cred.endDateTime -Format 'yyyy-MM-dd'))"
        }
        else {
            Write-Warn "Failed to remove secret: $($cred.keyId)"
        }
    }
}
elseif ($RemoveExpired) {
    Write-Step "No expired secrets to remove"
}

# ============================================================================
# SUMMARY
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  Secret Rotation Complete!" -ForegroundColor Green
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host ""
Write-Host "  New secret        : $secretDisplayName" -ForegroundColor White
Write-Host "  Expires           : $(Get-Date $endDate -Format 'yyyy-MM-dd')" -ForegroundColor White
if (-not $SkipGitHub) {
    Write-Host "  GitHub secret     : $GitHubSecretName ($scopeLabel)" -ForegroundColor White
}
Write-Host ""

if ($SkipGitHub) {
    Write-Host "  Client secret value (store securely):" -ForegroundColor Yellow
    Write-Host "  $newSecret" -ForegroundColor White
    Write-Host ""
    Write-Host "  This will not be shown again!" -ForegroundColor Yellow
    Write-Host ""
}

# Warn if approaching max secrets (Azure AD limit is ~2 per app for password credentials)
$updatedCredsJson = az ad app credential list --id $ClientId -o json 2>&1
if ($LASTEXITCODE -eq 0) {
    $updatedCreds = $updatedCredsJson | ConvertFrom-Json
    $activeCreds = ($updatedCreds | Where-Object { [datetime]$_.endDateTime -gt $now }).Count
    if ($activeCreds -gt 2) {
        Write-Warn "You have $activeCreds active secrets. Consider running with -RemoveExpired or manually cleaning up old secrets."
    }
}
