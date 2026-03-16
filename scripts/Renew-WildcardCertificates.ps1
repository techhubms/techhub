#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Renews wildcard TLS certificates for *.hub.ms and *.xebia.ms using Let's Encrypt and imports them into Azure Key Vault.

.DESCRIPTION
    Uses certbot with the certbot-dns-azure plugin to obtain or renew wildcard certificates
    via DNS-01 challenge. The ACME challenge TXT records are created automatically in the
    Azure DNS zone (acme.hub.ms) — external DNS (GoDaddy) delegates _acme-challenge via CNAME.

    After renewal, certificates are converted to PFX and imported into Key Vault.

    Prerequisites:
    - certbot and certbot-dns-azure installed (pip install certbot certbot-dns-azure)
    - Azure CLI authenticated with access to rg-techhub-shared
    - ACME DNS zone deployed (acme.hub.ms in rg-techhub-shared)
    - GoDaddy CNAME records configured (see docs/wildcard-certificates.md)

.PARAMETER KeyVaultName
    Azure Key Vault name. Defaults to 'kv-techhub-shared'.

.PARAMETER ResourceGroup
    Resource group containing the ACME DNS zone. Defaults to 'rg-techhub-shared'.

.PARAMETER AcmeDnsZone
    Azure DNS zone used for ACME challenges. Defaults to 'acme.hub.ms'.

.PARAMETER Email
    Email address for Let's Encrypt account registration and expiry notifications.

.PARAMETER DryRun
    If set, runs certbot in --dry-run mode (no real certificates issued).

.PARAMETER Force
    If set, forces renewal even if certificates are not yet due.

.EXAMPLE
    ./Renew-WildcardCertificates.ps1 -Email admin@xebia.com
    Renew certificates and import into Key Vault.

.EXAMPLE
    ./Renew-WildcardCertificates.ps1 -Email admin@xebia.com -DryRun
    Test the entire flow without issuing real certificates.
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$Email,

    [Parameter(Mandatory = $false)]
    [string]$KeyVaultName = 'kv-techhub-shared',

    [Parameter(Mandatory = $false)]
    [string]$ResourceGroup = 'rg-techhub-shared',

    [Parameter(Mandatory = $false)]
    [string]$AcmeDnsZone = 'acme.hub.ms',

    [Parameter(Mandatory = $false)]
    [switch]$DryRun,

    [Parameter(Mandatory = $false)]
    [switch]$Force
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# Domains to obtain wildcard certificates for
$domains = @(
    @{ Wildcard = '*.hub.ms'; Bare = 'hub.ms'; CertName = 'wildcard-hub-ms'; AcmeAlias = 'hub-ms' }
    @{ Wildcard = '*.xebia.ms'; Bare = 'xebia.ms'; CertName = 'wildcard-xebia-ms'; AcmeAlias = 'xebia-ms' }
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
Write-Host "  TechHub Wildcard Certificate Renewal" -ForegroundColor White
Write-Host "  Key Vault  : $KeyVaultName" -ForegroundColor Gray
Write-Host "  DNS Zone   : $AcmeDnsZone" -ForegroundColor Gray
Write-Host "  Domains    : $($domains | ForEach-Object { $_.Wildcard }) " -ForegroundColor Gray
if ($DryRun) {
    Write-Host "  Mode       : DRY RUN (no real certs)" -ForegroundColor Yellow
}
Write-Host "===============================================================" -ForegroundColor DarkCyan

# ============================================================================
# PRE-FLIGHT CHECKS
# ============================================================================

Write-Step "Validating prerequisites"

# Check Azure CLI login
$account = az account show -o json 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Fail "Not logged in to Azure CLI. Run 'az login' first."
    exit 1
}
$accountInfo = $account | ConvertFrom-Json
$subscriptionId = $accountInfo.id
Write-Ok "Azure CLI authenticated (subscription: $($accountInfo.name))"

# Check certbot
if (-not (Get-Command certbot -ErrorAction SilentlyContinue)) {
    Write-Fail "certbot not found. Install with: pip install certbot certbot-dns-azure"
    exit 1
}
Write-Ok "certbot found"

# Check certbot-dns-azure plugin
$plugins = certbot plugins 2>&1
if (-not ($plugins -match 'dns-azure')) {
    Write-Fail "certbot-dns-azure plugin not found. Install with: pip install certbot-dns-azure"
    exit 1
}
Write-Ok "certbot-dns-azure plugin found"

# Verify DNS zone exists
az network dns zone show --resource-group $ResourceGroup --name $AcmeDnsZone -o json 2>&1 | Out-Null
if ($LASTEXITCODE -ne 0) {
    Write-Fail "DNS zone '$AcmeDnsZone' not found in resource group '$ResourceGroup'."
    Write-Detail "Deploy shared infrastructure first: ./scripts/Deploy-Infrastructure.ps1 -Environment shared -Mode deploy"
    exit 1
}
Write-Ok "ACME DNS zone '$AcmeDnsZone' exists"

# ============================================================================
# GENERATE CERTBOT-DNS-AZURE CONFIG
# ============================================================================

Write-Step "Generating certbot-dns-azure configuration"

# Build the azure.ini config — maps each domain's ACME challenge to the Azure DNS zone
$azureIniPath = Join-Path ([System.IO.Path]::GetTempPath()) "certbot-azure-$([System.Guid]::NewGuid().ToString('N').Substring(0, 8)).ini"

$iniLines = @(
    "dns_azure_use_cli_credentials = true"
    ""
)

# Map each domain to an explicit TXT record in the ACME delegation zone.
# Uses full ARM resource IDs so certbot-dns-azure creates TXT records at the
# correct alias (e.g. hub-ms.acme.hub.ms) rather than _acme-challenge.hub.ms.
# Format: dns_azure_zoneN = domain:/subscriptions/.../dnsZones/zone/TXT/record
$armPrefix = "/subscriptions/$subscriptionId/resourceGroups/$ResourceGroup/providers/Microsoft.Network/dnsZones/$AcmeDnsZone"

for ($i = 0; $i -lt $domains.Count; $i++) {
    $zoneNum = $i + 1
    $iniLines += "dns_azure_zone$($zoneNum) = $($domains[$i].Bare):$armPrefix/TXT/$($domains[$i].AcmeAlias)"
}

$iniContent = $iniLines -join "`n"
Set-Content -Path $azureIniPath -Value $iniContent -NoNewline
# Restrict permissions (certbot warns about world-readable credentials)
if (Get-Command -Name chmod -ErrorAction SilentlyContinue) {
    chmod 600 $azureIniPath
} else {
    Write-Detail "Skipping chmod on $azureIniPath (not available on this platform)"
}
Write-Ok "Config written to $azureIniPath"
Write-Detail "Contents:"
Get-Content $azureIniPath | ForEach-Object { Write-Detail "  $_" }

# ============================================================================
# OBTAIN/RENEW CERTIFICATES
# ============================================================================

foreach ($domain in $domains) {
    Write-Step "Processing $($domain.Wildcard)"

    $certbotArgs = @(
        'certonly'
        '--authenticator', 'dns-azure'
        '--dns-azure-credentials', $azureIniPath
        '--dns-azure-propagation-seconds', '60'
        '-d', $domain.Wildcard
        '-d', $domain.Bare
        '--email', $Email
        '--agree-tos'
        '--non-interactive'
        '--cert-name', $domain.CertName
    )

    if ($Force) {
        $certbotArgs += '--force-renewal'
    }

    if ($DryRun) {
        $certbotArgs += '--dry-run'
        Write-Detail "Running certbot in dry-run mode..."
    }

    Write-Detail "Running: certbot $($certbotArgs -join ' ')"
    & certbot @certbotArgs

    if ($LASTEXITCODE -ne 0) {
        Write-Fail "certbot failed for $($domain.Wildcard)"
        exit 1
    }
    Write-Ok "Certificate obtained for $($domain.Wildcard)"
}

# ============================================================================
# CONVERT TO PFX AND IMPORT TO KEY VAULT
# ============================================================================

if ($DryRun) {
    Write-Step "Dry run — skipping PFX conversion and Key Vault import"
    Write-Ok "Dry run completed successfully. Re-run without -DryRun to issue real certificates."
} else {
    Write-Step "Converting to PFX and importing to Key Vault"

    # Validate environment for PFX conversion
    if (-not $IsLinux) {
        Write-Fail "PFX conversion requires a Linux environment (certbot stores certificates under /etc/letsencrypt/live)."
        exit 1
    }

    if (-not (Get-Command openssl -ErrorAction SilentlyContinue)) {
        Write-Fail "OpenSSL not found in PATH. Install OpenSSL before running this script."
        exit 1
    }

    $letsencryptLiveDir = '/etc/letsencrypt/live'
    if (-not (Test-Path $letsencryptLiveDir)) {
        Write-Fail "Let's Encrypt live directory '$letsencryptLiveDir' not found. Ensure certbot has issued certificates."
        exit 1
    }

    foreach ($domain in $domains) {
        $certDir = Join-Path $letsencryptLiveDir $domain.CertName
        $pfxPath = Join-Path ([System.IO.Path]::GetTempPath()) "$($domain.CertName).pfx"

        if (-not (Test-Path "$certDir/fullchain.pem")) {
            Write-Fail "Certificate files not found at $certDir"
            exit 1
        }

        # Convert to PFX (empty password — Key Vault protects at rest)
        Write-Detail "Converting $certDir to PFX..."
        openssl pkcs12 -export `
            -out $pfxPath `
            -inkey "$certDir/privkey.pem" `
            -in "$certDir/fullchain.pem" `
            -password pass:

        if ($LASTEXITCODE -ne 0) {
            Write-Fail "PFX conversion failed for $($domain.CertName)"
            exit 1
        }

        # Import to Key Vault
        Write-Detail "Importing $($domain.CertName) into Key Vault '$KeyVaultName'..."
        az keyvault certificate import `
            --vault-name $KeyVaultName `
            --name $domain.CertName `
            --file $pfxPath

        if ($LASTEXITCODE -ne 0) {
            Write-Fail "Key Vault import failed for $($domain.CertName)"
            Write-Detail "Ensure you have network access to the Key Vault (VPN connected)."
            exit 1
        }
        Write-Ok "Imported $($domain.CertName) into Key Vault"

        # Clean up PFX
        Remove-Item $pfxPath -Force
        Write-Detail "Cleaned up temp PFX file"
    }
}

# ============================================================================
# CLEANUP
# ============================================================================

Remove-Item $azureIniPath -Force -ErrorAction SilentlyContinue
Write-Ok "Cleaned up temporary config"

# ============================================================================
# SUMMARY
# ============================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor DarkCyan
Write-Host "  Certificate renewal complete!" -ForegroundColor Green
Write-Host ""
if (-not $DryRun) {
    foreach ($domain in $domains) {
        Write-Host "  $($domain.Wildcard) + $($domain.Bare)" -ForegroundColor White
        Write-Host "    Key Vault certificate: $($domain.CertName)" -ForegroundColor Gray
    }
    Write-Host ""
    Write-Host "  Let's Encrypt certs expire after 90 days." -ForegroundColor Yellow
    Write-Host "  Schedule this script to run every 60 days." -ForegroundColor Yellow
}
Write-Host "===============================================================" -ForegroundColor DarkCyan
