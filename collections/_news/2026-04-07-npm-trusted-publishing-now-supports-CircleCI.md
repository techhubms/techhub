---
feed_name: The GitHub Blog
date: 2026-04-07 00:20:50 +00:00
author: Allison
tags:
- AI
- CI/CD
- CircleCI
- Credentialless Publishing
- Dark Mode
- DevOps
- GitHub Actions
- GitHub Copilot
- GitHub Copilot Agent Mode
- GitLab CI/CD
- Improvement
- News
- npm
- npm CLI
- npm Trust
- Npmjs.com
- OIDC
- OpenID Connect
- Security
- Supply Chain Security
- Tokenless Authentication
- Trusted Publishing
section_names:
- ai
- devops
- github-copilot
- security
external_url: https://github.blog/changelog/2026-04-06-npm-trusted-publishing-now-supports-circleci
primary_section: github-copilot
title: npm trusted publishing now supports CircleCI
---

Allison announces that npm Trusted Publishing now supports CircleCI as an OIDC provider, enabling credentialless releases from CI/CD workflows, and notes that npmjs.com dark mode was built using GitHub Copilot agent mode.<!--excerpt_end-->

# npm trusted publishing now supports CircleCI

npm Trusted Publishing now supports **CircleCI** as an **OIDC (OpenID Connect) provider**, in addition to **GitHub Actions** and **GitLab CI/CD**.

## What’s new

- **CircleCI can now be used for npm Trusted Publishing** via OIDC.
- Maintainers publishing from **CircleCI workflows** can **eliminate stored credentials** and authenticate directly through their CI/CD pipeline.
- With CircleCI added, trusted publishing now covers a “large majority” of npm publishers by CI provider.

## How to configure

Configuration is available through:

- The **npm website**
- The `npm trust` CLI command

For setup steps, see the official documentation:

- Trusted publishing docs: https://docs.npmjs.com/trusted-publishers

## Other shipping notes

- **Dark mode** is now available on **npmjs.com**: https://npmjs.com/
- The team states they built dark mode using **GitHub Copilot agent mode** (final review and shipping still done by engineers).

## Feedback / questions

- Community discussion: https://github.com/orgs/community/discussions/191807


[Read the entire article](https://github.blog/changelog/2026-04-06-npm-trusted-publishing-now-supports-circleci)

