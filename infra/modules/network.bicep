@description('Azure region for resources')
param location string

@description('VNet name')
param vnetName string

@description('VNet address space prefix')
param addressSpacePrefix string = '10.0.0.0/16'

@description('App Service Regional VNet Integration subnet name')
param appServiceSubnetName string = 'snet-app-service'

@description('App Service Regional VNet Integration subnet prefix')
param appServiceSubnetPrefix string = '10.0.0.0/23'

@description('PR-preview App Service Plan Regional VNet Integration subnet name. Shared by all PR preview sites, which sit on their own dedicated App Service Plan (kept separate from production to avoid PR traffic ever affecting prod memory/CPU).')
param appServicePrSubnetName string = 'snet-app-service-pr'

@description('PR-preview App Service Plan Regional VNet Integration subnet prefix')
param appServicePrSubnetPrefix string = '10.0.4.0/23'

@description('Private endpoints subnet name')
param privateEndpointsSubnetName string = 'snet-private-endpoints'

@description('Private endpoints subnet prefix')
param privateEndpointsSubnetPrefix string = '10.0.2.0/27'

@description('Tags applied to networking resources')
param tags object = {}

// Virtual Network with an App Service Regional VNet Integration subnet and a private
// endpoints subnet. Key Vault, PostgreSQL, and AI Foundry all use private endpoints in the
// dedicated subnet below — private endpoints cannot share a subnet delegated to
// Microsoft.Web/serverFarms.
resource vnet 'Microsoft.Network/virtualNetworks@2025-01-01' = {
  name: vnetName
  location: location
  tags: tags
  properties: {
    addressSpace: {
      addressPrefixes: [
        addressSpacePrefix
      ]
    }
    enableDdosProtection: false
    subnets: [
      {
        name: appServiceSubnetName
        properties: {
          addressPrefix: appServiceSubnetPrefix
          delegations: [
            {
              name: 'Microsoft.Web.serverFarms'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
              }
            }
          ]
          // Enables VNet-based access restrictions (e.g. api.bicep's ipSecurityRestrictions)
          // to recognize traffic sourced from this subnet.
          serviceEndpoints: [
            {
              service: 'Microsoft.Web'
            }
          ]
        }
      }
      {
        // Regional VNet Integration is strictly 1 subnet : 1 App Service Plan — the
        // PR-preview Plan (a separate Plan from production, see infrastructure.bicep) needs
        // its own dedicated subnet even though it is deployed and reused persistently
        // (not created/torn down per PR).
        name: appServicePrSubnetName
        properties: {
          addressPrefix: appServicePrSubnetPrefix
          delegations: [
            {
              name: 'Microsoft.Web.serverFarms'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'
              }
            }
          ]
          serviceEndpoints: [
            {
              service: 'Microsoft.Web'
            }
          ]
        }
      }
      {
        name: privateEndpointsSubnetName
        properties: {
          addressPrefix: privateEndpointsSubnetPrefix
          privateEndpointNetworkPolicies: 'Disabled'
        }
      }
    ]
  }
}

// Private DNS zone for PostgreSQL private endpoints — linked to the VNet so the App Service
// Plan's VNet-integrated apps resolve <server>.postgres.database.azure.com to the private
// endpoint IP automatically.
resource postgresPrivateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink.postgres.database.azure.com'
  location: 'global'
  tags: tags
}

resource postgresPrivateDnsZoneVnetLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = {
  parent: postgresPrivateDnsZone
  name: '${vnetName}-link'
  location: 'global'
  properties: {
    virtualNetwork: {
      id: vnet.id
    }
    registrationEnabled: false
  }
}

// Private DNS zone for Key Vault private endpoints — linked to the VNet so App Service
// resolves <vault>.vault.azure.net to the private endpoint IP automatically.
resource keyVaultPrivateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink.vaultcore.azure.net'
  location: 'global'
  tags: tags
}

resource keyVaultPrivateDnsZoneVnetLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = {
  parent: keyVaultPrivateDnsZone
  name: '${vnetName}-link'
  location: 'global'
  properties: {
    virtualNetwork: {
      id: vnet.id
    }
    registrationEnabled: false
  }
}

// Private DNS zone for AI Foundry (Azure OpenAI) private endpoints — linked to the VNet so
// App Service resolves <account>.openai.azure.com to the private endpoint IP automatically.
// This is the domain the application actually calls (see AiCategorizationOptions.Endpoint).
resource openAiPrivateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink.openai.azure.com'
  location: 'global'
  tags: tags
}

resource openAiPrivateDnsZoneVnetLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = {
  parent: openAiPrivateDnsZone
  name: '${vnetName}-link'
  location: 'global'
  properties: {
    virtualNetwork: {
      id: vnet.id
    }
    registrationEnabled: false
  }
}

// Private DNS zone for AI Foundry (Cognitive Services) private endpoints — linked to the VNet so
// App Service resolves <account>.cognitiveservices.azure.com to the private endpoint IP automatically.
// The account is kind 'AIServices' (multi-service, "Foundry Tools" in Microsoft's private
// endpoint DNS zone reference), which exposes management/data-plane endpoints across
// openai.azure.com, cognitiveservices.azure.com, and services.ai.azure.com; all three zones are
// registered on the same private endpoint's DNS zone group per Microsoft's documented mapping for
// Microsoft.CognitiveServices/accounts (subresource "account").
resource cognitiveServicesPrivateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink.cognitiveservices.azure.com'
  location: 'global'
  tags: tags
}

resource cognitiveServicesPrivateDnsZoneVnetLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = {
  parent: cognitiveServicesPrivateDnsZone
  name: '${vnetName}-link'
  location: 'global'
  properties: {
    virtualNetwork: {
      id: vnet.id
    }
    registrationEnabled: false
  }
}

// Private DNS zone for AI Foundry (AI Services) private endpoints — linked to the VNet so
// App Service resolves <account>.services.ai.azure.com to the private endpoint IP automatically.
// This is the third zone Microsoft's private endpoint DNS reference lists for
// Microsoft.CognitiveServices/accounts, used by newer AI Foundry project/agent APIs.
resource servicesAiPrivateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink.services.ai.azure.com'
  location: 'global'
  tags: tags
}

resource servicesAiPrivateDnsZoneVnetLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = {
  parent: servicesAiPrivateDnsZone
  name: '${vnetName}-link'
  location: 'global'
  properties: {
    virtualNetwork: {
      id: vnet.id
    }
    registrationEnabled: false
  }
}

// Outputs
// Subnet IDs are looked up by name rather than array index — indexing is brittle because
// reordering or inserting subnets would silently change which subnet ID is exported.
output vnetId string = vnet.id
output vnetName string = vnet.name
output appServiceSubnetId string = filter(vnet.properties.subnets, s => s.name == appServiceSubnetName)[0].id
output appServicePrSubnetId string = filter(vnet.properties.subnets, s => s.name == appServicePrSubnetName)[0].id
output privateEndpointsSubnetId string = filter(vnet.properties.subnets, s => s.name == privateEndpointsSubnetName)[0].id
output postgresPrivateDnsZoneId string = postgresPrivateDnsZone.id
output keyVaultPrivateDnsZoneId string = keyVaultPrivateDnsZone.id
output openAiPrivateDnsZoneId string = openAiPrivateDnsZone.id
output cognitiveServicesPrivateDnsZoneId string = cognitiveServicesPrivateDnsZone.id
output servicesAiPrivateDnsZoneId string = servicesAiPrivateDnsZone.id
