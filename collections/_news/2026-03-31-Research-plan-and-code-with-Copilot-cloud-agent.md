---
author: Allison
date: 2026-03-31 18:36:08 +00:00
primary_section: github-copilot
feed_name: The GitHub Blog
title: Research, plan, and code with Copilot cloud agent
external_url: https://github.blog/changelog/2026-03-31-research-plan-and-code-with-copilot-cloud-agent
section_names:
- ai
- github-copilot
tags:
- Agentic Coding
- Agents Tab
- AI
- Branch Workflows
- Copilot
- Copilot Business
- Copilot Chat
- Copilot Cloud Agent
- Copilot Coding Agent
- Copilot Enterprise
- Deep Research
- Diff Review
- Enterprise Enablement
- GitHub Community Discussion
- GitHub Copilot
- Implementation Plan Generation
- News
- Pull Requests
- Repository Context
---

Allison announces updates to GitHub Copilot cloud agent, covering branch-based coding without automatic pull requests, plan-first implementation flows, and deep research sessions that answer questions using repository context.<!--excerpt_end-->

## Summary

Copilot cloud agent (formerly Copilot coding agent) now supports more workflows than just pull-request-based sessions. The update adds:

- Branch-based work without automatically opening a pull request
- Plan-first implementation (generate and approve an implementation plan before code is written)
- “Deep research” sessions for questions that require broader investigation across a repository

## More control over when you open a pull request

Previously, using Copilot cloud agent meant opening a pull request as part of the workflow. Now Copilot can work directly on a branch without creating a PR.

- Copilot generates code on a branch without opening a pull request.
- You can review the full diff before deciding to create a PR by clicking **Diff**.
- You can iterate with Copilot until you are ready for review.
- If you want a PR from the start, you can say so in your prompt and Copilot will create one when the session completes.

## Generate implementation plans

You can ask Copilot to produce an implementation plan and review the approach before any code is written.

- Ask for a plan in your prompt and Copilot generates one before taking action.
- Review the proposed approach and approve it or provide feedback.
- After approval, Copilot uses the plan to guide its implementation.

## Conduct deep research in your codebase

You can start a research session for questions that need thorough investigation and more comprehensive answers.

- Ask broad questions about your codebase and get answers grounded in your repository context.
- You can also start a deep research session from a Copilot chat conversation by asking Copilot a question.

## To get started

This functionality is available via agent entry points, including:

- The **Agents** tab in the repository: https://github.blog/changelog/2026-01-26-introducing-the-agents-tab-in-your-repository/
- Copilot Chat

Availability and admin requirements:

- Copilot cloud agent is available with all paid Copilot plans.
- For **Copilot Business** or **Copilot Enterprise**, an administrator must enable Copilot cloud agent:
  - https://docs.github.com/enterprise-cloud@latest/copilot/concepts/agents/coding-agent/coding-agent-for-business-and-enterprise

Discussion:

- GitHub Community thread: https://github.com/orgs/community/discussions/190573


[Read the entire article](https://github.blog/changelog/2026-03-31-research-plan-and-code-with-copilot-cloud-agent)

