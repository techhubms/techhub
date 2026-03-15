---
external_url: https://github.blog/changelog/2025-08-08-dependabot-reviewers-configuration-option-is-replaced-by-code-owners
title: Dependabot Reviewers Option Removed in Favor of GitHub Code Owners
author: Allison
feed_name: The GitHub Blog
date: 2025-08-08 18:53:03 +00:00
tags:
- Automation Tools
- Code Owners
- Configuration Migration
- Dependabot
- GitHub
- GitHub Actions
- Open Source Tools
- Repository Management
- Repository Settings
- Workflow Automation
- DevOps
- News
section_names:
- devops
primary_section: devops
---
Allison shares details on the removal of the Dependabot reviewers configuration on GitHub, explaining how users can transition to using code owners for repository management.<!--excerpt_end-->

# Dependabot Reviewers Configuration Option Replaced by Code Owners

GitHub has announced the retirement of the Dependabot reviewers configuration option. This decision comes because the reviewers configuration's functionality significantly overlaps with GitHub's existing code owners feature.

## Why Was the Change Made?

- The role of defining code reviewers in Dependabot is now covered by GitHub's code owners.
- This streamlines repository management and reduces redundancy in assigning responsibility for code changes.

## How to Migrate to Code Owners

If you were using Dependabot reviewer assignments, you will need to move those settings to a code owners configuration. GitHub provides several resources to help:

- **Migration Guide and Tools:**
  - Visit the [dependabot/codeowner-migration-action repository](https://github.com/dependabot/codeowner-migration-action) for:
    - A GitHub Actions workflow to automate migration
    - A command-line script
    - Manual step-by-step instructions

## Additional Support

For questions or issues during migration, you are encouraged to:

- Review the migration action’s README documentation
- Participate in the [GitHub Community discussion](https://github.com/orgs/community/discussions)

This change is designed to simplify maintenance and ensure consistent workflows for repository teams.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-08-dependabot-reviewers-configuration-option-is-replaced-by-code-owners)
