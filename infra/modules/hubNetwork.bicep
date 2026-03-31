@description('Azure region')
param location string

@description('Hub VNet name')
param vnetName string

@description('Private endpoints subnet name')
param privateEndpointsSubnetName string = 'snet-private-endpoints'

@description('Private endpoints subnet prefix (must match the subnet definition)')
param privateEndpointsSubnetPrefix string = '10.100.1.0/24'

// NSG for private endpoints subnet — only allows traffic from within the hub VNet
resource privateEndpointsNsg 'Microsoft.Network/networkSecurityGroups@2025-01-01' = {
  name: 'nsg-${privateEndpointsSubnetName}'
  location: location
  properties: {
    securityRules: [
      {
        name: 'AllowVNetInbound'
        properties: {
          priority: 100
          direction: 'Inbound'
          access: 'Allow'
          protocol: 'Tcp'
          sourceAddressPrefix: 'VirtualNetwork'
          sourcePortRange: '*'
          destinationAddressPrefix: privateEndpointsSubnetPrefix
          destinationPortRanges: [
            '443'  // Key Vault
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

// Hub Virtual Network (VPN Gateway removed — admin access via NSP + IP firewall rules)
resource vnet 'Microsoft.Network/virtualNetworks@2025-01-01' = {
  name: vnetName
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        '10.100.0.0/16'
      ]
    }
    subnets: [
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
output privateEndpointsSubnetId string = vnet.properties.subnets[0].id
