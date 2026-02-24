param location string
param containerAppName string
param containerAppsEnvironmentId string
param containerRegistryName string
param imageTag string
param apiBaseUrl string
param appInsightsConnectionString string

// Use public placeholder on first deploy if custom image doesn't exist
var imageReference = imageTag == 'initial' 
  ? 'mcr.microsoft.com/dotnet/samples:aspnetapp' 
  : '${containerRegistryName}.azurecr.io/techhub-web:${imageTag}'

resource web 'Microsoft.App/containerApps@2024-03-01' = {
  name: containerAppName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    managedEnvironmentId: containerAppsEnvironmentId
    configuration: {
      ingress: {
        external: true
        targetPort: 8080
        transport: 'http'
        stickySessions: {
          affinity: 'sticky'
        }
      }
      registries: imageTag == 'initial' ? [] : [
        {
          server: '${containerRegistryName}.azurecr.io'
          identity: 'system'
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
              name: 'TECHHUB_TMP'
              value: '/tmp/techhub'
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
