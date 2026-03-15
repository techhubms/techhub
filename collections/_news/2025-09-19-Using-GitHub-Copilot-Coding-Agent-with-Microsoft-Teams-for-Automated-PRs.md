---
external_url: https://github.blog/changelog/2025-09-19-work-with-copilot-coding-agent-in-microsoft-teams
title: Using GitHub Copilot Coding Agent with Microsoft Teams for Automated PRs
author: Allison
feed_name: The GitHub Blog
date: 2025-09-19 14:07:53 +00:00
tags:
- Async Development
- Automation
- Copilot
- Copilot Coding Agent
- Developer Workflow
- GitHub App
- GitHub Business
- GitHub Enterprise
- Microsoft Teams
- Productivity Tools
- Pull Requests
- Repository Management
- Team Collaboration
- VS Code
- AI
- GitHub Copilot
- News
- .NET
section_names:
- ai
- dotnet
- github-copilot
primary_section: github-copilot
---
Allison outlines how developers can use the GitHub Copilot coding agent within Microsoft Teams to automate code tasks such as pull request generation, enhancing collaborative workflows.<!--excerpt_end-->

# Using GitHub Copilot Coding Agent with Microsoft Teams for Automated Pull Requests

With the new [GitHub app](https://appsource.microsoft.com/en-us/product/office/WA200009189?tab=Overview) for Microsoft Teams, developers can utilize the [Copilot coding agent](https://docs.github.com/en/copilot/concepts/agents/coding-agent/about-coding-agent) to generate pull requests (PRs) directly from a Teams conversation. This enables asynchronous and autonomous code generation, helping teams offload routine coding tasks.

## How It Works

- **Mention `@GitHub` in Teams:** Start a conversation in Teams and trigger the Copilot coding agent by mentioning `@GitHub` with a coding prompt—e.g., "Fix this issue with database query timeouts." The agent will begin working on your request and keep you updated through the Teams thread.
- **Supported Tasks:** Suitable for bug fixes, minor features, refactoring, adding logging, and scaffolding—freeing up time for deeper problem-solving and collaboration.
- **Workflow Integration:** Once the pull request is prepared, you can review and request changes directly in the Teams thread, promoting seamless collaboration within your development pipeline.

## Getting Started

1. **Enable Copilot Coding Agent:** Ensure the agent is enabled for your GitHub account. It's available for all paid Copilot plans. Business and Enterprise users may require an admin to [enable relevant policies](https://docs.github.com/en/enterprise-cloud@latest/copilot/concepts/agents/coding-agent/coding-agent-for-business-and-copilot-enterprise#copilot-coding-agent-policies-for-copilot-business-and-copilot-enterprise).
2. **Install the GitHub App in Teams:** Add the [GitHub app](https://teams.microsoft.com/l/app/836ecc9e-6dca-4696-a2e9-15e252cd3f31) to your Microsoft Teams workspace.
3. **Link Your GitHub Account:** Connect your GitHub account to Teams following the app's prompts.
4. **Choose a Repository:** Either set a default repository or specify a target repository using the syntax `repo=owner/repository-name` when starting a task.
5. **Trigger the Agent:** Mention `@GitHub` in a Teams thread with your prompt, and follow progress and discussions within the same conversation.

## Additional Notes

- The legacy GitHub for Teams app has been renamed [GitHub Notifications](https://appsource.microsoft.com/product/office/wa200002077?tab=overview), focusing solely on notifications.
- The new GitHub app for Teams is currently in public preview, and more details can be found in [GitHub's documentation](https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/integrate-coding-agent-with-teams).
- Join the conversation or share feedback via the [GitHub Community discussions](https://github.com/orgs/community/discussions/categories/copilot-conversations).

## Benefits

- **Automation:** Offload repetitive tasks for increased productivity.
- **Collaboration:** Tight integration with Teams enhances team communication.
- **Flexibility:** Works with all paid Copilot plans and across different repositories.

---
For more setup details and best practices, visit the links above.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-19-work-with-copilot-coding-agent-in-microsoft-teams)
