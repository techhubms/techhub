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

// Azure AI Foundry Account (AIServices)
resource openAiAccount 'Microsoft.CognitiveServices/accounts@2025-06-01' = {
  name: openAiName
  location: location
  sku: {
    name: sku
  }
  kind: 'AIServices'
  properties: {
    customSubDomainName: openAiName
    // Must remain public: content processing scripts run from GitHub Actions runners
    // which have dynamic IPs. Authentication is via API key.
    publicNetworkAccess: 'Enabled'
    networkAcls: {
      defaultAction: 'Allow'
      virtualNetworkRules: []
      ipRules: []
    }
    allowProjectManagement: false
  }
}

// Note: AIServices kind includes Microsoft.DefaultV2 content safety policy by default.
// No need to explicitly create it - it's a system-managed policy.

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
    raiPolicyName: 'Microsoft.DefaultV2'
  }
}

// Defender for AI (disabled by default)
resource defenderForAI 'Microsoft.CognitiveServices/accounts/defenderForAISettings@2025-06-01' = {
  parent: openAiAccount
  name: 'Default'
  properties: {
    state: 'Disabled'
  }
}

// Outputs
output openAiName string = openAiAccount.name
output openAiEndpoint string = openAiAccount.properties.endpoint
output openAiId string = openAiAccount.id
output deploymentName string = modelDeployment.name
