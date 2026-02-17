using '../main.bicep'

param location = 'westeurope'
param openAiLocation = 'swedencentral'
param environmentName = 'prod'
param resourceGroupName = 'rg-techhub-prod'
param appInsightsName = 'appi-techhub-prod'
param containerRegistryName = 'crtechhub'
param containerAppsEnvName = 'cae-techhub-prod'
param apiAppName = 'ca-techhub-api-prod'
param webAppName = 'ca-techhub-web-prod'
// Use placeholder image for initial deployment - workflow will immediately update with real images
param apiImageTag = 'initial'
param webImageTag = 'initial'
// Azure OpenAI configuration
param openAiName = 'oai-techhub-prod'
param gptDeploymentName = 'gpt-4.1'
param gptModelName = 'gpt-4.1'
param gptModelVersion = '2025-04-14'
param gptModelCapacity = 100
