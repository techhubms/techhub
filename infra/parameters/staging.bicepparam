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
// Networking
param vnetName = 'vnet-techhub-staging'
// PostgreSQL configuration
param postgresServerName = 'psql-techhub-staging'
param postgresAdminLogin = 'techhubadmin'
param postgresAdminPassword = readEnvironmentVariable('POSTGRES_ADMIN_PASSWORD')
// Custom domains (requires CNAME + TXT record in GoDaddy DNS first)
param primaryHosts = ['staging-tech.hub.ms']
param subdomainShortcuts = {}
