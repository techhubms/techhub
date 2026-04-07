---
feed_name: Microsoft Security Blog
date: 2026-04-07 14:00:00 +00:00
author: Microsoft Threat Intelligence
tags:
- AADSignInEventsBeta
- Advanced Hunting
- Adversary in The Middle
- Adversary in The Middle (aitm)
- AI
- AiTM
- CloudAppEvents
- Conditional Access
- Continuous Access Evaluation
- Credential Theft
- Cyberespionage
- DNS Hijacking
- Dnsmasq
- Edge Device Compromise
- Entra ID Protection
- Forest Blizzard
- Forest Blizzard (strontium)
- KQL
- Microsoft Defender
- Microsoft Defender For Endpoint
- Microsoft Defender XDR
- Microsoft Entra ID
- Microsoft Outlook On The Web
- Microsoft Security Copilot
- Multifactor Authentication
- News
- Passkeys
- Risky Sign Ins
- Security
- SIEM
- SOHO Routers
- State Sponsored Threat Actor
- Storm 2754
- STRONTIUM
- Threat Hunting
- Threat Intelligence
- TLS Interception
- Zero Trust DNS
- ZTDNS
section_names:
- ai
- security
external_url: https://www.microsoft.com/en-us/security/blog/2026/04/07/soho-router-compromise-leads-to-dns-hijacking-and-adversary-in-the-middle-attacks/
primary_section: ai
title: SOHO router compromise leads to DNS hijacking and adversary-in-the-middle attacks
---

Microsoft Threat Intelligence breaks down how the Forest Blizzard (STRONTIUM) actor compromises SOHO routers to hijack DNS and selectively perform TLS adversary-in-the-middle attacks, and provides concrete mitigations plus Microsoft Defender and Entra ID hunting guidance.<!--excerpt_end-->

# SOHO router compromise leads to DNS hijacking and adversary-in-the-middle attacks

## Executive summary

Forest Blizzard, a threat actor linked to the Russian military, has been compromising insecure home and small-office internet equipment (SOHO routers) and modifying settings to turn those devices into malicious infrastructure.

The actor then hides behind that legitimate-but-compromised infrastructure to:

- Spy on additional targets
- Run follow-on attacks, including adversary-in-the-middle (AiTM)

Microsoft Threat Intelligence shared this campaign to increase awareness and to provide mitigation, detection, and hunting guidance.

Key points Microsoft highlights:

- Activity observed since at least **August 2025**.
- Actor: **Forest Blizzard** (STRONTIUM) and sub-group **Storm-2754**.
- Technique at scale: exploiting vulnerable SOHO devices to **hijack DNS requests** and facilitate collection of network traffic.
- Microsoft identified **200+ organizations** and **5,000 consumer devices** impacted by the malicious DNS infrastructure.
- Telemetry did **not** indicate compromise of Microsoft-owned assets or services.
- DNS hijacking was also leveraged to support post-compromise **TLS AiTM** against **Microsoft Outlook on the web** domains.

## DNS hijacking attack chain: From compromised devices to AiTM and other follow-on activity

![Diagram of the Forest Blizzard DNS hijacking attack chain](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/Forest-Blizzard-DNS-hijcaking-attack-chain.webp)

### Edge router compromise

Forest Blizzard gained access to SOHO devices and altered default network configurations to use **actor-controlled DNS resolvers**.

Why this works:

- Endpoints usually receive network configuration from edge devices via **DHCP**.
- Compromising SOHO devices is low effort and provides wide visibility.
- The actor can collect DNS traffic and passively observe DNS requests, enabling follow-on collection.

### DNS hijacking

Microsoft assesses Forest Blizzard is almost certainly using **dnsmasq** to:

- Perform DNS resolution and respond to queries
- Listen on **port 53** for DNS queries

Microsoft notes dnsmasq is a legitimate, lightweight service often used in home routers/smaller networks, providing:

- DNS forwarding and caching
- DHCP server capabilities

### Adversary-in-the-middle attacks

Microsoft observed AiTM activity related to the initial access campaign. Both observed variants are **TLS AiTM** attacks, enabling collection of transmitted data.

Observed behaviors:

- In most cases, DNS requests were **transparently proxied**, resulting in connections to legitimate service endpoints.
- In a limited set of compromises, the actor **spoofed DNS responses** for targeted domains so endpoints connected to actor-controlled infrastructure.

If a victim connects to the actor-controlled infrastructure:

- The infrastructure presents an **invalid TLS certificate** while spoofing the legitimate service.
- If a user ignores TLS warnings, the actor can intercept underlying plaintext traffic, potentially including **emails and other customer content**.

Microsoft notes Forest Blizzard appears to use AiTM selectively for high-priority intelligence targets.

Observed targets include:

- **AiTM against Microsoft 365 domains**: a subset of domains associated with **Microsoft Outlook on the web**.
- **AiTM against specific government servers**: non-Microsoft hosted servers in at least three government organizations in Africa.

### Possible post-compromise activities

Microsoft reports observed use was for information collection, but an AiTM position could also enable:

- Malware deployment
- Denial of service

## Mitigation and protection guidance

### Protection against DNS hijacking

Microsoft recommends:

- Enforce domain-name-based access controls using **Zero Trust DNS (ZTDNS)** on Windows endpoints so devices only resolve DNS through trusted servers: https://learn.microsoft.com/windows/security/operating-system-security/network-security/zero-trust-dns/
- Block known or malicious domains and maintain DNS logs: https://learn.microsoft.com/azure/dns/dns-security-policy
- Follow best practices for cloud network security: https://learn.microsoft.com/azure/security/fundamentals/network-best-practices
- Enable **network protection** and **web protection** in Microsoft Defender for Endpoint:
  - Network protection: https://learn.microsoft.com/defender-endpoint/network-protection?ocid=magicti_ta_learndoc
  - Web protection overview: https://learn.microsoft.com/defender-endpoint/web-protection-overview?ocid=magicti_ta_learndoc
- Avoid using home router solutions in corporate environments.

### Protection against AiTM and credential theft

Microsoft recommends:

- Centralize identity management; in hybrid environments integrate on-prem directories with cloud directories. If using third-party identity management, ensure logs flow to a SIEM or connect to **Microsoft Entra** for centralized monitoring.
- Use centralized identity data to support **Single Sign-On (SSO)** and to help Microsoft Entra’s machine learning models distinguish legitimate vs malicious access:
  - SSO planning: https://learn.microsoft.com/azure/active-directory/manage-apps/plan-sso-deployment?ocid=magicti_ta_learndoc
- Synchronize user accounts (except admin/high-privileged accounts) to keep a boundary between on-prem and cloud:
  - Password hash sync guidance: https://learn.microsoft.com/azure/active-directory/hybrid/connect/how-to-connect-password-hash-synchronization?ocid=magicti_ta_learndoc
- Enforce **MFA** and apply **Conditional Access**, especially for privileged/high-risk accounts; use passwordless options like **passkeys**.
- Use Microsoft Authenticator for passkeys and MFA: https://learn.microsoft.com/entra/identity/authentication/how-to-enable-authenticator-passkey?ocid=magicti_ta_learndoc
- Conditional Access overview: https://learn.microsoft.com/azure/active-directory/conditional-access/overview?ocid=magicti_ta_learndoc
- Use Conditional Access for phishing-resistant MFA for privileged accounts: https://learn.microsoft.com/entra/identity/conditional-access/policy-admin-phish-resistant-mfa#create-a-conditional-access-policy?ocid=magicti_ta_learndoc
- Restrict MFA/passkey registrations to trusted locations/devices: https://learn.microsoft.com/entra/id-protection/howto-identity-protection-configure-mfa-policy?ocid=magicti_ta_learndoc
- Implement **continuous access evaluation** and a **sign-in risk policy** to automate responses to risky sign-ins:
  - Continuous access evaluation: https://learn.microsoft.com/azure/active-directory/conditional-access/concept-continuous-access-evaluation?ocid=magicti_ta_learndoc
  - Configure risk policies: https://learn.microsoft.com/azure/active-directory/identity-protection/howto-identity-protection-configure-risk-policies?ocid=magicti_ta_learndoc
- Follow Microsoft IR best practices for recovery from systemic identity compromises: https://www.microsoft.com/security/blog/2020/12/21/advice-for-incident-responders-on-recovery-from-systemic-identity-compromises/

## Microsoft Defender detection and hunting guidance

Microsoft describes detections and hunting guidance across Defender and Entra.

### Microsoft Defender for Endpoint

Potential alerts (may also be triggered by unrelated activity):

- Forest Blizzard Actor activity detected
- Storm-2754 activity

### Entra ID Protection

Risk detection:

- Microsoft Entra threat intelligence (sign-in) (RiskEventType: investigationsThreatIntelligence)
  - https://learn.microsoft.com/entra/id-protection/concept-identity-protection-risks

### Hunting recommendations

Because initial compromise and DNS modification occur at the router level, Microsoft focuses hunting on post-compromise behavior.

#### Modifications to DNS settings

Microsoft notes that in identified activity, router compromise resulted in updates to default DNS settings on connected Windows machines.

- Look for unusual modifications to DNS settings.
- Reset DNS settings and remediate vulnerable SOHO devices.
- Note: this does not remediate credential theft if it already occurred.

#### Post-compromise activity

Microsoft highlights that AiTM can let an actor operate as a valid user. Establishing normal behavior baselines is key.

For Entra environments, Microsoft suggests daily monitoring using:

- Risky sign-in reports: https://portal.azure.com/#view/Microsoft_AAD_IAM/SecurityMenuBlade/~/RiskySignIns
- Risky user reports: https://portal.azure.com/#view/Microsoft_AAD_IAM/SecurityMenuBlade/~/RiskyUsers

Example advanced hunting query (as provided):

```kusto
AADSignInEventsBeta | where RiskLevelAggregated == 100 and (ErrorCode == 0 or ErrorCode == 50140) | project Timestamp, Application, LogonType, AccountDisplayName, UserAgent, IPAddress
```

For Microsoft 365 environments, Microsoft notes *ActionType* values like **Search** or **MailItemsAccessed** in the **CloudAppEvents** table can provide additional signals:

```kusto
CloudAppEvents | where AccountObjectId == " " // limit results to specific suspicious user accounts by adding the user here | where ActionType has_any ("Search", "MailItemsAccessed")
```

### Threat intelligence reports (Defender portal)

Microsoft lists threat analytics reports available to Microsoft Defender XDR customers:

- Actor profile: Forest Blizzard: https://security.microsoft.com/threatanalytics3/752538a3-9738-420d-8ca5-fa8afbfb8b5a/overview
- Activity profile: SOHO router compromise leads to DNS hijacking and adversary-in-the-middle attacks: https://security.microsoft.com/threatanalytics3/c88575f0-6eff-4d28-afeb-8b8ef590ec74/overview

## Microsoft Security Copilot

Microsoft describes **Microsoft Security Copilot** as embedded in Microsoft Defender, supporting tasks like summarizing incidents, analyzing files/scripts, generating hunting queries, and creating reports.

Links Microsoft provided:

- Microsoft Security Copilot overview: https://www.microsoft.com/en-us/security/business/ai-machine-learning/microsoft-security-copilot
- Embedded in Defender: https://learn.microsoft.com/defender-xdr/security-copilot-in-microsoft-365-defender
- Deploy agents: https://learn.microsoft.com/defender-xdr/security-copilot-agents-defender
- Agents overview: https://learn.microsoft.com/copilot/security/agents-overview

Example agents Microsoft listed:

- Threat Intelligence Briefing agent: https://learn.microsoft.com/defender-xdr/threat-intel-briefing-agent-defender
- Phishing Triage agent: https://learn.microsoft.com/defender-xdr/phishing-triage-agent
- Threat Hunting agent: https://learn.microsoft.com/defender-xdr/advanced-hunting-security-copilot-threat-hunting-agent
- Dynamic Threat Detection agent: https://learn.microsoft.com/defender-xdr/dynamic-threat-detection-agent

Standalone experience and developer scenarios:

- Standalone experience: https://learn.microsoft.com/en-us/copilot/security/experiences-security-copilot
- Developer scenarios (custom agents/plugins): https://learn.microsoft.com/copilot/security/developer/custom-agent-overview

## Learn more

- Microsoft Threat Intelligence Blog: https://aka.ms/threatintelblog
- LinkedIn: https://www.linkedin.com/showcase/microsoft-threat-intelligence
- X (formerly Twitter): https://x.com/MsftSecIntel
- Bluesky: https://bsky.app/profile/threatintel.microsoft.com
- Microsoft Threat Intelligence podcast: https://thecyberwire.com/podcasts/microsoft-threat-intelligence


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/04/07/soho-router-compromise-leads-to-dns-hijacking-and-adversary-in-the-middle-attacks/)

