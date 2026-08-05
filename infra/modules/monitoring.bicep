@description('Azure region for resources')
param location string

@description('Application Insights resource name')
param appInsightsName string

@description('Log Analytics Workspace name')
param logAnalyticsWorkspaceName string

@description('Log Analytics daily ingestion cap in GB (-1 = unlimited)')
param dailyQuotaGb int = -1

@description('Application Insights retention in days')
param appInsightsRetentionInDays int = 90

@description('Host names to monitor with availability tests (e.g. ["tech.hub.ms", "tech.xebia.ms"]). Leave empty to skip.')
param availabilityTestHosts string[] = []

@description('Enable smart detection alert rules (Failure Anomalies). Set false for staging where alerts add no value.')
param enableSmartDetection bool = true

@description('Tags applied to the Log Analytics workspace, Application Insights component and availability resources')
param tags object = {}

// No Azure Monitor Private Link Scope (AMPLS) / private endpoint for this workspace, unlike
// Key Vault, PostgreSQL, and AI Foundry — deliberately, not an oversight:
// - Application Insights JS SDK sends browser (RUM/client-side) telemetry directly from end-user
//   browsers on the public internet. Browsers cannot reach our VNet, so ingestion MUST stay public
//   for that telemetry to arrive at all — an AMPLS would not let us close it.
// - Server-side telemetry (Container Apps) would be the only traffic that could move to a private
//   path via AMPLS, but ingestion still has to stay public for the browser path above, so the
//   security win is small (marginally less public egress) relative to the added cost/complexity
//   (~$7-8/month private endpoint + DNS zones + AMPLS resource).
// - Query access is intentionally left public too (RBAC-protected) for portal/admin use; an AMPLS
//   would only add value here if we also wanted to force queries through a VPN/jumpbox in the VNet.
// See docs/network-architecture.md for the full writeup.
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2025-02-01' = {
  name: logAnalyticsWorkspaceName
  location: location
  tags: tags
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
    publicNetworkAccessForIngestion: 'Enabled'
    // Query enabled: allows portal and admin access (protected by RBAC)
    publicNetworkAccessForQuery: 'Enabled'
    workspaceCapping: {
      dailyQuotaGb: dailyQuotaGb
    }
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  tags: tags
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
    IngestionMode: 'LogAnalytics'
    RetentionInDays: appInsightsRetentionInDays
    // Ingestion enabled: browser JS SDK sends telemetry over the public internet — this is
    // required, not just permissive, since browsers cannot reach the VNet. Server-side (Container
    // Apps) telemetry also goes over this same public endpoint; there is no AMPLS/private endpoint
    // fronting it (see comment above logAnalyticsWorkspace for why).
    // Availability tests use Azure-internal paths and are not affected by this setting.
    publicNetworkAccessForIngestion: 'Enabled'
    // Query enabled: allows portal and admin access (protected by RBAC)
    publicNetworkAccessForQuery: 'Enabled'
  }
}

output appInsightsName string = appInsights.name
output appInsightsId string = appInsights.id
output appInsightsConnectionString string = appInsights.properties.ConnectionString
output logAnalyticsWorkspaceId string = logAnalyticsWorkspace.id

// Single probe location (West Europe) to control Application Insights availability test costs.
// Previously probed from 3 global locations every 15 minutes; that multiplies test executions
// (locations x frequency) and was a major driver of Azure Monitor costs.
var availabilityLocations = [
  { Id: 'emea-nl-ams-azr' }  // West Europe (Amsterdam)
]

// Standard availability test (HTTP GET, SSL check, expect HTTP 200) per host
resource availabilityTests 'Microsoft.Insights/webtests@2022-06-15' = [for host in availabilityTestHosts: {
  name: 'avail-${replace(host, '.', '-')}'
  location: location
  kind: 'standard'
  tags: {
    // Required hidden-link tag so Azure associates this test with the App Insights resource
    'hidden-link:${appInsights.id}': 'Resource'
  }
  properties: {
    Name: host
    SyntheticMonitorId: 'avail-${replace(host, '.', '-')}'
    Kind: 'standard'
    Enabled: true
    Frequency: 1800
    Timeout: 30
    RetryEnabled: true
    Locations: availabilityLocations
    Request: {
      RequestUrl: 'https://${host}/'
      HttpVerb: 'GET'
      ParseDependentRequests: false
      FollowRedirects: true
    }
    ValidationRules: {
      ExpectedHttpStatusCode: 200
      SSLCheck: true
      SSLCertRemainingLifetimeCheck: 7
    }
  }
}]

// Alert when the (single) probe location fails (fires ~immediately on real outages)
resource availabilityAlerts 'Microsoft.Insights/metricAlerts@2018-03-01' = [for (host, i) in availabilityTestHosts: {
  name: 'alert-avail-${replace(host, '.', '-')}'
  location: 'global'
  properties: {
    description: 'Fires when the West Europe probe location cannot reach https://${host}/'
    severity: 1
    enabled: true
    scopes: [
      appInsights.id
      availabilityTests[i].id
    ]
    evaluationFrequency: 'PT1M'
    windowSize: 'PT5M'
    criteria: {
      'odata.type': 'Microsoft.Azure.Monitor.WebtestLocationAvailabilityCriteria'
      webTestId: availabilityTests[i].id
      componentId: appInsights.id
      failedLocationCount: 1
    }
    actions: []
  }
}]

// Smart detection: Failure Anomalies alert rule.
// Azure auto-creates this when App Insights is provisioned with emails enabled.
// Only deploy explicitly when we need to disable it — for production, leave Azure's
// auto-created rule untouched so it keeps its default notification settings.
resource failureAnomaliesRule 'Microsoft.AlertsManagement/smartDetectorAlertRules@2021-04-01' = if (!enableSmartDetection) {
  name: 'Failure Anomalies - ${appInsightsName}'
  location: 'global'
  properties: {
    description: 'Failure Anomalies notifies you of an unusual rise in the rate of failed HTTP requests or dependency calls.'
    state: 'Disabled'
    severity: 'Sev3'
    frequency: 'PT1M'
    detector: {
      id: 'FailureAnomaliesDetector'
    }
    scope: [appInsights.id]
    actionGroups: {
      groupIds: []
    }
  }
}
