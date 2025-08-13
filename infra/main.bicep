@description('Base name for all resources - will be used to generate resource names')
param baseName string

@description('Azure region where resources will be deployed')
param location string = resourceGroup().location

@description('SQL Database service tier')
@allowed([
  'Basic'
  'Standard'
  'Premium'
  'GeneralPurpose'
  'BusinessCritical'
])
param sqlDatabaseTier string = 'Standard'

@description('SQL Database performance level/SKU name')
param sqlDatabaseSku string = 'S2'

@description('Maximum size of the database in bytes (default: 250GB)')
param maxSizeBytes int = 268435456000

@description('Admin username for SQL Server')
param sqlAdminUsername string

@description('Admin password for SQL Server')
@secure()
param sqlAdminPassword string

@description('Environment suffix for resource naming')
@allowed([
  'dev'
  'test'
  'prod'
])
param environment string = 'dev'

@description('Enable Advanced Threat Protection')
param enableAdvancedThreatProtection bool = false

@description('Configure backup retention in days (1-35 for Basic/Standard, 1-35 for Premium/Business Critical)')
param backupRetentionDays int = 7

@description('Enable audit logging')
param enableAuditLogging bool = false

@description('Storage account name for audit logs (required if enableAuditLogging is true)')
param auditStorageAccountName string = ''

@description('GitHub repository URL for Static Web Apps deployment')
param repositoryUrl string = 'https://github.com/techhubms/techhub'

@description('GitHub branch for Static Web Apps deployment')
param repositoryBranch string = 'main'

@description('GitHub personal access token for Static Web Apps (leave empty for manual configuration)')
@secure()
param githubToken string = ''

// Variables for consistent naming
var sqlServerName = '${baseName}-sqlserver-${environment}'
var sqlDatabaseName = '${baseName}-sqldb-${environment}'
var keyVaultName = '${baseName}-kv-${environment}'
var staticWebAppName = '${baseName}-staticapp-${environment}'

// SQL Server resource
resource sqlServer 'Microsoft.Sql/servers@2023-05-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: sqlAdminUsername
    administratorLoginPassword: sqlAdminPassword
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    restrictOutboundNetworkAccess: 'Disabled'
  }
  
  identity: {
    type: 'SystemAssigned'
  }

  tags: {
    Environment: environment
    Project: baseName
    ResourceType: 'Database'
  }
}

// SQL Database resource
resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-05-01-preview' = {
  parent: sqlServer
  name: sqlDatabaseName
  location: location
  sku: {
    name: sqlDatabaseSku
    tier: sqlDatabaseTier
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: maxSizeBytes
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
    zoneRedundant: false
    readScale: 'Disabled'
    requestedBackupStorageRedundancy: 'Local'
    isLedgerOn: false
    
    // Optimize for expected workload (1K-100K rows)
    autoPauseDelay: sqlDatabaseTier == 'GeneralPurpose' ? 60 : null
    minCapacity: sqlDatabaseTier == 'GeneralPurpose' ? json('0.5') : null
  }

  tags: {
    Environment: environment
    Project: baseName
    ResourceType: 'Database'
  }
}

// Configure backup retention
resource backupShortTermRetention 'Microsoft.Sql/servers/databases/backupShortTermRetentionPolicies@2023-05-01-preview' = {
  parent: sqlDatabase
  name: 'default'
  properties: {
    retentionDays: backupRetentionDays
  }
}

// Firewall rule to allow Azure services
resource allowAzureServices 'Microsoft.Sql/servers/firewallRules@2023-05-01-preview' = {
  parent: sqlServer
  name: 'AllowAzureServices'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

// Advanced Threat Protection (optional)
resource advancedThreatProtection 'Microsoft.Sql/servers/advancedThreatProtectionSettings@2023-05-01-preview' = if (enableAdvancedThreatProtection) {
  parent: sqlServer
  name: 'Default'
  properties: {
    state: 'Enabled'
  }
}

// Key Vault for storing connection strings (optional but recommended)
resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: keyVaultName
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    accessPolicies: []
    enabledForDeployment: false
    enabledForDiskEncryption: false
    enabledForTemplateDeployment: true
    enableSoftDelete: true
    softDeleteRetentionInDays: 7
    enableRbacAuthorization: true
    publicNetworkAccess: 'Enabled'
  }

  tags: {
    Environment: environment
    Project: baseName
    ResourceType: 'Security'
  }
}

// Store connection string in Key Vault
resource connectionStringSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  parent: keyVault
  name: '${baseName}-sql-connection-string'
  properties: {
    value: 'Server=tcp:${sqlServer.properties.fullyQualifiedDomainName},1433;Initial Catalog=${sqlDatabaseName};Persist Security Info=False;User ID=${sqlAdminUsername};Password=${sqlAdminPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
  }
}

// Audit logging configuration (optional)
resource auditingSettings 'Microsoft.Sql/servers/auditingSettings@2023-05-01-preview' = if (enableAuditLogging && !empty(auditStorageAccountName)) {
  parent: sqlServer
  name: 'default'
  properties: {
    state: 'Enabled'
    storageEndpoint: 'https://${auditStorageAccountName}.blob.${az.environment().suffixes.storage}/'
    retentionDays: 90
    auditActionsAndGroups: [
      'SUCCESSFUL_DATABASE_AUTHENTICATION_GROUP'
      'FAILED_DATABASE_AUTHENTICATION_GROUP'
      'BATCH_COMPLETED_GROUP'
    ]
    isStorageSecondaryKeyInUse: false
    isAzureMonitorTargetEnabled: false
  }
}

// Static Web Apps resource for Jekyll site
resource staticWebApp 'Microsoft.Web/staticSites@2023-01-01' = {
  name: staticWebAppName
  location: location
  sku: {
    name: 'Free'
    tier: 'Free'
  }
  properties: {
    repositoryUrl: repositoryUrl
    branch: repositoryBranch
    repositoryToken: !empty(githubToken) ? githubToken : null
    buildProperties: {
      appLocation: '/'
      apiLocation: ''
      outputLocation: '_site'
      appBuildCommand: 'bundle exec jekyll build --destination _site'
      apiBuildCommand: ''
      skipGithubActionWorkflowGeneration: false
    }
    stagingEnvironmentPolicy: 'Enabled'
    allowConfigFileUpdates: true
    enterpriseGradeCdnStatus: 'Disabled'
  }

  tags: {
    Environment: environment
    Project: baseName
    ResourceType: 'Web'
  }
}

// Outputs
output sqlServerName string = sqlServer.name
output sqlServerFqdn string = sqlServer.properties.fullyQualifiedDomainName
output sqlDatabaseName string = sqlDatabase.name
output connectionStringSecretName string = connectionStringSecret.name
output keyVaultName string = keyVault.name
output sqlServerResourceId string = sqlServer.id
output sqlDatabaseResourceId string = sqlDatabase.id
output staticWebAppName string = staticWebApp.name
output staticWebAppUrl string = staticWebApp.properties.defaultHostname
output staticWebAppId string = staticWebApp.id

// Output sample table creation script
output sampleTableScript string = '''
-- Sample table structure for your application
-- Adjust column names and types according to your specific needs

CREATE TABLE [dbo].[ApplicationData] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [Category] NVARCHAR(100) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL DEFAULT 'Active',
    [CreatedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [ModifiedDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] NVARCHAR(255) NOT NULL,
    [ModifiedBy] NVARCHAR(255) NOT NULL,
    [Tags] NVARCHAR(500) NULL,
    [AdditionalData] NVARCHAR(MAX) NULL -- JSON data or additional text
);

-- Create indexes for better performance with expected 1K-100K rows
CREATE INDEX IX_ApplicationData_Category ON [dbo].[ApplicationData] ([Category]);
CREATE INDEX IX_ApplicationData_Status ON [dbo].[ApplicationData] ([Status]);
CREATE INDEX IX_ApplicationData_CreatedDate ON [dbo].[ApplicationData] ([CreatedDate]);

-- Create a trigger to automatically update ModifiedDate
CREATE TRIGGER TR_ApplicationData_UpdateModifiedDate
ON [dbo].[ApplicationData]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE [dbo].[ApplicationData]
    SET [ModifiedDate] = GETUTCDATE()
    WHERE [Id] IN (SELECT [Id] FROM inserted);
END;
'''
