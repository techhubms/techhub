---
layout: post
title: Code Signing Windows Apps Easier and More Secure with Azure Artifact Signing
author: Tim Anderson
canonical_url: https://devclass.com/2026/01/14/code-signing-windows-apps-may-be-easier-and-more-secure-with-new-azure-artifact-service/
viewing_mode: external
feed_name: DevClass
feed_url: https://devclass.com/feed/
date: 2026-01-14 15:32:25 +00:00
permalink: /azure/blogs/Code-Signing-Windows-Apps-Easier-and-More-Secure-with-Azure-Artifact-Signing
tags:
- AAS
- Application Security
- Artifact Signing SDK
- Azure
- Azure Artifact Signing
- Azure DevOps
- Blogs
- Certificate Authority
- Certificates
- Cloud Workflows
- Code Signing
- DevOps
- GitHub Actions
- Security
- Signtool
- Time Stamping
- Windows
section_names:
- azure
- devops
- security
---
Tim Anderson delivers a detailed analysis of Azure Artifact Signing, Microsoft's new service to streamline and secure code signing for Windows applications, addressing modern security requirements and developer workflows.<!--excerpt_end-->

# Code Signing Windows Apps Easier and More Secure with Azure Artifact Signing

Microsoft has launched Azure Artifact Signing (AAS) as a generally available service in the USA, Canada, and Europe. AAS, previously known as Trusted Signing, allows Windows developers to sign their applications with daily-renewed certificates, reducing security risks associated with long-lived credentials.

## Key Features of Azure Artifact Signing

- **Short-lived Certificates:** Certificates are renewed every 24 hours, minimizing the risk of key theft or misuse. Applications, once signed and time-stamped, remain valid even after the signing certificate expires, unless revoked.
- **Regional Availability:** Available to individuals and organizations in the USA, Canada, EU, and UK, with plans for additional regions in the future.
- **Pricing:** Two plans are available:
  - Basic: $9.99/month (up to 5,000 signatures, 1 certificate profile)
  - Premium: $99.99/month (up to 100,000 signatures, 10 certificate profiles)
- **Integration Tools:** Supports workflows with signtool (Windows SDK), GitHub Actions, Azure DevOps tasks, PowerShell, and a dedicated Artifact Signing SDK.
- **Supported Windows Versions:** Certificates work back to Windows 7.0 SP1, provided the necessary root certificate authority is installed (typically automatic for internet-connected devices).
- **No Extended Validation (EV) Certificates:** The service does not offer EV certificates, which require additional validation and are typically more expensive.

## Improvements Over Traditional Methods

Traditionally, code signing required developers to source and manage long-term certificates, often stored on local PCs or hardware tokens like USB HCMs (Hardware Crypto Modules). Modern industry requirements demand:

- Encryption key protection (e.g., HCM, not regular PC storage)
- Shorter certificate validity (up to 460 days, soon required by certification authorities)
- No use of deprecated algorithms (e.g., SHA-1)

AAS not only adheres to these requirements but goes further with one-day certificate lifetimes and a fully managed cloud approach, making it suitable for CI/CD and cloud-centric workflows.

## Security and Administrative Benefits

- **Reduced Key Management Burden:** Developers no longer need to handle or protect long-lived certificates directly.
- **Enterprise/Org-Vetted Certificates:** Organization certificates use a validated legal entity name, meeting compliance needs.
- **Automated Signing Workflows:** Fits naturally into automated build and deployment pipelines using GitHub Actions and Azure DevOps.

## Limitations and Considerations

- **Regional Restrictions:** Currently, it is limited to specific geographies.
- **EV Certificates Not Supported:** Some software ecosystems or customers may require EV certs, which are not available through AAS.
- **Requirements for Certificate Authorities:** For the certificates to be recognized, certain root certificates must be present on the target systems.
- **Documentation Cautions:** Developers must follow Microsoft’s documentation closely, especially regarding validation procedures and documentation.

## Comparison and Costs

Azure Artifact Signing is described as one of the most affordable code signing options for Windows, especially compared to traditional vendors who may require hardware tokens or EV validation. Electron documentation confirms its low cost for Windows applications; however, code signing for macOS still requires a separate, Apple-managed process.

## References

- [Azure Artifact Signing FAQ](https://learn.microsoft.com/en-us/azure/artifact-signing/faq)
- [Microsoft Security Blog Announcement](https://techcommunity.microsoft.com/blog/microsoft-security-blog/simplifying-code-signing-for-windows-apps-artifact-signing-ga/4482789)
- [Windows Support for Trusted Signing](https://support.microsoft.com/en-gb/topic/kb5022661-windows-support-for-the-trusted-signing-formerly-azure-code-signing-program-4b505a31-fa1e-4ea6-85dd-6630229e8ef4)
- [Certificate Authority Browser Forum Code Signing Requirements](https://cabforum.org/working-groups/code-signing/requirements/)
- [Electron Documentation on Code Signing](https://www.electronforge.io/guides/code-signing/code-signing-windows)
- [AppLocker Policies](https://learn.microsoft.com/en-us/windows/security/application-security/application-control/app-control-for-business/applocker/working-with-applocker-policies)

---
*Author: Tim Anderson, presented by DevClass.*

This post appeared first on "DevClass". [Read the entire article here](https://devclass.com/2026/01/14/code-signing-windows-apps-may-be-easier-and-more-secure-with-new-azure-artifact-service/)
