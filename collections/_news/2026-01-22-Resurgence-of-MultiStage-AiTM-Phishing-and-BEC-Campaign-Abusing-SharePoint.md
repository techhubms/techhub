---
layout: "post"
title: "Resurgence of Multi‑Stage AiTM Phishing and BEC Campaign Abusing SharePoint"
description: "This Microsoft Defender Security Research Team report details the discovery and mitigation of a sophisticated adversary-in-the-middle (AiTM) phishing and business email compromise (BEC) campaign targeting energy sector organizations. The campaign leveraged trusted SharePoint services and advanced persistence techniques. Thorough detection and remediation steps are outlined, including recommendations for Microsoft Defender XDR, Entra ID, and Sentinel users."
author: "Microsoft Defender Security Research Team"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.microsoft.com/en-us/security/blog/2026/01/21/multistage-aitm-phishing-bec-campaign-abusing-sharepoint/"
viewing_mode: "external"
feed_name: "Microsoft Security Blog"
feed_url: "https://www.microsoft.com/en-us/security/blog/feed/"
date: 2026-01-22 05:14:14 +00:00
permalink: "/2026-01-22-Resurgence-of-MultiStage-AiTM-Phishing-and-BEC-Campaign-Abusing-SharePoint.html"
categories: ["Azure", "Security"]
tags: ["Advanced Persistent Threat", "AiTM Phishing", "Azure", "Azure Active Directory", "Business Email Compromise", "Conditional Access", "Continuous Access Evaluation", "Defender For Cloud Apps", "Email Security", "Energy Sector Security", "Entra ID", "Inbox Rule Manipulation", "Microsoft Defender", "Microsoft Defender XDR", "Microsoft Sentinel", "Mitigation Guidance", "Multi Factor Authentication", "News", "Security", "Session Cookie Theft", "SharePoint", "Threat Detection"]
tags_normalized: ["advanced persistent threat", "aitm phishing", "azure", "azure active directory", "business email compromise", "conditional access", "continuous access evaluation", "defender for cloud apps", "email security", "energy sector security", "entra id", "inbox rule manipulation", "microsoft defender", "microsoft defender xdr", "microsoft sentinel", "mitigation guidance", "multi factor authentication", "news", "security", "session cookie theft", "sharepoint", "threat detection"]
---

Microsoft Defender Security Research Team investigates a sophisticated AiTM phishing and BEC attack campaign leveraging SharePoint, providing in-depth insights, detection analytics, and actionable defense strategies for security practitioners.<!--excerpt_end-->

# Resurgence of Multi‑Stage AiTM Phishing and BEC Campaign Abusing SharePoint

*By Microsoft Defender Security Research Team*

## Overview

Microsoft Defender researchers identified an advanced adversary-in-the-middle (AiTM) phishing and business email compromise (BEC) campaign that targeted multiple energy sector organizations. Exploiting trusted Microsoft SharePoint file-sharing services and manipulating mailboxes via malicious inbox rules, the attackers established persistence while evading detection and expanded the attack both within and across organizations.

## Attack Chain Summary

### 1. Initial Access

- **Trusted Vendor Compromise**: Attackers sent phishing emails from trusted but compromised organizations.
- The suspect email mimicked legitimate SharePoint document-sharing notifications to trick users into authenticating.

### 2. Phishing Payload Delivery

- **Abuse of SharePoint**: Phishing links hosted on SharePoint bypassed traditional email filters due to the trusted platform.
- Authentication prompts replicated enterprise workflows, increasing credibility.

### 3. Adversary-in-the-Middle (AiTM) Attack

- Users redirected to malicious credential prompts capturing login details.
- Session cookies intercepted allowed attackers to maintain persistence.

### 4. Inbox Rule Manipulation

- Attackers created mailbox rules to auto-delete/newly mark emails as read, hiding evidence and notifications from the user.

### 5. Intra- and Inter-Organizational Phishing

- Leveraged compromised identities to send high-volume phishing emails (>600) to contacts and distribution lists.
- Used recent email threads to target credible recipients.

### 6. Business Email Compromise (BEC)

- Monitored mailboxes for suspicious replies and deleted critical correspondence to avoid exposure.
- Engaged with recipients to falsely validate the phishing attempt.

### 7. Lateral Movement and Further Account Compromise

- Additional AiTM attacks targeted recipients who clicked malicious links, propagating the compromise within organizations.
- Defender Experts identified compromised accounts using IP analysis.

## Detection and Mitigation

### Microsoft Defender XDR and Associated Tools

- Automatically detects account compromise patterns: malicious rule creation, session cookie theft, anomalous sign-ins.
- Utilizes signals across Microsoft Defender XDR, Defender for Cloud Apps, Defender for Endpoint, and Entra ID Protection.
- Initiates zero-hour auto purge (ZAP) to remove malicious emails.

### Recommended Defense Strategies

- **Revoking Session Cookies**: Resetting passwords is insufficient; active session cookies must also be revoked.
- **Multi-Factor Authentication (MFA)**: While AiTM attacks attempt to circumvent MFA, proper MFA implementation, such as Microsoft Authenticator and FIDO2 keys, remains vital.
- **Conditional Access Policies**: Enable identity-driven, risk-based access control in Entra ID.
- **Continuous Access Evaluation**: Quickly revoke compromised sessions.
- **Security Defaults**: Use as a baseline while implementing more granular controls.
- **Advanced Anti-Phishing**: Deploy solutions that identify and block malicious sites and emails.
- **Continuous Monitoring**: Hunt for anomalous or suspicious activities in sign-ins, mailbox activity, and unusual email patterns.

### Sample Hunting Queries

- **Identify phishing emails** with specific subject lines using EmailEvents.
- **Detect sign-in from suspicious IPs** with AADSignInEventsBeta.

### Sentinel Analytics Templates

- Analytic rules for detecting mail access, inbox manipulation, suspicious SharePoint operations, unusual sign-ins, and BEC-related threats are provided (with links to GitHub for query details).

## Indicators of Compromise

- **Malicious IPs**: 178.130.46.8, 193.36.221.10
- **Defender XDR Alerts**: Session cookie theft, suspicious inbox rule, impossible travel, anomalous token, BEC-related harvesting activity

## Actionable Recommendations

- Configure and enforce Conditional Access and risk-based policies.
- Implement FIDO2 passwordless sign-in.
- Activate Defender for Endpoint protections.
- Use Microsoft Edge for enhanced phishing detection.
- Continuously educate users on phishing awareness and vendor impersonation risks.

## References and Tools

- Direct links to Microsoft docs for configuring recommended policies, Defender tools, and Sentinel hunting queries.

---

This report underscores the criticality of a multi-layered security strategy leveraging Microsoft's cloud-native defense tools. Security professionals are encouraged to implement the outlined controls to detect, respond, and recover from advanced phishing and BEC threats.

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2026/01/21/multistage-aitm-phishing-bec-campaign-abusing-sharepoint/)
