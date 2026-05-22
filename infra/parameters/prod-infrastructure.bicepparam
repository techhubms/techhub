using '../infrastructure.bicep'

param location = 'swedencentral'
param resourceGroupName = 'rg-techhub-prod'
param appInsightsName = 'appi-techhub-prod'
param keyVaultName = 'kv-techhub-prod'
param containerAppsEnvName = 'cae-techhub-prod'
// Networking
param vnetName = 'vnet-techhub-prod'
param addressSpacePrefix = '10.2.0.0/16'
param containerAppsSubnetPrefix = '10.2.0.0/23'
// Container Apps subnet IP range for PostgreSQL firewall rule (Bicep cannot parse CIDR).
// These must match containerAppsSubnetPrefix = '10.2.0.0/23'. Update all three if the subnet changes.
// Also update the matching constants in scripts/Deploy-PrPreview.ps1 if the subnet changes.
param containerAppsSubnetStartIp = '10.2.0.0'
param containerAppsSubnetEndIp = '10.2.1.255'
// Availability tests — same hosts as the production web app
param primaryHosts = ['tech.hub.ms', 'tech.xebia.ms']
// PostgreSQL configuration
param postgresServerName = 'psql-techhub-prod'
param postgresAdminLogin = 'techhubadmin'
param postgresAdminPassword = readEnvironmentVariable('POSTGRES_ADMIN_PASSWORD')
// Azure AI Foundry (OpenAI)
param openAiName = 'oai-techhub-prod'
param openAiModelCapacity = 200
// Admin IP allow-list — grants firewall access to PostgreSQL and Key Vault.
// MUST be set via ADMIN_IP_ADDRESSES env var — no default to prevent leaking IPs into git.
param adminIpAddresses = readEnvironmentVariable('ADMIN_IP_ADDRESSES')
// Alerts and budget
param alertEmailAddress = 'reinier.vanmaanen@xebia.com'
param monthlyBudgetAmount = 250
param budgetStartDate = '2026-04-01'
