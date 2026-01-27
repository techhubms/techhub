---
external_url: https://github.blog/changelog/2025-10-03-github-copilot-cli-enhanced-model-selection-image-support-and-streamlined-ui
title: 'GitHub Copilot CLI: Enhanced Model Selection, Image Recognition, and Streamlined UI Improvements'
author: Dylan Birtolo
feed_name: The GitHub Blog
date: 2025-10-03 16:23:18 +00:00
tags:
- AI Coding Assistant
- Anthropic Claude Sonnet
- Client Apps
- Command Line Interface
- Copilot
- Developer Tools
- Enterprise Authentication
- GitHub Copilot CLI
- Image Recognition
- Improvement
- Markdown Rendering
- Model Selection
- npm
- PowerShell
- Security Permissions
- Session Management
- Shell Commands
- Usage Statistics
section_names:
- ai
- coding
- github-copilot
primary_section: github-copilot
---
Dylan Birtolo announces significant updates to GitHub Copilot CLI, including model selection improvements, image support, and enhanced UI for developers.<!--excerpt_end-->

# GitHub Copilot CLI: Enhanced Model Selection, Image Recognition, and Streamlined UI Improvements

**Author:** Dylan Birtolo

GitHub Copilot CLI has just received a series of significant updates since its public preview launch, enhancing both developer productivity and usability. Below are the key highlights from this release:

## Anthropic Claude Sonnet 4.5 Support

- The CLI now supports Anthropic's Claude Sonnet 4.5 AI model, bringing advanced coding and real-world agent capabilities.
- This model is available for Copilot Pro, Pro+, Business, and Enterprise users during the public preview phase.

## Flexible Model Selection

- Developers can now choose which AI model powers their CLI session with a new inline `/model` command.
- The currently selected model is prominently shown in the interface for workflow transparency.

## Image Recognition Input

- By using the `@` symbol with images, users can directly include images as model input.

## Shell Command Execution

- Prepending input with `!` executes the command directly in the shell without invoking the AI model.
- Improved path extraction and PowerShell parsing for accurate permission prompts and safer execution.

## Improved UI and Navigation

- Smarter input handling limits and scrolls long input to maintain a clean workspace.
- Enhanced file operation timeline and better navigation through scrollbars and session pickers.
- Streamlined layout with removed extra UI borders and more readable Markdown by excluding `#` prefixes.

## Robust Permission Controls

- Fine-grained permissions: use glob patterns in `--allow-tool` and `--deny-tool` to specify command restrictions.
- Developers gain precise control integrating Copilot CLI securely into build, test, or deployment workflows.

## Rich Usage Statistics & Context Awareness

- `/usage` command provides session usage metrics: premium requests, session duration, code edits, and token consumption per model.
- Early truncation warnings when nearing context/token limits.

## Better Enterprise and Authentication Support

- Per-subscription API endpoints now comply with GitHub network access management best practices.
- Improved error feedback when organizational policies or permissions (like missing Copilot Requests) block the CLI.
- User listing commands are now robust across all authentication modes.

## Getting Started

- To upgrade, run `npm install -g @github/copilot@latest`.
- Feedback is welcome via `/feedback` in the CLI or GitHub Issues.
- Try Copilot CLI and explore its documentation for deeper integration.

### Resources

- [GitHub Copilot CLI Public Repository](https://github.com/github/copilot-cli)
- [GitHub Copilot CLI Documentation](https://docs.github.com/copilot/concepts/agents/about-copilot-cli)

These updates further streamline the developer experience and strengthen security and permission controls for teams relying on Copilot CLI in their workflows.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-03-github-copilot-cli-enhanced-model-selection-image-support-and-streamlined-ui)
