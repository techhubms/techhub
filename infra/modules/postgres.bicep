@description('Azure region for resources')
param location string

@description('PostgreSQL server name')
param serverName string

@description('Administrator login name')
param administratorLogin string

@secure()
@description('Administrator login password')
param administratorLoginPassword string

@description('Database name')
param databaseName string = 'techhub'

@description('PostgreSQL version')
@allowed(['16', '17'])
param postgresVersion string = '17'

@description('SKU name for the server')
param skuName string = 'Standard_B1ms'

@description('SKU tier')
@allowed(['Burstable', 'GeneralPurpose', 'MemoryOptimized'])
param skuTier string = 'Burstable'

@description('Storage size in GB')
param storageSizeGB int = 32

@description('Enable storage auto-grow (recommended for production)')
param storageAutoGrow bool = true

@description('Backup retention in days (7-35)')
@minValue(7)
@maxValue(35)
param backupRetentionDays int = 14

@description('Enable geo-redundant backup for disaster recovery')
param geoRedundantBackup bool = false

@description('Admin IP addresses for firewall rules (optional — leave empty to keep public access disabled)')
param adminIpAddresses string[] = []

@description('Static outbound IP of the Container Apps Environment (load balancer SNAT IP). Container Apps uses this IP when connecting to PostgreSQL public endpoint. Leave empty to skip.')
param containerAppsStaticIp string = ''

@description('Entra (AAD) object ID of the principal to register as the Active Directory administrator. Leave empty to skip Entra admin setup.')
param entraAdminObjectId string = ''

@description('Display name of the Entra (AAD) administrator principal (e.g. managed identity name). Required when entraAdminObjectId is provided.')
param entraAdminName string = ''

@description('Tags applied to the PostgreSQL server')
param tags object = {}

// PostgreSQL Flexible Server
resource postgresServer 'Microsoft.DBforPostgreSQL/flexibleServers@2024-08-01' = {
  name: serverName
  location: location
  tags: tags
  sku: {
    name: skuName
    tier: skuTier
  }
  properties: {
    version: postgresVersion
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginPassword
    authConfig: {
      // Enable both password (for infrastructure management) and Entra ID auth.
      // Applications use Entra tokens (Database:UseEntraAuth=true); password remains
      // available for admin access and emergency scenarios.
      activeDirectoryAuth: !empty(entraAdminObjectId) ? 'Enabled' : 'Disabled'
      passwordAuth: 'Enabled'
    }
    dataEncryption: {
      type: 'SystemManaged'
    }
    storage: {
      storageSizeGB: storageSizeGB
      autoGrow: storageAutoGrow ? 'Enabled' : 'Disabled'
      tier: 'P4'
    }
    backup: {
      backupRetentionDays: backupRetentionDays
      geoRedundantBackup: geoRedundantBackup ? 'Enabled' : 'Disabled'
    }
    highAvailability: {
      mode: 'Disabled'
    }
    maintenanceWindow: {
      customWindow: 'Disabled'
      dayOfWeek: 0
      startHour: 0
      startMinute: 0
    }
    network: {
      publicNetworkAccess: (!empty(adminIpAddresses) || !empty(containerAppsStaticIp)) ? 'Enabled' : 'Disabled'
    }
  }
}

// Firewall rules: allow admin IPs
resource adminFirewallRules 'Microsoft.DBforPostgreSQL/flexibleServers/firewallRules@2024-08-01' = [for (ip, i) in adminIpAddresses: {
  parent: postgresServer
  name: 'allow-admin-ip-${i}'
  properties: {
    startIpAddress: ip
    endIpAddress: ip
  }
}]

// Firewall rule: allow the Container Apps Environment's static outbound IP.
// VNet-integrated Container Apps use Azure SNAT (load balancer frontend IP) for outbound
// connections to public endpoints. The VNet subnet IP range (10.x.x.x) is NOT the source IP
// — it is SNAT'd to the Container Apps Environment's staticIp before leaving Azure.
resource containerAppsStaticIpFirewallRule 'Microsoft.DBforPostgreSQL/flexibleServers/firewallRules@2024-08-01' = if (!empty(containerAppsStaticIp)) {
  parent: postgresServer
  name: 'allow-container-apps-static-ip'
  properties: {
    startIpAddress: containerAppsStaticIp
    endIpAddress: containerAppsStaticIp
  }
}

// Entra ID (AAD) administrator for the PostgreSQL server.
// When set, applications can authenticate with a managed identity token instead of a password.
// The entraAdminObjectId is the AAD object ID of the principal (e.g. a user-assigned managed identity).
// dependsOn firewall rules: ensures the server has finished applying network changes before we
// attempt the Entra admin operation — avoids AadAuthOperationCannotBePerformedWhenServerIsNotAccessible
// which occurs when the server is still in 'Updating' state after the parent resource update.
resource entraAdmin 'Microsoft.DBforPostgreSQL/flexibleServers/administrators@2024-08-01' = if (!empty(entraAdminObjectId)) {
  parent: postgresServer
  // The resource name must be the AAD object ID (GUID) of the principal.
  name: entraAdminObjectId
  properties: {
    principalName: entraAdminName
    principalType: 'ServicePrincipal'
    tenantId: subscription().tenantId
  }
  dependsOn: [adminFirewallRules, containerAppsStaticIpFirewallRule]
}

// Database
resource database 'Microsoft.DBforPostgreSQL/flexibleServers/databases@2024-08-01' = {
  parent: postgresServer
  name: databaseName
  properties: {
    charset: 'UTF8'
    collation: 'en_US.utf8'
  }
}

// Outputs
output serverFqdn string = postgresServer.properties.fullyQualifiedDomainName
output serverName string = postgresServer.name
output serverId string = postgresServer.id
output databaseName string = database.name
