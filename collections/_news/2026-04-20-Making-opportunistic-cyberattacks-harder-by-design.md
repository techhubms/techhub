---
feed_name: Microsoft Security Blog
primary_section: azure
external_url: https://www.microsoft.com/en-us/security/blog/2026/04/20/making-opportunistic-cyberattacks-harder-by-design/
section_names:
- azure
- security
date: 2026-04-20 16:00:00 +00:00
tags:
- Attack Surface Reduction
- Azure
- Azure Bastion
- Copilot Studio Agents
- Dataverse
- Endpoint Reduction
- Federated Identity Credentials
- Just in Time Access
- Lateral Movement
- Least Privilege
- Managed Identities
- Microsoft Entra ID
- Microsoft Secure Future Initiative (sfi)
- Mtls
- News
- Paved Paths
- Platform Engineering
- Policy as Code
- Power Platform Managed Identity (ppmi)
- Private Endpoints
- Private Link
- RDP
- Security
- SSH
- Telemetry Standardization
- Token Based Authentication
- Workload Identity
- Zero Trust Principles
title: Making opportunistic cyberattacks harder by design
author: Ilya Grebnov
---

Ilya Grebnov (Deputy CISO for Dynamics 365 and Power Platform) explains how Microsoft reduces opportunistic attacks by eliminating credentials with Microsoft Entra ID–issued managed identities, shrinking public endpoints with Azure networking patterns, and using platform engineering “paved paths” to enforce secure defaults at scale.<!--excerpt_end-->

# Making opportunistic cyberattacks harder by design

*By Ilya Grebnov (Microsoft Deputy CISO for Dynamics 365 and Power Platform). Part of the Microsoft Deputy CISO series on practical security recommendations.*

This article focuses on **opportunistic attacks**—cases where attackers gain a foothold in adjacent systems and attempt to **move laterally** into higher-value environments. The core message: make opportunistic intrusion paths less viable by removing reusable credentials, reducing inbound exposure, and standardizing secure defaults through platform engineering.

## Credential elimination and the benefits of managed identities

A recurring root cause in intrusions is **stolen or leaked credentials**. The approach described here is to **remove credentials from workloads wherever possible**:

- If a workload can authenticate **without a secret**, it should.
- Redesign standards and retire legacy patterns that depend on:
  - passwords
  - client secrets
  - API keys

### How Microsoft does credential-free authentication on Azure

The primary mechanisms called out:

- **Managed identities** (workload identities issued by **Microsoft Entra ID**)
- **Federated identity patterns** that mint tokens **just-in-time**, with **just-enough-access** scoped to the required resource

Key benefit: there’s **nothing to store or rotate**, and less risk of credentials being:

- phished
- guessed
- reused
- accidentally committed to a repository
- left unexpired

### Extending credential-free patterns to Power Platform

Two examples mentioned for broader ecosystem usage:

- **Power Platform Managed Identity (PPMI)**
  - Provides Power Platform components (for example, **Dataverse plugins** and **Power Automate**) with a *tenant-owned identity*.
  - Authenticates to Azure resources using **federated credentials** rather than embedded passwords/secrets.
  - Practical effects called out:
    - fewer outages from expired secrets
    - less need for makers to create app registrations they may not have permissions for

- **Microsoft Entra Agent ID**
  - Treats AI agents (such as those created in **Copilot Studio**) as **first-class identities**.
  - Enables inventory, governance, and binding agents to a **human sponsor** for accountability.

## Endpoint elimination to reduce the attack surface

Credential elimination is paired with **endpoint elimination**: reduce or remove **public inbound-reachable endpoints**.

Patterns described (in an Azure context):

- Use **Private Link / private endpoints** to keep services off the public internet.
- Disable inbound administrative ports (**RDP/SSH**) and prefer brokered access such as:
  - just-in-time access
  - bastion
  - serial console
- Rely on **service-to-service OAuth** rather than IP-based allowlists.
- Enforce **least privilege** at the token level to reduce blast radius.

The combined effect: fewer public surfaces to probe, fewer reusable secrets to steal, and reduced lateral movement because identities are distinct and auditable.

## Platform engineering for security

The article argues opportunistic attackers exploit **inconsistency**—one-off exceptions and “snowflake” architectures with unique configuration and failure modes.

Platform engineering is positioned as a way to convert “do the right thing” into **enforced defaults**:

- Standardize compute and communications.
- Disallow brittle or deprecated patterns.
- Apply the same controls everywhere to reduce misconfiguration opportunities.

### When platform engineering becomes worth it

A heuristic given: platform engineering becomes most beneficial around **~500 engineers**.

- Too early: can dampen experimentation.
- Too late: fragmentation makes cleanup and migration much harder.

### What to line up before enforcing paved paths

- **Paved paths** teams actually want to use (secure-by-default runtimes, libraries, pipelines).
- **Policy-as-code** that blocks deprecated patterns and enforces identity-based auth + networking.
- **Executive sponsorship** to limit exceptions and keep platform friction low.

### Balancing product velocity vs risk reduction

The piece frames a common tension:

- Product/feature teams optimize for shipping and integration speed.
- Platform/security teams optimize for minimizing risk and enforcing scalable patterns.

The goal is a deliberate balance: enough flexibility for innovation, within guardrails that prevent fragmentation and reduce attack surface.

### Example: “core services” standardization

Microsoft’s internal example describes:

- Standardizing compute through **core services** used for execution and communications.
- Centralizing defenses so a change can land once and be inherited broadly.

Specific capabilities mentioned as part of standardization:

- a common communication library with:
  - uniform authentication
  - mTLS
  - retries
  - telemetry
  - policy hooks
- centralized resource management and telemetry

Impact described: one platform change can protect **450+ services**, and centralized evidence helps with approvals and compliance demonstrations.

## Resilience, consistency, and fewer weak links

Credential elimination and platform engineering are presented as **long-term** coordination efforts rather than quick wins, with payoff in:

- resilience
- consistency
- smaller attack surface

Forward-looking identity direction mentioned:

- Move from customer-usable PPMI toward **platform provisioned identities** that are:
  - automatically created per service
  - partitioned at the cell level
  - scoped to minimum privileges

## References

- Original post: Making opportunistic cyberattacks harder by design: https://www.microsoft.com/en-us/security/blog/2026/04/20/making-opportunistic-cyberattacks-harder-by-design/
- Secure Future Initiative (SFI): https://www.microsoft.com/en-us/trust-center/security/secure-future-initiative
- Microsoft Entra ID: https://www.microsoft.com/en-us/security/business/identity-access/microsoft-entra-id
- Microsoft Deputy CISO series (OCISO): https://www.microsoft.com/en-us/security/blog/topic/office-of-the-ciso/

![Man with smile on face working with laptop](https://www.microsoft.com/en-us/security/blog/wp-content/uploads/2025/10/Man-with-smile-on-face-working-with-laptop-2.webp)


[Read the entire article](https://www.microsoft.com/en-us/security/blog/2026/04/20/making-opportunistic-cyberattacks-harder-by-design/)

