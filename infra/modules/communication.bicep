param emailServiceName string
param managedDomainName string = 'AzureManagedDomain'
param communicationServiceName string
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
  name: managedDomainName
  location: 'global'
  properties: {
    domainManagement: 'AzureManaged'
    userEngagementTracking: 'Disabled'
  }
}

resource communicationService 'Microsoft.Communication/communicationServices@2026-03-18' = {
  name: communicationServiceName
  location: 'global'
  tags: tags
  properties: {
    dataLocation: 'Europe'
    linkedDomains: [
      domain.id
    ]
  }
}

output communicationServiceEndpoint string = 'https://${communicationService.name}.communication.azure.com/'
output communicationServiceId string = communicationService.id
output senderAddress string = 'DoNotReply@${domain.properties.mailFromSenderDomain}'
