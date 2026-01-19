---
external_url: https://github.blog/changelog/2025-10-07-upcoming-changes-to-github-dependabot-pull-request-comment-commands
title: Deprecation of Dependabot Pull Request Comment Commands in Favor of GitHub Native Features
author: Allison
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-10-07 13:14:05 +00:00
tags:
- Dependabot
- Developer Productivity
- GitHub
- GitHub CLI
- GitHub Platform
- Merge Operations
- Open Source Security
- Pull Requests
- REST API
- Retired
- Supply Chain Security
- Workflow Automation
section_names:
- devops
---
Allison announces key changes for developers: several Dependabot pull request comment commands will be deprecated in January 2026, with recommendations to use GitHub's built-in tools for PR workflows.<!--excerpt_end-->

# Deprecation of Dependabot Pull Request Comment Commands in Favor of GitHub Native Features

On January 27, 2026, several dependabot-specific pull request comment commands will be deprecated. These include:

- `@dependabot merge`
- `@dependabot cancel merge`
- `@dependabot squash and merge`
- `@dependabot close`
- `@dependabot reopen`

## What’s changing?

GitHub is retiring these Dependabot comment-driven commands to reduce confusion and to encourage the use of GitHub’s built-in capabilities for managing pull requests. As a result, developers will need to rely on standard methods for handling merges, closures, and reopens.

## Next steps for developers

**Migration Guidance:**

- Use GitHub’s native pull request UI for merging, closing, or reopening pull requests.
- Consider the [GitHub CLI](https://cli.github.com/manual/gh_pr) for command-line PR management.
- Use [REST API endpoints for pull requests](https://docs.github.com/rest/pulls?apiVersion=2022-11-28) to automate or integrate PR operations into custom workflows.

**Timeline and Transition:**

- Leading up to the deprecation date, Dependabot will post reminders in pull requests when these commands are used.
- After January 27, 2026, these commands will no longer function.

## Summary

Shifting to GitHub's native features is aimed at making workflow management more reliable and reducing confusion for developers who rely on Dependabot for dependency updates.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-07-upcoming-changes-to-github-dependabot-pull-request-comment-commands)
