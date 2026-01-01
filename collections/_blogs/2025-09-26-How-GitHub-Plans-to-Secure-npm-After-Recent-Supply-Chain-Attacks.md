---
layout: "post"
title: "How GitHub Plans to Secure npm After Recent Supply Chain Attacks"
description: "This article explains GitHub's response to the recent Shai-Hulud worm attack on the npm registry. It covers new security measures including mandatory two-factor authentication, granular short-lived tokens, deprecation of legacy authentication, and the rollout of trusted publishing for npm and other package registries. The implications for software supply chain security and actionable recommendations for maintainers are discussed."
author: "Tom Smith"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devops.com/how-github-plans-to-secure-npm-after-recent-supply-chain-attacks/"
viewing_mode: "external"
feed_name: "DevOps Blog"
feed_url: "https://devops.com/feed/"
date: 2025-09-26 07:58:54 +00:00
permalink: "/2025-09-26-How-GitHub-Plans-to-Secure-npm-After-Recent-Supply-Chain-Attacks.html"
categories: ["DevOps", "Security"]
tags: ["API Token Deprecation", "Blogs", "Business Of DevOps", "CI/CD Authentication", "CI/CD Pipelines", "Crates.io", "Crates.io Security", "DevOps", "DevSecOps", "GitHub", "npm", "npm 2FA", "npm Registry Attack", "npm Token Theft", "NuGet", "Open Source Security", "Package Registry", "Package Registry Malware", "PyPI", "PyPI Trusted Publishing", "RubyGems", "Security", "Shai Hulud Worm", "Social Facebook", "Social LinkedIn", "Social X", "Software Supply Chain", "Software Supply Chain Security", "Supply Chain Security", "Token Deprecation", "Trusted Publishing", "WebAuthn"]
tags_normalized: ["api token deprecation", "blogs", "business of devops", "cislashcd authentication", "cislashcd pipelines", "cratesdotio", "cratesdotio security", "devops", "devsecops", "github", "npm", "npm 2fa", "npm registry attack", "npm token theft", "nuget", "open source security", "package registry", "package registry malware", "pypi", "pypi trusted publishing", "rubygems", "security", "shai hulud worm", "social facebook", "social linkedin", "social x", "software supply chain", "software supply chain security", "supply chain security", "token deprecation", "trusted publishing", "webauthn"]
---

Tom Smith discusses how GitHub is overhauling npm security after the Shai-Hulud worm attack, describing new authentication requirements and trusted publishing to defend the software supply chain.<!--excerpt_end-->

# How GitHub Plans to Secure npm After Recent Supply Chain Attacks

Recent supply chain attacks, particularly the Shai-Hulud worm infiltration in September 2025, have exposed vulnerabilities in open source package registries like npm. In response, GitHub is rolling out a comprehensive set of security measures designed to protect the software ecosystem and prevent the misuse of developer credentials.

## The Shai-Hulud Worm Impact

- The attack spread via compromised maintainer accounts and post-install scripts, exfiltrating API tokens and other secrets.
- Over 500 npm packages were compromised, highlighting flaws in traditional authentication and the risks of long-lived credentials.
- GitHub reacted by removing affected packages and enhancing detection capabilities, but the incident demonstrated the need for systemic change.

## New Security Roadmap for npm

- **Mandatory Two-Factor Authentication (2FA):** Local publishing now requires 2FA, eliminating previous bypass methods.
- **Granular Tokens:** API publishing tokens will expire after seven days, tightening credential exposure windows.
- **Trusted Publishing:** npm and other registries will use CI/CD-based attestation, where workflows issue signed tokens to prove package origin. This replaces secret management with automation inherent to trusted pipelines.
- **Deprecation of Weak Authentication:**
  - Legacy tokens are being retired.
  - Time-based one-time passwords (TOTP) will be replaced by more secure FIDO/WebAuthn keys.
  - Token-based publishing is disabled by default for new packages.

## The Rise of Trusted Publishing

- First introduced with PyPI in 2023 and later adopted by RubyGems, crates.io, npm, and NuGet.
- In CI/CD pipelines (such as GitHub Actions), the identity of the workflow is verified and used to authenticate publishing events without exposing static tokens.
- This model shifts responsibility from developers managing secrets to automated workflows with provable lineage, dramatically reducing the attack surface for malware or account takeovers.

## How Maintainers Can Prepare

- **Switch to trusted publishing:** Move projects to registry-supported trusted publishing modes as soon as possible.
- **Enable 2FA across accounts and organizations:** Make it mandatory for all publishing actions.
- **Adopt hardware-based authentication:** Use WebAuthn or FIDO keys for verifiable 2FA.
- **Regularly audit and prune credentials:** Remove unused tokens from all repositories and build environments.

## Broader Industry Implications

- The speed of adoption for these measures is accelerating due to increasing threats.
- The OpenSSF Securing Software Repositories Working Group is now recommending trusted publishing as an industry best practice.
- While these measures may break some existing workflows or feel less convenient, the risk of widespread malware in major package registries means robustness must take priority over ease.

## Conclusion

The Shai-Hulud attack is a wake-up call for the open source community. GitHub’s push towards 2FA, expiring tokens, and especially trusted publishing represents a necessary evolution in DevOps and supply chain practices. These improvements ask all maintainers and contributors to take proactive steps—now—to strengthen the entire ecosystem.

**Further reading:**

- [Shai-Hulud Attacks Shake Software Supply Chain Security Confidence](https://devops.com/shai-hulud-attacks-shake-software-supply-chain-security-confidence/)
- [OpenSSF Securing Software Repositories Working Group](https://openssf.org/)

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/how-github-plans-to-secure-npm-after-recent-supply-chain-attacks/)
