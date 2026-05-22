param location string
param vaultName string

@description('Log Analytics Workspace ID for audit logging (optional)')
param logAnalyticsWorkspaceId string = ''

@description('Admin IP addresses for firewall rules')
param adminIpAddresses string[]

@description('Container Apps subnet resource ID (service endpoint grants VNet access without a private endpoint)')
param containerAppsSubnetId string

@description('Tags applied to the Key Vault')
param tags object = {}

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
    publicNetworkAccess: 'Enabled'
    networkAcls: {
      defaultAction: 'Deny'
      bypass: 'None'
      ipRules: [for ip in adminIpAddresses: { value: ip }]
      // VNet service endpoint on the Container Apps subnet allows Container Apps to
      // reach Key Vault over the Microsoft backbone (no private endpoint required).
      virtualNetworkRules: [
        {
          id: containerAppsSubnetId
          ignoreMissingVnetServiceEndpoint: false
        }
      ]
    }
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
