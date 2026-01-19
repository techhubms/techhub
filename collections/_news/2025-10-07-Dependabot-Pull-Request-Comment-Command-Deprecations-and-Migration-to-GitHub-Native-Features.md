---
layout: post
title: Dependabot Pull Request Comment Command Deprecations and Migration to GitHub Native Features
author: Allison
canonical_url: https://github.blog/changelog/2025-10-06-upcoming-changes-to-github-dependabot-pull-request-comment-commands
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-10-07 13:14:05 +00:00
permalink: /devops/news/Dependabot-Pull-Request-Comment-Command-Deprecations-and-Migration-to-GitHub-Native-Features
tags:
- Automation
- CLI
- Dependabot
- Deprecation
- Developer Tools
- GitHub
- PR Management
- Pull Requests
- REST API
- Retired
- Supply Chain Security
- Workflow Migration
section_names:
- devops
---
Allison outlines the deprecation of certain Dependabot pull request commands and recommends using GitHub’s native pull request management tools, offering actionable migration guidance.<!--excerpt_end-->

# Upcoming Deprecation of Dependabot Pull Request Comment Commands

On November 11, 2025, GitHub will retire several Dependabot-specific pull request comment commands to streamline pull request management and encourage adoption of GitHub’s integrated features. The deprecated commands include:

- `@dependabot merge`
- `@dependabot cancel merge`
- `@dependabot squash and merge`
- `@dependabot close`
- `@dependabot reopen`

**Timeline and Impact:**

- Through the next month, using these commands will trigger Dependabot to comment with a deprecation notice on affected pull requests.
- Starting November 11th, the above comment commands will no longer be supported on GitHub pull requests.

## Migration Guidance

To adapt your workflows, utilize GitHub’s native methods for pull request operations:

- **GitHub Web UI:** Merge, close, or reopen pull requests directly through the standard interface.
- **GitHub CLI:** Use the [gh pr](https://cli.github.com/manual/gh_pr) commands for managing pull requests from the command line.
- **REST API:** Automate PR management via [pull request REST API endpoints](https://docs.github.com/rest/pulls?apiVersion=2022-11-28).

### Why This Change?

- Reduces confusion between third-party commands and GitHub’s native capabilities.
- Improves reliability by centralizing pull request management in supported GitHub interfaces.
- Encourages use of modern GitHub workflow tooling, benefiting security and usability.

For more details and migration examples, refer to the official GitHub CLI and REST API documentation.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-06-upcoming-changes-to-github-dependabot-pull-request-comment-commands)
