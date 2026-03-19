---
section_names:
- ai
- github-copilot
feed_name: The GitHub Blog
primary_section: github-copilot
date: 2026-03-19 22:26:33 +00:00
author: Allison
external_url: https://github.blog/changelog/2026-03-19-more-visibility-into-copilot-coding-agent-sessions
tags:
- Agent Firewall
- Agent Sessions
- AI
- Copilot
- Copilot Coding Agent
- Copilot Setup Steps.yml
- Custom Setup Steps
- Debugging
- Environment Configuration
- GitHub Actions
- GitHub Copilot
- Improvement
- News
- Repository Cloning
- Session Logs
- Subagents
title: More visibility into Copilot coding agent sessions
---

Allison explains updates to GitHub Copilot coding agent session logs, making it easier for developers to see setup progress, understand custom environment steps, and track subagent activity while the agent works in the background.<!--excerpt_end-->

# More visibility into Copilot coding agent sessions

When you delegate a task to [Copilot coding agent](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent), it works in the background and then requests your review. You can view the agent session logs to understand what Copilot did while working on your task.

## What’s improved in session logs

GitHub has shipped several improvements to Copilot coding agent’s session logs to provide clearer visibility into what the agent is doing.

### Better visibility of built-in setup steps

Before Copilot starts working on your task, it prepares the environment by:

- Cloning your repository
- Starting the [agent firewall](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/use-copilot-agents/coding-agent/customize-the-agent-firewall) (if enabled)

The session logs now show updates when these built-in steps start and finish.

![Screenshot of logs showing Copilot coding agent's built-in setup steps](https://github.com/user-attachments/assets/aa52adf0-16a2-433b-8f0d-97b366e5885a)

### Better visibility of your custom setup steps

You can customize Copilot’s development environment using a `copilot-setup-steps.yml` file in your repository. If you’ve defined setup steps for Copilot to run:

- The output from those steps now appears in the session logs.
- This helps you verify the environment is configured correctly and debug issues without switching to the more verbose GitHub Actions logs.

Related documentation: [customize Copilot’s development environment](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/use-copilot-agents/coding-agent/customize-the-agent-environment)

![Screenshot of logs showing custom setup step for Copilot coding agent](https://github.com/user-attachments/assets/719af188-a616-4bc2-9dde-479d0071ae0a)

### Better visibility when Copilot delegates work to subagents

Copilot can delegate work to subagents—for example, spinning up a subagent to research and understand the current state of your code before making changes.

When Copilot delegates to a subagent:

- The subagent’s activity is now collapsed by default.
- A heads-up display shows what the subagent is working on right now.
- You can expand the details at any time to see full output.

![Screen recording showing visibility of subagents in Copilot coding agent's session logs](https://github.com/user-attachments/assets/23db5540-d1c4-4cd5-80f6-f23df7f0e7b6)

## Learn more

- Documentation: [Copilot coding agent](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent)


[Read the entire article](https://github.blog/changelog/2026-03-19-more-visibility-into-copilot-coding-agent-sessions)

