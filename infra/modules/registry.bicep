param location string
param registryName string

@allowed(['Basic', 'Standard', 'Premium'])
param sku string = 'Basic'

@description('Admin IP addresses for firewall rules. Private endpoints require Premium SKU (not cost-effective); instead we restrict to admin IPs and add the runner IP on the fly during CI pushes.')
param adminIpAddresses string[] = []

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
    // Allow trusted Azure services (e.g. Container Apps pulling images) to bypass the IP firewall.
    networkRuleBypassOptions: 'AzureServices'
    networkRuleSet: {
      // Deny all by default when admin IPs are provided; fall back to Allow for initial bootstrapping.
      defaultAction: empty(adminIpAddresses) ? 'Allow' : 'Deny'
      ipRules: [for ip in adminIpAddresses: {
        action: 'Allow'
        value: ip
      }]
    }
  }
}

output loginServer string = containerRegistry.properties.loginServer
output name string = containerRegistry.name
