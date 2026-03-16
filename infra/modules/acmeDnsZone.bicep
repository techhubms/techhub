@description('Azure region (must be "global" for DNS zones)')
param location string = 'global'

@description('DNS zone name for ACME challenges (e.g. acme.hub.ms)')
param zoneName string

@description('Domains that need ACME delegation. Each gets a CNAME target record in this zone.')
param delegatedDomains string[]

// Public DNS zone for ACME challenge delegation.
// External DNS (e.g. GoDaddy) CNAMEs _acme-challenge.<domain> to <alias>.acme.hub.ms,
// and certbot-dns-azure creates/deletes TXT records here during certificate renewal.
resource dnsZone 'Microsoft.Network/dnsZones@2023-07-01-preview' = {
  name: zoneName
  location: location
  properties: {
    zoneType: 'Public'
  }
}

// Placeholder TXT records for each delegated domain.
// certbot-dns-azure will overwrite these during renewal.
resource acmeRecords 'Microsoft.Network/dnsZones/TXT@2023-07-01-preview' = [for domain in delegatedDomains: {
  parent: dnsZone
  name: replace(replace(domain, '.', '-'), '*', 'wildcard')
  properties: {
    TTL: 60
    TXTRecords: [
      { value: ['placeholder - replaced by certbot during renewal'] }
    ]
  }
}]

output zoneName string = dnsZone.name
output nameServers string[] = dnsZone.properties.nameServers
output delegatedRecordNames string[] = [for (domain, i) in delegatedDomains: acmeRecords[i].name]
