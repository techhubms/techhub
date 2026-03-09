---
external_url: https://github.blog/changelog/2026-03-05-pick-a-model-for-copilot-in-pull-request-comments
title: Pick a Model for @copilot in Pull Request Comments
author: Allison
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-03-05 20:44:19 +00:00
tags:
- '@copilot'
- AI
- AI Models
- Automation
- Continuous Integration
- Copilot
- Development Tools
- GitHub Actions
- GitHub Copilot
- Improvement
- Model Selection
- News
- PR Comments
- Pull Requests
section_names:
- ai
- github-copilot
---
Allison explains how developers can now pick a model for GitHub Copilot in pull request comments, allowing flexible AI assistance directly from the PR workflow.<!--excerpt_end-->

# Pick a Model for @copilot in Pull Request Comments

Developers can now leverage enhanced flexibility when using the GitHub Copilot coding agent in pull request (PR) workflows. By mentioning `@copilot` in any PR comment (whether the PR was created by a human or by Copilot itself), a model picker appears in the bottom-left corner of the comment box. This interface lets users select from a set of supported AI models, tailoring Copilot’s suggestions and actions to the needs of the codebase or the specific change being discussed.

### How It Works

- Mention `@copilot` in your PR comment on github.com.
- A model picker appears, enabling you to choose among available Copilot-supported models (see the [supported models documentation](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/changing-the-ai-model#supported-models)).
- Copilot, running in a dedicated development environment powered by GitHub Actions, can then make code changes, build the project, and execute tests based on the chosen model’s capabilities.

### Key Notes

- This functionality is currently limited to comments within pull requests (not review comments) on github.com.
- Expansion to GitHub Mobile and additional comment types is planned in the future.

### Why This Matters

Selecting different AI models enables developers to fine-tune Copilot’s responses for various scenarios, improving code review efficiency and workflow integration.

For more details, see the [official Copilot coding agent documentation](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-05-pick-a-model-for-copilot-in-pull-request-comments)
