targetScope = 'subscription'

// Infrastructure as Code for Tech Hub Azure resources.
// Single production environment in rg-techhub-prod.
// PR previews run in the same Container Apps Environment as production.

@description('Azure region for resources')
param location string = 'swedencentral'

@description('Resource group name')
param resourceGroupName string = 'rg-techhub-prod'

@description('Application Insights name')
param appInsightsName string = 'appi-techhub-prod'

@description('Key Vault name (stores wildcard certs, app secrets, and GitHub registry token)')
param keyVaultName string = 'kv-techhub-prod'

@description('Azure AD object IDs for Key Vault administrators')
param keyVaultAdminObjectIds array = []

@description('Container Apps Environment name')
param containerAppsEnvName string = 'cae-techhub-prod'

@description('API Container App name')
param apiAppName string = 'ca-techhub-api-prod'

@description('Web Container App name')
param webAppName string = 'ca-techhub-web-prod'

@description('API Docker image tag (yyyyMMddHHmmss format)')
param apiImageTag string = ''

@description('Web Docker image tag (yyyyMMddHHmmss format)')
param webImageTag string = ''

@description('Azure AD tenant ID for admin dashboard authentication (public Entra identifier)')
param azureAdTenantId string = ''

@description('Azure AD client ID for admin dashboard authentication (public Entra identifier)')
param azureAdClientId string = ''

@description('VNet name')
param vnetName string = 'vnet-techhub-prod'

@description('VNet address space')
param addressSpacePrefix string = '10.2.0.0/16'

@description('Container Apps subnet prefix')
param containerAppsSubnetPrefix string = '10.2.0.0/23'

@description('First IP of the Container Apps subnet (for PostgreSQL firewall rule — Bicep cannot parse CIDR)')
param containerAppsSubnetStartIp string = '10.2.0.0'

@description('Last IP of the Container Apps subnet (for PostgreSQL firewall rule — Bicep cannot parse CIDR)')
param containerAppsSubnetEndIp string = '10.2.1.255'

@description('Primary host names for the web app (e.g. ["tech.hub.ms", "tech.xebia.ms"]). Used for app configuration (CORS, canonical URLs). Leave empty to use default Container Apps FQDN.')
param primaryHosts string[] = []

@description('Wildcard certificate names in Key Vault, keyed by base domain (e.g. { "hub.ms": "wildcard-hub-ms" }). Wildcard custom domain bindings (*.hub.ms) are derived from these keys.')
param wildcardCertNames object = {}

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
param openAiModelCapacity int = 200

@description('Comma-separated admin IP addresses for PostgreSQL and Key Vault firewall rules (e.g. "1.2.3.4,5.6.7.8")')
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

@description('DNS zone name for ACME challenge delegation (used by certbot-dns-azure for wildcard cert renewal)')
param acmeDnsZoneName string = 'acme.hub.ms'

@description('Domains that need ACME-delegated wildcard certificate renewal')
param acmeDelegatedDomains string[] = ['hub.ms', 'xebia.ms']

@description('GitHub organization username for ghcr.io registry (used as the image namespace: ghcr.io/{githubRegistryUsername}/...)')
param githubRegistryUsername string = 'techhubms'

@description('GitHub username of the PAT owner used to authenticate with ghcr.io. Must match the account that created techhub-github-registry-token in Key Vault. Defaults to githubRegistryUsername.')
param githubRegistryAuthUsername string = githubRegistryUsername

@description('Common tags applied to all resources managed by this template')
param commonTags object = {
  owner: 'techhub-maintainer'
  project: 'techhub'
  managedBy: 'bicep'
}

// Tags for all prod resources
var prodTags = union(commonTags, { env: 'prod' })

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

// User-Assigned Managed Identity (used by Container Apps to access Key Vault)
module identity './modules/identity.bicep' = {
  scope: resourceGroup
  name: 'identity-deployment'
  params: {
    location: location
    identityName: prodIdentityName
    tags: prodTags
  }
}

// Networking (VNet + Container Apps subnet with Key Vault service endpoint)
module network './modules/network.bicep' = {
  scope: resourceGroup
  name: 'network-deployment'
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
  name: 'monitoring-deployment'
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

// Key Vault (absorbed from shared RG — stores wildcard certs, app secrets, GitHub registry token)
module keyVault './modules/keyVault.bicep' = {
  scope: resourceGroup
  name: 'keyVault-deployment'
  params: {
    location: location
    vaultName: keyVaultName
    adminObjectIds: keyVaultAdminObjectIds
    logAnalyticsWorkspaceId: monitoring.outputs.logAnalyticsWorkspaceId
    adminIpAddresses: adminIpList
    containerAppsSubnetId: network.outputs.containerAppsSubnetId
    tags: prodTags
  }
}

// Azure AI Foundry (OpenAI) — Container Apps access over public internet (no private endpoint)
module openai './modules/openai.bicep' = {
  scope: resourceGroup
  name: 'openai-deployment'
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
  name: 'containerAppsEnv-deployment'
  params: {
    location: location
    environmentName: containerAppsEnvName
    logAnalyticsWorkspaceId: monitoring.outputs.logAnalyticsWorkspaceId
    infrastructureSubnetId: network.outputs.containerAppsSubnetId
    identityId: identity.outputs.identityId
    tags: prodTags
  }
}

// Wildcard certificates from Key Vault → Container Apps Environment
// The managed identity needs Key Vault Secrets User on the prod KV.
module wildcardCerts './modules/wildcardCertificates.bicep' = if (!empty(wildcardCertNames)) {
  scope: resourceGroup
  name: 'wildcardCerts-prod'
  params: {
    location: location
    containerAppsEnvironmentName: containerAppsEnvName
    keyVaultName: keyVaultName
    wildcardCertNames: wildcardCertNames
    identityId: identity.outputs.identityId
    identityPrincipalId: identity.outputs.identityPrincipalId
  }
  dependsOn: [containerAppsEnv, keyVault]
}

// Build a map of base domain → certificate resource ID for the web module.
// BCP318 warnings are safe: the !empty() guard ensures outputs are only accessed when the module deployed.
#disable-next-line BCP318
var _certDomains = !empty(wildcardCertNames) ? wildcardCerts.outputs.certDomains : []
#disable-next-line BCP318
var _certIds = !empty(wildcardCertNames) ? wildcardCerts.outputs.certIds : []
var wildcardCertIds = toObject(_certDomains, domain => domain, domain => _certIds[indexOf(_certDomains, domain)])

// Wildcard custom domain bindings (*.hub.ms, *.xebia.ms) derived from wildcardCertNames keys.
var allCustomDomains = [for entry in items(wildcardCertNames): '*.${entry.key}']

// Key Vault URI used for Container App KV-reference secrets.
var keyVaultUri = 'https://${keyVaultName}${environment().suffixes.keyvaultDns}/'
var aadClientSecretSecretName = 'techhub-prod-aad-client-secret'

// Passwordless PostgreSQL connection string — the app authenticates with a managed identity token
// (Database:UseEntraAuth=true) so no password is required. The FQDN is resolved from the server output.
var dbConnectionString = 'Host=${postgres.outputs.serverFqdn};Database=${postgres.outputs.databaseName};Username=${prodIdentityName};SSL Mode=Require'

// Grant Key Vault Secrets User to the managed identity on the prod Key Vault.
// Required for Container App KV-reference secrets (AAD client secret, ghcr.io token).
module kvSecretsUserRole './modules/kvSecretsUserRole.bicep' = {
  scope: resourceGroup
  name: 'kvSecretsUserRole-prod'
  params: {
    keyVaultName: keyVaultName
    principalId: identity.outputs.identityPrincipalId
  }
  dependsOn: [keyVault]
}

// PostgreSQL Flexible Server (no private endpoint — firewall allows admin IPs + Container Apps subnet)
module postgres './modules/postgres.bicep' = {
  scope: resourceGroup
  name: 'postgres-deployment'
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
    containerAppsSubnetStartIp: containerAppsSubnetStartIp
    containerAppsSubnetEndIp: containerAppsSubnetEndIp
    // Register the prod managed identity as the Entra admin so the Container App can
    // authenticate with a managed identity token instead of a password.
    entraAdminObjectId: identity.outputs.identityPrincipalId
    entraAdminName: prodIdentityName
    tags: prodTags
  }
}

// Grant Cognitive Services OpenAI User to the prod managed identity on the AI Foundry account.
// This allows the Container App to call the AI Foundry API using a managed identity token
// instead of an API key.
module openAiUserRoleProd './modules/openAiUserRole.bicep' = {
  scope: resourceGroup
  name: 'openAiUserRole-prod-identity'
  params: {
    openAiName: openAiName
    principalId: identity.outputs.identityPrincipalId
    principalType: 'ServicePrincipal'
  }
  dependsOn: [openai]
}

// Grant Cognitive Services OpenAI User to each developer in keyVaultAdminObjectIds so they
// can call the AI Foundry API from their local machine after 'az login'.
module openAiUserRoleDevs './modules/openAiUserRole.bicep' = [for (objectId, i) in keyVaultAdminObjectIds: {
  scope: resourceGroup
  name: 'openAiUserRole-developer-${i}'
  params: {
    openAiName: openAiName
    principalId: objectId
    principalType: 'User'
  }
  dependsOn: [openai]
}]

// API Container App
module apiApp './modules/api.bicep' = {
  scope: resourceGroup
  name: 'api-deployment'
  params: {
    location: location
    containerAppName: apiAppName
    containerAppsEnvironmentId: containerAppsEnv.outputs.environmentId
    githubRegistryUsername: githubRegistryUsername
    githubRegistryAuthUsername: githubRegistryAuthUsername
    identityId: identity.outputs.identityId
    imageTag: apiImageTag
    appInsightsConnectionString: monitoring.outputs.appInsightsConnectionString
    keyVaultUri: keyVaultUri
    dbConnectionString: dbConnectionString
    webFqdns: !empty(primaryHosts) ? primaryHosts : ['${webAppName}.${containerAppsEnv.outputs.defaultDomain}']
    azureAdTenantId: azureAdTenantId
    azureAdClientId: azureAdClientId
    aiCategorizationEndpoint: openai.outputs.openAiEndpoint
    aiCategorizationDeploymentName: openai.outputs.deploymentName
    tags: prodTags
  }
  dependsOn: [kvSecretsUserRole]
}

// Web Container App
module webApp './modules/web.bicep' = {
  scope: resourceGroup
  name: 'web-deployment'
  params: {
    location: location
    containerAppName: webAppName
    containerAppsEnvironmentId: containerAppsEnv.outputs.environmentId
    githubRegistryUsername: githubRegistryUsername
    githubRegistryAuthUsername: githubRegistryAuthUsername
    identityId: identity.outputs.identityId
    imageTag: webImageTag
    apiBaseUrl: apiApp.outputs.fqdn
    appInsightsConnectionString: monitoring.outputs.appInsightsConnectionString
    customDomains: allCustomDomains
    primaryHosts: primaryHosts
    wildcardCertificateIds: wildcardCertIds
    keyVaultUri: keyVaultUri
    aadClientSecretSecretName: aadClientSecretSecretName
    azureAdTenantId: azureAdTenantId
    azureAdClientId: azureAdClientId
    tags: prodTags
  }
  dependsOn: [kvSecretsUserRole]
}

// Shared action group — email notifications for operational alerts (absorbed from shared RG)
module actionGroup './modules/actionGroup.bicep' = {
  scope: resourceGroup
  name: 'actionGroup-deployment'
  params: {
    emailAddress: alertEmailAddress
    tags: prodTags
  }
}

// Operational alerts — use the action group created above
module alerts './modules/alerts.bicep' = {
  scope: resourceGroup
  name: 'alerts-deployment'
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

// Public DNS zone for ACME challenge delegation (absorbed from shared RG)
module acmeDnsZone './modules/acmeDnsZone.bicep' = {
  scope: resourceGroup
  name: 'acmeDnsZone-deployment'
  params: {
    zoneName: acmeDnsZoneName
    delegatedDomains: acmeDelegatedDomains
  }
}

// Subscription cost budget aligned to billing cycle, with 80 / 100 / 120% email alerts.
module budget './modules/budget.bicep' = {
  name: 'budget-deployment'
  params: {
    amount: monthlyBudgetAmount
    contactEmails: [alertEmailAddress]
    startDate: budgetStartDate
  }
}

// Subscription-level Azure Policy assignments (governance baseline).
module policyAssignments './modules/policy.bicep' = {
  name: 'policy-deployment'
  params: {
    allowedLocations: allowedLocations
    requiredTagName: requiredResourceGroupTagName
  }
}

// Outputs
output resourceGroupName string = resourceGroup.name
output apiUrl string = 'https://${apiApp.outputs.fqdn}'
output webUrl string = 'https://${webApp.outputs.fqdn}'
output appInsightsName string = monitoring.outputs.appInsightsName
output keyVaultName string = keyVaultName
output openAiEndpoint string = openai.outputs.openAiEndpoint
output openAiDeploymentName string = openai.outputs.deploymentName
output vnetName string = vnetName
output postgresServerFqdn string = postgres.outputs.serverFqdn
output postgresDatabaseName string = postgres.outputs.databaseName
output acmeDnsZoneName string = acmeDnsZone.outputs.zoneName
output acmeDnsNameServers string[] = acmeDnsZone.outputs.nameServers
output actionGroupId string = actionGroup.outputs.actionGroupId
