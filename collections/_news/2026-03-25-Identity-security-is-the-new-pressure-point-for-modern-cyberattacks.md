---
section_names:
- ai
- security
date: 2026-03-25 16:00:00 +00:00
author: Rob Lefferts and Nadim Abdo
primary_section: ai
external_url: https://www.microsoft.com/en-us/security/blog/2026/03/25/identity-security-is-the-new-pressure-point-for-modern-cyberattacks/
title: Identity security is the new pressure point for modern cyberattacks
feed_name: Microsoft Security Blog
tags:
- Agentic Identities
- AI
- Automatic Attack Disruption
- Conditional Access
- Identity And Access Management (iam)
- Identity Risk Score
- Identity Security
- Lateral Movement
- Microsoft Defender
- Microsoft Defender XDR
- Microsoft Entra
- Microsoft Entra Conditional Access
- Microsoft Entra ID Protection
- News
- Non Human Identities
- Privilege Escalation
- Privileged Identity Management
- Risk Based Access
- RSAC
- Security
- Security Copilot
- Security Operations Center (soc)
- Session Revocation
- Single Sign On (sso)
- Threat Detection
- Threat Response
- Triage Agent
---

Rob Lefferts and Nadim Abdo outline why identity has become the main pressure point in modern attacks, and how Microsoft’s Entra + Defender approach ties identity infrastructure, risk-based access control, and threat protection together for faster detection and response—especially as non-human and agentic identities grow.<!--excerpt_end-->

## Overview

Identity attacks increasingly depend on **what an identity can access**, not just which user/account gets compromised. As organizations add more **human, non-human, and agentic identities**, access paths spread across apps, resources, and environments—raising operational complexity and increasing risk.

Microsoft’s position in this article: fragmented, vendor-sprawled identity and security tooling creates blind spots. A modern identity security approach should unify identity infrastructure, access control, and threat protection into an integrated, real-time system.

## Why fragmentation fails

Fragmented identity security (siloed directories, disconnected access policies, bolt-on detection) leads to:

- **Uncorrelated permissions** and policy drift as environments change
- **Lateral movement hidden in gaps** between systems
- A split between:
  - Identity teams enforcing access without full threat visibility
  - SOC teams getting floods of identity signals without enough context

The article cites research from the Secure Access report:

- **32%** of organizations say access management solutions are **duplicative**
- **40%** say they have **too many vendors**

Link: Secure Access report: http://Aka.ms/secureaccessreport

## What an end-to-end identity security solution must unify

The post describes three layers that need to operate together:

- **Identity infrastructure**
  - Identity provider, authentication services, SSO
  - User/group management
  - Trust establishment across the enterprise
- **Identity control plane**
  - Privileged identity management and real-time access decisions
  - Uses dynamic risk signals, behavior context, and policy intent
- **End-to-end identity threat protection**
  - Pre-attack: reduces posture risk (remove excessive access, close exposure gaps)
  - During attack: detects misuse, surfaces lateral movement, drives containment

## Microsoft’s reference approach (Entra + Conditional Access + Defender)

### Identity infrastructure: Microsoft Entra

Microsoft positions **Microsoft Entra** as the foundational identity layer providing:

- Resilient **SSO**
- User and group management
- Trust establishment at global scale

Link: https://www.microsoft.com/en-us/security/business/microsoft-entra

The post also highlights “identity sprawl collapse”: correlating related accounts across cloud and on-premises into a **single identity view** so teams can better understand what an identity (and linked accounts) can access.

### Real-time identity control plane: Entra Conditional Access

**Microsoft Entra Conditional Access** is described as continuously evaluating risk **during the session** (as access is used), using signals such as:

- Identity
- Device
- Network
- Threat intelligence

Link: https://www.microsoft.com/en-us/security/business/identity-access/microsoft-entra-conditional-access

### Threat protection and response

The article emphasizes **automatic attack disruption**: intervening mid-attack by actions like:

- Terminating user sessions
- Revoking access
- Applying just-in-time hardening

It frames this as “defense in motion,” intended to stop lateral movement and privilege escalation while an attack is underway.

## Security Copilot: triage agent extended to identity

The post says **Microsoft Security Copilot** has extended its **triage agent** to identity, using AI to:

- Filter noise
- Surface high-confidence alerts
- Provide explainable insights
- Reduce analyst fatigue and time to action

Link: https://techcommunity.microsoft.com/blog/microsoftthreatprotectionblog/security-copilot-in-defender-empowering-the-soc-with-assistive-and-autonomous-ai/4503047

## RSAC 2026 identity security innovations called out

At RSAC 2026, Microsoft highlights:

- **Identity security dashboard in Microsoft Defender**
  - Shows where identity risk concentrates across identity types and providers
  - Link: https://aka.ms/IDSecurity-Defender-RSA
- **Unified identity risk score**
  - Correlates signals across Microsoft Security into a single view
- **Adaptive risk remediation** with Entra ID Protection
  - Link: https://learn.microsoft.com/en-us/entra/id-protection/concept-identity-protection-user-experience
  - Entra ID Protection link: https://www.microsoft.com/en-us/security/business/identity-access/microsoft-entra-id-protection
- **Automatic attack disruption** in Defender XDR
  - Link: https://learn.microsoft.com/en-us/defender-xdr/automatic-attack-disruption
- **Security Copilot triage agent** extended to identity
  - Link: https://techcommunity.microsoft.com/blog/microsoftthreatprotectionblog/security-copilot-in-defender-empowering-the-soc-with-assistive-and-autonomous-ai/4503047
- Expanded identity fabric coverage, including **non-human identities**, and integrations with third parties like **SailPoint** and **CyberArk**
- A “coverage and maturity view” to assess posture, identify gaps, and prioritize next steps

## Related links mentioned

- Redefining identity security for the modern enterprise: https://aka.ms/IDSecurity-Defender-RSA
- What is a Security Operations Center (SOC)?: https://www.microsoft.com/en-us/security/business/security-101/what-is-a-security-operations-center-soc
- Microsoft Security business site: https://www.microsoft.com/en-us/security/business
- Microsoft Security blog: https://www.microsoft.com/security/blog/
- LinkedIn: Microsoft Security: https://www.linkedin.com/showcase/microsoft-security/
- X: @MSFTSecurity: https://twitter.com/@MSFTSecurity


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/03/25/identity-security-is-the-new-pressure-point-for-modern-cyberattacks/)

