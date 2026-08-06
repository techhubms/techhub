param location string
param siteName string
param appServicePlanId string

@description('Subnet resource ID (delegated to Microsoft.Web/serverFarms) used for Regional VNet Integration — required to reach Key Vault/PostgreSQL/AI Foundry private endpoints')
param vnetIntegrationSubnetId string

@description('Subnet resource ID that is allowed to call this app (the Web app\'s VNet integration subnet). All other inbound traffic is denied — this keeps the API non-publicly-reachable, listening only for traffic routed from the Web app over the VNet.')
param allowedCallerSubnetId string

@description('GitHub Container Registry organization/namespace for image names (e.g. techhubms → ghcr.io/techhubms/...)')
param githubRegistryUsername string

@description('GitHub username of the PAT owner used to authenticate with ghcr.io. Must match the account that created the PAT stored in Key Vault.')
param githubRegistryAuthUsername string = githubRegistryUsername

@description('User-assigned managed identity resource ID (used to access Key Vault secrets)')
param identityId string

@description('Client ID of the user-assigned managed identity. Required for DefaultAzureCredential to select the correct identity when multiple are available.')
param identityClientId string

param imageTag string
param appInsightsConnectionString string

@description('FQDNs for the web frontend (used for CORS and BaseUrl configuration)')
param webFqdns string[] = []

@description('Key Vault URI (e.g. https://kv-techhub-prod.vault.azure.net/) — used to resolve KV secret references')
param keyVaultUri string

@description('Full PostgreSQL connection string (passwordless — app uses managed identity token)')
param dbConnectionString string

@description('Azure AD tenant ID (not a secret — public Entra identifier)')
param azureAdTenantId string = ''

@description('Azure AD client ID (not a secret — public Entra identifier)')
param azureAdClientId string = ''

@description('Azure AI Foundry endpoint URL')
param aiCategorizationEndpoint string = ''

@description('Azure AI Foundry deployment name')
param aiCategorizationDeploymentName string = ''

@description('Key Vault secret name for Newsletter ACS endpoint URL. Leave empty to disable.')
param acsEndpointSecretName string = ''

@description('Key Vault secret name for Newsletter sender address. Leave empty to disable.')
param acsSenderAddressSecretName string = ''

@description('Key Vault secret name for Newsletter__UnsubscribeSecret. Leave empty to disable Key Vault binding.')
param newsletterUnsubscribeSecretName string = ''

@description('ASPNETCORE_ENVIRONMENT value. Use "Staging" for PR preview environments.')
param aspNetCoreEnvironment string = 'Production'

@description('Tags applied to the Web App')
param tags object = {}

var imageReference = 'ghcr.io/${githubRegistryUsername}/techhub-api:${imageTag}'
var hasAcsEndpoint = !empty(acsEndpointSecretName)
var hasAcsSenderAddress = !empty(acsSenderAddressSecretName)
var newsletterWebsiteBaseUrl = !empty(webFqdns) ? 'https://${webFqdns[0]}' : 'https://${siteName}.azurewebsites.net'
var customOrigins = [for fqdn in webFqdns: 'https://${fqdn}']
var corsOrigins = union(['https://*.azurewebsites.net'], customOrigins)
var corsEnvVars = [for (fqdn, i) in webFqdns: {
  name: 'Cors__AllowedOrigins__${i}'
  value: 'https://${fqdn}'
}]

func kvRef(vaultUri string, secretName string) string => '@Microsoft.KeyVault(SecretUri=${vaultUri}secrets/${secretName})'

var newsletterSecretEnvVars = empty(newsletterUnsubscribeSecretName)
  ? []
  : [
      {
        name: 'Newsletter__UnsubscribeSecret'
        value: kvRef(keyVaultUri, newsletterUnsubscribeSecretName)
      }
    ]
var newsletterAcsEnvVars = hasAcsEndpoint
  ? [
      {
        name: 'Newsletter__Endpoint'
        value: kvRef(keyVaultUri, acsEndpointSecretName)
      }
    ]
  : []
var newsletterSenderEnvVars = hasAcsSenderAddress
  ? [
      {
        name: 'Newsletter__SenderAddress'
        value: kvRef(keyVaultUri, acsSenderAddressSecretName)
      }
    ]
  : []
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
    value: 'techhub-api'
  }
  {
    // Required for DefaultAzureCredential / ManagedIdentityCredential to select the correct
    // user-assigned managed identity. Without this, IMDS returns HTTP 400 when multiple
    // identities exist or when the managed identity endpoint requires an explicit client_id.
    name: 'AZURE_CLIENT_ID'
    value: identityClientId
  }
  {
    name: 'Database__Provider'
    value: 'PostgreSQL'
  }
  {
    name: 'Database__ConnectionString'
    value: dbConnectionString
  }
  {
    name: 'Database__UseEntraAuth'
    value: 'true'
  }
  {
    name: 'AppSettings__BaseUrl'
    value: newsletterWebsiteBaseUrl
  }
  {
    name: 'Newsletter__WebsiteBaseUrl'
    value: newsletterWebsiteBaseUrl
  }
  {
    name: 'TECHHUB_TMP'
    value: '/tmp/techhub'
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
    name: 'AiCategorization__Endpoint'
    value: aiCategorizationEndpoint
  }
  {
    name: 'AiCategorization__DeploymentName'
    value: aiCategorizationDeploymentName
  }
  {
    // Container listens on 8080 — tells the App Service container host which port to route to.
    name: 'WEBSITES_PORT'
    value: '8080'
  }
  {
    // Containers have no need for the default persistent /home file share.
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
    // GitHub Container Registry PAT for image pulls (read:packages scope), resolved via
    // Key Vault reference using the user-assigned managed identity (keyVaultReferenceIdentity below).
    name: 'DOCKER_REGISTRY_SERVER_PASSWORD'
    value: kvRef(keyVaultUri, 'techhub-github-registry-token')
  }
]

resource api 'Microsoft.Web/sites@2023-12-01' = {
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
    // Resolve Key Vault references (@Microsoft.KeyVault(...) app settings above) using the
    // user-assigned identity instead of a system-assigned one.
    keyVaultReferenceIdentity: identityId
    // Route all outbound traffic (including Key Vault reference resolution) through the
    // VNet integration subnet so it reaches Key Vault/PostgreSQL/AI Foundry over their
    // private endpoints — keeps all backing-service traffic on the VNet.
    vnetRouteAllEnabled: true
    siteConfig: {
      linuxFxVersion: 'DOCKER|${imageReference}'
      alwaysOn: true
      healthCheckPath: '/health'
      http20Enabled: true
      minTlsVersion: '1.2'
      ftpsState: 'Disabled'
      cors: {
        allowedOrigins: corsOrigins
        supportCredentials: false
      }
      appSettings: concat(staticEnvVars, newsletterAcsEnvVars, newsletterSenderEnvVars, newsletterSecretEnvVars, corsEnvVars)
      // Deny all public inbound traffic except from the Web app's VNet integration subnet —
      // keeps the API reachable only from the Web app's subnet, never from the public internet.
      ipSecurityRestrictions: [
        {
          vnetSubnetResourceId: allowedCallerSubnetId
          action: 'Allow'
          priority: 100
          name: 'AllowWebApp'
        }
      ]
      ipSecurityRestrictionsDefaultAction: 'Deny'
    }
  }
}

// Regional VNet Integration — gives the API outbound access into the delegated subnet.
resource vnetIntegration 'Microsoft.Web/sites/networkConfig@2023-12-01' = {
  parent: api
  name: 'virtualNetwork'
  properties: {
    subnetResourceId: vnetIntegrationSubnetId
    swiftSupported: true
  }
}

output fqdn string = api.properties.defaultHostName
output id string = api.id
output principalId string = api.identity.principalId
