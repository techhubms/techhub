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

@description('Tags applied to networking resources')
param tags object = {}

// Virtual Network with a single Container Apps subnet.
// Key Vault uses a VNet service endpoint on this subnet (no private endpoint needed).
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
          // Key Vault service endpoint — allows Container Apps to reach Key Vault
          // over the Microsoft backbone without a private endpoint.
          serviceEndpoints: [
            {
              service: 'Microsoft.KeyVault'
            }
          ]
        }
      }
    ]
  }
}

// Outputs
output vnetId string = vnet.id
output vnetName string = vnet.name
output containerAppsSubnetId string = vnet.properties.subnets[0].id
