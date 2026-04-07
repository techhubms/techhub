// AI Foundry (Cognitive Services) private endpoint per spoke VNet.
// groupIds: ['account'] requires three private DNS zones:
//   - privatelink.cognitiveservices.azure.com
//   - privatelink.openai.azure.com (routes <name>.openai.azure.com — what Container Apps call)
//   - privatelink.services.ai.azure.com

@description('Azure region for the private endpoint')
param location string

@description('Name for the private endpoint resource')
param privateEndpointName string

@description('Subnet ID for the private endpoint (must not be delegated)')
param subnetId string

@description('Resource ID of the AI Foundry (Cognitive Services) account')
param openAiAccountId string

@description('VNet ID to link the private DNS zones to')
param vnetId string

// DNS zones required for AIServices account private endpoint
var dnsZoneNames = [
  'privatelink.cognitiveservices.azure.com'
  'privatelink.openai.azure.com'
  'privatelink.services.ai.azure.com'
]

// DNS zones required for AIServices account private endpoint (idempotent — safe to redeploy)
resource dnsZones 'Microsoft.Network/privateDnsZones@2024-06-01' = [for zone in dnsZoneNames: {
  name: zone
  location: 'global'
}]

// Link DNS zones to the spoke VNet
resource dnsZoneLinks 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = [for (zone, i) in dnsZoneNames: {
  parent: dnsZones[i]
  name: '${privateEndpointName}-link'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: vnetId
    }
  }
}]

// Private Endpoint connecting to the AI Foundry account
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
          privateLinkServiceId: openAiAccountId
          groupIds: [
            'account'
          ]
        }
      }
    ]
  }
}

// Auto-register the private endpoint IP in all required DNS zones
resource dnsZoneGroup 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2025-01-01' = {
  parent: privateEndpoint
  name: 'default'
  properties: {
    privateDnsZoneConfigs: [for (zone, i) in dnsZoneNames: {
      name: replace(zone, '.', '-')
      properties: {
        privateDnsZoneId: dnsZones[i].id
      }
    }]
  }
}
