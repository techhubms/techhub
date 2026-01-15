---
layout: post
title: GitHub Code Scanning Default Setup Bypasses Restrictive Actions Policies
author: Allison
canonical_url: https://github.blog/changelog/2025-11-25-code-scanning-default-setup-bypasses-github-actions-policy-blocks
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-11-25 16:09:17 +00:00
permalink: /devops/news/GitHub-Code-Scanning-Default-Setup-Bypasses-Restrictive-Actions-Policies
tags:
- Actions
- Application Security
- Code Scanning
- Continuous Integration
- Default Setup
- DevOps
- DevOps Tools
- Enterprise Server 3.19
- GitHub Actions
- Improvement
- News
- Platform Governance
- Policy Enforcement
- Repository Management
- Security
- Security Automation
- Security Coverage
- Workflow Policies
section_names:
- devops
- security
---
Allison summarizes a recent enhancement to GitHub code scanning, explaining how the default setup workflow now overrides restrictive GitHub Actions policies except for when actions are globally disabled.<!--excerpt_end-->

# GitHub Code Scanning Default Setup Bypasses Restrictive Actions Policies

GitHub has released an improvement to its code scanning service: the default setup workflow will now run regardless of most restrictive GitHub Actions policies in place for an organization or repository. Previously, organizations could unintentionally reduce their security coverage when workflow restriction policies blocked code scanning from executing.

## Key Points

- **Workflow Execution Change:** If a GitHub Actions policy limits which workflows may run (e.g., only allowing enterprise-approved workflows), code scanning's default setup will still execute as long as GitHub Actions are enabled for the repository or organization.
- **Exception – Actions Disabled:** The only policy that stops code scanning default setup is the global 'Disable actions' policy. If GitHub Actions are disabled, code scanning does not run.
- **Availability:** This change is now generally available for all GitHub plans and will be released for Enterprise Server in version 3.19.

## Implications for Security and Governance

- **Improved Security Coverage:** Organizations gain consistent code scanning regardless of custom workflow policies, lowering the risk of missed vulnerabilities due to policy misconfiguration.
- **Platform Governance:** Administrators still retain ultimate control; disabling GitHub Actions halts code scanning workflows entirely.

## More Information

- [Documentation for code scanning default setup](https://docs.github.com/code-security/code-scanning/default-setup)
- [GitHub Blog Announcement](https://github.blog/changelog/2025-11-25-code-scanning-default-setup-bypasses-github-actions-policy-blocks)

## Summary

This enhancement automatically protects repositories with code scanning, even under restrictive workflow policies, without requiring manual configuration. It reflects GitHub’s commitment to balancing platform governance with effective security tooling.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-25-code-scanning-default-setup-bypasses-github-actions-policy-blocks)
