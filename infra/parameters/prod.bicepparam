using '../main.bicep'

param location = 'swedencentral'
param environmentName = 'prod'
// Image tags — injected via environment variables by Deploy-Infrastructure.ps1
param apiImageTag = readEnvironmentVariable('API_IMAGE_TAG')
param webImageTag = readEnvironmentVariable('WEB_IMAGE_TAG')
param resourceGroupName = 'rg-techhub-prod'
param appInsightsName = 'appi-techhub-prod'
param containerRegistryName = 'crtechhubms'
param containerAppsEnvName = 'cae-techhub-prod'
param apiAppName = 'ca-techhub-api-prod'
param webAppName = 'ca-techhub-web-prod'
// Networking (10.2.x range — must not overlap with hub 10.100.x or staging 10.1.x)
param vnetName = 'vnet-techhub-prod'
param addressSpacePrefix = '10.2.0.0/16'
param containerAppsSubnetPrefix = '10.2.0.0/23'
param privateEndpointsSubnetPrefix = '10.2.2.0/24'
// PostgreSQL configuration
param postgresServerName = 'psql-techhub-prod'
param postgresAdminLogin = 'techhubadmin'
param postgresAdminPassword = readEnvironmentVariable('POSTGRES_ADMIN_PASSWORD')
// Hub VNet (for peering — VPN access to spoke resources)
param hubVnetId = '/subscriptions/bc8ab567-c645-4e51-9317-992203eb369a/resourceGroups/rg-techhub-shared/providers/Microsoft.Network/virtualNetworks/vnet-techhub-hub'
param hubVnetName = 'vnet-techhub-hub'
// Custom domains — wildcard CNAME in GoDaddy routes all *.hub.ms / *.xebia.ms to the Container App.
// Wildcard certificates from Key Vault cover all subdomains — no per-domain managed certs needed.
// Subdomain shortcuts (e.g. ai.hub.ms → /ai section) are configured in appsettings.json, not here.
param primaryHosts = ['tech.hub.ms', 'tech.xebia.ms']
param wildcardCertNames = {
  'hub.ms': 'wildcard-hub-ms'
  'xebia.ms': 'wildcard-xebia-ms'
}
// Azure AI Foundry (OpenAI)
param openAiName = 'oai-techhub-prod'
param openAiModelCapacity = 100

