targetScope = 'subscription'

// Infrastructure as Code for Tech Hub Azure resources.
// Environment-specific deployments (staging, prod) with shared ACR.
// Trigger: fix deploy-production-infra skip by adding always() guard

@description('Azure region for resources')
param location string = 'westeurope'

@description('Environment name (staging, prod)')
@allowed(['staging', 'prod'])
param environmentName string

@description('Resource group name')
param resourceGroupName string = 'rg-techhub-${environmentName}'

@description('Application Insights name')
param appInsightsName string = 'appi-techhub-${environmentName}'

@description('Shared resource group name (where ACR lives)')
param sharedResourceGroupName string = 'rg-techhub-shared'

@description('Shared Container Registry name (must already exist)')
param containerRegistryName string = 'crtechhubms'

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

@description('VNet name')
param vnetName string = 'vnet-techhub-${environmentName}'

@description('Optional custom domain for the web app (e.g. staging-tech.hub.ms). Leave empty to skip.')
param webCustomDomain string = ''

@description('PostgreSQL server name')
param postgresServerName string = 'psql-techhub-${environmentName}'

@description('PostgreSQL administrator login')
param postgresAdminLogin string = 'techhubadmin'

@secure()
@description('PostgreSQL administrator password')
param postgresAdminPassword string

// Resource Group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: resourceGroupName
  location: location
}

// Reference shared resource group (for cross-RG role assignment)
resource sharedResourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' existing = {
  name: sharedResourceGroupName
}

// User-Assigned Managed Identity (used by Container Apps to pull images from ACR)
module identity './modules/identity.bicep' = {
  scope: resourceGroup
  name: 'identity-deployment'
  params: {
    location: location
    identityName: 'id-techhub-${environmentName}'
  }
}

// Grant AcrPull role to the managed identity on the shared Container Registry
module acrRoleAssignment './modules/acrRoleAssignment.bicep' = {
  scope: sharedResourceGroup
  name: 'acrRole-deployment'
  params: {
    containerRegistryName: containerRegistryName
    principalId: identity.outputs.identityPrincipalId
  }
}

// Networking (VNet + Subnets + Private DNS)
module network './modules/network.bicep' = {
  scope: resourceGroup
  name: 'network-deployment'
  params: {
    location: location
    vnetName: vnetName
  }
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

// Note: Azure AI Foundry (OpenAI) is deployed separately at resource-group level
// rather than as a nested deployment from subscription scope.
// This works around Azure bug 715-123420 where CognitiveServices validation
// fails in nested subscription-level deployments.

// Container Apps Environment (VNet-integrated)
module containerAppsEnv './modules/containerApps.bicep' = {
  scope: resourceGroup
  name: 'containerAppsEnv-deployment'
  params: {
    location: location
    environmentName: containerAppsEnvName
    logAnalyticsWorkspaceId: monitoring.outputs.logAnalyticsWorkspaceId
    infrastructureSubnetId: network.outputs.containerAppsSubnetId
  }
}

// PostgreSQL Flexible Server (private access via VNet)
module postgres './modules/postgres.bicep' = {
  scope: resourceGroup
  name: 'postgres-deployment'
  params: {
    location: location
    serverName: postgresServerName
    administratorLogin: postgresAdminLogin
    administratorLoginPassword: postgresAdminPassword
    delegatedSubnetId: network.outputs.postgresSubnetId
    privateDnsZoneId: network.outputs.privateDnsZoneId
  }
}

// API Container App
module apiApp './modules/api.bicep' = {
  scope: resourceGroup
  name: 'api-deployment'
  dependsOn: [acrRoleAssignment]
  params: {
    location: location
    containerAppName: apiAppName
    containerAppsEnvironmentId: containerAppsEnv.outputs.environmentId
    containerRegistryName: containerRegistryName
    acrPullIdentityId: identity.outputs.identityId
    imageTag: apiImageTag
    appInsightsConnectionString: monitoring.outputs.appInsightsConnectionString
    databaseConnectionString: 'Host=${postgres.outputs.serverFqdn};Database=${postgres.outputs.databaseName};Username=${postgresAdminLogin};Password=${postgresAdminPassword};SSL Mode=Require;Trust Server Certificate=true'
  }
}

// Web Container App
module webApp './modules/web.bicep' = {
  scope: resourceGroup
  name: 'web-deployment'
  dependsOn: [acrRoleAssignment]
  params: {
    location: location
    containerAppName: webAppName
    containerAppsEnvironmentId: containerAppsEnv.outputs.environmentId
    containerAppsEnvironmentName: containerAppsEnvName
    containerRegistryName: containerRegistryName
    acrPullIdentityId: identity.outputs.identityId
    imageTag: webImageTag
    apiBaseUrl: apiApp.outputs.fqdn
    appInsightsConnectionString: monitoring.outputs.appInsightsConnectionString
    customDomain: webCustomDomain
  }
}

// Outputs
output resourceGroupName string = resourceGroup.name
output apiUrl string = 'https://${apiApp.outputs.fqdn}'
output webUrl string = 'https://${webApp.outputs.fqdn}'
output appInsightsName string = monitoring.outputs.appInsightsName
output containerRegistryName string = containerRegistryName
output vnetName string = vnetName
output postgresServerFqdn string = postgres.outputs.serverFqdn
output postgresDatabaseName string = postgres.outputs.databaseName
