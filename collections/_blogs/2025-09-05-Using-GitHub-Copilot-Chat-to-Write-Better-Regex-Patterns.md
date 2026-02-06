---
external_url: https://cooknwithcopilot.com/blog/draft-smarter-regex-without-the-headaches.html
title: Using GitHub Copilot Chat to Write Better Regex Patterns
author: randy.pagels@xebia.com (Randy Pagels)
feed_name: Randy Pagels's Blog
date: 2025-09-05 00:00:00 +00:00
tags:
- Agent Mode
- Code Automation
- Code Review
- Coding Productivity
- Copilot Chat
- Developer Tools
- Pattern Matching
- Programming
- Regex
- Regex Testing
- Regular Expressions
- Software Development
- AI
- GitHub Copilot
- Blogs
- .NET
section_names:
- ai
- dotnet
- github-copilot
primary_section: github-copilot
---
Randy Pagels demonstrates how to boost regex productivity in your editor using GitHub Copilot Chat for drafting, explaining, and testing patterns.<!--excerpt_end-->

# Using GitHub Copilot Chat to Write Better Regex Patterns

By Randy Pagels

Regular expressions are powerful tools, but writing them by hand often leads to frustration. This guide shows how GitHub Copilot Chat streamlines the process, saving you time and reducing guesswork.

## Why Use Copilot Chat for Regex?

- **Draft patterns quickly:** Skip manual trial and error—generate working regex examples on demand.
- **Get instant explanations:** Understand complex patterns with clear, inline comments from Copilot.
- **Automate testing:** Prompt Copilot Chat to generate test cases for validating patterns.
- **Improve maintainability:** Use Agent Mode to review and optimize regex usage across your project.

## Practical Workflow

### 1. Generate a Regex From Scratch

Type a prompt like:

```python
# Write a regex that validates an email address with a domain ending in .com, .net, or .org
```

Copilot will return a suitable pattern, significantly reducing the time spent on syntax details.

### 2. Simplify a Complex Pattern

Paste your complex regex and ask Copilot:

```python
# Rewrite this regex to be more readable, and explain each part
```

You'll get a refactored version with comments, so you can understand it at a glance.

### 3. Add Test Cases Automatically

Ask Copilot Chat:

```python
# Generate sample strings that match and do not match this regex
```

This quickly validates your pattern without handcrafting examples.

### 4. Use Agent Mode for Broader Coverage

Audit all regex patterns in your repository:

```
Review all regex patterns in this repo and suggest improvements for readability and maintainability
```

Perfect for large codebases where consistency is important.

## Extra Prompts

- Create a regex that only matches U.S. phone numbers
- Simplify this pattern for matching dates in YYYY-MM-DD format
- Suggest a regex for extracting hashtags from text

## Summary

With Copilot Chat, you can write, explain, and test regular expressions more efficiently—directly in your editor. Whether you need to generate new patterns or improve existing ones, these techniques help you ship high-quality code with less hassle.

This post appeared first on "Randy Pagels's Blog". [Read the entire article here](https://cooknwithcopilot.com/blog/draft-smarter-regex-without-the-headaches.html)
