---
layout: "post"
title: "Malicious AI Browser Extensions Expose LLM Chat Histories: Microsoft Defender Analysis"
description: "This Microsoft Defender Security Research Team report investigates a widespread malware campaign using malicious AI-themed Chromium browser extensions. The extensions, distributed via legitimate marketplaces, harvest large language model (LLM) chat histories and browsing data—posing significant privacy, compliance, and security threats to enterprises by targeting AI usage and exfiltrating sensitive data."
author: "Microsoft Defender Security Research Team"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.microsoft.com/en-us/security/blog/2026/03/05/malicious-ai-assistant-extensions-harvest-llm-chat-histories/"
viewing_mode: "external"
feed_name: "Microsoft Security Blog"
feed_url: "https://www.microsoft.com/en-us/security/blog/feed/"
date: 2026-03-05 16:02:12 +00:00
permalink: "/2026-03-05-Malicious-AI-Browser-Extensions-Expose-LLM-Chat-Histories-Microsoft-Defender-Analysis.html"
categories: ["AI", "Security"]
tags: ["AI", "AI Security", "Browser Extension Security", "Browser Telemetry", "ChatGPT", "Chromium", "Data Exfiltration", "DeepSeek", "Defender For Endpoint", "Enterprise Security", "Google Chrome", "LLM Security", "Malware", "Microsoft Defender", "Microsoft Edge", "News", "Purview", "Security", "SmartScreen"]
tags_normalized: ["ai", "ai security", "browser extension security", "browser telemetry", "chatgpt", "chromium", "data exfiltration", "deepseek", "defender for endpoint", "enterprise security", "google chrome", "llm security", "malware", "microsoft defender", "microsoft edge", "news", "purview", "security", "smartscreen"]
---

The Microsoft Defender Security Research Team analyzes how malicious AI-themed browser extensions harvest LLM chat histories and enterprise data, highlighting significant security risks.<!--excerpt_end-->

# Malicious AI Browser Extensions Expose LLM Chat Histories: Microsoft Defender Analysis

## Overview

Malicious AI-themed Chromium browser extensions have been discovered impersonating legitimate tools to collect large language model (LLM) chat histories and browsing data from platforms like ChatGPT and DeepSeek. With close to 900,000 installs spanning over 20,000 enterprise tenants, the scale of this campaign accentuates the emerging risks posed by browser extensions in corporate settings—particularly as knowledge workers increasingly interact with AI tools in the browser.

## Attack Chain

![Attack chain illustrating how malicious AI extensions move from distribution to data exfiltration.](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/image-21.webp)

### Reconnaissance

- Threat actors targeted the AI assistant extension ecosystem, exploiting user trust and behaviors.
- Many users granted broad permissions to extensions named and designed to resemble familiar AI productivity tools.
- Some agentic browsers even auto-installed these extensions without explicit consent.
- Malicious extensions mimicked legitimate offerings like AITOPIA, aligning branding and consent prompts to user expectations.

### Weaponization

- A single malicious Chromium-based extension, compatible with both Chrome and Edge, observed users' page navigations and captured AI chat content.
- Data was staged locally before periodic exfiltration, providing actors with continuous access to organizational browsing and AI usage telemetry.
- Activity was disguised as benign analytics to minimize suspicion.

### Delivery

- Distributed through the Chrome Web Store with authentic-sounding AI-related branding.
- Extensions installed across Chrome and Edge, reaching large user bases through normal productivity tool adoption.

### Exploitation

- Extensions leveraged Chrome's permission model for post-installation data access.
- Default or auto-reenabled telemetry meant even users who had opted out could unwittingly share sensitive data.

### Persistence

- Extensions persisted via normal browser behavior, reloading on browser restart without requiring high privileges.
- Local storage allowed session continuity and queued telemetry through restarts.

### Command and Control (C2)

- Data (including URLs, chat content, context, and model names) exfiltrated via HTTPS POST requests to attacker-controlled domains such as `deepaichats[.]com` and `chatsaigpt[.]com`.
- Minimal filtering and weak consent controls enabled stealthy, regular exfiltration while reducing forensic footprints.

### Technical Analysis

- Background scripts captured nearly all navigational and AI chat interactions, saving them as Base64-encoded JSON.
- Telemetry included sensitive URLs, chat snippets, persistent UUIDs, and minimal local retention.

## Impact

- LLM chat histories and application URLs—including those of internal corporate sites—were exposed to threat actors.
- This facilitated data leakage of proprietary code, business workflows, and confidential organizational conversations shared with AI tools.
- The campaign’s success highlights browser extensions as a rising vector for enterprise data compromise, especially where AI adoption is widespread.

## Mitigation & Protection

1. **Monitor for Suspicious Outbound Traffic**: Audit network POST traffic to known attacker endpoints (e.g., `chatsaigpt.com`, `deepaichats.com`, etc.).
2. **Inventory and Restrict Extensions**: Use Defender Vulnerability Management to assess installed browser extensions and set organizational controls.
3. **Enable Protection Mechanisms**: Deploy Defender SmartScreen and Network Protection for broader malware defense.
4. **Implement Data Security**: Use Microsoft Purview to enforce compliance and monitor data flows in browser-based AI scenarios.
5. **Policy and Awareness**: Establish clear AI usage and extension policies, and regularly educate users on safe extension practices.

### Defender XDR Detections & Queries

- Microsoft Defender for Endpoint identifies suspicious extension installs/loads, outbound traffic to malicious domains, and on-disk artifacts in known extension directories.
- Example **KQL queries** are provided in the original post for SOC teams to hunt for execution, exfiltration, and persistence phases.

### Security Copilot Integration

- Security Copilot within Microsoft Defender assists in investigating incidents, hunting threats, and provides relevant AI security intelligence.

## References

- [Malicious Chrome Extensions Steal ChatGPT Conversations](https://www.ox.security/blog/malicious-chrome-extensions-steal-chatgpt-deepseek-conversations/)
- [Microsoft Defender for Endpoint Documentation](https://learn.microsoft.com/en-us/defender-endpoint/microsoft-defender-endpoint)
- [Browser Extensions Assessment in Microsoft Defender](https://learn.microsoft.com/en-us/defender-vulnerability-management/tvm-browser-extensions)
- [Microsoft Defender SmartScreen Overview](https://learn.microsoft.com/en-us/windows/security/operating-system-security/virus-and-threat-protection/microsoft-defender-smartscreen/)
- [Microsoft Purview Data Security for AI](https://learn.microsoft.com/en-us/purview/ai-microsoft-purview)
- [Securing Copilot Studio agents with Microsoft Defender](https://learn.microsoft.com/en-us/defender-cloud-apps/ai-agent-protection)

---

*Research by the Microsoft Defender Security Research Team, with contributions from Geoff McDonald and Dana Baril.*

## Key Takeaways

- Attackers are increasingly targeting enterprises by weaponizing trusted browser extensions—especially those linked to AI tools.
- Enterprises must combine endpoint security, vigilant extension management, user awareness, and cloud data protections to reduce these emerging risks.

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2026/03/05/malicious-ai-assistant-extensions-harvest-llm-chat-histories/)
