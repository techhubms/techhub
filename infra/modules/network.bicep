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
// Key Vault uses a VNet service endpoint on the Container Apps subnet (no private endpoint
// needed). PostgreSQL uses a private endpoint in the dedicated subnet below — private
// endpoints cannot share a subnet delegated to Microsoft.App/environments.
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
          // Service endpoints — allows Container Apps to reach Azure services
          // over the Microsoft backbone without private endpoints.
          // Microsoft.KeyVault: Key Vault traffic uses private backbone.
          serviceEndpoints: [
            {
              service: 'Microsoft.KeyVault'
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

// Outputs
output vnetId string = vnet.id
output vnetName string = vnet.name
output containerAppsSubnetId string = vnet.properties.subnets[0].id
output privateEndpointsSubnetId string = vnet.properties.subnets[1].id
output postgresPrivateDnsZoneId string = postgresPrivateDnsZone.id
