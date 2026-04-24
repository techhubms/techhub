param location string
param environmentName string
param logAnalyticsWorkspaceId string

@description('Subnet resource ID for VNet integration')
param infrastructureSubnetId string

@description('Optional user-assigned managed identity resource ID (needed for Key Vault certificate access)')
param identityId string = ''

@description('Tags applied to the Container Apps Environment')
param tags object = {}

resource containerAppsEnvironment 'Microsoft.App/managedEnvironments@2025-07-01' = {
  name: environmentName
  location: location
  tags: tags
  identity: !empty(identityId) ? {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${identityId}': {}
    }
  } : {
    type: 'None'
  }
  properties: {
    appLogsConfiguration: {
      destination: 'azure-monitor'
    }
    vnetConfiguration: {
      infrastructureSubnetId: infrastructureSubnetId
      internal: false
    }
    workloadProfiles: [
      {
        name: 'Consumption'
        workloadProfileType: 'Consumption'
      }
    ]
  }
}

// Route container app system logs to Log Analytics via diagnostic settings.
// Application console logs (stdout/stderr) are intentionally excluded:
// structured logs already flow to Application Insights via OpenTelemetry,
// so console logs would be double-logging (~630 MB/month saved).
resource diagnosticSettings 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  name: 'logs-to-law'
  scope: containerAppsEnvironment
  properties: {
    workspaceId: logAnalyticsWorkspaceId
    logs: [
      {
        category: 'ContainerAppSystemLogs'
        enabled: true
      }
    ]
  }
}

output environmentId string = containerAppsEnvironment.id
output defaultDomain string = containerAppsEnvironment.properties.defaultDomain
