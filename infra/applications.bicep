targetScope = 'subscription'

// Phase 2: Container App deployments for Tech Hub production.
// Reads existing infrastructure resources created by infrastructure.bicep via `existing`
// references — no cross-deployment output passing required.
// Run after infrastructure.bicep and after secrets have been synced to Key Vault.

@description('Azure region for resources')
param location string = 'swedencentral'

@description('Resource group name (must already exist — created by infrastructure.bicep)')
param resourceGroupName string = 'rg-techhub-prod'

@description('Application Insights name (existing resource)')
param appInsightsName string = 'appi-techhub-prod'

@description('Key Vault name (existing resource)')
param keyVaultName string = 'kv-techhub-prod'

@description('Container Apps Environment name (existing resource)')
param containerAppsEnvName string = 'cae-techhub-prod'

@description('API Container App name')
param apiAppName string = 'ca-techhub-api-prod'

@description('Web Container App name')
param webAppName string = 'ca-techhub-web-prod'

@description('API Docker image tag (yyyyMMddHHmmss format)')
param apiImageTag string = ''

@description('Web Docker image tag (yyyyMMddHHmmss format)')
param webImageTag string = ''

@description('Azure AD tenant ID for admin dashboard authentication')
param azureAdTenantId string = ''

@description('Azure AD client ID for admin dashboard authentication')
param azureAdClientId string = ''

@description('Primary host names for the web app (e.g. ["tech.hub.ms", "tech.xebia.ms"])')
param primaryHosts string[] = []

@description('Wildcard certificate names in Key Vault, keyed by base domain')
param wildcardCertNames object = {}

@description('PostgreSQL server name (existing resource)')
param postgresServerName string = 'psql-techhub-prod'

@description('Azure AI Foundry (OpenAI) resource name (existing resource)')
param openAiName string = 'oai-techhub-prod'

@description('GitHub organization username for ghcr.io registry')
param githubRegistryUsername string = 'techhubms'

@description('GitHub username of the PAT owner for ghcr.io authentication')
param githubRegistryAuthUsername string = githubRegistryUsername

@description('Application Insights connection string override. Default (@existing) reads the value from the existing Azure resource. Set to empty string to disable telemetry (e.g. for PR preview environments).')
param appInsightsConnectionString string = '@existing'

@description('Google Analytics Measurement ID (e.g. G-XXXXXXXXXX). Set to empty string to disable GA telemetry for PR preview environments.')
param googleAnalyticsMeasurementId string = 'G-95LLB67KJV'

@description('Key Vault secret name for Newsletter ACS endpoint URL. Populated by Deploy-Infrastructure.ps1 via Sync-KeyVaultSecrets.ps1.')
param acsEndpointSecretName string = 'techhub-prod-newsletter-acs-endpoint'

@description('Common tags applied to all resources managed by this template')
param commonTags object = {
  owner: 'techhub-maintainer'
  project: 'techhub'
  managedBy: 'bicep'
}

@description('UTC timestamp used to make nested deployment names unique per run.')
param deploymentTimestamp string = utcNow()

// Tags for all prod resources
var prodTags = union(commonTags, { env: 'prod' })

// Short unique hash per run — appended to all nested module `name` values to prevent DeploymentActive conflicts
var deploymentSuffix = uniqueString(deploymentTimestamp)

// Managed identity name — must match the value used in infrastructure.bicep
var prodIdentityName = 'id-techhub-prod'

// Database name — must match the default in modules/postgres.bicep
var prodDatabaseName = 'techhub'

// OpenAI model deployment name — must match the default in modules/openai.bicep
var openAiDeploymentModelName = 'gpt-5.2'

// ============================================================================
// Existing resource references — read outputs from Phase 1 infrastructure
// ============================================================================

resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' existing = {
  name: resourceGroupName
}

resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' existing = {
  scope: resourceGroup
  name: prodIdentityName
}

resource containerAppsEnvResource 'Microsoft.App/managedEnvironments@2025-07-01' existing = {
  scope: resourceGroup
  name: containerAppsEnvName
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  scope: resourceGroup
  name: appInsightsName
}

resource postgresServer 'Microsoft.DBforPostgreSQL/flexibleServers@2024-08-01' existing = {
  scope: resourceGroup
  name: postgresServerName
}

resource postgresDatabase 'Microsoft.DBforPostgreSQL/flexibleServers/databases@2024-08-01' existing = {
  parent: postgresServer
  name: prodDatabaseName
}

resource openAiAccount 'Microsoft.CognitiveServices/accounts@2025-06-01' existing = {
  scope: resourceGroup
  name: openAiName
}

resource openAiModelDeployment 'Microsoft.CognitiveServices/accounts/deployments@2025-06-01' existing = {
  parent: openAiAccount
  name: openAiDeploymentModelName
}

// ============================================================================
// Derived values
// ============================================================================

var keyVaultUri = 'https://${keyVaultName}${environment().suffixes.keyvaultDns}/'
var aadClientSecretSecretName = 'techhub-prod-aad-client-secret'
var dbConnectionString = 'Host=${postgresServer.properties.fullyQualifiedDomainName};Database=${postgresDatabase.name};Username=${prodIdentityName};SSL Mode=Require'
var allCustomDomains = [for entry in items(wildcardCertNames): '*.${entry.key}']
// Resolve App Insights connection string: use override when provided, otherwise read from existing resource.
// Pass '@existing' (default) to use the Azure resource value; pass '' to disable telemetry for PR environments.
var effectiveAppInsightsConnStr = appInsightsConnectionString == '@existing' ? appInsights.properties.ConnectionString : appInsightsConnectionString

// ============================================================================
// Wildcard certificates
// ============================================================================

// Reference existing wildcard certificates via `existing` — do NOT re-deploy them.
//
// Background: re-deploying Microsoft.App/managedEnvironments/certificates via Bicep on every
// CD run triggers an idempotent ARM PUT on the Azure Container Apps cert provider. When the
// cert already exists the provider starts an async re-import operation that never signals
// completion, causing ARM to poll indefinitely and the deployment to hang.
//
// Certificates are created once (initial environment setup) and renewed via
// infra/modules/wildcardCertificates.bicep run separately after cert rotation.
// See docs/wildcard-certificates.md for the renewal process.
var certEntries = items(wildcardCertNames)
resource existingCerts 'Microsoft.App/managedEnvironments/certificates@2025-07-01' existing = [for entry in certEntries: {
  parent: containerAppsEnvResource
  name: 'wildcard-${replace(entry.key, '.', '-')}'
}]

// Build cert ID lookup: { 'hub.ms': '/subscriptions/.../certificates/wildcard-hub-ms', ... }
var wildcardCertIdPairs = [for (entry, i) in certEntries: {
  key: entry.key
  value: existingCerts[i].id
}]
var wildcardCertIds = !empty(certEntries) ? toObject(wildcardCertIdPairs, item => item.key, item => item.value) : {}

// ============================================================================
// Container Apps
// ============================================================================

// API Container App
module apiApp './modules/api.bicep' = {
  scope: resourceGroup
  name: 'api-${deploymentSuffix}'
  params: {
    location: location
    containerAppName: apiAppName
    containerAppsEnvironmentId: containerAppsEnvResource.id
    githubRegistryUsername: githubRegistryUsername
    githubRegistryAuthUsername: githubRegistryAuthUsername
    identityId: managedIdentity.id
    identityClientId: managedIdentity.properties.clientId
    imageTag: apiImageTag
    appInsightsConnectionString: effectiveAppInsightsConnStr
    keyVaultUri: keyVaultUri
    dbConnectionString: dbConnectionString
    webFqdns: !empty(primaryHosts) ? primaryHosts : ['${webAppName}.${containerAppsEnvResource.properties.defaultDomain}']
    azureAdTenantId: azureAdTenantId
    azureAdClientId: azureAdClientId
    aiCategorizationEndpoint: openAiAccount.properties.endpoint
    aiCategorizationDeploymentName: openAiModelDeployment.name
    acsEndpointSecretName: acsEndpointSecretName
    acsSenderAddressSecretName: 'techhub-prod-acs-sender-address'
    newsletterUnsubscribeSecretName: 'techhub-prod-newsletter-unsubscribe-secret'
    tags: prodTags
  }
}

// Web Container App
module webApp './modules/web.bicep' = {
  scope: resourceGroup
  name: 'web-${deploymentSuffix}'
  params: {
    location: location
    containerAppName: webAppName
    containerAppsEnvironmentId: containerAppsEnvResource.id
    githubRegistryUsername: githubRegistryUsername
    githubRegistryAuthUsername: githubRegistryAuthUsername
    identityId: managedIdentity.id
    imageTag: webImageTag
    apiBaseUrl: apiApp.outputs.fqdn
    appInsightsConnectionString: effectiveAppInsightsConnStr
    customDomains: allCustomDomains
    primaryHosts: primaryHosts
    wildcardCertificateIds: wildcardCertIds
    keyVaultUri: keyVaultUri
    aadClientSecretSecretName: aadClientSecretSecretName
    azureAdTenantId: azureAdTenantId
    azureAdClientId: azureAdClientId
    googleAnalyticsMeasurementId: googleAnalyticsMeasurementId
    tags: prodTags
  }
}

// Outputs
output apiUrl string = 'https://${apiApp.outputs.fqdn}'
output webUrl string = 'https://${webApp.outputs.fqdn}'
