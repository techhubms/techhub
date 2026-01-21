---
external_url: https://www.microsoft.com/en-us/security/blog/2025/08/21/think-before-you-clickfix-analyzing-the-clickfix-social-engineering-technique/
title: Analyzing the ClickFix Social Engineering Technique and Defenses with Microsoft Security
author: Microsoft Threat Intelligence and Microsoft Defender Experts
feed_name: Microsoft Security Blog
date: 2025-08-21 16:00:00 +00:00
tags:
- Attack Chain
- ClickFix
- Detection
- Drive By Compromise
- Endpoint Protection
- Incident Response
- Infostealer
- LOLBins
- Macos Security
- Malvertising
- Malware
- Microsoft Defender
- Microsoft Defender For Office 365
- Microsoft Defender XDR
- Microsoft Security Copilot
- Microsoft Threat Intelligence
- Phishing
- PowerShell
- RunMRU
- Security Mitigation
- Social Engineering
- Windows Security
section_names:
- azure
- security
---
Microsoft Threat Intelligence and Defender Experts offer a comprehensive analysis of the evolving ClickFix social engineering campaigns targeting enterprise users, with detailed recommendations for detecting, responding, and mitigating these attacks using Microsoft security solutions.<!--excerpt_end-->

# Analyzing the ClickFix Social Engineering Technique and Defenses with Microsoft Security

## Overview

Microsoft Threat Intelligence and Microsoft Defender Experts have observed a surge in "ClickFix" social engineering attacks. These campaigns target enterprise and end-users by tricking them into running malicious commands, often leading to information theft, remote access, or malware delivery. The article analyzes how these campaigns operate, showcases real-world attack chains, and provides actionable recommendations for detection and mitigation.

## How ClickFix Campaigns Work

### Attack Chain Summary

- **Initial Access:** Threat actors rely on phishing emails, malvertising, or compromised websites to lure victims to fake landing pages.
- **User Interaction:** The landing page imitates familiar verification dialogs (CAPTCHA, Cloudflare Turnstile, Discord server joins, etc.) and instructs users to copy and run commands in the Windows Run dialog, PowerShell, or Terminal.
- **Payload Delivery:** Executed commands often download infostealers (like Lumma Stealer, Lampion), RATs (Xworm, AsyncRAT), loaders, or rootkits—sometimes using fileless, in-memory execution through LOLBins (living-off-the-land binaries).

### Notable Observations

- ClickFix campaigns have evolved, using sophisticated obfuscation and delivery chain tactics to evade automated detection.
- Attacks have expanded beyond Windows to macOS devices, targeting a wider set of victims and employing OS-specific techniques.
- Cybercriminals sell ClickFix builder kits and services to other threat actors, accelerating attack proliferation.

## Case Studies

### Lampion Malware Campaign

- Targeted organizations in Portugal and other countries.
- Utilized phishing emails with ZIP file attachments, guiding users through multiple obfuscated steps to ultimately attempt delivering Lampion infostealer.
- Attackers made use of scheduled tasks, multi-stage scripts, and persistence via DLL execution.

### Phishing Impersonation of US Social Security Administration (SSA)

- Employed realistic phishing emails with redirectors masking malicious landing pages.
- Final landing page mimicked the SSA and contained ClickFix instructions leading to the installation of tools like ScreenConnect for remote control.

### Malvertising and Drive-By Attacks

- Fake movie streaming sites, compromised WordPress installations, and other platforms have all been leveraged to funnel victims into ClickFix lures.

## Technical Deep Dive

### Detecting ClickFix Activity

- Look for suspicious entries in the Windows RunMRU registry key (`CurrentVersion\Explorer\RunMRU`) involving LOLBins (e.g., powershell, mshta, rundll32), or suspicious file extensions and URLs.
- Campaigns often employ obfuscation, such as Base64 encoding, string concatenation, escape characters, and nested execution patterns.
- Defender for Endpoint and Defender XDR can surface relevant threat alerts, such as suspicious command usage, scheduled tasks, or flagged malware.

### Notable Indicators of Compromise (IOCs)

- Domains: `mein-lonos-cloude.de`, `access-ssa-gov.es`, `binancepizza.info`, etc.
- Malware Samples: Infostealers, rootkits, ransomware loaders.
- Key Tactics: Copy-paste social engineering, drive-by payload delivery, script obfuscation, abuse of trusted Windows/macOS binaries.

### Hunting and Detection with Microsoft Defender XDR & Sentinel

- Defender XDR and Sentinel queries are included to discover registry manipulations, PowerShell behaviors, file hashes, and network indicators associated with ClickFix.
- Example queries target RunMRU activity, process trees involving suspicious parent-child relationships, network connections to known C2 infrastructure, and more.

## Hardening and Defense Recommendations

- Educate users to recognize and avoid suspicious prompts, especially those involving the Run dialog, PowerShell, or terminal windows.
- Enable and configure [Microsoft Defender for Endpoint](https://www.microsoft.com/en-us/security/business/endpoint-security/microsoft-defender-endpoint) and [Microsoft Defender for Office 365](https://www.microsoft.com/en-us/security/business/siem-and-xdr/microsoft-defender-office-365) for advanced threat detection, link and attachment analysis, and blocking of malicious campaigns.
- Implement Group Policy to disable unnecessary access to the Run dialog, restrict PowerShell execution, and limit native binary launches.
- Leverage [Attack Surface Reduction (ASR) rules](https://learn.microsoft.com/defender-endpoint/attack-surface-reduction-rules-deployment-implement) and [script block logging](https://learn.microsoft.com/en-us/powershell/scripting/security/security-features?view=powershell-7.5).
- Use [network protection](https://learn.microsoft.com/defender-endpoint/network-protection) to block C2 domains and restrict access to suspicious infrastructure.

## Microsoft Security Solutions Involved

- **Microsoft Defender XDR**: Coordinates detection, prevention, investigation, and response across endpoints, identities, and cloud services.
- **Microsoft Defender Antivirus / Endpoint**: Provides behavioral, file, and network-based detection.
- **Microsoft Defender for Office 365**: Advanced email protection and phishing defense.
- **Microsoft Security Copilot**: Automates investigation and response using threat intelligence and pre-built promptbooks.

## References and Additional Resources

- [Microsoft Threat Intelligence Blog](https://aka.ms/threatintelblog)
- [Microsoft Defender Experts for XDR](https://www.microsoft.com/security/business/services/microsoft-defender-experts-xdr)
- [Microsoft Security Copilot in Defender](https://learn.microsoft.com/en-us/defender-xdr/security-copilot-in-microsoft-365-defender)
- [Securonix OBSCURE#BAT Analysis](https://www.securonix.com/blog/analyzing-obscurebat-threat-actors-lure-victims-into-executing-malicious-batch-scripts-to-deploy-stealthy-rootkits/)
- [CloudSek AMOS ClickFix Report](https://www.cloudsek.com/blog/amos-variant-distributed-via-clickfix-in-spectrum-themed-dynamic-delivery-campaign-by-russian-speaking-hackers)

## Conclusion

The ClickFix social engineering technique is an increasingly prevalent and dangerous threat vector. With evolving lures, sophisticated delivery chains, and multi-stage malware, this attack requires layered detection and defense. Microsoft Defender XDR, Defender for Endpoint, Defender for Office 365, and Security Copilot provide end-to-end protection and guidance, but user education and hardening policies remain essential for prevention.

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2025/08/21/think-before-you-clickfix-analyzing-the-clickfix-social-engineering-technique/)
