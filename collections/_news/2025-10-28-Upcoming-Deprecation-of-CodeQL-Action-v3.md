---
layout: post
title: Upcoming Deprecation of CodeQL Action v3
author: Allison
canonical_url: https://github.blog/changelog/2025-10-28-upcoming-deprecation-of-codeql-action-v3
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-10-28 18:05:01 +00:00
permalink: /devops/news/Upcoming-Deprecation-of-CodeQL-Action-v3
tags:
- Action Deprecation
- Actions
- Application Security
- Code Scanning
- CodeQL Action
- CodeQL V4
- Dependabot
- DevOps
- GitHub Actions
- GitHub Enterprise Server
- News
- Node.js 24
- Retired
- Security
- Security Automation
- Workflow Update
section_names:
- devops
- security
---
Allison announces the upcoming deprecation of CodeQL Action v3, detailing the introduction of v4 on Node.js 24 and explaining workflow update requirements for security and DevOps teams using GitHub Actions.<!--excerpt_end-->

# Upcoming Deprecation of CodeQL Action v3

**Author: Allison**

On October 7, 2025, CodeQL Action v4 was released, running on the Node.js 24 runtime. CodeQL Action v3 is scheduled for deprecation alongside GitHub Enterprise Server (GHES) 3.19 in December 2026. This update affects teams leveraging GitHub Actions for code scanning and application security.

## Who Needs to Take Action?

### Default Setup

- **No immediate changes required** for users of [code scanning default setup](https://docs.github.com/code-security/code-scanning/enabling-code-scanning/configuring-default-setup-for-code-scanning).

### Advanced Setup

- **Workflow modifications needed.** Advanced setup users must update their workflow files in the `.github` directory of their repository to upgrade to CodeQL Action v4.

#### Instructions Based on Your Platform

- **github.com and GHES 3.20+:** Update workflow file references from `@v3` to `@v4` for all CodeQL Action steps (`init`, `autobuild`, `analyze`, `upload-sarif`).
- **GHES 3.19:** Node.js 24 support exists, but CodeQL Action v4 must be manually downloaded if not present. System administrators should use [GitHub Connect](https://docs.github.com/enterprise-server@3.11/admin/github-actions/managing-access-to-actions-from-githubcom/using-the-latest-version-of-the-official-bundled-actions#using-github-connect-to-access-the-latest-actions) to enable this.
- **GHES 3.18 and Older:** Node.js 24 Actions—including CodeQL Action v4—are not supported. Upgrade your GHES version before making workflow changes.

## Updating Your Workflow

Open the CodeQL workflow file(s) and replace any occurrence of these:

- `github/codeql-action/init@v3`
- `github/codeql-action/autobuild@v3`
- `github/codeql-action/analyze@v3`
- `github/codeql-action/upload-sarif@v3`

with the corresponding `@v4` versions.

## Using Dependabot

You can employ Dependabot to automate action dependency upgrades. [Learn more](https://docs.github.com/code-security/dependabot/working-with-dependabot/keeping-your-actions-up-to-date-with-dependabot).

## What Happens in December 2026?

- CodeQL Action v3 will be deprecated with the end of support for GHES 3.19.
- No further updates will be shipped for v3—new features and analysis capabilities will be exclusive to v4 users.
- GitHub may schedule "brownout" periods to encourage migration if many workflow files still reference v3.

## Key Takeaways

- **Upgrade as soon as possible** to CodeQL Action v4 for ongoing security enhancements and support.
- Leverage automation tools like Dependabot where possible.
- Monitor GitHub communications for further updates regarding brownouts or required migrations.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-28-upcoming-deprecation-of-codeql-action-v3)
