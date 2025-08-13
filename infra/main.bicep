targetScope = 'subscription'

@description('Base name for all resources - will be used to generate resource names')
param baseName string

@description('Azure region where resources will be deployed')
param location string

@description('SQL Database service tier')
@allowed([
  'Basic'
  'Standard'
  'Premium'
  'GeneralPurpose'
  'BusinessCritical'
])
param sqlDatabaseTier string

@description('SQL Database performance level/SKU name')
param sqlDatabaseSku string

@description('Maximum size of the database in bytes')
param maxSizeBytes int

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
param environment string

@description('Enable Advanced Threat Protection')
param enableAdvancedThreatProtection bool

@description('Configure backup retention in days (1-35 for Basic/Standard, 1-35 for Premium/Business Critical)')
param backupRetentionDays int

@description('Enable audit logging')
param enableAuditLogging bool

@description('Storage account name for audit logs (required if enableAuditLogging is true)')
param auditStorageAccountName string

@description('GitHub repository URL for Static Web Apps deployment')
param repositoryUrl string

@description('GitHub branch for Static Web Apps deployment')
param repositoryBranch string

@description('GitHub personal access token for Static Web Apps (leave empty for manual configuration)')
@secure()
param githubToken string

// Variables for consistent naming
var resourceGroupName = '${baseName}-rg-${environment}'

// Resource Group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2023-07-01' = {
  name: resourceGroupName
  location: location
  
  tags: {
    Environment: environment
    Project: baseName
    ResourceType: 'ResourceGroup'
  }
}

// Deploy resources using module pattern to target the resource group
module resources 'modules/resources.bicep' = {
  name: 'resources'
  scope: resourceGroup
  params: {
    baseName: baseName
    location: location
    sqlDatabaseTier: sqlDatabaseTier
    sqlDatabaseSku: sqlDatabaseSku
    maxSizeBytes: maxSizeBytes
    sqlAdminUsername: sqlAdminUsername
    sqlAdminPassword: sqlAdminPassword
    environment: environment
    enableAdvancedThreatProtection: enableAdvancedThreatProtection
    backupRetentionDays: backupRetentionDays
    enableAuditLogging: enableAuditLogging
    auditStorageAccountName: auditStorageAccountName
    repositoryUrl: repositoryUrl
    repositoryBranch: repositoryBranch
    githubToken: githubToken
  }
}

// Outputs
output resourceGroupName string = resourceGroup.name
output sqlServerName string = resources.outputs.sqlServerName
output sqlServerFqdn string = resources.outputs.sqlServerFqdn
output sqlDatabaseName string = resources.outputs.sqlDatabaseName
output connectionStringSecretName string = resources.outputs.connectionStringSecretName
output keyVaultName string = resources.outputs.keyVaultName
output sqlServerResourceId string = resources.outputs.sqlServerResourceId
output sqlDatabaseResourceId string = resources.outputs.sqlDatabaseResourceId
output staticWebAppName string = resources.outputs.staticWebAppName
output staticWebAppUrl string = resources.outputs.staticWebAppUrl
output staticWebAppId string = resources.outputs.staticWebAppId
