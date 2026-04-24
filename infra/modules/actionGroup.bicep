// Shared action group for operational alerts — email notifications to the maintainer.
// Global-scope resource, deployed once in the shared resource group.

@description('Name of the action group')
param actionGroupName string = 'ag-techhub-ops'

@description('Short name (max 12 chars) shown in alert notifications')
param shortName string = 'techhubops'

@description('Email address to notify on alerts')
param emailAddress string

@description('Tags to apply to all resources')
param tags object = {}

resource actionGroup 'Microsoft.Insights/actionGroups@2023-01-01' = {
  name: actionGroupName
  location: 'global'
  tags: tags
  properties: {
    groupShortName: shortName
    enabled: true
    emailReceivers: [
      {
        name: 'maintainer'
        emailAddress: emailAddress
        useCommonAlertSchema: true
      }
    ]
  }
}

output actionGroupId string = actionGroup.id
output actionGroupName string = actionGroup.name
