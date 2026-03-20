@description('Azure region')
param location string

@description('Hub VNet name')
param vnetName string

@description('VPN Gateway name')
param vpnGatewayName string

@description('Public IP name for VPN Gateway')
param vpnPublicIpName string = '${vpnGatewayName}-pip'

@description('VPN client address pool (CIDR for P2S clients)')
param vpnClientAddressPool string = '172.16.0.0/24'

@description('Azure AD tenant ID for VPN authentication')
param aadTenantId string = subscription().tenantId

@description('Azure AD audience value for VPN authentication. Uses the Microsoft-registered App ID by default (no manual app registration needed).')
param aadAudienceAppId string = 'c632b3df-fb67-4d84-bdcf-b95ad541b5c8'

@description('Private endpoints subnet name')
param privateEndpointsSubnetName string = 'snet-private-endpoints'

@description('Private endpoints subnet prefix (must match the subnet definition)')
param privateEndpointsSubnetPrefix string = '10.100.1.0/24'

var aadIssuer = 'https://sts.windows.net/${aadTenantId}/'

// NSG for private endpoints subnet — only allows traffic from within the hub VNet (VPN clients)
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

// Hub Virtual Network
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
        // GatewaySubnet — required name for VPN Gateway
        name: 'GatewaySubnet'
        properties: {
          addressPrefix: '10.100.0.0/24'
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

// Public IP for VPN Gateway
resource vpnPublicIp 'Microsoft.Network/publicIPAddresses@2025-01-01' = {
  name: vpnPublicIpName
  location: location
  sku: {
    name: 'Standard'
  }
  properties: {
    publicIPAllocationMethod: 'Static'
  }
}

// VPN Gateway (Point-to-Site)
resource vpnGateway 'Microsoft.Network/virtualNetworkGateways@2025-01-01' = {
  name: vpnGatewayName
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'default'
        properties: {
          privateIPAllocationMethod: 'Dynamic'
          publicIPAddress: {
            id: vpnPublicIp.id
          }
          subnet: {
            id: vnet.properties.subnets[0].id
          }
        }
      }
    ]
    gatewayType: 'Vpn'
    vpnType: 'RouteBased'
    sku: {
      name: 'VpnGw1AZ'
      tier: 'VpnGw1AZ'
    }
    vpnGatewayGeneration: 'Generation1'
    vpnClientConfiguration: {
      vpnClientAddressPool: {
        addressPrefixes: [
          vpnClientAddressPool
        ]
      }
      vpnClientProtocols: [
        'OpenVPN'
      ]
      vpnAuthenticationTypes: [
        'AAD'
      ]
      aadTenant: '${environment().authentication.loginEndpoint}${aadTenantId}/'
      aadAudience: aadAudienceAppId
      aadIssuer: aadIssuer
    }
  }
}

// Outputs
output vnetId string = vnet.id
output vnetName string = vnet.name
output privateEndpointsSubnetId string = vnet.properties.subnets[1].id
output vpnGatewayId string = vpnGateway.id
output vpnPublicIpAddress string = vpnPublicIp.properties.ipAddress
