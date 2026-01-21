---
external_url: https://github.blog/changelog/2025-12-16-conda-ecosystem-support-for-dependabot-now-generally-available
title: Dependabot Adds Conda Ecosystem Support for Automated Version Updates
author: Allison
feed_name: The GitHub Blog
date: 2025-12-16 17:00:54 +00:00
tags:
- Automated Updates
- Conda
- Continuous Integration
- Dependabot
- Dependency Management
- Environment.yml
- GitHub
- GitHub Enterprise Server
- Improvement
- Python
- Supply Chain Security
section_names:
- devops
---
Allison announces that Dependabot now supports version updates for Conda's environment.yml files, helping teams automate the management of Conda dependencies on GitHub.<!--excerpt_end-->

# Dependabot Adds Conda Ecosystem Support for Automated Version Updates

Dependabot, GitHub's automated dependency updater, now supports parsing and updating `environment.yml` files used in Conda-based projects. This means teams using Conda for dependency and environment management can take advantage of Dependabot to automatically track and update the versions of Python dependencies listed in their Conda manifest files.

## Why it Matters

- **Broader Ecosystem Coverage**: Many projects depend on Conda for managing complex dependencies. By supporting Conda, Dependabot helps ensure that these projects remain up-to-date with the latest dependency versions.
- **Security and Maintenance**: Automated dependency updates reduce the risk of vulnerabilities by keeping libraries current, improving overall project security.
- **Efficiency**: Teams benefit from reduced manual effort in tracking and updating version changes.

## How It Works

- Dependabot detects `environment.yml` files in repositories.
- It parses the listed packages and their versions.
- When an update is needed, Dependabot creates a pull request to update dependencies directly in the file.

Refer to the [Dependabot documentation](https://docs.github.com/code-security/dependabot/ecosystems-supported-by-dependabot/supported-ecosystems-and-repositories#supported-ecosystems-and-repositories) for further details, examples, and guidance on enabling this feature.

## Availability

- **On GitHub.com**: This capability is available today.
- **On GitHub Enterprise Server (GHES)**: Planned availability with version 3.21.

You can join the discussion or ask questions in the [Dependabot Community](https://github.com/dependabot/dependabot-core/issues/2227).

---

Stay up to date on improvements to dependency management workflows and supply chain security by following the latest announcements from GitHub.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-16-conda-ecosystem-support-for-dependabot-now-generally-available)
