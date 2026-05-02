param location string
param containerAppName string
param containerAppsEnvironmentId string
param containerRegistryName string
param acrPullIdentityId string
param imageTag string
param apiBaseUrl string
param appInsightsConnectionString string

@description('Environment name (staging, prod) - maps to ASPNETCORE_ENVIRONMENT')
@allowed(['staging', 'prod'])
param environmentName string

var aspnetEnvironment = environmentName == 'prod' ? 'Production' : 'Staging'

@description('Optional custom domains (e.g. ["tech.hub.ms", "tech.xebia.ms"]). Leave empty to skip.')
param customDomains string[] = []

@description('Primary host names for the SubdomainRedirectMiddleware configuration')
param primaryHosts string[] = []

@description('Key Vault certificate resource IDs for wildcard TLS (mapped by base domain, e.g. { "hub.ms": "cert-resource-id" }). When provided, domains use SniEnabled binding with these certs instead of managed certificates.')
param wildcardCertificateIds object = {}

@description('Key Vault URI (e.g. https://kv-techhub-shared.vault.azure.net/) — used to resolve KV secret references')
param keyVaultUri string

@description('Key Vault secret name holding the Azure AD client secret')
param aadClientSecretSecretName string

@description('Azure AD tenant ID (not a secret — public Entra identifier)')
param azureAdTenantId string = ''

@description('Azure AD client ID (not a secret — public Entra identifier)')
param azureAdClientId string = ''

@description('Tags applied to the Container App')
param tags object = {}

var imageReference = '${containerRegistryName}.azurecr.io/techhub-web:${imageTag}'
var revisionSuffix = 'web-${imageTag}'

// Environment variables: static config + dynamic shortcuts/primary hosts from Bicep params
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
]
// AzureAd__ClientSecret is only needed when AAD is enabled (azureAdClientId is set).
// When AAD is disabled the KV reference is omitted entirely — the revision would crash-loop
// if the secret entry existed in the secrets list but the KV secret did not.
var aadSecretEnvVars = empty(azureAdClientId)
  ? []
  : [
      {
        name: 'AzureAd__ClientSecret'
        secretRef: 'azure-ad-client-secret'
      }
    ]
var primaryHostEnvVars = [for (host, i) in primaryHosts: {
  name: 'PrimaryHosts__${i}'
  value: host
}]
var allEnvVars = concat(staticEnvVars, aadSecretEnvVars, primaryHostEnvVars)

resource web 'Microsoft.App/containerApps@2025-07-01' = {
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
        external: true
        allowInsecure: false
        targetPort: 8080
        transport: 'auto'
        stickySessions: {
          affinity: 'sticky'
        }
        customDomains: [for domain in customDomains: {
            name: domain
            bindingType: 'SniEnabled'
            certificateId: wildcardCertificateIds[substring(domain, indexOf(domain, '.') + 1)]
          }]
      }
      registries: [
        {
          server: '${containerRegistryName}.azurecr.io'
          identity: acrPullIdentityId
        }
      ]
      secrets: empty(azureAdClientId)
        ? []
        : [
            // Container App references the Key Vault secret at revision start via the managed identity.
            // Rotate by updating Key Vault + restarting the revision — no redeploy required.
            {
              name: 'azure-ad-client-secret'
              keyVaultUrl: '${keyVaultUri}secrets/${aadClientSecretSecretName}'
              identity: acrPullIdentityId
            }
          ]
    }
    template: {
      revisionSuffix: revisionSuffix
      containers: [
        {
          name: 'web'
          image: imageReference
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          env: allEnvVars
          probes: [
            {
              type: 'startup'
              httpGet: {
                path: '/alive'
                port: 8080
                scheme: 'HTTP'
              }
              initialDelaySeconds: 3
              periodSeconds: 5
              failureThreshold: 12  // 3s + 12×5s = 63s max startup
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
        maxReplicas: environmentName == 'staging' ? 3 : 2
        cooldownPeriod: 300
        pollingInterval: 30
        rules: [
          {
            name: 'http-scaling'
            http: {
              metadata: {
                concurrentRequests: '50'
              }
            }
          }
        ]
      }
    }
  }
}

output fqdn string = web.properties.configuration.ingress.fqdn
output id string = web.id
