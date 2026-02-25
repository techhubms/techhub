---
layout: "post"
title: "Dependabot Update Grouping Across Multiple Directories"
description: "This news update highlights a new Dependabot feature allowing users to group dependency updates by name across multiple directories. This enhancement streamlines dependency management for repositories—especially monorepos—with several packages or services, consolidating update pull requests to reduce clutter and improve workflow efficiency. Instructions for configuration and supporting documentation links are provided."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-24-dependabot-can-group-updates-by-dependency-name-across-multiple-directories"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-24 21:28:17 +00:00
permalink: "/2026-02-24-Dependabot-Update-Grouping-Across-Multiple-Directories.html"
categories: ["DevOps"]
tags: ["Automation", "Code Security", "Configuration", "Dependabot", "Dependabot.yml", "Dependency Management", "DevOps", "GHES 3.21", "GitHub", "Improvement", "Monorepo", "News", "Pull Requests", "Supply Chain Security", "Version Updates"]
tags_normalized: ["automation", "code security", "configuration", "dependabot", "dependabotdotyml", "dependency management", "devops", "ghes 3dot21", "github", "improvement", "monorepo", "news", "pull requests", "supply chain security", "version updates"]
---

Allison introduces a Dependabot feature that allows grouping dependency updates by name across directories, simplifying dependency upgrades and reducing pull request volume for developers.<!--excerpt_end-->

# Dependabot Update Grouping Across Multiple Directories

Dependabot now enables grouping updates by dependency name across multiple directories, making it easier to manage dependency upgrades in repositories with multiple packages or services.

## What’s Changed

Previously, Dependabot would generate a separate pull request (PR) for each directory that required a dependency update. For example, if three different services in a monorepo needed the same dependency bumped, this would result in three PRs:

- `chore(deps): bump requests in /service-a`
- `chore(deps): bump requests in /service-b`
- `chore(deps): bump requests in /service-c`

With the new feature, you can consolidate these updates into a single PR per dependency, regardless of how many directories it affects. This is especially useful in monorepos, where updating a core package could otherwise generate a high number of PRs. The result is a simplified, more manageable upgrade workflow.

## Who Can Use This Feature?

This update is available to all github.com users and will also be included in GitHub Enterprise Server (GHES) 3.21.

## Getting Started

To leverage this new capability:

- Configure directory groups in your repository's `dependabot.yml` file.
- Refer to the [Dependabot configuration docs](https://docs.github.com/code-security/dependabot/dependabot-version-updates/configuration-options-for-the-dependabot.yml-file#groups) for setup instructions.

For questions and discussion, see the [Dependabot Community thread](https://github.com/dependabot/dependabot-core/issues/13284).

---

This update makes managing dependencies in complex repositories far more efficient for developers.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-24-dependabot-can-group-updates-by-dependency-name-across-multiple-directories)
