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

// PostgreSQL Flexible Server
resource postgresServer 'Microsoft.DBforPostgreSQL/flexibleServers@2024-08-01' = {
  name: serverName
  location: location
  sku: {
    name: skuName
    tier: skuTier
  }
  properties: {
    version: postgresVersion
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginPassword
    authConfig: {
      activeDirectoryAuth: 'Disabled'
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
      // Public access enabled with per-IP firewall rules for admin; app uses private endpoint
      publicNetworkAccess: !empty(adminIpAddresses) ? 'Enabled' : 'Disabled'
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
