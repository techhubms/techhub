---
external_url: https://github.blog/changelog/2025-11-05-copilot-coding-agent-now-supports-pull-request-templates
title: Copilot Coding Agent Now Supports Pull Request Templates
author: Allison
feed_name: The GitHub Blog
date: 2025-11-05 18:18:54 +00:00
tags:
- Automation
- Autonomous Agents
- Best Practices
- Code Collaboration
- Copilot
- Copilot Coding Agent
- GitHub
- Improvement
- Pull Request Templates
- Repository Management
section_names:
- ai
- devops
- github-copilot
primary_section: github-copilot
---
Allison shares news on the Copilot coding agent's new ability to follow repository pull request templates, streamlining automated code contributions on GitHub.<!--excerpt_end-->

# Copilot Coding Agent Now Supports Pull Request Templates

GitHub's Copilot coding agent has introduced a new feature that enhances automated development workflows. The coding agent, designed as an asynchronous and autonomous assistant, now updates its pull request bodies according to the repository's pull request templates.

## Key Update

- **Template Integration**: When Copilot coding agent finishes its work, it now follows the repository’s existing pull request templates when summarizing and submitting changes.
- **Multiple Templates**: If your repository contains multiple pull request templates (`pull_request_template.md` files), Copilot will automatically select the one that best fits the context of the pull request.

## How to Enable

- Add a `pull_request_template.md` file to your GitHub repository.
- For multiple templates, Copilot will decide which template is the most relevant for each task based on its evaluation.

## Additional Resources

- [Creating a pull request template](https://docs.github.com/communities/using-templates-to-encourage-useful-issues-and-pull-requests/creating-a-pull-request-template-for-your-repository)
- [Copilot coding agent documentation](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent)

This improvement helps standardize change summaries and streamlines collaboration for automated contributions. Review best practices in the official documentation to maximize the effectiveness of Copilot in your development workflow.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-05-copilot-coding-agent-now-supports-pull-request-templates)
