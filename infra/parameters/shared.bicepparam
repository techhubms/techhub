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
// Set ADMIN_IP_ADDRESSES env var or GitHub Actions secret to override.
param adminIpAddresses = readEnvironmentVariable('ADMIN_IP_ADDRESSES', '86.89.119.3')
// ACME DNS zone for automated wildcard certificate renewal via certbot-dns-azure.
// External DNS (GoDaddy) delegates _acme-challenge CNAMEs to this zone.
param acmeDnsZoneName = 'acme.hub.ms'
param acmeDelegatedDomains = ['hub.ms', 'xebia.ms']
