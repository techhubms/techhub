---
external_url: https://github.blog/security/supply-chain-security/strengthening-supply-chain-security-preparing-for-the-next-malware-campaign/
title: Strengthening Supply Chain Security for Developers and Maintainers
author: Madison Oliver
feed_name: The GitHub Blog
date: 2025-12-23 23:52:08 +00:00
tags:
- Artifact Validation
- Branch Protection
- CI/CD
- Code Scanning
- Credentials
- Dependency Review
- GitHub
- GitHub Security Lab
- Malware
- Microsoft
- Multi Factor Authentication
- npm
- OIDC
- Supply Chain
- Supply Chain Security
- Token Management
- Trusted Publishing
section_names:
- devops
- security
primary_section: devops
---
Madison Oliver shares practical security strategies for developers and maintainers to defend against supply chain attacks, with a focus on securing GitHub and npm workflows.<!--excerpt_end-->

# Strengthening Supply Chain Security for Developers and Maintainers

## Overview

Organized supply chain threats continue to evolve, targeting open source ecosystems through compromised credentials, malicious package scripts, and sophisticated publication pipeline abuse. Recent multi-stage campaigns like Shai-Hulud highlight how adversaries rapidly adapt and iterate to bypass community defenses.

## Shai-Hulud Campaign Analysis

- **Credential-adjacent compromise:** Attackers exploit stolen credentials or OAuth tokens to pivot and harvest a wide range of secrets, spreading infections across packages and organizations.
- **Install-time execution with obfuscation:** Malicious scripts are injected into packages, activating conditionally and exfiltrating sensitive environment data such as tokens and credentials.
- **Targeted namespaces:** Popular packages are targeted, with attackers publishing malicious versions disguised as legitimate updates, blending into regular maintainer activity.
- **Rapid engineering:** Waves of attacks iterate quickly, bypassing prior mitigations and demonstrating a well-organized, durable access strategy.
- **Publication pipeline blind spots:** Build-time and source/published artifact discrepancies enable attackers to inject behavior unnoticed, underscoring the need for artifact validation and staged approvals.

## Upcoming npm Security Enhancements

- **Bulk OIDC onboarding:** Enabling organizations to migrate large numbers of packages to trusted, secure publishing workflows.
- **Expanded OIDC provider support:** Supporting more CI solutions beyond GitHub Actions and GitLab.
- **Staged publishing:** Introducing review periods with MFA-verified approval before packages go live, empowering teams to catch and remediate issues pre-release.

## Key Advice for GitHub and npm Ecosystem

### For Everyone

- Enable phishing-resistant MFA on all accounts, especially package managers like npm, PyPI, RubyGems, NuGet, as well as code repositories and email/social media.
- Set expiration dates on access tokens and enforce maximum lifetime policies; regularly rotate credentials.
- Audit and revoke access for unused GitHub/OAuth applications.
- Use sandboxed environments (e.g., GitHub Codespaces, VMs, containers) for development to limit malware impact.

### For Maintainers

- Enable branch protection to prevent direct malicious pushes on main branches.
- Transition to trusted publishing models and avoid dependency on static tokens.
- Pin CI dependencies, enable code scanning for repositories, and promptly resolve alerts.
- Monitor artifacts: Validate published bundles against source using SRI or artifact attestations.

## Responding to an Incident

If you suspect compromise, contact GitHub Support for immediate assistance and refer to Microsoft’s attacker detection and defense guidance.

## References

- [Report a malicious npm package](https://docs.npmjs.com/reporting-malware-in-an-npm-package)
- [GitHub Supply Chain Security](https://github.blog/security/supply-chain-security/our-plan-for-a-more-secure-npm-supply-chain/)
- [Microsoft Guidance Against Shai-Hulud](https://www.microsoft.com/en-us/security/blog/2025/12/09/shai-hulud-2-0-guidance-for-detecting-investigating-and-defending-against-the-supply-chain-attack/)
- [Quickstart for Securing GitHub Repos](https://docs.github.com/en/code-security/getting-started/quickstart-for-securing-your-repository)

## Further Reading

See reference links for in-depth analysis and best practices.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/security/supply-chain-security/strengthening-supply-chain-security-preparing-for-the-next-malware-campaign/)
