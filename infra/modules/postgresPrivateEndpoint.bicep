@description('Azure region')
param location string

@description('Name for the private endpoint resource')
param privateEndpointName string

@description('Subnet ID for the private endpoint (must not be delegated)')
param subnetId string

@description('Resource ID of the PostgreSQL Flexible Server to connect to')
param postgresServerId string

@description('Shared resource group name where the PostgreSQL private DNS zone lives')
param sharedResourceGroupName string

// Reference the shared PostgreSQL private DNS zone (created in shared.bicep)
resource privateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' existing = {
  name: 'privatelink.postgres.database.azure.com'
  scope: resourceGroup(sharedResourceGroupName)
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
