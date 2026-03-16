@description('Azure region for resources')
param location string

@description('VNet name')
param vnetName string

@description('VNet address space prefix (must be unique per environment when peered)')
param addressSpacePrefix string = '10.0.0.0/16'

@description('Container Apps subnet name')
param containerAppsSubnetName string = 'snet-container-apps'

@description('Container Apps subnet prefix')
param containerAppsSubnetPrefix string = '10.0.0.0/23'

@description('Private endpoints subnet name')
param privateEndpointsSubnetName string = 'snet-private-endpoints'

@description('Private endpoints subnet prefix')
param privateEndpointsSubnetPrefix string = '10.0.2.0/24'

// NSG for private endpoints subnet — only allows traffic from the Container Apps subnet
resource privateEndpointsNsg 'Microsoft.Network/networkSecurityGroups@2025-01-01' = {
  name: 'nsg-${privateEndpointsSubnetName}'
  location: location
  properties: {
    securityRules: [
      {
        name: 'AllowContainerAppsSubnetInbound'
        properties: {
          priority: 100
          direction: 'Inbound'
          access: 'Allow'
          protocol: 'Tcp'
          sourceAddressPrefix: containerAppsSubnetPrefix
          sourcePortRange: '*'
          destinationAddressPrefix: privateEndpointsSubnetPrefix
          destinationPortRanges: [
            '443'   // Key Vault
            '5432'  // PostgreSQL
          ]
        }
      }
      {
        name: 'DenyAllOtherInbound'
        properties: {
          priority: 4096
          direction: 'Inbound'
          access: 'Deny'
          protocol: '*'
          sourceAddressPrefix: '*'
          sourcePortRange: '*'
          destinationAddressPrefix: '*'
          destinationPortRange: '*'
        }
      }
    ]
  }
}

// Virtual Network
resource vnet 'Microsoft.Network/virtualNetworks@2025-01-01' = {
  name: vnetName
  location: location
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
          networkSecurityGroup: {
            id: privateEndpointsNsg.id
          }
        }
      }
    ]
  }
}

// Outputs
output vnetId string = vnet.id
output vnetName string = vnet.name
output containerAppsSubnetId string = vnet.properties.subnets[0].id
output privateEndpointsSubnetId string = vnet.properties.subnets[1].id
