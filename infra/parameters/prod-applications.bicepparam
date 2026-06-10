using '../applications.bicep'

param location = 'swedencentral'
param resourceGroupName = 'rg-techhub-prod'
param appInsightsName = 'appi-techhub-prod'
param keyVaultName = 'kv-techhub-prod'
param containerAppsEnvName = 'cae-techhub-prod'
param apiAppName = 'ca-techhub-api-prod'
param webAppName = 'ca-techhub-web-prod'
// Image tags — injected via environment variables by Deploy-Infrastructure.ps1
param apiImageTag = readEnvironmentVariable('API_IMAGE_TAG')
param webImageTag = readEnvironmentVariable('WEB_IMAGE_TAG')
// Custom domains
param primaryHosts = ['tech.hub.ms', 'tech.xebia.ms']
param wildcardCertNames = {
  'hub.ms': 'wildcard-hub-ms'
  'xebia.ms': 'wildcard-xebia-ms'
}
// PostgreSQL server name (for existing resource lookup)
param postgresServerName = 'psql-techhub-prod'
// Azure AI Foundry (OpenAI) resource name (for existing resource lookup)
param openAiName = 'oai-techhub-prod'
// Azure AD — admin dashboard authentication
// Tenant ID and Client ID are public Entra identifiers (issuer URL contains tenant ID).
// The actual Client Secret lives in Key Vault — see scripts/Sync-KeyVaultSecrets.ps1.
param azureAdTenantId = '459df0d4-819b-42e9-9ff4-f9ddf3065c0c'
param azureAdClientId = '03886f87-2316-4ee8-ba3b-353cfcb43f4c'
// GitHub Container Registry
param githubRegistryUsername = 'techhubms'
// Telemetry — set to empty string in PR preview environments to prevent data from
// appearing in production dashboards.
param googleAnalyticsMeasurementId = 'G-95LLB67KJV'
// appInsightsConnectionString is left at its default ('@existing') so the deploy
// script reads the live connection string from the Azure resource at deployment time.
