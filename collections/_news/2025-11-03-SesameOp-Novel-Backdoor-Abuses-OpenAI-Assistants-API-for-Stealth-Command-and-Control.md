---
layout: "post"
title: "SesameOp: Novel Backdoor Abuses OpenAI Assistants API for Stealth Command and Control"
description: "This in-depth analysis from Microsoft Incident Response’s DART team explores the discovery of a sophisticated backdoor, SesameOp, which leverages the OpenAI Assistants API for command-and-control (C2) operations. The post discusses how attackers use this API to relay commands and results, details the technical implementation of the malware, its obfuscation and persistence mechanisms, and the coordinated investigation and response with OpenAI. The article also provides practical mitigation steps and threat hunting queries for defenders."
author: "Microsoft Incident Response"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.microsoft.com/en-us/security/blog/2025/11/03/sesameop-novel-backdoor-uses-openai-assistants-api-for-command-and-control/"
viewing_mode: "external"
feed_name: "Microsoft Security Blog"
feed_url: "https://www.microsoft.com/en-us/security/blog/feed/"
date: 2025-11-03 17:00:00 +00:00
permalink: "/news/2025-11-03-SesameOp-Novel-Backdoor-Abuses-OpenAI-Assistants-API-for-Stealth-Command-and-Control.html"
categories: ["AI", "Security"]
tags: [".NET", "AI", "API Abuse", "AppDomainManager Injection", "Backdoor", "Command And Control", "Cybersecurity", "Eazfuscator.NET", "Encryption", "Incident Response", "Malware", "Microsoft DART", "Microsoft Defender", "News", "OpenAI Assistants API", "Security", "SesameOp", "Threat Intelligence", "VS"]
tags_normalized: ["dotnet", "ai", "api abuse", "appdomainmanager injection", "backdoor", "command and control", "cybersecurity", "eazfuscatordotnet", "encryption", "incident response", "malware", "microsoft dart", "microsoft defender", "news", "openai assistants api", "security", "sesameop", "threat intelligence", "vs"]
---

Microsoft Incident Response’s DART team, led by their security researchers, uncovers and analyzes SesameOp—a covert backdoor abusing the OpenAI Assistants API for command and control, with detailed technical breakdown and defense guidance.<!--excerpt_end-->

# SesameOp: Novel Backdoor Abuses OpenAI Assistants API for Command and Control

## Overview

Microsoft Incident Response’s Detection and Response Team (DART) has uncovered a sophisticated backdoor, named **SesameOp**, that uses the OpenAI Assistants API for command-and-control (C2) operations rather than traditional infrastructure. This allows threat actors to stealthily orchestrate malicious activities inside compromised systems.

## Attack Analysis

- **Discovery**: Initial detection occurred during a response to a long-term intrusion involving web shells and malicious .NET libraries loaded through Visual Studio utilities.
- **Unique Approach**: Instead of conventional C2 channels, SesameOp relays commands and responses using the OpenAI Assistants API. This abuse of a legitimate service complicates detection and remediation.

## Technical Details

### Infection Chain

- **Loader (Netapi64.dll)**: Obfuscated with Eazfuscator.NET, loaded at runtime via .NET AppDomainManager injection from a crafted config file. Ensures persistence and avoids duplicate instances with mutex markers.
- **Backdoor (OpenAIAgent.Netapi64)**: Main payload communicates with OpenAI’s API, fetching encrypted payload instructions and posting results using assistant, thread, and message mechanisms within the API. Payloads and results are layered with symmetric and asymmetric encryption and compressed to remain hidden from analysts and defenders.

### Core Mechanisms

- Uses configuration split across API key, selector, and optional proxy address.
- Establishes communication by querying and creating OpenAI Assistants and vector stores, with host information and instructions encoded into API usage.
- Command messages are encrypted with dynamically generated AES keys, which themselves are encrypted with hardcoded RSA keys.
- Payloads results are similarly compressed, encrypted, and returned via the API.
- Execution utilizes JScript via the .NET VsaEngine, with dynamic module loading and in-memory code evaluation for task execution.

### Mitigations

- Frequently review firewalls, web server logs, and restrict unnecessary Internet exposure.
- Use Microsoft Defender Firewall, enable tamper protection, and ensure endpoint detection and response in block mode.
- Regularly update and enforce proxy and network rules to block unauthorized C2 channels.
- Activate cloud-delivered and real-time protection in Microsoft Defender.

### Detections

- Microsoft Defender Antivirus detects SesameOp as **Trojan:MSIL/Sesameop.A** and **Backdoor:MSIL/Sesameop.A**.
- Defender XDR can be used to analyze device connections to OpenAI API endpoints with provided Kusto queries.

### Additional Resources

- Detailed threat hunting queries, detection recommendations, and further research references are available through Microsoft’s official security blogs and threat intelligence community channels.

## Summary Table

| Component                   | Role                                           |
|----------------------------|------------------------------------------------|
| Netapi64.dll                | Loader; ensures persistence and stealth        |
| OpenAIAgent.Netapi64        | Backdoor; C2 logic via OpenAI API             |
| Eazfuscator.NET             | Obfuscation tool for .NET assemblies           |
| AppDomainManager Injection  | Loading mechanism for DLL and persistence      |

## Guidance for Defenders

- Monitor for unusual .NET process behavior or AppDomainManager injection alerts.
- Investigate any outbound connections to api.openai.com from production or critical systems.
- Employ layered encryption/compression detection techniques for memory-resident threats.
- Leverage Microsoft Defender XDR and Security Copilot for automated investigation and response.

## Conclusion

The SesameOp backdoor highlights the evolving landscape of threat actor tactics, leveraging legitimate APIs in novel ways. Collaboration between Microsoft and OpenAI was critical in neutralizing the abused API keys and understanding threat vectors. Organizations should stay vigilant, follow published defense guidance, and leverage the latest threat intelligence.

For continued updates, visit the [Microsoft Security Blog](https://www.microsoft.com/en-us/security/blog) and follow Microsoft Threat Intelligence on social media and podcast platforms.

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2025/11/03/sesameop-novel-backdoor-uses-openai-assistants-api-for-command-and-control/)
