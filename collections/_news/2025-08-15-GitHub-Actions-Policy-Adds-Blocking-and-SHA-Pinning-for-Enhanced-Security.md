---
external_url: https://github.blog/changelog/2025-08-15-github-actions-policy-now-supports-blocking-and-sha-pinning-actions
title: GitHub Actions Policy Adds Blocking and SHA Pinning for Enhanced Security
author: Allison
feed_name: The GitHub Blog
date: 2025-08-15 15:05:08 +00:00
tags:
- Action Blocking
- Artifact Integrity
- CI/CD
- Dependabot
- DevOps Security
- GitHub Actions
- GitHub Well Architected Framework
- Immutable Releases
- Policy Enforcement
- Release Management
- Repository Administration
- SHA Pinning
- Software Supply Chain
- Version Control
- Workflow Governance
section_names:
- devops
- security
primary_section: devops
---
Allison outlines recent improvements to GitHub Actions policies, focusing on blocking vulnerable actions and enforcing SHA pinning to enhance repository and workflow security.<!--excerpt_end-->

# GitHub Actions Policy Adds Blocking and SHA Pinning for Enhanced Security

*Author: Allison*

## Overview

GitHub Actions now offers administrators and developers improved control over workflows through new policy features: the ability to **explicitly block actions or versions**, and to **enforce dependency pinning via commit SHA**. These tools address potential vulnerabilities and supply chain risks inherent in using third-party and community-contributed actions.

## Key Features

### 1. Explicit Blocking of Actions or Versions

- **New Policy Support**: Administrators can now block specific actions or particular versions by prefixing them with `!` in the allowed actions list.
- **Blocklist Precedence**: Blocklist rules take precedence, overriding other policy configurations.
- **Use Case**: Protect repositories from actions discovered to be malicious, even if previously allowed by other policies.

### 2. Mandatory SHA Pinning

- **What is SHA Pinning?**: Workflow dependencies can be restricted to exact commit SHAs, preventing the automatic use of new or potentially malicious updates to tags or branches.
- **Policy Enforcement**: Administrators have the option to require SHA pinning, visible as a new checkbox in the GitHub Actions settings (unless all actions are disabled at the repo/org/enterprise level).
- **Compliance**: Any workflow not following SHA pinning will fail to run if this enforcement is enabled.

## Documentation and Best Practices

- Updated GitHub documentation covers [allowed actions policies](https://docs.github.com/repositories/managing-your-repositorys-settings-and-features/enabling-features-for-your-repository/managing-github-actions-settings-for-a-repository#managing-github-actions-permissions-for-your-repository) and [SHA pinning enforcement](https://docs.github.com/actions/reference/security/secure-use#using-third-party-actions).
- The [GitHub Well-Architected Framework](https://wellarchitected.github.com/library/application-security/recommendations/actions-security) is updated to recommend blocking and SHA pinning for robust supply chain security.

## Roadmap and Ecosystem Hardening

- New **Immutable Releases** are being introduced. Once a release is marked immutable, it and its associated assets/tags cannot be altered or deleted.
- Release attestation supports artifact integrity and integrates with protection rulesets via both UI and API.
- Benefits to the GitHub Actions ecosystem include:
  - Reliable pinning of dependencies to immutable references
  - Trusted verifiable workflow assets
  - Greater pipeline stability and reproducibility
- Dependabot continues to play a role in identifying and upgrading actions with known vulnerabilities.

## Actionable Takeaways

- Review and update your organization/repository allowed actions and reusable workflows policies.
- Enable and enforce SHA pinning for third-party actions to reduce the risk of supply chain compromise.
- Reference updated documentation as policies evolve and leverage immutable releases for stronger artifact security.

## References

- [GitHub Actions Security Best Practices](https://docs.github.com/actions/reference/security/secure-use#using-third-party-actions)
- [GitHub Actions Policy Documentation](https://docs.github.com/repositories/managing-your-repositorys-settings-and-features/enabling-features-for-your-repository/managing-github-actions-settings-for-a-repository#managing-github-actions-permissions-for-your-repository)
- [GitHub Well-Architected Framework – Application Security](https://wellarchitected.github.com/library/application-security/recommendations/actions-security)

---

By integrating these new blocking and SHA pinning features, organizations can respond faster to supply chain threats and enforce consistent, secure software delivery with GitHub Actions.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-15-github-actions-policy-now-supports-blocking-and-sha-pinning-actions)
