---
layout: "post"
title: "Building a More Accessible GitHub CLI: Improving Terminal Accessibility for Developers"
description: "Ryan Hecht details GitHub CLI's efforts to make command-line tools more accessible by adapting screen reader compatibility, color contrast, and customization for diverse developer needs. The journey involves creative solutions where web standards fall short, aiming for a cohesive, inclusive terminal experience."
author: "Ryan Hecht"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/engineering/user-experience/building-a-more-accessible-github-cli/"
viewing_mode: "external"
feed_name: "GitHub Engineering Blog"
feed_url: "https://github.blog/engineering/feed/"
date: 2025-05-02 14:30:00 +00:00
permalink: "/news/2025-05-02-Building-a-More-Accessible-GitHub-CLI-Improving-Terminal-Accessibility-for-Developers.html"
categories: ["DevOps"]
tags: ["Accessibility", "CLI", "Color Contrast", "Command Line", "Customization", "DevOps", "Engineering", "GitHub CLI", "High Contrast", "News", "Prompting Library", "Screen Readers", "Terminal", "User Experience"]
tags_normalized: ["accessibility", "cli", "color contrast", "command line", "customization", "devops", "engineering", "github cli", "high contrast", "news", "prompting library", "screen readers", "terminal", "user experience"]
---

In this insightful article, Ryan Hecht shares GitHub’s journey to improve the accessibility of its CLI. The post explores the challenges and solutions in making terminal-based tools more inclusive for all developers.<!--excerpt_end-->

# Building a More Accessible GitHub CLI

*Author: Ryan Hecht*

At GitHub, we’re committed to making our tools truly accessible for every developer, regardless of ability or toolset. The command line interface (CLI) is a vital part of the developer experience, and the [GitHub CLI](https://cli.github.com/) brings the power of GitHub to the terminal.

## The Accessibility Challenge in the Terminal

Unlike web browsers or graphical interfaces, the terminal environment has no universal accessibility standards like the Web Content Accessibility Guidelines (WCAG) for the web. While the W3C offers [high-level guidance for non-web software](https://www.w3.org/TR/wcag2ict-22/#applying-wcag-2-to-text-applications), concrete techniques for terminal and CLI accessibility are largely undefined. This opens both a challenge and an opportunity for teams developing CLI tools.

## GitHub CLI’s Public Preview Focus

The [recent Public Preview](https://github.blog/changelog/2025-05-01-improved-accessibility-features-in-github-cli/) of GitHub CLI’s accessibility features targets three key user groups:

1. **Screen reader users**
2. **Those who require high contrast between background and text**
3. **Users needing customizable color options**

You can enable these features in the latest GitHub CLI release using `gh a11y`.

## Understanding the Terminal Accessibility Landscape

Text-based applications differ vastly from web applications in their interaction with assistive technology. Web assistive tools leverage the DOM to infer structure, but a CLI’s output is plain text without hidden markup. Terminals act as user agents, rendering characters and leaving assistive technologies to analyze and infer structure just based on this matrix of text. As [WCAG2ICT](https://www.w3.org/TR/wcag2ict-22/) guidance notes, accessibility here means ensuring all text output is available and structural cues are programmatically determinable.

The GitHub CLI team explored how screen readers interact with terminal output, the impact of color and contrast, and strategies for conveying structure in plain text—all in the absence of comprehensive standards.

## Rethinking Prompts and Progress for Screen Readers

The CLI’s rich prompting enables interactive input but often confuses screen readers, particularly when visual cues or constant redraws are used. Non-alphanumeric symbols and visual effects (like spinners) can be misinterpreted by speech synthesis.

**Key Accessibility Improvements:**

- **Prompting:**
  - Introducing a new experience using Charm’s open source [`charmbracelet/huh` prompting library](https://github.com/charmbracelet/huh).
  - Prompts are now designed for accurate reading by screen readers.

- **Progress Indicators:**
  - The animated spinner—problematic for screen readers—has been replaced by a static progress indicator.
  - Relevant action messages provide context or fall back on a generic “Working…” message.

## Color, Contrast, and Customization

- Color highlighting is critical in terminal workflows, but low contrast can exclude some users.
- Unlike browsers, CLI apps can’t specify the terminal background. The user’s emulator dictates the background, so CLI applications must design color output for variable backgrounds.
- **Legacy Markdown palette:** Previously, GitHub CLI did not account for background color, producing low contrast for some users.
  - ![A screenshot of the legacy Markdown palette.](https://github.blog/wp-content/uploads/2025/05/github-cli-legacy-markdown-palette.png?resize=614%2C662)
- **Improved Markdown palette and customization:**
  - The team focused on the widely supported ANSI 4-bit color table, aligning color palettes so users can customize experiences through terminal preferences.
  - [Primer accessibility principles](https://primer.style/accessibility/color-considerations/) informed the new palette choices.
  - ![A screenshot showing the improved Markdown palette.](https://github.blog/wp-content/uploads/2025/05/github-cli-improved-markdown-palette.png?resize=614%2C662)

## Supporting Extension Authors and Community Feedback

- The team’s improvements aim to aid blind, low vision, and colorblind developers.
- Extension authors will be enabled to implement these accessibility improvements in their own GitHub CLI extensions, providing a cohesive and accessible experience throughout the CLI ecosystem.
- Future work includes better table formatting for easier screen reader interpretation.
- Collaboration was key, with gratitude extended to [Charm](https://github.com/charmbracelet) and the GitHub Accessibility team.

## Get Involved and Share Feedback

- **Try the features**: Update GitHub CLI to [v2.72.0](https://github.com/cli/cli/releases/tag/v2.72.0), then run `gh a11y` to explore accessible features.
- **Join the discussion**: Share suggestions and experiences via the [GitHub CLI accessibility discussion](https://github.com/orgs/community/discussions/158037).
- **Connect with the team**: Users with relevant lived experience are encouraged to engage with the GitHub Accessibility team or join the [discussion panel](https://github.com/orgs/community/discussions/128172).

## Looking Forward

Adapting accessibility standards for CLIs is both a challenge and an opportunity. GitHub’s continued efforts are aimed at setting new standards and supporting all developers in terminal environments.

Thank you for your feedback and for building a more accessible GitHub together.

**Interested in GitHub’s accessibility efforts?** [Learn more](https://accessibility.github.com/feedback).

This post appeared first on "GitHub Engineering Blog". [Read the entire article here](https://github.blog/engineering/user-experience/building-a-more-accessible-github-cli/)
