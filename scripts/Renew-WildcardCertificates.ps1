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
    - Azure CLI authenticated with access to rg-techhub-prod
    - ACME DNS zone deployed (acme.hub.ms in rg-techhub-prod)
    - GoDaddy CNAME records configured (see docs/wildcard-certificates.md)

.PARAMETER KeyVaultName
    Azure Key Vault name. Defaults to 'kv-techhub-prod'.

.PARAMETER ResourceGroup
    Resource group containing the ACME DNS zone and other resources. Defaults to 'rg-techhub-prod'.

.PARAMETER AppServicePlanName
    App Service Plan to associate the renewed certificates with (App Service certificates are
    scoped to a resource group but validated/priced against a plan's region). Defaults to
    'asp-techhub-prod'.

.PARAMETER AppServicePlanResourceGroup
    Resource group containing the App Service Plan. Defaults to the same as ResourceGroup.

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
    [string]$KeyVaultName = 'kv-techhub-prod',

    [Parameter(Mandatory = $false)]
    [string]$ResourceGroup = 'rg-techhub-prod',

    [Parameter(Mandatory = $false)]
    [string]$AcmeDnsZone = 'acme.hub.ms',

    [Parameter(Mandatory = $false)]
    [string]$AppServicePlanName = 'asp-techhub-prod',

    [Parameter(Mandatory = $false)]
    [string]$AppServicePlanResourceGroup = '',

    [Parameter(Mandatory = $false)]
    [switch]$DryRun,

    [Parameter(Mandatory = $false)]
    [switch]$Force
)

# Default the App Service Plan's resource group to the same RG if not specified
if (-not $AppServicePlanResourceGroup) {
    $AppServicePlanResourceGroup = $ResourceGroup
}

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
$account = az account show -o json --only-show-errors
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
    Write-Detail "Deploy infrastructure first: ./scripts/Deploy-Infrastructure.ps1 -Mode deploy"
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

# Use a temp-based certbot working directory so the script works without root.
# certbot defaults to /etc/letsencrypt (requires root); these flags redirect all
# state to a user-writable path.
$certbotBaseDir = Join-Path ([System.IO.Path]::GetTempPath()) "certbot-$([System.Guid]::NewGuid().ToString('N').Substring(0, 8))"
$certbotConfigDir = Join-Path $certbotBaseDir 'config'
$certbotWorkDir   = Join-Path $certbotBaseDir 'work'
$certbotLogsDir   = Join-Path $certbotBaseDir 'logs'
New-Item -ItemType Directory -Force -Path $certbotConfigDir, $certbotWorkDir, $certbotLogsDir | Out-Null

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
        '--config-dir', $certbotConfigDir
        '--work-dir',   $certbotWorkDir
        '--logs-dir',   $certbotLogsDir
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

    if (-not (Get-Command openssl -ErrorAction SilentlyContinue)) {
        Write-Fail "OpenSSL not found in PATH. Install OpenSSL before running this script."
        exit 1
    }

    $letsencryptLiveDir = Join-Path $certbotConfigDir 'live'
    if (-not (Test-Path $letsencryptLiveDir)) {
        Write-Fail "Let's Encrypt live directory '$letsencryptLiveDir' not found. Ensure certbot has issued certificates."
        exit 1
    }

    # Resolve the App Service Plan and Key Vault resource details once — reused for every
    # domain's wildcardCert.bicep redeploy below.
    Write-Step "Resolving App Service Plan and Key Vault resource IDs"
    $appServicePlanId = (az appservice plan show --name $AppServicePlanName --resource-group $AppServicePlanResourceGroup --query id --output tsv --only-show-errors 2>&1)
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Could not resolve App Service Plan '$AppServicePlanName' in resource group '$AppServicePlanResourceGroup'."
        exit 1
    }
    $appServicePlanLocation = (az appservice plan show --name $AppServicePlanName --resource-group $AppServicePlanResourceGroup --query location --output tsv --only-show-errors 2>&1)
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Could not resolve location for App Service Plan '$AppServicePlanName'."
        exit 1
    }
    $keyVaultResourceId = (az keyvault show --name $KeyVaultName --query id --output tsv --only-show-errors 2>&1)
    if ($LASTEXITCODE -ne 0) {
        Write-Fail "Could not resolve Key Vault '$KeyVaultName' resource ID."
        exit 1
    }
    Write-Ok "App Service Plan: $AppServicePlanName ($appServicePlanLocation)"

    # --- Temporarily open Key Vault firewall for this machine's IP ---
    Write-Step "Opening Key Vault firewall for current IP"
    $currentIp = $null
    foreach ($provider in @('https://checkip.amazonaws.com', 'https://api.ipify.org', 'https://icanhazip.com')) {
        try {
            $response = (Invoke-RestMethod -Uri $provider -TimeoutSec 10).Trim()
            if ($response -match '^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$') {
                $currentIp = $response
                break
            }
        }
        catch { }
    }
    if (-not $currentIp) {
        Write-Fail "Failed to detect outbound IP from any provider. Cannot open Key Vault firewall."
        exit 1
    }
    $ipCidr = "$currentIp/32"
    Write-Detail "Outbound IP: $currentIp"

    $existingRules = @(az keyvault network-rule list --name $KeyVaultName --query 'ipRules[].value' --output tsv --only-show-errors 2>&1 |
        ForEach-Object { $_.Trim() } | Where-Object { $_ -ne '' })
    $ruleAlreadyPresent = $existingRules -contains $currentIp -or $existingRules -contains $ipCidr

    $ipWasAdded = $false
    try {
        if (-not $ruleAlreadyPresent) {
            Write-Detail "Adding $currentIp to Key Vault '$KeyVaultName' firewall..."
            az keyvault network-rule add --name $KeyVaultName --ip-address $ipCidr --output none --only-show-errors
            if ($LASTEXITCODE -ne 0) {
                Write-Fail "Failed to add IP $currentIp to Key Vault firewall."
                exit 1
            }
            $ipWasAdded = $true
            # Poll until the rule takes effect
            $maxWaitSecs = 60; $elapsed = 0; $propagated = $false
            while ($elapsed -lt $maxWaitSecs -and -not $propagated) {
                Start-Sleep -Seconds 5; $elapsed += 5
                az keyvault secret list --vault-name $KeyVaultName --query '[]' --output none 2>$null
                if ($LASTEXITCODE -eq 0) { $propagated = $true; Write-Detail "Firewall rule active after ${elapsed}s." }
                else { Write-Detail "Still propagating (${elapsed}s / ${maxWaitSecs}s)..." }
            }
            if (-not $propagated) { Write-Warning "Firewall rule may not have propagated after ${maxWaitSecs}s — proceeding anyway." }
        }
        else {
            Write-Detail "IP $currentIp already permitted."
        }
        Write-Ok "Key Vault firewall open"

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

            # Write as a Key Vault secret (base64-encoded PFX, content-type application/x-pkcs12).
            # Using az keyvault secret set rather than certificate import because:
            # 1. The infra reads from /secrets/{name} (not /certificates/{name})
            # 2. The KV has purge protection enabled, so import conflicts with existing plain
            #    secrets cannot be resolved by purging — updating the secret directly is safer.
            Write-Detail "Writing $($domain.CertName) to Key Vault '$KeyVaultName'..."
            $pfxBytes = [System.IO.File]::ReadAllBytes($pfxPath)
            $b64 = [Convert]::ToBase64String($pfxBytes)
            az keyvault secret set `
                --vault-name $KeyVaultName `
                --name $domain.CertName `
                --value $b64 `
                --content-type 'application/x-pkcs12' `
                --only-show-errors | Out-Null

            if ($LASTEXITCODE -ne 0) {
                Write-Fail "Key Vault write failed for $($domain.CertName)"
                exit 1
            }
            Write-Ok "Imported $($domain.CertName) into Key Vault"

            # Update the certificate in Microsoft.Web/certificates (App Service) by redeploying
            # modules/wildcardCert.bicep — App Service certificates read the PFX from Key Vault
            # only at deploy time, they do not auto-refresh when the underlying KV secret changes.
            Write-Detail "Updating App Service certificate '$($domain.CertName)'..."
            az deployment group create `
                --resource-group $AppServicePlanResourceGroup `
                --template-file (Join-Path $PSScriptRoot '../infra/modules/wildcardCert.bicep') `
                --parameters `
                    location=$appServicePlanLocation `
                    appServicePlanId=$appServicePlanId `
                    certResourceName=$($domain.CertName) `
                    keyVaultResourceId=$keyVaultResourceId `
                    keyVaultSecretName=$($domain.CertName) `
                --output none --only-show-errors
            if ($LASTEXITCODE -ne 0) {
                Write-Warning "App Service certificate update failed for $($domain.CertName). Run manually:"
                Write-Warning "  az deployment group create --resource-group $AppServicePlanResourceGroup --template-file infra/modules/wildcardCert.bicep --parameters location=$appServicePlanLocation appServicePlanId=$appServicePlanId certResourceName=$($domain.CertName) keyVaultResourceId=$keyVaultResourceId keyVaultSecretName=$($domain.CertName)"
            } else {
                Write-Ok "Updated App Service certificate '$($domain.CertName)'"
            }

            # Clean up PFX
            Remove-Item $pfxPath -Force
            Write-Detail "Cleaned up temp PFX file"
        }
    }
    finally {
        if ($ipWasAdded) {
            Write-Detail "Removing $currentIp from Key Vault '$KeyVaultName' firewall..."
            az keyvault network-rule remove --name $KeyVaultName --ip-address $ipCidr --output none --only-show-errors
            if ($LASTEXITCODE -ne 0) {
                Write-Warning "Failed to remove IP. Remove manually: az keyvault network-rule remove --name $KeyVaultName --ip-address $ipCidr"
            }
            else {
                Write-Ok "Firewall rule removed"
            }
        }
    }
}

# ============================================================================
# CLEANUP
# ============================================================================

Remove-Item $azureIniPath -Force -ErrorAction SilentlyContinue
Remove-Item $certbotBaseDir -Recurse -Force -ErrorAction SilentlyContinue
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
    Write-Host "  App Service Plan: $AppServicePlanName" -ForegroundColor Gray
}
Write-Host "===============================================================" -ForegroundColor DarkCyan
