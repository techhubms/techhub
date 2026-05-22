using '../main.bicep'

param location = 'swedencentral'
// Image tags — injected via environment variables by Deploy-Infrastructure.ps1
param apiImageTag = readEnvironmentVariable('API_IMAGE_TAG')
param webImageTag = readEnvironmentVariable('WEB_IMAGE_TAG')
param resourceGroupName = 'rg-techhub-prod'
param appInsightsName = 'appi-techhub-prod'
param keyVaultName = 'kv-techhub-prod'
param containerAppsEnvName = 'cae-techhub-prod'
param apiAppName = 'ca-techhub-api-prod'
param webAppName = 'ca-techhub-web-prod'
// Networking
param vnetName = 'vnet-techhub-prod'
param addressSpacePrefix = '10.2.0.0/16'
param containerAppsSubnetPrefix = '10.2.0.0/23'
// Container Apps subnet IP range for PostgreSQL firewall rule (Bicep cannot parse CIDR).
// These must match containerAppsSubnetPrefix = '10.2.0.0/23'. Update all three if the subnet changes.
// Also update the matching constants in scripts/Deploy-PrPreview.ps1 if the subnet changes.
param containerAppsSubnetStartIp = '10.2.0.0'
param containerAppsSubnetEndIp = '10.2.1.255'
// PostgreSQL configuration
param postgresServerName = 'psql-techhub-prod'
param postgresAdminLogin = 'techhubadmin'
param postgresAdminPassword = readEnvironmentVariable('POSTGRES_ADMIN_PASSWORD')
// Custom domains — wildcard CNAME in GoDaddy routes all *.hub.ms / *.xebia.ms to the Container App.
// Wildcard certificates from Key Vault cover all subdomains — no per-domain managed certs needed.
param primaryHosts = ['tech.hub.ms', 'tech.xebia.ms']
param wildcardCertNames = {
  'hub.ms': 'wildcard-hub-ms'
  'xebia.ms': 'wildcard-xebia-ms'
}
// Azure AI Foundry (OpenAI)
param openAiName = 'oai-techhub-prod'
param openAiModelCapacity = 200
// Admin IP allow-list — grants firewall access to PostgreSQL and Key Vault.
// MUST be set via ADMIN_IP_ADDRESSES env var — no default to prevent leaking IPs into git.
param adminIpAddresses = readEnvironmentVariable('ADMIN_IP_ADDRESSES')
// Azure AD — admin dashboard authentication
// Tenant ID and Client ID are public Entra identifiers (issuer URL contains tenant ID).
// The actual Client Secret lives in Key Vault — see scripts/Sync-KeyVaultSecrets.ps1.
param azureAdTenantId = '3d4d17ea-1ae4-4705-947e-51369c5a5f79'
param azureAdClientId = '6f993c39-347a-49a2-a854-836d07358905'
// Alerts and budget
param alertEmailAddress = 'reinier.vanmaanen@xebia.com'
param monthlyBudgetAmount = 250
param budgetStartDate = '2026-04-01'
// GitHub Container Registry
param githubRegistryUsername = 'techhubms'
// Phase 1: skip Container Apps so Key Vault exists before secrets are synced
param deployApps = false
