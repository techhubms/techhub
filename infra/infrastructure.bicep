targetScope = 'subscription'

// Phase 1: Base infrastructure for Tech Hub production.
// Deploys networking, identity, Key Vault, PostgreSQL, OpenAI, monitoring,
// Container Apps Environment, and governance resources.
// Run this before applications.bicep — it creates the Key Vault that must
// exist before application secrets can be synced.

@description('Azure region for resources')
param location string = 'swedencentral'

@description('Resource group name')
param resourceGroupName string = 'rg-techhub-prod'

@description('Application Insights name')
param appInsightsName string = 'appi-techhub-prod'

@description('Key Vault name (stores wildcard certs, app secrets, and GitHub registry token)')
param keyVaultName string = 'kv-techhub-prod'

@description('Container Apps Environment name')
param containerAppsEnvName string = 'cae-techhub-prod'

@description('VNet name')
param vnetName string = 'vnet-techhub-prod'

@description('VNet address space')
param addressSpacePrefix string = '10.2.0.0/16'

@description('Container Apps subnet prefix')
param containerAppsSubnetPrefix string = '10.2.0.0/23'

@description('Primary host names for the web app. Used for Application Insights availability tests.')
param primaryHosts string[] = []

@description('PostgreSQL server name')
param postgresServerName string = 'psql-techhub-prod'

@description('PostgreSQL administrator login')
param postgresAdminLogin string = 'techhubadmin'

@secure()
@description('PostgreSQL administrator password')
param postgresAdminPassword string = ''

@description('Azure AI Foundry (OpenAI) resource name')
param openAiName string = 'oai-techhub-prod'

@description('Azure AI Foundry model capacity (TPM in thousands)')
param openAiModelCapacity int = 100

@description('Comma-separated admin IP addresses for PostgreSQL and Key Vault firewall rules')
@minLength(7)
param adminIpAddresses string

@description('Email address for operational alerts and budget notifications')
param alertEmailAddress string = 'reinier.vanmaanen@xebia.com'

@description('Monthly budget amount (in billing currency, typically EUR)')
param monthlyBudgetAmount int = 250

@description('Budget start date (YYYY-MM-DD, aligned to billing period start)')
param budgetStartDate string = '2026-04-01'

@description('Azure Policy: allowed deployment locations')
param allowedLocations string[] = ['swedencentral', 'westeurope', 'global']

@description('Tag name required on resource groups (enforced by Azure Policy)')
param requiredResourceGroupTagName string = 'owner'

@description('DNS zone name for ACME challenge delegation')
param acmeDnsZoneName string = 'acme.hub.ms'

@description('Domains that need ACME-delegated wildcard certificate renewal')
param acmeDelegatedDomains string[] = ['hub.ms', 'xebia.ms']

@description('Email Communication Service name')
param emailServiceName string = 'eml-techhub-prod'

@description('Communication Service name')
param communicationServiceName string = 'acs-techhub-prod'

@description('Custom email sending domain linked to ACS (e.g. mail.hub.ms). Must be verified in ACS after deploy.')
param customEmailDomain string = 'mail.hub.ms'

@description('Set to true only after the custom domain DNS records have been added and verified in ACS. Redeploy after verification.')
param linkEmailDomain bool = false

@description('Common tags applied to all resources managed by this template')
param commonTags object = {
  owner: 'techhub-maintainer'
  project: 'techhub'
  managedBy: 'bicep'
}

@description('UTC timestamp used to make nested deployment names unique per run.')
param deploymentTimestamp string = utcNow()

// Tags for all prod resources
var prodTags = union(commonTags, { env: 'prod' })

// Short unique hash per run — appended to all nested module `name` values to prevent DeploymentActive conflicts
var deploymentSuffix = uniqueString(deploymentTimestamp)

// Parse comma-separated admin IPs into a trimmed, filtered array
var adminIpList = [for ip in filter(split(adminIpAddresses, ','), entry => !empty(trim(entry))): trim(ip)]

// Resource Group
resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: resourceGroupName
  location: location
  tags: prodTags
}

// Managed identity name — shared between the identity resource, PostgreSQL Entra admin registration,
// and the passwordless connection string so they always stay in sync.
var prodIdentityName = 'id-techhub-prod'

// User-Assigned Managed Identity (used by Container Apps to access Key Vault and PostgreSQL)
module identity './modules/identity.bicep' = {
  scope: resourceGroup
  name: 'identity-${deploymentSuffix}'
  params: {
    location: location
    identityName: prodIdentityName
    tags: prodTags
  }
}

// Networking (VNet + Container Apps subnet with Key Vault service endpoint)
module network './modules/network.bicep' = {
  scope: resourceGroup
  name: 'network-${deploymentSuffix}'
  params: {
    location: location
    vnetName: vnetName
    addressSpacePrefix: addressSpacePrefix
    containerAppsSubnetPrefix: containerAppsSubnetPrefix
    tags: prodTags
  }
}

// Monitoring (Application Insights + Log Analytics)
module monitoring './modules/monitoring.bicep' = {
  scope: resourceGroup
  name: 'monitoring-${deploymentSuffix}'
  params: {
    location: location
    appInsightsName: appInsightsName
    logAnalyticsWorkspaceName: 'law-techhub-prod'
    dailyQuotaGb: -1
    appInsightsRetentionInDays: 30
    availabilityTestHosts: primaryHosts
    enableSmartDetection: true
    tags: prodTags
  }
}

// Key Vault (stores wildcard certs, app secrets, GitHub registry token)
module keyVault './modules/keyVault.bicep' = {
  scope: resourceGroup
  name: 'keyVault-${deploymentSuffix}'
  params: {
    location: location
    vaultName: keyVaultName
    logAnalyticsWorkspaceId: monitoring.outputs.logAnalyticsWorkspaceId
    adminIpAddresses: adminIpList
    containerAppsSubnetId: network.outputs.containerAppsSubnetId
    tags: prodTags
  }
}

// Azure AI Foundry (OpenAI)
module openai './modules/openai.bicep' = {
  scope: resourceGroup
  name: 'openai-${deploymentSuffix}'
  params: {
    location: location
    openAiName: openAiName
    modelCapacity: openAiModelCapacity
    tags: prodTags
  }
}

// Container Apps Environment (VNet-integrated)
module containerAppsEnv './modules/containerApps.bicep' = {
  scope: resourceGroup
  name: 'containerAppsEnv-${deploymentSuffix}'
  params: {
    location: location
    environmentName: containerAppsEnvName
    logAnalyticsWorkspaceId: monitoring.outputs.logAnalyticsWorkspaceId
    infrastructureSubnetId: network.outputs.containerAppsSubnetId
    identityId: identity.outputs.identityId
    tags: prodTags
  }
}

// Grant Key Vault Secrets User to the managed identity on the prod Key Vault
module kvSecretsUserRole './modules/kvSecretsUserRole.bicep' = {
  scope: resourceGroup
  name: 'kvSecretsUserRole-${deploymentSuffix}'
  params: {
    keyVaultName: keyVaultName
    principalId: identity.outputs.identityPrincipalId
  }
  dependsOn: [keyVault]
}

// Shared managed identity for PR preview Container Apps.
// All PR environments reuse this single identity — no per-PR identity lifecycle needed.
// infrastructure.bicep owns this resource; Deploy-PrPreview.ps1 only reads it.
var prIdentityName = 'id-techhub-pr'

module prIdentity './modules/identity.bicep' = {
  scope: resourceGroup
  name: 'pr-identity-${deploymentSuffix}'
  params: {
    location: location
    identityName: prIdentityName
    tags: union(commonTags, { env: 'pr' })
  }
}

// Grant Key Vault Secrets User to the shared PR identity so PR Container Apps can
// resolve the ghcr-token KV secret reference for image pull authentication.
module kvSecretsUserRolePr './modules/kvSecretsUserRole.bicep' = {
  scope: resourceGroup
  name: 'kvSecretsUserRole-pr-${deploymentSuffix}'
  params: {
    keyVaultName: keyVaultName
    principalId: prIdentity.outputs.identityPrincipalId
  }
  dependsOn: [keyVault]
}

// PostgreSQL Flexible Server
module postgres './modules/postgres.bicep' = {
  scope: resourceGroup
  name: 'postgres-${deploymentSuffix}'
  params: {
    location: location
    serverName: postgresServerName
    administratorLogin: postgresAdminLogin
    administratorLoginPassword: postgresAdminPassword
    skuName: 'Standard_B1ms'
    skuTier: 'Burstable'
    backupRetentionDays: 21
    geoRedundantBackup: true
    adminIpAddresses: adminIpList
    // Container Apps routes outbound traffic through the NAT Gateway, which has a single
    // stable public IP. PostgreSQL's firewall must allowlist this IP.
    containerAppsNatGatewayIp: network.outputs.natGatewayPublicIp
    entraAdminObjectId: identity.outputs.identityPrincipalId
    entraAdminName: prodIdentityName
    tags: prodTags
  }
}

// Grant Cognitive Services OpenAI User to the prod managed identity
module openAiUserRoleProd './modules/openAiUserRole.bicep' = {
  scope: resourceGroup
  name: 'openAiUserRole-${deploymentSuffix}'
  params: {
    openAiName: openAiName
    principalId: identity.outputs.identityPrincipalId
    principalType: 'ServicePrincipal'
  }
  dependsOn: [openai]
}

// Shared action group — email notifications for operational alerts
module actionGroup './modules/actionGroup.bicep' = {
  scope: resourceGroup
  name: 'actionGroup-${deploymentSuffix}'
  params: {
    emailAddress: alertEmailAddress
    tags: prodTags
  }
}

// Operational alerts
module alerts './modules/alerts.bicep' = {
  scope: resourceGroup
  name: 'alerts-${deploymentSuffix}'
  params: {
    location: location
    environmentName: 'prod'
    actionGroupId: actionGroup.outputs.actionGroupId
    appInsightsId: monitoring.outputs.appInsightsId
    logAnalyticsWorkspaceId: monitoring.outputs.logAnalyticsWorkspaceId
    postgresServerId: postgres.outputs.serverId
    openAiAccountId: openai.outputs.openAiId
    tags: prodTags
  }
}

// Public DNS zone for ACME challenge delegation
module acmeDnsZone './modules/acmeDnsZone.bicep' = {
  scope: resourceGroup
  name: 'acmeDnsZone-${deploymentSuffix}'
  params: {
    zoneName: acmeDnsZoneName
    delegatedDomains: acmeDelegatedDomains
  }
}

// Azure Communication Services for email delivery
module communication './modules/communication.bicep' = {
  scope: resourceGroup
  name: 'communication-${deploymentSuffix}'
  params: {
    emailServiceName: emailServiceName
    customEmailDomain: customEmailDomain
    linkEmailDomain: linkEmailDomain
    communicationServiceName: communicationServiceName
    tags: prodTags
  }
}

// Grant API managed identity access to ACS for sending emails
module acsDataContributorRole './modules/acsDataContributorRole.bicep' = {
  scope: resourceGroup
  name: 'acs-data-contributor-role-${deploymentSuffix}'
  params: {
    communicationServiceName: communicationServiceName
    principalId: identity.outputs.identityPrincipalId
    principalType: 'ServicePrincipal'
  }
  dependsOn: [communication]
}

// Store ACS sender address in Key Vault for Phase 2 consumption
module acsSenderAddressSecret './modules/keyVaultSecret.bicep' = {
  scope: resourceGroup
  name: 'acs-sender-address-secret-${deploymentSuffix}'
  params: {
    keyVaultName: keyVaultName
    secretName: 'techhub-prod-acs-sender-address'
    secretValue: communication.outputs.senderAddress
  }
  dependsOn: [keyVault]
}

// Subscription cost budget
module budget './modules/budget.bicep' = {
  name: 'budget-${deploymentSuffix}'
  params: {
    amount: monthlyBudgetAmount
    contactEmails: [alertEmailAddress]
    startDate: budgetStartDate
  }
}

// Subscription-level Azure Policy assignments
module policyAssignments './modules/policy.bicep' = {
  name: 'policy-${deploymentSuffix}'
  params: {
    allowedLocations: allowedLocations
    requiredTagName: requiredResourceGroupTagName
  }
}

// Outputs
output resourceGroupName string = resourceGroup.name
output appInsightsName string = monitoring.outputs.appInsightsName
output keyVaultName string = keyVaultName
output openAiEndpoint string = openai.outputs.openAiEndpoint
output openAiDeploymentName string = openai.outputs.deploymentName
output vnetName string = vnetName
output postgresServerFqdn string = postgres.outputs.serverFqdn
output postgresDatabaseName string = postgres.outputs.databaseName
output acmeDnsZoneName string = acmeDnsZone.outputs.zoneName
output acmeDnsNameServers string[] = acmeDnsZone.outputs.nameServers
output acsEndpoint string = communication.outputs.communicationServiceEndpoint
