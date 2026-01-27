---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-the-general-availability-ga-of-the-premium-v2-tier-of/ba-p/4471499
title: Azure API Management Premium v2 Tier Now Generally Available
author: Sreekanth_Thirthala
feed_name: Microsoft Tech Community
date: 2025-11-19 21:10:17 +00:00
tags:
- API Gateway
- Authorization Credentials
- Availability Zones
- Azure API Management
- Backend Integration
- Cloud Infrastructure
- Custom CA Certificates
- Enterprise API
- Network Security
- Premium V2
- Private Link
- TLS
- Virtual Networks
- VNet Injection
- VNet Integration
- Zone Redundancy
section_names:
- azure
- security
primary_section: azure
---
Sreekanth_Thirthala presents the general availability of Azure API Management Premium v2 tier, detailing new enterprise-grade features, networking capabilities, and security improvements for API workloads.<!--excerpt_end-->

# Announcing Azure API Management Premium v2 Tier General Availability

Azure API Management Premium v2 offers enhanced capacity, higher entity limits, unlimited API calls, and an advanced feature set for running enterprise-wide APIs with high availability and performance.

## Key Features and Improvements

### New Architecture and Networking

- **Management Traffic Isolation:** Premium v2 eliminates management traffic from customer VNets, making private networking more secure and easier to set up.
- **VNet Options:** Choose between [VNet injection](https://learn.microsoft.com/en-us/azure/api-management/virtual-network-concepts#virtual-network-injection) and [VNet integration](https://learn.microsoft.com/en-us/azure/api-management/virtual-network-concepts#outbound-integration) for flexible network configuration.
- **Simplified VNet Injection:** No manual configuration of routes or endpoints required, allowing independent management of network security settings for customers and Microsoft. Route outbound traffic to on-premises, through NVAs, or monitor with WAF devices—all without constraints.

### Inbound Private Link

- **Private Endpoint Connectivity:** Secure inbound access to your API Management instance using [Azure Private Link](https://learn.microsoft.com/en-us/azure/api-management/private-endpoint?tabs=premv2), restricting exposure from the public internet.
- **Multiple Connections:** Support for up to 100 Private Link connections per instance.
- **DNS Integration:** Option to use custom DNS or [Azure DNS private zones](https://learn.microsoft.com/en-us/azure/dns/private-dns-overview) for hostname mapping.
- **Policy Control:** Apply API management policies based on connection type (private/public), and restrict incoming traffic to private endpoints for data protection.

### Availability Zone Support

- **Zone Redundancy:** Enable [Availability Zones](https://learn.microsoft.com/en-us/azure/api-management/enable-availability-zone-support?tabs=premv2) for increased resilience and reliability, distributing service units (Gateway, management plane, developer portal) across physically separate zones in a region.
- **Configuration:** Simple flag to enable during deployment for AZ-enabled regions.

### Custom CA Certificates

- **Backend TLS Authentication:** Configure [custom CA certificates](https://learn.microsoft.com/en-us/azure/api-management/backends?tabs=portal#configure-ca-certificate) for API Management Gateway to validate backends secured by private CAs.
- **Certificate Management:** Add thumbprints or subject name plus issuer thumbprint pairs to Backend entities.
- **Authorization Credentials:** Custom CA certificates managed as part of Backend Authorization Credentials.

## Regional Availability

Azure API Management Premium v2 is now available in Australia East, East US2, Germany West Central, Korea Central, Norway East, and UK South, with more regions planned.

For pricing and up-to-date availability, visit the [API Management pricing page](https://aka.ms/apimpricing).

## Further Resources

- [API Management v2 tiers FAQ](https://aka.ms/apim/skuv2/faq)
- [API Management v2 documentation](https://aka.ms/apimdocs/skuv2/overview)
- [API Management overview](https://aka.ms/acom-faq-whatisapim)

_Updated Nov 19, 2025 by Sreekanth_Thirthala_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-the-general-availability-ga-of-the-premium-v2-tier-of/ba-p/4471499)
