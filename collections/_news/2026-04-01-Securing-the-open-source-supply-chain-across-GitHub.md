---
tags:
- CI/CD
- Code Scanning
- CodeQL
- Commit SHA Pinning
- Dependabot
- DevOps
- GitHub Actions
- GitHub Advisory Database
- GitHub Security Lab
- Malware Scanning
- News
- npm
- NuGet
- OIDC
- OpenID Connect
- OpenSSF
- Pull Request Target
- PyPI
- Script Injection
- Secret Exfiltration
- Security
- Security Hardening
- Supply Chain Security
- Trusted Publishing
- Workflow Security
- Workload Identity
title: Securing the open source supply chain across GitHub
external_url: https://github.blog/security/supply-chain-security/securing-the-open-source-supply-chain-across-github/
primary_section: devops
feed_name: The GitHub Blog
author: Zachary Steindler
date: 2026-04-01 19:20:12 +00:00
section_names:
- devops
- security
---

Zachary Steindler explains how recent open source supply-chain attacks often start with compromised GitHub Actions workflows, and lays out concrete steps—like CodeQL scanning, pinning Actions by SHA, and moving from secrets to OIDC—to reduce risk.<!--excerpt_end-->

# Securing the open source supply chain across GitHub

Over the past year, a new pattern has emerged in open source supply-chain attacks: attackers focus on **exfiltrating secrets** (for example API keys) so they can:

- Publish malicious packages from an attacker-controlled machine
- Gain access to additional projects and propagate the attack

A common entry point is compromising a **GitHub Actions** workflow.

## What you can do today

### 1) Scan your workflows with CodeQL

Enable **CodeQL** to review your GitHub Actions workflow implementation (free for public repositories):

- [CodeQL queries for GitHub Actions workflows](https://docs.github.com/code-security/reference/code-scanning/codeql/codeql-queries/actions-built-in-queries)

### 2) Follow GitHub Actions security guidance

Review GitHub’s Actions security guidance:

- [Secure use of GitHub Actions](https://docs.github.com/actions/reference/security/secure-use)

Key recommendations highlighted:

- Don’t trigger workflows on `pull_request_target`.
- Pin third-party Actions to **full-length commit SHAs**:
  - [Pinning third-party actions](https://docs.github.com/actions/reference/security/secure-use#using-third-party-actions)
- Do pinning yourself or via Dependabot; be suspicious of external PRs that update pinned references.
- Watch for **script injection** when referencing user-submitted content:
  - [Mitigating script injection attacks](https://docs.github.com/actions/reference/security/secure-use#good-practices-for-mitigating-script-injection-attacks)

### 3) Monitor compromised dependencies

GitHub publishes information about compromised dependencies in the Advisory Database:

- [GitHub Advisory Database](https://github.com/advisories)

You can also use tools like Dependabot to get notified about malicious/vulnerable dependencies (free for public repositories):

- [Dependabot malware alerts](https://docs.github.com/code-security/concepts/supply-chain-security/dependabot-malware-alerts)

## What GitHub has done

### Prefer OIDC over long-lived secrets

Instead of using secrets inside workflows, use an **OpenID Connect (OIDC) token** that contains the workflow’s workload identity to authorize activities:

- [Security harden deployments (OIDC)](https://docs.github.com/actions/how-tos/secure-your-work/security-harden-deployments)

GitHub notes it has worked with many systems to integrate with Actions via this approach, including cloud providers, package repositories, and other hosted services.

### Trusted publishing in package repositories

GitHub partners with the **OpenSSF** to support “trusted publishing” in package repositories:

- [OpenSSF post: package repository security](https://openssf.org/blog/2024/07/31/how-to-make-programming-language-package-repositories-more-secure/)

Trusted publishing is described as:

- Removing secrets from build pipelines
- Providing a signal if a package that previously used trusted publishing stops doing so (which can indicate compromise)

GitHub says this is supported across multiple ecosystems, including:

- npm, PyPI, NuGet, RubyGems, Crates (and others)

### npm malware scanning at scale

GitHub describes npm scale and its approach:

- npm has 30,000+ packages published each day
- GitHub scans every npm package version for malware
- Hundreds of newly published packages contain malicious code daily
- Human review is used to confirm detections before action
- False positives matter operationally (even 1% would disrupt hundreds of legitimate publishes daily)

## What to expect in the coming months

### npm security roadmap work (late 2025 context)

The “Shai-Hulud” attacks motivated a revamped npm security roadmap. GitHub points to:

- [Our plan for a more secure npm supply chain](https://github.blog/security/supply-chain-security/our-plan-for-a-more-secure-npm-supply-chain/)
- [Strengthening supply chain security: Preparing for the next malware campaign](https://github.blog/security/supply-chain-security/strengthening-supply-chain-security-preparing-for-the-next-malware-campaign/)

They describe accelerating:

- npm trusted publishing
- malware detection/removal
- collaboration with maintainers

They also note that changes can require workflow updates and may cause backwards incompatibility, so they’re aiming for a smooth transition.

### GitHub Actions security roadmap work

GitHub says it’s revisiting the GitHub Actions security roadmap and accelerating security capabilities:

- [What’s coming to our GitHub Actions 2026 security roadmap](https://github.blog/news-insights/product-news/whats-coming-to-our-github-actions-2026-security-roadmap/)

Feedback link:

- [Community discussion: GitHub Actions security roadmap](https://github.com/orgs/community/discussions/190621)

## Where do we go from here?

GitHub positions open source as a global public good and expects continued attacks, while committing to defensive work across npm, GitHub Actions, and future areas—along with continued community feedback as capabilities roll out.

[Read the entire article](https://github.blog/security/supply-chain-security/securing-the-open-source-supply-chain-across-github/)

