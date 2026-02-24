@description('Name of the Container Registry to grant AcrPull on')
param containerRegistryName string

@description('Principal ID of the managed identity to grant access to')
param principalId string

// AcrPull built-in role definition
var acrPullRoleDefinitionId = subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d')

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2025-04-01' existing = {
  name: containerRegistryName
}

resource acrPullRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(principalId, containerRegistry.id, acrPullRoleDefinitionId)
  scope: containerRegistry
  properties: {
    principalId: principalId
    roleDefinitionId: acrPullRoleDefinitionId
    principalType: 'ServicePrincipal'
  }
}
