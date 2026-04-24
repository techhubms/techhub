// Operational alerts for an environment. Targets:
// - App Insights: elevated exception rate + failed request rate
// - Postgres Flexible Server: CPU saturation + connection count
// - AI Foundry (Cognitive Services account): client errors (429/5xx)
// - Container Apps: replica restart storms
//
// All alerts notify the shared action group.

@description('Azure region (for metric alerts that support regions; log alerts use global)')
param location string = resourceGroup().location

@description('Environment name (staging, prod)')
param environmentName string

@description('Shared action group resource ID')
param actionGroupId string

@description('Application Insights resource ID')
param appInsightsId string

@description('Log Analytics workspace ID (used for log query alerts)')
param logAnalyticsWorkspaceId string

@description('PostgreSQL Flexible Server resource ID')
param postgresServerId string

@description('AI Foundry (Cognitive Services) account resource ID')
param openAiAccountId string

@description('Tags to apply to all alerts')
param tags object = {}

var severityHigh = 1
var severityMedium = 2

// --- Application Insights: failed request rate ---
resource failedRequestsAlert 'Microsoft.Insights/metricAlerts@2018-03-01' = {
  name: 'alert-${environmentName}-failed-requests'
  location: 'global'
  tags: tags
  properties: {
    description: 'Fires when failed HTTP requests exceed 10 in 15 minutes.'
    severity: severityHigh
    enabled: true
    scopes: [appInsightsId]
    evaluationFrequency: 'PT5M'
    windowSize: 'PT15M'
    criteria: {
      'odata.type': 'Microsoft.Azure.Monitor.SingleResourceMultipleMetricCriteria'
      allOf: [
        {
          name: 'FailedRequests'
          metricNamespace: 'microsoft.insights/components'
          metricName: 'requests/failed'
          operator: 'GreaterThan'
          threshold: 10
          timeAggregation: 'Count'
          criterionType: 'StaticThresholdCriterion'
        }
      ]
    }
    actions: [
      {
        actionGroupId: actionGroupId
      }
    ]
  }
}

// --- Application Insights: exception burst ---
resource exceptionsAlert 'Microsoft.Insights/metricAlerts@2018-03-01' = {
  name: 'alert-${environmentName}-exceptions'
  location: 'global'
  tags: tags
  properties: {
    description: 'Fires when server-side exceptions exceed 20 in 15 minutes.'
    severity: severityMedium
    enabled: true
    scopes: [appInsightsId]
    evaluationFrequency: 'PT5M'
    windowSize: 'PT15M'
    criteria: {
      'odata.type': 'Microsoft.Azure.Monitor.SingleResourceMultipleMetricCriteria'
      allOf: [
        {
          name: 'Exceptions'
          metricNamespace: 'microsoft.insights/components'
          metricName: 'exceptions/server'
          operator: 'GreaterThan'
          threshold: 20
          timeAggregation: 'Count'
          criterionType: 'StaticThresholdCriterion'
        }
      ]
    }
    actions: [
      {
        actionGroupId: actionGroupId
      }
    ]
  }
}

// --- PostgreSQL: CPU saturation ---
resource postgresCpuAlert 'Microsoft.Insights/metricAlerts@2018-03-01' = {
  name: 'alert-${environmentName}-postgres-cpu'
  location: 'global'
  tags: tags
  properties: {
    description: 'Fires when PostgreSQL CPU is sustained above 80%.'
    severity: severityMedium
    enabled: true
    scopes: [postgresServerId]
    evaluationFrequency: 'PT5M'
    windowSize: 'PT15M'
    criteria: {
      'odata.type': 'Microsoft.Azure.Monitor.SingleResourceMultipleMetricCriteria'
      allOf: [
        {
          name: 'CpuPercent'
          metricNamespace: 'Microsoft.DBforPostgreSQL/flexibleServers'
          metricName: 'cpu_percent'
          operator: 'GreaterThan'
          threshold: 80
          timeAggregation: 'Average'
          criterionType: 'StaticThresholdCriterion'
        }
      ]
    }
    actions: [
      {
        actionGroupId: actionGroupId
      }
    ]
  }
}

// --- PostgreSQL: connection saturation (B1ms ~50 max connections) ---
resource postgresConnAlert 'Microsoft.Insights/metricAlerts@2018-03-01' = {
  name: 'alert-${environmentName}-postgres-connections'
  location: 'global'
  tags: tags
  properties: {
    description: 'Fires when active PostgreSQL connections exceed 40 (B1ms has ~50 max).'
    severity: severityMedium
    enabled: true
    scopes: [postgresServerId]
    evaluationFrequency: 'PT5M'
    windowSize: 'PT15M'
    criteria: {
      'odata.type': 'Microsoft.Azure.Monitor.SingleResourceMultipleMetricCriteria'
      allOf: [
        {
          name: 'ActiveConnections'
          metricNamespace: 'Microsoft.DBforPostgreSQL/flexibleServers'
          metricName: 'active_connections'
          operator: 'GreaterThan'
          threshold: 40
          timeAggregation: 'Average'
          criterionType: 'StaticThresholdCriterion'
        }
      ]
    }
    actions: [
      {
        actionGroupId: actionGroupId
      }
    ]
  }
}

// --- AI Foundry: client errors (429 / 5xx) ---
resource openAiErrorsAlert 'Microsoft.Insights/metricAlerts@2018-03-01' = {
  name: 'alert-${environmentName}-openai-errors'
  location: 'global'
  tags: tags
  properties: {
    description: 'Fires when AI Foundry client errors exceed 10 in 15 minutes (rate limits / failures).'
    severity: severityMedium
    enabled: true
    scopes: [openAiAccountId]
    evaluationFrequency: 'PT5M'
    windowSize: 'PT15M'
    criteria: {
      'odata.type': 'Microsoft.Azure.Monitor.SingleResourceMultipleMetricCriteria'
      allOf: [
        {
          name: 'ClientErrors'
          metricNamespace: 'Microsoft.CognitiveServices/accounts'
          metricName: 'ClientErrors'
          operator: 'GreaterThan'
          threshold: 10
          timeAggregation: 'Total'
          criterionType: 'StaticThresholdCriterion'
        }
      ]
    }
    actions: [
      {
        actionGroupId: actionGroupId
      }
    ]
  }
}

// --- Container Apps: replica restart storm (log query alert via Log Analytics) ---
// Detects repeated container restarts indicating a crash loop.
resource containerRestartAlert 'Microsoft.Insights/scheduledQueryRules@2023-03-15-preview' = {
  name: 'alert-${environmentName}-container-restarts'
  location: location
  tags: tags
  properties: {
    displayName: 'Container App restart storm (${environmentName})'
    description: 'Fires when a Container App revision restarts 5+ times within 15 minutes.'
    severity: severityMedium
    enabled: true
    evaluationFrequency: 'PT5M'
    windowSize: 'PT15M'
    scopes: [logAnalyticsWorkspaceId]
    criteria: {
      allOf: [
        {
          query: 'ContainerAppSystemLogs_CL\n| where Reason_s in ("StartupProbeFailed", "LivenessProbeFailed", "ContainerCrashed", "Killing")\n| summarize restartCount = count() by ContainerAppName_s, RevisionName_s\n| where restartCount >= 5'
          timeAggregation: 'Count'
          operator: 'GreaterThan'
          threshold: 0
          failingPeriods: {
            numberOfEvaluationPeriods: 1
            minFailingPeriodsToAlert: 1
          }
        }
      ]
    }
    actions: {
      actionGroups: [actionGroupId]
    }
  }
}

output alertIds string[] = [
  failedRequestsAlert.id
  exceptionsAlert.id
  postgresCpuAlert.id
  postgresConnAlert.id
  openAiErrorsAlert.id
  containerRestartAlert.id
]
