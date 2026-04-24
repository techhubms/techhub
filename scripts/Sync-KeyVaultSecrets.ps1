#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Syncs application secrets from env vars into the shared Tech Hub Key Vault.

.DESCRIPTION
    Tech Hub Container Apps reference secrets via `keyVaultUrl` instead of inline
    ARM values (see infra/modules/api.bicep and infra/modules/web.bicep). This
    script pushes the current values from environment variables into the shared
    Key Vault using Azure CLI, so the Bicep deploy can reference them.

    Secrets written (per environment):
        techhub-<env>-db-connection-string   — full PostgreSQL connection string
        techhub-<env>-ai-api-key             — AI Foundry API key
        techhub-<env>-aad-client-secret      — Entra client secret

    Typical workflow (from an admin machine allowed through the KV firewall):
        1. az login
        2. Set env vars: POSTGRES_ADMIN_PASSWORD, AI_API_KEY, AZURE_AD_CLIENT_SECRET
        3. ./scripts/Sync-KeyVaultSecrets.ps1 -Environment prod

    Requires the caller to have the 'Key Vault Secrets Officer' role on the vault.

.PARAMETER Environment
    Target environment name: 'staging' or 'prod'.

.PARAMETER KeyVaultName
    Shared Key Vault name. Defaults to 'kv-techhub-shared'.

.PARAMETER PostgresHost
    PostgreSQL server FQDN. Defaults to the convention 'psql-techhub-<env>.postgres.database.azure.com'.

.PARAMETER PostgresUser
    PostgreSQL admin login. Defaults to 'techhubadmin'.

.PARAMETER PostgresDatabase
    Database name. Defaults to 'techhub'.
#>

param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('staging', 'prod')]
    [string]$Environment,

    [Parameter(Mandatory = $false)]
    [string]$KeyVaultName = 'kv-techhub-shared',

    [Parameter(Mandatory = $false)]
    [string]$PostgresHost,

    [Parameter(Mandatory = $false)]
    [string]$PostgresUser = 'techhubadmin',

    [Parameter(Mandatory = $false)]
    [string]$PostgresDatabase = 'techhub'
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

if (-not $PostgresHost) {
    $PostgresHost = "psql-techhub-$($Environment).postgres.database.azure.com"
}

function Set-KvSecret {
    param(
        [Parameter(Mandatory = $true)][string]$Name,
        [Parameter(Mandatory = $true)][string]$Value,
        [Parameter(Mandatory = $true)][string]$Description
    )

    if ([string]::IsNullOrWhiteSpace($Value)) {
        Write-Warning "$($Description): value is empty. Skipping."
        return
    }

    Write-Host "Writing $($Name) ($($Description))..." -ForegroundColor Cyan
    az keyvault secret set `
        --vault-name $KeyVaultName `
        --name $Name `
        --value $Value `
        --output none

    if ($LASTEXITCODE -ne 0) {
        throw "Failed to write secret '$($Name)' to Key Vault '$($KeyVaultName)'."
    }
}

# --- Collect values from env ---
$postgresPassword = $env:POSTGRES_ADMIN_PASSWORD
$aiApiKey = $env:AI_API_KEY
$aadClientSecret = $env:AZURE_AD_CLIENT_SECRET

if ([string]::IsNullOrWhiteSpace($postgresPassword)) {
    throw "POSTGRES_ADMIN_PASSWORD env var is required."
}

$dbConnectionString = "Host=$($PostgresHost);Database=$($PostgresDatabase);Username=$($PostgresUser);Password=$($postgresPassword);SSL Mode=Require"

# --- Write secrets ---
try {
    Set-KvSecret -Name "techhub-$($Environment)-db-connection-string" -Value $dbConnectionString -Description 'PostgreSQL connection string'
    Set-KvSecret -Name "techhub-$($Environment)-ai-api-key" -Value $aiApiKey -Description 'AI Foundry API key'
    Set-KvSecret -Name "techhub-$($Environment)-aad-client-secret" -Value $aadClientSecret -Description 'Entra client secret'

    Write-Host ""
    Write-Host "All secrets synchronised into '$($KeyVaultName)' for environment '$($Environment)'." -ForegroundColor Green
    Write-Host "Container Apps pick up new values on next revision. Restart a revision to apply immediately." -ForegroundColor Yellow
}
catch {
    Write-Error "Sync-KeyVaultSecrets.ps1 failed: $($_.Exception.Message)"
    throw
}
