// Subscription-level Azure Policy assignments for baseline governance.
// - Allowed locations: restricts where resources can be deployed.
// - Require tag on resource groups: enforces a standard tag on new RGs.
targetScope = 'subscription'

@description('Allowed Azure regions for all resources (plus "global" for global resources).')
param allowedLocations string[] = ['swedencentral', 'westeurope', 'global']

@description('Required tag name on resource groups')
param requiredTagName string = 'owner'

// Built-in policy definitions
var allowedLocationsPolicyId = '/providers/Microsoft.Authorization/policyDefinitions/e56962a6-4747-49cd-b67b-bf8b01975c4c' // Allowed locations
var requireTagRgPolicyId = '/providers/Microsoft.Authorization/policyDefinitions/96670d01-0a4d-4649-9c89-2d3abc0a5025' // Require a tag on resource groups

resource allowedLocationsAssignment 'Microsoft.Authorization/policyAssignments@2025-01-01' = {
  name: 'techhub-allowed-locations'
  properties: {
    displayName: 'TechHub — Allowed locations'
    description: 'Restricts the locations where TechHub resources can be deployed.'
    policyDefinitionId: allowedLocationsPolicyId
    enforcementMode: 'Default'
    parameters: {
      listOfAllowedLocations: {
        value: allowedLocations
      }
    }
  }
}

resource requireTagOnRgAssignment 'Microsoft.Authorization/policyAssignments@2025-01-01' = {
  name: 'techhub-require-rg-tag'
  properties: {
    displayName: 'TechHub — Require ${requiredTagName} tag on resource groups'
    description: 'Requires the "${requiredTagName}" tag on new resource groups.'
    policyDefinitionId: requireTagRgPolicyId
    enforcementMode: 'Default'
    parameters: {
      tagName: {
        value: requiredTagName
      }
    }
  }
}
