param location string
param vaultName string

@description('Azure AD object IDs that should get full Key Vault management access')
param adminObjectIds string[] = []

@description('Log Analytics Workspace ID for audit logging (optional)')
param logAnalyticsWorkspaceId string = ''

@description('Admin IP addresses for firewall rules (app traffic uses the private endpoint)')
param adminIpAddresses string[]

resource keyVault 'Microsoft.KeyVault/vaults@2024-04-01-preview' = {
  name: vaultName
  location: location
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
    }
  }
}

// Grant Key Vault Administrator role to specified principals
resource adminRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = [for (objectId, i) in adminObjectIds: {
  name: guid(keyVault.id, objectId, 'Key Vault Administrator')
  scope: keyVault
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '00482a5a-887f-4fb3-b363-3b7fe8e74483') // Key Vault Administrator
    principalId: objectId
    principalType: 'User'
  }
}]

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
