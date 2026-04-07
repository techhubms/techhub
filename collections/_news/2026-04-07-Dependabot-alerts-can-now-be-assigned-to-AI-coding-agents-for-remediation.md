---
date: 2026-04-07 14:22:51 +00:00
section_names:
- ai
- devops
- github-copilot
- security
title: Dependabot alerts can now be assigned to AI coding agents for remediation
feed_name: The GitHub Blog
external_url: https://github.blog/changelog/2026-04-07-dependabot-alerts-are-now-assignable-to-ai-agents-for-remediation
primary_section: github-copilot
tags:
- AI
- AI Coding Agents
- Breaking Changes
- Claude
- Codex
- Dependabot
- Dependabot Alerts
- Dependabot Security Updates
- Dependency Vulnerabilities
- DevOps
- Draft Pull Request
- GitHub Advanced Security
- GitHub Code Security
- GitHub Copilot
- GitHub Copilot Coding Agent
- Improvement
- Major Version Upgrade
- News
- Package Downgrade
- Secure Development Lifecycle
- Security
- Software Supply Chain Security
- Supply Chain Security
- Test Failures
- Vulnerability Remediation
author: Allison
---

Allison announces a GitHub feature that lets you assign Dependabot alerts to AI coding agents (including GitHub Copilot) to analyze vulnerabilities and open draft pull requests with proposed fixes, with guidance on when it helps and why human review is still required.<!--excerpt_end-->

## Overview

Some dependency vulnerabilities can’t be fixed with a simple version bump—they may require code changes across the project. GitHub now lets you assign a Dependabot alert to an AI coding agent (including Copilot, Claude, and Codex) so the agent can analyze the issue and open a draft pull request with a proposed remediation.

## How it works

From the Dependabot alert detail page:

1. Select **Assign to Agent**.
2. Choose a coding agent (Copilot, Claude, or Codex).

After assignment, the agent will:

1. Analyze the alert, including advisory details and your repository’s dependency usage.
2. Open a **draft pull request** with a proposed fix.
3. Attempt to resolve **test failures** introduced by the update.

### Multiple agents per alert

You can assign multiple agents to the same alert. Each agent works independently and opens its own draft pull request, so you can compare different approaches.

## When coding agents help (beyond Dependabot’s automatic PRs)

Dependabot security updates already open pull requests to upgrade vulnerable dependencies to the nearest patched version. For many alerts, merging that PR is enough.

Coding agents are intended to help when updates are more complex, for example:

- **Fixing breaking changes**: Major version upgrades can introduce breaking API changes, deprecated calls, or incompatible type signatures. Agents can analyze failing builds/tests, identify likely causes, and propose code changes.
- **Package downgrades**: If a dependency is compromised (e.g., malware) and no patched version exists, an agent can downgrade to the last known safe version.
- **Complex pull requests**: For scenarios that don’t fit Dependabot’s rule-based update engine, an agent can generate a more involved remediation PR.

In short: Dependabot handles the version bump; the coding agent can propose the additional code changes needed to make the upgrade (or downgrade) work.

## Always review agent output

AI-generated fixes aren’t guaranteed to be correct. Agents can:

- produce incomplete patches
- miss edge cases
- introduce new issues

Always review the pull request, ensure tests pass, and confirm the fix is appropriate before merging.

## Who can use this feature?

Requirements:

- [GitHub Code Security / GitHub Advanced Security](https://docs.github.com/get-started/learning-about-github/about-github-advanced-security)
- A [Copilot plan that includes coding agent access](https://docs.github.com/enterprise-cloud@latest/copilot/concepts/agents/about-third-party-agents)

Availability: github.com.

## Try it now

Open a Dependabot alert in your repository and select **Assign to Agent**.

Related docs:

- [Dependabot security updates](https://docs.github.com/code-security/dependabot/dependabot-security-updates/about-dependabot-security-updates)
- [Managing Dependabot alerts](https://docs.github.com/code-security/dependabot/dependabot-alerts/viewing-and-updating-dependabot-alerts)

Discussion:

- GitHub Community announcements: https://github.com/orgs/community/discussions/categories/announcements


[Read the entire article](https://github.blog/changelog/2026-04-07-dependabot-alerts-are-now-assignable-to-ai-agents-for-remediation)

