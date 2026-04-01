// Adds scoped resources to an existing Azure Monitor Private Link Scope (AMPLS).
// Used by environment deployments to add their App Insights and Log Analytics to the shared AMPLS.

@description('AMPLS name (must exist in this resource group)')
param amplsName string

@description('Scope name prefix (must be unique per AMPLS)')
param scopePrefix string

@description('Resource IDs to add to the AMPLS scope')
param resourceIds string[]

// Reference existing AMPLS
resource ampls 'microsoft.insights/privateLinkScopes@2021-07-01-preview' existing = {
  name: amplsName
}

// Add each resource to the AMPLS scope
resource scopedResources 'Microsoft.Insights/privateLinkScopes/scopedResources@2021-07-01-preview' = [for (id, i) in resourceIds: {
  parent: ampls
  name: '${scopePrefix}-${i}'
  properties: {
    linkedResourceId: id
  }
}]
