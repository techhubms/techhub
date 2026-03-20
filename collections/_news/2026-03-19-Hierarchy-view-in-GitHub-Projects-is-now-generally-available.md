---
primary_section: github-copilot
section_names:
- ai
- devops
- github-copilot
title: Hierarchy view in GitHub Projects is now generally available
date: 2026-03-19 16:18:09 +00:00
external_url: https://github.blog/changelog/2026-03-19-hierarchy-view-in-github-projects-is-now-generally-available
feed_name: The GitHub Blog
author: Allison
tags:
- '@copilot'
- Accessibility
- AI
- Assignees Field
- DevOps
- GitHub Copilot
- GitHub Copilot Assignment
- GitHub Issues
- GitHub Projects
- Hierarchy View
- Improvement
- Issue Filters
- Issue Templates
- Maintainers
- News
- Project Views
- Projects & Issues
- Repository Permissions
- Sub Issues
- Triage Permission
- Write Access
---

Allison announces general availability of GitHub Projects’ hierarchy view and related Issues/Projects improvements, including how to assign GitHub Copilot via issue templates and new behavior for maintainer-only blank issues.<!--excerpt_end-->

# Hierarchy view in GitHub Projects is now generally available

[Hierarchy view in GitHub Projects](https://github.blog/changelog/2026-01-15-hierarchy-view-now-available-in-github-projects/) is now generally available. Starting today, **hierarchy view is enabled by default for all new project views**.

If you have existing views, you can enable it from the **View** menu by toggling **Show hierarchy**.

## Improvements and bug fixes since the last release

- **Improved discoverability of sub-issue filters**: Sub-issue specific filters are now easier to find in the filter suggestion bar.
- **Auto-apply filters to sub-issues**: When sub-issues are added to a filtered view, GitHub will attempt to automatically apply matching filters.

GitHub also shipped multiple **accessibility improvements** for a better overall experience.

For feedback or questions, join the discussion in GitHub Community: https://github.com/orgs/community/discussions/184225

## Additional improvements to Issues and Projects

## Assign Copilot via issue templates

You can now auto-assign Copilot in issue templates.

To get started, add `@copilot` to the `assignees` field in your issue template:

```yaml
assignees:
  - "@copilot"
```

This works for users with **write access** to the repository who are allowed to assign Copilot.

## Blank issues for maintainers

![Create new issue dialog showing multiple issue type options, with "Blank issue - maintainers only" highlighted](https://github.com/user-attachments/assets/b3f163d9-ca70-4ada-86ff-b33448c08068)

Users with **triage**, **write**, or **admin** access can now create blank issues even when `blank_issues_enabled: false` is configured in the repository’s issue template settings.

- For these users, the **Blank issue** option will appear in the template picker automatically.
- Contributors without write access will continue to be guided by templates.

[Read the entire article](https://github.blog/changelog/2026-03-19-hierarchy-view-in-github-projects-is-now-generally-available)

