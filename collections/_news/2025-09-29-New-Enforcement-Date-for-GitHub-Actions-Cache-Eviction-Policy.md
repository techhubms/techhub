---
external_url: https://github.blog/changelog/2025-09-29-new-date-for-enforcement-of-cache-eviction-policy
title: New Enforcement Date for GitHub Actions Cache Eviction Policy
author: Allison
feed_name: The GitHub Blog
date: 2025-09-29 22:02:07 +00:00
tags:
- Actions
- Automation
- Cache Eviction
- Cache Management
- CI/CD
- Continuous Integration
- GitHub Actions
- Policy Change
- Repository Management
- Retired
- Workflow Optimization
- DevOps
- News
section_names:
- devops
primary_section: devops
---
Allison reports on the postponement of GitHub Actions' new cache eviction policy, providing guidance for developers and organizations to review workflows before enforcement begins.<!--excerpt_end-->

# New Enforcement Date for GitHub Actions Cache Eviction Policy

GitHub has announced a delay in the enforcement of its updated cache eviction policy for GitHub Actions. Originally set for mid-October, the new enforcement date is now scheduled for November. This change impacts how often cache eviction occurs in GitHub Actions workflows, which may affect repository and organization cache management strategies.

## Current and Upcoming Cache Policy

- **Current Policy:**
  - Each repository has a **10 GB cache size limit** for GitHub Actions.
  - The oldest cache entries are evicted every **24 hours** to maintain the limit.
- **Updated Policy:**
  - Eviction checks will occur **every hour** instead of once per day.

## Impact on Repositories and Organizations

If your repository uses more than 10 GB of cache in a 24-hour period, you should have received an email notification from GitHub regarding this update. The increased eviction check frequency may cause additional cache thrashing and could require workflow adjustments to optimize cache usage.

## Recommendations

- **Review your workflows:** Check for unnecessary or inefficient cache usage that might trigger more frequent evictions under the new policy.
- **Monitor your cache usage:**
  - At the repository level: [GitHub documentation – usage limits and eviction policy](https://docs.github.com/actions/advanced-guides/caching-dependencies-to-speed-up-workflows#usage-limits-and-eviction-policy)
  - At the organization level: Visit `https://github.com/organizations/ORG/settings/actions/caches` (replace `ORG` with your organization’s name) to see aggregate cache usage.

## Action Required

With this enforcement delay, repository maintainers and developers have additional time to review and adjust their caching strategies to avoid disruptions. GitHub recommends making the necessary workflow changes as soon as possible to accommodate the new, more frequent eviction schedule.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-29-new-date-for-enforcement-of-cache-eviction-policy)
