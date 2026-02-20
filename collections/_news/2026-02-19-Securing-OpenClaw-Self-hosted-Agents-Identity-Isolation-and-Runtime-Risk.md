---
external_url: https://www.microsoft.com/en-us/security/blog/2026/02/19/running-openclaw-safely-identity-isolation-runtime-risk/
title: 'Securing OpenClaw Self-hosted Agents: Identity, Isolation, and Runtime Risk'
author: Microsoft Defender Security Research Team
primary_section: security
feed_name: Microsoft Security Blog
date: 2026-02-19 16:27:00 +00:00
tags:
- Advanced Hunting
- Agent Runtime
- ClawHub
- Credential Management
- Defender For Cloud Apps
- Enterprise Security
- Malicious Skills
- Microsoft Defender For Endpoint
- Microsoft Defender XDR
- Microsoft Entra ID
- Microsoft Purview
- Microsoft Sentinel
- Moltbook
- News
- OAuth Security
- OpenClaw
- Prompt Injection
- Runtime Isolation
- Security
- Self Hosted Agent
- Supply Chain Security
section_names:
- security
---
The Microsoft Defender Security Research Team examines the unique security risks of self-hosted agents like OpenClaw, detailing how identity, isolation, and runtime controls are critical for safe deployment.<!--excerpt_end-->

# Securing OpenClaw Self-hosted Agents: Identity, Isolation, and Runtime Risk

**Author**: Microsoft Defender Security Research Team

Self-hosted agents such as OpenClaw are being rapidly adopted in enterprise environments, yet present significant security challenges due to their ability to execute code and handle untrusted input with persistent credentials. This creates a dual supply chain risk from both skill/code ingestion and instruction manipulation.

## Key Risks in Agent Runtimes

- **Exposed Credentials/Data**: Agents can inadvertently expose sensitive tokens and data.
- **Persistent State Manipulation**: Attackers can modify agent memory or state, resulting in long-term compromise.
- **Host Compromise**: Retrieval and execution of malicious code may compromise host environments.

## Core Concepts: Runtime vs Platform

- **OpenClaw (runtime)**: Operates with the permissions of its host machine; installing skills is tantamount to installing privileged code.
- **Moltbook (platform)**: Identity layer/API platform channeling agent instructions, which can become a vector for indirect prompt injection at scale.

## Attack Scenarios

- **Indirect Prompt Injection**: Malicious instructions included in external content can steer agent execution or tamper with memory.
- **Skill Malware**: Third-party skills or extensions may contain malicious payloads, with public registries like ClawHub being a potential source.

### Five-step Compromise Chain

1. **Distribution**: Malicious skill published to ClawHub.
2. **Installation**: Skill is added to an agent, possibly without strong oversight.
3. **State Access**: Attacker accesses agent tokens, config, or instruction channels.
4. **Privilege Reuse**: Uses valid credentials for unauthorized actions.
5. **Persistence**: Maintains control via configuration or scheduled activity.

## Minimum Safe Operating Posture for OpenClaw

1. **Isolation**: Deploy agents only in dedicated VMs or physical systems, never on workstations with sensitive data.
2. **Dedicated Credentials**: Use non-privileged, purpose-specific identities and datasets.
3. **Continuous Monitoring**: Track agent state and instructions for manipulation.
4. **Regular Rebuilds**: Snapshot agent state frequently; be prepared to rebuild/reroll at the sign of compromise.
5. **Containment and Recovery**: Prioritize containment and fast recovery over relying solely on prevention.

## Implementing Controls with Microsoft Security Tools

- **Microsoft Entra ID**: Enforce least privilege and controlled consent for OAuth scopes.
- **Microsoft Defender for Endpoint**: Onboard agent hosts and enforce stricter policies.
- **Defender for Cloud Apps (App Governance)**: Inventory OAuth apps, monitor privilege drift.
- **Microsoft Purview**: Use sensitivity labeling and DLP to track agent data movement.
- **Microsoft Sentinel & Defender XDR**: Enable hunting, logging, and playbooks for rapid detection and response.

### Examples of Hunting Queries

- **Discover agent runtimes:** Surface where OpenClaw/moltbot/clawdbot are installed.
- **Cloud workload monitoring:** Identify containerized agent processes.
- **Skill install review:** Detect rare or newly installed skills.
- **Extension/file monitoring:** Audit changes in developer endpoints for signs of compromise.
- **OAuth high-privilege monitoring:** Surface new high-privilege apps requiring review.
- **Network and shell activity:** Detect unexpected outbound and process creation events.

## Summary Guidance

Organizations are advised to treat self-hosted agents as high-risk, focusing on isolation, dedicated non-sensitive resources, restriction of install sources, and aggressive logging/monitoring. Assume compromise is possible: monitor, contain, rebuild rapidly, and use provided hunting queries to detect anomalies. For most organizations, not deploying such agents is the safest path, but if evaluation is required, rigorous controls as described above are mandatory.

## References & Further Reading

- [Microsoft Defender XDR Advanced Hunting overview](https://learn.microsoft.com/en-us/defender-xdr/advanced-hunting-overview)
- [Securing Copilot Studio agents with Microsoft Defender](https://learn.microsoft.com/en-us/defender-cloud-apps/ai-agent-protection)
- [How Microsoft discovers and mitigates evolving attacks against AI guardrails](https://www.microsoft.com/en-us/security/blog/2024/04/11/how-microsoft-discovers-and-mitigates-evolving-attacks-against-ai-guardrails/)
- [Protect agents in real-time during runtime – Microsoft Defender for Cloud Apps](https://learn.microsoft.com/en-us/defender-cloud-apps/real-time-agent-protection-during-runtime)
- [Microsoft Purview Endpoint DLP](https://learn.microsoft.com/en-us/purview/endpoint-dlp-learn-about)
- [Entra admin consent workflow](https://learn.microsoft.com/en-us/entra/identity/enterprise-apps/admin-consent-workflow-overview)

> *Contributions from Idan Hen and the Microsoft Defender Security Research Team.*

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2026/02/19/running-openclaw-safely-identity-isolation-runtime-risk/)
