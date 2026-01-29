---
external_url: https://github.blog/changelog/2025-11-20-github-actions-cache-size-can-now-exceed-10-gb-per-repository
title: Expanded GitHub Actions Cache Limits Exceed 10 GB per Repository
author: Allison
feed_name: The GitHub Blog
date: 2025-11-20 21:01:36 +00:00
tags:
- Actions
- Automation
- Billing
- Cache Management
- CI/CD
- Codespaces
- Enterprise Account
- Enterprise Admin
- Eviction Policies
- Git LFS
- GitHub Actions
- Improvement
- Pro Account
- Repository Management
- Retention Policies
- Storage Policies
- Team Account
- Workflow Optimization
- DevOps
- News
section_names:
- devops
primary_section: devops
---
Allison details the update allowing GitHub Actions cache size to exceed 10 GB per repository, introducing new policy controls and pay-as-you-go storage options for administrators.<!--excerpt_end-->

# Expanded GitHub Actions Cache Limits Exceed 10 GB per Repository

GitHub Actions now enables repositories to store more build dependencies by extending the cache size beyond the existing 10 GB cap. This major improvement supports larger workflows and faster builds, especially for complex projects.

## Key Updates

- **Expanded Storage**: All repositories have access to 10 GB cache at no cost. Above this limit, storage follows a pay-as-you-go model, comparable to pricing for Git LFS and Codespaces.
- **Admin Controls**: Enterprise, Organization, and Repository admins can configure increased cache limits for their repositories. Upgrades require a valid Pro, Team, or Enterprise account.

## Cache Management Policies

Two new management options have been introduced:

- **Cache Size Eviction Limit (GB)**: Sets the maximum cached data allowed per repository. Exceeding this limit automatically evicts the least recently used entries, ensuring the configured cap is respected.
- **Cache Retention Limit (days)**: Controls how long cache entries are preserved after their last access. Defaults are set to 10 GB and seven days at no additional cost.

Admins can adjust these policies using Actions settings for individual repositories and organizations, or via cascading Enterprise policies. For instance, an enterprise-level maximum applies to all child organizations.

## Billing and Budgets

- **Flexible Billing**: Storage above default limits is billed as a separate SKU.
- **Budget Controls**: Billing owners can set spending limits. When reached, affected repositories’ caches switch to read-only mode until the next billing cycle.

## References

- [GitHub Actions cache documentation](https://docs.github.com/en/actions/using-workflows/caching-dependencies-to-speed-up-workflows)
- [Official GitHub Blog Announcement](https://github.blog/changelog/2025-11-20-github-actions-cache-size-can-now-exceed-10-gb-per-repository)

## Summary

This update offers greater flexibility for teams managing large builds and workflows. Admins now have granular control over cache storage and retention, with built-in budget enforcement for cost management.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-20-github-actions-cache-size-can-now-exceed-10-gb-per-repository)
