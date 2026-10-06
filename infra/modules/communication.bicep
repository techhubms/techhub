param emailServiceName string
param customEmailDomain string
param communicationServiceName string
@description('Set to true only after the custom domain DNS records have been added and verified in ACS.')
param linkEmailDomain bool = false
@description('Create the custom domain. Must be false once it exists: every PUT resets its DNS verification state, which then breaks linking it to ACS.')
param createEmailDomain bool = true
param tags object = {}

resource emailService 'Microsoft.Communication/emailServices@2026-03-18' = {
  name: emailServiceName
  location: 'global'
  tags: tags
  properties: {
    dataLocation: 'Europe'
  }
}

resource domain 'Microsoft.Communication/emailServices/domains@2026-03-18' = if (createEmailDomain) {
  parent: emailService
  name: customEmailDomain
  location: 'global'
  properties: {
    domainManagement: 'CustomerManaged'
    userEngagementTracking: 'Disabled'
  }
}

resource domainRef 'Microsoft.Communication/emailServices/domains@2026-03-18' existing = {
  parent: emailService
  name: customEmailDomain
}

resource newsletterSenderUsername 'Microsoft.Communication/emailServices/domains/senderUsernames@2026-03-18' = {
  parent: domainRef
  name: 'newsletter'
  properties: {
    username: 'newsletter'
    displayName: 'TechHub Newsletter'
  }
  dependsOn: [domain]
}

resource communicationService 'Microsoft.Communication/communicationServices@2026-03-18' = {
  name: communicationServiceName
  location: 'global'
  tags: tags
  properties: {
    dataLocation: 'Europe'
    linkedDomains: linkEmailDomain ? [domainRef.id] : []
  }
  dependsOn: [domain]
}

output communicationServiceEndpoint string = 'https://${communicationService.name}.communication.azure.com/'
output communicationServiceId string = communicationService.id
output senderAddress string = 'newsletter@${domainRef.properties.mailFromSenderDomain}'
