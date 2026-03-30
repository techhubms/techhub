// Network Security Perimeter (NSP) — controls public access to associated Azure resources.
// Admin access via IP allowlist; same-subscription Azure services allowed implicitly.
// Associated resources: Key Vault, App Insights, Log Analytics, AI Foundry.

@description('Azure region for the NSP')
param location string

@description('NSP name')
param nspName string

@description('NSP profile name')
param profileName string = 'profile-techhub'

@description('Admin IP address (CIDR notation, e.g. "1.2.3.4/32")')
param adminIpCidr string

@description('Subscription ID to allow inbound access from (defaults to current subscription)')
param allowedSubscriptionId string = subscription().subscriptionId

@description('Resource IDs to associate with this NSP')
param associatedResourceIds string[]

// Network Security Perimeter
resource nsp 'Microsoft.Network/networkSecurityPerimeters@2023-08-01-preview' = {
  name: nspName
  location: location
}

// NSP Profile — single profile for all environments
resource profile 'Microsoft.Network/networkSecurityPerimeters/profiles@2023-08-01-preview' = {
  parent: nsp
  name: profileName
  location: location
}

// Inbound rule: allow admin IP
resource adminIpRule 'Microsoft.Network/networkSecurityPerimeters/profiles/accessRules@2023-08-01-preview' = {
  parent: profile
  name: 'allow-admin-ip'
  location: location
  properties: {
    direction: 'Inbound'
    addressPrefixes: [
      adminIpCidr
    ]
  }
}

// Inbound rule: allow same subscription (Azure services within the subscription)
resource subscriptionRule 'Microsoft.Network/networkSecurityPerimeters/profiles/accessRules@2023-08-01-preview' = {
  parent: profile
  name: 'allow-subscription'
  location: location
  properties: {
    direction: 'Inbound'
    subscriptions: [
      {
        id: '/subscriptions/${allowedSubscriptionId}'
      }
    ]
  }
}

// Associate each resource with the NSP (Enforced mode)
resource associations 'Microsoft.Network/networkSecurityPerimeters/resourceAssociations@2023-08-01-preview' = [for (resourceId, i) in associatedResourceIds: {
  name: '${nspName}-assoc-${i}'
  location: location
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

output nspId string = nsp.id
output profileId string = profile.id
