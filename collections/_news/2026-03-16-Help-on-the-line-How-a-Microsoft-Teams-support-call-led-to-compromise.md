---
primary_section: security
feed_name: Microsoft Security Blog
tags:
- Allowlist
- Command And Control
- Credential Harvesting
- Credential Phishing
- DART
- DLL Sideloading
- Encrypted Loader
- External Collaboration Restrictions
- Identity First Intrusion
- Lateral Movement
- Microsoft Incident Response
- Microsoft Teams
- MSI
- News
- Phishing
- Quick Assist
- Remote Access
- Security
- Session Hijacking
- Social Engineering
- Spoofed Login Page
- Vishing
- Voice Phishing
date: 2026-03-16 16:00:00 +00:00
external_url: https://www.microsoft.com/en-us/security/blog/2026/03/16/help-on-the-line-how-a-microsoft-teams-support-call-led-to-compromise/
author: Microsoft Incident Response
section_names:
- security
title: 'Help on the line: How a Microsoft Teams support call led to compromise'
---

Microsoft Incident Response (DART) investigates a Microsoft Teams voice-phishing incident where an attacker impersonated IT support, used Quick Assist for remote access, and stole credentials via a spoofed site—then shares concrete defensive steps to reduce this kind of identity-led compromise.<!--excerpt_end-->

## Overview

In this Cyberattack Series report, **Microsoft Incident Response (DART)** describes an **identity-first, human-operated intrusion** that relied on **deception and legitimate tools** rather than exploiting software vulnerabilities.

A threat actor:

- Impersonated IT support over **Microsoft Teams voice phishing (vishing)**
- Convinced a user to grant **remote access via Quick Assist**
- Redirected the user to a **malicious website** to collect credentials
- Deployed multiple payloads to establish and expand access

## What happened?

After gaining interactive access through Quick Assist, the attacker moved from social engineering to hands-on compromise:

- The user was guided to a **spoofed web form** and entered **corporate credentials**.
- The phishing flow triggered downloads of multiple malicious payloads.
- An early artifact was a disguised **Microsoft Installer (MSI)** that abused trusted Windows behavior to:
  - **Sideload a malicious DLL**
  - Establish **outbound command-and-control (C2)**
  - Execute code “under the guise of legitimate software”

Subsequent payloads:

- Added **encrypted loaders**
- Enabled **remote command execution** via standard admin tooling
- Used **proxy-based connectivity** to hide activity
- Introduced capabilities for **credential harvesting** and **session hijacking**

The overall theme: techniques designed to blend into normal enterprise activity rather than trigger obvious alarms.

## How did Microsoft respond?

DART focused on quickly validating scope and preventing identity/directory impact:

- Confirmed initial access originated from **Microsoft Teams vishing**.
- Prioritized actions to prevent **identity or directory-level** impact.
- Performed targeted eviction and containment to protect privileged assets and reduce lateral movement.
- Collected and analyzed evidence across affected systems.
- Confirmed objectives were not met and found **no persistence mechanisms**.

## What can customers do to strengthen their defenses?

DART’s mitigation guidance focuses on reducing how collaboration and built-in remote access tools can be abused:

- **Tighten external collaboration in Microsoft Teams**:
  - Restrict inbound communications from **unmanaged Teams accounts**
  - Use an **allowlist model** permitting contact only from trusted external domains
- **Review and minimize remote monitoring/management tools**:
  - Inventory what’s actually required
  - Remove/disable utilities such as **Quick Assist** where unnecessary

These steps aim to reduce attack surface and limit opportunities for **identity-driven compromise**.

## References

- Report: Cyberattack Series PDF (Q3): https://cdn-dynmedia-1.microsoft.com/is/content/microsoftcorp/microsoft/bade/documents/products-and-services/en-us/security/cyberattacks-series-report-q3-final.pdf
- Microsoft Incident Response (DART): https://www.microsoft.com/security/business/microsoft-incident-response
- DLL background (Microsoft Learn): https://learn.microsoft.com/en-us/troubleshoot/windows-client/setup-upgrade-and-drivers/dynamic-link-library
- Microsoft Digital Defense Report 2025: https://www.microsoft.com/corporate-responsibility/cybersecurity/microsoft-digital-defense-report-2025/


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/03/16/help-on-the-line-how-a-microsoft-teams-support-call-led-to-compromise/)

