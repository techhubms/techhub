#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Syncs application secrets from env vars into the production Tech Hub Key Vault.

.DESCRIPTION
    Tech Hub Container Apps reference secrets via `keyVaultUrl` instead of inline
    ARM values (see infra/modules/api.bicep and infra/modules/web.bicep). This
    script pushes the current values from environment variables into the production
    Key Vault using Azure CLI, so the Bicep deploy can reference them.

    Secrets written:
        techhub-prod-aad-client-secret      — Entra client secret
        techhub-github-registry-token       — GHCR PAT for pulling container images
        techhub-prod-newsletter-acs-endpoint — ACS email endpoint URL
        techhub-prod-postgres-admin-password — PostgreSQL admin password (for re-deploys without re-specifying)
        wildcard-hub-ms                     — Wildcard TLS certificate for *.hub.ms
        wildcard-xebia-ms                   — Wildcard TLS certificate for *.xebia.ms

    Secrets no longer written (replaced by managed identity / RBAC):
        techhub-prod-db-connection-string   — removed; app uses Entra token auth (Database:UseEntraAuth=true)
        techhub-prod-ai-api-key             — removed; app uses Cognitive Services OpenAI User RBAC role

    This script is called AUTOMATICALLY by Deploy-Infrastructure.ps1 in deploy
    mode, so you normally do not need to run it manually. The CI/CD workflow
    provides AZURE_AD_CLIENT_SECRET as a GitHub secret.

    Manual workflow (from an admin machine allowed through the KV firewall):
        1. az login
        2. Set env vars: AZURE_AD_CLIENT_SECRET, GHCR_PAT, NEWSLETTER_ACS_ENDPOINT,
           POSTGRES_ADMIN_PASSWORD, WILDCARD_CERT_HUB_MS, WILDCARD_CERT_XEBIA_MS
        3. ./scripts/Sync-KeyVaultSecrets.ps1

    Fail-fast behaviour: if a secret value is empty AND the secret does not yet
    exist in Key Vault, the script throws so the caller can fix the missing value
    before the Bicep deployment tries to reference it. If the secret already
    exists in Key Vault, an empty value causes it to be skipped (existing value
    is left unchanged — useful for re-deploying without re-specifying stable secrets).

    Requires the caller to have both:
      - 'Key Vault Contributor' (ARM management-plane — needed to add/remove network firewall rules)
      - 'Key Vault Secrets Officer' (data-plane — needed to read/write secrets)
    Or simply 'Key Vault Administrator' which covers both.

.PARAMETER KeyVaultName
    Production Key Vault name. Defaults to 'kv-techhub-prod'.
#>

param(
    [Parameter(Mandatory = $false)]
    [string]$KeyVaultName = 'kv-techhub-prod'
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

function Set-KvSecret {
    param(
        [Parameter(Mandatory = $true)][string]$Name,
        [Parameter(Mandatory = $true)][AllowEmptyString()][string]$Value,
        [Parameter(Mandatory = $true)][string]$Description
    )

    if ([string]::IsNullOrWhiteSpace($Value)) {
        # Value is empty — check whether the secret already exists in Key Vault.
        # If it does, leave it as-is (stable secrets don't need to be re-specified on every deploy).
        # If it doesn't, fail fast so the crash-loop caused by a missing KV reference is caught here.
        # Use ARM management-plane API to check existence (works even without data-plane RBAC).
        $armUrl = "https://management.azure.com/subscriptions/$((az account show --query id -o tsv))/resourceGroups/rg-techhub-prod/providers/Microsoft.KeyVault/vaults/$KeyVaultName/secrets/${Name}?api-version=2022-07-01"
        $armResult = az rest --method get --url $armUrl 2>$null
        if ($LASTEXITCODE -eq 0 -and $armResult) {
            Write-Host "   [SKIP] $($Description): no new value provided — existing secret kept." -ForegroundColor Gray
            return
        }
        throw "$($Description): value is empty and the secret '$($Name)' does not exist in Key Vault '$($KeyVaultName)'. " +
              "Set the required env var before deploying."
    }

    Write-Host "Writing $($Name) ($($Description))..." -ForegroundColor Cyan

    # Write the secret value via a temp file so the value never appears in process arguments
    # or shell history (avoids exposure through /proc/<pid>/cmdline and logging).
    $tmpFile = [System.IO.Path]::GetTempFileName()
    try {
        [System.IO.File]::WriteAllText($tmpFile, $Value)
        az keyvault secret set `
            --vault-name $KeyVaultName `
            --name $Name `
            --file $tmpFile `
            --output none
    }
    finally {
        Remove-Item $tmpFile -Force -ErrorAction SilentlyContinue
    }

    if ($LASTEXITCODE -ne 0) {
        throw "Failed to write secret '$($Name)' to Key Vault '$($KeyVaultName)'."
    }
}

# --- Collect values from env ---
$aadClientSecret       = $env:AZURE_AD_CLIENT_SECRET
$ghcrToken             = $env:GHCR_PAT
$newsletterAcsEndpoint = $env:NEWSLETTER_ACS_ENDPOINT
$postgresAdminPassword = $env:POSTGRES_ADMIN_PASSWORD
$wildcardHubMs         = $env:WILDCARD_CERT_HUB_MS
$wildcardXebiaMs       = $env:WILDCARD_CERT_XEBIA_MS

# --- Temporarily add this machine's IP to the Key Vault firewall ---
# Key Vault is IP-restricted. GitHub Actions runners have dynamic IPs that are not in the
# static allow-list, so we add the current outbound IP before writing secrets and remove it
# in a finally block so the rule is always cleaned up even if the sync fails.
# Detect outbound IP — try multiple providers and validate the response is a valid IPv4 address.
# This prevents a hard deployment failure if checkip.amazonaws.com is temporarily unavailable.
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

$existingRules = az keyvault network-rule list `
    --name $KeyVaultName `
    --query 'ipRules[].value' `
    --output tsv 2>&1
if ($LASTEXITCODE -ne 0) {
    $existingRulesError = ($existingRules | Out-String).Trim()
    if ([string]::IsNullOrWhiteSpace($existingRulesError)) {
        $existingRulesError = 'Azure CLI returned a non-zero exit code with no error output.'
    }
    Write-Warning "Failed to list existing Key Vault network rules for '$KeyVaultName': $existingRulesError"
    $existingRules = $null
}
# Normalize existing rules and use exact matching to avoid false positives.
# e.g. IP 1.2.3.4 must not match an existing rule 1.2.3.40/32.
$existingRuleValues = @()
if ($existingRules) {
    $existingRuleValues = @(($existingRules -split "`n") |
        ForEach-Object { $_.Trim() } |
        Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
}
$ruleAlreadyPresent = $existingRuleValues -contains $currentIp -or $existingRuleValues -contains $ipCidr

$ipWasAdded = $false
# --- Add firewall rule and write secrets ---
# The firewall rule add is inside the try block so the finally cleanup always runs if the
# add succeeds but a subsequent step fails.
try {
    if (-not $ruleAlreadyPresent) {
        Write-Host "Adding current IP $currentIp to Key Vault '$($KeyVaultName)' firewall..." -ForegroundColor Cyan
        az keyvault network-rule add `
            --name $KeyVaultName `
            --ip-address $ipCidr `
            --output none
        if ($LASTEXITCODE -ne 0) {
            throw "Failed to add IP $currentIp to Key Vault '$($KeyVaultName)' firewall."
        }
        $ipWasAdded = $true
        # Poll until the rule takes effect instead of a fixed sleep, so CI runs don't wait
        # longer than needed and won't time out if propagation is slow.
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
        Write-Host "   IP $currentIp is already permitted by Key Vault '$($KeyVaultName)' firewall." -ForegroundColor Gray
    }

    Set-KvSecret -Name "techhub-prod-aad-client-secret"       -Value $aadClientSecret       -Description 'Entra client secret'
    Set-KvSecret -Name "techhub-github-registry-token"        -Value $ghcrToken             -Description 'GitHub Container Registry PAT'
    Set-KvSecret -Name "techhub-prod-newsletter-acs-endpoint" -Value $newsletterAcsEndpoint -Description 'ACS email endpoint URL'
    Set-KvSecret -Name "techhub-prod-postgres-admin-password" -Value $postgresAdminPassword -Description 'PostgreSQL admin password'
    Set-KvSecret -Name "wildcard-hub-ms"                      -Value $wildcardHubMs         -Description 'Wildcard TLS certificate (*.hub.ms)'
    Set-KvSecret -Name "wildcard-xebia-ms"                    -Value $wildcardXebiaMs       -Description 'Wildcard TLS certificate (*.xebia.ms)'

    Write-Host ""
    Write-Host "All secrets synchronised into '$($KeyVaultName)'." -ForegroundColor Green
    Write-Host "Container Apps pick up new values on next revision. Restart a revision to apply immediately." -ForegroundColor Yellow
}
catch {
    Write-Error "Sync-KeyVaultSecrets.ps1 failed: $($_.Exception.Message)"
    throw
}
finally {
    if ($ipWasAdded) {
        Write-Host "Removing current IP $currentIp from Key Vault '$($KeyVaultName)' firewall..." -ForegroundColor Cyan
        az keyvault network-rule remove `
            --name $KeyVaultName `
            --ip-address $ipCidr `
            --output none
        if ($LASTEXITCODE -ne 0) {
            Write-Warning "Failed to remove IP $currentIp from Key Vault firewall. Remove manually: az keyvault network-rule remove --name $($KeyVaultName) --ip-address $ipCidr"
        }
    }
}
