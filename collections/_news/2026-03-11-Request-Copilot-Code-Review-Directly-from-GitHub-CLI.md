---
external_url: https://github.blog/changelog/2026-03-11-request-copilot-code-review-from-github-cli
title: Request Copilot Code Review Directly from GitHub CLI
author: Allison
primary_section: github-copilot
feed_name: The GitHub Blog
date: 2026-03-11 18:11:51 +00:00
tags:
- Accessibility
- AI
- Automation
- CLI Tools
- Client Apps
- Code Collaboration
- Code Review
- Continuous Integration
- Copilot
- Copilot Code Review
- Developer Workflow
- DevOps
- GitHub CLI
- GitHub Copilot
- Improvement
- News
- Productivity
- Pull Requests
- Reviewer Selection
- V2.88.0
section_names:
- ai
- devops
- github-copilot
---
Allison introduces a GitHub CLI update that enables developers to request Copilot code reviews directly from the terminal, improving code workflow and reviewer selection performance.<!--excerpt_end-->

# Request Copilot Code Review Directly from GitHub CLI

Developers can now leverage GitHub Copilot to request code reviews right from their terminal using the GitHub CLI. This enhancement streamlines the pull request workflow, letting you add Copilot as a reviewer without leaving the command line environment.

## How It Works

- **Non-interactive usage:**
  - Add Copilot as a reviewer with:

    ```shell
    gh pr edit --add-reviewer @copilot
    ```

- **Interactive usage:**
  - While selecting reviewers interactively (e.g., when running `gh pr create`), Copilot is now listed along with other team members, making it easy to select as a reviewer during the process.

## Improved Reviewer and Assignee Selection

This release introduces a search-based selection experience for reviewers and assignees. Instead of loading every collaborator and team up front, results are retrieved as you type. This not only enhances performance—especially for large organizations—but also resolves accessibility issues, such as screen readers having to read out thousands of options.

## Availability

- The feature is available on all [plans that include Copilot code review](https://docs.github.com/copilot/concepts/agents/code-review#availability).
- To access these enhancements, install or upgrade to GitHub CLI [v2.88.0](https://github.com/cli/cli/releases/tag/v2.88.0) or later.

## Learn More

- [GitHub CLI Release Notes](https://github.com/cli/cli/releases)
- [Copilot Code Review Documentation](https://docs.github.com/copilot/using-github-copilot/code-review/using-copilot-code-review)
- [Open issues or feedback](https://github.com/cli/cli/issues)

---
**Author:** Allison

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-11-request-copilot-code-review-from-github-cli)
