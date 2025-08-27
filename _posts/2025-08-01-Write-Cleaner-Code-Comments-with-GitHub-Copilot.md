---
layout: "post"
title: "Write Cleaner Code Comments with GitHub Copilot"
description: "Randy Pagels demonstrates how developers can leverage GitHub Copilot to generate clear, meaningful code comments. The guide covers using Copilot's Chat, Edit, and Agent modes to draft effective documentation, improving code maintainability and team collaboration."
author: "randy.pagels@xebia.com (Randy Pagels)"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://pagelsr.github.io/CooknWithCopilot/blog/write-cleaner-code-comments-with-github-copilot.html"
viewing_mode: "external"
feed_name: "Randy Pagels's Blog"
feed_url: "https://pagelsr.github.io/CooknWithCopilot/rss.xml"
date: 2025-08-01 00:00:00 +00:00
permalink: "/2025-08-01-Write-Cleaner-Code-Comments-with-GitHub-Copilot.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["AI", "Best Practices", "Code Clarity", "Code Comments", "Code Documentation", "Coding", "Copilot Agent Mode", "Copilot Chat", "Copilot Edit Mode", "Developer Productivity", "GitHub Copilot", "Posts", "Software Documentation"]
tags_normalized: ["ai", "best practices", "code clarity", "code comments", "code documentation", "coding", "copilot agent mode", "copilot chat", "copilot edit mode", "developer productivity", "github copilot", "posts", "software documentation"]
---

Randy Pagels introduces practical strategies for using GitHub Copilot to enhance code comments. The article explores Copilot’s various modes to generate clear explanations, helping teams and future developers better understand code logic.<!--excerpt_end-->

# Write Cleaner Code Comments with GitHub Copilot

*By Randy Pagels*

### Overview

Maintaining clear, understandable code is essential for effective collaboration and long-term software success. In this guide, Randy Pagels explores how developers can use GitHub Copilot to automate and improve code commenting, ensuring documentation is helpful for teammates and one's future self.

---

## The Value of Clear Code Comments

- **Well-written comments** do more than state what the code does; they explain the *why* behind design choices and logic.
- Clear documentation saves time when onboarding new collaborators or revisiting code months later.

## How GitHub Copilot Supports Better Code Comments

GitHub Copilot simplifies the documentation process. Rather than spending extra time crafting comments, you can use Copilot’s various prompt-driven modes to automate or assist in this task:

---

### ProTip

**Ask Copilot to Generate Comments That Explain Your Code Clearly**  
Instead of leaving code bare or writing vague remarks, use Copilot’s features to instantly create helpful, structured comments.

---

## Approaches to Generating Better Comments

### 1️⃣ Copilot Chat: Add Explanations

Prompt example:

```
# Add inline comments explaining each step of this function
```

**Usage:**

- Ideal for complex methods where you want step-by-step walkthroughs.
- Great for highlighting algorithm logic, data transformations, or nuanced decisions.

### 2️⃣ Edit Mode: Comment While You Code

Highlight a code block and enter:

```
# Add docstrings to describe inputs, outputs, and edge cases
```

**Usage:**

- Copilot can automatically generate docstrings.
- Ensures functions are easy to reuse and maintain.

### 3️⃣ Agent Mode: Review for Readability

Prompt example:
> Review this file and suggest clearer inline comments where the logic might confuse a new developer.

**Usage:**

- Useful for preparing for code reviews or when onboarding new team members.
- Helps make code more accessible to those unfamiliar with its context.

---

## Additional Prompts to Use with Copilot

- `# Add comments that explain the purpose of each class property`
- `// Generate a top-level comment explaining what this script does`
- `# Add inline notes for this algorithm to clarify performance trade-offs`

These prompts target different documentation needs: from high-level overviews to property-specific notes and algorithm analysis.

---

## Quick Takeaway

Readable code is more than just clean syntax—it needs clear explanations. With targeted prompts, GitHub Copilot can help you document your code as you write, making software easier to maintain and share within your team or with your future self.

---

*Posted on Aug 1, 2025 by Randy Pagels*

This post appeared first on "Randy Pagels's Blog". [Read the entire article here](https://pagelsr.github.io/CooknWithCopilot/blog/write-cleaner-code-comments-with-github-copilot.html)
