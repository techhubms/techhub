#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Sets up .NET user secrets for local development by fetching values from Azure.

.DESCRIPTION
    Populates all required user secrets for the TechHub API and Web projects by
    combining localhost Entra ID app registration values with production AI/cookie
    secrets from the Container App and Key Vault.

    AzureAd settings (TenantId, ClientId, ClientSecret, Scopes) are read/created
    from the localhost app registration ('TechHub Local Dev') so that local
    development uses the correct redirect URIs and token audience. A new client
    secret is created on the app registration (appended — old secrets remain valid).

    AI and YouTube secrets are fetched from the production Container App env vars
    and Key Vault, since those services are shared across environments.

    Requires:
        - Azure CLI (`az`) authenticated with access to rg-techhub-prod, kv-techhub-shared,
          and the Entra ID tenant (to read/write app registrations)
        - Key Vault Secrets User/Officer role on kv-techhub-shared
        - Localhost app registration must exist (run Manage-EntraId.ps1 -Environment localhost first)

    User secrets populated per project:

    TechHub.Api (techhub-api):
        AzureAd:TenantId               — Entra ID tenant (from current Azure CLI session)
        AzureAd:ClientId               — Localhost app registration client ID
        AiCategorization:Endpoint       — Azure OpenAI endpoint URL (production)
        AiCategorization:DeploymentName — Azure OpenAI deployment name (production)
        AiCategorization:ApiKey         — Azure OpenAI API key (from Key Vault)
        ContentProcessor:YouTubeCookies — YouTube cookies (from Key Vault)

    TechHub.Web (techhub-web):
        AzureAd:TenantId               — Entra ID tenant (from current Azure CLI session)
        AzureAd:ClientId               — Localhost app registration client ID
        AzureAd:ClientSecret            — Newly created client secret (90-day expiry)
        AzureAd:Scopes                  — API scope (derived from localhost client ID)

.PARAMETER Force
    Overwrite existing secrets (default: skip already-set values).

.PARAMETER KeyVaultName
    Key Vault name. Defaults to 'kv-techhub-shared'.

.PARAMETER AppDisplayName
    Display name of the localhost app registration. Defaults to 'TechHub Local Dev'.

.PARAMETER SecretExpiryDays
    Number of days until the newly created client secret expires. Defaults to 90.

.EXAMPLE
    ./scripts/Setup-UserSecrets.ps1
.EXAMPLE
    ./scripts/Setup-UserSecrets.ps1 -Force
#>

param(
    [switch]$Force,

    [Parameter(Mandatory = $false)]
    [string]$KeyVaultName = 'kv-techhub-shared',

    [Parameter(Mandatory = $false)]
    [string]$AppDisplayName = 'TechHub Local Dev',

    [Parameter(Mandatory = $false)]
    [ValidateRange(1, 730)]
    [int]$SecretExpiryDays = 90
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$repoRoot = Split-Path -Parent $PSScriptRoot

# --- Fetch localhost app registration from Entra ID ---
Write-Host ""
Write-Host "Looking up localhost app registration '$AppDisplayName'..." -ForegroundColor Cyan

$localhostApps = @(az ad app list --display-name $AppDisplayName --query "[].{appId:appId}" -o json 2>&1 | ConvertFrom-Json)
if ($localhostApps.Count -eq 0) {
    Write-Host "  [FAIL] App registration '$AppDisplayName' not found." -ForegroundColor Red
    Write-Host "  Run: ./scripts/Manage-EntraId.ps1 -Environment localhost" -ForegroundColor Yellow
    exit 1
}

$clientId = $localhostApps[0].appId
$tenantId = (az account show --query "tenantId" -o tsv 2>&1)
$scopes = "api://$clientId/Admin.Access"

Write-Host "  TenantId:  $tenantId" -ForegroundColor Gray
Write-Host "  ClientId:  $clientId" -ForegroundColor Gray
Write-Host "  Scopes:    $scopes" -ForegroundColor Gray

# --- Check if client secret needs to be created ---
$webProject = Join-Path $repoRoot "src/TechHub.Web"
$needsClientSecret = $true
$clientSecret = $null

if (-not $Force) {
    $existing = dotnet user-secrets list --project $webProject 2>&1 | Select-String -Pattern "^AzureAd:ClientSecret = "
    if ($existing) {
        $needsClientSecret = $false
    }
}

if ($needsClientSecret) {
    Write-Host ""
    Write-Host "Creating new client secret on '$AppDisplayName' (expires in $SecretExpiryDays days)..." -ForegroundColor Cyan

    $endDate = (Get-Date).AddDays($SecretExpiryDays).ToString("yyyy-MM-ddTHH:mm:ssZ")
    $secretDisplayName = "Setup-UserSecrets $(Get-Date -Format 'yyyy-MM-dd')"

    $secretOutput = az ad app credential reset `
        --id $clientId `
        --append `
        --display-name $secretDisplayName `
        --end-date $endDate `
        --query "{password:password}" `
        -o json 2>&1

    if ($LASTEXITCODE -ne 0) {
        Write-Host "  [FAIL] Failed to create client secret" -ForegroundColor Red
        Write-Host $secretOutput -ForegroundColor Red
        exit 1
    }

    $secretJson = $secretOutput | Where-Object { $_ -is [string] -and $_ -notmatch '^\s*WARNING' }
    $clientSecret = ($secretJson | ConvertFrom-Json).password
    Write-Host "  Client Secret: $('*' * 8)...(created, expires $(Get-Date $endDate -Format 'yyyy-MM-dd'))" -ForegroundColor Gray
}
else {
    Write-Host ""
    Write-Host "Client secret already set in user secrets — skipping creation (use -Force to rotate)" -ForegroundColor Gray
}

# --- Fetch AI config from production Container App env vars ---
Write-Host ""
Write-Host "Fetching AI configuration from production..." -ForegroundColor Cyan

$apiEnvVars = az containerapp show `
    --name ca-techhub-api-prod `
    --resource-group rg-techhub-prod `
    --query "properties.template.containers[0].env" `
    --output json 2>&1 | ConvertFrom-Json

function Get-EnvVar($envVars, $name) {
    $entry = $envVars | Where-Object { $_.name -eq $name }
    return $entry.value
}

$aiEndpoint = Get-EnvVar $apiEnvVars 'AiCategorization__Endpoint'
$aiDeployment = Get-EnvVar $apiEnvVars 'AiCategorization__DeploymentName'

Write-Host "  AI Endpoint:   $aiEndpoint" -ForegroundColor Gray
Write-Host "  AI Deployment: $aiDeployment" -ForegroundColor Gray

# --- Fetch secrets from Key Vault ---
Write-Host ""
Write-Host "Fetching secrets from Key Vault '$KeyVaultName'..." -ForegroundColor Cyan

$aiApiKey = az keyvault secret show --vault-name $KeyVaultName --name techhub-prod-ai-api-key --query "value" --output tsv
$youtubeCookies = az keyvault secret show --vault-name $KeyVaultName --name techhub-prod-youtube-cookies --query "value" --output tsv

Write-Host "  AI API Key:       $('*' * 8)...(fetched)" -ForegroundColor Gray
Write-Host "  YouTube Cookies:  $('*' * 8)...(fetched)" -ForegroundColor Gray

# --- Helper to set a secret ---
function Set-Secret($project, $key, $value) {
    if (-not $Force) {
        $existing = dotnet user-secrets list --project $project 2>&1 | Select-String -Pattern "^$([regex]::Escape($key)) = "
        if ($existing) {
            Write-Host "  [SKIP] $key — already set (use -Force to overwrite)" -ForegroundColor Gray
            return
        }
    }
    dotnet user-secrets set $key $value --project $project | Out-Null
    # Don't print secret values
    if ($key -match 'Key|Secret|Cookies') {
        Write-Host "  [SET]  $key = ********" -ForegroundColor Green
    }
    else {
        Write-Host "  [SET]  $key = $value" -ForegroundColor Green
    }
}

# --- TechHub.Api secrets ---
$apiProject = Join-Path $repoRoot "src/TechHub.Api"

Write-Host ""
Write-Host "Setting TechHub.Api user secrets..." -ForegroundColor Cyan
Set-Secret $apiProject "AzureAd:TenantId" $tenantId
Set-Secret $apiProject "AzureAd:ClientId" $clientId
Set-Secret $apiProject "AiCategorization:Endpoint" $aiEndpoint
Set-Secret $apiProject "AiCategorization:DeploymentName" $aiDeployment
Set-Secret $apiProject "AiCategorization:ApiKey" $aiApiKey
Set-Secret $apiProject "ContentProcessor:YouTubeCookies" $youtubeCookies

# --- TechHub.Web secrets ---
Write-Host ""
Write-Host "Setting TechHub.Web user secrets..." -ForegroundColor Cyan
Set-Secret $webProject "AzureAd:TenantId" $tenantId
Set-Secret $webProject "AzureAd:ClientId" $clientId
if ($clientSecret) {
    Set-Secret $webProject "AzureAd:ClientSecret" $clientSecret
}
else {
    Write-Host "  [SKIP] AzureAd:ClientSecret — already set (use -Force to overwrite)" -ForegroundColor Gray
}
Set-Secret $webProject "AzureAd:Scopes" $scopes

Write-Host ""
Write-Host "Done. All secrets populated." -ForegroundColor Green
Write-Host ""
Write-Host "  AzureAd values use the localhost app registration ('$AppDisplayName')." -ForegroundColor Gray
Write-Host "  AI and YouTube values use production." -ForegroundColor Gray
if ($clientSecret) {
    Write-Host "  Client secret created on app registration (appended — old secrets remain valid)." -ForegroundColor Gray
}
Write-Host ""
Write-Host "Verify with:" -ForegroundColor Gray
Write-Host "  dotnet user-secrets list --project src/TechHub.Api" -ForegroundColor Gray
Write-Host "  dotnet user-secrets list --project src/TechHub.Web" -ForegroundColor Gray
Write-Host ""
