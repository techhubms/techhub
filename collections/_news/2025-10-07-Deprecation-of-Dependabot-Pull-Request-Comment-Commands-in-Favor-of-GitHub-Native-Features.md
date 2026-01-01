---
layout: "post"
title: "Deprecation of Dependabot Pull Request Comment Commands in Favor of GitHub Native Features"
description: "This announcement details the upcoming deprecation of several Dependabot-specific pull request comment commands, effective January 27, 2026. It provides guidance on switching to GitHub's native features like the UI, CLI, and REST API for PR management, aiming to streamline workflows and improve platform reliability for developers managing dependencies."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-10-07-upcoming-changes-to-github-dependabot-pull-request-comment-commands"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-10-07 13:14:05 +00:00
permalink: "/2025-10-07-Deprecation-of-Dependabot-Pull-Request-Comment-Commands-in-Favor-of-GitHub-Native-Features.html"
categories: ["DevOps"]
tags: ["Dependabot", "Developer Productivity", "DevOps", "GitHub", "GitHub CLI", "GitHub Platform", "Merge Operations", "News", "Open Source Security", "Pull Requests", "REST API", "Retired", "Supply Chain Security", "Workflow Automation"]
tags_normalized: ["dependabot", "developer productivity", "devops", "github", "github cli", "github platform", "merge operations", "news", "open source security", "pull requests", "rest api", "retired", "supply chain security", "workflow automation"]
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
