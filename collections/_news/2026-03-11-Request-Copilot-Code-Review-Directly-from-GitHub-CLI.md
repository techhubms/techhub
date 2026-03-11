---
layout: "post"
title: "Request Copilot Code Review Directly from GitHub CLI"
description: "This announcement details a new feature that lets developers request code reviews from GitHub Copilot using the GitHub CLI, both non-interactively and through interactive prompts. The update also introduces improved search functionality when selecting reviewers or assignees, enhancing accessibility and performance for large projects."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-03-11-request-copilot-code-review-from-github-cli"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-03-11 18:11:51 +00:00
permalink: "/2026-03-11-Request-Copilot-Code-Review-Directly-from-GitHub-CLI.html"
categories: ["AI", "DevOps", "GitHub Copilot"]
tags: ["Accessibility", "AI", "Automation", "CLI Tools", "Client Apps", "Code Collaboration", "Code Review", "Continuous Integration", "Copilot", "Copilot Code Review", "Developer Workflow", "DevOps", "GitHub CLI", "GitHub Copilot", "Improvement", "News", "Productivity", "Pull Requests", "Reviewer Selection", "V2.88.0"]
tags_normalized: ["accessibility", "ai", "automation", "cli tools", "client apps", "code collaboration", "code review", "continuous integration", "copilot", "copilot code review", "developer workflow", "devops", "github cli", "github copilot", "improvement", "news", "productivity", "pull requests", "reviewer selection", "v2dot88dot0"]
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
