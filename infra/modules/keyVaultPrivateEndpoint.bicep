@description('Azure region')
param location string

@description('Name for the private endpoint resource')
param privateEndpointName string

@description('Subnet ID for the private endpoint (must not be delegated)')
param subnetId string

@description('Resource ID of the Key Vault to connect to')
param keyVaultId string

@description('VNet ID to link the private DNS zone to')
param vnetId string

@description('Additional VNet IDs to link the private DNS zone to (e.g. spoke VNets)')
param additionalVnetIds array = []

// Private DNS Zone for Key Vault
resource privateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink.vaultcore.azure.net'
  location: 'global'
}

// Link DNS Zone to primary VNet (hub)
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

// Link DNS Zone to additional VNets (spoke VNets, so containers can resolve the KV private IP)
resource additionalDnsZoneLinks 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = [for (additionalVnetId, i) in additionalVnetIds: {
  parent: privateDnsZone
  name: '${privateEndpointName}-spoke-${i}-link'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: additionalVnetId
    }
  }
}]

// Private Endpoint connecting to the Key Vault
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
          privateLinkServiceId: keyVaultId
          groupIds: [
            'vault'
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
        name: 'privatelink-vaultcore-azure-net'
        properties: {
          privateDnsZoneId: privateDnsZone.id
        }
      }
    ]
  }
}
