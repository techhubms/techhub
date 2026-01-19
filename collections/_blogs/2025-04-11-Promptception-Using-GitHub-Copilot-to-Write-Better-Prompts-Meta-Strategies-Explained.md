---
layout: post
title: 'Promptception: Using GitHub Copilot to Write Better Prompts (Meta Strategies Explained)'
author: randy.pagels@xebia.com (Randy Pagels)
canonical_url: https://cooknwithcopilot.com/blog/promptception-improve-prompts-with-copilot.html
viewing_mode: external
feed_name: Randy Pagels's Blog
feed_url: https://cooknwithcopilot.com/rss.xml
date: 2025-04-11 00:00:00 +00:00
permalink: /github-copilot/blogs/Promptception-Using-GitHub-Copilot-to-Write-Better-Prompts-Meta-Strategies-Explained
tags:
- Code Review
- Copilot Agent Mode
- Copilot Chat
- Copilot Edit
- Developer Productivity
- Playwright
- Prompt Engineering
- Prompt Optimization
- README Generation
section_names:
- ai
- github-copilot
---
In this post, Randy Pagels introduces 'Promptception'—the process of leveraging GitHub Copilot to craft better prompts for Copilot itself. Learn strategies to enhance prompt effectiveness using Chat, Edit, and Agent Modes.<!--excerpt_end-->

# Promptception – Improve Prompts with Copilot

*By Randy Pagels*

## Overview

Randy Pagels discusses the concept of 'Promptception': using GitHub Copilot tools to help you write better prompts for Copilot, enhancing the effectiveness of its suggestions and code generation features. The article provides detailed, practical examples across various Copilot modes.

## What is Promptception?

Promptception refers to the meta approach of leveraging GitHub Copilot's capabilities—such as Chat, Edit, or Agent Mode—not only for code generation, but also to aid in crafting, refining, or optimizing the prompts you use within those very tools. The aim is to improve your workflow by starting with higher-quality prompts, which in turn produce better results from Copilot.

> **ProTip:** *Use Copilot to Help You Shape Smarter Prompts Before You Use Them!*

Promptception isn’t magic—it’s a strategic enhancement of your Copilot-assisted development process. By engaging Copilot to generate or revise prompts for itself, you can:

- Clarify requirements
- Achieve better code or documentation outputs
- Reduce iteration time

## Promptception in Action (Across Copilot Modes)

### 1️⃣ Chat Mode: Prompt to Build a Prompt

**Example**: If you want to test a checkout flow using Playwright, ask Copilot Chat directly:
> *"I want to test a checkout flow using Playwright. Help me write a Copilot Chat prompt to generate this test."*

Let Copilot Chat generate a clear, structured prompt you can reuse or iterate on.

### 2️⃣ Edit Mode: Prompt Rewrite Assistance

**Example**:

```js
// Rewrite the comment above to be a more specific Copilot Edit instruction that includes a retry step and network mock
```

Use Copilot Edit in your code editor to refine instructions and ensure specificity, such as adding features or adjusting the scope.

### 3️⃣ Agent Mode: Meta-Prompt to Set Intent

**Example**:
> *"Act as a GitHub Copilot Agent. Write a prompt I can use to create a custom action in Agent Mode that validates PR titles."*

This usage is invaluable for defining agent behaviors or specifying tasks in Agent Mode, such as workflow automations or validations.

### 4️⃣ Chat Mode: Style-Guided Prompt

**Example**:
> *"Suggest a prompt that tells Copilot Chat to generate test code that uses async/await and avoids callbacks."*

Use this approach when you want Copilot to adopt a particular coding style or pattern in its outputs.

### 5️⃣ Chat or Edit Mode: Prompt for Visual Review

**Example**:
> *"Help me write a prompt I can use to review this code block for clarity, structure, and comments."*

Generate custom review prompts to ensure your code quality meets your standards before finalization.

### 6️⃣ Chat Mode: Document Review Promptception

**Example**:
> *"Create a short prompt for me to send back to you. I want you to review a document. Make sure it's grammatically correct, doesn't duplicate any details, and follows a good flow."*

This assists in ensuring your documentation is polished prior to sharing.

### 7️⃣ Chat Mode: README Promptception

**Example**:
> *"Create a prompt for me to send back to you. I want to generate a README.md file that summarizes what this project does, including its purpose, domain, tech stack, and overall functionality."*

Great for building a comprehensive, reusable README prompt before engaging Copilot to generate the file.

## Quick Takeaway

Promptception is a practical and effective technique: ask GitHub Copilot to help you craft the right prompt before running it in Chat, Edit, or Agent Mode. This simple strategy can greatly enhance the utility and accuracy of Copilot’s responses.

---
_Article originally by Randy Pagels_

This post appeared first on "Randy Pagels's Blog". [Read the entire article here](https://cooknwithcopilot.com/blog/promptception-improve-prompts-with-copilot.html)
