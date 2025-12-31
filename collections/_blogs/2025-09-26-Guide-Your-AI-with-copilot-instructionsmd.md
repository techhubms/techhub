---
layout: "post"
title: "Guide Your AI with copilot-instructions.md"
description: "This post explains how development teams can ensure consistent code style, documentation, and safe practices across their projects by using the copilot-instructions.md file. It covers how to guide GitHub Copilot's code suggestions with custom instructions, offering practical examples for naming conventions, documentation standards, and security policies. The article also provides actionable tips for effectively maintaining and applying these standards in repositories."
author: "randy.pagels@xebia.com (Randy Pagels)"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.cooknwithcopilot.com/blog/guide-your-ai-with-copilot-instructions-md.html"
viewing_mode: "external"
feed_name: "Randy Pagels's Blog"
feed_url: "https://www.cooknwithcopilot.com/rss.xml"
date: 2025-09-26 00:00:00 +00:00
permalink: "/blogs/2025-09-26-Guide-Your-AI-with-copilot-instructionsmd.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["AI", "Async/await", "Code Consistency", "Coding", "Copilot Instructions.md", "Developer Productivity", "Documentation Standards", "GitHub Copilot", "JavaScript", "JSDoc", "Naming Conventions", "Blogs", "Project Style Guides", "React", "Repository Configuration", "Security Best Practices", "Style Automation", "Test Frameworks"]
tags_normalized: ["ai", "asyncslashawait", "code consistency", "coding", "copilot instructionsdotmd", "developer productivity", "documentation standards", "github copilot", "javascript", "jsdoc", "naming conventions", "blogs", "project style guides", "react", "repository configuration", "security best practices", "style automation", "test frameworks"]
---

Randy Pagels explains how to use the copilot-instructions.md file to set project-wide rules for GitHub Copilot, helping teams achieve consistency and safer code generation.<!--excerpt_end-->

# Guide Your AI with copilot-instructions.md

Consistency on a development team can be a challenge, especially with code style, naming conventions, documentation formats, and secure coding practices. The `copilot-instructions.md` file helps address these challenges by providing GitHub Copilot with explicit project-specific rules directly in your repository.

## Why Use copilot-instructions.md?

- **Centralizes project standards:** Define naming conventions, documentation requirements, and safe coding practices in one place.
- **Improves Copilot’s suggestions:** Ensures generated code aligns with your team’s established rules without constant prompting.
- **Reduces manual cleanup:** By guiding Copilot, less time is spent fixing inconsistent or risky code.

## Examples of Effective Instructions

- **Naming Conventions:**
  - `Use camelCase for variables, PascalCase for classes, and ALL_CAPS_WITH_UNDERSCORES for constants.`
- **Documentation Style:**
  - `Use JSDoc format for function documentation. Each function must include parameters, return type, and one example.`
- **Security & Performance:**
  - `Never log sensitive data such as passwords, API keys, or tokens. Always use async/await for database calls.`

## How To Apply copilot-instructions.md

- Save the file in the `.github` folder at the root of your repository.
- GitHub Copilot will automatically reference the file when generating code suggestions in that repo.
- Update the instructions as your standards evolve; Copilot will use the latest version.

## Advanced Usage Ideas

- **Test Frameworks:** Specify, for example: `Use Jest for unit tests with describe and it blocks.`
- **Formatting Preferences:** Such as: `Use 2 spaces for indentation, no semicolons in JavaScript.`
- **Domain-Specific Conventions:** For React: `Always use function components with hooks.`
- **Combine with .prompt.md:** Use together for both style rules and task-specific generation.

## Quick Takeaway

The `copilot-instructions.md` file acts as a living style guide for your project that GitHub Copilot respects. Define it once, keep it updated, and let Copilot help your whole team stay consistent and secure.

This post appeared first on "Randy Pagels's Blog". [Read the entire article here](https://www.cooknwithcopilot.com/blog/guide-your-ai-with-copilot-instructions-md.html)
