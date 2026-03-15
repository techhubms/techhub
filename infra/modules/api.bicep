param location string
param containerAppName string
param containerAppsEnvironmentId string
param containerRegistryName string
param acrPullIdentityId string
param imageTag string
param appInsightsConnectionString string

@description('FQDNs for the web frontend (used for CORS and BaseUrl configuration)')
param webFqdns array = []

@secure()
@description('PostgreSQL connection string')
param databaseConnectionString string

var imageReference = '${containerRegistryName}.azurecr.io/techhub-api:${imageTag}'
var customOrigins = [for fqdn in webFqdns: 'https://${fqdn}']
var corsOrigins = union(['https://*.azurecontainerapps.io'], customOrigins)
var corsEnvVars = [for (fqdn, i) in webFqdns: {
  name: 'Cors__AllowedOrigins__${i}'
  value: 'https://${fqdn}'
}]
var staticEnvVars = [
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
]

resource api 'Microsoft.App/containerApps@2025-07-01' = {
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
          env: concat(staticEnvVars, corsEnvVars)
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 10
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
