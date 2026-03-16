@description('Azure region')
param location string

@description('Container Apps Environment name')
param containerAppsEnvironmentName string

@description('Certificate resource name in the managed environment')
param certResourceName string

@description('Key Vault secret URL for the certificate')
param keyVaultUrl string

@description('User-assigned managed identity resource ID')
param identityId string

resource containerAppsEnv 'Microsoft.App/managedEnvironments@2025-07-01' existing = {
  name: containerAppsEnvironmentName
}

resource cert 'Microsoft.App/managedEnvironments/certificates@2025-07-01' = {
  parent: containerAppsEnv
  name: certResourceName
  location: location
  properties: {
    certificateKeyVaultProperties: {
      identity: identityId
      keyVaultUrl: keyVaultUrl
    }
  }
}

output certificateId string = cert.id
