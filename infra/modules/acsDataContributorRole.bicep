@description('Azure Communication Services resource name')
param communicationServiceName string

@description('Principal ID to grant Contributor role on the ACS resource (scoped to the ACS resource; enables email sending via managed identity)')
param principalId string

@description('Principal type: ServicePrincipal (managed identity) or User')
@allowed(['ServicePrincipal', 'User', 'Group'])
param principalType string = 'ServicePrincipal'

resource communicationService 'Microsoft.Communication/communicationServices@2026-03-18' existing = {
  name: communicationServiceName
}

// Contributor role scoped to ACS resource — grants data plane access for email sending via managed identity.
// Applications use DefaultAzureCredential with EmailClient(endpoint, credential) instead of connection string.
// Local developers can use 'az login' once this role is assigned to them.
resource acsContributorRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(communicationService.id, principalId, 'Contributor')
  scope: communicationService
  properties: {
    // Contributor (b24988ac-6180-42a0-ab88-20f7382dd24c) scoped to ACS resource
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', 'b24988ac-6180-42a0-ab88-20f7382dd24c')
    principalId: principalId
    principalType: principalType
  }
}
