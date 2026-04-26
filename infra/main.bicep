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

@description('Azure AD tenant ID for admin dashboard authentication (public Entra identifier)')
param azureAdTenantId string = ''

@description('Azure AD client ID for admin dashboard authentication (public Entra identifier)')
param azureAdClientId string = ''

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

@description('Comma-separated admin IP addresses for PostgreSQL firewall rules (e.g. "1.2.3.4,5.6.7.8")')
@minLength(7)
param adminIpAddresses string

@description('Shared action group resource ID (notification target for operational alerts). Leave empty to skip alert creation.')
param actionGroupId string = ''

@description('Common tags applied to all resources managed by this template')
param commonTags object = {
  owner: 'techhub-maintainer'
  project: 'techhub'
  managedBy: 'bicep'
}

// Per-environment tag set — combines commonTags with the env name for clear resource attribution.
var envTags = union(commonTags, {
  env: environmentName
})

// Resource Group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: resourceGroupName
  location: location
  tags: envTags
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
    tags: envTags
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
    tags: envTags
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
    appInsightsRetentionInDays: 30
    availabilityTestHosts: primaryHosts
    enableSmartDetection: environmentName != 'staging'
    tags: envTags
  }
}

// VNet Peering: spoke → hub (gateway transit removed — no VPN Gateway)
module peeringSpokeToHub './modules/vnetPeering.bicep' = if (!empty(hubVnetId)) {
  scope: resourceGroup
  name: 'peering-spoke-to-hub'
  params: {
    localVnetName: vnetName
    remoteVnetId: hubVnetId
    peeringName: 'peer-${vnetName}-to-hub'
    allowForwardedTraffic: true
    useRemoteGateways: false
  }
  dependsOn: [network, peeringHubToSpoke]
}

// VNet Peering: hub → spoke (gateway transit removed — no VPN Gateway)
module peeringHubToSpoke './modules/vnetPeering.bicep' = if (!empty(hubVnetId)) {
  scope: sharedResourceGroup
  name: 'peering-hub-to-${environmentName}'
  params: {
    localVnetName: hubVnetName
    remoteVnetId: network.outputs.vnetId
    peeringName: 'peer-hub-to-${vnetName}'
    allowForwardedTraffic: true
    allowGatewayTransit: false
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
    tags: envTags
  }
}

// AI Foundry Private Endpoint in spoke VNet (so Container Apps use a private path)
module openAiPrivateEndpoint './modules/openAiPrivateEndpoint.bicep' = {
  scope: resourceGroup
  name: 'openAiPe-deployment'
  params: {
    location: location
    privateEndpointName: 'pe-oai-techhub-${environmentName}'
    subnetId: network.outputs.privateEndpointsSubnetId
    openAiAccountId: openai.outputs.openAiId
    vnetId: network.outputs.vnetId
  }
}

// Add environment monitoring to the shared AMPLS (private telemetry path)
module amplsScope './modules/amplsScope.bicep' = {
  scope: sharedResourceGroup
  name: 'amplsScope-${environmentName}'
  params: {
    amplsName: 'ampls-techhub'
    scopePrefix: 'scope-${environmentName}'
    resourceIds: [
      monitoring.outputs.appInsightsId
      monitoring.outputs.logAnalyticsWorkspaceId
    ]
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
    tags: envTags
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

// Parse comma-separated admin IPs into a trimmed, filtered array
var adminIpList = [for ip in filter(split(adminIpAddresses, ','), entry => !empty(trim(entry))): trim(ip)]

// PostgreSQL Flexible Server (private endpoint + admin IP firewall rule)
module postgres './modules/postgres.bicep' = {
  scope: resourceGroup
  name: 'postgres-deployment'
  params: {
    location: location
    serverName: postgresServerName
    administratorLogin: postgresAdminLogin
    administratorLoginPassword: postgresAdminPassword
    skuName: 'Standard_B1ms'
    skuTier: 'Burstable'
    // Prod keeps backups longer for a wider PITR window; staging stays on the minimum to save cost.
    backupRetentionDays: environmentName == 'prod' ? 21 : 7
    // Prod enables geo-redundant backup for cross-region restore; staging does not need it.
    geoRedundantBackup: environmentName == 'prod'
    adminIpAddresses: adminIpList
    tags: envTags
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

// Key Vault URI used for Container App KV-reference secrets (e.g. https://kv-techhub-shared.vault.azure.net/).
// Shared secrets live in the shared Key Vault under names like: techhub-<env>-db-connection-string.
var sharedKeyVaultUri = 'https://${keyVaultName}${environment().suffixes.keyvaultDns}/'
var dbConnectionSecretName = 'techhub-${environmentName}-db-connection-string'
var aiApiKeySecretName = 'techhub-${environmentName}-ai-api-key'
var aadClientSecretSecretName = 'techhub-${environmentName}-aad-client-secret'

// Grant Key Vault Secrets User to the managed identity on the shared Key Vault.
// Required for Container App KV-reference secrets (db connection string, AI key, AAD secret).
// This is unconditional — wildcardCertificates.bicep also grants this role but only runs when
// wildcardCertNames is non-empty; we need it for all environments including bare deployments.
module kvSecretsUserRole './modules/kvSecretsUserRole.bicep' = {
  scope: sharedResourceGroup
  name: 'kvSecretsUserRole-${environmentName}'
  params: {
    keyVaultName: keyVaultName
    principalId: identity.outputs.identityPrincipalId
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
    keyVaultUri: sharedKeyVaultUri
    dbConnectionSecretName: dbConnectionSecretName
    aiApiKeySecretName: aiApiKeySecretName
    webFqdns: !empty(primaryHosts) ? primaryHosts : ['${webAppName}.${containerAppsEnv.outputs.defaultDomain}']
    environmentName: environmentName
    azureAdTenantId: azureAdTenantId
    azureAdClientId: azureAdClientId
    aiCategorizationEndpoint: openai.outputs.openAiEndpoint
    aiCategorizationDeploymentName: openai.outputs.deploymentName
    tags: envTags
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
    keyVaultUri: sharedKeyVaultUri
    aadClientSecretSecretName: aadClientSecretSecretName
    azureAdTenantId: azureAdTenantId
    azureAdClientId: azureAdClientId
    tags: envTags
  }
}

// Operational alerts — only when an action group has been provided by the shared deployment.
module alerts './modules/alerts.bicep' = if (!empty(actionGroupId)) {
  scope: resourceGroup
  name: 'alerts-deployment'
  params: {
    location: location
    environmentName: environmentName
    actionGroupId: actionGroupId
    appInsightsId: monitoring.outputs.appInsightsId
    logAnalyticsWorkspaceId: monitoring.outputs.logAnalyticsWorkspaceId
    postgresServerId: postgres.outputs.serverId
    openAiAccountId: openai.outputs.openAiId
    tags: envTags
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
