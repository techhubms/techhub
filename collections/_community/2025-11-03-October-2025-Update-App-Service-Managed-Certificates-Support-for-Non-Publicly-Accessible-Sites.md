---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/follow-up-to-important-changes-to-app-service-managed/ba-p/4466120
title: 'October 2025 Update: App Service Managed Certificates Support for Non-Publicly Accessible Sites'
author: YutangLin
feed_name: Microsoft Tech Community
date: 2025-11-03 15:30:00 +00:00
tags:
- App Service Managed Certificates
- ASMC
- Azure App Service
- Azure Front Ends
- Azure Security
- Certificate Issuance
- Certificate Renewal
- DigiCert
- DNS
- Domain Validation
- HTTP Token Validation
- Platform Updates
- Private Endpoints
- Traffic Manager
- Web Apps
- Azure
- Security
- Community
section_names:
- azure
- security
primary_section: azure
---
YutangLin summarizes the latest Azure App Service Managed Certificates (ASMC) changes as of October 2025, emphasizing new support for non-publicly accessible sites and key requirements for successful certificate issuance.<!--excerpt_end-->

# October 2025 Update: App Service Managed Certificates Support for Non-Publicly Accessible Sites

**Author:** YutangLin

This article presents the recent updates to Azure App Service Managed Certificates (ASMC), building on the prior Tech Community post [‘Important Changes to App Service Managed Certificates: Is Your Certificate Affected?’](https://techcommunity.microsoft.com/blog/appsonazureblog/important-changes-to-app-service-managed-certificates-is-your-certificate-affect/4435193). It explains how recent changes affect certificate issuance, what exceptions exist, and what site administrators need to know to ensure smooth certificate validation and renewal.

## Key Changes Since July and October 2025

- **HTTP Token Validation Is Mandatory:**
  - Since July 2025, all ASMC certificate issuance and renewals require HTTP token validation.
  - DigiCert must reach the endpoint `https://<hostname>/.well-known/pki-validation/fileauth.txt` where App Service automatically places a domain validation token.
  - If this endpoint is inaccessible, domain validation fails.

- **Support for Non-Publicly Accessible Sites (October/November 2025):**
  - App Service now allows DigiCert's requests to the validation endpoint even if public access to the site is blocked.
  - DigiCert's request terminates at the [App Service front end](https://learn.microsoft.com/archive/msdn-magazine/2017/february/azure-inside-the-azure-app-service-architecture#front-end); the application itself is not exposed.
  - No need to specifically allow DigiCert’s IP addresses anymore.

## Requirements and Exceptions

- **A public DNS record remains required** for domain validation and issuance.
- **Unsupported scenarios:**
  - Sites on private endpoints with custom domains using only private DNS (no public DNS record).
  - Sites configured as "Nested" or "External" endpoints behind Azure Traffic Manager (only "Azure" endpoints are supported).
  - Certificates for domains ending in `*.trafficmanager.net`.

## Validation and Testing

- Site owners can test if their configuration supports ASMC by attempting certificate issuance. Success with an initial request means renewals are also likely to work, assuming all requirements are maintained.

## Practical Implications

- **Security:**
  - The update improves security by enabling certificate validation without requiring the entire application to be public, adhering to access restrictions while meeting operational requirements.
- **Operational Ease:**
  - Customers no longer need to manage DigiCert IP allowlists.
  - Exception handling for complex traffic management scenarios helps clarify required configurations.

## Summary Table

| Scenario                                                   | Supported for ASMC?   |
|------------------------------------------------------------|----------------------|
| Public site with DNS                                       | Yes                  |
| Non-public site with public DNS                            | Yes                  |
| Private endpoint, custom domain without public DNS         | No                   |
| Nested or External endpoints behind Traffic Manager         | No                   |
| Domains ending in *.trafficmanager.net                     | No                   |

## References and Further Reading

- [Important Changes to App Service Managed Certificates: Is Your Certificate Affected?](https://techcommunity.microsoft.com/blog/appsonazureblog/important-changes-to-app-service-managed-certificates-is-your-certificate-affect/4435193)
- [Inside the Azure App Service Architecture](https://learn.microsoft.com/archive/msdn-magazine/2017/february/azure-inside-the-azure-app-service-architecture)

---

*Updated Nov 01, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/follow-up-to-important-changes-to-app-service-managed/ba-p/4466120)
