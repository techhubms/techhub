// Associates resources with an existing Network Security Perimeter (NSP).
// Used by environment deployments to add selected resources, such as
// App Insights and Log Analytics, to the shared NSP.

@description('NSP name (must exist in this resource group)')
param nspName string

@description('NSP profile name (child of the NSP)')
param profileName string = 'profile-techhub'

@description('Association name prefix (must be unique per NSP)')
param associationPrefix string

@description('Resource IDs to associate with the NSP')
param resourceIds string[]

// Reference existing NSP
resource nsp 'Microsoft.Network/networkSecurityPerimeters@2023-08-01-preview' existing = {
  name: nspName
}

// Reference existing profile (child of NSP)
resource profile 'Microsoft.Network/networkSecurityPerimeters/profiles@2023-08-01-preview' existing = {
  parent: nsp
  name: profileName
}

// Associate each resource with the NSP (Enforced mode)
resource associations 'Microsoft.Network/networkSecurityPerimeters/resourceAssociations@2023-08-01-preview' = [for (resourceId, i) in resourceIds: {
  name: '${associationPrefix}-${i}'
  location: resourceGroup().location
  parent: nsp
  properties: {
    privateLinkResource: {
      id: resourceId
    }
    profile: {
      id: profile.id
    }
    accessMode: 'Enforced'
  }
}]
