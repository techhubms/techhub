---
layout: post
title: Copilot Coding Agent Now Remembers Context Within Pull Requests
author: Allison
canonical_url: https://github.blog/changelog/2025-09-30-copilot-coding-agent-remembers-context-within-the-same-pull-request
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-09-30 16:50:47 +00:00
permalink: /github-copilot/news/Copilot-Coding-Agent-Now-Remembers-Context-Within-Pull-Requests
tags:
- AI
- AI Automation
- Code Collaboration
- Code Review
- Copilot
- Copilot Business
- Copilot Coding Agent
- Copilot Enterprise
- Developer Tools
- GitHub
- GitHub Copilot
- Improvement
- News
- Pro+
- Pull Request
- Session Context
- Workflow Automation
section_names:
- ai
- github-copilot
---
Allison introduces a key enhancement to the GitHub Copilot coding agent: it now retains context between sessions in the same pull request, streamlining developer workflows and making automated coding support more efficient.<!--excerpt_end-->

# Copilot Coding Agent Now Remembers Context Within Pull Requests

The Copilot coding agent, GitHub’s asynchronous background developer agent, has been updated to remember context between sessions for the same pull request. Previously, Copilot would re-explore the repository and reread the same files with each session, but now it maintains awareness within a given pull request. This enhancement makes Copilot’s follow-up requests faster and more reliable, allowing developers to delegate iterative coding tasks without losing progress or incurring unnecessary processing.

## How Copilot Coding Agent Works

- **Delegate Tasks**: Developers can assign tasks to Copilot, which then opens a draft pull request and makes relevant changes in the background.
- **Iterative Feedback**: After requesting a review, developers can add comments and mention `@copilot` to initiate new sessions or additional changes.
- **Session Persistence**: The new update allows Copilot to remember the context of ongoing pull request sessions, streamlining subsequent requests.

## Availability and Administration

- The Copilot coding agent is available for subscribers to Copilot Pro, Copilot Pro+, Copilot Business, and Copilot Enterprise.
- For Copilot Business and Copilot Enterprise users, an administrator must enable the feature from the “Policies” page. [See activation guide](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/agents/copilot-coding-agent/enabling-copilot-coding-agent).

## Learn More

For more details about the Copilot coding agent’s capabilities and setup, refer to [GitHub’s official documentation](https://docs.github.com/enterprise-cloud@latest/copilot/concepts/agents/coding-agent/about-coding-agent).

This improvement is part of GitHub’s ongoing efforts to enhance AI-driven development tools for a more efficient and reliable coding experience.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-30-copilot-coding-agent-remembers-context-within-the-same-pull-request)
