#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Rotates YouTube cookies in Key Vault for transcript fetching.

.DESCRIPTION
    Interactive script that prompts for YouTube cookie values and writes them
    as a single semicolon-delimited secret to the shared Key Vault.

    The cookies help YoutubeExplode bypass YouTube's EU consent wall and
    appear more like a real browser. Only anonymous/consent cookies are
    requested — no login/session cookies that could risk account bans.

    Cookies to provide (extract from browser DevTools > Application > Cookies > youtube.com):
        PREF                     — Browser preferences (timezone, language)
        SOCS                     — EU consent acceptance cookie
        VISITOR_PRIVACY_METADATA — GDPR/consent cookie (bypasses EU consent wall)

    The secret is stored as:
        techhub-<env>-youtube-cookies = "PREF=<value>;SOCS=<value>;VISITOR_PRIVACY_METADATA=<value>"

    After rotating, restart the Container App revision to pick up the new values.

    Requires:
        - Azure CLI (`az`) authenticated with Key Vault Secrets Officer role
        - Network access to the Key Vault (script handles firewall rules)

.PARAMETER Environment
    Target environment: 'staging' or 'prod'.

.PARAMETER KeyVaultName
    Key Vault name. Defaults to 'kv-techhub-shared'.

.EXAMPLE
    ./scripts/Rotate-YouTubeCookies.ps1 -Environment prod
#>

param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('staging', 'prod')]
    [string]$Environment,

    [Parameter(Mandatory = $false)]
    [string]$KeyVaultName = 'kv-techhub-shared'
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

# --- Collect cookie values interactively ---
Write-Host ""
Write-Host "YouTube Cookie Rotation" -ForegroundColor Cyan
Write-Host "========================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Extract these values from your browser:" -ForegroundColor Yellow
Write-Host "  DevTools > Application > Cookies > https://www.youtube.com" -ForegroundColor Yellow
Write-Host ""
Write-Host "Only anonymous cookies are needed. Do NOT provide login cookies (SID, HSID, LOGIN_INFO, etc.)." -ForegroundColor Yellow
Write-Host ""

$cookieNames = @('PREF', 'SOCS', 'VISITOR_PRIVACY_METADATA')
$cookieParts = @()

foreach ($name in $cookieNames) {
    $value = Read-Host -Prompt "  $($name)"
    if ([string]::IsNullOrWhiteSpace($value)) {
        Write-Host "   [SKIP] $($name) — no value provided, will be omitted." -ForegroundColor Gray
        continue
    }
    $cookieParts += "$($name)=$($value.Trim())"
}

if ($cookieParts.Count -eq 0) {
    Write-Host ""
    Write-Host "No cookies provided. Nothing to write." -ForegroundColor Yellow
    exit 0
}

$cookieString = $cookieParts -join ';'
$secretName = "techhub-$($Environment)-youtube-cookies"

Write-Host ""
Write-Host "Will write $($cookieParts.Count) cookie(s) to secret '$($secretName)' in '$($KeyVaultName)'." -ForegroundColor Cyan

# --- Detect outbound IP ---
$currentIp = $null
foreach ($provider in @('https://checkip.amazonaws.com', 'https://api.ipify.org', 'https://icanhazip.com')) {
    try {
        $response = (Invoke-RestMethod -Uri $provider -TimeoutSec 10).Trim()
        if ($response -match '^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$') {
            $currentIp = $response
            break
        }
        Write-Warning "IP provider '$provider' returned unexpected response: '$response'"
    }
    catch {
        Write-Warning "IP provider '$provider' failed: $($_.Exception.Message)"
    }
}
if (-not $currentIp) {
    throw "Failed to detect outbound IP from any provider. Cannot add Key Vault firewall rule."
}
$ipCidr = "$currentIp/32"

# --- Check if IP is already allowed ---
$existingRules = az keyvault network-rule list `
    --name $KeyVaultName `
    --query 'ipRules[].value' `
    --output tsv 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Warning "Failed to list Key Vault network rules — proceeding anyway."
    $existingRules = $null
}
$existingRuleValues = @()
if ($existingRules) {
    $existingRuleValues = @(($existingRules -split "`n") |
        ForEach-Object { $_.Trim() } |
        Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
}
$ruleAlreadyPresent = $existingRuleValues -contains $currentIp -or $existingRuleValues -contains $ipCidr

$ipWasAdded = $false
try {
    # --- Add firewall rule if needed ---
    if (-not $ruleAlreadyPresent) {
        Write-Host "Adding IP $currentIp to Key Vault firewall..." -ForegroundColor Cyan
        az keyvault network-rule add `
            --name $KeyVaultName `
            --ip-address $ipCidr `
            --output none
        if ($LASTEXITCODE -ne 0) {
            throw "Failed to add IP to Key Vault firewall."
        }
        $ipWasAdded = $true

        Write-Host "   Waiting for firewall rule to propagate..." -ForegroundColor Gray
        $maxWaitSecs = 60
        $elapsed = 0
        $propagated = $false
        while ($elapsed -lt $maxWaitSecs -and -not $propagated) {
            Start-Sleep -Seconds 5
            $elapsed += 5
            az keyvault secret list --vault-name $KeyVaultName --query '[]' --output none 2>$null
            if ($LASTEXITCODE -eq 0) {
                $propagated = $true
                Write-Host "   Firewall rule active after ${elapsed}s." -ForegroundColor Gray
            }
            else {
                Write-Host "   Still propagating (${elapsed}s / ${maxWaitSecs}s)..." -ForegroundColor Gray
            }
        }
        if (-not $propagated) {
            Write-Warning "Firewall rule may not have propagated after ${maxWaitSecs}s — proceeding anyway."
        }
    }
    else {
        Write-Host "   IP $currentIp is already permitted by the Key Vault firewall." -ForegroundColor Gray
    }

    # --- Write the secret via temp file (no secrets in process args) ---
    $tmpFile = [System.IO.Path]::GetTempFileName()
    try {
        [System.IO.File]::WriteAllText($tmpFile, $cookieString)
        az keyvault secret set `
            --vault-name $KeyVaultName `
            --name $secretName `
            --file $tmpFile `
            --output none
    }
    finally {
        Remove-Item $tmpFile -Force -ErrorAction SilentlyContinue
    }

    if ($LASTEXITCODE -ne 0) {
        throw "Failed to write secret '$($secretName)' to Key Vault."
    }

    Write-Host ""
    Write-Host "Secret '$($secretName)' written to '$($KeyVaultName)'." -ForegroundColor Green
    Write-Host ""
    Write-Host "To apply immediately, restart the Container App revision:" -ForegroundColor Yellow
    Write-Host "  az containerapp revision restart -n ca-techhub-api-$($Environment) -g rg-techhub-$($Environment) --revision <name>" -ForegroundColor Yellow
}
catch {
    Write-Error "Rotate-YouTubeCookies.ps1 failed: $($_.Exception.Message)"
    throw
}
finally {
    if ($ipWasAdded) {
        Write-Host "Removing IP $currentIp from Key Vault firewall..." -ForegroundColor Cyan
        az keyvault network-rule remove `
            --name $KeyVaultName `
            --ip-address $ipCidr `
            --output none
        if ($LASTEXITCODE -ne 0) {
            Write-Warning "Failed to remove IP from firewall. Remove manually: az keyvault network-rule remove --name $($KeyVaultName) --ip-address $ipCidr"
        }
    }
}
