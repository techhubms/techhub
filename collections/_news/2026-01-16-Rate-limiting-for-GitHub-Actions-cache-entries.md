---
external_url: https://github.blog/changelog/2026-01-16-rate-limiting-for-actions-cache-entries
title: Rate limiting for GitHub Actions cache entries
author: Allison
feed_name: The GitHub Blog
date: 2026-01-16 21:31:29 +00:00
tags:
- Actions
- Cache Management
- Caching
- CI/CD
- GitHub Actions
- Marketplace Actions
- Rate Limiting
- Repository Management
- Retired
- System Stability
- Workflow Optimization
- DevOps
- News
section_names:
- devops
primary_section: devops
---
Allison details the new rate limit for GitHub Actions cache uploads, outlining its impact on repository workflows and system stability.<!--excerpt_end-->

# Rate Limiting for GitHub Actions Cache Entries

GitHub Actions cache now enforces a rate limit of 200 cache uploads per minute per repository. This update primarily affects the process of uploading new cache entries during workflow runs.

## Key Details

- **Affected Operations**: Only uploads of new cache entries are rate-limited. Downloads remain unaffected.
- **Reason for Change**: The update addresses scenarios where repositories upload many cache entries in quick succession, which increases cache thrash and can impact system stability for all users.
- **Collaborative Improvement**: GitHub is working with authors of certain Marketplace actions to refine their use of caching and reduce behaviors that exacerbate these issues.
- **User Impact**: If a repository exceeds the 200 uploads per minute limit, some cache entry uploads will be rejected until the limit resets. Users should anticipate and adjust workflows to account for this change.

For more information and ongoing updates, see the [GitHub Blog](https://github.blog/changelog/2026-01-16-rate-limiting-for-actions-cache-entries).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-16-rate-limiting-for-actions-cache-entries)
