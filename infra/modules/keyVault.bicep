param location string
param vaultName string

@description('Log Analytics Workspace ID for audit logging (optional)')
param logAnalyticsWorkspaceId string = ''

@description('Admin IP addresses for firewall rules')
param adminIpAddresses string[]

@description('Subnet ID for the Key Vault private endpoint. The private endpoint is only created when both this and privateDnsZoneId are non-empty; supplying only one silently skips creation.')
param privateEndpointSubnetId string = ''

@description('Private DNS zone ID for privatelink.vaultcore.azure.net. The private endpoint is only created when both this and privateEndpointSubnetId are non-empty; supplying only one silently skips creation.')
param privateDnsZoneId string = ''

@description('Tags applied to the Key Vault')
param tags object = {}

var deployPrivateEndpoint = !empty(privateEndpointSubnetId) && !empty(privateDnsZoneId)

resource keyVault 'Microsoft.KeyVault/vaults@2024-04-01-preview' = {
  name: vaultName
  location: location
  tags: tags
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    enableRbacAuthorization: true
    enableSoftDelete: true
    softDeleteRetentionInDays: 90
    enablePurgeProtection: true
    // Public access is only needed for admin IP allowlisting — App Service sites reach Key Vault
    // over the private endpoint below, not the public endpoint.
    publicNetworkAccess: !empty(adminIpAddresses) ? 'Enabled' : 'Disabled'
    networkAcls: {
      defaultAction: 'Deny'
      bypass: 'None'
      ipRules: [for ip in adminIpAddresses: { value: ip }]
    }
  }
}

// Private endpoint — gives App Service sites a private IP path to Key Vault, removing the need
// for the VNet service endpoint (which still crossed the Microsoft backbone to a public endpoint).
resource keyVaultPrivateEndpoint 'Microsoft.Network/privateEndpoints@2024-05-01' = if (deployPrivateEndpoint) {
  name: 'pe-${vaultName}'
  location: location
  tags: tags
  properties: {
    subnet: {
      id: privateEndpointSubnetId
    }
    privateLinkServiceConnections: [
      {
        name: 'pe-${vaultName}-connection'
        properties: {
          privateLinkServiceId: keyVault.id
          groupIds: ['vault']
        }
      }
    ]
  }
}

resource keyVaultPrivateDnsZoneGroup 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2024-05-01' = if (deployPrivateEndpoint) {
  parent: keyVaultPrivateEndpoint
  name: 'default'
  properties: {
    privateDnsZoneConfigs: [
      {
        name: 'vault-config'
        properties: {
          privateDnsZoneId: privateDnsZoneId
        }
      }
    ]
  }
}

// Audit logging for Key Vault operations
resource diagnosticSettings 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = if (!empty(logAnalyticsWorkspaceId)) {
  name: 'audit-logs'
  scope: keyVault
  properties: {
    workspaceId: logAnalyticsWorkspaceId
    logs: [
      {
        categoryGroup: 'audit'
        enabled: true
      }
    ]
  }
}

output vaultName string = keyVault.name
output vaultUri string = keyVault.properties.vaultUri
output vaultId string = keyVault.id
