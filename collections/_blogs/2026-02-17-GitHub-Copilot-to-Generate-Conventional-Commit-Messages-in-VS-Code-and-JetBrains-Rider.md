---
author: Emanuele Bartolesi
tags:
- AI
- Atomic Commits
- Blogs
- BREAKING CHANGE Footer
- Changelog Generation
- CI
- Commit Message Generation
- Commit Messages
- Conventional Commits
- Copilot Chat
- Developer Workflow
- DevOps
- Git
- GitHub
- GitHub Actions
- GitHub Copilot
- JetBrains Rider
- Semantic Versioning
- Settings.json
- Source Control
- VS Code
- VS Code Extension
- VS Code Marketplace
title: GitHub Copilot to Generate Conventional Commit Messages in VS Code and JetBrains Rider
feed_name: Emanuele Bartolesi's Blog
section_names:
- ai
- devops
- github-copilot
date: 2026-02-17 08:03:45 +00:00
external_url: https://dev.to/playfulprogramming/github-copilot-to-generate-conventional-commit-messages-in-vscode-and-jetbrains-rider-1n3b
primary_section: github-copilot
---

Emanuele Bartolesi shows how to use GitHub Copilot as a guardrail for generating strict Conventional Commit messages in VS Code and JetBrains Rider, with concrete instruction snippets you can paste into each IDE to make the output consistent and automation-friendly.<!--excerpt_end-->

## Overview

Most commit messages degrade into vague placeholders (for example: “fix stuff”, “update code”, “changes”). They’re understandable in the moment but become useless later.

The post argues commit messages are primarily for:

- Your future self
- Your teammates
- Automation pipelines
- Changelog generators
- Semantic versioning

Because writing structured commit messages takes discipline (and discipline drops under pressure), the author suggests using GitHub Copilot as a **commit quality guardrail**—not just as a coding assistant.

## Why Conventional Commits still matter

Conventional Commits are framed as **structure**, not style.

Basic format:

```text
type(scope): description
```

Examples:

```text
feat(auth): add JWT token validation
fix(api): handle null response in user endpoint
refactor(ui): simplify navbar component
```

### Readable Git history

Unstructured history:

```text
update stuff
fix bug
changes
```

Structured history:

```text
fix(auth): prevent null pointer on login
feat(api): add user filtering by role
ci(github-actions): add build cache
```

The structured version is intended to be self-documenting.

## Using GitHub Copilot for commit messages in VS Code

Copilot can generate commit messages from the **Source Control** panel.

By default, generation may be generic, so the key is to add **custom instructions**.

Add the following to `settings.json`:

```json
"github.copilot.chat.commitMessageGeneration.instructions": [
  { "text": "Follow Conventional Commits: type(scope): description." },
  { "text": "Use lowercase type and scope." },
  { "text": "Use imperative mood: 'add', 'fix', 'update', not past tense." },
  { "text": "Keep subject under 50 characters. No period." },
  { "text": "Describe the intent clearly. Avoid vague messages like 'update code'." },
  { "text": "Use only these types: feat, fix, docs, style, refactor, perf, test, chore, ci." },
  { "text": "Include a scope when the change targets a specific area." },
  { "text": "Ensure each commit represents one logical change." },
  { "text": "Add a body when needed, separated by a blank line." },
  { "text": "Use bullet points (*) in the body for multiple changes." },
  { "text": "Explain why the change was made, not only what changed." },
  { "text": "Add BREAKING CHANGE: in the footer when applicable." }
]
```

Example improvement:

Instead of:

```text
update login logic
```

Copilot can generate:

```text
fix(auth): prevent null pointer on login

* add null check for user object
* improve error handling for invalid credentials
```

The point is that Copilot becomes consistent when rules are explicit.

## Using GitHub Copilot for commit messages in JetBrains Rider

In Rider, Copilot integrates into the **Commit** tool window.

Example instruction structure:

```text
Follow the Conventional Commits specification strictly.

<type>(<scope>): <short description>

All sections except the first line are optional. Use only these types: feat, fix, docs, style, refactor, perf, test, chore, ci. Max 50 characters. Imperative mood. No period. Each commit must represent one logical change. Use BREAKING CHANGE: footer when applicable.
```

Example output:

```text
feat(auth): add refresh token support

* implement refresh token endpoint
* update token validation logic
* improve session security
```

Breaking change example:

```text
refactor(api): rename user endpoint

BREAKING CHANGE: /users endpoint is now /v2/users
```

Where to set it in Rider:

- **Settings** > **Tools** > **GitHub Copilot** > **Git Commit Instructions** > **Global**

Screenshot:

![Rider GitHub Copilot commit instructions settings](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fpq34h6um4euucbpvtqaf.png)

The author notes:

- Rider allows more verbose enforcement
- VS Code favors compact rule lists
- Both approaches work

## Making Copilot strict (not creative)

Copilot predicts text; adding constraints reduces variance.

Key principles:

- Write hard rules, not suggestions
- Limit allowed types
- Enforce atomic commits
- Require proper breaking change footers
- Review output before committing

Copilot isn’t replacing judgment; it reduces repetitive formatting work.

## GitHub Copilot quota visibility in VS Code (extension)

The post mentions a VS Code extension by the author, **Copilot Insights**, to show Copilot plan and quota status inside VS Code.

- No usage analytics
- No productivity scoring

VS Code Marketplace link:

https://marketplace.visualstudio.com/items?itemName=emanuelebartolesi.vscode-copilot-insights

Screenshot:

![GitHub Copilot quota visibility in VS Code](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fsqzk5hamyymcmuh515a4.png)

## Notes

The input text included unrelated page/community UI fragments (templates, dismiss links, user profiles, and a comment thread). Those are not part of the core article content and are intentionally omitted here.


[Read the entire article](https://dev.to/playfulprogramming/github-copilot-to-generate-conventional-commit-messages-in-vscode-and-jetbrains-rider-1n3b)

