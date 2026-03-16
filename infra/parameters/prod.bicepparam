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
// Networking
param vnetName = 'vnet-techhub-prod'
// PostgreSQL configuration
param postgresServerName = 'psql-techhub-prod'
param postgresAdminLogin = 'techhubadmin'
param postgresAdminPassword = readEnvironmentVariable('POSTGRES_ADMIN_PASSWORD')
// Custom domains — primary hosts + subdomain shortcuts (requires CNAME records in GoDaddy DNS)
// Wildcard CNAME in GoDaddy routes all *.hub.ms / *.xebia.ms traffic to the Container App.
// Each shortcut subdomain is registered explicitly so Azure can issue managed certificates.
param primaryHosts = ['tech.hub.ms', 'tech.xebia.ms']
param subdomainShortcuts = {
  all: 'all'
  'github-copilot': 'github-copilot'
  ghc: 'github-copilot'
  ai: 'ai'
  ml: 'ml'
  devops: 'devops'
  azure: 'azure'
  dotnet: 'dotnet'
  security: 'security'
}
