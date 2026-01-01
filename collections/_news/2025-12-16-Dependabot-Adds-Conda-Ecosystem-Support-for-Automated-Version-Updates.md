---
layout: "post"
title: "Dependabot Adds Conda Ecosystem Support for Automated Version Updates"
description: "This announcement details Dependabot's new ability to parse and update Conda environment.yml files, enabling automated dependency management for Conda-based projects on GitHub. Teams using Conda can now benefit from automated version updates, improving security and project maintenance, with this feature available on GitHub.com and soon to GitHub Enterprise Server."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-12-16-conda-ecosystem-support-for-dependabot-now-generally-available"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-12-16 17:00:54 +00:00
permalink: "/2025-12-16-Dependabot-Adds-Conda-Ecosystem-Support-for-Automated-Version-Updates.html"
categories: ["DevOps"]
tags: ["Automated Updates", "Conda", "Continuous Integration", "Dependabot", "Dependency Management", "DevOps", "Environment.yml", "GitHub", "GitHub Enterprise Server", "Improvement", "News", "Python", "Supply Chain Security"]
tags_normalized: ["automated updates", "conda", "continuous integration", "dependabot", "dependency management", "devops", "environmentdotyml", "github", "github enterprise server", "improvement", "news", "python", "supply chain security"]
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
