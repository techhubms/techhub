// Shared infrastructure for TechHub — ACR, Key Vault, Hub VNet, AMPLS
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

@description('Comma-separated admin IP addresses for Key Vault and PostgreSQL firewall (e.g. "1.2.3.4,5.6.7.8")')
@minLength(7)
param adminIpAddresses string

@description('DNS zone name for ACME challenge delegation (used by certbot-dns-azure for wildcard cert renewal)')
param acmeDnsZoneName string = 'acme.hub.ms'

@description('Domains that need ACME-delegated wildcard certificate renewal')
param acmeDelegatedDomains string[] = ['hub.ms', 'xebia.ms']

@description('Spoke VNet resource IDs to link AMPLS DNS zones to (pass after spoke VNets are created)')
param spokeVnetIds string[] = []

@description('Email address that receives operational alerts and budget notifications')
param alertEmailAddress string = 'reinier.vanmaanen@xebia.com'

@description('Monthly subscription budget amount (in billing currency, typically EUR).')
param monthlyBudgetAmount int = 250

@description('Budget start date (first-of-month, YYYY-MM-01). Must be in the current or past month at deploy time.')
param budgetStartDate string = '2026-04-01'

@description('Azure Policy: allowed deployment locations')
param allowedLocations string[] = ['swedencentral', 'westeurope', 'global']

@description('Tag name required on resource groups (enforced by Azure Policy)')
param requiredResourceGroupTagName string = 'owner'

@description('Common tags applied to all resources managed by this template')
param commonTags object = {
  owner: 'techhub-maintainer'
  project: 'techhub'
  managedBy: 'bicep'
}

// Shared Resource Group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: resourceGroupName
  location: location
  tags: commonTags
}

// Shared Container Registry
module registry './modules/registry.bicep' = {
  scope: resourceGroup
  name: 'registry-deployment'
  params: {
    location: location
    registryName: containerRegistryName
    sku: 'Standard'
    adminIpAddresses: adminIpList
    tags: commonTags
  }
}

// Shared Log Analytics workspace (for Key Vault audit logs)
module sharedLogAnalytics './modules/logAnalytics.bicep' = {
  scope: resourceGroup
  name: 'logAnalytics-deployment'
  params: {
    location: location
    logAnalyticsWorkspaceName: 'law-techhub-shared'
    tags: commonTags
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
    logAnalyticsWorkspaceId: sharedLogAnalytics.outputs.logAnalyticsWorkspaceId
    adminIpAddresses: adminIpList
    tags: commonTags
  }
}

// Hub VNet (admin access via IP firewall rules on each resource)
module hubNetwork './modules/hubNetwork.bicep' = {
  scope: resourceGroup
  name: 'hubNetwork-deployment'
  params: {
    location: location
    vnetName: hubVnetName
    tags: commonTags
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

// Parse comma-separated admin IPs into a trimmed, filtered array
var adminIpList = [for ip in filter(split(adminIpAddresses, ','), entry => !empty(trim(entry))): trim(ip)]

// Azure Monitor Private Link Scope — routes app telemetry privately through the hub VNet
module ampls './modules/monitorPrivateLink.bicep' = {
  scope: resourceGroup
  name: 'ampls-deployment'
  params: {
    location: location
    amplsName: 'ampls-techhub'
    subnetId: hubNetwork.outputs.privateEndpointsSubnetId
    vnetId: hubNetwork.outputs.vnetId
    spokeVnetIds: spokeVnetIds
    appInsightsIds: []
    logAnalyticsWorkspaceIds: [
      sharedLogAnalytics.outputs.logAnalyticsWorkspaceId
    ]
  }
}

// Shared action group — email notifications for all operational alerts (per-env alerts reference this).
module actionGroup './modules/actionGroup.bicep' = {
  scope: resourceGroup
  name: 'actionGroup-deployment'
  params: {
    emailAddress: alertEmailAddress
    tags: commonTags
  }
}

// Subscription monthly cost budget with 80 / 100 / 120% email alerts.
module budget './modules/budget.bicep' = {
  name: 'budget-deployment'
  params: {
    amount: monthlyBudgetAmount
    contactEmails: [alertEmailAddress]
    startDate: budgetStartDate
  }
}

// Subscription-level Azure Policy assignments (governance baseline).
module policyAssignments './modules/policy.bicep' = {
  name: 'policy-deployment'
  params: {
    allowedLocations: allowedLocations
    requiredTagName: requiredResourceGroupTagName
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
output acmeDnsZoneName string = acmeDnsZone.outputs.zoneName
output acmeDnsNameServers string[] = acmeDnsZone.outputs.nameServers
output postgresDnsZoneName string = postgresDnsZone.outputs.dnsZoneName
output actionGroupId string = actionGroup.outputs.actionGroupId
