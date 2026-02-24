using '../main.bicep'

param location = 'swedencentral'
param openAiLocation = 'swedencentral'
param environmentName = 'staging'
param resourceGroupName = 'rg-techhub-staging'
param appInsightsName = 'appi-techhub-staging'
param containerRegistryName = 'crtechhubms'
param containerAppsEnvName = 'cae-techhub-staging'
param apiAppName = 'ca-techhub-api-staging'
param webAppName = 'ca-techhub-web-staging'
// Use staging-latest images built and pushed to ACR
param apiImageTag = 'staging-latest'
param webImageTag = 'staging-latest'
// Azure AI Foundry configuration
param openAiName = 'oai-techhub-staging'
param gptDeploymentName = 'gpt-5.2'
param gptModelName = 'gpt-5.2'
param gptModelVersion = '2025-12-11'
param gptModelCapacity = 50
// Networking
param vnetName = 'vnet-techhub-staging'
// PostgreSQL configuration
param postgresServerName = 'psql-techhub-staging'
param postgresAdminLogin = 'techhubadmin'
param postgresAdminPassword = readEnvironmentVariable('POSTGRES_ADMIN_PASSWORD')
