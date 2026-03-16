@description('Azure region')
param location string

@description('Name for the private endpoint resource')
param privateEndpointName string

@description('Subnet ID for the private endpoint (must not be delegated)')
param subnetId string

@description('Resource ID of the PostgreSQL Flexible Server to connect to')
param postgresServerId string

@description('VNet ID to link the private DNS zone to')
param vnetId string

@description('Hub VNet ID to also link the private DNS zone to (for VPN access)')
param hubVnetId string = ''

// Private DNS Zone for PostgreSQL Flexible Server private endpoints
resource privateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink.postgres.database.azure.com'
  location: 'global'
}

// Link DNS Zone to spoke VNet so containers can resolve the PostgreSQL private IP
resource privateDnsZoneLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = {
  parent: privateDnsZone
  name: '${privateEndpointName}-link'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: vnetId
    }
  }
}

// Link DNS Zone to hub VNet so VPN clients can resolve the PostgreSQL private IP
resource privateDnsZoneHubLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = if (!empty(hubVnetId)) {
  parent: privateDnsZone
  name: '${privateEndpointName}-hub-link'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: hubVnetId
    }
  }
}

// Private Endpoint connecting to the PostgreSQL Flexible Server
resource privateEndpoint 'Microsoft.Network/privateEndpoints@2025-01-01' = {
  name: privateEndpointName
  location: location
  properties: {
    subnet: {
      id: subnetId
    }
    privateLinkServiceConnections: [
      {
        name: '${privateEndpointName}-connection'
        properties: {
          privateLinkServiceId: postgresServerId
          groupIds: [
            'postgresqlServer'
          ]
        }
      }
    ]
  }
}

// Auto-register the private endpoint IP in the private DNS zone
resource dnsZoneGroup 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2025-01-01' = {
  parent: privateEndpoint
  name: 'default'
  properties: {
    privateDnsZoneConfigs: [
      {
        name: 'privatelink-postgres-database-azure-com'
        properties: {
          privateDnsZoneId: privateDnsZone.id
        }
      }
    ]
  }
}
