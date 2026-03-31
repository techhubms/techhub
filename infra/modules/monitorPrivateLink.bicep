// Azure Monitor Private Link Scope (AMPLS) — routes app telemetry privately through the VNet.
// Connects Application Insights and Log Analytics workspaces to a private endpoint in the hub VNet.
// Access mode: Open (allows both private and public ingestion/query).

@description('Azure region for the private endpoint')
param location string

@description('AMPLS name')
param amplsName string

@description('Subnet ID in the hub VNet for the AMPLS private endpoint')
param subnetId string

@description('VNet ID for private DNS zone links (hub VNet)')
param vnetId string

@description('Additional spoke VNet IDs to link AMPLS DNS zones to')
param spokeVnetIds string[] = []

@description('Application Insights resource IDs to scope')
param appInsightsIds string[]

@description('Log Analytics workspace resource IDs to scope')
param logAnalyticsWorkspaceIds string[]

// Azure Monitor Private Link Scope
resource ampls 'microsoft.insights/privateLinkScopes@2021-07-01-preview' = {
  name: amplsName
  location: 'global'
  properties: {
    accessModeSettings: {
      ingestionAccessMode: 'Open'
      queryAccessMode: 'Open'
    }
  }
}

// Scope Application Insights resources
resource appInsightsConnections 'Microsoft.Insights/privateLinkScopes/scopedResources@2021-07-01-preview' = [for (id, i) in appInsightsIds: {
  parent: ampls
  name: 'scoped-appi-${i}'
  properties: {
    linkedResourceId: id
  }
}]

// Scope Log Analytics workspaces
resource logAnalyticsConnections 'Microsoft.Insights/privateLinkScopes/scopedResources@2021-07-01-preview' = [for (id, i) in logAnalyticsWorkspaceIds: {
  parent: ampls
  name: 'scoped-law-${i}'
  properties: {
    linkedResourceId: id
  }
}]

// Private DNS zones required by AMPLS
// Suppressed: these are Azure private DNS zone names, not environment URLs
var dnsZoneNames = [
  'privatelink.monitor.azure.com'
  'privatelink.oms.opinsights.azure.com'
  'privatelink.ods.opinsights.azure.com'
  'privatelink.agentsvc.azure-automation.net'
  #disable-next-line no-hardcoded-env-urls
  'privatelink.blob.core.windows.net'
]

resource dnsZones 'Microsoft.Network/privateDnsZones@2024-06-01' = [for zone in dnsZoneNames: {
  name: zone
  location: 'global'
}]

resource dnsZoneLinks 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = [for (zone, i) in dnsZoneNames: {
  parent: dnsZones[i]
  name: 'link-hub'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: vnetId
    }
  }
}]

// Link AMPLS DNS zones to spoke VNets (so spoke workloads resolve telemetry through private endpoint)
module spokeDnsLinks './amplsSpokeLinks.bicep' = [for (spokeVnetId, si) in spokeVnetIds: {
  name: 'ampls-spoke-dns-${si}'
  params: {
    dnsZoneNames: dnsZoneNames
    spokeVnetId: spokeVnetId
    spokeIndex: si
  }
  dependsOn: [dnsZones]
}]

// AMPLS Private Endpoint in hub VNet
resource privateEndpoint 'Microsoft.Network/privateEndpoints@2025-01-01' = {
  name: 'pe-${amplsName}'
  location: location
  properties: {
    subnet: {
      id: subnetId
    }
    privateLinkServiceConnections: [
      {
        name: 'pe-${amplsName}-connection'
        properties: {
          privateLinkServiceId: ampls.id
          groupIds: [
            'azuremonitor'
          ]
        }
      }
    ]
  }
}

// Auto-register the private endpoint IP in all AMPLS DNS zones
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

output amplsId string = ampls.id
