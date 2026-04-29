param location string
param containerAppName string
param containerAppsEnvironmentId string
param containerRegistryName string
param acrPullIdentityId string
param imageTag string
param appInsightsConnectionString string

@description('Environment name (staging, prod) - maps to ASPNETCORE_ENVIRONMENT')
@allowed(['staging', 'prod'])
param environmentName string

var aspnetEnvironment = environmentName == 'prod' ? 'Production' : 'Staging'

@description('FQDNs for the web frontend (used for CORS and BaseUrl configuration)')
param webFqdns string[] = []

@description('Key Vault URI (e.g. https://kv-techhub-shared.vault.azure.net/) — used to resolve KV secret references')
param keyVaultUri string

@description('Key Vault secret name holding the full PostgreSQL connection string')
param dbConnectionSecretName string

@description('Key Vault secret name holding the AI Foundry API key')
param aiApiKeySecretName string

@description('Key Vault secret name holding semicolon-delimited YouTube cookies (optional — empty secret is fine)')
param youtubeCookiesSecretName string = ''

@description('Azure AD tenant ID (not a secret — public Entra identifier)')
param azureAdTenantId string = ''

@description('Azure AD client ID (not a secret — public Entra identifier)')
param azureAdClientId string = ''

@description('Azure AI Foundry endpoint URL')
param aiCategorizationEndpoint string = ''

@description('Azure AI Foundry deployment name')
param aiCategorizationDeploymentName string = ''

@description('Tags applied to the Container App')
param tags object = {}

var imageReference = '${containerRegistryName}.azurecr.io/techhub-api:${imageTag}'
var revisionSuffix = 'api-${imageTag}'
var customOrigins = [for fqdn in webFqdns: 'https://${fqdn}']
var corsOrigins = union(['https://*.azurecontainerapps.io'], customOrigins)
var corsEnvVars = [for (fqdn, i) in webFqdns: {
  name: 'Cors__AllowedOrigins__${i}'
  value: 'https://${fqdn}'
}]
var staticEnvVars = [
  {
    name: 'ASPNETCORE_ENVIRONMENT'
    value: aspnetEnvironment
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
    name: 'Database__Provider'
    value: 'PostgreSQL'
  }
  {
    name: 'Database__ConnectionString'
    secretRef: 'db-connection-string'
  }
  {
    name: 'AppSettings__Content__CollectionsPath'
    value: '/app/collections'
  }
  {
    name: 'AppSettings__BaseUrl'
    value: !empty(webFqdns) ? 'https://${webFqdns[0]}' : 'https://${containerAppName}.azurecontainerapps.io'
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
    name: 'AiCategorization__ApiKey'
    secretRef: 'ai-categorization-api-key'
  }
  ...(!empty(youtubeCookiesSecretName) ? [{
    name: 'ContentProcessor__YouTubeCookies'
    secretRef: 'youtube-cookies'
  }] : [])
]

resource api 'Microsoft.App/containerApps@2025-07-01' = {
  name: containerAppName
  location: location
  tags: tags
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${acrPullIdentityId}': {}
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
          server: '${containerRegistryName}.azurecr.io'
          identity: acrPullIdentityId
        }
      ]
      secrets: [
        // Container App references Key Vault secrets at revision start via the managed identity.
        // Rotate secrets by updating Key Vault + restarting the revision — no redeploy required.
        {
          name: 'db-connection-string'
          keyVaultUrl: '${keyVaultUri}secrets/${dbConnectionSecretName}'
          identity: acrPullIdentityId
        }
        {
          name: 'ai-categorization-api-key'
          keyVaultUrl: '${keyVaultUri}secrets/${aiApiKeySecretName}'
          identity: acrPullIdentityId
        }
        ...(!empty(youtubeCookiesSecretName) ? [{
          name: 'youtube-cookies'
          keyVaultUrl: '${keyVaultUri}secrets/${youtubeCookiesSecretName}'
          identity: acrPullIdentityId
        }] : [])
      ]
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
          env: concat(staticEnvVars, corsEnvVars)
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
        minReplicas: environmentName == 'staging' ? 0 : 1
        maxReplicas: environmentName == 'staging' ? 2 : 3
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
