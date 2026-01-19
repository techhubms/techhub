---
layout: post
title: GitHub Actions Adds YAML Anchors and Workflow Templates from Non-Public Repositories
author: Allison
canonical_url: https://github.blog/changelog/2025-09-18-actions-yaml-anchors-and-non-public-workflow-templates
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-09-18 18:37:50 +00:00
permalink: /devops/news/GitHub-Actions-Adds-YAML-Anchors-and-Workflow-Templates-from-Non-Public-Repositories
tags:
- .github Repositories
- Actions
- Automation
- Check Run Id
- CI/CD
- Continuous Deployment
- Continuous Integration
- GitHub
- GitHub Actions
- Improvement
- Job Context
- Workflow Configuration
- Workflow Templates
- YAML Anchors
section_names:
- devops
---
Allison details new updates to GitHub Actions, including YAML anchor support, workflow templates from non-public repositories, and a new job check run ID feature for streamlined DevOps automation.<!--excerpt_end-->

# GitHub Actions Adds YAML Anchors and Workflow Templates from Non-Public Repositories

GitHub Actions continues its evolution with a set of key updates that improve workflow configuration and automation flexibility for development teams:

## Reuse Workflow Configuration with YAML Anchors

YAML anchors are now supported in GitHub Actions workflows. This highly requested feature enables you to reuse workflow snippets and configurations across multiple jobs or steps, improving consistency and maintainability in complex pipelines. With YAML anchors:

- You can define reusable blocks (anchors) and reference them (aliases) throughout your workflow files.
- This ensures your workflows adhere more closely to the YAML specification, reducing configuration errors.
- The feature is enabled automatically for all users and repositories—no setup required.

For examples and best practices, refer to [YAML anchors documentation](https://docs.github.com/actions/reference/workflows-and-actions/reusing-workflow-configurations#yaml-anchors-and-aliases).

## Use Workflow Templates from Non-Public Repositories

Organizations can now store workflow templates in internal or private `.github` repositories, not just public ones. This allows for:

- Sharing automation templates securely within your organization without exposing them publicly.
- Internal `.github` repositories support use in internal and private repositories.
- Private `.github` repositories restrict use to private repos only.
- These changes apply only to Actions workflows. Other GitHub products (e.g., Issues templates) still require public `.github` repositories.

Read more about [workflow templates](https://docs.github.com/actions/concepts/workflows-and-actions/reusing-workflow-configurations#workflow-templates).

## Identify Running Jobs with `check_run_id`

A new value, `check_run_id`, is now available in the GitHub Actions job context. This unique identifier:

- Helps you track the currently running job from inside the workflow.
- Simplifies advanced scenarios like custom reporting, sending targeted notifications, and artifact uploads linked to specific jobs.
- Access it using `job.check_run_id` in your workflow configuration.

These enhancements are available now to all GitHub Actions users.

For community questions or to discuss these changes, join the conversation in the [GitHub Community](https://github.com/orgs/community/discussions/categories/announcements).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-18-actions-yaml-anchors-and-non-public-workflow-templates)
