targetScope = 'subscription'

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

// Variables for consistent naming
var resourceGroupName = 'rg-${baseName}-${environment}'

// Resource Group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2023-07-01' = {
  name: resourceGroupName
  location: location
  
  tags: {
    Environment: environment
    Project: baseName
    ResourceType: 'ResourceGroup'
  }
}

// Deploy resources using module pattern to target the resource group
module resources 'modules/resources.bicep' = {
  name: 'resources'
  scope: resourceGroup
  params: {
    baseName: baseName
    location: location
    environment: environment
    repositoryUrl: repositoryUrl
    repositoryBranch: repositoryBranch
  }
}

// Outputs
output resourceGroupName string = resourceGroup.name
output staticWebAppName string = resources.outputs.staticWebAppName
output staticWebAppUrl string = resources.outputs.staticWebAppUrl
output staticWebAppId string = resources.outputs.staticWebAppId
