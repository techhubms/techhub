// Writes a single secret value to the shared Key Vault.
// Called conditionally from main.bicep so unchanged secrets are not rewritten on every deploy.
// Container Apps reference these secrets via keyVaultUrl — see api.bicep / web.bicep.

@description('Key Vault name (must exist in the current resource group scope)')
param keyVaultName string

@description('Secret name (KV secret names may contain letters, digits and dashes)')
param secretName string

@secure()
@description('Secret value')
param secretValue string

@description('Optional tags (not currently used by KV secrets but kept for consistency)')
param tags object = {}

resource keyVault 'Microsoft.KeyVault/vaults@2024-04-01-preview' existing = {
  name: keyVaultName
}

resource secret 'Microsoft.KeyVault/vaults/secrets@2024-04-01-preview' = {
  parent: keyVault
  name: secretName
  tags: tags
  properties: {
    value: secretValue
  }
}

output secretUri string = secret.properties.secretUri
// secretUri includes the version; strip it so Container Apps always pick the latest version.
output secretUriLatest string = '${keyVault.properties.vaultUri}secrets/${secret.name}'
