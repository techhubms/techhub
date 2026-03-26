#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Creates Entra ID (Azure AD) resources for local TechHub development.

.DESCRIPTION
    Automates the creation of all required Entra ID resources for running TechHub
    locally with authentication enabled:

    1. App registration with OIDC redirect URIs for localhost
    2. API scope (Admin.Access) exposed on the app registration
    3. Client secret for the Web app's OIDC flow
    4. Service principal (enterprise app) with "assignment required" enabled
    5. Current user assigned to the enterprise app

    After running, copy the output values into your local configuration
    (user secrets, environment variables, or appsettings.Development.json).

.PARAMETER DisplayName
    Display name for the app registration. Defaults to 'TechHub Local Dev'.

.PARAMETER WebPort
    Port number for the local Web app (HTTPS). Defaults to 5003.

.PARAMETER SecretExpiryDays
    Number of days until the client secret expires. Defaults to 90.

.PARAMETER SkipUserAssignment
    Skip adding the current user to the enterprise app. Useful if you want
    to manage assignments manually.

.EXAMPLE
    ./scripts/Setup-EntraIdDev.ps1
    Create all Entra ID resources with default settings.

.EXAMPLE
    ./scripts/Setup-EntraIdDev.ps1 -DisplayName "TechHub Dev - John" -WebPort 5003
    Create resources with a custom display name and port.

.EXAMPLE
    ./scripts/Setup-EntraIdDev.ps1 -SecretExpiryDays 30 -SkipUserAssignment
    Create resources with a 30-day secret, without auto-assigning the current user.
#>

param(
    [Parameter(Mandatory = $false)]
    [string]$DisplayName = 'TechHub Local Dev',

    [Parameter(Mandatory = $false)]
    [ValidateRange(1, 65535)]
    [int]$WebPort = 5003,

    [Parameter(Mandatory = $false)]
    [ValidateRange(1, 730)]
    [int]$SecretExpiryDays = 90,

    [Parameter(Mandatory = $false)]
    [switch]$SkipUserAssignment
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
Write-Host "  TechHub Entra ID Local Development Setup" -ForegroundColor White
Write-Host "  Display Name     : $DisplayName" -ForegroundColor Gray
Write-Host "  Web Port         : $WebPort" -ForegroundColor Gray
Write-Host "  Secret Expiry    : $SecretExpiryDays days" -ForegroundColor Gray
Write-Host "  User Assignment  : $(if ($SkipUserAssignment) { 'Skip' } else { 'Auto-assign current user' })" -ForegroundColor Gray
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

# Check if an app with the same name already exists
$existingApps = az ad app list --display-name $DisplayName --query "[].{appId:appId, displayName:displayName}" -o json 2>&1 | ConvertFrom-Json
if ($existingApps.Count -gt 0) {
    Write-Warn "App registration '$DisplayName' already exists (appId: $($existingApps[0].appId))"
    Write-Host ""
    Write-Host "   To remove it and start fresh, run:" -ForegroundColor Yellow
    Write-Host "   az ad app delete --id $($existingApps[0].appId)" -ForegroundColor Yellow
    Write-Host ""
    exit 1
}
Write-Ok "No existing app registration with name '$DisplayName'"

# ============================================================================
# STEP 1: CREATE APP REGISTRATION
# ============================================================================

Write-Step "Creating app registration '$DisplayName'"

$redirectUris = @(
    "https://localhost:$($WebPort)/signin-oidc"
)

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

# ============================================================================
# STEP 2: EXPOSE AN API SCOPE
# ============================================================================

Write-Step "Configuring API scope"

# Set the Application ID URI (required before adding scopes)
$appIdUri = "api://$clientId"
az ad app update --id $clientId --identifier-uris $appIdUri 2>&1 | Out-Null
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Failed to set Application ID URI"
    exit 1
}
Write-Ok "Application ID URI set to $appIdUri"

# Generate a unique GUID for the scope
$scopeId = [guid]::NewGuid().ToString()
$scopeName = "Admin.Access"
$fullScope = "$($appIdUri)/$($scopeName)"

# Define the OAuth2 permission scope
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

# Save to temp file (az cli doesn't support complex JSON via --body inline reliably)
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

Write-Ok "API scope '$scopeName' created ($fullScope)"

# Pre-authorize the app to use its own scope (so users don't see a consent prompt)
$preAuthBody = @{
    api = @{
        preAuthorizedApplications = @(
            @{
                appId                = $clientId
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

# ============================================================================
# STEP 3: CREATE CLIENT SECRET
# ============================================================================

Write-Step "Creating client secret (expires in $SecretExpiryDays days)"

$endDate = (Get-Date).AddDays($SecretExpiryDays).ToString("yyyy-MM-ddTHH:mm:ssZ")

$secretJson = az ad app credential reset `
    --id $clientId `
    --append `
    --display-name "TechHub Local Dev" `
    --end-date $endDate `
    --query "{password:password}" `
    -o json 2>&1

if ($LASTEXITCODE -ne 0) {
    Write-Fail "Failed to create client secret"
    Write-Host $secretJson -ForegroundColor Red
    exit 1
}

$secret = ($secretJson | ConvertFrom-Json).password
Write-Ok "Client secret created (expires: $(Get-Date $endDate -Format 'yyyy-MM-dd'))"

# ============================================================================
# STEP 4: CREATE SERVICE PRINCIPAL (ENTERPRISE APP)
# ============================================================================

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

# Set "Assignment required" = true
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

# ============================================================================
# STEP 5: ASSIGN CURRENT USER
# ============================================================================

if (-not $SkipUserAssignment) {
    Write-Step "Assigning current user to the enterprise app"

    $currentUser = az ad signed-in-user show --query "id" -o tsv 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Warn "Could not determine current user. Assign yourself manually in the Azure Portal."
    }
    else {
        # Use the default access role (00000000-0000-0000-0000-000000000000)
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
else {
    Write-Step "Skipping user assignment (use Azure Portal to add users)"
}

# ============================================================================
# OUTPUT CONFIGURATION
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  Setup Complete!" -ForegroundColor Green
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
Write-Host "    dotnet user-secrets set AzureAd:Scopes 'Admin.Access'" -ForegroundColor DarkGray
Write-Host ""
Write-Host "  Redirect URI: https://localhost:$($WebPort)/signin-oidc" -ForegroundColor Gray
Write-Host "  API Scope   : $fullScope" -ForegroundColor Gray
Write-Host ""
Write-Host "  Portal link : https://portal.azure.com/#view/Microsoft_AAD_RegisteredApps/ApplicationMenuBlade/~/Overview/appId/$clientId" -ForegroundColor Gray
Write-Host ""
