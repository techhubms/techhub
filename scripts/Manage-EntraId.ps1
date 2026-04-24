#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Manages Entra ID (Azure AD) resources for TechHub across all environments.

.DESCRIPTION
    Unified script for creating and maintaining Entra ID resources for TechHub.
    Handles all environments: localhost, staging, and production.

    On first run (no existing app registration):
    1. Creates app registration with OIDC redirect URIs for the target environment
    2. Exposes an API scope (Admin.Access) on the app registration
    3. Creates a client secret
    4. Creates a service principal (enterprise app) with "assignment required" enabled
    5. Assigns the current user to the enterprise app (localhost only)

    On subsequent runs (app registration already exists):
    1. Verifies the app registration
    2. Creates a new client secret (appended — old secrets remain valid for overlap)
    3. Optionally cleans up expired secrets

    Secret delivery:
    - localhost:   Prints values to the console for manual configuration
    - staging:     Updates GitHub Actions environment secret
    - production:  Updates GitHub Actions environment secret

.PARAMETER Environment
    Target environment. Determines redirect URIs and secret delivery method.
    - localhost:   Redirect to https://localhost:<WebPort>/signin-oidc
    - staging:     Redirect to https://staging-tech.hub.ms/signin-oidc
    - production:  Redirect to https://tech.hub.ms/signin-oidc and https://tech.xebia.ms/signin-oidc

.PARAMETER DisplayName
    Display name for the app registration. Defaults to 'TechHub <Environment>'.

.PARAMETER WebPort
    Port number for the local Web app (HTTPS). Only used for localhost. Defaults to 5003.

.PARAMETER SecretExpiryDays
    Number of days until the client secret expires. Defaults to 90 for localhost, 180 for staging/production.

.PARAMETER SkipUserAssignment
    Skip adding the current user to the enterprise app on first run.

.PARAMETER RemoveExpired
    Remove all expired client secrets after creating the new one.

.PARAMETER GitHubSecretName
    Name of the GitHub Actions secret. Defaults to 'AZURE_AD_CLIENT_SECRET'.

.PARAMETER GitHubRepo
    GitHub repository in 'owner/repo' format. Defaults to the current repo.

.EXAMPLE
    ./scripts/Manage-EntraId.ps1 -Environment localhost
    First run: create app registration + secret. Subsequent runs: rotate secret.

.EXAMPLE
    ./scripts/Manage-EntraId.ps1 -Environment staging
    Create or rotate for staging. Pushes new secret to GitHub Actions.

.EXAMPLE
    ./scripts/Manage-EntraId.ps1 -Environment production -RemoveExpired
    Rotate production secret and clean up expired secrets.
#>

param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('localhost', 'staging', 'production')]
    [string]$Environment,

    [Parameter(Mandatory = $false)]
    [string]$DisplayName,

    [Parameter(Mandatory = $false)]
    [ValidateRange(1, 65535)]
    [int]$WebPort = 5003,

    [Parameter(Mandatory = $false)]
    [ValidateRange(1, 730)]
    [int]$SecretExpiryDays,

    [Parameter(Mandatory = $false)]
    [switch]$SkipUserAssignment,

    [Parameter(Mandatory = $false)]
    [switch]$RemoveExpired,

    [Parameter(Mandatory = $false)]
    [string]$GitHubSecretName = 'AZURE_AD_CLIENT_SECRET',

    [Parameter(Mandatory = $false)]
    [string]$GitHubRepo
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# ============================================================================
# DEFAULTS
# ============================================================================

if (-not $DisplayName) {
    $DisplayName = switch ($Environment) {
        'localhost'  { 'TechHub Local Dev' }
        'staging'    { 'TechHub Staging' }
        'production' { 'TechHub Production' }
    }
}

if (-not $PSBoundParameters.ContainsKey('SecretExpiryDays')) {
    $SecretExpiryDays = if ($Environment -eq 'localhost') { 90 } else { 180 }
}

$isLocalhost = $Environment -eq 'localhost'

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
Write-Host "  TechHub Entra ID Management" -ForegroundColor White
Write-Host "  Environment      : $Environment" -ForegroundColor Gray
Write-Host "  Display Name     : $DisplayName" -ForegroundColor Gray
Write-Host "  Secret Expiry    : $SecretExpiryDays days" -ForegroundColor Gray
if ($isLocalhost) {
    Write-Host "  Web Port         : $WebPort" -ForegroundColor Gray
}
Write-Host "  Remove Expired   : $RemoveExpired" -ForegroundColor Gray
Write-Host "===============================================================" -ForegroundColor DarkCyan

# ============================================================================
# PRE-FLIGHT CHECKS
# ============================================================================

Write-Step "Validating prerequisites"

$account = az account show -o json 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Not logged in to Azure CLI. Run 'az login' first."
    exit 1
}
$accountInfo = $account | ConvertFrom-Json
$tenantId = $accountInfo.tenantId
Write-Ok "Azure CLI authenticated (tenant: $tenantId)"

# For staging/production, verify GitHub CLI is ready
if (-not $isLocalhost) {
    $null = gh auth status 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Not logged in to GitHub CLI. Run 'gh auth login' first."
        exit 1
    }
    Write-Ok "GitHub CLI authenticated"

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
# DETERMINE REDIRECT URIs
# ============================================================================

$redirectUris = switch ($Environment) {
    'localhost' {
        @("https://localhost:$($WebPort)/signin-oidc")
    }
    'staging' {
        @("https://staging-tech.hub.ms/signin-oidc")
    }
    'production' {
        @(
            "https://tech.hub.ms/signin-oidc",
            "https://tech.xebia.ms/signin-oidc"
        )
    }
}

# ============================================================================
# CHECK IF APP REGISTRATION EXISTS
# ============================================================================

Write-Step "Looking for existing app registration '$DisplayName'"

$existingApps = @(az ad app list --display-name $DisplayName --query "[].{appId:appId, id:id, displayName:displayName}" -o json 2>&1 | ConvertFrom-Json)
$isNewApp = $existingApps.Count -eq 0

if ($isNewApp) {
    Write-Detail "No existing app registration found — will create one"
}
else {
    $clientId = $existingApps[0].appId
    $objectId = $existingApps[0].id
    Write-Ok "Found existing app registration (appId: $clientId)"
}

# ============================================================================
# CREATE APP REGISTRATION (first run only)
# ============================================================================

if ($isNewApp) {
    Write-Step "Creating app registration '$DisplayName'"

    $appJson = az ad app create `
        --display-name $DisplayName `
        --sign-in-audience "AzureADMyOrg" `
        --web-redirect-uris $redirectUris `
        --enable-id-token-issuance true `
        -o json 2>&1

    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to create app registration"
        Write-Host $appJson -ForegroundColor Red
        exit 1
    }

    $app = $appJson | ConvertFrom-Json
    $clientId = $app.appId
    $objectId = $app.id
    Write-Ok "App registration created (appId: $clientId)"

    # ── Expose an API scope ────────────────────────────────────────────────

    Write-Step "Configuring API scope"

    $appIdUri = "api://$clientId"
    az ad app update --id $clientId --identifier-uris $appIdUri 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to set Application ID URI"
        exit 1
    }
    Write-Ok "Application ID URI set to $appIdUri"

    $scopeId = [guid]::NewGuid().ToString()
    $scopeName = "Admin.Access"

    $scopeBody = @{
        api = @{
            oauth2PermissionScopes = @(
                @{
                    id                      = $scopeId
                    adminConsentDescription = "Allows admin access to TechHub API endpoints"
                    adminConsentDisplayName = "Admin Access to TechHub API"
                    isEnabled               = $true
                    type                    = "Admin"
                    value                   = $scopeName
                }
            )
        }
    } | ConvertTo-Json -Depth 10 -Compress

    $scopeFile = Join-Path ([System.IO.Path]::GetTempPath()) "techhub-scope-$([guid]::NewGuid().ToString('N')).json"
    try {
        $scopeBody | Set-Content -Path $scopeFile -Encoding utf8

        az rest `
            --method PATCH `
            --uri "https://graph.microsoft.com/v1.0/applications/$objectId" `
            --headers "Content-Type=application/json" `
            --body "@$scopeFile" 2>&1 | Out-Null

        if ($LASTEXITCODE -ne 0) {
            Write-Fail "Failed to add API scope"
            exit 1
        }
    }
    finally {
        if (Test-Path $scopeFile) {
            Remove-Item $scopeFile -Force
        }
    }

    Write-Ok "API scope '$scopeName' created ($($appIdUri)/$($scopeName))"

    # Pre-authorize the app
    $preAuthBody = @{
        api = @{
            preAuthorizedApplications = @(
                @{
                    appId                  = $clientId
                    delegatedPermissionIds = @($scopeId)
                }
            )
        }
    } | ConvertTo-Json -Depth 10 -Compress

    $preAuthFile = Join-Path ([System.IO.Path]::GetTempPath()) "techhub-preauth-$([guid]::NewGuid().ToString('N')).json"
    try {
        $preAuthBody | Set-Content -Path $preAuthFile -Encoding utf8

        az rest `
            --method PATCH `
            --uri "https://graph.microsoft.com/v1.0/applications/$objectId" `
            --headers "Content-Type=application/json" `
            --body "@$preAuthFile" 2>&1 | Out-Null

        if ($LASTEXITCODE -ne 0) {
            Write-Warn "Failed to pre-authorize app (users may see a consent prompt)"
        }
        else {
            Write-Ok "App pre-authorized for its own scope (no consent prompt)"
        }
    }
    finally {
        if (Test-Path $preAuthFile) {
            Remove-Item $preAuthFile -Force
        }
    }

    # ── Create service principal ───────────────────────────────────────────

    Write-Step "Creating service principal (enterprise app)"

    $spJson = az ad sp create --id $clientId -o json 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Failed to create service principal"
        Write-Host $spJson -ForegroundColor Red
        exit 1
    }

    $sp = $spJson | ConvertFrom-Json
    $spObjectId = $sp.id
    Write-Ok "Service principal created (objectId: $spObjectId)"

    az rest `
        --method PATCH `
        --uri "https://graph.microsoft.com/v1.0/servicePrincipals/$spObjectId" `
        --headers "Content-Type=application/json" `
        --body '{"appRoleAssignmentRequired":true}' 2>&1 | Out-Null

    if ($LASTEXITCODE -ne 0) {
        Write-Warn "Failed to enable 'Assignment required'. Set it manually in the Azure Portal."
    }
    else {
        Write-Ok "Assignment required = true (only assigned users can sign in)"
    }

    # ── Assign current user (localhost only) ───────────────────────────────

    if ($isLocalhost -and -not $SkipUserAssignment) {
        Write-Step "Assigning current user to the enterprise app"

        $currentUser = az ad signed-in-user show --query "id" -o tsv 2>&1
        if ($LASTEXITCODE -ne 0) {
            Write-Warn "Could not determine current user. Assign yourself manually in the Azure Portal."
        }
        else {
            $assignmentBody = @{
                principalId = $currentUser
                resourceId  = $spObjectId
                appRoleId   = "00000000-0000-0000-0000-000000000000"
            } | ConvertTo-Json -Compress

            $assignmentFile = Join-Path ([System.IO.Path]::GetTempPath()) "techhub-assignment-$([guid]::NewGuid().ToString('N')).json"
            try {
                $assignmentBody | Set-Content -Path $assignmentFile -Encoding utf8

                az rest `
                    --method POST `
                    --uri "https://graph.microsoft.com/v1.0/servicePrincipals/$spObjectId/appRoleAssignments" `
                    --headers "Content-Type=application/json" `
                    --body "@$assignmentFile" 2>&1 | Out-Null

                if ($LASTEXITCODE -ne 0) {
                    Write-Warn "Failed to assign current user. Add yourself manually in the Azure Portal."
                }
                else {
                    Write-Ok "Current user assigned to enterprise app"
                }
            }
            finally {
                if (Test-Path $assignmentFile) {
                    Remove-Item $assignmentFile -Force
                }
            }
        }
    }
    elseif ($isLocalhost) {
        Write-Step "Skipping user assignment (use Azure Portal to add users)"
    }
}

# ============================================================================
# LIST CURRENT SECRETS
# ============================================================================

Write-Step "Checking existing secrets"

$credentialsJson = az ad app credential list --id $clientId -o json 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Warn "Could not list existing credentials"
    $credentials = @()
}
else {
    $credentials = @($credentialsJson | ConvertFrom-Json)
}

$now = Get-Date
$activeCount = @($credentials | Where-Object { [datetime]$_.endDateTime -gt $now }).Count
$expiredCount = @($credentials | Where-Object { [datetime]$_.endDateTime -le $now }).Count
Write-Detail "Active secrets: $activeCount, Expired: $expiredCount"

# ============================================================================
# CREATE NEW CLIENT SECRET (always appended — old secrets stay valid)
# ============================================================================

Write-Step "Creating new client secret (expires in $SecretExpiryDays days)"

$endDate = $now.AddDays($SecretExpiryDays).ToString("yyyy-MM-ddTHH:mm:ssZ")
$secretDisplayName = "TechHub $Environment $(Get-Date -Format 'yyyy-MM-dd')"

$secretOutput = az ad app credential reset `
    --id $clientId `
    --append `
    --display-name $secretDisplayName `
    --end-date $endDate `
    --query "{password:password}" `
    -o json 2>&1

if ($LASTEXITCODE -ne 0) {
    Write-Fail "Failed to create client secret"
    Write-Host $secretOutput -ForegroundColor Red
    exit 1
}

# Filter out Azure CLI warnings (stderr captured by 2>&1) before parsing JSON
$secretJson = $secretOutput | Where-Object { $_ -is [string] -and $_ -notmatch '^\s*WARNING' }
$secret = ($secretJson | ConvertFrom-Json).password
Write-Ok "New secret created: '$secretDisplayName' (expires: $(Get-Date $endDate -Format 'yyyy-MM-dd'))"
Write-Detail "Previous secrets remain valid — no downtime during rotation"

# ============================================================================
# REMOVE EXPIRED SECRETS (optional)
# ============================================================================

if ($RemoveExpired -and $expiredCount -gt 0) {
    Write-Step "Removing $expiredCount expired secret(s)"

    $expiredCreds = $credentials | Where-Object { [datetime]$_.endDateTime -le $now }
    foreach ($cred in $expiredCreds) {
        az ad app credential delete --id $clientId --key-id $cred.keyId 2>&1 | Out-Null
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
# DELIVER SECRET
# ============================================================================

$appIdUri = "api://$clientId"
$scopeName = "Admin.Access"
$fullScope = "$appIdUri/$scopeName"

if ($isLocalhost) {
    # ── Localhost: print to console ────────────────────────────────────────

    Write-Host ""
    Write-Host "===============================================================" -ForegroundColor DarkCyan
    Write-Host "  $(if ($isNewApp) { 'Setup' } else { 'Secret Rotation' }) Complete!" -ForegroundColor Green
    Write-Host "===============================================================" -ForegroundColor DarkCyan
    Write-Host ""
    Write-Host "  Copy these values to your local configuration:" -ForegroundColor White
    Write-Host ""
    Write-Host "  ┌─────────────────────────────────────────────────────────────" -ForegroundColor DarkGray
    Write-Host "  │ TenantId     : $tenantId" -ForegroundColor White
    Write-Host "  │ ClientId     : $clientId" -ForegroundColor White
    Write-Host "  │ ClientSecret : $secret" -ForegroundColor White
    Write-Host "  │ Scopes       : $fullScope" -ForegroundColor White
    Write-Host "  └─────────────────────────────────────────────────────────────" -ForegroundColor DarkGray
    Write-Host ""
    Write-Host "  Store the client secret securely! It will not be shown again." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "  Option 1: Set environment variables" -ForegroundColor Gray
    Write-Host "    `$env:AzureAd__TenantId = '$tenantId'" -ForegroundColor DarkGray
    Write-Host "    `$env:AzureAd__ClientId = '$clientId'" -ForegroundColor DarkGray
    Write-Host "    `$env:AzureAd__ClientSecret = '$secret'" -ForegroundColor DarkGray
    Write-Host "    `$env:AzureAd__Scopes = '$fullScope'" -ForegroundColor DarkGray
    Write-Host ""
    Write-Host "  Option 2: .NET User Secrets (recommended)" -ForegroundColor Gray
    Write-Host "    cd src/TechHub.Web" -ForegroundColor DarkGray
    Write-Host "    dotnet user-secrets set AzureAd:TenantId '$tenantId'" -ForegroundColor DarkGray
    Write-Host "    dotnet user-secrets set AzureAd:ClientId '$clientId'" -ForegroundColor DarkGray
    Write-Host "    dotnet user-secrets set AzureAd:ClientSecret '$secret'" -ForegroundColor DarkGray
    Write-Host "    dotnet user-secrets set AzureAd:Scopes '$fullScope'" -ForegroundColor DarkGray
    Write-Host "    cd ../TechHub.Api" -ForegroundColor DarkGray
    Write-Host "    dotnet user-secrets set AzureAd:TenantId '$tenantId'" -ForegroundColor DarkGray
    Write-Host "    dotnet user-secrets set AzureAd:ClientId '$clientId'" -ForegroundColor DarkGray
    Write-Host ""
    Write-Host "  Redirect URI: $($redirectUris -join ', ')" -ForegroundColor Gray
    Write-Host "  API Scope   : $fullScope" -ForegroundColor Gray
    Write-Host ""
}
else {
    # ── Staging/Production: push to GitHub Actions ─────────────────────────

    Write-Step "Updating GitHub Actions secrets for '$Environment'"

    $secrets = @{
        'AZURE_AD_TENANT_ID' = $tenantId
        'AZURE_AD_CLIENT_ID' = $clientId
        $GitHubSecretName    = $secret
    }

    $allOk = $true
    foreach ($entry in $secrets.GetEnumerator()) {
        $ghArgs = @("secret", "set", $entry.Key, "--repo", $GitHubRepo, "--body", $entry.Value, "--env", $Environment)

        $ghResult = & gh @ghArgs 2>&1
        if ($LASTEXITCODE -ne 0) {
            Write-Fail "Failed to set GitHub secret '$($entry.Key)'"
            Write-Host $ghResult -ForegroundColor Red
            $allOk = $false
        }
        else {
            Write-Ok "GitHub secret '$($entry.Key)' updated (environment: $Environment)"
        }
    }

    if (-not $allOk) {
        Write-Host ""
        Write-Host "  Some secrets failed to update. Values for manual setup:" -ForegroundColor Yellow
        Write-Host "  ┌─────────────────────────────────────────────────────────────" -ForegroundColor DarkGray
        Write-Host "  │ TenantId     : $tenantId" -ForegroundColor White
        Write-Host "  │ ClientId     : $clientId" -ForegroundColor White
        Write-Host "  │ ClientSecret : $secret" -ForegroundColor White
        Write-Host "  │ Scopes       : $fullScope" -ForegroundColor White
        Write-Host "  └─────────────────────────────────────────────────────────────" -ForegroundColor DarkGray
        Write-Host ""
        exit 1
    }

    Write-Host ""
    Write-Host "===============================================================" -ForegroundColor DarkCyan
    Write-Host "  $(if ($isNewApp) { 'Setup' } else { 'Secret Rotation' }) Complete!" -ForegroundColor Green
    Write-Host "===============================================================" -ForegroundColor DarkCyan
    Write-Host ""
    Write-Host "  Environment       : $Environment" -ForegroundColor White
    Write-Host "  App Registration  : $DisplayName (appId: $clientId)" -ForegroundColor White
    Write-Host "  New secret        : $secretDisplayName" -ForegroundColor White
    Write-Host "  Expires           : $(Get-Date $endDate -Format 'yyyy-MM-dd')" -ForegroundColor White
    Write-Host "  GitHub secrets    : Updated in '$Environment' environment" -ForegroundColor White
    Write-Host "  Redirect URIs     : $($redirectUris -join ', ')" -ForegroundColor White
    Write-Host ""
    Write-Host "  Old secrets remain valid — deploy at your convenience." -ForegroundColor Gray
    Write-Host ""
}

# Warn if too many active secrets
$updatedCredsJson = az ad app credential list --id $clientId -o json 2>&1
if ($LASTEXITCODE -eq 0) {
    $updatedCreds = @($updatedCredsJson | ConvertFrom-Json)
    $activeCreds = @($updatedCreds | Where-Object { [datetime]$_.endDateTime -gt $now }).Count
    if ($activeCreds -gt 2) {
        Write-Warn "You have $activeCreds active secrets. Consider running with -RemoveExpired to clean up."
    }
}
