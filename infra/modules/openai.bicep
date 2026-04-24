param location string
param openAiName string

@description('SKU for AI Foundry service')
@allowed(['S0'])
param sku string = 'S0'

@description('GPT model deployment name')
param deploymentName string = 'gpt-5.2'

@description('GPT model name')
param modelName string = 'gpt-5.2'

@description('GPT model version')
param modelVersion string = '2025-12-11'

@description('Model capacity (TPM in thousands)')
@minValue(1)
@maxValue(1000)
param modelCapacity int = 100

@description('Tags applied to the AI Foundry account')
param tags object = {}

// Azure AI Foundry Account (AIServices)
resource openAiAccount 'Microsoft.CognitiveServices/accounts@2025-06-01' = {
  name: openAiName
  location: location
  tags: tags
  sku: {
    name: sku
  }
  kind: 'AIServices'
  properties: {
    customSubDomainName: openAiName
    // Public access is required for admin operations and is restricted via NSP association.
    // Container Apps access AI Foundry through the private endpoint in the spoke VNet.
    publicNetworkAccess: 'Enabled'
  }
}

// Custom RAI (content filter) policy for content categorization.
// The default Microsoft.DefaultV2 policy is too strict for processing tech articles,
// causing false-positive content filter rejections. This policy uses 'High' severity
// thresholds so only the most severe content is blocked — appropriate for automated
// ingestion of published tech articles that are not user-generated.
resource raiPolicy 'Microsoft.CognitiveServices/accounts/raiPolicies@2025-06-01' = {
  parent: openAiAccount
  name: 'content-categorization'
  properties: {
    mode: 'Blocking'
    basePolicyName: 'Microsoft.DefaultV2'
    contentFilters: [
      { name: 'hate',     blocking: true, enabled: true, severityThreshold: 'High', source: 'Prompt' }
      { name: 'sexual',   blocking: true, enabled: true, severityThreshold: 'High', source: 'Prompt' }
      { name: 'selfharm', blocking: true, enabled: true, severityThreshold: 'High', source: 'Prompt' }
      { name: 'violence', blocking: true, enabled: true, severityThreshold: 'High', source: 'Prompt' }
      { name: 'hate',     blocking: true, enabled: true, severityThreshold: 'High', source: 'Completion' }
      { name: 'sexual',   blocking: true, enabled: true, severityThreshold: 'High', source: 'Completion' }
      { name: 'selfharm', blocking: true, enabled: true, severityThreshold: 'High', source: 'Completion' }
      { name: 'violence', blocking: true, enabled: true, severityThreshold: 'High', source: 'Completion' }
    ]
  }
}

// GPT Model Deployment
resource modelDeployment 'Microsoft.CognitiveServices/accounts/deployments@2025-06-01' = {
  parent: openAiAccount
  name: deploymentName
  sku: {
    name: 'GlobalStandard'
    capacity: modelCapacity
  }
  properties: {
    model: {
      format: 'OpenAI'
      name: modelName
      version: modelVersion
    }
    versionUpgradeOption: 'OnceNewDefaultVersionAvailable'
    raiPolicyName: raiPolicy.name
  }
}

// Note: Defender for AI settings are managed by Azure Policy / portal.
// Explicitly deploying defenderForAISettings via ARM causes validation errors (715-123420).

// Outputs
output openAiName string = openAiAccount.name
output openAiEndpoint string = openAiAccount.properties.endpoint
output openAiId string = openAiAccount.id
@secure()
output openAiApiKey string = openAiAccount.listKeys().key1
output deploymentName string = modelDeployment.name
