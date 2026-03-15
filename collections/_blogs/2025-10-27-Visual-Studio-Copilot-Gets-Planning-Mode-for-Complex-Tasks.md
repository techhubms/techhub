---
external_url: https://devops.com/visual-studio-copilot-gets-planning-mode-for-complex-tasks/
title: Visual Studio Copilot Gets Planning Mode for Complex Tasks
author: Tom Smith
feed_name: DevOps Blog
date: 2025-10-27 09:14:43 +00:00
tags:
- Agentic Development
- AI Assisted Coding
- AI Assisted Development
- AI Coding Tools
- AI For Developers
- Benchmarking
- Business Of DevOps
- Claude Sonnet 4
- Coding Productivity
- Copilot AI
- Copilot Planning
- Copilot Planning Benchmark
- Copilot Planning Feature
- Copilot Preview
- DevOps AI Tools
- DevOps Tools
- GPT 5
- GPT 5 Copilot
- Intent Driven Development
- Microsoft Copilot
- Microsoft Developer Tools
- Multistep Task Management
- Planning Driven Development
- Planning Feature
- Social Facebook
- Social LinkedIn
- Social X
- Software Automation
- Software Development Automation
- Structured Planning
- SWE Bench
- SWE Bench Results
- Visual Studio Copilot
- VS
- Workflow Efficiency
- AI
- DevOps
- GitHub Copilot
- Blogs
- .NET
section_names:
- ai
- dotnet
- devops
- github-copilot
primary_section: github-copilot
---
Tom Smith introduces Microsoft’s new Planning feature for Visual Studio Copilot, highlighting how it improves AI-powered development workflows for complex, multistep tasks.<!--excerpt_end-->

# Visual Studio Copilot Gets Planning Mode for Complex Tasks

**Author: Tom Smith**

Microsoft has released the Planning feature in Visual Studio Copilot, now in public preview for Visual Studio 2022 version 17.14. This feature is designed to help developers manage multistep coding tasks more efficiently and transparently.

## What is Planning in Visual Studio Copilot?

The Planning feature enables Copilot to break down complex tasks into structured stages, rather than responding to an entire multistep prompt at once. When a developer requests a substantial change, Copilot evaluates if it requires a plan. For such tasks, the feature creates a markdown file outlining:

- The main objective
- Subtasks and research steps
- Progress tracking information

This file lives in your temporary directory by default (`%TEMP%\VisualStudio\copilot-vs\`) but can be moved to your repository for longer-term use.

## How Does Planning Work?

- **Triggering Planning:** Simple requests get quick answers; complex or multistep requests prompt Copilot to work in stages using planning.
- **Plan File:** The system transparently shows each step, letting developers follow the AI's process and review or adjust as needed before execution.
- **Hierarchical Planning:** Copilot uses hierarchical and closed-loop planning, starting with high-level strategies before executing each step. Plans update as Copilot learns more or encounters problems.
- **Progress Tracking:** Developers can monitor task progress in the plan file, improving visibility.

**Note:** Editing the plan during execution requires pausing Copilot, updating, and then restarting. Mid-execution edits may not be applied immediately; Microsoft is working on making this smoother.

## Benchmark Results

- Microsoft tested Planning on the SWE-bench benchmark for AI coding tools.
- Models like GPT-5 and Claude Sonnet 4 showed 15% higher success rates and 20% more tasks resolved with Planning enabled, especially with complex, multistep problems.
- Testing is ongoing with additional models to confirm these improvements are consistent across scenarios.

## Impact for DevOps Teams

Complex DevOps work involves interconnected tasks: updating pipelines, refactoring code, or infrastructure migration. Standard AI assistants can lose context in these scenarios, but Planning improves reliability through structured, transparent workflows. Developers can review and influence the plan before it’s executed, leading to fewer surprises and improved outcomes.

## Practical Developer Considerations

- **Preview Limitations:** Planning is in preview, so plan files are temporarily stored and manual actions are needed for persistent storage.
- **Editing:** Workflow adjustments might be needed for mid-plan changes.
- **Feedback:** Microsoft is actively soliciting developer feedback to improve Planning’s usability, formatting, and adaptability.

## Analyst Perspective

Mitch Ashley (The Futurum Group) describes Planning as a key milestone for Agentic and Intent-Driven Development. The feature allows developers to set high-level goals while Copilot handles execution details, changing the interaction model from reactive prompts to structured plans and outcome-based workflows.

## Roadmap and Next Steps

Microsoft plans to further enhance Planning with improved caching, smarter reasoning, and deeper integration with project context.

**Recommendation:** If your projects involve multistep coding or DevOps workflows, trial the Planning feature to see if it improves efficiency and project transparency. Start with non-critical tasks to assess the benefits in your environment.

## References & Resources

- [Microsoft Copilot documentation](https://learn.microsoft.com/en-us/copilot/)
- [SWE-bench Benchmark](https://github.com/princeton-nlp/SWE-bench)
- [Visual Studio Copilot official page](https://learn.microsoft.com/en-us/visualstudio/ide/copilot)

This post appeared first on "DevOps Blog". [Read the entire article here](https://devops.com/visual-studio-copilot-gets-planning-mode-for-complex-tasks/)
