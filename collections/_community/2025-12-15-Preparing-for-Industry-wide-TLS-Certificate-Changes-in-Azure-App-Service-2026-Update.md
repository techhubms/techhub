---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/industry-wide-certificate-changes-impacting-azure-app-service/ba-p/4477924
title: Preparing for Industry-wide TLS Certificate Changes in Azure App Service (2026 Update)
author: YutangLin
feed_name: Microsoft Tech Community
date: 2025-12-15 10:23:57 +00:00
tags:
- Azure App Service
- Azure Security
- Browser Standards
- CA/B Forum
- Certificate Pinning
- Certificates
- DigiCert
- EKU
- GoDaddy
- Managed Certificates
- Mtls
- Security Compliance
- SSL
- TLS
- Trusted Root CA
- Validity Period
- Azure
- Security
- Community
section_names:
- azure
- security
primary_section: azure
---
YutangLin explains how upcoming industry-wide requirements will impact TLS certificate issuance and management in Azure App Service, including steps administrators should take to maintain compliance and service reliability.<!--excerpt_end-->

# Preparing for Industry-wide TLS Certificate Changes in Azure App Service (2026 Update)

## Executive Summary

Beginning in early 2026, the CA/B Forum and major browser vendors will enforce new standards for TLS/SSL certificate issuance and validation. Azure App Service will align with these changes for both App Service Managed Certificates (ASMC, DigiCert-issued, free) and App Service Certificates (ASC, GoDaddy-issued, paid).

### Who Should Read This?

- App Service administrators
- Security and compliance teams
- Anyone managing TLS certificates in Azure

## Quick Reference: What's Changing & Required Actions

| Topic | ASMC (Managed, free) | ASC (GoDaddy, paid) | Required Action |
|-------|---------------------|---------------------|-----------------|
| New Cert Chain | New chain (no action unless pinned) | New chain (no action unless pinned) | Remove certificate pinning if used |
| Client Auth EKU | Not supported (no action unless cert used for mTLS) | Same | Transition from mTLS before cut-off |
| Validity | No change (already compliant) | Two overlapping certs per year | No action (automatic process) |

**No action is required for most users. Only those who pin certificates or use them for mTLS (client authentication EKU) must act.**

## Timeline of Key Dates

| Date | What Changes | Action Needed |
|------|-------------|--------------|
| Mid-Jan 2026+ | ASMC migrates to new chain; no client auth EKU | Remove certificate pinning; migrate from mTLS authentication if used |
| Mar 2026+ | ASC validity shortened; migrates to new chain; no client auth EKU | Remove certificate pinning; migrate from mTLS authentication if used |

## Actions Checklist

### For All Users

- Review usage of App Service certificates.
- If you don't pin certificates or use them for mTLS, you don't need to act.

### If You Pin Certificates (ASMC or ASC)

- Remove all pinning before respective change dates to avoid disruptions.
- Reference: [Azure App Service Best Practices – Certificate pinning](https://learn.microsoft.com/azure/app-service/app-service-best-practices)

### If You Use Certificates for Client Authentication (mTLS)

- Switch to another authentication method before the change dates, since client authentication EKU will no longer be supported.
- References:
  - [Sunsetting the client authentication EKU from DigiCert public TLS certificates](https://knowledge.digicert.com/alerts/sunsetting-client-authentication-eku-from-digicert-public-tls-certificates)
  - [Set Up TLS Mutual Authentication - Azure App Service](https://learn.microsoft.com/en-us/azure/app-service/app-service-web-configure-tls-mutual-auth?tabs=azureportal%2Cflask)

## Details & Rationale

### Why Are These Changes Happening?

Industry and browser mandates require certificate chains to improve trust and security. These changes apply across all certificate authorities.

### What's Changing?

#### 1. New Certificate Chain

- All certificates issued on Azure App Service will come from new chains to maintain browser trust.
- **Impact:** Remove any pinning, or risk disruption of your apps.

#### 2. Removal of Client Authentication EKU

- New certificates won't support client auth EKU. If you rely on mTLS, you must migrate auth methods.
- Driven by Chrome's root program and broader industry policies.

#### 3. Shortened Certificate Validity

- Maximum validity will be 200 days. ASMC is already compliant; ASC will auto-issue two overlapping certificates per paid year.

## FAQ

- **Loss of coverage due to validity?** No. ASC will issue two certs to cover your purchased term.
- **DigiCert/GoDaddy only?** No—these changes are industry-wide.
- **Other CAs affected?** Yes. Ask your CA for details.
- **Act today?** No action if you don't pin or use for mTLS.

## Glossary

- ASMC: App Service Managed Certificate (free, DigiCert)
- ASC: App Service Certificate (paid, GoDaddy)
- EKU: Extended Key Usage
- mTLS: Mutual TLS (client certificate authentication)
- CA/B Forum: Certification Authority/Browser Forum

## Additional Resources

- [Azure Security Fundamentals: Managed TLS](https://learn.microsoft.com/azure/security/fundamentals/managed-tls-changes)
- [Azure App Service Best Practices – Certificate pinning](https://learn.microsoft.com/azure/app-service/app-service-best-practices)
- [Set Up TLS Mutual Authentication](https://learn.microsoft.com/azure/app-service/app-service-web-configure-tls-mutual-auth?tabs=azureportal%2Cflask)
- [DigiCert Root and CA Updates](https://knowledge.digicert.com/general-information/digicert-root-and-intermediate-ca-certificate-updates-2023)

## Feedback & Support

If you have questions, visit Microsoft's official support channels or [Microsoft Q&A](https://learn.microsoft.com/answers/tags/436/azure-app-service).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/industry-wide-certificate-changes-impacting-azure-app-service/ba-p/4477924)
