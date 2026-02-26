param location string
param containerAppName string
param containerAppsEnvironmentId string
param containerAppsEnvironmentName string
param containerRegistryName string
param acrPullIdentityId string
param imageTag string
param apiBaseUrl string
param appInsightsConnectionString string

@description('Optional custom domain (e.g. staging-tech.hub.ms). Leave empty to skip.')
param customDomain string = ''

var imageReference = '${containerRegistryName}.azurecr.io/techhub-web:${imageTag}'

// Managed certificate for custom domain (issued by Azure, auto-renewed)
resource containerAppsEnv 'Microsoft.App/managedEnvironments@2025-01-01' existing = {
  name: containerAppsEnvironmentName
}

resource managedCert 'Microsoft.App/managedEnvironments/managedCertificates@2025-01-01' = if (customDomain != '') {
  parent: containerAppsEnv
  name: 'cert-${replace(customDomain, '.', '-')}'
  location: location
  properties: {
    subjectName: customDomain
    domainControlValidation: 'CNAME'
  }
}

resource web 'Microsoft.App/containerApps@2025-01-01' = {
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
        customDomains: customDomain != '' ? [
          {
            name: customDomain
            bindingType: 'SniEnabled'
            certificateId: managedCert.id
          }
        ] : []
      }
      registries: [
        {
          server: '${containerRegistryName}.azurecr.io'
          identity: acrPullIdentityId
        }
      ]
    }
    template: {
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
              value: 'Production'
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
        }
      ]
      scale: {
        minReplicas: 2
        maxReplicas: 20
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
