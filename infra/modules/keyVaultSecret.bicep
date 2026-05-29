// Helper module to add secrets to Key Vault from subscription-scoped deployment
param keyVaultName string
param secretName string

@secure()
param secretValue string

resource keyVault 'Microsoft.KeyVault/vaults@2024-04-01-preview' existing = {
  name: keyVaultName
}

resource secret 'Microsoft.KeyVault/vaults/secrets@2024-04-01-preview' = {
  parent: keyVault
  name: secretName
  properties: {
    value: secretValue
  }
}

output secretUri string = secret.properties.secretUri
