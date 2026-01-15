---
layout: post
title: GitHub Ruleset Exemptions and New Repository Insights Features
author: Allison
canonical_url: https://github.blog/changelog/2025-09-10-github-ruleset-exemptions-and-repository-insights-updates
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-09-10 17:25:36 +00:00
permalink: /devops/news/GitHub-Ruleset-Exemptions-and-New-Repository-Insights-Features
tags:
- Accessibility
- Automation
- Branch Protection
- DevOps
- Exempt Bypass
- GitHub
- Improvement
- Migration
- News
- Platform Governance
- Pulse Pages
- Repository Analytics
- Repository Rulesets
- Traffic Insights
section_names:
- devops
---
Allison details new features in GitHub repository ruleset management, introducing the exempt bypass type for trusted actors and improved accessibility in repository insights.<!--excerpt_end-->

# GitHub Ruleset Exemptions and Repository Insights Updates

GitHub has introduced two important improvements for repository management:

## New Ruleset Bypass Type: Exemptions

- Repository rulesets now offer an `exempt` bypass type, allowing specific actors (users, teams, or GitHub Apps) to be excused from all enforced rules.
- Unlike the standard 'break glass' bypass, exemptions do not generate audit signals and operate silently, ideal for trusted automation or large-scale migration.
- **Benefits:**
  - Reduces friction for trusted, high-volume automation (including bots handling migrations or integrations).
  - Speeds up large repository migrations without disabling overall rules.
  - Enables behavior parity with classic branch protection's “Restrict who can push.”
- **How to Configure an Exemption:**
  1. Navigate to your repository's ruleset settings.
  2. In the “Bypass” list, locate the actor you want to exempt.
  3. Open the context menu (**…**).
  4. Select the **Exempt** option.
- This feature helps maintain security while optimizing workflow for automation and migration scenarios.

## Enhanced Repository Insights for Accessibility

- GitHub repository insights (traffic and pulse) now feature more accessible graphs for users relying on screen readers.
- Users can switch graphs to tabular view or download the data as CSV or PNG, aiding reporting and analysis workflows.
- **Temporary Note:** Referring sites and popular content data on traffic insights may be missing or inaccurate as of this update — GitHub is working on a fix.

## Additional Information

- For more information or to participate in discussions, see the [GitHub Community](https://github.com/orgs/community/discussions/categories/repositories).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-10-github-ruleset-exemptions-and-repository-insights-updates)
