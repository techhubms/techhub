param location string
param registryName string

@allowed(['Basic', 'Standard', 'Premium'])
param sku string = 'Basic'

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2025-04-01' = {
  name: registryName
  location: location
  sku: {
    name: sku
  }
  properties: {
    adminUserEnabled: false
    publicNetworkAccess: 'Enabled'
    dataEndpointEnabled: false
    encryption: {
      status: 'disabled'
    }
  }
}

output loginServer string = containerRegistry.properties.loginServer
output name string = containerRegistry.name
