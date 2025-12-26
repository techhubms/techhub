---
layout: "post"
title: "Craft Prompts That Get Better Results"
description: "This guide helps developers refine their prompting strategies for GitHub Copilot. It explains why detailed, contextual, and structured prompts lead to more accurate and usable code suggestions. Through practical formulas and real-world examples, developers will learn how to guide Copilot as they would a junior team member, improving their workflow and the output's relevance."
author: "randy.pagels@xebia.com (Randy Pagels)"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.cooknwithcopilot.com/blog/craft-prompts-that-get-better-results.html"
viewing_mode: "external"
feed_name: "Randy Pagels's Blog"
feed_url: "https://www.cooknwithcopilot.com/rss.xml"
date: 2025-05-02 00:00:00 +00:00
permalink: "/posts/2025-05-02-Craft-Prompts-That-Get-Better-Results.html"
categories: ["AI", "GitHub Copilot"]
tags: ["AI", "Best Practices", "Code Quality", "Developer Workflow", "GitHub Actions", "GitHub Copilot", "Posts", "Prompt Engineering", "Python", "React", "Structured Prompts", "Tailwind CSS", "TypeScript"]
tags_normalized: ["ai", "best practices", "code quality", "developer workflow", "github actions", "github copilot", "posts", "prompt engineering", "python", "react", "structured prompts", "tailwind css", "typescript"]
---

Randy Pagels offers actionable advice for developers on how to write effective prompts that significantly improve GitHub Copilot’s coding suggestions, emphasizing clarity and structure.<!--excerpt_end-->

# Craft Prompts That Get Better Results

*Posted on May 23, 2025 by Randy Pagels*

When your prompts for GitHub Copilot don’t deliver what you expect, it’s time to step up your prompt-writing game. This guide covers the essentials of making your requests as clear and effective as possible—think of guiding Copilot like mentoring a junior developer.

## Why Prompt Clarity Matters

Large language models respond best to instructions that are unambiguous and well-structured. Instead of short, vague comments, give Copilot specific goals, context, and constraints. It’s not about sounding smart—it’s about being precise.

## Practical Formula for Strong Prompts

**[Goal] + [Tech/Context] + [Constraints or Style]**

This formula can help shape your prompts for maximum clarity.

### Examples

- **Task + Context**
  - `# Write a Python function that parses a CSV and returns rows where age > 30`
    - Direct and focused.

- **Step-by-Step Structure**
  - `# Step-by-step: 1. Read a CSV file 2. Filter rows by a column value 3. Return filtered list`
    - Breaking the problem into steps often yields cleaner code.

- **Style or Approach Guidance**
  - `// Write a TypeScript function that uses map/filter instead of a for loop`
    - Guides Copilot toward your preferred coding style or technique.

- **Persona-Based Prompt**
  - `// Act like a senior front-end engineer writing a reusable button component in React with TypeScript and Tailwind`
    - Setting a role helps Copilot align with real-world conventions.

- **Context-Framing Prompt**
  - `# This YAML defines a GitHub Actions workflow that runs tests on every push to the main branch using Node.js 18`
    - Summarizing the file’s purpose helps Copilot generate the right boilerplate.

## Debugging Your Prompts

If Copilot’s output isn’t right, try rewording your comment or add steps one at a time. The more specific and structured your instructions, the better Copilot performs.

## Quick Takeaway

When Copilot isn’t hitting the mark, structure and context will unlock better results. Treat your prompt as a set of clear instructions for a new teammate, and you’ll see more relevant, high-quality code suggestions.

This post appeared first on "Randy Pagels's Blog". [Read the entire article here](https://www.cooknwithcopilot.com/blog/craft-prompts-that-get-better-results.html)
