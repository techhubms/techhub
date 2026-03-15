param location string
param containerAppName string
param containerAppsEnvironmentId string
param containerAppsEnvironmentName string
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
param customDomains array = []

var imageReference = '${containerRegistryName}.azurecr.io/techhub-web:${imageTag}'
var revisionSuffix = 'web-${imageTag}'

// Reference the managed environment (needed for managed certificate)
resource containerAppsEnv 'Microsoft.App/managedEnvironments@2025-07-01' existing = {
  name: containerAppsEnvironmentName
}

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
            bindingType: 'Auto'
          }]
      }
      registries: [
        {
          server: '${containerRegistryName}.azurecr.io'
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
            cpu: json('0.5')
            memory: '1Gi'
          }
          env: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: aspnetEnvironment
            }
            {
              name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
              value: appInsightsConnectionString
            }
            {
              name: 'OTEL_EXPORTER_OTLP_ENDPOINT'
              value: 'https://otlp.applicationinsights.azure.com/'
            }
            {
              name: 'OTEL_EXPORTER_OTLP_HEADERS'
              value: 'Authorization=Bearer ${appInsightsConnectionString}'
            }
            {
              name: 'ApiBaseUrl'
              value: 'https://${apiBaseUrl}'
            }
            {
              name: 'TECHHUB_TMP'
              value: '/tmp/techhub'
            }
          ]
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
        minReplicas: 2
        maxReplicas: 20
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

// Managed certificates for custom domains (issued by Azure, auto-renewed).
// Uses 2025-07-01 API with bindingType 'Auto' on the container app to solve
// the chicken-and-egg problem (see github.com/microsoft/azure-container-apps/issues/796).
// The certs must be created AFTER the container app has the hostnames registered.
resource managedCerts 'Microsoft.App/managedEnvironments/managedCertificates@2025-07-01' = [for domain in customDomains: {
  parent: containerAppsEnv
  name: 'cert-${replace(domain, '.', '-')}'
  location: location
  properties: {
    subjectName: domain
    domainControlValidation: 'CNAME'
  }
  dependsOn: [web]
}]
