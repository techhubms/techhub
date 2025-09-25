---
layout: "post"
title: "Avoid These Common Copilot Prompts: How to Get Better Results with Specific Guidance"
description: "Randy Pagels explores why generic prompts like 'write a function' limit GitHub Copilot’s effectiveness and provides actionable tips for crafting prompts that yield more accurate and helpful results. The post highlights the value of context, specificity, and practical examples for coding with Copilot."
author: "randy.pagels@xebia.com (Randy Pagels)"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://cooknwithcopilot.com/blog/avoid-these-common-prompts.html"
viewing_mode: "external"
feed_name: "Randy Pagels's Blog"
feed_url: "https://cooknwithcopilot.com/rss.xml"
date: 2025-07-04 00:00:00 +00:00
permalink: "/2025-07-04-Avoid-These-Common-Copilot-Prompts-How-to-Get-Better-Results-with-Specific-Guidance.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["AI", "Best Practices", "Bug Fixing", "Code Generation", "Code Review", "Coding", "Developer Productivity", "GitHub Copilot", "Posts", "Programming Languages", "Prompt Engineering", "Software Development", "Unit Testing"]
tags_normalized: ["ai", "best practices", "bug fixing", "code generation", "code review", "coding", "developer productivity", "github copilot", "posts", "programming languages", "prompt engineering", "software development", "unit testing"]
---

Randy Pagels discusses why vague prompts like 'write a function' often produce suboptimal results in GitHub Copilot, and shares specific strategies and examples for crafting effective, context-rich prompts.<!--excerpt_end-->

# Avoid These Common Prompts

*Posted by Randy Pagels on Jul 4, 2025*

## Introduction

Not all Copilot prompts are created equal. Using generic, overused prompts can limit GitHub Copilot’s potential, resulting in bland, inaccurate, or even incorrect code suggestions. This guide reviews which prompts to avoid and how you can markedly improve results by being specific and providing context.

---

## Prompts to Avoid

Many developers ask Copilot to help in ways that are too vague, such as:

- **“Write a function”** (with no additional detail)
- **“Fix this code”** (without context or code provided)
- **“Make this better”**
- **“Generate unit tests”** (without requirements or actual code reference)
- **“Explain this code”** (with no file, method, or details provided)

While Copilot may respond to these, the responses often lack quality or miss the point. Copilot performs best with well-defined context.

---

## Pro Tip: Be Specific!

To make the most of Copilot, be detailed in your prompts. Here are ways to provide the needed context:

- **Specify Language and Framework:**
  - Example: “Write a Python function to…” or “In C#, create a method in ASP.NET Core that…”
- **Add Constraints:**
  - Example: “…using recursion”, “…that handles errors”, “…for all edge cases”
- **Provide Examples and Requirements:**
  - Show Copilot expected input and output: “Given an array of user records, filter…”
  - Describe your intent clearly
- **For Bug Fixes:**
  - Paste the error message, stack trace, and the buggy code block for targeted suggestions.

---

## Better Prompt Examples

- “In Python, write a function to find all unique values in a list, ignoring case.”
- “Fix the following code to handle division by zero errors.”
- “Generate unit tests for my ‘parseInvoice’ function to cover edge cases.”

---

## Key Takeaway

The more context you give Copilot, the more accurately it can interpret your intent and offer smart, relevant code or explanations. Avoid the common traps of short, vague prompts—describe your problem, constraints, and intent, and benefit from Copilot's capabilities to their fullest.

This post appeared first on "Randy Pagels's Blog". [Read the entire article here](https://cooknwithcopilot.com/blog/avoid-these-common-prompts.html)
