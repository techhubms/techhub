param location string
param containerAppName string
param containerAppsEnvironmentId string
param containerRegistryName string
param acrPullIdentityId string
param imageTag string
param appInsightsConnectionString string

@description('Environment name (staging, prod) - maps to DOTNET_ENVIRONMENT')
@allowed(['staging', 'prod'])
param environmentName string

var dotnetEnvironment = environmentName == 'prod' ? 'Production' : 'Staging'

@secure()
@description('PostgreSQL connection string')
param databaseConnectionString string

@secure()
@description('Azure OpenAI API key')
param openAiApiKey string

@description('Azure OpenAI endpoint URL')
param openAiEndpoint string

@description('Azure OpenAI deployment name (e.g. gpt-4.1)')
param openAiDeploymentName string = 'gpt-4.1'

@description('Content processing interval in hours')
param intervalHours int = 1

var imageReference = '${containerRegistryName}.azurecr.io/techhub-content-processor:${imageTag}'
var revisionSuffix = 'cp-${imageTag}'

resource contentProcessor 'Microsoft.App/containerApps@2025-07-01' = {
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
      // No ingress — this is a background worker with no HTTP endpoints
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
        {
          name: 'openai-api-key'
          value: openAiApiKey
        }
      ]
    }
    template: {
      revisionSuffix: revisionSuffix
      containers: [
        {
          name: 'content-processor'
          image: imageReference
          resources: {
            // Minimal resources — this is a low-traffic background worker
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          env: [
            {
              name: 'DOTNET_ENVIRONMENT'
              value: dotnetEnvironment
            }
            {
              name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
              value: appInsightsConnectionString
            }
            {
              name: 'OTEL_SERVICE_NAME'
              value: 'techhub-content-processor'
            }
            {
              name: 'Database__ConnectionString'
              secretRef: 'db-connection-string'
            }
            {
              name: 'AiCategorization__Endpoint'
              value: openAiEndpoint
            }
            {
              name: 'AiCategorization__ApiKey'
              secretRef: 'openai-api-key'
            }
            {
              name: 'AiCategorization__DeploymentName'
              value: openAiDeploymentName
            }
            {
              name: 'ContentProcessor__IntervalHours'
              value: string(intervalHours)
            }
          ]
        }
      ]
      scale: {
        // Always keep exactly 1 replica — this is a singleton background worker
        minReplicas: 1
        maxReplicas: 1
      }
    }
  }
}

output id string = contentProcessor.id
