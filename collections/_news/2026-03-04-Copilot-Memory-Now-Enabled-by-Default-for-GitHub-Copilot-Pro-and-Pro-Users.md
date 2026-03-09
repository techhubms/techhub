---
external_url: https://github.blog/changelog/2026-03-04-copilot-memory-now-on-by-default-for-pro-and-pro-users-in-public-preview
title: Copilot Memory Now Enabled by Default for GitHub Copilot Pro and Pro+ Users
author: Allison
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-03-04 10:07:12 +00:00
tags:
- Agentic Memory
- AI
- AI Development
- Automation
- Code Review
- Codebase Understanding
- Coding Agent
- Copilot
- Copilot CLI
- Copilot Memory
- Developer Tools
- Feature Update
- GitHub Blog
- GitHub Copilot
- Improvement
- News
- Persistent Memory
- Pro
- Pro Plus
- Productivity
- Repository Context
- User Settings
section_names:
- ai
- github-copilot
---
Allison shares updates to GitHub Copilot: Copilot Memory is now enabled by default for all Copilot Pro and Pro+ users. This enhancement gives Copilot agents persistent, repository-level awareness, streamlining coding and review workflows.<!--excerpt_end-->

# Copilot Memory Now Enabled by Default for GitHub Copilot Pro and Pro+ Users

Copilot Memory, which was previously available as an opt-in public preview, is now enabled by default for all users of GitHub Copilot Pro and Copilot Pro+.

---

## What is Copilot Memory?

Copilot Memory allows GitHub Copilot agents to build and retain a persistent, repository-level understanding of your codebase. This reduces the need for reexplaining project context and helps developers focus on writing and shipping code faster.

### Key Capabilities

- **Repository Context**: Copilot Memory discovers and stores knowledge about each repository, such as:
  - Coding conventions
  - Architectural patterns
  - Critical cross-file dependencies
- **Scoped and Secure**: Memories are strictly scoped to individual repositories and are validated against the current code before use. This ensures that outdated or inaccurate information is not applied.
- **Expiration**: Stored memories automatically expire after 28 days, ensuring freshness.

### Integration Points

- **Copilot coding agent**: Applies repository insights during task completion and pull request creation.
- **Copilot code review**: Utilizes stored patterns to offer targeted, accurate review feedback.
- **Copilot CLI**: Brings repository context to your terminal workflows.

Memories shared between these agents allow improvements in one area (such as code review) to enhance others (such as active coding).

---

## What’s Changing

- Copilot Memory is **now enabled by default** for all Copilot Pro and Copilot Pro+ individual users.
- No setup is needed—users automatically benefit from improved context awareness immediately.
- Users can opt out anytime via their [Copilot settings](https://github.com/settings/copilot) under *Features > Copilot Memory*.
- Enterprise and organization administrators retain control over memory features for members through Copilot policies.

---

## Managing Copilot Memories

- Repository owners can review and delete stored memories via *Repository Settings > Copilot > Memory*.
- For additional details and management guides, see:
  - [About agentic memory for GitHub Copilot](https://docs.github.com/copilot/concepts/agents/copilot-memory)
  - [Enabling and curating Copilot Memory](https://docs.github.com/copilot/how-tos/use-copilot-agents/copilot-memory)

---

## Further Engagement

- Join the discussion in the [GitHub Community announcements](https://github.com/orgs/community/discussions/categories/announcements).

---

*Shared by Allison via the [GitHub Blog](https://github.blog/changelog/2026-03-04-copilot-memory-now-on-by-default-for-pro-and-pro-users-in-public-preview).*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-04-copilot-memory-now-on-by-default-for-pro-and-pro-users-in-public-preview)
