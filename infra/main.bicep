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

@description('Shared resource group name (where ACR and Key Vault live)')
param sharedResourceGroupName string = 'rg-techhub-shared'

@description('Shared Key Vault name (stores wildcard certificates)')
param keyVaultName string = 'kv-techhub-shared'

@description('Shared Container Registry name (must already exist)')
param containerRegistryName string = 'crtechhubms'

@description('Hub VNet resource ID (for VNet peering and DNS zone links)')
param hubVnetId string = ''

@description('Hub VNet name (in shared resource group)')
param hubVnetName string = 'vnet-techhub-hub'

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

@description('VNet address space (must be unique per environment)')
param addressSpacePrefix string = '10.0.0.0/16'

@description('Container Apps subnet prefix')
param containerAppsSubnetPrefix string = '10.0.0.0/23'

@description('Private endpoints subnet prefix')
param privateEndpointsSubnetPrefix string = '10.0.2.0/24'

@description('Primary host names for the web app (e.g. ["tech.hub.ms", "tech.xebia.ms"]). Leave empty to use default Container Apps FQDN.')
param primaryHosts array = []

// Custom domains are just the primary hosts — wildcard DNS + wildcard certificates
// handle all subdomains automatically. Subdomain shortcuts are configured in appsettings.json.
var allCustomDomains = primaryHosts

@description('Wildcard certificate names in Key Vault, keyed by base domain (e.g. { "hub.ms": "wildcard-hub-ms" }). Leave empty to use Azure managed certificates.')
param wildcardCertNames object = {}

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
    addressSpacePrefix: addressSpacePrefix
    containerAppsSubnetPrefix: containerAppsSubnetPrefix
    privateEndpointsSubnetPrefix: privateEndpointsSubnetPrefix
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

// VNet Peering: spoke → hub (use hub's VPN gateway for P2S access)
module peeringSpokeToHub './modules/vnetPeering.bicep' = if (!empty(hubVnetId)) {
  scope: resourceGroup
  name: 'peering-spoke-to-hub'
  params: {
    localVnetName: vnetName
    remoteVnetId: hubVnetId
    peeringName: 'peer-${vnetName}-to-hub'
    allowForwardedTraffic: true
    useRemoteGateways: true
  }
  dependsOn: [network, peeringHubToSpoke]
}

// VNet Peering: hub → spoke (allow gateway transit so VPN clients can reach spoke resources)
module peeringHubToSpoke './modules/vnetPeering.bicep' = if (!empty(hubVnetId)) {
  scope: sharedResourceGroup
  name: 'peering-hub-to-${environmentName}'
  params: {
    localVnetName: hubVnetName
    remoteVnetId: network.outputs.vnetId
    peeringName: 'peer-hub-to-${vnetName}'
    allowForwardedTraffic: true
    allowGatewayTransit: true
  }
}

// Link Key Vault private DNS zone (in shared RG) to this spoke VNet
// so Container Apps and services in this environment can resolve the KV private endpoint
module kvDnsLink './modules/privateDnsZoneLink.bicep' = if (!empty(hubVnetId)) {
  scope: sharedResourceGroup
  name: 'kvDnsLink-${environmentName}'
  params: {
    dnsZoneName: 'privatelink.vaultcore.azure.net'
    linkName: 'link-${vnetName}'
    vnetId: network.outputs.vnetId
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

// Wildcard certificates from Key Vault → Container Apps Environment
// The managed identity needs Key Vault Secrets User on the shared KV.
module wildcardCerts './modules/wildcardCertificates.bicep' = if (!empty(wildcardCertNames)) {
  scope: resourceGroup
  params: {
    location: location
    containerAppsEnvironmentName: containerAppsEnvName
    keyVaultName: keyVaultName
    sharedResourceGroupName: sharedResourceGroupName
    wildcardCertNames: wildcardCertNames
    identityId: identity.outputs.identityId
    identityPrincipalId: identity.outputs.identityPrincipalId
  }
  dependsOn: [containerAppsEnv]
}

// Build a map of base domain → certificate resource ID for the web module.
// BCP318 warnings are safe: the !empty() guard ensures outputs are only accessed when the module deployed.
#disable-next-line BCP318
var _certDomains = !empty(wildcardCertNames) ? wildcardCerts.outputs.certDomains : []
#disable-next-line BCP318
var _certIds = !empty(wildcardCertNames) ? wildcardCerts.outputs.certIds : []
var wildcardCertIds = toObject(_certDomains, domain => domain, domain => _certIds[indexOf(_certDomains, domain)])

// PostgreSQL Flexible Server (private endpoint only — no public access)
module postgres './modules/postgres.bicep' = {
  scope: resourceGroup
  name: 'postgres-deployment'
  params: {
    location: location
    serverName: postgresServerName
    administratorLogin: postgresAdminLogin
    administratorLoginPassword: postgresAdminPassword
  }
}

// PostgreSQL Private Endpoint (connects environment VNet to PostgreSQL)
module postgresPrivateEndpoint './modules/postgresPrivateEndpoint.bicep' = {
  scope: resourceGroup
  name: 'postgresPe-deployment'
  params: {
    location: location
    privateEndpointName: 'pe-psql-techhub-${environmentName}'
    subnetId: network.outputs.privateEndpointsSubnetId
    postgresServerId: postgres.outputs.serverId
    vnetId: network.outputs.vnetId
    hubVnetId: hubVnetId
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
    containerRegistryName: containerRegistryName
    acrPullIdentityId: identity.outputs.identityId
    imageTag: webImageTag
    apiBaseUrl: apiApp.outputs.fqdn
    appInsightsConnectionString: monitoring.outputs.appInsightsConnectionString
    customDomains: allCustomDomains
    primaryHosts: primaryHosts
    environmentName: environmentName
    wildcardCertificateIds: wildcardCertIds
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
