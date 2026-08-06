targetScope = 'subscription'

// Phase 2: App Service site deployments for Tech Hub production.
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

@description('App Service Plan name (existing resource, Basic B1, hosts both API and Web sites)')
param appServicePlanName string = 'asp-techhub-prod'

@description('API Web App name')
param apiAppName string = 'app-techhub-api-prod'

@description('Web Web App name')
param webAppName string = 'app-techhub-web-prod'

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

@description('Wildcard certificate resource names in Microsoft.Web/certificates, keyed by base domain (e.g. { "hub.ms": "wildcard-hub-ms" })')
param wildcardCertNames object = {}

@description('VNet name (existing resource)')
param vnetName string = 'vnet-techhub-prod'

@description('App Service Regional VNet Integration subnet name (existing resource)')
param appServiceSubnetName string = 'snet-app-service'

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

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' existing = {
  scope: resourceGroup
  name: appServicePlanName
}

// Regional VNet Integration subnet — shared by both API and Web sites (they're on the same
// App Service Plan, and a single subnet can serve one Plan's VNet integration).
resource appServiceSubnet 'Microsoft.Network/virtualNetworks/subnets@2025-01-01' existing = {
  scope: resourceGroup
  name: '${vnetName}/${appServiceSubnetName}'
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
//
// Wildcard domains (e.g. *.hub.ms, *.xebia.ms) are computed here from wildcardCertNames'
// keys and passed straight through to modules/web.bicep, which resolves the matching
// Microsoft.Web/certificates thumbprint via `existing` references and binds the hostname.
// Certificates are imported once via modules/wildcardCert.bicep — see docs/wildcard-certificates.md
// for the renewal process.

// ============================================================================
// App Service sites
// ============================================================================

// API Web App
module apiApp './modules/api.bicep' = {
  scope: resourceGroup
  name: 'api-${deploymentSuffix}'
  params: {
    location: location
    siteName: apiAppName
    appServicePlanId: appServicePlan.id
    vnetIntegrationSubnetId: appServiceSubnet.id
    allowedCallerSubnetId: appServiceSubnet.id
    githubRegistryUsername: githubRegistryUsername
    githubRegistryAuthUsername: githubRegistryAuthUsername
    identityId: managedIdentity.id
    identityClientId: managedIdentity.properties.clientId
    imageTag: apiImageTag
    appInsightsConnectionString: effectiveAppInsightsConnStr
    keyVaultUri: keyVaultUri
    dbConnectionString: dbConnectionString
    webFqdns: !empty(primaryHosts) ? primaryHosts : ['${webAppName}.azurewebsites.net']
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

// Web Web App
module webApp './modules/web.bicep' = {
  scope: resourceGroup
  name: 'web-${deploymentSuffix}'
  params: {
    location: location
    siteName: webAppName
    appServicePlanId: appServicePlan.id
    vnetIntegrationSubnetId: appServiceSubnet.id
    githubRegistryUsername: githubRegistryUsername
    githubRegistryAuthUsername: githubRegistryAuthUsername
    identityId: managedIdentity.id
    imageTag: webImageTag
    apiBaseUrl: apiApp.outputs.fqdn
    appInsightsConnectionString: effectiveAppInsightsConnStr
    customDomains: allCustomDomains
    primaryHosts: primaryHosts
    wildcardCertNames: wildcardCertNames
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
