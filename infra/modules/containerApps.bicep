param location string
param environmentName string
param logAnalyticsWorkspaceId string

@description('Subnet resource ID for VNet integration')
param infrastructureSubnetId string

resource containerAppsEnvironment 'Microsoft.App/managedEnvironments@2025-01-01' = {
  name: environmentName
  location: location
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

// Route container app logs to Log Analytics via diagnostic settings
// instead of the shared key approach, which triggers redeployments.
resource diagnosticSettings 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  name: 'logs-to-law'
  scope: containerAppsEnvironment
  properties: {
    workspaceId: logAnalyticsWorkspaceId
    logs: [
      {
        categoryGroup: 'allLogs'
        enabled: true
      }
    ]
  }
}

output environmentId string = containerAppsEnvironment.id
output defaultDomain string = containerAppsEnvironment.properties.defaultDomain
