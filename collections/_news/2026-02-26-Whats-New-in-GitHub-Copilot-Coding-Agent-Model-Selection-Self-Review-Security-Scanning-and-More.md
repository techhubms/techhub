---
external_url: https://github.blog/ai-and-ml/github-copilot/whats-new-with-github-copilot-coding-agent/
title: 'What’s New in GitHub Copilot Coding Agent: Model Selection, Self-Review, Security Scanning, and More'
author: Andrea Griffiths
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-02-26 20:47:02 +00:00
tags:
- AI
- AI & ML
- AI Development
- Automation
- Branch Management
- CLI Handoff
- Code Generation
- Code Review
- Copilot Coding Agent
- Copilot Pro
- Copilot Pro+
- Custom Agents
- Dev Tools
- Developer Productivity
- GitHub
- GitHub Checkout
- GitHub Copilot
- GitHub Copilot Coding Agent
- Model Picker
- News
- Pull Requests
- Security Scanning
- Self Review
- .NET
section_names:
- ai
- dotnet
- github-copilot
---
Andrea Griffiths presents the latest features of GitHub Copilot coding agent, explaining how model selection, self-review, security scanning, and custom agents streamline coding and enhance developer experience.<!--excerpt_end-->

# What’s New in GitHub Copilot Coding Agent: Model Selection, Self-Review, Security Scanning, and More

**Author:** Andrea Griffiths

The GitHub Copilot coding agent continues to evolve, promising to further automate and improve how developers handle coding tasks, reviews, and integrations. Below are the latest updates and features—plus practical advice on how to leverage them.

## Model Picker for Task-Specific Control

- **Feature:** The coding agent now allows you to select which AI model should handle each coding task, giving the ability to opt for faster models for simple changes, or more robust models for complex work.
- **Usage:** Access via the Agents panel in GitHub. Select your repository, choose a model, write your prompt, and kick off the task. 'Auto' mode lets GitHub pick for you.
- **Availability:** Available for Copilot Pro and Pro+ users, with support for Business and Enterprise coming.
- [Learn more](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/changing-the-ai-model)

## Self-Review with Copilot Code Review

- **Feature:** Before opening pull requests, Copilot coding agent uses Copilot code review to check and iterate on its own changes, addressing quality concerns that would normally fall to the human reviewer.
- **Usage:** Assign issues or tasks to the agent via the panel, track progress in the logs, and review pull requests that have already been pre-reviewed by the agent.
- [Learn more](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/request-a-code-review/use-code-review)

## Integrated Security Scanning

- **Feature:** The agent now runs code scanning, secrets scanning, and dependency vulnerability checks on generated code as part of its workflow—no separate setup required.
- **Benefit:** Major vulnerabilities are caught before the pull request stage.
- [Learn more](https://github.blog/changelog/2025-10-28-copilot-coding-agent-now-automatically-validates-code-security-and-quality/)

## Custom Agents for Team-Specific Workflows

- **Feature:** Teams can create agent definitions under `.github/agents/` to enforce process or domain-specific benchmarks (e.g., performance optimization, testing flows).
- **Usage:** Define a custom agent in your repo, select it from the Agents panel, and prompt for the desired scope.
- **Sharing:** Custom agents can be used across an entire organization.
- [Learn more](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/create-custom-agents?utm_source=chatgpt.com)

## Seamless CLI Handoff

- **Feature:** Easily move work between GitHub cloud sessions and your local terminal/CLI without losing progress or context.
- **How:** Start a session in GitHub, continue in Copilot CLI, or push work back to the cloud as needed. All state and logs are preserved.
- [Learn more](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/manage-agents)

## What’s Next?

The post hints at upcoming features like private mode, auto-planning before coding, and broader non-code uses of the agent (such as summarizing issues and generating reports).

---

### Additional Resources

- [GitHub Copilot coding agent overview](https://docs.github.com/en/copilot/concepts/agents/coding-agent/about-coding-agent)
- [GitHub Community discussions](https://github.com/orgs/community/discussions/categories/copilot-conversations)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/github-copilot/whats-new-with-github-copilot-coding-agent/)
