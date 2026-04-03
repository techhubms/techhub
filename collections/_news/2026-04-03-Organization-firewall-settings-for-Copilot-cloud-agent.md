---
feed_name: The GitHub Blog
external_url: https://github.blog/changelog/2026-04-03-organization-firewall-settings-for-copilot-cloud-agent
primary_section: github-copilot
section_names:
- ai
- github-copilot
- security
tags:
- Admin Controls
- Agent Firewall
- AI
- Allowlist
- Coding Agent
- Copilot
- Copilot Cloud Agent
- Custom Allowlist
- Data Exfiltration
- GitHub Copilot
- GitHub Docs
- GitHub Organization
- Improvement
- News
- Organization Settings
- Prompt Injection
- Repository Settings
- Security
- Security Guardrails
author: Allison
date: 2026-04-03 14:12:27 +00:00
title: Organization firewall settings for Copilot cloud agent
---

Allison announces an update to GitHub Copilot cloud agent: organization admins can now manage the agent firewall settings across all repositories, making it easier to apply consistent security defaults and guardrails at scale.<!--excerpt_end-->

# Organization firewall settings for Copilot cloud agent

[Copilot cloud agent](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent) includes a built-in [agent firewall](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/customize-the-agent-firewall) that controls Copilot’s internet access to help protect against:

- Prompt injection
- Data exfiltration

Previously, the agent firewall could only be configured at the **repository level** by repository admins.

## What’s changed

**Organization admins** can now manage the agent firewall across **all repositories** in their organization. This is meant to make it easier to roll out Copilot cloud agent with consistent defaults and guardrails.

## Organization admin controls

Organization admins can now:

- Turn the firewall **on or off** across all repositories, or allow each repository to decide.
- Turn the **recommended allowlist** on or off across all repositories, or allow each repository to decide.
- Add entries to an **organization-wide custom allowlist** that applies to all repositories (for example, allowing access to an internal package registry).
- Control whether **repository admins** are allowed to add their own custom allowlist entries.

## Default behavior

By default, all settings continue to **allow each repository to decide**, preserving existing behavior.

## Reference

- GitHub Docs: [Customizing the agent firewall for Copilot cloud agent](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/customize-the-agent-firewall)


[Read the entire article](https://github.blog/changelog/2026-04-03-organization-firewall-settings-for-copilot-cloud-agent)

