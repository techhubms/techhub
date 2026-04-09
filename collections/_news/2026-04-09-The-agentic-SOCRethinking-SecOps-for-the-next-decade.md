---
section_names:
- ai
- security
primary_section: ai
title: The agentic SOC—Rethinking SecOps for the next decade
feed_name: Microsoft Security Blog
external_url: https://www.microsoft.com/en-us/security/blog/2026/04/09/the-agentic-soc-rethinking-secops-for-the-next-decade/
author: Rob Lefferts and David Weston
date: 2026-04-09 19:00:00 +00:00
tags:
- Agentic SOC
- AI
- AI Agents
- Attack Disruption
- Autonomous Defense
- Cloud Security
- Detection Engineering
- Endpoint Detection And Response (edr)
- Endpoint Security
- Extended Detection And Response (xdr)
- Generative AI
- Governance
- Identity Security
- Incident Response
- Lateral Movement
- Malware Investigation
- Microsoft Defender XDR
- News
- Phishing Investigation
- Predictive Shielding
- SecOps
- Security
- Security Operations Center (soc)
- Security Posture
- Threat Hunting
---

Rob Lefferts and David Weston outline what an “agentic SOC” could look like over the next decade, combining autonomous, policy-bound defenses with AI agents that assemble context and orchestrate investigations so humans can focus on judgment, governance, and risk-driven security outcomes.<!--excerpt_end-->

## Overview

An “agentic SOC” is presented as a future operating model for security operations where:

- **Autonomous, deterministic defenses** stop high-confidence threats at machine speed.
- **AI agents** handle investigation, correlation, and orchestration across domains.
- **Humans** focus on judgment, governance, risk, and improving the environment over time.

The authors argue that if defense requires humans to initiate response, defenders will remain at a disadvantage (attackers only need to succeed once).

## What Microsoft means by “the agentic SOC”

The agentic SOC shifts security from reacting to incidents toward anticipating attacker movement and reshaping the environment to block attack paths.

Key elements:

- A platform that increasingly **defends itself** through built-in autonomous defense.
- **AI agents working alongside analysts** to accelerate:
  - Investigation
  - Prioritization
  - Response actions
- Analysts spend less time on “execution” and more time on:
  - Interpreting ambiguous situations
  - Assessing risk
  - Deciding systemic changes (hardening, posture improvements)

### Example flow (credential theft)

In the proposed model:

- Built-in defenses automatically:
  - Lock the affected account
  - Isolate the compromised device
- An AI agent initiates investigation by:
  - Hunting related activity across **identity, endpoint, email, and cloud** signals
  - Correlating evidence into a single view

The intended outcome is that analysts open their queue with:

- Less alert noise
- Evidence pre-assembled
- Suggested next steps

## A layered model: disruption + operational agents

The authors describe two interdependent layers.

### Layer 1: Disruption layer (platform-level autonomous defense)

Built on an underlying “threat protection” platform, this layer focuses on:

- Handling **high-confidence threats automatically**
- Using **deterministic, policy-bound controls**
- Blocking known patterns in real time

The post frames this layer as a prerequisite so agent-driven response can be:

- Safe
- Scalable
- Sustainable

Link: [Threat protection platform overview](https://www.microsoft.com/en-us/security/business/solutions/ai-powered-unified-secops-defender)

### Layer 2: Operational layer (agents augmenting SecOps workflows)

Agents take on correlation and analysis work to increase the leverage of security teams. Over time, these agents are expected to:

- Reason over evidence
- Coordinate investigations
- Orchestrate responses across domains
- Learn from outcomes
- Identify recurring attack paths and posture gaps
- Recommend environment changes to reduce repeat attacks

## What’s “real now” (claims and examples)

The post points to existing capabilities as evidence that autonomous defense can work at scale:

- Autonomous attack disruption operating “for years”
- Ransomware attacks disrupted in **an average of three minutes**
- Tens of thousands of attacks **contained every month**
- Actions executed with a claimed **99.99% confidence**

Link: [Microsoft Defender XDR automatic action](https://www.microsoft.com/en-us/security/business/siem-and-xdr/microsoft-defender-xdr)

### Predictive shielding

A newer capability highlighted is **predictive shielding**, described as anticipating likely attacker progression and proactively restricting high-risk paths/assets during an intrusion.

Link: [Predictive shielding documentation](https://learn.microsoft.com/en-us/defender-xdr/shield-predict-threats)

Case study link: [Predictive shielding stopping GPO-based ransomware](https://www.microsoft.com/en-us/security/blog/2026/03/23/case-study-predictive-shielding-defender-stopped-gpo-based-ransomware-before-started/)

### Internal and live agent testing

The authors describe scoped/controlled agent usage:

- Task agents for triage and investigations tested under expert supervision
- In live environments, agents automate:
  - **75% of phishing and malware investigations**
- Agents tested for vulnerability exposure assessment:
  - Work that took a full day of engineering effort reduced to under an hour

Link: [Explore Microsoft Defender](https://www.microsoft.com/en-us/security/business/microsoft-defender?msockid=1a673acdad396c3d27002c33ac216dd4)

## How day-to-day SOC roles may change

Agentic systems increase the need for:

- Oversight
- Tuning
- Governance

Detection/response engineering becomes more central as teams define:

- Policies
- Confidence thresholds
- Escalation paths

### Role shifts called out

- **Analysts**: from triaging alerts to supervising outcomes; validate agent investigations, focus on ambiguity, guide system learning.
- **Detection engineers**: from writing rules to defining trustworthy signals/context and confidence thresholds.
- **Threat hunters**: from manual queries to hypothesis-driven exploration with AI surfacing anomalies.
- **SOC leadership**: from managing queues to defining automation policy, governance, and aligning AI action with risk.

Image: ![A split comparison graphic labeled “Before” and “After” showing SOC roles shifting toward detection engineering, agent orchestration, and strategic advisory work.](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/Picture1.webp)

## Maturity model: the “agentic SOC journey”

The post proposes a staged path to adoption.

Image: ![A three-stage maturity model labeled SOC I (unify platform foundation), SOC II (accelerate with generative AI), and SOC III (deploy agentic automation).](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/04/Picture2.webp)

### SOC 1 — Unify your platform foundation

- Establish unified security platform enabling autonomous defense
- Deterministic protections stop high-confidence threats automatically
- Integrate signals across identity, endpoint, and cloud to avoid stitching evidence across tools

### SOC 2 — Accelerate operations with generative AI and task agents

- AI assembles context and synthesizes signals
- Repetitive high-volume tasks (triage, correlation, basic investigation) handled by the system
- Humans and AI collaborate with explicit accountability patterns

### SOC 3 — Deploy agentic automation

- Agents move from assistance to action
- Specialized agents autonomously orchestrate tasks like:
  - Containing compromised identities
  - Isolating devices
  - Remediating reported phishing
- Humans supervise and refine
- Agents help identify patterns, anticipate attack paths, and optimize defenses

## What the authors say comes next

The post sets expectations for a series focused on:

- Platform foundations needed for autonomous defense
- Governance and trust mechanisms for safe autonomy
- Adoption journey and operational discipline

Whitepaper link: [The agentic SOC: Your teammate for tomorrow, today](https://marketingassets.microsoft.com/gdc/gdc46cegG/original)

## Learn more

- [Microsoft Security website](https://www.microsoft.com/en-us/security/business)
- [Microsoft Security Blog](https://www.microsoft.com/security/blog/)
- [Microsoft Security on LinkedIn](https://www.linkedin.com/showcase/microsoft-security/)
- [@MSFTSecurity on X](https://twitter.com/@MSFTSecurity)


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/04/09/the-agentic-soc-rethinking-secops-for-the-next-decade/)

