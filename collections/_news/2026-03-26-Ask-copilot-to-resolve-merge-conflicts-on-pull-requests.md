---
tags:
- Admin Enablement
- Agentic Coding
- AI
- Build And Test Verification
- CI/CD
- Cloud Based Development Environment
- Code Review
- Copilot
- Copilot Business
- Copilot Coding Agent
- Copilot Enterprise
- DevOps
- GitHub
- GitHub Actions
- GitHub Copilot
- Improvement
- Merge Conflicts
- News
- Pull Requests
section_names:
- ai
- devops
- github-copilot
external_url: https://github.blog/changelog/2026-03-26-ask-copilot-to-resolve-merge-conflicts-on-pull-requests
author: Allison
primary_section: github-copilot
feed_name: The GitHub Blog
title: Ask @copilot to resolve merge conflicts on pull requests
date: 2026-03-26 17:32:22 +00:00
---

Allison announces that GitHub Copilot’s coding agent can now resolve merge conflicts on pull requests by mentioning @copilot in a PR comment, with the agent applying changes in a cloud environment and validating builds/tests before pushing updates.<!--excerpt_end-->

## Overview
[Copilot coding agent](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent) can now resolve merge conflicts on pull requests.

## How to ask Copilot to resolve merge conflicts
Add a comment on the pull request that mentions `@copilot` and describes the action you want it to take.

Example:

```text
@copilot Merge in main and resolve the conflicts
```

## How the coding agent works
The agent runs in its own cloud-based development environment where it can:

- Make the necessary code changes
- Check that the build still passes
- Run tests and confirm they still pass
- Push the resulting updates

## Other things Copilot coding agent can do
In addition to resolving merge conflicts, Copilot coding agent can:

- Fix failing GitHub Actions workflows

  ```text
  @copilot Fix the failing tests
  ```

- Address code review comments

  ```text
  @copilot Address this comment
  ```

- Make other requested changes (example: add tests)

  ```text
  @copilot Add a unit test covering the case when the model argument is missing
  ```

## Availability and enablement
Copilot coding agent is available with all paid Copilot plans.

If you’re a **Copilot Business** or **Copilot Enterprise** user, an administrator must first enable the feature:

- [Enable Copilot coding agent for Business and Enterprise](https://docs.github.com/enterprise-cloud@latest/copilot/concepts/agents/coding-agent/coding-agent-for-business-and-enterprise)

## Learn more
- [Asking GitHub Copilot to make changes to an existing pull request](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/make-changes-to-an-existing-pr)


[Read the entire article](https://github.blog/changelog/2026-03-26-ask-copilot-to-resolve-merge-conflicts-on-pull-requests)

