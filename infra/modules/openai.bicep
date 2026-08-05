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

@description('Admin IP addresses for firewall rules (optional — leave empty to keep public access disabled)')
param adminIpAddresses string[] = []

@description('Subnet ID for the AI Foundry private endpoint. The private endpoint is only created when both this and privateDnsZoneId are non-empty; supplying only one silently skips creation.')
param privateEndpointSubnetId string = ''

@description('Private DNS zone ID for privatelink.openai.azure.com — resolves the endpoint the app actually calls (<account>.openai.azure.com). The private endpoint is only created when both this and privateEndpointSubnetId are non-empty; supplying only one silently skips creation.')
param privateDnsZoneId string = ''

@description('Private DNS zone ID for privatelink.cognitiveservices.azure.com. Optional — the account is kind "AIServices" (multi-service) and Microsoft recommends registering all three zones on the same private endpoint, but only privateDnsZoneId is required for the app to function.')
param cognitiveServicesPrivateDnsZoneId string = ''

@description('Private DNS zone ID for privatelink.services.ai.azure.com. Optional — the third zone Microsoft documents for Microsoft.CognitiveServices/accounts private endpoints (subresource "account"), used by newer AI Foundry project/agent APIs.')
param servicesAiPrivateDnsZoneId string = ''

@description('Tags applied to the AI Foundry account')
param tags object = {}

var deployPrivateEndpoint = !empty(privateEndpointSubnetId) && !empty(privateDnsZoneId)
var deployCognitiveServicesDnsZoneConfig = !empty(cognitiveServicesPrivateDnsZoneId)
var deployServicesAiDnsZoneConfig = !empty(servicesAiPrivateDnsZoneId)

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
    // Access is secured by the Cognitive Services OpenAI User RBAC role assigned to
    // id-techhub-prod (Container App managed identity) and to developer accounts.
    // Container Apps acquire an Entra token (cognitiveservices.azure.com scope) at runtime.
    disableLocalAuth: true
    // Public access is only needed for admin IP allowlisting — Container Apps reach AI Foundry
    // over the private endpoint below, not the public endpoint.
    publicNetworkAccess: !empty(adminIpAddresses) ? 'Enabled' : 'Disabled'
    networkAcls: {
      defaultAction: 'Deny'
      bypass: 'None'
      ipRules: [for ip in adminIpAddresses: { value: ip }]
    }
  }
}

// Private endpoint — gives Container Apps a private IP path to AI Foundry, removing the need
// for the fully-open public endpoint that previously had no IP restriction at all.
resource openAiPrivateEndpoint 'Microsoft.Network/privateEndpoints@2024-05-01' = if (deployPrivateEndpoint) {
  name: 'pe-${openAiName}'
  location: location
  tags: tags
  properties: {
    subnet: {
      id: privateEndpointSubnetId
    }
    privateLinkServiceConnections: [
      {
        name: 'pe-${openAiName}-connection'
        properties: {
          privateLinkServiceId: openAiAccount.id
          groupIds: ['account']
        }
      }
    ]
  }
}

// The account is kind 'AIServices' (multi-service — "Foundry Tools" in Microsoft's private
// endpoint DNS reference), so openai.azure.com, cognitiveservices.azure.com, and
// services.ai.azure.com domains can all resolve to this endpoint depending on which API is
// called. The app itself only calls <account>.openai.azure.com (see AiCategorizationOptions),
// so privateDnsZoneId is required; the other two zone IDs are optional but recommended so any
// Cognitive Services- or AI Foundry project/agent-style calls also resolve privately.
resource openAiPrivateDnsZoneGroup 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2024-05-01' = if (deployPrivateEndpoint) {
  parent: openAiPrivateEndpoint
  name: 'default'
  properties: {
    privateDnsZoneConfigs: concat(
      [
        {
          name: 'openai-config'
          properties: {
            privateDnsZoneId: privateDnsZoneId
          }
        }
      ],
      deployCognitiveServicesDnsZoneConfig
        ? [
            {
              name: 'cognitiveservices-config'
              properties: {
                privateDnsZoneId: cognitiveServicesPrivateDnsZoneId
              }
            }
          ]
        : [],
      deployServicesAiDnsZoneConfig
        ? [
            {
              name: 'servicesai-config'
              properties: {
                privateDnsZoneId: servicesAiPrivateDnsZoneId
              }
            }
          ]
        : []
    )
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
output deploymentName string = modelDeployment.name
