---
layout: "post"
title: "Dependabot Reviewers Option Removed in Favor of GitHub Code Owners"
description: "This post announces the retirement of the Dependabot reviewers configuration option on GitHub, highlighting that its functionality now overlaps with GitHub code owners. The article provides guidance for migrating Dependabot reviewer assignments to code owners, including workflow tools and detailed instructions for users making this transition."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-08-dependabot-reviewers-configuration-option-is-replaced-by-code-owners"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-08 18:53:03 +00:00
permalink: "/news/2025-08-08-Dependabot-Reviewers-Option-Removed-in-Favor-of-GitHub-Code-Owners.html"
categories: ["DevOps"]
tags: ["Automation Tools", "Code Owners", "Configuration Migration", "Dependabot", "DevOps", "GitHub", "GitHub Actions", "News", "Open Source Tools", "Repository Management", "Repository Settings", "Workflow Automation"]
tags_normalized: ["automation tools", "code owners", "configuration migration", "dependabot", "devops", "github", "github actions", "news", "open source tools", "repository management", "repository settings", "workflow automation"]
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
