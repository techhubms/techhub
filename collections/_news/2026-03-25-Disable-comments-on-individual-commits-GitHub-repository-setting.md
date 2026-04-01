---
tags:
- Code Review
- Collaboration
- Collaboration Tools
- Commit Comments
- Commit Pages
- DevOps
- GitHub
- GraphQL API
- Improvement
- Inline Diff Comments
- Maintainers
- Moderation
- News
- Noise Reduction
- Repository Administration
- Repository Settings
- REST API
primary_section: devops
date: 2026-03-25 17:07:52 +00:00
section_names:
- devops
external_url: https://github.blog/changelog/2026-03-25-disable-comments-on-individual-commits
feed_name: The GitHub Blog
title: Disable comments on individual commits (GitHub repository setting)
author: Allison
---

Allison explains a new GitHub repository setting that lets admins disable comments on individual commits, including what changes in the UI and how comment creation is blocked via the REST and GraphQL APIs.<!--excerpt_end-->

# Disable comments on individual commits (GitHub repository setting)

Repository admins can now disable comments on individual commits, aimed at reducing unwanted noise on older commits.

## What changed

A new **Commits** section is available in repository settings with an option:

- **Allow comments on individual commits** (enabled by default to preserve existing behavior)

![The Commits section in repository settings, showing a checkbox labeled 'Allow comments on individual commits'](https://github.com/user-attachments/assets/6bf4a702-452e-450f-8bb1-a09e607aff2d)

## Behavior when commit comments are disabled

When **Allow comments on individual commits** is unchecked:

- The comment form is hidden on commit pages.
- Inline diff comment UI and inline thread reply capabilities are hidden on commit pages.
- Creating commit comments is blocked via:
  - REST API
  - GraphQL API
- Existing commit comments are **not** removed:
  - They can still be viewed, edited, and deleted.

## How to enable/disable

1. Go to your repository **Settings**.
2. Scroll to the **Commits** section.
3. Uncheck **Allow comments on individual commits**.

## Feedback

GitHub is collecting feedback in the Community discussion:

- Community discussion: https://github.com/orgs/community/discussions/189794


[Read the entire article](https://github.blog/changelog/2026-03-25-disable-comments-on-individual-commits)

