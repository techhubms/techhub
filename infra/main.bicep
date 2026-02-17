targetScope = 'subscription'

@description('Azure region for resources')
param location string = 'westeurope'

@description('Azure region for OpenAI (must support model availability)')
param openAiLocation string = 'swedencentral'

@description('Environment name (staging, prod)')
@allowed(['staging', 'prod'])
param environmentName string

@description('Resource group name')
param resourceGroupName string = 'rg-techhub-${environmentName}'

@description('Application Insights name')
param appInsightsName string = 'appi-techhub-${environmentName}'

@description('Shared Container Registry name (must already exist)')
param containerRegistryName string = 'crtechhub'

@description('Container Apps Environment name')
param containerAppsEnvName string = 'cae-techhub-${environmentName}'

@description('API Container App name')
param apiAppName string = 'ca-techhub-api-${environmentName}'

@description('Web Container App name')
param webAppName string = 'ca-techhub-web-${environmentName}'

@description('API Docker image tag')
param apiImageTag string = 'latest'

@description('Web Docker image tag')
param webImageTag string = 'latest'

@description('Azure OpenAI account name')
param openAiName string = 'oai-techhub-${environmentName}'

@description('GPT model deployment name')
param gptDeploymentName string = 'gpt-4.1'

@description('GPT model name')
param gptModelName string = 'gpt-4.1'

@description('GPT model version')
param gptModelVersion string = '2026-01-15'

@description('GPT model capacity (TPM in thousands)')
param gptModelCapacity int = 100

// Resource Group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: resourceGroupName
  location: location
}

// Monitoring (Application Insights + Log Analytics)
module monitoring './modules/monitoring.bicep' = {
  scope: resourceGroup
  name: 'monitoring-deployment'
  params: {
    location: location
    appInsightsName: appInsightsName
    logAnalyticsWorkspaceName: 'law-techhub-${environmentName}'
  }
}

// Azure OpenAI (per-environment for independent testing and quotas)
module openAi './modules/openai.bicep' = {
  scope: resourceGroup
  name: 'openai-deployment'
  params: {
    location: openAiLocation
    openAiName: openAiName
    deploymentName: gptDeploymentName
    modelName: gptModelName
    modelVersion: gptModelVersion
    modelCapacity: gptModelCapacity
  }
}

// Container Apps Environment
module containerAppsEnv './modules/containerApps.bicep' = {
  scope: resourceGroup
  name: 'containerAppsEnv-deployment'
  params: {
    location: location
    environmentName: containerAppsEnvName
    logAnalyticsWorkspaceId: monitoring.outputs.logAnalyticsWorkspaceId
  }
}

// API Container App
module apiApp './modules/api.bicep' = {
  scope: resourceGroup
  name: 'api-deployment'
  params: {
    location: location
    containerAppName: apiAppName
    containerAppsEnvironmentId: containerAppsEnv.outputs.environmentId
    containerRegistryName: containerRegistryName
    imageTag: apiImageTag
    appInsightsConnectionString: monitoring.outputs.appInsightsConnectionString
  }
}

// Web Container App
module webApp './modules/web.bicep' = {
  scope: resourceGroup
  name: 'web-deployment'
  params: {
    location: location
    containerAppName: webAppName
    containerAppsEnvironmentId: containerAppsEnv.outputs.environmentId
    containerRegistryName: containerRegistryName
    imageTag: webImageTag
    apiBaseUrl: apiApp.outputs.fqdn
    appInsightsConnectionString: monitoring.outputs.appInsightsConnectionString
  }
}

// Outputs
output resourceGroupName string = resourceGroup.name
output apiUrl string = 'https://${apiApp.outputs.fqdn}'
output webUrl string = 'https://${webApp.outputs.fqdn}'
output appInsightsName string = monitoring.outputs.appInsightsName
output containerRegistryName string = containerRegistryName
output openAiName string = openAi.outputs.openAiName
output openAiEndpoint string = openAi.outputs.openAiEndpoint
output openAiDeploymentName string = openAi.outputs.deploymentName
