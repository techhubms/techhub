---
layout: post
title: Managing Issue Dependencies in GitHub Now Generally Available
author: Allison
canonical_url: https://github.blog/changelog/2025-08-21-dependencies-on-issues
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-08-21 16:32:58 +00:00
permalink: /devops/news/Managing-Issue-Dependencies-in-GitHub-Now-Generally-Available
tags:
- Agile Practices
- Blocked Issues
- GitHub
- GraphQL API
- Issue Dependencies
- Project Management
- REST API
- Software Development
- Team Collaboration
- Webhooks
- Workflow Automation
section_names:
- devops
---
Allison announces that issue dependencies are now generally available in GitHub, empowering teams to manage blocking relationships between issues and streamline project workflows.<!--excerpt_end-->

# Managing Issue Dependencies in GitHub Now Generally Available

GitHub has announced that dependencies on issues are now generally available. This new feature allows teams to specify which issues are **blocked by** or **blocking** others, streamlining project planning and ensuring that work is completed in the correct order. This capability has been highly requested by the GitHub community.

## Getting Started

- Open the **Relationships** section in the issues sidebar.
- Select **Mark as blocked by** or **Mark as blocking** to define issue dependencies.
- Link up to 50 issues for each relationship type.

Visual walkthroughs are available in the linked [demo videos](https://github.blog/wp-content/uploads/2025/08/mark_blocked_by.mp4#t=0.001).

## Searching and Viewing Blocked Issues

- Blocked issues can be viewed within projects and the repository **Issues** tab.
- Use the following search filters to manage dependencies:
  - `is:blocked`: Lists all blocked issues
  - `is:blocking`: Lists all blocking issues
  - `blocked-by:`: Shows issues blocked by a specific issue
  - `blocking:`: Shows issues blocking a specific issue

A sample search video is also [provided](https://github.blog/wp-content/uploads/2025/08/search_blocked_by.mp4#t=0.001).

## API and Webhook Support

- Issue dependencies are fully supported in both the [REST API](https://docs.github.com/rest/issues/issue-dependencies?versionId=free-pro-team@latest&page=mutations&apiVersion=2022-11-28) and [GraphQL API](https://docs.github.com/graphql/reference/mutations#addblockedby).
- Webhooks also support dependency notifications ([see documentation](https://docs.github.com/webhooks/webhook-events-and-payloads#issue_dependencies)).

## Feedback and Further Learning

- Join discussions in the [GitHub Community](https://github.com/orgs/community/discussions/categories/projects-and-issues).
- Use the **Give feedback** option in projects menus.
- For additional help, check [GitHub Issues and Projects](https://github.com/features/issues) and related [documentation](https://docs.github.com/issues).

This update helps teams manage their work more transparently and keep complex workflows organized.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-21-dependencies-on-issues)
