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

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2025-02-01' = {
  name: logAnalyticsWorkspaceName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
    // Ingestion disabled: app telemetry uses AMPLS private path
    publicNetworkAccessForIngestion: 'Disabled'
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
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
    IngestionMode: 'LogAnalytics'
    RetentionInDays: appInsightsRetentionInDays
    // Ingestion enabled: browser JS SDK sends telemetry over the public internet.
    // Server-side telemetry uses the AMPLS private path.
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

// Five geographically distributed probe locations for global coverage
var availabilityLocations = [
  { Id: 'emea-nl-ams-azr' }  // West Europe (Amsterdam)
  { Id: 'us-ca-sjc-azr' }    // West US (San Jose)
  { Id: 'us-tx-sn1-azr' }    // South Central US
  { Id: 'apac-sg-sin-azr' }  // Southeast Asia (Singapore)
  { Id: 'emea-gb-db3-azr' }  // North Europe (Dublin)
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
    Frequency: 300
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

// Alert when 2+ probe locations fail simultaneously (fires ~immediately on real outages)
resource availabilityAlerts 'Microsoft.Insights/metricAlerts@2018-03-01' = [for (host, i) in availabilityTestHosts: {
  name: 'alert-avail-${replace(host, '.', '-')}'
  location: 'global'
  properties: {
    description: 'Fires when 2 or more probe locations cannot reach https://${host}/'
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
      failedLocationCount: 2
    }
    actions: []
  }
}]
