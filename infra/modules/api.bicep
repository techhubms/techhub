param location string
param containerAppName string
param containerAppsEnvironmentId string

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

@description('When false, disables scheduled background jobs (ContentProcessor, RoundupGenerator). Set to false for PR preview environments.')
param enableBackgroundJobs bool = true

@description('Minimum replica count. Use 0 to enable scale-to-zero for PR preview environments.')
param minReplicas int = 1

@description('Maximum replica count.')
param maxReplicas int = 3

@description('Tags applied to the Container App')
param tags object = {}

var imageReference = 'ghcr.io/${githubRegistryUsername}/techhub-api:${imageTag}'
var revisionSuffix = 'api-${imageTag}'
var hasAcsEndpoint = !empty(acsEndpointSecretName)
var hasAcsSenderAddress = !empty(acsSenderAddressSecretName)
var newsletterWebsiteBaseUrl = !empty(webFqdns) ? 'https://${webFqdns[0]}' : 'https://${containerAppName}.azurecontainerapps.io'
var customOrigins = [for fqdn in webFqdns: 'https://${fqdn}']
var corsOrigins = union(['https://*.azurecontainerapps.io'], customOrigins)
var corsEnvVars = [for (fqdn, i) in webFqdns: {
  name: 'Cors__AllowedOrigins__${i}'
  value: 'https://${fqdn}'
}]
var backgroundJobEnvVars = enableBackgroundJobs ? [] : [
  {
    name: 'ContentProcessor__Enabled'
    value: 'false'
  }
  {
    name: 'RoundupGenerator__Enabled'
    value: 'false'
  }
]
var newsletterSecretEnvVars = empty(newsletterUnsubscribeSecretName)
  ? []
  : [
      {
        name: 'Newsletter__UnsubscribeSecret'
        secretRef: 'newsletter-unsubscribe-secret'
      }
    ]
var newsletterAcsEnvVars = hasAcsEndpoint
  ? [
      {
        name: 'Newsletter__Endpoint'
        secretRef: 'newsletter-acs-endpoint'
      }
    ]
  : []
var newsletterSenderEnvVars = hasAcsSenderAddress
  ? [
      {
        name: 'Newsletter__SenderAddress'
        secretRef: 'acs-sender-address'
      }
    ]
  : []
var newsletterSecrets = empty(newsletterUnsubscribeSecretName)
  ? []
  : [
      {
        name: 'newsletter-unsubscribe-secret'
        keyVaultUrl: '${keyVaultUri}secrets/${newsletterUnsubscribeSecretName}'
        identity: identityId
      }
    ]

var newsletterAcsSecrets = hasAcsEndpoint
  ? [
      {
        name: 'newsletter-acs-endpoint'
        keyVaultUrl: '${keyVaultUri}secrets/${acsEndpointSecretName}'
        identity: identityId
      }
    ]
  : []

var newsletterSenderSecrets = hasAcsSenderAddress
  ? [
      {
        name: 'acs-sender-address'
        keyVaultUrl: '${keyVaultUri}secrets/${acsSenderAddressSecretName}'
        identity: identityId
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
]

resource api 'Microsoft.App/containerApps@2025-07-01' = {
  name: containerAppName
  location: location
  tags: tags
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${identityId}': {}
    }
  }
  properties: {
    managedEnvironmentId: containerAppsEnvironmentId
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: false
        allowInsecure: false
        targetPort: 8080
        transport: 'http'
        corsPolicy: {
          allowedOrigins: corsOrigins
          allowedMethods: ['GET', 'POST', 'PUT', 'DELETE', 'OPTIONS']
          allowedHeaders: ['*']
          allowCredentials: false
        }
      }
      registries: [
        {
          // Pull images from GitHub Container Registry using a PAT stored in Key Vault.
          // username must match the PAT owner (githubRegistryAuthUsername), which may differ
          // from the image namespace (githubRegistryUsername / the org).
          server: 'ghcr.io'
          username: githubRegistryAuthUsername
          passwordSecretRef: 'ghcr-token'
        }
      ]
      secrets: concat([
        // GitHub Container Registry PAT for image pulls (read:packages scope)
        {
          name: 'ghcr-token'
          keyVaultUrl: '${keyVaultUri}secrets/techhub-github-registry-token'
          identity: identityId
        }
      ], newsletterSenderSecrets, newsletterAcsSecrets, newsletterSecrets)
    }
    template: {
      revisionSuffix: revisionSuffix
      containers: [
        {
          name: 'api'
          image: imageReference
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          env: concat(staticEnvVars, newsletterAcsEnvVars, newsletterSenderEnvVars, newsletterSecretEnvVars, corsEnvVars, backgroundJobEnvVars)
          probes: [
            {
              type: 'startup'
              httpGet: {
                path: '/alive'
                port: 8080
                scheme: 'HTTP'
              }
              initialDelaySeconds: 5
              periodSeconds: 10
              failureThreshold: 30  // 5s + 30×10s = 305s max startup (DB migrations + content sync)
              timeoutSeconds: 5
            }
            {
              type: 'liveness'
              httpGet: {
                path: '/alive'
                port: 8080
                scheme: 'HTTP'
              }
              periodSeconds: 30
              failureThreshold: 3
              timeoutSeconds: 5
            }
            {
              type: 'readiness'
              httpGet: {
                path: '/health'
                port: 8080
                scheme: 'HTTP'
              }
              periodSeconds: 10
              failureThreshold: 3
              timeoutSeconds: 5
            }
          ]
        }
      ]
      scale: {
        minReplicas: minReplicas
        maxReplicas: maxReplicas
        cooldownPeriod: 300
        pollingInterval: 30
        rules: [
          {
            name: 'http-scaling'
            http: {
              metadata: {
                concurrentRequests: '100'
              }
            }
          }
        ]
      }
    }
  }
}

output fqdn string = api.properties.configuration.ingress.fqdn
output id string = api.id
