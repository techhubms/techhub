targetScope = 'subscription'

// Infrastructure as Code for Tech Hub Azure resources.
// Environment-specific deployments (staging, prod) with shared ACR.
// Trigger: deleted westeurope resources, redeploy to swedencentral

@description('Azure region for resources')
param location string = 'swedencentral'

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

@description('API Docker image tag (yyyyMMddHHmmss format)')
param apiImageTag string

@description('Web Docker image tag (yyyyMMddHHmmss format)')
param webImageTag string

@description('VNet name')
param vnetName string = 'vnet-techhub-${environmentName}'

@description('Primary host names for the web app (e.g. ["tech.hub.ms", "tech.xebia.ms"]). Leave empty to use default Container Apps FQDN.')
param primaryHosts array = []

@description('Subdomain shortcut mapping (e.g. { ai: "ai", ghc: "github-copilot" }). Each subdomain gets a custom domain for every base domain derived from primaryHosts.')
param subdomainShortcuts object = {}

// Derive base domains from primary hosts (e.g., 'tech.hub.ms' -> 'hub.ms')
// and compute the full list of custom domains (primary hosts + subdomain entries)
var baseDomains = [for host in primaryHosts: substring(host, indexOf(host, '.') + 1)]
var subdomainKeys = objectKeys(subdomainShortcuts)
var subdomainDomains = flatten(map(baseDomains, baseDomain => map(subdomainKeys, key => '${key}.${baseDomain}')))
var allCustomDomains = concat(primaryHosts, subdomainDomains)

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
    webFqdns: !empty(primaryHosts) ? primaryHosts : ['${webAppName}.${containerAppsEnv.outputs.defaultDomain}']
    environmentName: environmentName
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
    customDomains: allCustomDomains
    subdomainShortcuts: subdomainShortcuts
    primaryHosts: primaryHosts
    environmentName: environmentName
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
