---
external_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-platform-support-for-transport-layer-security-tls-1-1-and-earlier-versions-ending-soon/
title: Fabric Platform Ends Support for TLS 1.1 and Earlier
author: Microsoft Fabric Blog
viewing_mode: external
feed_name: Microsoft Fabric Blog
date: 2025-08-04 11:00:00 +00:00
tags:
- Cloud Security
- Data Integration
- Deprecation Notice
- Encryption Protocols
- Microsoft
- Microsoft Fabric
- Migration Guidance
- Platform Update
- Secure Connections
- Security Compliance
- TLS 1.2
- Transport Layer Security
section_names:
- ml
- security
---
Microsoft Fabric Blog announces the end of support for TLS 1.1 and earlier on the Fabric platform. Authors provide key migration steps and security implications for organizations relying on Fabric integrations.<!--excerpt_end-->

# Fabric Platform Ends Support for TLS 1.1 and Earlier

The Microsoft Fabric team has announced that support for TLS 1.1 and all earlier versions has officially ended on the Fabric platform. Beginning July 31, 2025, all outbound connections from Fabric to customer data sources will require TLS 1.2 or newer, as part of Microsoft's ongoing commitment to security and compliance.

## What You Need to Know

- **Deadline:** TLS 1.1 and older versions will no longer be supported after **July 31, 2025**.
- **Impact:** Any services or resources integrated with Fabric that use TLS 1.1 or older will fail to connect.
- **Required Action:** Organizations must update all systems and integrations interacting with Fabric to use TLS 1.2 or later.

## Migration Resources

- [Enable TLS 1.2 client documentation](https://aka.ms/RemoveLegacyTLS)
- [Microsoft Q&A for community support](https://aka.ms/azureqa)
- [How to create a Microsoft Azure support request](https://learn.microsoft.com/azure/azure-portal/supportability/how-to-create-azure-support-request)

## Next Steps

- Review system documentation and ensure all endpoints interfacing with Fabric support TLS 1.2 or higher.
- Leverage the provided migration guides and community support links if you need help updating your systems.
- Validate connections prior to the deadline to avoid service interruptions following the deprecation date.

## Why This Matters

Migrating to TLS 1.2 improves platform security, protects data in transit, and aligns with industry-standard best practices. Microsoft continues to invest in enhancing the security posture of its data platforms, and this update reflects the importance of evolving encryption standards.

For more information, see the [official announcement](https://blog.fabric.microsoft.com/en-us/blog/fabric-platform-support-for-transport-layer-security-tls-1-1-and-earlier-versions-ending-soon/) and the previous [TLS deprecation overview](https://blog.fabric.microsoft.com/blog/tls-deprecation-for-fabric/).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/fabric-platform-support-for-transport-layer-security-tls-1-1-and-earlier-versions-ending-soon/)
