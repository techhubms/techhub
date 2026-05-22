@description('Azure region for resources')
param location string

@description('VNet name')
param vnetName string

@description('VNet address space prefix')
param addressSpacePrefix string = '10.0.0.0/16'

@description('Container Apps subnet name')
param containerAppsSubnetName string = 'snet-container-apps'

@description('Container Apps subnet prefix')
param containerAppsSubnetPrefix string = '10.0.0.0/23'

@description('Tags applied to networking resources')
param tags object = {}

@description('''
Availability zones for the NAT Gateway and its public IP.
  - ['1'] (default) — pin to a single zone; use when the existing resource was created zonal
    (zones are immutable on public IPs — must match the existing resource to avoid deployment errors).
  - [] — non-zonal / regional; works in regions without availability zone support.
  Pass an explicit value to override the default for new or zone-redundant deployments.
''')
param natGatewayZones array = ['1']

// NAT Gateway public IP — provides a stable, predictable outbound IP for Container Apps.
// Without a NAT Gateway, Container Apps uses a large shared pool of ephemeral Azure
// infrastructure SNAT IPs (~150+ IPs) that change unpredictably and cannot be allowlisted
// in PostgreSQL firewall rules. The NAT Gateway gives a single fixed outbound IP.
resource natGatewayPublicIp 'Microsoft.Network/publicIPAddresses@2025-01-01' = {
  name: replace(vnetName, 'vnet-', 'pip-nat-')
  location: location
  tags: tags
  sku: {
    name: 'Standard'
    tier: 'Regional'
  }
  zones: natGatewayZones
  properties: {
    publicIPAllocationMethod: 'Static'
    publicIPAddressVersion: 'IPv4'
  }
}

// NAT Gateway — routes all outbound Container Apps traffic through the stable public IP above.
resource natGateway 'Microsoft.Network/natGateways@2025-01-01' = {
  name: replace(vnetName, 'vnet-', 'natgw-')
  location: location
  tags: tags
  sku: {
    name: 'Standard'
  }
  zones: natGatewayZones
  properties: {
    idleTimeoutInMinutes: 10
    publicIpAddresses: [
      {
        id: natGatewayPublicIp.id
      }
    ]
  }
}

// Virtual Network with a single Container Apps subnet.
// Key Vault uses a VNet service endpoint on this subnet (no private endpoint needed).
resource vnet 'Microsoft.Network/virtualNetworks@2025-01-01' = {
  name: vnetName
  location: location
  tags: tags
  properties: {
    addressSpace: {
      addressPrefixes: [
        addressSpacePrefix
      ]
    }
    enableDdosProtection: false
    subnets: [
      {
        name: containerAppsSubnetName
        properties: {
          addressPrefix: containerAppsSubnetPrefix
          natGateway: {
            id: natGateway.id
          }
          delegations: [
            {
              name: 'Microsoft.App.environments'
              properties: {
                serviceName: 'Microsoft.App/environments'
              }
            }
          ]
          // Service endpoints — allows Container Apps to reach Azure services
          // over the Microsoft backbone without private endpoints.
          // Microsoft.KeyVault: Key Vault traffic uses private backbone.
          // Note: Microsoft.DBforPostgreSQL is NOT a supported service endpoint type.
          //   PostgreSQL access is controlled via firewall rules on the IP range instead.
          serviceEndpoints: [
            {
              service: 'Microsoft.KeyVault'
            }
          ]
        }
      }
    ]
  }
}

// Outputs
output vnetId string = vnet.id
output vnetName string = vnet.name
output containerAppsSubnetId string = vnet.properties.subnets[0].id
// Public IP used by the NAT Gateway — must be allowlisted in PostgreSQL firewall rules
// so Container Apps can reach PostgreSQL over the public internet.
output natGatewayPublicIp string = natGatewayPublicIp.properties.ipAddress
