---
layout: "post"
title: "Write Better Commit Messages with GitHub Copilot"
description: "This post by Randy Pagels explores how GitHub Copilot can assist developers in crafting clear, meaningful commit messages directly from code changes. It covers practical steps for using Copilot within VS Code and Copilot Chat, and provides extra prompt ideas for optimizing commit narratives."
author: "randy.pagels@xebia.com (Randy Pagels)"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://pagelsr.github.io/CooknWithCopilot/blog/write-better-commit-messages-with-github-copilot.html"
viewing_mode: "external"
feed_name: "Randy Pagels's Blog"
feed_url: "https://pagelsr.github.io/CooknWithCopilot/rss.xml"
date: 2025-05-16 00:00:00 +00:00
permalink: "/2025-05-16-Write-Better-Commit-Messages-with-GitHub-Copilot.html"
categories: ["AI", "DevOps", "GitHub Copilot"]
tags: ["AI", "Best Practices", "Code History", "Code Reviews", "Commit Messages", "Conventional Commits", "Copilot Chat", "Developer Productivity", "DevOps", "GitHub Copilot", "Posts", "Pull Requests", "VS Code"]
tags_normalized: ["ai", "best practices", "code history", "code reviews", "commit messages", "conventional commits", "copilot chat", "developer productivity", "devops", "github copilot", "posts", "pull requests", "vs code"]
---

Randy Pagels shares practical guidance for using GitHub Copilot to generate clear, concise commit messages, highlighting actionable steps and prompt ideas for developers looking to streamline their Git version control workflows.<!--excerpt_end-->

## Write Better Commit Messages with GitHub Copilot

**Posted on Apr 11, 2025 by Randy Pagels**

A well-written commit message tells the story behind your code change—without requiring a lengthy writing process. Whether you’re addressing a minor bug or implementing a major feature, GitHub Copilot can help you quickly generate clear, professional commit messages.

### Why Good Commit Messages Matter

- **Storytelling:** A strong commit message provides context to teammates and your future self, clarifying why the change was made.
- **Efficiency:** Automating message suggestions saves time and reduces cognitive load, making version control smoother.

### Let GitHub Copilot Suggest Commit Messages

GitHub Copilot can analyze the diff of your staged changes and suggest meaningful, context-aware commit messages. These suggestions can fit your team's preferred style, such as conventional commits, concise summaries, or past-tense descriptions.

#### Ways to Use GitHub Copilot for Commit Messages

#### 1️⃣ In Copilot Chat: Ask for a Commit Message

Use natural language instructions in Copilot Chat, such as:

```
# Suggest a commit message based on my staged changes
```

Copilot Chat will review your diffs and generate a relevant message summarizing your work.

#### 2️⃣ In the Commit Message Field (Visual Studio Code)

When you’re in the commit message field, type a comment that describes what you’ve changed:

```js
// Suggest a concise commit message for the updated login form validation
```

Trigger Copilot’s suggestion (by pressing `Tab` or using the Copilot action), and Copilot will autocomplete a commit message.

#### 3️⃣ For Pull Request Titles or Descriptions

Use Copilot to summarize PRs with a clear, professional overview of what’s changed, for example:

```
# Summarize this pull request in a clear, professional message
```

This ensures your PR descriptions match the content and improve team communication.

### Extra Prompt Ideas

- `# Write a commit message using the Conventional Commits format`
- `# Generate a descriptive commit message for a refactor`
- `# Create a commit message that mentions the related issue #245`

### Quick Takeaway

Good commit messages improve code history and team collaboration. GitHub Copilot makes it easy by turning your changes into clear, useful messages—helping you focus on coding while keeping your version control organized.

---

*For additional support and tutorials, consult GitHub Copilot documentation or experiment with your own custom prompts to further tailor Copilot’s output to your workflow.*

This post appeared first on "Randy Pagels's Blog". [Read the entire article here](https://pagelsr.github.io/CooknWithCopilot/blog/write-better-commit-messages-with-github-copilot.html)
