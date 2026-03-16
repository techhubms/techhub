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
param vpnGatewayName = 'vpng-techhub'
// Uses the Microsoft-registered App ID for VPN auth — no manual app registration needed.
// Override with a custom audience value only if you need user/group-based access control.
param vpnAadAudienceAppId = 'c632b3df-fb67-4d84-bdcf-b95ad541b5c8'
// ACME DNS zone for automated wildcard certificate renewal via certbot-dns-azure.
// External DNS (GoDaddy) delegates _acme-challenge CNAMEs to this zone.
param acmeDnsZoneName = 'acme.hub.ms'
param acmeDelegatedDomains = ['hub.ms', 'xebia.ms']
