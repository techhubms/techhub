using '../main.bicep'

param location = 'westeurope'
param environmentName = 'prod'
param resourceGroupName = 'rg-techhub-prod'
param appInsightsName = 'appi-techhub-prod'
param containerRegistryName = 'crtechhubms'
param containerAppsEnvName = 'cae-techhub-prod'
param apiAppName = 'ca-techhub-api-prod'
param webAppName = 'ca-techhub-web-prod'
// Use placeholder image for initial deployment - workflow will immediately update with real images
param apiImageTag = 'initial'
param webImageTag = 'initial'
// Networking
param vnetName = 'vnet-techhub-prod'
// PostgreSQL configuration
param postgresServerName = 'psql-techhub-prod'
param postgresAdminLogin = 'techhubadmin'
param postgresAdminPassword = readEnvironmentVariable('POSTGRES_ADMIN_PASSWORD')
