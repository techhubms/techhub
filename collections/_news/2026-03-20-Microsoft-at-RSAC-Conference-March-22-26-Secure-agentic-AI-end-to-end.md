---
tags:
- Agentic AI
- AI
- Company News
- Conditional Access
- Container Security
- Defender For Cloud
- DLP
- Identity Security
- ITDR
- MFA
- Microsoft Agent 365
- Microsoft Defender
- Microsoft Entra
- Microsoft Purview
- Microsoft Security
- Microsoft Sentinel
- Model Context Protocol (mcp)
- News
- Passkeys
- Prompt Injection Protection
- Risk Remediation
- RSAC
- Security
- Security Copilot
- Security Dashboard For AI
- Sentinel Playbooks
- Shadow AI Detection
- Zero Trust
section_names:
- ai
- security
date: 2026-03-20 16:10:10 +00:00
primary_section: ai
author: stclarke
external_url: https://www.microsoft.com/en-us/security/blog/2026/03/20/secure-agentic-ai-end-to-end/
title: 'Microsoft at RSAC Conference March 22-26: Secure agentic AI end-to-end'
feed_name: Microsoft News
---

In this RSAC 2026 roundup, stclarke summarizes Microsoft’s updates for securing agentic AI end-to-end, spanning new capabilities for governing agents, hardening identity and data protections, and using Security Copilot, Defender, Entra, Purview, and Sentinel to detect and respond to threats.<!--excerpt_end-->

## Overview
RSAC™ Conference marks its 35-year anniversary, and Microsoft frames this moment around the rise of **agentic AI** and the security risks that come with it (including AI-powered attacks and “double agents”). The core message: **trust starts with security**, and security needs to be embedded across every layer of the “AI estate”.

Microsoft positions its RSAC 2026 announcements around three themes:

- **Secure agents**
- **Secure your foundations** (identity, data, endpoints, cloud, AI services)
- **Defend with agents and experts**

## Secure agentic AI with Microsoft Security
Microsoft points to its scale signals (“more than 100 trillion daily signals”) and broad usage (“24 billion Copilot interactions”) as context for why it is building “purpose-built capabilities” to secure agentic AI.

## Secure agents
### Agent 365 (GA May 1)
Microsoft announced **Agent 365** (the “control plane for agents”), generally available **May 1**.

Agent 365 is positioned as a way for IT, security, and business teams to:

- Observe agents at scale
- Secure agents
- Govern agents

It includes new capabilities across:

- **Microsoft Defender**
- **Microsoft Entra**
- **Microsoft Purview**

These are described as helping:

- Secure agent access
- Prevent data oversharing
- Defend against emerging threats

Agent 365 is included in **Microsoft 365 E7: The Frontier Suite**, along with **Microsoft 365 Copilot**, **Microsoft Entra Suite**, and **Microsoft 365 E5**.

## Secure your foundations
Microsoft groups “foundations” as the systems agentic AI runs on, plus the people developing and using AI.

### Gain visibility into risks across your enterprise
New capabilities intended to help identify where AI is used and where risk exposure is increasing:

- **Security Dashboard for AI**: unified visibility into AI-related risk. *(Generally available now.)*
- **Entra Internet Access Shadow AI Detection**: detects previously unknown AI apps and unmanaged AI usage via the network layer. *(Generally available March 31.)*
- **Enhanced Intune app inventory**: visibility into apps installed on devices, including AI-enabled apps, to enable remediation of high-risk software. *(Generally available in May.)*

### Secure identities with continuous, adaptive access (Microsoft Entra)
Microsoft highlights identity as a primary target and introduces Entra capabilities aimed at resilience, governance, and phishing-resistant authentication:

- **Entra Backup and Recovery**: automated backup for Entra directory objects to enable rapid recovery. *(Preview.)*
- **Entra Tenant Governance**: discover unmanaged (shadow) Entra tenants and enforce consistent multi-tenant governance. *(Preview.)*
- **Entra passkeys**:
  - Synced passkeys and passkey profiles *(generally available)*
  - Optional device-bound passkeys
  - Passkeys natively integrated into **Windows Hello** *(in preview)*
- **Entra external MFA**: connect external MFA providers to Microsoft Entra. *(Generally available.)*
- **Entra adaptive risk remediation**: automatic self-remediation across authentication methods to reduce help-desk friction. *(Generally available in April.)*
- **Unified identity security** (Defender + Entra): end-to-end identity coverage including ITDR, with:
  - A new identity security dashboard in **Microsoft Defender**
  - A new identity risk score that unifies account-level risk signals *(Preview.)*

### Safeguard sensitive data across AI workflows (Microsoft Purview)
Microsoft focuses on data flowing through prompts, responses, and grounding. New Purview capabilities include:

- **Expanded Purview DLP for Microsoft 365 Copilot**: block sensitive data (PII, credit card numbers, custom data types) from being processed or used for web grounding. *(Generally available March 31.)*
- **Purview embedded in Copilot Control System**: unified view of AI-related data risk in the Microsoft 365 Admin Center. *(Generally available in April.)*
- **Purview customizable data security reports**: tailored reporting/drilldowns for prioritized data risks. *(Preview March 31.)*

### Defend against threats across endpoints, cloud, and AI services
Microsoft describes improvements across network, container, cloud posture, and identity protection:

- **Entra Internet Access prompt injection protection**: block malicious AI prompts via universal network-level policies. *(Generally available March 31.)*
- **Enhanced Defender for Cloud container security**: binary drift and antimalware prevention. *(Preview.)*
- **Defender for Cloud posture management**: broader resource coverage and support for **AWS** and **GCP**, providing recommendations and compliance insights. *(Preview in April.)*
- **Defender predictive shielding**: dynamically adjusts identity/access policies during active attacks. *(Preview.)*

## Defend with agents and experts
### Agents built into the flow of security work (Security Copilot)
**Security Copilot** is described as now included in **Microsoft 365 E5 and E7** and focused on accelerating investigations and reducing manual effort.

New agents include:

- **Security Analyst Agent in Microsoft Defender**: contextual analysis and guided workflows for investigations. *(Preview March 26.)*
- **Security Alert Triage Agent in Microsoft Defender**: autonomously analyzes/classifies/prioritizes/resolves repetitive alerts; extends beyond phishing to cloud and identity. *(Preview in April.)*
- **Conditional Access Optimization Agent** (Entra enhancements): context-aware recommendations, deeper analysis, and phased rollout. *(Agent GA; enhancements preview.)*
- **Data Security Posture Agent** (Purview enhancements): includes credential scanning to detect credential exposure. *(Preview.)*
- **Data Security Triage Agent** (Purview enhancements): advanced reasoning layer and better interpretation of custom Sensitive Information Types (SITs). *(Agent GA; enhancements preview March 31.)*
- “Over 15” partner-built agents available via the **Security Store**.

### Scale with an agentic defense platform (Microsoft Sentinel)
Microsoft describes Sentinel expansion to unify context, automate workflows, and standardize access/governance/deployment across security solutions.

New Sentinel capabilities include:

- **Sentinel data federation powered by Microsoft Fabric**: investigate external security data in place (Databricks, Microsoft Fabric, Azure Data Lake Storage) while preserving governance. *(Preview.)*
- **Sentinel playbook generator with natural language orchestration**: speed up investigation automation. *(Preview.)*
- **Granular delegated admin privileges + unified RBAC**: scaling management for partners and cross-tenant collaboration. *(Preview.)*
- **Security Store embedded in Purview and Entra**: discover/deploy agents inside existing experiences. *(Generally available March 31.)*
- **Sentinel custom graphs** powered by Microsoft Fabric: organization-specific relationship views. *(Preview.)*
- **Sentinel MCP entity analyzer**: uses natural language plus “flexibility of code” to accelerate responses. *(Generally available in April.)*

### Strengthen with experts
Microsoft highlights the **Microsoft Defender Experts Suite** (technical advisory, MXDR, and incident response services) for cases that need deeper partnership.

### Apply Zero Trust for AI
Microsoft references its Zero Trust principles (verify explicitly, least privilege, assume breach) and states it is extending its **Zero Trust architecture across the AI lifecycle**, and making it actionable via:

- Updated **Zero Trust for AI** reference architecture
- Workshop
- Assessment tool
- Patterns and practices articles

## RSAC event note
Microsoft invites attendees to connect at RSAC 2026 in San Francisco, including a Microsoft Pre-Day event and booth presence, featuring Agent 365, Defender, Entra, Purview, Sentinel, and Security Copilot.


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/03/20/secure-agentic-ai-end-to-end/)

