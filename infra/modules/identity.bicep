@description('Azure region for the managed identity')
param location string

@description('Name of the user-assigned managed identity')
param identityName string

@description('Tags applied to the managed identity')
param tags object = {}

resource identity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: identityName
  location: location
  tags: tags
}

output identityId string = identity.id
output identityPrincipalId string = identity.properties.principalId
