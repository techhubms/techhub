---
section_names:
- ai
- devops
- github-copilot
author: Allison
external_url: https://github.blog/changelog/2026-04-03-organization-runner-controls-for-copilot-cloud-agent
date: 2026-04-03 19:15:11 +00:00
title: Organization runner controls for Copilot cloud agent
tags:
- Agentic Coding
- AI
- Coding Agent
- Copilot
- Copilot Cloud Agent
- Copilot Setup Steps.yml
- DevOps
- Enterprise Governance
- GitHub Actions
- GitHub Copilot
- GitHub Hosted Runners
- Guardrails
- Large Runners
- News
- Organization Settings
- Performance
- Repository Settings
- Runner Configuration
- Self Hosted Runners
feed_name: The GitHub Blog
primary_section: github-copilot
---

Allison explains a GitHub Copilot cloud agent update that lets organization admins set and optionally lock the GitHub Actions runner used for agent tasks, making it easier to enforce consistent defaults and guardrails across repositories.<!--excerpt_end-->

# Organization runner controls for Copilot cloud agent

Each time the [Copilot cloud agent](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent) works on a task, it starts a new development environment powered by GitHub Actions.

## How the agent environment runs

- By default, the agent runs on a standard GitHub-hosted runner.
- Teams can also [customize the agent environment](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/use-copilot-agents/coding-agent/customize-the-agent-environment) to use:
  - Large runners
  - Self-hosted runners

Reasons to customize include faster performance and access to internal resources.

## What changed: organization-level runner governance

Previously, the runner was configured at the repository level via a `copilot-setup-steps.yml` file. This made it hard to roll out consistent defaults or enforce guardrails across an organization.

Organization admins can now:

- **Set a default runner** that applies automatically across all repositories (no per-repo configuration required).
- **Lock the runner setting** so individual repositories can’t override the organization default.

This enables org-wide defaults (for example, using larger GitHub Actions runners for performance) and enforcement that the agent runs on approved infrastructure (such as self-hosted runners).

## References

- GitHub Docs: [Configuring runners for GitHub Copilot cloud agent in your organization](https://docs.github.com/copilot/how-tos/administer-copilot/manage-for-organization/configure-runner-for-coding-agent)


[Read the entire article](https://github.blog/changelog/2026-04-03-organization-runner-controls-for-copilot-cloud-agent)

