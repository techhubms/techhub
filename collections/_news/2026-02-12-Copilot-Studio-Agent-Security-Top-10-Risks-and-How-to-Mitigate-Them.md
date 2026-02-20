---
external_url: https://www.microsoft.com/en-us/security/blog/2026/02/12/copilot-studio-agent-security-top-10-risks-detect-prevent/
title: 'Copilot Studio Agent Security: Top 10 Risks and How to Mitigate Them'
author: Microsoft Defender Security Research Team
primary_section: ai
feed_name: Microsoft Security Blog
date: 2026-02-12 20:38:49 +00:00
tags:
- Advanced Hunting
- Agent Misconfiguration
- AI
- AI Security
- Authentication
- Azure Key Vault
- Copilot Studio
- Data Exfiltration
- Governance
- Least Privilege
- Lifecycle Management
- MCP Tools
- Microsoft Defender
- News
- Power Platform
- Privileged Access
- Security
- Security Playbook
section_names:
- ai
- security
---
Microsoft Defender Security Research Team provides a detailed overview of the top 10 security risks in Copilot Studio agent deployments, offering practical detection and mitigation strategies for secure use of AI-powered business workflows.<!--excerpt_end-->

# Copilot Studio Agent Security: Top 10 Risks and How to Mitigate Them

*By Microsoft Defender Security Research Team*

Copilot Studio agents are gaining traction as flexible tools for automating business workflows using AI. However, as their capabilities grow, so do potential security exposures. This guide consolidates the ten most common security risks observed in Copilot Studio deployments and provides clear detection and mitigation approaches leveraging Microsoft Defender capabilities.

## Why Security for Copilot Studio Agents Matters

AI agents can streamline data access, automate processes, and integrate across systems—but misconfigurations can open organizations to exploitation. Minor oversights like unauthenticated access or overbroad sharing can create real attack vectors, often going unnoticed until abused. Early detection and sound governance are essential components of a robust AI security posture.

## Top 10 Security Risks and How to Detect Them

| #  | Risk                                                         | Detection/Community Query                                   |
|----|--------------------------------------------------------------|-------------------------------------------------------------|
| 1  | Agent shared organization-wide or with broad groups           | AI Agents – Organization or Multi-tenant Shared              |
| 2  | Unauthenticated agents                                       | AI Agents – No Authentication Required                       |
| 3  | HTTP Request actions with risky configs                       | AI Agents – HTTP Requests to insecure/non-standard endpoints |
| 4  | Email-based data exfiltration risk                            | AI Agents – Email to external or AI-controlled recipients    |
| 5  | Dormant/unmonitored agents and actions                        | AI Agents – Dormant/Unused/Unmodified over 30d               |
| 6  | Author (maker) authentication used                            | AI Agents – Published Agents with Author Authentication      |
| 7  | Hard-coded credentials in agents                              | AI Agents – Hard-coded Credentials in Topics/Actions         |
| 8  | Model Context Protocol (MCP) tools enabled                    | AI Agents – MCP Tool Configured                              |
| 9  | Generative orchestration without defined instructions         | AI Agents – Orchestration without Instructions               |
| 10 | Orphaned (ownerless) agents                                   | AI Agents – Orphaned Agents with Disabled Owners             |

## Example Risk: Help Desk Agent

A help desk AI agent, connected to organizational data via Dataverse and MCP tools, may lack authentication and be shared widely—quietly meeting several of the above risks. These often slip into production without alerting security teams.

![Help Desk Agent Example](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/02/image-28.webp)

## Detailed Breakdown of Risks and Mitigations

### 1. Overbroad Sharing

- Agents visible organization-wide drastically increase the attack surface.
- **Mitigation:** Restrict sharing to designated security groups; review sharing policies.

### 2. No Authentication Required

- Public or unauthenticated agents may allow unauthorized access or data leaks.
- **Mitigation:** Require authentication for all agents unless explicitly justified; audit agent authentication settings.

### 3. Risky HTTP Requests

- Direct HTTP calls may bypass governance and introduce insecure communications.
- **Mitigation:** Prefer built-in Power Platform connectors; ensure all HTTP requests use HTTPS and standard ports.

### 4. Email-Based Exfiltration

- Generative orchestration or misconfigured email actions could leak sensitive data externally.
- **Mitigation:** Restrict email actions to approved recipients; avoid dynamic destination logic.

### 5. Dormant or Orphaned Assets

- Unused, dormant, or ownerless agents can act as shadow entry points with outdated logic.
- **Mitigation:** Periodically review all agents and actions; retire or reassign ownership where necessary.

### 6. Author Authentication

- Agents running under maker credentials risk privilege escalation and bypass of least privilege.
- **Mitigation:** Replace maker authentication with user or system-based authentication; audit actions running under maker credentials.

### 7. Hard-Coded Credentials

- Storing credentials in agent logic exposes sensitive information.
- **Mitigation:** Store all secrets in Azure Key Vault, reference via environment variables.

### 8. Inactive or Unmonitored MCP Tools

- Unneeded integration tools may introduce hidden privileged logic or legacy access.
- **Mitigation:** Deactivate, audit, and document all MCP tool usage.

### 9. Orchestration Without Instructions

- Lack of clear boundaries allows generative AI to behave unpredictably.
- **Mitigation:** Always configure explicit instructions for generative orchestration.

### 10. Orphaned Agents

- Agents without active owners evade lifecycle management and review.
- **Mitigation:** Regularly audit agent ownership; reassign or decommission as needed.

## Practical Security Checklist

1. **Verify Intent and Ownership**
   - Confirm business justification for access and configuration settings.
   - Ensure every agent has an accountable owner.
   - Periodically review agent lifecycles.
   - [See: Configure data policies for agents](https://learn.microsoft.com/en-us/microsoft-copilot-studio/admin-data-loss-prevention)

2. **Reduce Exposure**
   - Limit access using role-based groups.
   - Enforce authentication by default.
   - [See: Control how agents are shared](https://learn.microsoft.com/en-us/microsoft-copilot-studio/admin-sharing-controls-limits)

3. **Enforce Least Privilege**
   - Avoid creator authentication in production settings.
   - [See: Control maker-provided credentials](https://learn.microsoft.com/en-us/microsoft-copilot-studio/configure-no-maker-authentication)

4. **Harden Orchestration and Dynamic Behaviors**
   - Define explicit instructions for all generative agents.
   - Review and restrict dynamic outbound actions like HTTP requests and emails.
   - [See: Orchestrate agent behavior with generative AI](https://learn.microsoft.com/en-us/microsoft-copilot-studio/advanced-generative-actions)

5. **Eliminate Dead Weight and Protect Secrets**
   - Remove dormant agents and unused actions.
   - Store all credentials in secure vaults.
   - Treat agents as production assets with regular reviews.

## Additional Resources

- [AI agents inventory in Microsoft Defender](https://learn.microsoft.com/en-us/defender-cloud-apps/ai-agent-inventory)
- [How to secure AI agents using Microsoft Defender](https://learn.microsoft.com/en-us/defender-xdr/ai-agent-inventory)
- [Blog: Securing AI agents](https://www.microsoft.com/en-us/security/blog/2026/01/23/runtime-risk-realtime-defense-securing-ai-agents/)

> *This research and guidance is developed by Microsoft Defender Security Research with contributions from Dor Edry and Uri Oren.*

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2026/02/12/copilot-studio-agent-security-top-10-risks-detect-prevent/)
