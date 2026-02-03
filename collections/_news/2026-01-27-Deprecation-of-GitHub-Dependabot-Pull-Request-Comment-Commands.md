---
external_url: https://github.blog/changelog/2026-01-27-changes-to-github-dependabot-pull-request-comment-commands
title: Deprecation of GitHub Dependabot Pull Request Comment Commands
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-01-27 19:35:52 +00:00
tags:
- Automation
- Comment Commands
- Dependabot
- Deprecation
- DevOps
- GitHub
- GitHub CLI
- News
- Pull Requests
- REST API
- Retired
- Supply Chain Security
- Workflow Migration
section_names:
- devops
---
Allison announces the deprecation of specific Dependabot pull request comment commands, guiding users to adopt GitHub’s native pull request features for workflow management.<!--excerpt_end-->

# Deprecation of GitHub Dependabot Pull Request Comment Commands

As announced in October 2025, GitHub has officially deprecated several Dependabot-specific pull request comment commands in favor of the platform’s built-in features for pull request management. The intention is to simplify workflows, reduce confusion, and ensure greater reliability by consolidating on GitHub’s natively supported tools and interfaces.

## Deprecated Comment Commands

The following Dependabot comment commands are no longer supported:

- `@dependabot merge`
- `@dependabot cancel merge`
- `@dependabot squash and merge`
- `@dependabot close`
- `@dependabot reopen`

## Migration Guidance

Users are encouraged to update their workflows by leveraging GitHub-native features for pull request operations, such as merging, closing, or reopening pull requests. Recommended alternatives include:

- GitHub’s web-based user interface for pull requests
- [GitHub CLI](https://cli.github.com/manual/gh_pr) for managing pull requests from the command line
- [REST API endpoints for pull requests](https://docs.github.com/rest/pulls?apiVersion=2022-11-28) for programmatic interaction

These changes aim to streamline pull request management and promote the use of GitHub's continually improving native tooling.

---

For further details, see [the original change announcement](https://github.blog/changelog/2025-10-07-upcoming-changes-to-github-dependabot-pull-request-comment-commands/) and [the GitHub Blog post](https://github.blog/changelog/2026-01-27-changes-to-github-dependabot-pull-request-comment-commands).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-27-changes-to-github-dependabot-pull-request-comment-commands)
