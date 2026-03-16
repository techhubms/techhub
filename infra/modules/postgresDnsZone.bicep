// Shared PostgreSQL private DNS zone — created once in shared resource group,
// linked from each spoke environment via privateDnsZoneLink module.

@description('Hub VNet ID to link the DNS zone to (for VPN access)')
param hubVnetId string = ''

resource privateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink.postgres.database.azure.com'
  location: 'global'
}

// Link to hub VNet so VPN clients can resolve PostgreSQL private endpoints
resource hubLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = if (!empty(hubVnetId)) {
  parent: privateDnsZone
  name: 'hub-link'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: hubVnetId
    }
  }
}

output dnsZoneId string = privateDnsZone.id
output dnsZoneName string = privateDnsZone.name
