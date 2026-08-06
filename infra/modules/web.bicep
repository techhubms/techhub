param location string
param siteName string
param appServicePlanId string

@description('Subnet resource ID (delegated to Microsoft.Web/serverFarms) used for Regional VNet Integration — required to reach Key Vault private endpoint and to call the API app')
param vnetIntegrationSubnetId string

@description('GitHub Container Registry organization/namespace for image names (e.g. techhubms → ghcr.io/techhubms/...)')
param githubRegistryUsername string

@description('GitHub username of the PAT owner used to authenticate with ghcr.io. Must match the account that created the PAT stored in Key Vault.')
param githubRegistryAuthUsername string = githubRegistryUsername

@description('User-assigned managed identity resource ID (used to access Key Vault secrets)')
param identityId string

param imageTag string
param apiBaseUrl string
param appInsightsConnectionString string

@description('Optional custom domains (e.g. ["*.hub.ms", "*.xebia.ms"]). Leave empty to skip.')
param customDomains string[] = []

@description('Primary host names for the SubdomainRedirectMiddleware configuration')
param primaryHosts string[] = []

@description('Wildcard TLS certificate resource names in Microsoft.Web/certificates, mapped by base domain (e.g. { "hub.ms": "wildcard-hub-ms" }). Certificates must already be imported from Key Vault — see modules/wildcardCert.bicep.')
param wildcardCertNames object = {}

@description('Key Vault URI (e.g. https://kv-techhub-prod.vault.azure.net/) — used to resolve KV secret references')
param keyVaultUri string

@description('Key Vault secret name holding the Azure AD client secret')
param aadClientSecretSecretName string = 'techhub-prod-aad-client-secret'

@description('Azure AD tenant ID (not a secret — public Entra identifier)')
param azureAdTenantId string = ''

@description('Azure AD client ID (not a secret — public Entra identifier)')
param azureAdClientId string = ''

@description('Google Analytics Measurement ID (e.g. G-XXXXXXXXXX). Pass empty string to disable GA telemetry (PR preview environments).')
param googleAnalyticsMeasurementId string = ''

@description('Tags applied to the Web App')
param tags object = {}

@description('ASPNETCORE_ENVIRONMENT value. Use "Staging" for PR preview environments.')
param aspNetCoreEnvironment string = 'Production'

var imageReference = 'ghcr.io/${githubRegistryUsername}/techhub-web:${imageTag}'

func kvRef(vaultUri string, secretName string) string => '@Microsoft.KeyVault(SecretUri=${vaultUri}secrets/${secretName})'

// Resolve wildcard certificate thumbprints (already imported into Microsoft.Web/certificates
// from Key Vault by modules/wildcardCert.bicep), keyed by base domain so each custom domain
// binding below can look up its matching cert regardless of array ordering.
var certEntries = items(wildcardCertNames)
resource wildcardCerts 'Microsoft.Web/certificates@2023-12-01' existing = [for entry in certEntries: {
  name: entry.value
}]
var certThumbprintPairs = [for (entry, i) in certEntries: {
  key: entry.key
  value: i
}]
var certIndexByDomain = !empty(certEntries) ? toObject(certThumbprintPairs, item => item.key, item => item.value) : {}

// Environment variables: static config + dynamic shortcuts/primary hosts from Bicep params
var staticEnvVars = [
  {
    name: 'ASPNETCORE_ENVIRONMENT'
    value: aspNetCoreEnvironment
  }
  {
    name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
    value: appInsightsConnectionString
  }
  {
    name: 'OTEL_SERVICE_NAME'
    value: 'techhub-web'
  }
  {
    name: 'ApiBaseUrl'
    value: 'https://${apiBaseUrl}'
  }
  {
    name: 'TECHHUB_TMP'
    value: '/tmp/techhub'
  }
  {
    name: 'DEPLOY_IMAGE_TAG'
    value: imageTag
  }
  {
    name: 'AzureAd__TenantId'
    value: azureAdTenantId
  }
  {
    name: 'AzureAd__ClientId'
    value: azureAdClientId
  }
  {
    name: 'AzureAd__Scopes'
    value: empty(azureAdClientId) ? '' : 'api://${azureAdClientId}/Admin.Access'
  }
  {
    name: 'GoogleAnalytics__MeasurementId'
    value: googleAnalyticsMeasurementId
  }
]
// AzureAd__ClientSecret is only needed when AAD is enabled (azureAdClientId is set).
// When AAD is disabled the KV reference is omitted entirely — the revision would crash-loop
// if the secret entry existed in the secrets list but the KV secret did not.
var aadSecretEnvVars = empty(azureAdClientId)
  ? []
  : [
      {
        name: 'AzureAd__ClientSecret'
        value: kvRef(keyVaultUri, aadClientSecretSecretName)
      }
    ]
var primaryHostEnvVars = [for (host, i) in primaryHosts: {
  name: 'PrimaryHosts__${i}'
  value: host
}]
var allEnvVars = concat(staticEnvVars, aadSecretEnvVars, primaryHostEnvVars, [
  {
    name: 'WEBSITES_PORT'
    value: '8080'
  }
  {
    name: 'WEBSITES_ENABLE_APP_SERVICE_STORAGE'
    value: 'false'
  }
  {
    name: 'DOCKER_REGISTRY_SERVER_URL'
    value: 'https://ghcr.io'
  }
  {
    name: 'DOCKER_REGISTRY_SERVER_USERNAME'
    value: githubRegistryAuthUsername
  }
  {
    name: 'DOCKER_REGISTRY_SERVER_PASSWORD'
    value: kvRef(keyVaultUri, 'techhub-github-registry-token')
  }
])

resource web 'Microsoft.Web/sites@2023-12-01' = {
  name: siteName
  location: location
  tags: tags
  kind: 'app,linux,container'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${identityId}': {}
    }
  }
  properties: {
    serverFarmId: appServicePlanId
    httpsOnly: true
    // Blazor InteractiveServer keeps a persistent SignalR circuit per browser tab — client
    // affinity (ARR cookie) keeps a client pinned to the same instance, mirroring Container
    // Apps' `stickySessions.affinity: sticky`. Matters most if instance count > 1 later.
    clientAffinityEnabled: true
    keyVaultReferenceIdentity: identityId
    vnetRouteAllEnabled: true
    siteConfig: {
      linuxFxVersion: 'DOCKER|${imageReference}'
      alwaysOn: true
      healthCheckPath: '/health'
      http20Enabled: true
      minTlsVersion: '1.2'
      ftpsState: 'Disabled'
      // WebSockets are required for Blazor InteractiveServer's SignalR circuit; supported on
      // Linux App Service from the Basic tier up.
      webSocketsEnabled: true
      appSettings: allEnvVars
    }
  }
}

// Regional VNet Integration — gives Web outbound access into the delegated subnet so it can
// reach Key Vault (secret resolution) and call the API app (which only accepts inbound
// traffic from this subnet — see modules/api.bicep's ipSecurityRestrictions).
resource vnetIntegration 'Microsoft.Web/sites/networkConfig@2023-12-01' = {
  parent: web
  name: 'virtualNetwork'
  properties: {
    subnetResourceId: vnetIntegrationSubnetId
    swiftSupported: true
  }
}

// Wildcard custom domain bindings (e.g. *.hub.ms, *.xebia.ms), SNI-bound to the matching
// certificate imported from Key Vault. The domain's DNS (CNAME/TXT verification) must already
// point at this app's default hostname before these bindings will succeed.
resource hostNameBindings 'Microsoft.Web/sites/hostNameBindings@2023-12-01' = [for domain in customDomains: {
  parent: web
  name: domain
  properties: {
    sslState: 'SniEnabled'
    thumbprint: wildcardCerts[certIndexByDomain[substring(domain, indexOf(domain, '.') + 1)]].properties.thumbprint
    hostNameType: 'Verified'
  }
}]

output fqdn string = web.properties.defaultHostName
output id string = web.id
output principalId string = web.identity.principalId
