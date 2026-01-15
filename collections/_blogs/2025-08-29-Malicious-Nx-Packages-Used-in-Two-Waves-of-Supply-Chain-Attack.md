---
layout: post
title: Malicious Nx Packages Used in Two Waves of Supply Chain Attack
author: Jeff Burt
canonical_url: https://devops.com/malicious-nx-packages-used-in-two-waves-of-supply-chain-attack/?utm_source=rss&utm_medium=rss&utm_campaign=malicious-nx-packages-used-in-two-waves-of-supply-chain-attack
viewing_mode: external
feed_name: DevOps Blog
feed_url: https://devops.com/feed/
date: 2025-08-29 17:15:10 +00:00
permalink: /ai/blogs/Malicious-Nx-Packages-Used-in-Two-Waves-of-Supply-Chain-Attack
tags:
- AI
- AI CLI Tools
- Automation
- Blogs
- Credential Theft
- DevOps
- DevOps And Open Technologies
- DevOps Security
- GitGuardian
- GitHub
- GitHub Workflow
- Malware
- npm Registry
- Nx
- Open Source Security
- Publish.yml
- Repository Permissions
- Security
- Social Facebook
- Social LinkedIn
- Social X
- StepSecurity
- Supply Chain Attack
- Supply Chain Attacks
- Token Leakage
- Trusted Publisher
- Wiz
section_names:
- ai
- devops
- security
---
Jeff Burt reports on a major supply chain attack against the Nx build system, highlighting credential theft, GitHub workflow abuse, and innovative use of AI CLI tools in a rapidly evolving DevOps security landscape.<!--excerpt_end-->

# Malicious Nx Packages Used in Two Waves of Supply Chain Attack

**Author: Jeff Burt**

## Overview

The Nx build system, an open source platform widely used to manage large codebases, was recently targeted in a highly sophisticated supply chain attack dubbed "s1ngularity." The attack leveraged stolen npm tokens and abused GitHub workflows, ultimately leaking thousands of developer secrets and introducing new challenges for DevOps and security teams.

## Timeline and Attack Mechanics

- **Initial Compromise:** Attackers stole an Nx npm token, enabling them to publish malicious versions of the Nx package and supporting plugins. These versions included code to scan file systems, steal credentials, and exfiltrate data via GitHub repositories.

- **Vulnerable Workflow Exploited:** The breach originated from a workflow added to an outdated branch on August 21. Even after the maintainers quickly reverted the malicious workflow on the master branch, attackers used a pull request to reintroduce it through less-monitored code branches.

- **Detection and Remediation:** Nx maintainers identified the exploit via posts on social media platforms and their internal GitHub audits. They responded by:
  - Revoking all potentially compromised npm tokens.
  - Enforcing two-factor authentication (2FA) for all Nx npm package publishing.
  - Migrating publication to npm's Trusted Publisher, which removes dependency on npm tokens.

## Second Wave of Attack

Security firms (Wiz, StepSecurity) and independent researchers reported a second wave, utilizing compromised GitHub tokens to:

- Make victim's private repositories public and rename them following a distinct pattern.
- Turn thousands of sensitive repositories into publicly accessible data dumps.

## Technical Innovations and AI Tool Abuse

- Attackers implemented comprehensive credential harvesting, targeting GitHub tokens, npm auth keys, SSH private keys, various API keys, and even cryptocurrency wallet files.
- Notably, they leveraged large language model (LLM) AI CLI tools (e.g., Claude, Gemini, Q) for additional reconnaissance and enumeration of secrets—marking the first known weaponization of developer AI assistants in a supply chain exploit.
- Data was obfuscated using double-base64 encoding to evade standard detection mechanisms.

## Impact

- **GitGuardian** identified over 1,300 impacted repositories and more than 2,300 leaked secrets, including OAuth tokens and AI service credentials.
- 90% of leaked GitHub tokens were still valid as of reporting.
- Over 400 users and organizations were affected, with more than 5,500 repositories exposed in the second attack phase.

## Lessons and Moving Forward

- The incident highlights the evolving complexity and sophistication of supply chain attacks;
- Emphasizes the critical need for tight permissions, branch protection, secure workflow design, and continuous monitoring.
- Demonstrates how AI and automation tools can expand the attack surface, requiring new defense strategies in modern DevOps pipelines.

## References

- [Official Nx Security Advisory](https://github.com/nrwl/nx/security/advisories/GHSA-cxm3-wv7p-598c)
- [StepSecurity Incident Analysis](https://www.stepsecurity.io/blog/supply-chain-security-alert-popular-nx-build-system-package-compromised-with-data-stealing-malware)
- [Wiz Security Blog](https://www.wiz.io/blog/s1ngularity-supply-chain-attack)
- [GitGuardian Blog](https://devops.com/malicious-nx-packages-used-in-two-waves-of-supply-chain-attack/)

## Key Takeaways

- Secure CI/CD workflows through branch protection and least-privilege operation.
- Enforce 2FA and prefer trusted publisher mechanisms over static tokens.
- Monitor for lateral movement and credential harvesting in DevOps environments.
- Stay vigilant for malware exploiting both conventional credentials and new toolsets, such as AI-assisted development platforms.

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/malicious-nx-packages-used-in-two-waves-of-supply-chain-attack/?utm_source=rss&utm_medium=rss&utm_campaign=malicious-nx-packages-used-in-two-waves-of-supply-chain-attack)
