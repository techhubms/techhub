---
external_url: https://blogs.windows.com/windowsexperience/2026/01/22/windows-365-for-agents-the-cloud-pcs-next-chapter/
title: 'Windows 365 for Agents: Enabling Secure AI Cloud PCs'
author: stclarke
feed_name: Microsoft News
date: 2026-01-22 18:33:27 +00:00
tags:
- Agent 365
- AI Agents
- Automation
- Azure Virtual Machines
- Cloud Architecture
- Cloud PC
- Company News
- Enterprise Security
- Human in The Loop
- Identity And Access Management
- Intune
- Microsoft Copilot Studio
- Microsoft Entra ID
- Programmatic Control
- Windows 365
- AI
- Azure
- Security
- News
section_names:
- ai
- azure
- security
primary_section: ai
---
stclarke brings an in-depth look at Windows 365 for Agents, highlighting how Microsoft is evolving Cloud PC technology to securely support enterprise AI automation and agent-driven workflows.<!--excerpt_end-->

# Windows 365 for Agents: The Cloud PC’s Next Chapter

## Overview

Windows 365 for Agents represents the next evolution of Microsoft’s Cloud PC strategy. The platform now enables organizations to run autonomous AI agents alongside traditional users, delivering secure, scalable, and cost-effective virtual environments for both people and software agents.

## Key Concepts and Innovations

- **Agentic Cloud PCs:** Enterprises can now run autonomous AI agents on Windows 365 Cloud PCs, automating complex workflows and unlocking productivity gains.
- **Enterprise-Ready Architecture:** Leveraging Azure-hosted virtual machines, Microsoft Intune for unified management, and Microsoft Entra ID for robust authentication and policy enforcement.
- **Security and Compliance:** Agents operate in isolated environments with unique Entra Agent IDs, supporting cryptographic credentials and granular activity tracking.

## Architecture That Powers Windows 365 for Agents

- **Hosted on Behalf Of (HOBO) Architecture:** Windows 365 employs single-instance Azure VMs managed by Microsoft, integrated deeply with Intune and Entra ID.
- **Unified Management:** Administrators use the same Intune policies and provisioning workflows for Cloud PCs—whether used by humans or agents.
- **Reverse Connect Transport:** Ensures secure global connectivity, eliminating inbound ports and using intelligent routing (STUN/TURN) for reliability.
- **Cloud PC Pools and Elastic Scaling:** Agents share resource pools for efficient utilization; provisioning and billing are based on actual use, with ephemeral "check-in/check-out" Cloud PC assignments.

## New Capabilities for Agent Workloads

- **Programmatic Interfaces:** Developers and agent builders can automate the control and lifecycle of agent Cloud PCs via Agent 365 APIs and SDKs.
- **Computer-Using Agents (CUAs):** These agents leverage AI vision to interact with graphical UIs, adapt to UI changes, and execute tasks reliably.
- **Human-in-the-loop:** Human intervention is built in, allowing users to guide or override agent actions for sensitive or complex decisions.
- **Agent Identity:** Each agent uses a unique Entra Agent ID, ensuring clear separation of agent and user actions in audit logs.

## Security Features

- **End-to-end Security:** Enforced through Entra ID (including passwordless MFA and Conditional Access), Intune policy compliance, and network isolation.
- **Compliance and Observability:** Supports enterprise requirements for confidentiality, auditability, and regulatory compliance.
- **No-Password Architecture:** Agents use cryptographically-secured credentials, reducing risk from phishing and credential theft.

## Platform Integrations and Tooling

- **Agent 365 Developer Tools:** APIs and SDKs facilitate deep integration and customized control of agent workloads in enterprise workflows.
- **Microsoft Copilot Studio (No-Code Option):** Enables business users to build and automate agent workflows within the secure Cloud PC environment.

## Where to Learn More

- [Windows 365 for Agents unlocks secured, scalable AI automation – Windows IT Pro Blog](https://techcommunity.microsoft.com/blog/Windows-ITPro-blog/windows-365-for-agents-unlocks-secured-scalable-ai-automation/4468107)
- [Automate web and desktop apps with computer use (preview) – Microsoft Copilot Studio](https://learn.microsoft.com/en-us/microsoft-copilot-studio/computer-use)
- [Microsoft Agent 365: The control plane for AI agents | Microsoft 365 Blog](https://www.microsoft.com/en-us/microsoft-365/blog/2025/11/18/microsoft-agent-365-the-control-plane-for-ai-agents/?msockid=13c652066a10672c1d4746806b3d66ba)
- [Test agents using the Microsoft Agent 365 SDK | Microsoft Learn](https://learn.microsoft.com/en-us/microsoft-agent-365/developer/testing?tabs=python)

---

This article demonstrates the ongoing commitment to security, manageability, and innovation in Microsoft's Windows 365 ecosystem, now optimized for both human and AI agent use.

This post appeared first on "Microsoft News". [Read the entire article here](https://blogs.windows.com/windowsexperience/2026/01/22/windows-365-for-agents-the-cloud-pcs-next-chapter/)
