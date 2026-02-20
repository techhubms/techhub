---
external_url: https://github.blog/changelog/2026-02-17-required-reviewer-rule-is-now-generally-available
title: Required Reviewer Rule for Repository Rulesets Now Generally Available
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-02-17 15:35:39 +00:00
tags:
- Automated Workflows
- Branch Protection
- CODEOWNERS
- Collaboration Tools
- DevOps
- File Pattern Matching
- GitHub
- GitHub Enterprise
- Merge Approvals
- Negation Patterns
- News
- Platform Governance
- Policy Enforcement
- Repository Rulesets
- Required Reviewer
- Review Policies
- Team Collaboration
section_names:
- devops
---
Allison announces the general availability of GitHub's required reviewer rule for repository rulesets, highlighting improvements in review policy management for teams and organizations.<!--excerpt_end-->

# Required Reviewer Rule for Repository Rulesets Now Generally Available

**Author:** Allison

The required reviewer rule for GitHub repository rulesets is now generally available, providing organizations and enterprises with fine-grained control over code review requirements across branches and files.

## What's New Since Public Preview

- **Negation Patterns:** Negation (`!`), similar to `.gitignore`, is now supported in pattern matching for file and folder paths. This allows teams to exclude specific files or folders from review requirements for more precise policy implementation.
- **Granular Control:** You can require a specific number of approvals from designated teams before merging into protected branches.
- **Consistent Policy Scaling:** Review policies can be easily enforced across repositories, organizations, or at the enterprise level.

## Differences from CODEOWNERS

- **Policy Enforcement:** While CODEOWNERS assigns ownership and allows individuals as reviewers, the required reviewer rule is focused on enforcing review policies—such as mandating approvals for sensitive branches or critical paths.
- **Complementary Approach:** The new rule augments CODEOWNERS by providing policy controls, but doesn’t replace it. CODEOWNERS remains a primary tool for managing ownership and optional review requests.

### Example Use Cases

- Require reviews from a specific team (e.g., Data Platform) for all `*.sql` changes
- Enforce multiple reviews by the security team for authentication-related files before merging into `main`
- Exclude certain files or directories from mandatory review using the new negation pattern

For more details, see the [documentation about rulesets](https://docs.github.com/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/about-rulesets).

Discuss these features or ask questions in the [GitHub Community discussions](https://github.com/orgs/community/discussions/categories/repositories).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-17-required-reviewer-rule-is-now-generally-available)
