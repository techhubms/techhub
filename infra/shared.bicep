// Shared infrastructure for TechHub — ACR and related resources
targetScope = 'subscription'

@description('Azure region for shared resources')
param location string = 'westeurope'

@description('Shared resource group name')
param resourceGroupName string = 'rg-techhub-shared'

@description('Container Registry name (alphanumeric only)')
param containerRegistryName string = 'crtechhubms'

// Shared Resource Group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: resourceGroupName
  location: location
}

// Shared Container Registry
module registry './modules/registry.bicep' = {
  scope: resourceGroup
  name: 'registry-deployment'
  params: {
    location: location
    registryName: containerRegistryName
    sku: 'Standard' // Sufficient for both staging and prod
  }
}

// Outputs
output resourceGroupName string = resourceGroup.name
output containerRegistryName string = registry.outputs.name
output containerRegistryLoginServer string = registry.outputs.loginServer
