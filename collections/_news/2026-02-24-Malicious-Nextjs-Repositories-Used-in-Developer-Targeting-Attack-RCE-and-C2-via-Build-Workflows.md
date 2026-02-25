---
external_url: https://www.microsoft.com/en-us/security/blog/2026/02/24/c2-developer-targeting-campaign/
title: 'Malicious Next.js Repositories Used in Developer-Targeting Attack: RCE and C2 via Build Workflows'
author: Microsoft Defender Experts and Microsoft Defender Security Research Team
primary_section: azure
feed_name: Microsoft Security Blog
date: 2026-02-24 17:28:24 +00:00
tags:
- Advanced Hunting
- Attack Surface Reduction
- Azure
- Build Security
- C2 Infrastructure
- Cloud Apps Security
- Command And Control
- Conditional Access
- DevOps
- Incident Response
- JavaScript Loader
- Malicious Repositories
- Microsoft Defender
- Microsoft Defender For Endpoint
- Microsoft Defender XDR
- Microsoft Entra ID Protection
- Microsoft Sentinel
- News
- Next.js
- Node.js
- RCE
- Security
- Threat Hunting
- Vercel
- VS Code
section_names:
- azure
- devops
- security
---
Microsoft Defender Experts and the Security Research Team provide an in-depth report on a developer-targeted campaign using malicious Next.js repositories that exploit common coding workflows. The analysis details how attackers achieve remote code execution and persistent C2, with actionable security guidance.<!--excerpt_end-->

# Malicious Next.js Repositories Used in Developer-Targeting Attack: RCE and C2 via Build Workflows

## Overview

Microsoft Defender Experts identified a coordinated campaign targeting developers using malicious repositories disguised as legitimate Next.js projects and technical assessment materials. The attackers exploited standard developer tools and workflows—such as Visual Studio Code and Node.js—to covertly achieve remote code execution (RCE) and establish a multi-stage command-and-control (C2) channel, blending seamlessly with normal development activity.

## Threat Discovery and Analysis

- **Initial vector:** Attackers delivered Bitbucket- and public platform-hosted repositories that appeared as technical assessments (often recruiting-themed).
- **Pivoting methods:** Analysts mapped additional related repositories by analyzing repo naming conventions, structural reuse, and loader logic.
- **Entry points:** Malicious loaders were triggered via:
    - **VS Code workspace trust mechanisms** (`.vscode/tasks.json` and obfuscated startup scripts)
    - **Build-time assets** (e.g., compromised `jquery.min.js` executed during `npm run dev` or server launch)
    - **Server startup routines** (backend modules leveraging encoded endpoints in `.env` files for dynamic code loading and exfiltration)

## Attack Chain Explained

- **Stage 1:** After initial execution (opening the project, running dev server, or importing backend modules), a lightweight script is downloaded from attacker-controlled infrastructure. This script profiles the developer’s host and repeatedly communicates with a registration endpoint.
- **Stage 2:** The second-stage payload upgrades the foothold, polling for commands and executing further attacker-supplied JavaScript tasks in-memory, supporting directory enumeration, credential theft, and staged data exfiltration.

### Telemetry and Behavior

- **Process telemetry linked Node.js and VS Code processes to C2 endpoints.**
- **Attackers used Vercel staging servers and multiple domain variants** for loader delivery and tracking.
- **Malicious code** avoided on-disk artifacts by in-memory execution and dynamic function evaluation.
- **Exfiltration** targeted sensitive environment variables, credentials, and high-value source assets.

## Mitigations and Security Recommendations

- **Containment:** Rapidly scope and isolate suspected developer endpoints using advanced endpoint telemetry (e.g., Advanced Hunting in Microsoft Defender).
- **Identity Protection:** Employ Microsoft Entra ID Protection to triage risky sessions and remediate credential/session theft.
- **Conditional Access Controls:** Use Microsoft Defender for Cloud Apps to reduce data exfiltration risk and monitor suspicious app sessions.
- **IDE Hardening:** Default to restricted mode in VS Code for unknown repositories (Workspace Trust), and review all automation scripts before trusting a project.
- **Endpoint Security:** Enforce Attack Surface Reduction rules, keep Microsoft Defender Antivirus cloud-delivered protection enabled, and use SmartScreen reputation controls.
- **Centralized Monitoring:** Enhance threat hunting with Microsoft Sentinel hunting rules matching observed malicious behaviors (e.g., Node.js beacons, suspicious file access patterns).
- **Operational Best Practices:** Separate production/development credentials, apply least privilege principles, and minimize secrets on developer machines.

## Microsoft Defender XDR Coverage

Integrated and coordinated detection and response is available using Defender for Endpoint (behavioral and attack surface reduction alerts), Defender for Cloud Apps (session governance and DLP), and Sentinel (analytics and hunting rules).

## Indicators of Compromise

- **Malicious domains/IPs:** Multiple Vercel subdomains and C2 server addresses (see full list in original research)
- **File/Artifact:** Suspicious repo files such as `.vscode/tasks.json`, `jquery.min.js`, and backend modules (e.g., `auth.js`)

## Example Hunting Queries

KQL queries for threat hunting are provided, including:

- Detecting Node.js fetching remote scripts from untrusted domains
- Identifying dynamic loader behavior in `next.config.js`
- Beaconing to C2 endpoints
- Suspicious access to sensitive files

## References and Defense-in-Depth

For further research, see referenced Microsoft Learn documents on Defender XDR, Conditional Access, Entra ID Protection, and Sentinel. Adopt operational practices to strengthen trust boundaries and secure developer workflows against emerging threats.

---

**Attribution:** Research by Microsoft Defender Security Research, with contributions from Colin Milligan.

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2026/02/24/c2-developer-targeting-campaign/)
