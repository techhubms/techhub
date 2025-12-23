@description('Base name for all resources - will be used to generate resource names')
param baseName string

@description('Azure region where resources will be deployed')
param location string

@description('Environment suffix for resource naming')
@allowed([
  'dev'
  'test'
  'prod'
])
param environment string

@description('GitHub repository URL for Static Web Apps deployment')
param repositoryUrl string

@description('GitHub branch for Static Web Apps deployment')
param repositoryBranch string

// Variables for consistent naming following "abbreviation-name-environment" pattern
var staticWebAppName = 'stapp-${baseName}-${environment}'

// Static Web Apps resource for Jekyll site
resource staticWebApp 'Microsoft.Web/staticSites@2023-01-01' = {
  name: staticWebAppName
  location: location
  sku: {
    name: 'Standard'
    tier: 'Standard'
  }
  properties: {
    repositoryUrl: repositoryUrl
    branch: repositoryBranch
    repositoryToken: null
    buildProperties: {
      appLocation: '/'
      apiLocation: ''
      appArtifactLocation: '_site'
    }
    stagingEnvironmentPolicy: 'Enabled'
    allowConfigFileUpdates: true
    enterpriseGradeCdnStatus: 'Disabled'
  }

  tags: {
    Environment: environment
    Project: baseName
    ResourceType: 'Web'
  }
}

// Outputs
output staticWebAppName string = staticWebApp.name
output staticWebAppUrl string = staticWebApp.properties.defaultHostname
output staticWebAppId string = staticWebApp.id
