@description('Azure region for resources')
param location string

@description('VNet name')
param vnetName string

@description('Container Apps subnet name')
param containerAppsSubnetName string = 'snet-container-apps'

@description('PostgreSQL subnet name')
param postgresSubnetName string = 'snet-postgres'

// Virtual Network
resource vnet 'Microsoft.Network/virtualNetworks@2025-01-01' = {
  name: vnetName
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        '10.0.0.0/16'
      ]
    }
    enableDdosProtection: false
    subnets: [
      {
        name: containerAppsSubnetName
        properties: {
          addressPrefix: '10.0.0.0/23'
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
        name: postgresSubnetName
        properties: {
          addressPrefix: '10.0.2.0/24'
          delegations: [
            {
              name: 'Microsoft.DBforPostgreSQL.flexibleServers'
              properties: {
                serviceName: 'Microsoft.DBforPostgreSQL/flexibleServers'
              }
            }
          ]
        }
      }
    ]
  }
}

// Private DNS Zone for PostgreSQL
resource privateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: '${vnetName}.private.postgres.database.azure.com'
  location: 'global'
}

// Link DNS Zone to VNet
resource privateDnsZoneLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = {
  parent: privateDnsZone
  name: '${vnetName}-link'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: resourceId('Microsoft.Network/virtualNetworks', vnetName)
    }
  }
}

// Outputs
output vnetId string = vnet.id
output containerAppsSubnetId string = resourceId('Microsoft.Network/virtualNetworks/subnets', vnetName, containerAppsSubnetName)
output postgresSubnetId string = resourceId('Microsoft.Network/virtualNetworks/subnets', vnetName, postgresSubnetName)
output privateDnsZoneId string = resourceId('Microsoft.Network/privateDnsZones', '${vnetName}.private.postgres.database.azure.com')
