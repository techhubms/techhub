// Shared infrastructure for TechHub — ACR, Key Vault, Hub VNet, NSP, AMPLS
targetScope = 'subscription'

@description('Azure region for shared resources')
param location string = 'westeurope'

@description('Shared resource group name')
param resourceGroupName string = 'rg-techhub-shared'

@description('Container Registry name (alphanumeric only)')
param containerRegistryName string = 'crtechhubms'

@description('Key Vault name')
param keyVaultName string = 'kv-techhub-shared'

@description('Azure AD object IDs for Key Vault administrators')
param keyVaultAdminObjectIds array = []

@description('Hub VNet name')
param hubVnetName string = 'vnet-techhub-hub'

@description('Admin IP address for NSP inbound rule and PostgreSQL firewall (e.g. "1.2.3.4")')
@minLength(7)
param adminIpAddress string

@description('DNS zone name for ACME challenge delegation (used by certbot-dns-azure for wildcard cert renewal)')
param acmeDnsZoneName string = 'acme.hub.ms'

@description('Domains that need ACME-delegated wildcard certificate renewal')
param acmeDelegatedDomains string[] = ['hub.ms', 'xebia.ms']

// Shared Resource Group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: resourceGroupName
  location: location
}

// Shared Container Registry
module registry './modules/registry.bicep' = {
  scope: resourceGroup
  name: 'registry-deployment'
  params: {
    location: location
    registryName: containerRegistryName
    sku: 'Standard'
  }
}

// Shared Log Analytics workspace (for Key Vault audit logs)
module sharedMonitoring './modules/monitoring.bicep' = {
  scope: resourceGroup
  params: {
    location: location
    appInsightsName: 'appi-techhub-shared'
    logAnalyticsWorkspaceName: 'law-techhub-shared'
  }
}

// Shared Key Vault (stores wildcard certificates used by staging + production)
module keyVault './modules/keyVault.bicep' = {
  scope: resourceGroup
  name: 'keyVault-deployment'
  params: {
    location: location
    vaultName: keyVaultName
    adminObjectIds: keyVaultAdminObjectIds
    logAnalyticsWorkspaceId: sharedMonitoring.outputs.logAnalyticsWorkspaceId
  }
}

// Hub VNet (VPN Gateway removed — admin access via NSP + IP firewall rules)
module hubNetwork './modules/hubNetwork.bicep' = {
  scope: resourceGroup
  name: 'hubNetwork-deployment'
  params: {
    location: location
    vnetName: hubVnetName
  }
}

// Key Vault Private Endpoint in hub VNet
module keyVaultPrivateEndpoint './modules/keyVaultPrivateEndpoint.bicep' = {
  scope: resourceGroup
  name: 'keyVaultPe-deployment'
  params: {
    location: location
    privateEndpointName: 'pe-kv-techhub'
    subnetId: hubNetwork.outputs.privateEndpointsSubnetId
    keyVaultId: keyVault.outputs.vaultId
    vnetId: hubNetwork.outputs.vnetId
  }
}

// Public DNS zone for ACME challenge delegation (certbot-dns-azure writes TXT records here)
module acmeDnsZone './modules/acmeDnsZone.bicep' = {
  scope: resourceGroup
  name: 'acmeDnsZone-deployment'
  params: {
    zoneName: acmeDnsZoneName
    delegatedDomains: acmeDelegatedDomains
  }
}

// Shared PostgreSQL private DNS zone (linked by each spoke environment)
module postgresDnsZone './modules/postgresDnsZone.bicep' = {
  scope: resourceGroup
  name: 'postgresDnsZone-deployment'
  params: {
    hubVnetId: hubNetwork.outputs.vnetId
  }
}

// Network Security Perimeter — controls public access to Key Vault, App Insights, Log Analytics, AI Foundry
// Associated resources are added by environment deployments via the NSP resource ID output
module nsp './modules/networkSecurityPerimeter.bicep' = {
  scope: resourceGroup
  name: 'nsp-deployment'
  params: {
    location: location
    nspName: 'nsp-techhub'
    adminIpCidr: '${adminIpAddress}/32'
    associatedResourceIds: [
      keyVault.outputs.vaultId
      sharedMonitoring.outputs.appInsightsId
      sharedMonitoring.outputs.logAnalyticsWorkspaceId
    ]
  }
}

// Azure Monitor Private Link Scope — routes app telemetry privately through the hub VNet
module ampls './modules/monitorPrivateLink.bicep' = {
  scope: resourceGroup
  name: 'ampls-deployment'
  params: {
    location: location
    amplsName: 'ampls-techhub'
    subnetId: hubNetwork.outputs.privateEndpointsSubnetId
    vnetId: hubNetwork.outputs.vnetId
    appInsightsIds: [
      sharedMonitoring.outputs.appInsightsId
    ]
    logAnalyticsWorkspaceIds: [
      sharedMonitoring.outputs.logAnalyticsWorkspaceId
    ]
  }
}

// Outputs
output resourceGroupName string = resourceGroup.name
output containerRegistryName string = registry.outputs.name
output containerRegistryLoginServer string = registry.outputs.loginServer
output keyVaultName string = keyVault.outputs.vaultName
output keyVaultUri string = keyVault.outputs.vaultUri
output keyVaultId string = keyVault.outputs.vaultId
output hubVnetId string = hubNetwork.outputs.vnetId
output hubVnetName string = hubNetwork.outputs.vnetName
output nspId string = nsp.outputs.nspId
output nspProfileId string = nsp.outputs.profileId
output acmeDnsZoneName string = acmeDnsZone.outputs.zoneName
output acmeDnsNameServers string[] = acmeDnsZone.outputs.nameServers
output postgresDnsZoneName string = postgresDnsZone.outputs.dnsZoneName
