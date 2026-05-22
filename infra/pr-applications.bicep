targetScope = 'resourceGroup'

// PR preview Container Apps deployment.
// Deploys API and Web Container Apps for a single pull-request preview environment.
// Shares api.bicep and web.bicep with applications.bicep (production) so any Container App
// configuration change is automatically reflected in both environments.
//
// Called by scripts/Deploy-PrPreview.ps1 via `az deployment group create`.

@description('Pull request number — used to derive resource names.')
param prNumber int

@description('Docker image tag for both API and Web.')
param imageTag string

@description('Azure region for the Container Apps.')
param location string = 'swedencentral'

@description('Container Apps Environment name (shared with production).')
param containerAppsEnvName string = 'cae-techhub-prod'

@description('Key Vault name (shared with production — holds the GitHub registry token).')
param keyVaultName string = 'kv-techhub-prod'

@description('Per-PR managed identity name (must already exist before this deployment).')
param prIdentityName string

@description('GitHub Container Registry organization/namespace for image names.')
param githubRegistryUsername string = 'techhubms'

@description('GitHub username of the PAT owner used to authenticate with ghcr.io.')
param githubRegistryAuthUsername string = githubRegistryUsername

@description('UTC timestamp used to make nested deployment names unique per run.')
param deploymentTimestamp string = utcNow()

var deploymentSuffix = uniqueString(deploymentTimestamp)

var apiAppName = 'ca-techhub-api-pr-${prNumber}'
var webAppName = 'ca-techhub-web-pr-${prNumber}'
var keyVaultUri = 'https://${keyVaultName}${environment().suffixes.keyvaultDns}/'

// ============================================================================
// Existing resource references
// ============================================================================

resource containerAppsEnv 'Microsoft.App/managedEnvironments@2025-07-01' existing = {
  name: containerAppsEnvName
}

resource prManagedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' existing = {
  name: prIdentityName
}

// ============================================================================
// Derived values
// ============================================================================

// Compute expected Web FQDN so it can be passed to the API for CORS + BaseUrl.
// The FQDN is deterministic: <appName>.<cae-default-domain>
var webExpectedFqdn = '${webAppName}.${containerAppsEnv.properties.defaultDomain}'

// Passwordless PostgreSQL connection string for the PR database.
// The username is the managed identity display name — registered as Entra admin
// on the PR server by Deploy-PrPreview.ps1 before calling this template.
var prPostgresServerName = 'psql-techhub-pr-${prNumber}'
var dbConnectionString = 'Host=${prPostgresServerName}.postgres.database.azure.com;Database=techhub;Username=${prIdentityName};SSL Mode=Require'

// ============================================================================
// Container Apps
// ============================================================================

module apiApp './modules/api.bicep' = {
  name: 'pr-api-${deploymentSuffix}'
  params: {
    location: location
    containerAppName: apiAppName
    containerAppsEnvironmentId: containerAppsEnv.id
    githubRegistryUsername: githubRegistryUsername
    githubRegistryAuthUsername: githubRegistryAuthUsername
    identityId: prManagedIdentity.id
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
    enableBackgroundJobs: false // prevent PR from triggering content sync / roundup jobs
    minReplicas: 0 // scale to zero when idle to minimize cost
    maxReplicas: 1
  }
  dependsOn: []
}

module webApp './modules/web.bicep' = {
  name: 'pr-web-${deploymentSuffix}'
  params: {
    location: location
    containerAppName: webAppName
    containerAppsEnvironmentId: containerAppsEnv.id
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
    // No custom domains / wildcard certs / primary hosts for PR (uses CAE default domain)
    aspNetCoreEnvironment: 'Staging'
    minReplicas: 0 // scale to zero when idle to minimize cost
    maxReplicas: 1
  }
}

// ============================================================================
// Outputs
// ============================================================================

output apiUrl string = 'https://${apiApp.outputs.fqdn}'
output webUrl string = 'https://${webApp.outputs.fqdn}'
output webFqdn string = webApp.outputs.fqdn
