@description('Azure region')
param location string

@description('Container Apps Environment name')
param containerAppsEnvironmentName string

@description('Key Vault name (in shared resource group)')
param keyVaultName string

@description('Shared resource group name')
param sharedResourceGroupName string

@description('Wildcard certificate names in Key Vault, keyed by base domain (e.g. { "hub.ms": "wildcard-hub-ms" })')
param wildcardCertNames object

@description('User-assigned managed identity resource ID (used to access Key Vault)')
param identityId string

@description('Principal ID of the managed identity (for RBAC assignment)')
param identityPrincipalId string

// Grant Key Vault Secrets User to the managed identity on the shared Key Vault
// (needed to read certificate secrets from Key Vault)
module kvSecretsRole 'kvSecretsUserRole.bicep' = {
  name: 'kvSecretsRole-deployment'
  scope: resourceGroup(sharedResourceGroupName)
  params: {
    keyVaultName: keyVaultName
    principalId: identityPrincipalId
  }
}

// Deploy each wildcard certificate via sub-module (works around BCP247 lambda limitation).
// Each entry maps a base domain to a Key Vault certificate name.
var certEntries = items(wildcardCertNames)
module certDeployments 'wildcardCert.bicep' = [for entry in certEntries: {
  name: 'wildcardCert-${replace(entry.key, '.', '-')}'
  params: {
    location: location
    containerAppsEnvironmentName: containerAppsEnvironmentName
    certResourceName: 'wildcard-${replace(entry.key, '.', '-')}'
    keyVaultUrl: 'https://${keyVaultName}${environment().suffixes.keyvaultDns}/secrets/${entry.value}'
    identityId: identityId
  }
  dependsOn: [kvSecretsRole]
}]

// Output the certificate IDs as an array of {baseDomain, certId} objects.
// The parent module must reconstruct the map due to BCP247 limitations.
output certCount int = length(certEntries)
output certDomains string[] = [for entry in certEntries: entry.key]
output certIds string[] = [for (entry, i) in certEntries: certDeployments[i].outputs.certificateId]
