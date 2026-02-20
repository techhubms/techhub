---
external_url: https://github.blog/changelog/2026-02-05-github-actions-self-hosted-runner-minimum-version-enforcement-extended
title: 'GitHub Actions: Enforcement of Minimum Self-hosted Runner Version Extended to March 16, 2026'
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-02-05 12:53:50 +00:00
tags:
- Actions
- Automation
- Brownout Schedule
- CI/CD
- Configuration Management
- Continuous Deployment
- Continuous Integration
- DevOps
- GitHub Actions
- IaC
- News
- Pipeline
- Release Management
- Retired
- Runner Upgrade
- Self Hosted Runner
- Upgrade
- Version Enforcement
section_names:
- devops
---
Allison details GitHub's extension of the deadline for minimum self-hosted runner version enforcement in GitHub Actions until March 16, 2026, with clear guidance and brownout schedules for DevOps teams.<!--excerpt_end-->

# GitHub Actions: Self-hosted Runner Minimum Version Enforcement Extended

GitHub has officially extended the deadline for enforcing the minimum supported version of self-hosted GitHub Actions runners to March 16, 2026. Organizations now have an extra week to upgrade all self-hosted runners to version 2.329.0 or later. This change aims to standardize environments, improve security, and ensure feature compatibility with the GitHub Actions platform.

## What’s Changing

- **Minimum runner version enforced:** From March 16, 2026, GitHub Actions will *block* new configurations from self-hosted runners running versions earlier than v2.329.0 (released October 15, 2025).
- **Brownout period:** Between February 16 and March 16, 2026, GitHub will implement scheduled blocks (brownouts) at specific times across various global timezones to help admins identify outdated runners.
- **After the deadline:** Runners below v2.329.0 will **not** connect or self-upgrade after running the configuration script. Registration will be blocked for all outdated runners.

## Brownout Schedule

Brownout events will temporarily block configurations from runners below v2.329.0 according to the published schedule (see the original blog link for a detailed table by region and date). Examples:

- Feb 16: 08:00-09:00 UTC (with corresponding times US Pacific, Eastern, Central Europe, India, Japan/Korea, Australia)
- Multiple sessions over four weeks, culminating in full enforcement on March 16.

## What DevOps Teams Need to Do

1. **Download** the latest GitHub Actions runner from the [Actions Runner releases page](https://github.com/actions/runner/releases).
2. **Update installation scripts/images** to ensure version 2.329.0+ is installed before invoking `./config.sh`.
3. **Redeploy any outdated runners**: Recreate or redeploy from images, templates, or scripts using the older version.
4. **Review [upgrade documentation](https://docs.github.com/actions/hosting-your-own-runners/managing-self-hosted-runners/about-self-hosted-runners#communication-requirements)** for details about connectivity and compliance.

## Why It Matters

Keeping self-hosted runners current ensures you receive security updates, new features, and platform support from GitHub. The brownout schedule provides time and transparency for teams to modernize their automation infrastructure.

For more details and the full brownout schedule, see the [GitHub Actions changelog post](https://github.blog/changelog/2026-02-05-github-actions-self-hosted-runner-minimum-version-enforcement-extended).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-05-github-actions-self-hosted-runner-minimum-version-enforcement-extended)
