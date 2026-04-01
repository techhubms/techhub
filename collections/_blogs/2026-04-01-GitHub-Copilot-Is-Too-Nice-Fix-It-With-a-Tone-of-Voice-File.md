---
primary_section: github-copilot
feed_name: Emanuele Bartolesi's Blog
external_url: https://dev.to/playfulprogramming/github-copilot-is-too-nice-fix-it-with-a-tone-of-voice-file-39ij
tags:
- AI
- Blogs
- Code Review
- Coding Standards
- Copilot Chat
- Copilot Plan
- Copilot Quota
- Critical Feedback
- Developer Productivity
- Developer Workflow
- GitHub
- GitHub Copilot
- Maintainability
- Programming
- Prompt Engineering
- Repository Configuration
- Trade Offs
- Voice Instructions
- VS Code
- VS Code Extension
date: 2026-04-01 12:21:58 +00:00
section_names:
- ai
- github-copilot
title: GitHub Copilot Is Too Nice. Fix It With a Tone of Voice File.
author: Emanuele Bartolesi
---

Emanuele Bartolesi explains how to make GitHub Copilot less “agreeable” and more useful by adding a repo-level voice instructions file that pushes Copilot to be direct, critical, and focused on correctness and maintainability.<!--excerpt_end-->

## GitHub Copilot is “too nice” by default

The post argues that many GitHub Copilot setups are overly polite: they tend to agree, avoid criticism, and keep answers “safe”. In practice, that can lead to:

- Weak suggestions
- Missed problems
- Bad decisions slipping through

## Fix it with a tone-of-voice file

The suggested approach is to change Copilot’s behavior by adding a voice instructions file in your repository.

### Step 1: Create `voice-instructions.md`

Create a `voice-instructions.md` file in your repo and instruct Copilot to be more direct and critical.

```text
---
applyTo: '**'
---

Give direct, critical feedback. Identify mistakes, weak assumptions, unnecessary complexity, unclear naming, hidden risks, and poor trade-offs without softening the message.

Do not add generic praise or filler. Do not agree by default.

When something is wrong, say exactly what is wrong, why it is a problem, and what should be done instead.

Prioritize correctness, clarity, simplicity, maintainability, and practical delivery.

Challenge vague requirements and surface missing constraints, edge cases, and operational risks.

Be blunt but professional. Never be insulting. Always aim to be useful.
```

### What you get

- Clear problems called out
- Weak assumptions challenged
- Simpler and more maintainable alternatives

The claimed outcome is better code and faster decisions.

## Trade-offs

- It can feel harsher
- It’s not ideal for beginners
- You lose the “friendly assistant” vibe

The author’s view is that for “real systems”, friendliness is less important than usefulness.

## Appendix: VS Code extension for Copilot quota visibility

The post includes a promotional appendix for a VS Code extension the author built called **Copilot Insights**, which shows Copilot plan and quota status inside VS Code (without usage analytics or productivity scoring).

- Marketplace link: https://marketplace.visualstudio.com/items?itemName=emanuelebartolesi.vscode-copilot-insights

![Screenshot related to Copilot Insights extension](https://media2.dev.to/dynamic/image/width=256,height=,fit=scale-down,gravity=auto,format=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2F8j7kvp660rqzt99zui8e.png)

[Read the entire article](https://dev.to/playfulprogramming/github-copilot-is-too-nice-fix-it-with-a-tone-of-voice-file-39ij)

