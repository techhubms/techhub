---
date: 2026-03-24 20:08:03 +00:00
feed_name: The GitHub Blog
author: Allison
title: Ask @copilot to make changes to any pull request
external_url: https://github.blog/changelog/2026-03-24-ask-copilot-to-make-changes-to-any-pull-request
section_names:
- ai
- devops
- github-copilot
tags:
- '@copilot'
- Admin Enablement
- AI
- CI/CD
- Cloud Based Development Environment
- Code Review
- Copilot
- Copilot Business
- Copilot Coding Agent
- Copilot Enterprise
- Copilot Plans
- DevOps
- Failing Tests
- GitHub
- GitHub Actions
- GitHub Copilot
- Improvement
- Linters
- News
- Pull Requests
- Unit Tests
primary_section: github-copilot
---

Allison announces an update to GitHub Copilot’s coding agent: you can now mention @copilot directly in any pull request to request changes like fixing GitHub Actions failures, addressing review comments, or adding tests, with optional behavior to open a separate PR if requested.<!--excerpt_end-->

# Ask @copilot to make changes to any pull request

You can now mention `@copilot` in any pull request to ask GitHub Copilot to make changes.

## What you can ask `@copilot` to do

- **Fix failing GitHub Actions workflows**
  - Example: `@copilot Fix the failing tests`
- **Address code review comments**
  - Example: `@copilot Address this comment`
- **Make other code changes**
  - Example: `@copilot Add a unit test covering the case when the model argument is missing`

## How the Copilot coding agent works

[Copilot coding agent](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent) runs in its own cloud-based development environment. It can:

- make code changes
- validate its work using your tests and linter
- push updates

## Change from previous behavior (optional separate PR)

Previously, Copilot would open a new pull request on top of your existing pull request, using the existing pull request’s branch as its base branch.

If you still want that behavior, you can ask in natural language, for example:

- `@copilot open a PR to fix the failing tests`

## Availability and admin controls

Copilot coding agent is available with all paid Copilot plans.

- Copilot Business / Copilot Enterprise: an administrator must enable it before use.
  - See: [Enable Copilot coding agent for Business and Enterprise](https://docs.github.com/enterprise-cloud@latest/copilot/concepts/agents/coding-agent/coding-agent-for-business-and-enterprise)

## Learn more

- [Asking GitHub Copilot to make changes to an existing pull request](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/make-changes-to-an-existing-pr)


[Read the entire article](https://github.blog/changelog/2026-03-24-ask-copilot-to-make-changes-to-any-pull-request)

