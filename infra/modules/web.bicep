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

@secure()
@description('Azure AD tenant ID for admin authentication')
param azureAdTenantId string = ''

@secure()
@description('Azure AD client ID for admin authentication')
param azureAdClientId string = ''

@secure()
@description('Azure AD client secret for admin authentication')
param azureAdClientSecret string = ''

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
    secretRef: 'azure-ad-tenant-id'
  }
  {
    name: 'AzureAd__ClientId'
    secretRef: 'azure-ad-client-id'
  }
  {
    name: 'AzureAd__ClientSecret'
    secretRef: 'azure-ad-client-secret'
  }
]
var primaryHostEnvVars = [for (host, i) in primaryHosts: {
  name: 'PrimaryHosts__${i}'
  value: host
}]
var allEnvVars = concat(staticEnvVars, primaryHostEnvVars)

resource web 'Microsoft.App/containerApps@2025-07-01' = {
  name: containerAppName
  location: location
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
        transport: 'http'
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
      secrets: [
        {
          name: 'azure-ad-tenant-id'
          value: azureAdTenantId
        }
        {
          name: 'azure-ad-client-id'
          value: azureAdClientId
        }
        {
          name: 'azure-ad-client-secret'
          value: azureAdClientSecret
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
            cpu: json(environmentName == 'staging' ? '0.25' : '0.5')
            memory: environmentName == 'staging' ? '0.5Gi' : '1Gi'
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
        maxReplicas: environmentName == 'staging' ? 3 : 20
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
