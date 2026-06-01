param emailServiceName string
param customEmailDomain string
param communicationServiceName string
@description('Set to true only after the custom domain DNS records have been added and verified in ACS.')
param linkEmailDomain bool = false
param tags object = {}

resource emailService 'Microsoft.Communication/emailServices@2026-03-18' = {
  name: emailServiceName
  location: 'global'
  tags: tags
  properties: {
    dataLocation: 'Europe'
  }
}

resource domain 'Microsoft.Communication/emailServices/domains@2026-03-18' = {
  parent: emailService
  name: customEmailDomain
  location: 'global'
  properties: {
    domainManagement: 'CustomerManaged'
    userEngagementTracking: 'Disabled'
  }
}

resource newsletterSenderUsername 'Microsoft.Communication/emailServices/domains/senderUsernames@2026-03-18' = {
  parent: domain
  name: 'newsletter'
  properties: {
    username: 'newsletter'
    displayName: 'TechHub Newsletter'
  }
}

resource communicationService 'Microsoft.Communication/communicationServices@2026-03-18' = {
  name: communicationServiceName
  location: 'global'
  tags: tags
  properties: {
    dataLocation: 'Europe'
    linkedDomains: linkEmailDomain ? [domain.id] : []
  }
}

output communicationServiceEndpoint string = 'https://${communicationService.name}.communication.azure.com/'
output communicationServiceId string = communicationService.id
output senderAddress string = 'newsletter@${domain.properties.mailFromSenderDomain}'
