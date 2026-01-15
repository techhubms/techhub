---
layout: post
title: GitHub's Roadmap for Strengthening npm Supply Chain Security
author: Xavier René-Corail
canonical_url: https://github.blog/security/supply-chain-security/our-plan-for-a-more-secure-npm-supply-chain/
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/feed/
date: 2025-09-23 00:29:50 +00:00
permalink: /devops/news/GitHubs-Roadmap-for-Strengthening-npm-Supply-Chain-Security
tags:
- API Tokens
- Authentication
- CI/CD
- Cybersecurity
- DevOps
- GitHub
- GitHub Security Lab
- Malware
- News
- npm
- Open Source
- OpenSSF
- Package Registry
- Publishing Workflow
- Security
- Supply Chain Security
- Tokens
- Trusted Publishing
- Two Factor Authentication
section_names:
- devops
- security
---
Xavier René-Corail outlines GitHub's response to recent npm supply chain attacks, including plans for stricter authentication, granular tokens, and trusted publishing to help maintainers and organizations secure their npm projects.<!--excerpt_end-->

# GitHub's Roadmap for Strengthening npm Supply Chain Security

Open source software is essential to modern development, but recent attacks on major package registries, including npm, revealed serious security risks. In this update, Xavier René-Corail from GitHub explains how npm and the broader community are improving the security of the supply chain and provides clear actions for maintainers.

## Summary of Recent Attacks on the npm Ecosystem

- Surge in damaging account takeovers, letting attackers distribute malicious packages.
- Notable incident: The "Shai-Hulud" worm used compromised maintainer accounts and malicious post-install scripts to spread malware across npm packages.
- GitHub's incident response:
  - Removed 500+ compromised packages.
  - Blocked uploads containing malware indicators.

These threats highlight the need for higher authentication standards and more secure publishing practices across open source ecosystems.

## Roadmap for Hardening npm Publishing

GitHub is introducing changes to defend against token abuse and self-replicating malware:

1. **Enforced Two-Factor Authentication (2FA):** Required for local publishing.
2. **Granular Access Tokens:** Now have a seven-day lifetime to reduce exposure in the event of leakage.
3. **Trusted Publishing:** Adopts standards that avoid API tokens in build systems, improving CI/CD security.

Additional steps:

- Deprecation of classic tokens and TOTP 2FA (migrating users to FIDO/WebAuthn).
- Shorter expiration for publishing-permitted tokens.
- Default token publishing access disabled; encourages trusted publishing or 2FA.
- Eliminating the option to skip 2FA for local publishing.
- Expanded eligible providers for trusted publishing.

These changes will roll out gradually, with support resources and clear migration guides for the community.

## Trusted Publishing: Best Practice for Ecosystem Security

"Trusted publishing"—which avoids the need for API tokens in the CI/CD pipeline—is recommended by OpenSSF and already adopted across other registries such as PyPI, RubyGems, crates.io, and NuGet. npm now supports this practice and maintainers are strongly advised to switch.

## Recommended Actions for npm Maintainers

- Switch to [trusted publishing](https://docs.npmjs.com/trusted-publishers) instead of using tokens in your pipelines.
- Enforce 2FA for accounts and organizations—[set publishing settings](https://docs.npmjs.com/requiring-2fa-for-package-publishing-and-settings-modification) to mandate this.
- Prefer WebAuthn (FIDO) for 2FA rather than TOTP apps.

## Community Commitment

GitHub emphasizes that supply chain security is a shared responsibility and will support the ecosystem throughout this transition, aiming for long-term resilience by adopting modern secure publishing practices and increasing awareness of potential threats.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/security/supply-chain-security/our-plan-for-a-more-secure-npm-supply-chain/)
