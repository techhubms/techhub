---
layout: "post"
title: "Claude and Codex Now Available as Coding Agents for Copilot Business and Pro Users"
description: "This post details the rollout of Claude by Anthropic and OpenAI Codex as new coding agents for GitHub Copilot Business and Pro users. It explains how these AI-powered agents integrate into the GitHub platform, enabling code review, issue triage, and more directly within developer workflows across GitHub web, mobile, and VS Code. It also covers activation steps, usage scenarios, and requirements for both enterprise and organization administrators."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-26-claude-and-codex-now-available-for-copilot-business-pro-users"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-26 17:18:39 +00:00
permalink: "/2026-02-26-Claude-and-Codex-Now-Available-as-Coding-Agents-for-Copilot-Business-and-Pro-Users.html"
categories: ["AI", "DevOps", "GitHub Copilot"]
tags: ["Agent Control Plane", "AI", "AI Agents", "Claude", "Code Review", "Coding Agents", "Copilot", "Copilot Business", "Copilot Pro", "DevOps", "Enterprise Security", "GitHub Copilot", "GitHub Mobile", "News", "OpenAI Codex", "Pull Requests", "Repository Management", "VS Code", "Workflow Automation"]
tags_normalized: ["agent control plane", "ai", "ai agents", "claude", "code review", "coding agents", "copilot", "copilot business", "copilot pro", "devops", "enterprise security", "github copilot", "github mobile", "news", "openai codex", "pull requests", "repository management", "vs code", "workflow automation"]
---

Allison introduces the availability of Claude and Codex as coding agents for Copilot Business and Pro users, outlining activation procedures and usage across GitHub platforms.<!--excerpt_end-->

# Claude and Codex Now Available as Coding Agents for Copilot Business and Pro Users

GitHub has expanded the availability of Claude by Anthropic and OpenAI Codex as coding agents for Copilot Business and Copilot Pro customers. Previously limited to Copilot Enterprise and Pro+ users, these agents are now accessible to a wider range of teams.

## What’s New?

- **Claude and Codex** can now be used as coding agents alongside Copilot within GitHub.
- Available on **github.com**, **GitHub Mobile**, and **VS Code** (v1.109 or later).
- Agents can assist with code review, issue triage, and editor tasks, all while benefiting from shared context and history.

## Features and Benefits

- Work natively within GitHub workflows across multiple platforms.
- No extra subscription required; access is included in Copilot Business and Copilot Pro plans.
- Agents share a platform with unified governance, policy management, and centralized audit logging via the Agent Control Plane.
- Each session consumes one premium request during public preview.
- Agents can access repository code, issues, pull requests, and Copilot Memory.

## Getting Started / Enablement

### For Copilot Pro Users

1. Open **Copilot coding agent settings**.
2. Select repositories for agent access.
3. Toggle Claude and/or Codex on.

### For Copilot Business Users

- **Enterprise Level**
  - Go to **Enterprise AI Controls** → **Agents**.
  - Enable Claude and/or Codex under “Partner Agents.”
- **Organization Level**
  - Go to **Settings** → **Copilot** → **Coding agent**.
  - Enable partner agents as needed.

## How To Use

- Use the **Agents** tab in enabled repositories or from the GitHub Mobile “Agents” view.
- Assign agents to issues and pull requests, or mention them (`@copilot`, `@claude`, `@codex`) in PR comments for updates.
- In VS Code, start sessions from the Agent menu, choosing local, cloud, or background execution modes.

All agent outputs are provided as draft, reviewable artifacts within your pull request workflow.

## Resources

- [Enable Claude and Codex today](https://github.com/settings/copilot/coding_agent?utm_source=changelog-3p-agents-cca-settings-cta)

This release makes advanced AI-powered coding assistants widely available, further integrating AI capabilities into everyday development workflows in GitHub.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-26-claude-and-codex-now-available-for-copilot-business-pro-users)
