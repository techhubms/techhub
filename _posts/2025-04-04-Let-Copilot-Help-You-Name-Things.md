---
layout: "post"
title: "Let Copilot Help You Name Things"
description: "Randy Pagels describes how developers can use GitHub Copilot to generate better names for functions, variables, and other code elements. The article provides practical suggestions for using Copilot’s chat, edit, and agent modes to improve naming consistency and code clarity across your projects."
author: "randy.pagels@xebia.com (Randy Pagels)"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://pagelsr.github.io/CooknWithCopilot/blog/let-copilot-help-you-name-things.html"
viewing_mode: "external"
feed_name: "Randy Pagels's Blog"
feed_url: "https://pagelsr.github.io/CooknWithCopilot/rss.xml"
date: 2025-04-04 00:00:00 +00:00
permalink: "/2025-04-04-Let-Copilot-Help-You-Name-Things.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["AI", "AI Assisted Development", "Code Quality", "Code Refactoring", "Coding", "Copilot Agent Mode", "Copilot Chat", "Developer Productivity", "Function Naming", "GitHub Copilot", "Naming Conventions", "Posts", "Variable Naming"]
tags_normalized: ["ai", "ai assisted development", "code quality", "code refactoring", "coding", "copilot agent mode", "copilot chat", "developer productivity", "function naming", "github copilot", "naming conventions", "posts", "variable naming"]
---

Authored by Randy Pagels, this article explores how GitHub Copilot can assist developers in overcoming one of programming's hardest challenges: naming things clearly and consistently. Practical tips are shared for leveraging AI-driven name suggestions.<!--excerpt_end-->

## Let Copilot Help You Name Things

*Posted by Randy Pagels on Apr 25, 2025*

### Overview

Naming things—be it functions, variables, or database fields—is a well-known pain point for developers. Choosing the right name can interrupt your workflow, but GitHub Copilot offers AI-powered suggestions that make the process smoother, more consistent, and less taxing.

---

### Using Copilot to Improve Naming

**ProTip:**
*Use Copilot to generate functionally descriptive names instead of sticking with placeholders like `foo`, `temp`, or `xyz`. Copilot leverages the context of your code to propose meaningful names that enhance readability and maintainability.*

#### 1. Copilot Chat: Ask Directly for Naming Help

Example:

```js
// Suggest a more descriptive name for this function that calculates discount based on user role and cart total
```

Type your intent and let Copilot propose alternatives.

#### 2. Edit Mode: Inline Rename Prompts

- Highlight a function or variable.
- Add a comment, such as:
  - `# Rename this variable to better reflect what it represents`
- Trigger Copilot Agent Mode to implement the renaming suggestion directly in your code.

#### 3. Agent Mode: Apply Naming Conventions at Scale

- Use Agent Mode to generate consistent names for variables and methods—especially useful for large-scale refactoring or codebase cleanups (e.g., applying naming conventions across all controllers of a CRUD system).

---

### Extra Prompts to Try with Copilot

- `# Rename this class to better describe what it models`
- `# What's a better name for this loop counter?`
- `# Suggest field names for a form collecting user contact details`
- `// Rename this test to clearly describe what it's testing`

---

### Quick Takeaway

Letting Copilot name things can free you from decision paralysis and help ensure your code remains clear for future readers. Copilot's context-sensitive prompts expedite naming choices, making your projects easier to read, maintain, and scale.

**Authored by [Randy Pagels](mailto:randy.pagels@xebia.com)**

This post appeared first on "Randy Pagels's Blog". [Read the entire article here](https://pagelsr.github.io/CooknWithCopilot/blog/let-copilot-help-you-name-things.html)
