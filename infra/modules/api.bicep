param location string
param containerAppName string
param containerAppsEnvironmentId string
param containerRegistryName string
param imageTag string
param appInsightsConnectionString string

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
        external: true
        targetPort: 8080
        transport: 'http2'
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
              name: 'Content__SectionsJsonPath'
              value: '/app/data/sections.json'
            }
            {
              name: 'Content__CollectionsRootPath'
              value: '/app/data/collections'
            }
            {
              name: 'Content__Timezone'
              value: 'Europe/Brussels'
            }
            {
              name: 'Content__EnableCaching'
              value: 'true'
            }
            {
              name: 'Content__CacheExpirationMinutes'
              value: '60'
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
