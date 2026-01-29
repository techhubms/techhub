---
external_url: https://github.blog/changelog/2025-07-30-copilot-coding-agent-custom-setup-steps-are-more-reliable-and-easier-to-debug
title: 'Copilot Coding Agent: Enhanced Reliability and Debugging for Custom Setup Steps'
author: Allison
feed_name: The GitHub Blog
date: 2025-07-30 18:13:18 +00:00
tags:
- Agent Session Logs
- Continuous Integration
- Copilot Coding Agent
- Custom Setup
- Debugging
- Development Tools
- GitHub Actions
- Pro/Enterprise Features
- Workflow Automation
- AI
- DevOps
- GitHub Copilot
- News
section_names:
- ai
- devops
- github-copilot
primary_section: github-copilot
---
Allison introduces recent improvements to the GitHub Copilot coding agent, detailing enhanced reliability and easier debugging for custom workflow setup steps. These changes streamline automation for developers using Copilot Pro, Business, or Enterprise.<!--excerpt_end-->

## Copilot Coding Agent: Custom Setup Steps Are More Reliable and Easier to Debug

**Author:** Allison  
**Source:** [GitHub Blog](https://github.blog)

---

You can now delegate tasks to the [GitHub Copilot coding agent](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/agents/copilot-coding-agent), allowing it to work on tasks in the background. While executing tasks, Copilot accesses its own dedicated development environment, which is powered by GitHub Actions. Within this environment, it can:

- Compile code
- Execute automated tests
- Run linters

### Customizing Copilot's Development Environment

To tailor Copilot’s development context, you can add a `.github/workflows/copilot-setup-steps.yml` file to your repository. This file enables you to pre-install tools and dependencies by defining specific setup steps.

### Key Updates to Custom Setup Steps

GitHub has rolled out two important improvements:

1. **Progress Visibility**: You can now monitor the progress of custom setup steps directly within the agent session logs. This eliminates the need to navigate to verbose GitHub Actions logs, making debugging and oversight more efficient.

2. **Resilience to Failures**: If a custom setup step fails, Copilot will continue to work on the assigned task regardless. This ensures that your workflow isn’t blocked by configuration issues, allowing teams to remain productive even if there is a problem with the custom setup.

---

### Availability: Who Can Use Copilot Coding Agent?

Copilot coding agent is available in public preview for the following users:

- All users with Copilot Pro or Copilot Pro+
- Copilot Business and Copilot Enterprise users, provided an administrator has [enabled the relevant policy](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/agents/copilot-coding-agent/enabling-copilot-coding-agent)

It is integrated with:

- github.com
- Multiple IDEs
- GitHub Mobile
- GitHub CLI
- GitHub MCP Server

This broad integration allows users to delegate coding tasks from virtually anywhere.

### Learn More

For more detailed information about configuring and using Copilot coding agent, visit [the official documentation](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/agents/copilot-coding-agent).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-07-30-copilot-coding-agent-custom-setup-steps-are-more-reliable-and-easier-to-debug)
