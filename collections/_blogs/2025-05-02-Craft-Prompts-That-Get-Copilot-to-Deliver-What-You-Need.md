---
external_url: https://cooknwithcopilot.com/blog/craft-prompts-that-get-better-results.html
title: Craft Prompts That Get Copilot to Deliver What You Need
author: randy.pagels@xebia.com (Randy Pagels)
feed_name: Randy Pagels's Blog
date: 2025-05-02 00:00:00 +00:00
tags:
- Code Generation
- Contextual Prompting
- Debugging Copilot
- Developer Productivity
- Large Language Models
- Persona Based Prompts
- Prompt Engineering
- Structured Prompt
- Workflow Automation
- AI
- GitHub Copilot
- Blogs
section_names:
- ai
- github-copilot
primary_section: github-copilot
---
Randy Pagels shares techniques for writing prompts that help GitHub Copilot generate more accurate and relevant code. This guide covers structured prompting, providing context, and iterative refinement for best results.<!--excerpt_end-->

## Craft Prompts That Get Better Results

*Authored by Randy Pagels*

### Introduction

When using GitHub Copilot, sometimes a simple prompt is sufficient. However, if Copilot's suggestions aren't quite what you need, it's important to refine your prompting approach. By being specific, providing context, and guiding Copilot as you would a junior developer, you can obtain results that are better aligned with your goals.

---

### ProTip: Use Structured Prompts

Large language models, like Copilot, respond more accurately when you clearly specify your requirements. You don't need to use complex language—step-by-step instructions and focus are more effective. Consider the following formula:

**[Goal] + [Tech/Context] + [Constraints or Style]**

#### Effective Prompt Examples

1. **Task + Context**
   - `# Write a Python function that parses a CSV and returns rows where age > 30`
   - *Direct, goal-focused, and clear.*

2. **Step-by-Step Structure**
   - `# Step-by-step:`
   - `# 1. Read a CSV file`
   - `# 2. Filter rows by a column value`
   - `# 3. Return filtered list`
   - *Results in organized, cleaner code.*

3. **Style or Approach Guidance**
   - `// Write a TypeScript function that uses map/filter instead of a for loop`
   - *Guides the coding style of Copilot's output.*

4. **Persona-Based Prompt**
   - `// Act like a senior front-end engineer writing a reusable button component in React with TypeScript and Tailwind`
   - *Helps Copilot adopt conventions and patterns an expert would use.*

5. **Context-Framing Prompt**
   - `# This YAML defines a GitHub Actions workflow that runs tests on every push to the main branch using Node.js 18`
   - *A summary helps Copilot generate boilerplate that fits your need.*

---

### Prompt Debugging Tip

If Copilot's output isn't right, try rewording the comment with more details or break the task down into steps. Approaching the process as if you're teaching a new teammate—providing clear, specific instructions—usually leads to more desirable results.

---

### Quick Takeaway

When Copilot doesn't deliver what you expect, it isn't stuck; it may just need clearer, more structured prompts. By enhancing the specificity, adding structure, and iteratively refining your instructions, you can noticeably improve the quality of Copilot's generated output.

This post appeared first on "Randy Pagels's Blog". [Read the entire article here](https://cooknwithcopilot.com/blog/craft-prompts-that-get-better-results.html)
