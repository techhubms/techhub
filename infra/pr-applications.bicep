targetScope = 'resourceGroup'

// PR preview App Service deployment.
// Deploys API and Web sites (Web App for Containers) for a single pull-request preview
// environment onto the shared, dedicated PR-preview App Service Plan (asp-techhub-pr) —
// kept separate from the production Plan so idle preview apps can never affect prod.
// Shares api.bicep and web.bicep with applications.bicep (production) so any App Service
// configuration change is automatically reflected in both environments.
//
// Called by scripts/Deploy-PrPreview.ps1 via `az deployment group create`.

@description('Pull request number — used to derive resource names.')
param prNumber int

@description('Docker image tag for both API and Web.')
param imageTag string

@description('Azure region for the App Service sites.')
param location string = 'swedencentral'

@description('PR-preview App Service Plan name (shared by all PR preview sites, created once by infrastructure.bicep).')
param appServicePlanPrName string = 'asp-techhub-pr'

@description('VNet name (shared with production).')
param vnetName string = 'vnet-techhub-prod'

@description('PR-preview App Service Regional VNet Integration subnet name (shared by all PR preview sites — see modules/network.bicep).')
param appServicePrSubnetName string = 'snet-app-service-pr'

@description('Key Vault name (shared with production — holds the GitHub registry token).')
param keyVaultName string = 'kv-techhub-prod'

@description('GitHub Container Registry organization/namespace for image names.')
param githubRegistryUsername string = 'techhubms'

@description('GitHub username of the PAT owner used to authenticate with ghcr.io.')
param githubRegistryAuthUsername string = githubRegistryUsername

@description('UTC timestamp used to make nested deployment names unique per run.')
param deploymentTimestamp string = utcNow()

@description('Tags applied to the PR preview sites')
param tags object = {
  owner: 'techhub-maintainer'
  project: 'techhub'
  managedBy: 'bicep'
  env: 'pr'
}

// Include prNumber so two PRs deployed in the same second get distinct nested deployment names.
var deploymentSuffix = uniqueString(deploymentTimestamp, string(prNumber))

var apiAppName = 'app-techhub-api-pr-${prNumber}'
var webAppName = 'app-techhub-web-pr-${prNumber}'
var keyVaultUri = 'https://${keyVaultName}${environment().suffixes.keyvaultDns}/'
// Shared PR identity — created once by infrastructure.bicep, reused by all PR environments.
var prIdentityName = 'id-techhub-pr'

// ============================================================================
// Existing resource references
// ============================================================================

resource appServicePlanPr 'Microsoft.Web/serverfarms@2023-12-01' existing = {
  name: appServicePlanPrName
}

// Regional VNet Integration subnet — shared by both API and Web sites (they're on the same
// PR-preview App Service Plan, and a single subnet can serve one Plan's VNet integration).
resource appServicePrSubnet 'Microsoft.Network/virtualNetworks/subnets@2025-01-01' existing = {
  name: '${vnetName}/${appServicePrSubnetName}'
}

resource prManagedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' existing = {
  name: prIdentityName
}

// ============================================================================
// Derived values
// ============================================================================

// Web App default hostnames are deterministic (<name>.azurewebsites.net, global — not tied to
// any environment/region domain), so the expected Web FQDN can be computed directly, without a
// Container-Apps-style environment default-domain lookup.
var webExpectedFqdn = '${webAppName}.azurewebsites.net'

// Passwordless PostgreSQL connection string for the PR database.
// The username is the managed identity display name — registered as Entra admin
// on the PR server by Deploy-PrPreview.ps1 before calling this template.
var prPostgresServerName = 'psql-techhub-pr-${prNumber}'
var dbConnectionString = 'Host=${prPostgresServerName}.postgres.database.azure.com;Database=techhub;Username=${prIdentityName};SSL Mode=Require'

// ============================================================================
// App Service sites
// ============================================================================

module apiApp './modules/api.bicep' = {
  name: 'pr-api-${deploymentSuffix}'
  params: {
    location: location
    siteName: apiAppName
    appServicePlanId: appServicePlanPr.id
    vnetIntegrationSubnetId: appServicePrSubnet.id
    // API and Web share the same PR-preview subnet (they're on the same Plan) — allow
    // inbound only from that subnet, matching production's non-public API pattern.
    allowedCallerSubnetId: appServicePrSubnet.id
    githubRegistryUsername: githubRegistryUsername
    githubRegistryAuthUsername: githubRegistryAuthUsername
    identityId: prManagedIdentity.id
    identityClientId: prManagedIdentity.properties.clientId
    imageTag: imageTag
    appInsightsConnectionString: '' // disabled — PR telemetry must not reach production dashboards
    keyVaultUri: keyVaultUri
    dbConnectionString: dbConnectionString
    webFqdns: [webExpectedFqdn]
    azureAdTenantId: '' // AAD admin dashboard not needed for PR environments
    azureAdClientId: ''
    aiCategorizationEndpoint: '' // AI categorization disabled (background jobs are off)
    aiCategorizationDeploymentName: ''
    aspNetCoreEnvironment: 'Staging'
    tags: tags
  }
}

module webApp './modules/web.bicep' = {
  name: 'pr-web-${deploymentSuffix}'
  params: {
    location: location
    siteName: webAppName
    appServicePlanId: appServicePlanPr.id
    vnetIntegrationSubnetId: appServicePrSubnet.id
    githubRegistryUsername: githubRegistryUsername
    githubRegistryAuthUsername: githubRegistryAuthUsername
    identityId: prManagedIdentity.id
    imageTag: imageTag
    apiBaseUrl: apiApp.outputs.fqdn
    appInsightsConnectionString: '' // disabled — PR telemetry must not reach production dashboards
    googleAnalyticsMeasurementId: '' // disabled — PR traffic must not appear in GA reports
    keyVaultUri: keyVaultUri
    // aadClientSecretSecretName uses its default — value is irrelevant since AAD is disabled
    azureAdTenantId: '' // AAD admin dashboard not needed for PR environments
    azureAdClientId: ''
    // No custom domains / wildcard certs / primary hosts for PR (uses azurewebsites.net default)
    aspNetCoreEnvironment: 'Staging'
    tags: tags
  }
}

// ============================================================================
// Outputs
// ============================================================================

output apiUrl string = 'https://${apiApp.outputs.fqdn}'
output webUrl string = 'https://${webApp.outputs.fqdn}'
output webFqdn string = webApp.outputs.fqdn
