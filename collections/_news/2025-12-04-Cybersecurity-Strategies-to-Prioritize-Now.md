---
layout: "post"
title: "Cybersecurity Strategies to Prioritize Now"
description: "This article by Damon Becknel, Vice President and Deputy CISO for Regulated Industries at Microsoft, provides a practical breakdown of core cybersecurity practices every organization should prioritize. It covers actionable steps for cyber hygiene, modernizing security standards, advanced fingerprinting techniques, and stresses the importance of collaboration and knowledge sharing in the face of evolving threats. Microsoft technologies such as Entra ID, Intune, Defender, and Azure Front Door are featured as part of recommended solutions."
author: "stclarke"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.microsoft.com/en-us/security/blog/2025/12/04/cybersecurity-strategies-to-prioritize-now/"
viewing_mode: "external"
feed_name: "Microsoft News"
feed_url: "https://news.microsoft.com/source/feed/"
date: 2025-12-04 19:01:12 +00:00
permalink: "/news/2025-12-04-Cybersecurity-Strategies-to-Prioritize-Now.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Front Door", "Company News", "Cyber Hygiene", "Cybersecurity", "Data Loss Prevention", "DMARC", "DNS Security", "Endpoint Detection And Response", "Fingerprinting", "Identity Management", "Incident Response", "Logging And Monitoring", "Microsoft Defender", "Microsoft Entra ID", "Microsoft Intune", "Microsoft Security", "Multifactor Authentication", "Network Segmentation", "News", "Phishing Resistant MFA", "Security", "Security Collaboration", "Security Standards", "VPN"]
tags_normalized: ["azure", "azure front door", "company news", "cyber hygiene", "cybersecurity", "data loss prevention", "dmarc", "dns security", "endpoint detection and response", "fingerprinting", "identity management", "incident response", "logging and monitoring", "microsoft defender", "microsoft entra id", "microsoft intune", "microsoft security", "multifactor authentication", "network segmentation", "news", "phishing resistant mfa", "security", "security collaboration", "security standards", "vpn"]
---

Damon Becknel from Microsoft shares practical cybersecurity strategies, emphasizing essential defenses, identity management, modern protocols, and collaboration for stronger organizational security.<!--excerpt_end-->

# Cybersecurity Strategies to Prioritize Now

**By Damon Becknel, Vice President and Deputy CISO for Regulated Industries at Microsoft**

This article outlines four cybersecurity priorities for every organization:

---

### 1. Prioritize Essential Cyber Hygiene Basics

- **Asset Inventory**: Maintain a detailed inventory of all hardware, software, and cloud resources to ensure comprehensive security coverage.
- **Network Segmentation**: Use segmentation to limit exposure and enforce proper traffic patterns, leveraging jump boxes for sensitive access.
- **Block Malicious Traffic**: Restrict access to your systems by filtering out known attacker IPs (such as Tor nodes and suspicious country blocks).
- **Logging and Monitoring**: Implement robust, year-long logging and monitoring, making data machine-readable for effective incident response.
- **VPN Usage**: Enforce VPN access to keep users on trusted segments and simplify security patching (learn more about [Microsoft Entra ID authentication for VPN](https://learn.microsoft.com/azure/vpn-gateway/point-to-site-entra-gateway)).
- **Identity Hardening**: Segregate administrative and day-to-day accounts, enforce multifactor authentication—prefer phishing-resistant options like YubiKeys or Passkeys ([Microsoft guidance](https://learn.microsoft.com/en-us/entra/identity/conditional-access/policy-admin-phish-resistant-mfa)).
- **Patch Management**: Timely patching for all assets using inventory-driven processes.
- **Endpoint Security**: Deploy endpoint detection and response solutions ([EDR](https://www.microsoft.com/en-us/security/business/security-101/what-is-edr-endpoint-detection-response)), encryption, host-based firewalls, and inventory tooling (see [Microsoft Defender](https://www.microsoft.com/security/business/endpoint-security/microsoft-defender-endpoint) and [Intune](https://www.microsoft.com/security/business/Microsoft-Intune)).
- **Web and Email Gateways**: Use proxies and gateways to analyze and filter malicious traffic—enforce web proxy use and email security solutions.
- **Data Loss Prevention**: Automate policy-based actions to protect and block sensitive data ([DLP guidance](https://www.microsoft.com/en-us/security/business/security-101/what-is-data-loss-prevention-dlp)).
  
### 2. Prioritize Modern Security Standards, Products, and Protocols

- **Authentication**: Move to phishing-resistant multifactor authentication; eliminate passwords using passkey solutions ([what is passkey](https://www.microsoft.com/en-us/security/business/security-101/what-is-passkey)).
- **DNS Security**: Implement DNS security measures, filter/block threats, and monitor for malicious activity.
- **SMTP Security**: Close open relays, secure configurations, use encrypted connections, and enable DMARC ([DMARC setup](https://learn.microsoft.com/defender-office-365/email-authentication-dmarc-configure)).
- **Exchange Web Services Modernization**: Identify and migrate from EWS to Graph APIs ([migration mapping](https://aka.ms/ews2graphMap)); check dependencies ([app identification guidance](https://aka.ms/ewsIdentifyApps)).
- **BGP Practices**: Update operational protocols to reduce hijack and denial-of-service risks.

### 3. Prioritize Fingerprinting to Identify Bad Actors

- **Beyond IP Blocking**: Use unique device/browser/user identifiers for traffic analysis—track legitimate and malicious activity more reliably (integrate fingerprint solutions like [Azure Front Door](https://azure.microsoft.com/products/frontdoor/) and [Purview](https://learn.microsoft.com/purview/sit-document-fingerprinting)).
- **Behavior Analytics**: Identify anomalies through device/user usage patterns; consider ISP, connection types, and access behaviors for incident adjudication.
- **Layered Detection**: Deploy multiple fingerprinting solutions for comprehensive detection ([NIST guidance](https://learn.microsoft.com/azure/compliance/offerings/offering-nist-800-63)).

### 4. Prioritize Collaboration and Learning

- **Threat Intelligence Sharing**: Collaborate with other organizations, share incidents and best practices, join industry alliances (GASA, FSISAC, ARC, HISAC, TISAC, see [GASA partnership](https://www.microsoft.com/security/blog/2025/05/05/microsoft-partners-with-global-anti-scam-alliance-to-fight-cybercrime/)).
- **Continuous Learning**: Stay informed via [Microsoft’s Security blog](https://www.microsoft.com/security/blog/), [CISO Digest](https://info.microsoft.com/ww-landing-subscribe-to-ciso-digest.html?lcid=en-us), and various security-focused forums.

---

**Final Thoughts:**
Building a solid security foundation combines technical best practices and organizational transparency. Establishing cyber hygiene, modernizing security standards, deploying advanced detection/fingerprinting, and fostering collaboration equips teams to block common attacks and manage future threats.

For more OCISO insights, visit the [Deputy CISO blog series](https://www.microsoft.com/en-us/security/blog/topic/office-of-the-ciso/).

This post appeared first on "Microsoft News". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2025/12/04/cybersecurity-strategies-to-prioritize-now/)
