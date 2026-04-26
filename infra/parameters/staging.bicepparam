using '../main.bicep'

param location = 'swedencentral'
param environmentName = 'staging'
// apiImageTag and webImageTag are not required for staging — Container Apps are not deployed
// via Bicep for staging. PR environments use ephemeral Container Apps via Deploy-PrPreview.ps1.
param resourceGroupName = 'rg-techhub-staging'
param appInsightsName = 'appi-techhub-staging'
param containerRegistryName = 'crtechhubms'
param containerAppsEnvName = 'cae-techhub-staging'
// Networking (10.1.x range — must not overlap with hub 10.100.x or prod 10.2.x)
param vnetName = 'vnet-techhub-staging'
param addressSpacePrefix = '10.1.0.0/16'
param containerAppsSubnetPrefix = '10.1.0.0/23'
param privateEndpointsSubnetPrefix = '10.1.2.0/24'
// Hub VNet (for peering — private endpoint resolution across environments)
param hubVnetId = '/subscriptions/bc8ab567-c645-4e51-9317-992203eb369a/resourceGroups/rg-techhub-shared/providers/Microsoft.Network/virtualNetworks/vnet-techhub-hub'
param hubVnetName = 'vnet-techhub-hub'
// No custom domains or availability tests for PR-env infrastructure.
// PR environments are ephemeral — E2E tests serve as validation, not synthetic monitoring.
param primaryHosts = []
param wildcardCertNames = {}
// Azure AI Foundry (OpenAI) — used by PR environment Container Apps for AI categorization.
param openAiName = 'oai-techhub-staging'
// Admin IP allow-list — grants firewall access to Key Vault.
// MUST be set via ADMIN_IP_ADDRESSES env var — no default to prevent leaking IPs into git.
param adminIpAddresses = readEnvironmentVariable('ADMIN_IP_ADDRESSES')
// Shared action group resource ID — always empty for staging (no operational alerts).
// PR environments rely on E2E tests, not synthetic monitoring.
param actionGroupId = ''

