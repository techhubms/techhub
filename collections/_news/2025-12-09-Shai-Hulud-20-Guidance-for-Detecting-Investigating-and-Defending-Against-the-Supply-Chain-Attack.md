---
layout: post
title: 'Shai-Hulud 2.0: Guidance for Detecting, Investigating, and Defending Against the Supply Chain Attack'
author: Microsoft Defender Security Research Team
canonical_url: https://www.microsoft.com/en-us/security/blog/2025/12/09/shai-hulud-2-0-guidance-for-detecting-investigating-and-defending-against-the-supply-chain-attack/
viewing_mode: external
feed_name: Microsoft Security Blog
feed_url: https://www.microsoft.com/en-us/security/blog/feed/
date: 2025-12-09 21:41:32 +00:00
permalink: /azure/news/Shai-Hulud-20-Guidance-for-Detecting-Investigating-and-Defending-Against-the-Supply-Chain-Attack
tags:
- Attack Path Analysis
- Azure Key Vault
- Bun Runtime
- CI/CD Pipelines
- Cloud Security Explorer
- Credential Theft
- Defender For Cloud
- Defender XDR
- GitHub Actions
- Incident Response
- Key Vault
- Microsoft Defender
- Microsoft Sentinel
- Node.js
- npm Package Security
- Security Copilot
- Shai Hulud 2.0
- Supply Chain Attack
- Threat Intelligence
- TruffleHog
section_names:
- azure
- devops
- security
---
Microsoft Defender Security Research Team presents an in-depth analysis of the Shai-Hulud 2.0 attack, offering actionable detection, investigation, and defense guidance for developers and security professionals in cloud-native environments.<!--excerpt_end-->

# Shai-Hulud 2.0: Guidance for Detecting, Investigating, and Defending Against the Supply Chain Attack

## Overview

The Shai-Hulud 2.0 supply chain attack stands out as a major compromise in the cloud-native ecosystem, targeting developer environments, CI/CD pipelines, and cloud-connected workloads to steal credentials and configuration secrets. Attackers injected malicious code into hundreds of npm packages, with automation enabling rapid spread and an expanded target set.

### Key Attack Mechanisms

- Malicious code added in the `preinstall` phase of infected npm packages, allowing execution before security checks.
- Compromised maintainer accounts in popular projects (Zapier, PostHog, Postman).
- Stolen credentials exfiltrated to attacker-controlled public repositories.
- Automation facilitates faster propagation and broader impact.
- Use of the Bun runtime for malicious script execution.
- Impersonation in commit authors (e.g., “Linus Torvalds”) demonstrates the need for verified commit signatures.

#### Attack Chain Example

- Malicious script `setup_bun.js` checks for Bun runtime; installs it if missing.
- Bun executes `bun_environment.js`, which sets up a GitHub Actions runner agent (“SHA1Hulud”).
- Additional tooling (TruffleHog, Runner.Listener) for credential collection and exfiltration.

## Defense and Mitigation Recommendations

Microsoft Defender provides layered protection for these scenarios:

- **Posture management:** Scan workloads for compromised packages.
- **Credential management:** Rapidly rotate/revoke exposed credentials, audit Key Vault activity, and isolate affected CI/CD agents.
- **Access controls:** Remove unnecessary roles/permissions, particularly for Key Vault access in pipelines.
- **Alerts:** Dedicated detections for this campaign in Defender for Containers and Defender for Endpoint.

### Key Security Practices

- Use [npm trusted publishing](https://docs.npmjs.com/trusted-publishers) and enforce [2FA on npm packages](https://docs.npmjs.com/requiring-2fa-for-package-publishing-and-settings-modification).
- Prefer [WebAuthn](https://docs.npmjs.com/configuring-two-factor-authentication) to TOTP for two-factor authentication.
- Enable [cloud-delivered protection](https://learn.microsoft.com/defender-endpoint/configure-block-at-first-sight-microsoft-defender-antivirus) and sample submission.
- Apply [attack surface reduction rules](https://learn.microsoft.com/defender-endpoint/attack-surface-reduction).
- Connect your DevOps environments (Azure DevOps, GitHub, GitLab) to Defender for Cloud for repository mapping and vulnerability exposure tracking.

### Incident Detection and Response Integration

- Microsoft Defender XDR coordinates detection and response across endpoints, identities, email, and apps.
- Use [Security Copilot](https://learn.microsoft.com/defender-xdr/security-copilot-in-microsoft-365-defender) and prebuilt promptbooks for investigation and automation.
- Integrate threat analytics via Defender and Sentinel portals for continuous monitoring.

## Hunting Queries & Technical Guidance

Example KQL queries for threat hunting include:

- Finding malicious JS execution: ```DeviceProcessEvents | where FileName has "node" and ProcessCommandLine has_any ("setup_bun.js", "bun_environment.js") ```
- Detecting suspicious process launches: See article for advanced KQL patterns.
- Mapping attack paths: Queries leveraging the Exposure Graph to track credential theft and lateral movement to Key Vaults.
- Monitoring container images: Use Cloud Security Explorer templates to surface vulnerable containers.

## References and Further Reading

- [Microsoft Defender for Containers](https://learn.microsoft.com/azure/defender-for-cloud/defender-for-containers-azure-overview)
- [Microsoft Threat Intelligence Blog](https://aka.ms/threatintelblog)
- [Attack Surface Reduction rules](https://learn.microsoft.com/defender-endpoint/attack-surface-reduction)
- [Connecting Azure DevOps to Defender for Cloud](https://learn.microsoft.com/azure/defender-for-cloud/quickstart-onboard-devops)
- [GitHub Actions security best practices](https://github.blog/security/supply-chain-security/our-plan-for-a-more-secure-npm-supply-chain/)
- [Aikido analysis of Shai-Hulud](https://www.aikido.dev/blog/shai-hulud-strikes-again-hitting-zapier-ensdomains)

## Key Points for Security Teams

- Traditional network controls are insufficient for supply chain attacks embedded in package workflows.
- Defender’s code-to-runtime coverage and telemetry correlation are essential for rapid containment.
- Robust credential management, regular monitoring with hunting queries, and integration with cloud security tooling provide meaningful defense.
- Stay up-to-date with the latest threat intelligence via official Microsoft resources and maintain an active posture to respond to evolving attacks.

---

**Author:** Microsoft Defender Security Research Team

This post appeared first on "Microsoft Security Blog". [Read the entire article here](https://www.microsoft.com/en-us/security/blog/2025/12/09/shai-hulud-2-0-guidance-for-detecting-investigating-and-defending-against-the-supply-chain-attack/)
