@description('Name of the local VNet')
param localVnetName string

@description('Resource ID of the remote VNet to peer with')
param remoteVnetId string

@description('Name for the peering connection')
param peeringName string

@description('Allow traffic forwarded from the remote VNet (needed for VPN gateway transit)')
param allowForwardedTraffic bool = false

@description('Allow gateway transit (set true on the hub side that has the VPN gateway)')
param allowGatewayTransit bool = false

@description('Use the remote VNet gateway (set true on spoke side to route through hub VPN)')
param useRemoteGateways bool = false

resource vnet 'Microsoft.Network/virtualNetworks@2025-01-01' existing = {
  name: localVnetName
}

resource peering 'Microsoft.Network/virtualNetworks/virtualNetworkPeerings@2025-01-01' = {
  parent: vnet
  name: peeringName
  properties: {
    remoteVirtualNetwork: {
      id: remoteVnetId
    }
    allowVirtualNetworkAccess: true
    allowForwardedTraffic: allowForwardedTraffic
    allowGatewayTransit: allowGatewayTransit
    useRemoteGateways: useRemoteGateways
  }
}
