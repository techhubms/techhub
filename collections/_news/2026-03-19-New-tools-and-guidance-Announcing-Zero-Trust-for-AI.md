---
primary_section: ai
section_names:
- ai
- security
title: 'New tools and guidance: Announcing Zero Trust for AI'
date: 2026-03-19 19:00:00 +00:00
external_url: https://www.microsoft.com/en-us/security/blog/2026/03/19/new-tools-and-guidance-announcing-zero-trust-for-ai/
feed_name: Microsoft Security Blog
author: Mike Adams
tags:
- Agentic AI
- AI
- AI Agents
- AI Observability
- AI Security
- Assume Breach
- CIS
- CISA
- Data Classification
- Data Governance
- Data Labeling
- Data Loss Prevention
- Data Poisoning
- Least Privilege
- Microsoft Zero Trust Workshop
- Network Security
- News
- NIST
- Prompt Injection
- Reference Architecture
- RSAC
- Secure Future Initiative
- Security
- SFI
- Threat Modeling
- Verify Explicitly
- Zero Trust Assessment
- Zero Trust For AI
- ZT4AI
---

Mike Adams announces Microsoft’s “Zero Trust for AI (ZT4AI)” guidance, including a new AI pillar in the Zero Trust Workshop, expanded Zero Trust Assessment pillars (Data and Network), and a Zero Trust reference architecture aimed at securing AI systems and agentic workloads end-to-end.<!--excerpt_end-->

# New tools and guidance: Announcing Zero Trust for AI

Microsoft is publishing an updated set of tools and guidance for applying Zero Trust principles across the AI lifecycle—what it calls **Zero Trust for AI (ZT4AI)**—covering **data ingestion**, **model training**, **deployment**, and **agent behavior**.

## What Microsoft is releasing

The announcement includes:

- A **new AI pillar** in the **Zero Trust Workshop**: <https://microsoft.github.io/zerotrustassessment/guide>
- **Updated Data and Networking pillars** in the **Zero Trust Assessment** tool: <https://aka.ms/zerotrust/assessment>
- A **new Zero Trust reference architecture** for AI
- **Patterns and practices** intended to help teams secure AI systems “at scale”

## Why Zero Trust principles must extend to AI

The post argues that AI systems create new and shifting trust boundaries:

- Between **users and agents**
- Between **models and data**
- Between **humans and automated decision-making**

It highlights a specific risk with autonomous/semi-autonomous agents: **overprivileged, manipulated, or misaligned agents** that can act like “double agents.”

Related video:

- AI with Zero Trust Security: <https://www.youtube.com/watch?v=OnlN-2Q5QsE>

### Applying core Zero Trust principles to AI

Microsoft frames ZT4AI as applying the same foundational Zero Trust principles, but systematically for AI environments:

- **Verify explicitly**: continuously evaluate identity and behavior of **AI agents**, **workloads**, and **users**
- **Apply least privilege**: restrict access to **models**, **prompts**, **plugins**, and **data sources**
- **Assume breach**: design for resilience to **prompt injection**, **data poisoning**, and **lateral movement**

## A structured path: strategy → assessment → implementation

The post positions the updated workshop and assessment tools as a way to help teams move from “knowing what to do” to “doing it,” with more prescriptive guidance and automation.

## Zero Trust Workshop—now with an AI pillar

The **Zero Trust Workshop** has been updated with a dedicated **AI pillar** (building on a prior workshop expansion announcement):

- Workshop guide: <https://microsoft.github.io/zerotrustassessment/guide>
- Prior announcement: <https://www.microsoft.com/en-us/security/blog/2025/07/09/microsoft-expands-zero-trust-workshop-to-cover-network-secops-and-more/>

Microsoft states the workshop now covers:

- **700 security controls**
- Across **116 logical groups**
- And **33 functional swim lanes**

It’s described as scenario-based and prescriptive, intended to help organizations:

- Align security, IT, and business stakeholders on shared outcomes
- Apply Zero Trust principles across pillars (including AI)
- Work through real-world AI scenarios and their risks
- Identify cross-product integrations to reduce silos

Image:

- ![Image of the Zero Trust Workshop dashboard showing a “first, then, next” approach to a Zero Trust journey](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/ZT-Workshop-scaled.webp)

The AI pillar focuses on evaluating how an organization:

- Secures **AI access** and **agent identities**
- Protects sensitive data **used by** and **generated through** AI
- Monitors AI usage and behavior across the enterprise
- Governs AI responsibly (risk and compliance alignment)

## Zero Trust Assessment—expanded to Data and Networking

Microsoft describes why it expanded assessment coverage:

- AI agents can expose sensitive data, act on malicious prompts, or leak information in hard-to-detect ways
- Data controls such as **classification**, **labeling**, **governance**, and **loss prevention** are presented as essential
- Network-layer defenses should inspect agent behavior, block prompt injection attempts, and prevent unauthorized exposure

The **Zero Trust Assessment** is positioned as automation to reduce manual evaluation overhead and error:

- Tool: <https://aka.ms/zerotrust/assessment>
- Informed by the **Secure Future Initiative (SFI)**: <https://www.microsoft.com/en-us/trust-center/security/secure-future-initiative>

New pillars added:

- **Data**
- **Network**

Alongside existing pillars:

- **Identity**
- **Devices**

Images:

- ![Image of the Zero Trust Assessment dashboard showing results in a customer environment](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/Picture4.webp)

Assessment tests are said to be derived from:

- Industry standards and sources: **NIST**, **CISA**, **CIS**
- Microsoft SFI learnings
- Customer insights from security implementations

Planned work:

- A **Zero Trust Assessment for AI** pillar is “in development” and targeted for **summer 2026**.

## Zero Trust for AI reference architecture

Microsoft introduces a new reference architecture for AI that extends its existing Zero Trust reference architecture. It emphasizes:

- Policy-driven access controls
- Continuous verification
- Monitoring
- Governance
- Defense-in-depth for agentic workloads

Image:

- ![Image of the Zero Trust Framework showing the new AI capability being announced](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2026/03/Picture5.webp)

## Practical patterns and practices for AI security

Microsoft points to a set of “patterns and practices” meant to be reusable approaches for AI security challenges:

- Patterns and practices overview: <https://www.microsoft.com/en-us/trust-center/security/secure-future-initiative/patterns-and-practices>

Highlighted examples include:

- Threat modeling for AI: <https://aka.ms/SFI_ThreatModeling>
- AI observability: <https://aka.ms/SFI_AIObservability>

## RSAC 2026 sessions

The post lists sessions at RSAC 2026:

- RSAC event page: <https://microsoftsecurityevents.eventbuilder.com/Employees?ref=Employees>

Sessions:

- Wednesday, March 25, 2026 (11:00 AM PT-11:20 AM PT): **Zero Trust for AI: Securing the Expanding Attack Surface**
- Wednesday, March 25, 2026 (12:00 PM PT-1:00 PM PT): **Building Trust for a Secure Future: From Zero Trust to AI Confidence**
- Thursday, March 26, 2026 (11:00 AM PT-12:00 PM PT): **Zero Trust, SFI, and ZT4AI: Practical, actionable guidance for CISOs**

## Get started links

- Microsoft’s Zero Trust approach: <https://www.microsoft.com/en-us/security/business/zero-trust>
- Zero Trust Workshop: <https://aka.ms/ztworkshop>
- Zero Trust Assessment tool: <https://aka.ms/zerotrust/assessment>
- Microsoft Security Community: <https://techcommunity.microsoft.com/category/microsoft-security>
- Microsoft Security blog: <https://www.microsoft.com/security/blog/>
- Microsoft Security site: <https://www.microsoft.com/en-us/security/business>
- LinkedIn: <https://www.linkedin.com/showcase/microsoft-security/>
- X profile: <https://twitter.com/@MSFTSecurity>


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/03/19/new-tools-and-guidance-announcing-zero-trust-for-ai/)

