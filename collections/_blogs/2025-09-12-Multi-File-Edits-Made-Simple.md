---
external_url: https://www.cooknwithcopilot.com/blog/multi-file-edits-made-simple.html
title: Multi-File Edits Made Simple
author: randy.pagels@xebia.com (Randy Pagels)
feed_name: Randy Pagels's Blog
date: 2025-09-12 00:00:00 +00:00
tags:
- Async/Await
- Automation
- Code Consistency
- Code Review
- Copilot Edits
- Developer Tools
- Diff Preview
- ES Modules
- Modernization
- Multi File Editing
- Node.js
- Programming Productivity
- Project Wide Changes
- Refactoring
section_names:
- ai
- coding
- github-copilot
---
Randy Pagels explains how to use GitHub Copilot’s multi-file edits feature to refactor, update, and modernize your codebase efficiently—all while retaining developer control.<!--excerpt_end-->

# Multi-File Edits Made Simple

*Posted on Sep 12, 2025 by Randy Pagels*

## Overview

GitHub Copilot now features **multi-file edit** support (currently in preview), letting you apply broad changes across your project instead of updating each file manually. This enables developers to efficiently refactor code, update imports, modernize style, and maintain consistency project-wide—all while reviewing suggested changes before applying them.

## When to Use Multi-File Edits

Multi-file edits are perfect for scenarios such as:

- **Refactoring shared functionality:** Renaming functions or updating APIs across several files.
- **Modernizing code:** Switching all imports from CommonJS to ES modules in a codebase.
- **Code style improvements:** Converting promise chains to async/await in service files or standardizing error-handling approaches.

By running a prompt (e.g., `# Update all references to getUserData to use the new fetchUserData function`), Copilot will search the entire project and suggest matching changes wherever needed.

## Key Features

- **Project-wide Scope:** Copilot scans and suggests edits in all related files, not just the one you’re editing.
- **Interactive Diffs:** Every suggested change appears as a diff, allowing for granular approval—accept, reject, or tweak each change individually.
- **Faster Refactors:** Large, repetitive changes that used to take hours can now be managed in minutes.
- **Developer Control:** Even with recommendations, developers decide what gets applied, keeping you in charge of code quality.

## Example Prompts

Try using these prompts to leverage Copilot’s multi-file power:

- `# Update all references to getUserData to use fetchUserData`
- `# Change all CommonJS require statements to ES module imports`
- `# Apply async/await instead of promise chains in all service files`
- `# Replace deprecated helper function calls with the updated version`
- `# Update API routes and adjust the corresponding tests`
- `# Standardize error handling across controllers`

## Workflow

1. **Write a descriptive prompt** in your code editor.
2. **Copilot Edits scans** the project and proposes file-by-file changes.
3. **Review suggestions** as diffs.
4. **Accept, reject, or adjust** each change just like in a pull request.

## Best Practices

- Start with narrowly-scoped prompts for best results.
- Always review diffs to ensure auto-generated changes fit your codebase style and intent.
- Use Copilot Edits for changes that touch multiple files, like renaming functions or modernizing imports, to maximize efficiency.

## Summary

Copilot’s multi-file edit support streamlines wide-ranging refactoring tasks by automating repetitive edits, maintaining code quality, and keeping developers in control of every change.

This post appeared first on "Randy Pagels's Blog". [Read the entire article here](https://www.cooknwithcopilot.com/blog/multi-file-edits-made-simple.html)
