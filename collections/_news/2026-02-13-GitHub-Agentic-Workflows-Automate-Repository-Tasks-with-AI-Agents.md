---
external_url: https://github.blog/changelog/2026-02-13-github-agentic-workflows-are-now-in-technical-preview
title: 'GitHub Agentic Workflows: Automate Repository Tasks with AI Agents'
author: Allison
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-02-13 14:00:26 +00:00
tags:
- Actions
- AI
- AI Automation
- CI Failure Analysis
- Collaboration Tools
- Copilot
- Developer Tools
- DevOps
- GitHub Actions
- GitHub Agentic Workflows
- GitHub Copilot
- GitHub Copilot CLI
- Issue Triage
- Markdown Automation
- MCP Server
- Natural Language Workflows
- News
- Open Source
- Projects & Issues
- Pull Request Review
- Repository Automation
- Technical Preview
- VS Code Integration
- Workflow Security
section_names:
- ai
- devops
- github-copilot
---
Allison unveils GitHub Agentic Workflows, an AI-driven automation feature for GitHub repositories. Discover how natural language and Copilot CLI empower developers to automate repo management and CI tasks with security and deep GitHub integration.<!--excerpt_end-->

# GitHub Agentic Workflows: Technical Preview Announcement

GitHub has introduced **Agentic Workflows**, enabling repository automation using AI-powered agents directly within GitHub Actions. This new feature empowers developers to define automation tasks using plain Markdown and natural language—removing the need to author complex YAML workflows.

## Key Capabilities

- **Write Workflows in Markdown**: Define automation goals in `.github/workflows/` using clear, natural language. The system interprets your intent and generates operational workflows.
- **AI-Powered Orchestration**: Leverage AI coding agents—such as GitHub Copilot CLI—to perform advanced automation for:
  - Issue triage
  - Pull request reviews
  - Continuous Integration (CI) failure analysis
  - Repository maintenance
- **Security-First Architecture**: Agentic workflows run with read-only permissions by default, operate in sandboxed environments, enforce network isolation, and only permit write operations through pre-approved, "safe outputs."
- **Flexible Execution**: Automation can be triggered by repository events, schedules, manual dispatch, or command triggers in comments.
- **Multiple Agent Integrations**: Agents like GitHub Copilot CLI are supported out of the box. The system is open to other AI coding agents.
- **MCP Server and Tooling**: Deep GitHub platform integration provides automated access to issues, pull requests, repository content, and security operations via the GitHub MCP server. Additional support exists for browser automation, web search, and custom MCPs.
- **Agentic Authoring Experience**: Create, edit, and debug workflows in VS Code, directly on GitHub.com, or alongside other coding agents.
- **Open Source and Community-Driven**: The GitHub Agentic Workflows implementation is fully open source (MIT license) and welcomes community contributions. Explore curated examples at [Peli’s Agent Factory](https://github.github.com/gh-aw/blog/2026-01-12-welcome-to-pelis-agent-factory/), featuring over 50 real-world workflow templates.

## Getting Started

1. **Install the CLI Extension**: Set up the `gh aw` CLI extension.
2. **Author a Markdown Workflow**: Place a plain-text Markdown file in `.github/workflows/` describing your automation goal(s) in natural language.
3. **Compile and Commit**: Use the CLI to convert the Markdown into a GitHub Actions workflow powered by AI agents. Commit to your repository.
4. **Run and Monitor**: Workflows execute as standard GitHub Actions, responding to triggers and automating repository processes.

For full documentation and examples, visit the [GitHub Agentic Workflows documentation](https://github.github.com/gh-aw/). Connect with the community, share your feedback, and explore the open source implementation in the [`gh-aw` repository](https://github.com/github/gh-aw).

## Example Use Cases

- Automatically triage incoming issues and assign labels.
- Investigate CI build/test failures and suggest remediations.
- Maintain up-to-date documentation or improve test coverage.
- Monitor compliance rules and team engagement activities.

## Collaboration and Acknowledgments

This project is a partnership between GitHub Next, Microsoft Research, and the Azure Core Upstream team, reflecting GitHub and Microsoft’s broader commitment to advancing AI-powered developer tools.

## Further Reading

- [Agentic Workflows: Technical Preview Blog](https://github.blog/ai-and-ml/automate-repository-tasks-with-github-agentic-workflows/)
- [Peli’s Agent Factory Examples](https://github.github.com/gh-aw/blog/2026-01-12-welcome-to-pelis-agent-factory/)
- [GitHub Agentic Workflows Documentation](https://github.github.com/gh-aw/)

---

Agentic Workflows bring accessible, AI-driven automation to the GitHub ecosystem. This technical preview lets developers move faster, automate best practices, and ensure more robust repository management—all while benefiting from deep integration with GitHub Copilot and security-first design.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-13-github-agentic-workflows-are-now-in-technical-preview)
