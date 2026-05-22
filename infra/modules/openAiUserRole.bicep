@description('Azure AI Foundry (Cognitive Services) account name')
param openAiName string

@description('Principal ID to grant Cognitive Services OpenAI User role')
param principalId string

@description('Principal type: ServicePrincipal (managed identity) or User')
@allowed(['ServicePrincipal', 'User', 'Group'])
param principalType string = 'ServicePrincipal'

resource openAiAccount 'Microsoft.CognitiveServices/accounts@2025-06-01' existing = {
  name: openAiName
}

// Cognitive Services OpenAI User — grants inference (chat completion) access via Entra token.
// Applications use DefaultAzureCredential with scope https://cognitiveservices.azure.com/.default
// instead of an API key. Local developers can use 'az login' once this role is assigned to them.
resource openAiUserRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(openAiAccount.id, principalId, 'Cognitive Services OpenAI User')
  scope: openAiAccount
  properties: {
    // Cognitive Services OpenAI User (5e0bd9bd-7b93-4f28-af87-19fc36ad61bd)
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '5e0bd9bd-7b93-4f28-af87-19fc36ad61bd')
    principalId: principalId
    principalType: principalType
  }
}
