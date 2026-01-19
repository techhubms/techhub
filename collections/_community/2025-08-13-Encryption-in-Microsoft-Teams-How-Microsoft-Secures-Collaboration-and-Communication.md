---
layout: post
title: 'Encryption in Microsoft Teams: How Microsoft Secures Collaboration and Communication'
author: WillDixon
canonical_url: https://techcommunity.microsoft.com/t5/microsoft-teams-blog/encryption-in-microsoft-teams-june-2025/ba-p/4442913
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community
date: 2025-08-13 15:00:00 +00:00
permalink: /security/community/Encryption-in-Microsoft-Teams-How-Microsoft-Secures-Collaboration-and-Communication
tags:
- BYOK
- Compliance
- Customer Key
- Data At REST Encryption
- E2EE
- Encryption
- End To End Encryption
- Entra ID
- GDPR
- HIPAA
- Microsoft Purview
- Microsoft Teams
- OneDrive
- PCI DSS
- Security Architecture
- Service Encryption
- SharePoint
- TLS
- Zero Trust
section_names:
- security
---
WillDixon presents an in-depth guide on how Microsoft Teams uses encryption—including standard and end-to-end options—to secure sensitive data and support compliance for business communication.<!--excerpt_end-->

# Encryption in Microsoft Teams: How Microsoft Secures Collaboration and Communication

## Introduction

Microsoft Teams is a key platform for business collaboration, handling sensitive data for over 320 million monthly users. The platform's architecture supports a wide range of collaborative activities including messaging, file sharing, and meetings—all of which require strong security due to regulatory requirements and the prevalence of remote work.

## Why Encryption Matters for Microsoft Teams

- Protects intellectual property, financial details, and PII from interception and unauthorized access.
- Supports legal and industry compliance (GDPR, HIPAA, PCI DSS, CCPA, NIST 800-53).
- Maintains privacy and data integrity across varied business use cases.

## Encryption Layers in Microsoft Teams

Teams protects data using multiple encryption mechanisms:

### 1. Transport Layer Security (TLS)

- Uses TLS 1.2+ and AES-256 encryption for data in transit between clients and Microsoft servers.

### 2. Data at Rest Encryption

- Utilizes BitLocker and per-file encryption for data stored in Azure, SharePoint, OneDrive, and other Microsoft 365 backends.

### 3. Service Encryption

- Adds another layer by encrypting Teams data before storage using Microsoft-managed keys.
- Service encryption covers media content, with ongoing expansion to additional Teams services.

### 4. Customer Key Encryption (BYOK)

- Lets organizations supply and control their own encryption keys, enabling deeper data security management and regulatory control.
- Integrated via Microsoft Purview Customer Key.
- Supported for Teams files, chat messages, and other data modalities.

### 5. End-to-End Encryption (E2EE)

- Available for one-to-one calls and certain meetings—especially for Teams Premium users.
- E2EE keys are generated and managed on user devices, never accessible to Microsoft.
- Covers audio, video, and screen sharing (not chat, files, or recordings).
- Handshake uses DTLS and Group Key Manager Protocol; media stream encrypted with GCM_AES_256.
- E2EE disables server-side features like recording, transcription, DLP, eDiscovery, and real-time anti-malware.

## Compliance Considerations

Teams' encryption options are designed to meet requirements for:

- **GDPR:** Mandates robust technical controls for personal data security.
- **HIPAA:** Requires protections for electronic protected health information (ePHI).
- **PCI DSS:** Demands encryption of cardholder data during transmission.
- **CCPA/CPRA:** Provides safe harbor for encrypted personal data.
- **NIST 800-53:** Specifies cryptographic controls as a security baseline.

## Trade-offs: Standard Encryption vs. End-to-End Encryption

- **Standard Encryption:** Default for all Teams data in transit and at rest; does **not** inhibit service features.
- **E2EE:** Provides maximum privacy for high-sensitivity scenarios but removes access to collaboration and compliance features reliant on server processing.
- **Limitations of E2EE:** No recordings, transcripts, or server-side DLP/supervision, and certain anti-malware features are weakened.
- **Group vs. One-to-One:** E2EE is supported only in specific one-to-one calls and Premium meetings; not available for group chats, channels, or shared files.

## Security Posture and Strategy

- Teams encryption is part of a broader Microsoft Zero Trust approach, with customizable policies via Entra ID (formerly Azure AD).
- Customer Key (BYOK) enables organizations to maintain control over critical encryption keys.
- Organizations should adopt a hybrid encryption strategy—using standard encryption for normal collaboration and reserving E2EE for sensitive cases.
- A risk-based approach ensures compliance without undermining collaboration efficiency.

## Quick Reference: Modalities and Encryption Support

| Modality           | E2EE Available     | Standard Encryption |
|--------------------|-------------------|--------------------|
| One-to-One Calls   | Yes (VoIP only)   | Yes                |
| Scheduled Meetings | Yes (Premium)     | Yes                |
| Screen Sharing     | Yes (Premium/1-1) | Yes                |
| Video              | Yes (Premium/1-1) | Yes                |
| Group Calls        | No                | Yes                |
| Channel Meetings   | No                | Yes                |
| Chat Messages      | No                | Yes                |
| Shared Files       | No                | Yes                |
| Recordings         | No                | Yes                |

## Implementation Notes

- E2EE must be enabled by both IT admins and end users for one-to-one calls and premium meetings.
- Admins can set policies organization-wide, leveraging Microsoft Purview Customer Key for advanced scenarios.

## Learn More

- [Overview of Customer Key - Microsoft Purview | Microsoft Learn](https://learn.microsoft.com/en-us/purview/customer-key-overview)
- [Overview of security and compliance – Microsoft Teams | Microsoft Learn](https://learn.microsoft.com/en-us/microsoftteams/security-compliance-overview)

## Conclusion

Microsoft Teams incorporates robust encryption technologies at every layer, supporting compliance and operational needs. By understanding and configuring standard and end-to-end encryption appropriately, organizations can secure their communications while continuing to leverage the collaboration tools Teams offers.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-teams-blog/encryption-in-microsoft-teams-june-2025/ba-p/4442913)
