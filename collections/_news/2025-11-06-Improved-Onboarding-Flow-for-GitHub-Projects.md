---
external_url: https://github.blog/changelog/2025-11-06-improved-onboarding-flow-for-github-projects
title: Improved Onboarding Flow for GitHub Projects
author: Allison
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-11-06 17:50:59 +00:00
tags:
- DevOps Tools
- GitHub Projects
- GraphQL API
- Improvement
- Issue Management
- Onboarding
- Project Templates
- Projects & Issues
- Pull Requests
- Repository Integration
- Software Development
- Team Planning
- Workflow Automation
section_names:
- devops
---
Allison reports on recent upgrades to the GitHub Projects onboarding experience, detailing new automation workflows, repository integration features, and key API improvements for developers and project managers.<!--excerpt_end-->

# Improved Onboarding Flow for GitHub Projects

Allison details the latest enhancements to GitHub Projects onboarding, designed to help teams and developers get started more quickly and manage their projects more effectively.

## 🚀 Key Improvements

### Import Items from a Repository

You can now import open issues, open pull requests, or both directly from a chosen repository during the onboarding process. This allows instant access to existing items when creating a new project.

### Set a Default Repository

Project settings now include the option to select a default repository. Any new issues created within the project will automatically be associated with this repository, simplifying organization.

### Pull Request Linked to Issue Workflow

A new default workflow automatically updates an issue's status to “In progress” if a related pull request exists, providing better clarity on the project’s status.

## 🛠 Additional Onboarding Improvements

- Projects display linked pull requests and sub-issues as default fields.
- The Team planning template defaults to Table view instead of Board view for improved visibility.
- The **Insights** and **Workflows** buttons are more prominent in the UI for easier discovery.
- Minor fixes and UI enhancements further streamline the onboarding process.

## 🤖 API Improvements

Automation is now easier with updates to the Projects GraphQL API:

- Added event support for `project_v2_item_status_changed`, `added_to_project_v2`, `removed_from_project_v2`, and `converted_from_draft`, helping track progress and status changes.
- Project items can be filtered using the `query` argument with any project filter. Refer to [ProjectsV2 GraphQL documentation](https://docs.github.com/graphql/reference/objects#projectv2) for full details.

## Community Feedback

Share your feedback on these improvements by joining the [GitHub Community discussion](https://github.com/orgs/community/discussions/178930) or selecting **Give feedback** from the project menu.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-06-improved-onboarding-flow-for-github-projects)
