param location string
param appServicePlanName string

@description('Tags applied to the App Service Plan')
param tags object = {}

// Basic B1 (1 vCPU / 1.75 GB), Linux. Hosts both the API and Web sites — App Service Plan
// billing is per-plan (flat), not per-site, so multiple Web Apps share one price.
// Basic tier supports Always On, WebSockets (needed for Blazor InteractiveServer/SignalR),
// and Regional VNet Integration — no Premium tier required.
resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: appServicePlanName
  location: location
  tags: tags
  sku: {
    name: 'B1'
    tier: 'Basic'
  }
  kind: 'linux'
  properties: {
    reserved: true // required for Linux plans
  }
}

output id string = appServicePlan.id
output name string = appServicePlan.name
