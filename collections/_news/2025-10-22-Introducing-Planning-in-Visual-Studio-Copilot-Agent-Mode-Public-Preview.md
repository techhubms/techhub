---
layout: post
title: 'Introducing Planning in Visual Studio: Copilot Agent Mode Public Preview'
author: Rhea Patel
canonical_url: https://devblogs.microsoft.com/visualstudio/introducing-planning-in-visual-studio-public-preview/
viewing_mode: external
feed_name: Microsoft VisualStudio Blog
feed_url: https://devblogs.microsoft.com/visualstudio/feed/
date: 2025-10-22 15:54:18 +00:00
permalink: /github-copilot/news/Introducing-Planning-in-Visual-Studio-Copilot-Agent-Mode-Public-Preview
tags:
- Agent Mode
- AI
- Claude Sonnet 4
- Coding
- Copilot
- Copilot Chat
- Copilot Planning
- Developer Tools
- GitHub Copilot
- GPT 5
- News
- Planning
- Planning Mode
- Prompt Engineering
- Public Preview
- Software Engineering
- Spec Driven Development
- Structured Workflow
- Task Automation
- Transparent AI
- VS
section_names:
- ai
- coding
- github-copilot
---
Rhea Patel introduces the Planning feature in Visual Studio's Copilot Agent Mode, enabling developers to structure and track complex coding tasks for a more reliable, transparent workflow.<!--excerpt_end-->

# Introducing Planning in Visual Studio: Copilot Agent Mode Public Preview

The Copilot Agent Mode in Visual Studio 2022 now features Planning—a system designed to help developers manage larger or more complex coding tasks. Rather than relying on quick, one-off prompts, Planning gives Copilot a visible, structured path it follows and updates as the work progresses, keeping developers in control at every step.

## Key Features of Planning

- **Structured Plans for Big Tasks:** Copilot now breaks down multi-step tasks by researching your codebase and generating a plan, which it executes step by step and refines along the way.
- **Markdown-Based Plan Files:** Each plan is written to a temporary markdown file (e.g. `%TEMP%\VisualStudio\copilot-vs\`), outlining the task, research steps, and progress. To reuse or persist a plan, developers can add it to their source control or vote for expanded storage options.
- **Transparent Process:** Progress is tracked directly in the plan file, making it clear which tasks have completed and which are next.
- **Mid-Execution Edits:** While plans can be edited during execution, updates may not always take effect until the current response is stopped and restarted. The team is working to improve live editing in the future.

> Watch a brief demo: [Planning Quick Demo](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2025/10/Planning-quick-quick-1.mp4)

## How It Works

When handling a prompt, Copilot distinguishes between simple and multi-step tasks. For complex requests, it switches into Planning, creates a plan, and continuously updates it as it works through each step. The system is influenced by planning research techniques such as hierarchical and closed-loop planning, which make the process both structured and dynamically adaptable.

### Workflow Details

- **Temporary storage:** Plans are by default saved as temporary files, but you can manage them for reuse.
- **Editing plans:** Edit the plan content if needed, but remember to restart Copilot’s response to apply changes.
- **Feedback and improvement:** Share your experience through the [Visual Studio Developer Community](https://developercommunity.visualstudio.com/t/PlanningTo-dos-in-Copilot-Chat/10951150).

## Impact on Developer Experience

- **Predictability & Transparency:** Structured plans clarify what Copilot is doing at every step, reducing ambiguity in how complex changes are made.
- **Improved Task Completion:** Mentioned improvements in the SWE-bench benchmark show approximately 15% higher success rate and 20% more tasks resolved for large, multi-stage tasks with models like GPT-5 and Claude Sonnet 4 in Planning mode.

## What’s Next

The Planning feature remains in preview as the team gathers developer feedback and refines the system. Look forward to smarter context handling, improved mid-execution updates, and deeper integration with project structures in the future. Stay informed and help shape the development by participating in discussions and upvoting proposed enhancements.

---

For more details, visit the [official announcement](https://devblogs.microsoft.com/visualstudio/introducing-planning-in-visual-studio-public-preview/) or watch the [demo video](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2025/10/Planning-quick-quick-1.mp4).

This post appeared first on "Microsoft VisualStudio Blog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/introducing-planning-in-visual-studio-public-preview/)
