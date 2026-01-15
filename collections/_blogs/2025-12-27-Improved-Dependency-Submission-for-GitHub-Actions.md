---
layout: post
title: Improved Dependency Submission for GitHub Actions
author: Jesse Houwing
canonical_url: https://jessehouwing.net/github-actions-improved-dependency-submission/
viewing_mode: external
feed_name: Jesse Houwing's Blog
feed_url: https://jessehouwing.net/rss/
date: 2025-12-27 18:47:09 +00:00
permalink: /devops/blogs/Improved-Dependency-Submission-for-GitHub-Actions
tags:
- Actions Dependency Submission
- Automated Security
- Blogs
- CI/CD
- Dependabot
- Dependency Graph
- Dependency Submission
- DevOps
- GitHub
- GitHub Actions
- Open Source Security
- Permissions
- Security
- Security Advisories
- Supply Chain Security
- Supplychain Security
- Vulnerability Management
- YAML Workflow
section_names:
- devops
- security
---
Jesse Houwing addresses a visibility gap in GitHub Actions security when actions are pinned by SHA. The post details a workflow extension ensuring vulnerabilities are properly surfaced in the Dependency Graph and by Dependabot.<!--excerpt_end-->

# Improved Dependency Submission for GitHub Actions

GitHub Actions users often pin actions by SHA for enhanced security, following best practices. However, as [Jesse Houwing](https://jessehouwing.net) explains, doing so can prevent vulnerabilities from being detected via GitHub's Dependency Graph and Dependabot. This article details why, and introduces a manual submission approach to restore the lost security visibility.

## The Problem: Gaps in Security Advisory Matching

- GitHub Security Advisories and Dependency Graph rely on version metadata—**not SHAs**—to identify vulnerable dependencies.
- When you pin an action like `actions/checkout` using a SHA (e.g. `@8e8c483db...`), the advisory database can't match this to known vulnerable version ranges.
- The same problem applies to organizational forks of actions: advisories for `actions/checkout` don't match `myorg/actions_checkout`.

**Impacted features:**

- Vulnerabilities missing from Dependency Graph
- Dependabot Security Updates won't trigger
- The dependency-review-action can't block PRs adding vulnerable actions

## The Solution: Enhanced Dependency Submission

Jesse's solution is a custom [actions-dependency-submission](https://github.com/jessehouwing/actions-dependency-submission) workflow that:

- Resolves pinned SHAs to their corresponding highest specific version and records both
- Maps wildcard versions to current specific versions
- Detects and reports the upstream of a forked action

**With this enrichment:**

- The Dependency Graph will correctly recognize and flag vulnerable GitHub Actions
- Dependabot and the dependency-review-action can function as expected

## Implementation Example

Add the action to your workflow YAML, for example:

```yaml
name: Submit and validate dependencies of GitHub Actions

on:
  push:
    branches:
      - main
  pull_request:
  schedule:
    - cron: '33 4 * * *'

permissions: {}

jobs:
  submit-actions-dependencies:
    runs-on: ubuntu-latest
    permissions:
      contents: write # Required for submitting dependencies
    steps:
      - uses: actions/checkout@8e8c483db84b4bee98b60c0593521ed34d9990e8 # v6.0.1
      - uses: jessehouwing/actions-dependency-submission@e848a29fd84b874cce3e45ceb00619bc72dbeca3 # 1.0.2
        with:
          token: ${{ secrets.GITHUB_TOKEN }}
      - uses: actions/dependency-review-action@3c4e3dcb1aa7874d2c16be7d79418e9b7efd6261 # 4.8.2
        if: github.event_name == 'pull_request'
        with:
          retry-on-snapshot-warnings: true
```

## Testing the Flow

To confirm the action works:

- Deliberately add a known vulnerable action (e.g., `actions/download-artifact@v4.1.2`) and verify it is flagged by the Dependency Graph.
- The graph should now visualize both SHA-based and version-based dependencies.

## Resources

- [jessehouwing/actions-dependency-submission on GitHub](https://github.com/jessehouwing/actions-dependency-submission)
- Further examples, documentation, and badges available in the repo

---
By improving dependency reporting for GitHub Actions, you retain the benefits of pinned actions without losing out on automated vulnerability detection.

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/github-actions-improved-dependency-submission/)
