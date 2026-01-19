---
external_url: https://www.reddit.com/r/GithubCopilot/comments/1mkqvpn/vibe_debugging_gpt5_is_worse_than_o3gemini25_pro/
title: 'Comparing Copilot AI Models for C# Bug Fixing: GPT-5, Gemini 2.5 Pro, and Others'
author: Sea-Key3106
viewing_mode: external
feed_name: Reddit Github Copilot
date: 2025-08-08 09:45:28 +00:00
tags:
- .NET Bug Fixing
- AI Assisted Development
- AI Code Completion
- C#
- Claude Sonnet 4
- Copilot Model Comparison
- Debugger
- Dynamic Delegate
- Gemini 2.5 Pro
- GPT 5
- Inheritance Hierarchy
- IsAssignableFrom
- O3
- O3 High
section_names:
- ai
- coding
- github-copilot
---
Sea-Key3106 discusses a direct comparison between GPT-5, Gemini 2.5 Pro, Claude Sonnet 4, O3, and O3 High models within GitHub Copilot for a challenging C# bug, highlighting each model's effectiveness.<!--excerpt_end-->

# Comparing Copilot AI Models for C# Bug Fixing: GPT-5, Gemini 2.5 Pro, and Others

*Author: Sea-Key3106*

In this post, I share my experience debugging a tricky C# delegate issue using various AI models through GitHub Copilot and related tools. The problem involved dynamic invocation of a delegate and an overlooked inheritance hierarchy check for one of the parameters.

## The Problem

- **Scenario**: Bug caused by calling a delegate dynamically in C# without checking inheritance hierarchy of a parameter.
- **Goal**: Have an AI assistant spot and fix the bug, ideally with robust, elegant code.

## Models Tested

I used the same prompt and code file for each AI model:

1. **GPT-5 (in Copilot)**
   - Failed to find the root cause. Made unrelated code changes.
2. **Gemini 2.5 Pro (in Copilot)**
   - Successfully identified and fixed the root cause as well as a similar issue in the same file.
   - Downside: Kept editing the file for over 10 minutes, requiring manual intervention.
3. **Claude Sonnet 4 (in Copilot)**
   - Successfully found and fixed both the main and a related bug.
   - Used explicit inheritance type-checking rather than `IsAssignableFrom`.
4. **O3 (in windsurf)**
   - Found and fixed the main bug but missed a similar related issue.
5. **O3 High (in windsurf)**
   - Detected and fixed both issues, merging similar conditions efficiently in an `if` clause.

## Observations

- The bug required understanding C#'s inheritance checking, specifically around dynamic delegates.
- **GPT-5** underperformed in this scenario, failing to address the relevant code section.
- **Gemini 2.5 Pro** and **Claude Sonnet 4** were both successful, with minor differences in approach.
- **O3 High** produced the most comprehensive and robust fix.
- It's unclear if the limitations observed are due to the AI model itself or Copilot's implementation.

## Takeaway

Choosing the right AI model in GitHub Copilot can significantly impact code quality and developer productivity, especially for nuanced issues like inheritance in C#. This single-case comparison highlights practical strengths and weaknesses to consider.

## Further Exploration

I plan to test more with Claude Sonnet 4 in Copilot to see if its explicit approach to inheritance checking leads to consistently elegant solutions.

This post appeared first on "Reddit Github Copilot". [Read the entire article here](https://www.reddit.com/r/GithubCopilot/comments/1mkqvpn/vibe_debugging_gpt5_is_worse_than_o3gemini25_pro/)
