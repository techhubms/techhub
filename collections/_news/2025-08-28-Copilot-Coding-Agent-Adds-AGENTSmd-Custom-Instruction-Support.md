---
external_url: https://github.blog/changelog/2025-08-28-copilot-coding-agent-now-supports-agents-md-custom-instructions
title: Copilot Coding Agent Adds AGENTS.md Custom Instruction Support
author: Allison
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-08-28 18:00:58 +00:00
tags:
- AGENTS.md
- AI Development
- Automation
- Coding Best Practices
- Copilot Coding Agent
- Custom Instructions
- Developer Tools
- GitHub
- Instructions File
- Project Configuration
- Workflow
section_names:
- ai
- github-copilot
---
Allison announces that GitHub Copilot coding agent now supports AGENTS.md custom instructions, making it easier for developers to control automated workflows and agent behavior.<!--excerpt_end-->

# Copilot Coding Agent Adds AGENTS.md Custom Instruction Support

GitHub Copilot's autonomous coding agent now allows developers to add custom instructions using an `AGENTS.md` file. This enhancement lets developers guide Copilot on how to interpret their projects, including specific steps for building, testing, and validating changes.

## Key Features

- **AGENTS.md Support**: Place an `AGENTS.md` file at the root of your repository to establish project-wide agent instructions.
- **Nested Instructions**: Nested `AGENTS.md` files within subdirectories can target specific components or services for more granular control.
- **Compatibility with Existing Formats**:
  - `.github/copilot-instructions.md`
  - `.github/instructions/**.instructions.md`
  - `CLAUDE.md`
  - `GEMINI.md`

Developers can choose the most suitable format for their project structure. The Copilot agent reads these files to understand context, build and test steps, and validation requirements when working in the background.

## Benefits

- Improved ability to tailor Copilot's actions to complex project environments
- More control over automated workflows and code quality validation
- Seamless integration with existing Copilot and GitHub workflows

## Resources

To learn more about setting up custom instructions for Copilot coding agent, see the [official documentation](https://docs.github.com/enterprise-cloud@latest/copilot/tutorials/coding-agent/get-the-best-results).

## Related Links

- [Product Announcement](https://github.blog/changelog/2025-08-28-copilot-coding-agent-now-supports-agents-md-custom-instructions)
- [Copilot Coding Agent Overview](https://github.blog/news-insights/product-news/github-copilot-meet-the-new-coding-agent/)

These updates enable developers to harness more of Copilot’s capabilities by explicitly stating how their projects should be understood and automated by the agent.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-28-copilot-coding-agent-now-supports-agents-md-custom-instructions)
