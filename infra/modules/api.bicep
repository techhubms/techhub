param location string
param containerAppName string
param containerAppsEnvironmentId string
param containerRegistryName string
param imageTag string
param appInsightsConnectionString string

@description('Base URL for the web frontend (used for CORS and BaseUrl configuration)')
param webFqdn string = ''

@secure()
@description('PostgreSQL connection string')
param databaseConnectionString string

// Use public placeholder on first deploy if custom image doesn't exist
var imageReference = imageTag == 'initial' 
  ? 'mcr.microsoft.com/dotnet/samples:aspnetapp' 
  : '${containerRegistryName}.azurecr.io/techhub-api:${imageTag}'

resource api 'Microsoft.App/containerApps@2024-03-01' = {
  name: containerAppName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    managedEnvironmentId: containerAppsEnvironmentId
    configuration: {
      ingress: {
        external: false
        targetPort: 8080
        transport: 'http'
        corsPolicy: {
          allowedOrigins: [
            'https://*.azurecontainerapps.io'
          ]
          allowedMethods: ['GET', 'POST', 'PUT', 'DELETE', 'OPTIONS']
          allowedHeaders: ['*']
          allowCredentials: false
        }
      }
      registries: imageTag == 'initial' ? [] : [
        {
          server: '${containerRegistryName}.azurecr.io'
          identity: 'system'
        }
      ]
      secrets: [
        {
          name: 'db-connection-string'
          value: databaseConnectionString
        }
      ]
    }
    template: {
      containers: [
        {
          name: 'api'
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
              name: 'Database__Provider'
              value: 'PostgreSQL'
            }
            {
              name: 'Database__ConnectionString'
              secretRef: 'db-connection-string'
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
              name: 'AppSettings__Content__CollectionsPath'
              value: '/app/collections'
            }
            {
              name: 'AppSettings__BaseUrl'
              value: webFqdn != '' ? 'https://${webFqdn}' : 'https://${containerAppName}.azurecontainerapps.io'
            }
            {
              name: 'Cors__AllowedOrigins__0'
              value: webFqdn != '' ? 'https://${webFqdn}' : 'https://*.azurecontainerapps.io'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 10
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
