// Links all AMPLS DNS zones to a single spoke VNet.
// Called once per spoke VNet from monitorPrivateLink.bicep.

@description('AMPLS DNS zone names to create VNet links for')
param dnsZoneNames string[]

@description('Spoke VNet resource ID')
param spokeVnetId string

resource dnsZones 'Microsoft.Network/privateDnsZones@2024-06-01' existing = [for zone in dnsZoneNames: {
  name: zone
}]

resource links 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = [for (zone, i) in dnsZoneNames: {
  parent: dnsZones[i]
  name: 'link-spoke-${uniqueString(spokeVnetId)}'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: spokeVnetId
    }
  }
}]
