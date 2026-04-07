---
date: 2026-04-07 14:14:49 +00:00
section_names:
- devops
title: Dependabot version updates now support the Nix ecosystem
feed_name: The GitHub Blog
external_url: https://github.blog/changelog/2026-04-07-dependabot-version-updates-now-support-the-nix-ecosystem
primary_section: devops
tags:
- Dependabot
- Dependabot.yml
- Dependency Updates
- DevOps
- Flake.lock
- Git
- GitHub
- GitHub Code Security
- GitHub Pull Requests
- GitLab
- News
- Nix
- Nix Flakes
- SourceHut
- Supply Chain Security
- Version Updates
author: Allison
---

Allison announces that Dependabot now supports Nix flakes for version updates, letting teams configure `dependabot.yml` to track `flake.lock` inputs and receive pull requests when upstream commits change.<!--excerpt_end-->

# Dependabot version updates now support the Nix ecosystem

Dependabot now supports **Nix flakes** for **version updates**.

With this change, you can configure Dependabot to watch your flake inputs and automatically open pull requests when there are newer upstream commits.

## What’s supported

- **Nix flakes** as a package ecosystem
- Monitoring of **`flake.lock` inputs**
- **One pull request per outdated flake input**
- Supported input sources:
  - GitHub
  - GitLab
  - Sourcehut
  - Plain `git` inputs

## Important limitation

This support applies to **version updates only**, not security updates:

- Version updates: https://docs.github.com/code-security/dependabot/dependabot-version-updates/about-dependabot-version-updates
- Security updates: https://docs.github.com/code-security/dependabot/dependabot-security-updates/about-dependabot-security-updates

## Get started

1. Add a `nix` entry to your `.github/dependabot.yml`.
2. Dependabot will start opening pull requests for your flake inputs on the **next scheduled run**.

## Learn more

- Configuring Dependabot version updates: https://docs.github.com/code-security/dependabot/dependabot-version-updates/configuring-dependabot-version-updates
- Introduction to Nix flakes: https://zero-to-nix.com/concepts/flakes/
- Community discussion on Nix support: https://github.com/dependabot/dependabot-core/issues/7340


[Read the entire article](https://github.blog/changelog/2026-04-07-dependabot-version-updates-now-support-the-nix-ecosystem)

