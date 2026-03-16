@description('Name of the existing private DNS zone (e.g. privatelink.vaultcore.azure.net)')
param dnsZoneName string

@description('Name for the VNet link')
param linkName string

@description('Resource ID of the VNet to link')
param vnetId string

resource dnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' existing = {
  name: dnsZoneName
}

resource link 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = {
  parent: dnsZone
  name: linkName
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: vnetId
    }
  }
}
