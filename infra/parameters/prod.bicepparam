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
// Custom domains (requires CNAME + TXT records in GoDaddy DNS first)
param webCustomDomains = ['tech.hub.ms', 'tech.xebia.ms']
