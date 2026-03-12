---
layout: "post"
title: "Storm-2561 Distributes Fake VPN Clients via SEO Poisoning for Credential Theft"
description: "This Microsoft Threat Intelligence and Defender Experts report analyzes the Storm-2561 threat actor's credential theft campaign. Attackers use SEO poisoning to push fake VPN installers hosted on GitHub, exploiting trusted brands and abusing code signing to steal VPN credentials. The post covers detailed TTPs, indicators of compromise, technical attack chain analysis, Microsoft Defender mitigation guidance, Security Copilot capabilities, and links to hunting resources for threat response."
author: "Microsoft Threat Intelligence and Microsoft Defender Experts"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.microsoft.com/en-us/security/blog/2026/03/12/storm-2561-uses-seo-poisoning-to-distribute-fake-vpn-clients-for-credential-theft/"
viewing_mode: "external"
feed_name: "Microsoft Security Blog"
feed_url: "https://www.microsoft.com/en-us/security/blog/feed/"
date: 2026-03-12 17:00:00 +00:00
permalink: "/2026-03-12-Storm-2561-Distributes-Fake-VPN-Clients-via-SEO-Poisoning-for-Credential-Theft.html"
categories: ["Security"]
tags: ["Attack Chain", "Code Signing Abuse", "Credential Theft", "DLL Side Loading", "GitHub", "Hyrax", "Incident Response", "Indicators Of Compromise", "Infostealer", "Malware", "Microsoft Defender", "Microsoft Defender Experts", "Microsoft Security Copilot", "Mitigation", "News", "Persistence Mechanism", "Security", "SEO Poisoning", "Social Engineering", "Storm", "Storm 2561", "Threat Intelligence", "TTPs", "VPN"]
tags_normalized: ["attack chain", "code signing abuse", "credential theft", "dll side loading", "github", "hyrax", "incident response", "indicators of compromise", "infostealer", "malware", "microsoft defender", "microsoft defender experts", "microsoft security copilot", "mitigation", "news", "persistence mechanism", "security", "seo poisoning", "social engineering", "storm", "storm 2561", "threat intelligence", "ttps", "vpn"]
---

Microsoft Threat Intelligence and Microsoft Defender Experts detail how Storm-2561 uses SEO poisoning to distribute fake, signed VPN clients, stealing user credentials through technical deception and advanced evasion techniques.<!--excerpt_end-->

# Storm-2561 Distributes Fake VPN Clients via SEO Poisoning for Credential Theft

*By Microsoft Threat Intelligence and Microsoft Defender Experts*

## Overview

In January 2026, Microsoft Defender Experts uncovered a credential theft campaign orchestrated by Storm-2561, featuring fake VPN installers distributed via SEO poisoning. This sophisticated operation targeted users searching for legitimate VPN clients, tricking them into downloading trojanized, digitally signed software that exfiltrates credentials.

## Threat Actor: Storm-2561

Storm-2561 has been active since 2025 and is noted for distributing malware through SEO manipulation and impersonating popular enterprise software. The group’s campaigns exploit both user trust in search engine rankings and brand familiarity.

For information on Microsoft's threat actor naming conventions, see [here](https://learn.microsoft.com/unified-secops/microsoft-threat-actor-naming).

## Attack Chain: From Search to Stolen Credentials

1. **SEO Poisoning and Initial Access**
   - Attackers use SEO techniques to have fake VPN download sites (e.g., *vpn-fortinet[.]com*, *ivanti-vpn[.]org*) appear in search results for queries like "Pulse VPN download".
   - These sites host links to malicious ZIP files located on GitHub repositories (now removed).

2. **Payload Delivery and Execution**
   - ZIP file contains a Microsoft Windows Installer (MSI) that mimics VPN clients and includes malicious DLL files (*dwmapi.dll*, *inspector.dll*) alongside the fake executable.
   - Files are signed by “Taiyuan Lihua Near Information Technology Co., Ltd.” to bypass security controls.

3. **Credential Harvesting and Persistence**
   - The installed fake VPN shows a convincing interface, capturing user-entered credentials and stealing stored VPN data before exfiltrating it to attacker infrastructure.
   - Uses Windows RunOnce registry key to grant persistence on the system.

4. **Defense Evasion**
   - Following credential theft, the malware prompts users to download the real VPN client and, in some cases, opens the legitimate site to minimize user suspicion.
   - Code signing is abused to reduce detection.

## Technical Analysis

- **Malicious DLLs dropped** to legit-looking paths (e.g., `%CommonFiles%\Pulse Secure`).
- **In-memory shellcode loader** drops and launches infostealer Hyrax, extracting VPN credentials and configuration data.
- **Persistence** via *RunOnce* registry key.
- **Digital certificates** used to sign malware, adding false trust.

## Indicators of Compromise (IOCs)

- Multiple malicious SHA-256 hashes listed for various fake VPN installers and DLLs.
- Attacker-controlled domains host initial payloads and act as exfiltration C2s (e.g., *vpn-fortinet[.]com*, *ivanti-vpn[.]org*, *194.76.226[.]93*).
- Example GitHub URL: `hxxps[:]//github[.]com/latestver/vpn/releases/download/vpn-client2/VPN-CLIENT.zip`.

For full IOC tables and additional technical details, refer to the full post and linked hunting queries.

## Defender Mitigation and Hunting Guidance

- **Mitigations:**
  - Enable cloud-delivered protection and EDR block mode in Microsoft Defender.
  - Enable network and web protection, enforce MFA, and block executable files by prevalence/age/trusted list criteria.
  - Implement attack surface reduction rules, and reinforce browser SmartScreen usage.
- **Hunting:**
  - Provided KQL queries to identify files signed by the actor’s certificates and spot suspicious DLL activity in typical VPN product folders.

## Microsoft Security Copilot Integration

- Security Copilot can summarize incidents, analyze files/scripts, and produce automated reports on threats like Storm-2561.
- Integration with Microsoft Defender XDR and bespoke AI agents enables advanced investigation and response workflows.
- Further details on using [Security Copilot developer scenarios](https://learn.microsoft.com/copilot/security/developer/custom-agent-overview) provided.

## References & Further Reading

- [Microsoft Threat Intelligence Blog](https://aka.ms/threatintelblog)
- [Threat Analytics: Storm-2561](https://security.microsoft.com/threatanalytics3/a9b575b6-1705-44b1-93c2-9edad079e864/overview)
- External analysis: [Zscaler](https://www.zscaler.com/blogs/security-research/spoofed-ivanti-vpn-client-sites), [Cyjax](https://www.cyjax.com/resources/blog/a-sting-on-bing-bumblebee-delivered-through-bing-seo-poisoning-campaign)

---

**Author:** Microsoft Threat Intelligence and Microsoft Defender Experts

## Learn More

- Follow Microsoft Threat Intelligence on [LinkedIn](https://www.linkedin.com/showcase/microsoft-threat-intelligence) and [X (formerly Twitter)](https://x.com/MsftSecIntel).
- Listen to the [Microsoft Threat Intelligence podcast](https://thecyberwire.com/podcasts/microsoft-threat-intelligence).

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2026/03/12/storm-2561-uses-seo-poisoning-to-distribute-fake-vpn-clients-for-credential-theft/)
