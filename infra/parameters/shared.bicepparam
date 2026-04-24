using '../shared.bicep'

param location = 'westeurope'
param resourceGroupName = 'rg-techhub-shared'
param containerRegistryName = 'crtechhubms'
param keyVaultName = 'kv-techhub-shared'
// Add your Azure AD object ID(s) here for Key Vault admin access.
// Find yours with: az ad signed-in-user show --query id -o tsv
// Override in environment-specific parameter files or local overrides.
param keyVaultAdminObjectIds = []
param hubVnetName = 'vnet-techhub-hub'
// Comma-separated admin IP addresses for Key Vault and PostgreSQL firewall.
// MUST be set via ADMIN_IP_ADDRESSES env var or GitHub Actions secret — no default to prevent leaking IPs into git.
param adminIpAddresses = readEnvironmentVariable('ADMIN_IP_ADDRESSES')
// ACME DNS zone for automated wildcard certificate renewal via certbot-dns-azure.
// External DNS (GoDaddy) delegates _acme-challenge CNAMEs to this zone.
param acmeDnsZoneName = 'acme.hub.ms'
param acmeDelegatedDomains = ['hub.ms', 'xebia.ms']
// Monthly budget alert — tracks calendar months (not the Apr 7–May 6 billing cycle,
// which Azure budgets cannot match). Alerts at 80% / 100% / 120%.
param monthlyBudgetAmount = 150
param budgetStartDate = '2026-04-01'
