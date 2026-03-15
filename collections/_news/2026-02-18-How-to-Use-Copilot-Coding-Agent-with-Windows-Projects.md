---
external_url: https://github.blog/changelog/2026-02-18-use-copilot-coding-agent-with-windows-projects
title: How to Use Copilot Coding Agent with Windows Projects
author: Allison
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-02-18 22:04:40 +00:00
tags:
- AI
- CI/CD
- Copilot
- Copilot Coding Agent
- Copilot Setup Steps.yml
- Development Environment
- DevOps
- GitHub Actions
- GitHub Copilot
- Improvement
- Network Security
- News
- Pull Requests
- Self Hosted Runners
- Windows Development
- Workflow Automation
- .NET
section_names:
- ai
- dotnet
- devops
- github-copilot
---
Allison explains how to set up the Copilot coding agent for Windows-based projects—helpful for developers who want Copilot to build and test code autonomously with GitHub Actions integration.<!--excerpt_end-->

# Use Copilot Coding Agent with Windows Projects

Copilot coding agent is an asynchronous, autonomous background agent that lets you delegate tasks for it to handle in its own development environment. Powered by GitHub Actions, Copilot can help automate building, testing, and linting, so you get green pull requests with less manual intervention.

## Default Environment

- By default, Copilot coding agent runs in a Linux environment.

## Supporting Windows Projects

- Projects targeting Windows can now be configured to use a Windows environment for Copilot coding agent—still powered by GitHub Actions.

## How to Configure

1. **Create a `copilot-setup-steps.yml` file** in your repository.
2. **Set the `runs-on` parameter** to specify Windows as the environment.
3. This changes the Copilot coding agent's environment to Windows, so your build and test steps will run natively.

## Example YML Snippet

```yml
runs-on: windows-latest
```

## Security & Networking Notes

- Copilot coding agent’s integrated firewall does *not* work on Windows.
- For security, it's recommended to use Windows with self-hosted runners or larger runners configured for Azure private networking.
- Implement your own network controls to ensure corporate or project security requirements are met.

## Additional Resources

- [Official announcement](https://github.blog/changelog/2026-02-18-use-copilot-coding-agent-with-windows-projects)
- [Copilot coding agent documentation](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent)
- [Customizing the agent environment](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/customize-the-agent-environment#switching-copilot-to-a-windows-development-environment)
- [Firewall customization for Copilot agent](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/customize-the-agent-firewall)

---
This update makes GitHub Copilot more flexible for teams using Windows-based development workflows, enabling more robust automation and integration across various environments.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-18-use-copilot-coding-agent-with-windows-projects)
