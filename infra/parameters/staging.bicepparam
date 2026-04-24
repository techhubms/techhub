using '../main.bicep'

param location = 'swedencentral'
param environmentName = 'staging'
// Image tags — injected via environment variables by Deploy-Infrastructure.ps1
param apiImageTag = readEnvironmentVariable('API_IMAGE_TAG')
param webImageTag = readEnvironmentVariable('WEB_IMAGE_TAG')
param resourceGroupName = 'rg-techhub-staging'
param appInsightsName = 'appi-techhub-staging'
param containerRegistryName = 'crtechhubms'
param containerAppsEnvName = 'cae-techhub-staging'
param apiAppName = 'ca-techhub-api-staging'
param webAppName = 'ca-techhub-web-staging'
// Networking (10.1.x range — must not overlap with hub 10.100.x or prod 10.2.x)
param vnetName = 'vnet-techhub-staging'
param addressSpacePrefix = '10.1.0.0/16'
param containerAppsSubnetPrefix = '10.1.0.0/23'
param privateEndpointsSubnetPrefix = '10.1.2.0/24'
// PostgreSQL configuration
param postgresServerName = 'psql-techhub-staging'
param postgresAdminLogin = 'techhubadmin'
param postgresAdminPassword = readEnvironmentVariable('POSTGRES_ADMIN_PASSWORD')
// Hub VNet (for peering — private endpoint resolution across environments)
param hubVnetId = '/subscriptions/bc8ab567-c645-4e51-9317-992203eb369a/resourceGroups/rg-techhub-shared/providers/Microsoft.Network/virtualNetworks/vnet-techhub-hub'
param hubVnetName = 'vnet-techhub-hub'
// Custom domains — wildcard CNAME in GoDaddy routes all *.hub.ms to the Container App.
// Wildcard certificate from Key Vault — no per-domain managed certs needed.
param primaryHosts = ['staging-tech.hub.ms']
param wildcardCertNames = {
  'hub.ms': 'wildcard-hub-ms'
}
// Azure AI Foundry (OpenAI)
param openAiName = 'oai-techhub-staging'
param openAiModelCapacity = 100
// Admin IP allow-list — grants firewall access to PostgreSQL and Key Vault.
// MUST be set via ADMIN_IP_ADDRESSES env var — no default to prevent leaking IPs into git.
param adminIpAddresses = readEnvironmentVariable('ADMIN_IP_ADDRESSES')
// Azure AD — admin dashboard authentication (same app registration as prod, different redirect URIs registered)
// Tenant ID and Client ID are public Entra identifiers (issuer URL contains tenant ID).
// The actual Client Secret lives in Key Vault — see scripts/Sync-KeyVaultSecrets.ps1.
param azureAdTenantId = readEnvironmentVariable('AZURE_AD_TENANT_ID')
param azureAdClientId = readEnvironmentVariable('AZURE_AD_CLIENT_ID')
// Shared action group resource ID — resolved automatically by Deploy-Infrastructure.ps1.
// Leave empty to skip alert creation (e.g. when shared infra has not yet been deployed).
param actionGroupId = readEnvironmentVariable('ACTION_GROUP_ID', '')

