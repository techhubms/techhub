@description('Azure region for resources')
param location string

@description('VNet name')
param vnetName string

@description('VNet address space prefix')
param addressSpacePrefix string = '10.0.0.0/16'

@description('Container Apps subnet name')
param containerAppsSubnetName string = 'snet-container-apps'

@description('Container Apps subnet prefix')
param containerAppsSubnetPrefix string = '10.0.0.0/23'

@description('Private endpoints subnet name')
param privateEndpointsSubnetName string = 'snet-private-endpoints'

@description('Private endpoints subnet prefix')
param privateEndpointsSubnetPrefix string = '10.0.2.0/27'

@description('Tags applied to networking resources')
param tags object = {}

// Virtual Network with a Container Apps subnet and a private endpoints subnet.
// Key Vault, PostgreSQL, and AI Foundry all use private endpoints in the dedicated subnet
// below — private endpoints cannot share a subnet delegated to Microsoft.App/environments.
resource vnet 'Microsoft.Network/virtualNetworks@2025-01-01' = {
  name: vnetName
  location: location
  tags: tags
  properties: {
    addressSpace: {
      addressPrefixes: [
        addressSpacePrefix
      ]
    }
    enableDdosProtection: false
    subnets: [
      {
        name: containerAppsSubnetName
        properties: {
          addressPrefix: containerAppsSubnetPrefix
          delegations: [
            {
              name: 'Microsoft.App.environments'
              properties: {
                serviceName: 'Microsoft.App/environments'
              }
            }
          ]
        }
      }
      {
        name: privateEndpointsSubnetName
        properties: {
          addressPrefix: privateEndpointsSubnetPrefix
          privateEndpointNetworkPolicies: 'Disabled'
        }
      }
    ]
  }
}

// Private DNS zone for PostgreSQL private endpoints — linked to the VNet so Container Apps
// resolve <server>.postgres.database.azure.com to the private endpoint IP automatically.
resource postgresPrivateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink.postgres.database.azure.com'
  location: 'global'
  tags: tags
}

resource postgresPrivateDnsZoneVnetLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = {
  parent: postgresPrivateDnsZone
  name: '${vnetName}-link'
  location: 'global'
  properties: {
    virtualNetwork: {
      id: vnet.id
    }
    registrationEnabled: false
  }
}

// Private DNS zone for Key Vault private endpoints — linked to the VNet so Container Apps
// resolve <vault>.vault.azure.net to the private endpoint IP automatically.
resource keyVaultPrivateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink.vaultcore.azure.net'
  location: 'global'
  tags: tags
}

resource keyVaultPrivateDnsZoneVnetLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = {
  parent: keyVaultPrivateDnsZone
  name: '${vnetName}-link'
  location: 'global'
  properties: {
    virtualNetwork: {
      id: vnet.id
    }
    registrationEnabled: false
  }
}

// Private DNS zone for AI Foundry (Cognitive Services) private endpoints — linked to the VNet so
// Container Apps resolve <account>.cognitiveservices.azure.com to the private endpoint IP automatically.
resource openAiPrivateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink.cognitiveservices.azure.com'
  location: 'global'
  tags: tags
}

resource openAiPrivateDnsZoneVnetLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = {
  parent: openAiPrivateDnsZone
  name: '${vnetName}-link'
  location: 'global'
  properties: {
    virtualNetwork: {
      id: vnet.id
    }
    registrationEnabled: false
  }
}

// Outputs
// Subnet IDs are looked up by name rather than array index — indexing is brittle because
// reordering or inserting subnets would silently change which subnet ID is exported.
output vnetId string = vnet.id
output vnetName string = vnet.name
output containerAppsSubnetId string = filter(vnet.properties.subnets, s => s.name == containerAppsSubnetName)[0].id
output privateEndpointsSubnetId string = filter(vnet.properties.subnets, s => s.name == privateEndpointsSubnetName)[0].id
output postgresPrivateDnsZoneId string = postgresPrivateDnsZone.id
output keyVaultPrivateDnsZoneId string = keyVaultPrivateDnsZone.id
output openAiPrivateDnsZoneId string = openAiPrivateDnsZone.id
