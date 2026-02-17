param location string
param openAiName string

@description('SKU for OpenAI service')
@allowed(['S0'])
param sku string = 'S0'

@description('GPT model deployment name')
param deploymentName string = 'gpt-4.1'

@description('GPT model name')
param modelName string = 'gpt-4.1'

@description('GPT model version')
param modelVersion string = '2025-04-14'

@description('Model capacity (TPM in thousands)')
@minValue(1)
@maxValue(1000)
param modelCapacity int = 100

// Azure OpenAI Account
resource openAiAccount 'Microsoft.CognitiveServices/accounts@2025-06-01' = {
  name: openAiName
  location: location
  sku: {
    name: sku
  }
  kind: 'OpenAI'
  properties: {
    customSubDomainName: openAiName
    publicNetworkAccess: 'Enabled'
    networkAcls: {
      defaultAction: 'Allow'
      virtualNetworkRules: []
      ipRules: []
    }
    allowProjectManagement: false
  }
}

// Content Safety Policy (Microsoft DefaultV2)
resource contentSafetyPolicy 'Microsoft.CognitiveServices/accounts/raiPolicies@2025-06-01' = {
  parent: openAiAccount
  name: 'Microsoft.DefaultV2'
  properties: {
    mode: 'Blocking'
    contentFilters: [
      {
        name: 'Hate'
        severityThreshold: 'Medium'
        blocking: true
        enabled: true
        source: 'Prompt'
      }
      {
        name: 'Hate'
        severityThreshold: 'Medium'
        blocking: true
        enabled: true
        source: 'Completion'
      }
      {
        name: 'Sexual'
        severityThreshold: 'Medium'
        blocking: true
        enabled: true
        source: 'Prompt'
      }
      {
        name: 'Sexual'
        severityThreshold: 'Medium'
        blocking: true
        enabled: true
        source: 'Completion'
      }
      {
        name: 'Violence'
        severityThreshold: 'Medium'
        blocking: true
        enabled: true
        source: 'Prompt'
      }
      {
        name: 'Violence'
        severityThreshold: 'Medium'
        blocking: true
        enabled: true
        source: 'Completion'
      }
      {
        name: 'Selfharm'
        severityThreshold: 'Medium'
        blocking: true
        enabled: true
        source: 'Prompt'
      }
      {
        name: 'Selfharm'
        severityThreshold: 'Medium'
        blocking: true
        enabled: true
        source: 'Completion'
      }
      {
        name: 'Jailbreak'
        blocking: true
        enabled: true
        source: 'Prompt'
      }
      {
        name: 'Protected Material Text'
        blocking: true
        enabled: true
        source: 'Completion'
      }
      {
        name: 'Protected Material Code'
        blocking: false
        enabled: true
        source: 'Completion'
      }
    ]
  }
}

// GPT Model Deployment
resource modelDeployment 'Microsoft.CognitiveServices/accounts/deployments@2025-06-01' = {
  parent: openAiAccount
  name: deploymentName
  sku: {
    name: 'DataZoneStandard'
    capacity: modelCapacity
  }
  properties: {
    model: {
      format: 'OpenAI'
      name: modelName
      version: modelVersion
    }
    versionUpgradeOption: 'OnceNewDefaultVersionAvailable'
    raiPolicyName: contentSafetyPolicy.name
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
