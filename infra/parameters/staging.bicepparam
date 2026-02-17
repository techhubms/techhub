using '../main.bicep'

param location = 'westeurope'
param openAiLocation = 'swedencentral'
param environmentName = 'staging'
param resourceGroupName = 'rg-techhub-staging'
param appInsightsName = 'appi-techhub-staging'
param containerRegistryName = 'crtechhub'
param containerAppsEnvName = 'cae-techhub-staging'
param apiAppName = 'ca-techhub-api-staging'
param webAppName = 'ca-techhub-web-staging'
// Use placeholder image for initial deployment - workflow will immediately update with real images
param apiImageTag = 'initial'
param webImageTag = 'initial'
// Azure OpenAI configuration
param openAiName = 'oai-techhub-staging'
param gptDeploymentName = 'gpt-4.1'
param gptModelName = 'gpt-4.1'
param gptModelVersion = '2025-04-14'
param gptModelCapacity = 50
