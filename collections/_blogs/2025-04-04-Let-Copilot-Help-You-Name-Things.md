---
layout: "post"
title: "Let Copilot Help You Name Things"
description: "This practical guide demonstrates how developers can use GitHub Copilot to generate better, more descriptive names for variables, functions, classes, and more. Covering Copilot Chat, Edit Mode, and Agent Mode, it guides users in leveraging AI suggestions to maintain clear and consistent code throughout their projects."
author: "randy.pagels@xebia.com (Randy Pagels)"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.cooknwithcopilot.com/blog/let-copilot-help-you-name-things.html"
viewing_mode: "external"
feed_name: "Randy Pagels's Blog"
feed_url: "https://www.cooknwithcopilot.com/rss.xml"
date: 2025-04-04 00:00:00 +00:00
permalink: "/blogs/2025-04-04-Let-Copilot-Help-You-Name-Things.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["AI", "AI Coding Assistant", "Code Quality", "Coding", "Coding Best Practices", "Copilot Agent Mode", "Copilot Chat", "Developer Productivity", "Edit Mode", "Function Naming", "GitHub Copilot", "Naming Conventions", "Posts", "Programming Tips", "Refactoring", "Variable Naming"]
tags_normalized: ["ai", "ai coding assistant", "code quality", "coding", "coding best practices", "copilot agent mode", "copilot chat", "developer productivity", "edit mode", "function naming", "github copilot", "naming conventions", "posts", "programming tips", "refactoring", "variable naming"]
---

Randy Pagels demonstrates how GitHub Copilot can help developers generate more effective names for code elements, making software easier to maintain and understand.<!--excerpt_end-->

# Let Copilot Help You Name Things

*Posted on Apr 25, 2025 by Randy Pagels*

Naming things in programming—from functions and classes to variables and fields—is notoriously challenging. This post explores practical ways to use GitHub Copilot’s AI capabilities to generate clear, descriptive names and maintain consistent coding standards.

## Why Naming Matters

Poorly named code can become hard to maintain, difficult to read, and prone to bugs. Yet, coming up with the perfect name can interrupt your development flow. GitHub Copilot provides context-aware suggestions, making it easier to pick names that accurately describe what your code does.

## Copilot in Action: Practical Steps

### 1. Copilot Chat: Ask for a Better Name

- You can prompt Copilot with comments in your code, such as:

  ```
  // Suggest a more descriptive name for this function that calculates discount based on user role and cart total
  ```

- Copilot will analyze the code context and offer relevant name suggestions.

### 2. Edit Mode: Inline Rename Prompt

- Highlight a variable or function, and leave an inline comment like:

  ```python
  # Rename this variable to better reflect what it represents
  ```

- Trigger Copilot Agent Mode for quick, automated renaming.

### 3. Agent Mode: Naming Conventions at Scale

- When refactoring larger codebases, Copilot can generate consistent variable and method names across multiple files or controllers, such as applying CRUD naming patterns.

## Example Prompts to Use

- `# Rename this class to better describe what it models`
- `# What’s a better name for this loop counter?`
- `# Suggest field names for a form collecting user contact details`
- `// Rename this test to clearly describe what it's testing`

---

**Quick Takeaway**

Improving naming doesn’t need to slow you down. With GitHub Copilot, you can quickly generate clear, maintainable names, making your codebase easier to read and update.

This post appeared first on "Randy Pagels's Blog". [Read the entire article here](https://www.cooknwithcopilot.com/blog/let-copilot-help-you-name-things.html)
