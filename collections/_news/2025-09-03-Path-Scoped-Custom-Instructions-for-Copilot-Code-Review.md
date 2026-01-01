---
layout: "post"
title: "Path-Scoped Custom Instructions for Copilot Code Review"
description: "This update from GitHub announces that Copilot code review now supports path-scoped custom instruction files, allowing targeted code review guidance in repositories. The post outlines how developers can leverage `*.instructions.md` files and the new `applyTo` section for fine-grained feedback, and details the deprecation of the previous coding guidelines feature in favor of this new flexible approach."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-09-03-copilot-code-review-path-scoped-custom-instruction-file-support"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-09-03 20:41:26 +00:00
permalink: "/2025-09-03-Path-Scoped-Custom-Instructions-for-Copilot-Code-Review.html"
categories: ["AI", "GitHub Copilot"]
tags: ["AI", "Applyto Section", "Code Review Automation", "Copilot Code Review", "Custom Instructions", "Developer Tools", "GitHub", "GitHub Copilot", "News", "Path Scoped Instructions", "Pull Requests", "Repository Guidance"]
tags_normalized: ["ai", "applyto section", "code review automation", "copilot code review", "custom instructions", "developer tools", "github", "github copilot", "news", "path scoped instructions", "pull requests", "repository guidance"]
---

Allison introduces new path-scoped custom instruction file support in Copilot code review, enhancing developers’ ability to provide focused feedback within specific parts of their repositories.<!--excerpt_end-->

# Path-Scoped Custom Instructions for Copilot Code Review

GitHub has released a new capability for Copilot code review: path-scoped custom instruction file support. This allows engineering teams to deliver targeted feedback tailored for specific areas of a repository, making code review more actionable and relevant.

## What’s New

- **Path-Scoped Instructions**: Developers can now create `*.instructions.md` files in the `.github/instructions` directory, each with an `applyTo` section using path globs (e.g., `webapp/src/**`). Copilot code review references these files to deliver guidance only for specific files or directories, rather than applying one-size-fits-all criteria.

- **Granular Feedback**: By associating instructions with specific portions of the repository—like frontend, backend, or security directories—teams can ensure that pull requests receive context-aware feedback, improving code quality and consistency.

- **How to use**:
  1. Create/update a custom instruction file (e.g., `frontend.instructions.md`) under `.github/instructions`.
  2. Add an `applyTo:` section with one or more glob patterns.
  3. Write concise code review guidance relevant to the targeted paths.
  4. After committing and opening/updating a pull request that impacts these paths, Copilot code review will utilize the pertinent instructions.

**Example:**

```markdown
# frontend.instructions.md

applyTo:
- webapp/src/**
- ui/components/**
--- Emphasize accessibility (ARIA, focus management). Prefer design tokens. Flag deprecated components under `legacy/`.
```

- **Organization-Level Instructions**: Copilot code review will now also include organization-level instructions, ensuring shared coding standards across multiple repositories with no extra configuration.

- **Deprecation of Old Guidelines Feature**: The previous coding guidelines feature (from Copilot Enterprise private preview) has been fully deprecated. All customizations should now be specified in `copilot-instructions.md` or scoped instruction files, centralizing code review guidance.

For further details, see the [Copilot custom instructions documentation](https://docs.github.com/copilot/how-tos/configure-custom-instructions/add-repository-instructions#creating-a-repository-custom-instructions-file).

You can also join discussions on implementation and usage in the [GitHub Community](https://github.com/orgs/community/discussions/categories/announcements).

---

These changes give developers and teams more control and flexibility in maintaining consistent, high-quality code across growing repositories.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-03-copilot-code-review-path-scoped-custom-instruction-file-support)
