using '../main.bicep'

param location = 'westeurope'
param openAiLocation = 'swedencentral'
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
// Azure AI Foundry configuration
param openAiName = 'oai-techhub-prod'
param gptDeploymentName = 'gpt-5.2'
param gptModelName = 'gpt-5.2'
param gptModelVersion = '2025-12-11'
param gptModelCapacity = 100
// Networking
param vnetName = 'vnet-techhub-prod'
// PostgreSQL configuration
param postgresServerName = 'psql-techhub-prod'
param postgresAdminLogin = 'techhubadmin'
param postgresAdminPassword = readEnvironmentVariable('POSTGRES_ADMIN_PASSWORD')
param allowedClientIp = ''
