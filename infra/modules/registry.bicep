param location string
param registryName string

@allowed(['Basic', 'Standard', 'Premium'])
param sku string = 'Basic'

@description('Tags applied to the Container Registry')
param tags object = {}

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2025-04-01' = {
  name: registryName
  location: location
  tags: tags
  sku: {
    name: sku
  }
  properties: {
    adminUserEnabled: false
    publicNetworkAccess: 'Enabled'
    dataEndpointEnabled: false
  }
}

output loginServer string = containerRegistry.properties.loginServer
output name string = containerRegistry.name
