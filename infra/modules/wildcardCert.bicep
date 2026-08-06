@description('Azure region')
param location string

@description('App Service Plan resource ID the certificate is imported against (App Service certificates are scoped to a resource group but priced/validated against a plan\'s region)')
param appServicePlanId string

@description('Certificate resource name (e.g. wildcard-hub-ms)')
param certResourceName string

@description('Key Vault resource ID (not URL) holding the PFX certificate secret')
param keyVaultResourceId string

@description('Key Vault secret name holding the PFX certificate')
param keyVaultSecretName string

// Imports a certificate directly from Key Vault into Microsoft.Web/certificates so it can be
// SNI-bound to a Web App custom domain (see modules/web.bicep's hostNameBindings).
//
// One-time prerequisite (cannot be expressed in Bicep — run once per tenant/Key Vault):
// the first-party "Microsoft Azure App Service" service principal (app ID
// abfa0a7c-a6b6-4736-8310-5855508787cd) must be granted the "Key Vault Certificate User" (and
// "Key Vault Secrets User") RBAC role on this Key Vault so the App Service certificate provider
// can read the PFX secret. See docs/wildcard-certificates.md for the exact role assignment.
resource cert 'Microsoft.Web/certificates@2023-12-01' = {
  name: certResourceName
  location: location
  properties: {
    serverFarmId: appServicePlanId
    keyVaultId: keyVaultResourceId
    keyVaultSecretName: keyVaultSecretName
  }
}

output certificateId string = cert.id
output thumbprint string = cert.properties.thumbprint
