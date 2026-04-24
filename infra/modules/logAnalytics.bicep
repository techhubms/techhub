// Log Analytics workspace module — used by the shared deployment for Key Vault audit logs.

@description('Azure region for the workspace')
param location string

@description('Log Analytics Workspace name')
param logAnalyticsWorkspaceName string

@description('Log Analytics daily ingestion cap in GB (-1 = unlimited)')
param dailyQuotaGb int = -1

@description('Tags applied to the Log Analytics workspace')
param tags object = {}

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2025-02-01' = {
  name: logAnalyticsWorkspaceName
  location: location
  tags: tags
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
    publicNetworkAccessForIngestion: 'Disabled'
    // Query enabled: allows portal and admin access (protected by RBAC)
    publicNetworkAccessForQuery: 'Enabled'
    workspaceCapping: {
      dailyQuotaGb: dailyQuotaGb
    }
  }
}

output logAnalyticsWorkspaceId string = logAnalyticsWorkspace.id
