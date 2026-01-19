---
external_url: https://github.blog/changelog/2025-11-07-actions-pull_request_target-and-environment-branch-protections-changes
title: 'Important Changes to GitHub Actions: pull_request_target and Environment Branch Protection Rules'
author: Allison
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-11-07 16:18:15 +00:00
tags:
- Actions
- Branch Protection Rules
- CI/CD
- Code Scanning
- CodeQL
- Default Branch
- Environment Branch Protection
- GitHub Actions
- Improvement
- Merge Commit
- Permissions
- Pull Request Target
- Secrets Management
- Security Best Practices
- Workflow Security
section_names:
- devops
- security
---
Allison explains upcoming security-focused changes to how GitHub Actions handles pull_request_target events and environment branch protection rules, impacting workflow behavior for all developers and DevOps engineers.<!--excerpt_end-->

# Important Changes to GitHub Actions: pull_request_target and Environment Branch Protection Rules

**Author:** Allison

## Overview

GitHub will update the evaluation of `pull_request_target` events and environment branch protection rules for pull-request-related events, with these changes taking effect on **December 8, 2025**. The intention is to address long-standing security edge cases and make workflow execution more predictable and resilient against vulnerabilities.

## What is Changing?

### pull_request_target Event Behavior

- `pull_request_target` event workflows will **always use the repository’s default branch** (for workflow source and code reference) instead of the base branch of a pull request.
- This change ensures that only the default branch’s workflow definition and code execute for these events, avoiding outdated or vulnerable workflows present in other branches.
- Relevant environment variables:
  - `GITHUB_REF` now resolves to the default branch.
  - `GITHUB_SHA` points to the default branch’s latest commit.
- Previously, setting any branch as the base branch of a pull request could lead to execution from that branch. Now, only the default branch is authoritative, reducing the possibility of vulnerabilities introduced by outdated workflow code.

### Security Considerations

- The risk of user-supplied workflow code execution is reduced, helping mitigate known vulnerabilities associated with `pull_request_target` workflows.
- Still, using `pull_request_target` for pull requests from forks is **high risk**: these run code with environment secrets, potentially allowing untrusted contributors to exploit workflows.
- Developers should:
  - Avoid using user input or untrusted code sources in sensitive workflow steps.
  - Use `pull_request` instead of `pull_request_target` unless elevated permissions are necessary.
  - Restrict permissions (consider setting the default token permissions to read-only or a least-privilege setting).
  - Enable [CodeQL code scanning](https://docs.github.com/code-security/code-scanning/automatically-scanning-your-code-for-vulnerabilities-and-errors/about-code-scanning) to automatically spot workflow security issues.

### Environment Branch Protection Rules Changes

- Environment branch protection rules will be evaluated **against the execution reference** (`GITHUB_REF`), not the pull request’s head branch.
- For `pull_request`, `pull_request_review`, and `pull_request_review_comment` events, environment rules check `refs/pull/<number>/merge`, reflecting the merge commit context.
- For `pull_request_target`, environment rules evaluate against the default branch, consistent with the new event model.

### Breaking Changes and Impact

- Existing workflows relying on previous environment branch rule matching may **stop working as expected**.
- Update branch filter patterns to:
  - Add `refs/pull/*/merge` for `pull_request`-triggered workflows.
  - Add your repository's default branch for `pull_request_target`.
- Review whether existing environment branch protection rules still fit your security and operational needs, and consider alternative triggers or protection strategies if necessary.

## Recommended Actions

- **Assess**: Determine if and how you use `pull_request_target` and environment branch protection rules.
- **Update**: Modify workflow triggers and environment rules to match the new evaluation context.
- **Restrict Permissions**: Configure more restrictive permissions for workflow tokens wherever possible.
- **Enable CodeQL**: Use GitHub’s built-in [CodeQL code scanning](https://github.blog/changelog/2025-04-22-github-actions-workflow-security-analysis-with-codeql-is-now-generally-available/) for public repositories.
- **Stay Informed**: Monitor [GitHub Blog updates](https://github.blog/changelog/2025-11-07-actions-pull_request_target-and-environment-branch-protections-changes) and participate in the [community discussion](https://github.com/orgs/community/discussions/179107) if you have questions or feedback.

## Additional Resources

- [GitHub Actions Security Guides](https://docs.github.com/actions/security-guides/automatic-token-authentication#modifying-the-permissions-for-the-github_token)
- [Preventing Action Workflow Vulnerabilities](https://securitylab.github.com/resources/github-actions-preventing-pwn-requests/)

---

These changes require action from DevOps engineers and developers managing GitHub Actions workflows to maintain secure, functional pipelines. Be proactive in adjusting your configurations before December 8, 2025.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-07-actions-pull_request_target-and-environment-branch-protections-changes)
