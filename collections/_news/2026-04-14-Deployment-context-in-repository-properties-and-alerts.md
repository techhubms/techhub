---
tags:
- Application Security
- Artifacts
- Branch Protection Rules
- Compliance Policies
- Dependabot
- Deployable
- Deployed
- Deployment Context
- DevOps
- GitHub
- GitHub Code Scanning
- GitHub Rulesets
- News
- Platform Governance
- Production Context
- Repository Properties
- Runtime Risk Context
- Security
- Security Alerts
- Vulnerability Triage
external_url: https://github.blog/changelog/2026-04-14-deployment-context-in-repository-properties-and-alerts
date: 2026-04-14 20:26:26 +00:00
feed_name: The GitHub Blog
author: Allison
primary_section: devops
section_names:
- devops
- security
title: Deployment context in repository properties and alerts
---

Allison announces new GitHub features that surface deployment and runtime context in repository properties and security alert pages, helping teams automate policy enforcement and prioritize Dependabot and code scanning alerts based on real production risk.<!--excerpt_end-->

# Deployment context in repository properties and alerts

Artifact and deployment context now appears in two places in GitHub: **repository properties** and **security alert pages**.

## Repository properties: `deployable` and `deployed`

Two new built-in repository properties—`deployable` and `deployed`—are now available.

- These properties reflect **existing artifact and deployment metadata**.
- You don’t need to manually maintain lists of which repositories are actively deployed.

You can use these properties to:

- Filter repositories in your organization based on deployment context.
- Apply rulesets, branch protections, and compliance policies automatically to repositories based on deployment context.
- Keep policy enforcement accurate as deployment state changes over time.

![Filters being used to get list of deployed or deployable filters](https://github.com/user-attachments/assets/462b6784-baa8-46e3-83c1-d723903c044f)

## Runtime risk context in security alerts

**Dependabot** and **GitHub code scanning** alert pages now show **runtime risk context** directly on the alert.

When you open an alert, you’ll see additional runtime context for the affected artifact.

This context helps security teams:

- Triage alerts based on actual runtime context, rather than treating every alert as equally urgent.
- Quickly identify which vulnerabilities exist in services that are at higher risk.
- Reduce time spent manually cross-referencing environment and exposure data.

![Runtime Context metadata present on security alerts to enable triage and prioritization](https://github.com/user-attachments/assets/5922b5b5-f459-4a47-8b8c-422045ca5502)

## Availability

Both features are now **generally available**.

## Learn more

- [Associate artifacts with production context](https://docs.github.com/code-security/tutorials/secure-your-organization/prioritize-alerts-in-production-code#1-associate-artifacts-with-production-context)
- [Search repositories based on deployment context](https://docs.github.com/search-github/searching-on-github/searching-for-repositories?versionId=free-pro-team%40latest&productId=repositories&restPage=working-with-files%2Cusing-files%2Cnavigating-code-on-github#search-based-on-deployment-context)


[Read the entire article](https://github.blog/changelog/2026-04-14-deployment-context-in-repository-properties-and-alerts)

