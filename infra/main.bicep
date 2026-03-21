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

@description('Content Processor Container App name')
param contentProcessorAppName string = 'ca-techhub-content-processor-${environmentName}'

@description('API Docker image tag (yyyyMMddHHmmss format)')
param apiImageTag string

@description('Web Docker image tag (yyyyMMddHHmmss format)')
param webImageTag string

@description('Content Processor Docker image tag (yyyyMMddHHmmss format). Leave empty to skip deployment.')
param contentProcessorImageTag string = ''

@description('VNet name')
param vnetName string = 'vnet-techhub-${environmentName}'

@description('VNet address space (must be unique per environment)')
param addressSpacePrefix string = '10.0.0.0/16'

@description('Container Apps subnet prefix')
param containerAppsSubnetPrefix string = '10.0.0.0/23'

@description('Private endpoints subnet prefix')
param privateEndpointsSubnetPrefix string = '10.0.2.0/24'

@description('Primary host names for the web app (e.g. ["tech.hub.ms", "tech.xebia.ms"]). Used for app configuration (CORS, canonical URLs). Leave empty to use default Container Apps FQDN.')
param primaryHosts string[] = []

@description('Wildcard certificate names in Key Vault, keyed by base domain (e.g. { "hub.ms": "wildcard-hub-ms" }). Wildcard custom domain bindings (*.hub.ms) are derived from these keys.')
param wildcardCertNames object = {}

@description('PostgreSQL server name')
param postgresServerName string = 'psql-techhub-${environmentName}'

@description('PostgreSQL administrator login')
param postgresAdminLogin string = 'techhubadmin'

@secure()
@description('PostgreSQL administrator password')
param postgresAdminPassword string

@description('Azure AI Foundry (OpenAI) resource name')
param openAiName string = 'oai-techhub-${environmentName}'

@description('Azure AI Foundry model capacity (TPM in thousands)')
param openAiModelCapacity int = 100

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

// Monitoring (Application Insights + Log Analytics + availability tests)
module monitoring './modules/monitoring.bicep' = {
  scope: resourceGroup
  name: 'monitoring-deployment'
  params: {
    location: location
    appInsightsName: appInsightsName
    logAnalyticsWorkspaceName: 'law-techhub-${environmentName}'
    dailyQuotaGb: environmentName == 'staging' ? 1 : -1
    appInsightsRetentionInDays: environmentName == 'staging' ? 30 : 90
    availabilityTestHosts: primaryHosts
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

// Azure AI Foundry (OpenAI)
module openai './modules/openai.bicep' = {
  scope: resourceGroup
  params: {
    location: location
    openAiName: openAiName
    modelCapacity: openAiModelCapacity
  }
}

// Container Apps Environment (VNet-integrated)
module containerAppsEnv './modules/containerApps.bicep' = {
  scope: resourceGroup
  name: 'containerAppsEnv-deployment'
  params: {
    location: location
    environmentName: containerAppsEnvName
    logAnalyticsWorkspaceId: monitoring.outputs.logAnalyticsWorkspaceId
    infrastructureSubnetId: network.outputs.containerAppsSubnetId
    identityId: identity.outputs.identityId
  }
}

// Wildcard certificates from Key Vault → Container Apps Environment
// The managed identity needs Key Vault Secrets User on the shared KV.
module wildcardCerts './modules/wildcardCertificates.bicep' = if (!empty(wildcardCertNames)) {
  scope: resourceGroup
  name: 'wildcardCerts-${environmentName}'
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

// Wildcard custom domain bindings (*.hub.ms, *.xebia.ms) derived from wildcardCertNames keys.
// A single wildcard binding per domain covers all subdomains — no per-subdomain registration needed.
var allCustomDomains = [for entry in items(wildcardCertNames): '*.${entry.key}']

// PostgreSQL Flexible Server (private endpoint only — no public access)
module postgres './modules/postgres.bicep' = {
  scope: resourceGroup
  name: 'postgres-deployment'
  params: {
    location: location
    serverName: postgresServerName
    administratorLogin: postgresAdminLogin
    administratorLoginPassword: postgresAdminPassword
    skuName: environmentName == 'staging' ? 'Standard_B1ms' : 'Standard_B2s'
    skuTier: 'Burstable'
    backupRetentionDays: environmentName == 'staging' ? 7 : 14
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
    sharedResourceGroupName: sharedResourceGroupName
  }
}

// Link shared PostgreSQL private DNS zone to this spoke VNet
// so Container Apps can resolve the PostgreSQL private endpoint
module postgresDnsLink './modules/privateDnsZoneLink.bicep' = {
  scope: sharedResourceGroup
  name: 'postgresDnsLink-${environmentName}'
  params: {
    dnsZoneName: 'privatelink.postgres.database.azure.com'
    linkName: 'link-psql-${vnetName}'
    vnetId: network.outputs.vnetId
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
    databaseConnectionString: 'Host=${postgres.outputs.serverFqdn};Database=${postgres.outputs.databaseName};Username=${postgresAdminLogin};Password=${postgresAdminPassword};SSL Mode=Require'
    webFqdns: !empty(primaryHosts) ? primaryHosts : ['${webAppName}.${containerAppsEnv.outputs.defaultDomain}']
    environmentName: environmentName
  }
}

// Web Container App
// Note: implicit dependency on apiApp via apiBaseUrl parameter ensures API deploys first.
// The SectionCacheRefreshService in the web app handles data freshness after deployment.
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

// Content Processor Container App (production only — runs the RSS ingestion and AI categorization pipeline)
// Staging and local environments receive data via database restore (see scripts/Restore-Database.ps1).
// Deploy is conditional: set contentProcessorImageTag to skip.
module contentProcessorApp './modules/contentprocessor.bicep' = if (!empty(contentProcessorImageTag)) {
  scope: resourceGroup
  name: 'content-processor-deployment'
  dependsOn: [acrRoleAssignment]
  params: {
    location: location
    containerAppName: contentProcessorAppName
    containerAppsEnvironmentId: containerAppsEnv.outputs.environmentId
    containerRegistryName: containerRegistryName
    acrPullIdentityId: identity.outputs.identityId
    imageTag: contentProcessorImageTag
    appInsightsConnectionString: monitoring.outputs.appInsightsConnectionString
    databaseConnectionString: 'Host=${postgres.outputs.serverFqdn};Database=${postgres.outputs.databaseName};Username=${postgresAdminLogin};Password=${postgresAdminPassword};SSL Mode=Require'
    openAiApiKey: openai.outputs.openAiApiKey
    openAiEndpoint: openai.outputs.openAiEndpoint
    openAiDeploymentName: openai.outputs.deploymentName
    environmentName: environmentName
  }
}

// Outputs
output resourceGroupName string = resourceGroup.name
output apiUrl string = 'https://${apiApp.outputs.fqdn}'
output webUrl string = 'https://${webApp.outputs.fqdn}'
output appInsightsName string = monitoring.outputs.appInsightsName
output containerRegistryName string = containerRegistryName
output openAiEndpoint string = openai.outputs.openAiEndpoint
output openAiDeploymentName string = openai.outputs.deploymentName
output vnetName string = vnetName
output postgresServerFqdn string = postgres.outputs.serverFqdn
output postgresDatabaseName string = postgres.outputs.databaseName
