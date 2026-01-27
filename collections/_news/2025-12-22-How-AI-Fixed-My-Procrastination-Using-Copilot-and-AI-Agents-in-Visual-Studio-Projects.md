---
external_url: https://devblogs.microsoft.com/visualstudio/how-ai-fixed-my-procrastination/
title: 'How AI Fixed My Procrastination: Using Copilot and AI Agents in Visual Studio Projects'
author: Mads Kristensen
feed_name: Microsoft VisualStudio Blog
date: 2025-12-22 15:00:38 +00:00
tags:
- .NET 10
- Agent Mode
- Agentic Development
- AI Agents
- C#
- CI/CD
- Cloud Agent
- Color Themes
- Copilot
- Copilot Chat
- Extension Development
- GitHub Actions
- Language Service
- NuGet Package
- Parser Development
- Static Website
- TOON Language
- VS
section_names:
- ai
- coding
- devops
- github-copilot
primary_section: github-copilot
---
Mads Kristensen shares how using Copilot and AI agents in Visual Studio accelerated his web development, created a new programming language extension, and enabled efficient color theme creation—all while overcoming procrastination.<!--excerpt_end-->

# How AI Fixed My Procrastination: Using Copilot and AI Agents in Visual Studio Projects

**Author: Mads Kristensen**

Many developers struggle to get started on challenging projects, and I was no different. For a long time, I put off building a website, making a new language extension for Visual Studio, and experimenting with new color themes—everything felt overwhelming or too time-consuming.

A long holiday weekend gave me a chance to dive into project work, this time with an AI assistant: Copilot in Visual Studio. Here's how AI agent technology and Copilot fundamentally changed the way I work—allowing me to complete three different projects far faster than I'd previously imagined.

---

## Project 1: Turning an Automation Book into a Static Website

After writing "The Automated Home," I realized the book format was limiting for home automation recipes. With a copy of my manuscript exported to `.txt`, I opened the folder in Visual Studio and started a Copilot chat:

> *“Turn this book into a website that should function like a book of recipes for home automation.”*

Copilot proceeded to create the static website, incrementally generating files. While the raw output needed design improvements, prompting Copilot iteratively refined both CSS and organization. The resulting website ([homeautomationcookbook.com](https://www.homeautomationcookbook.com)) materialized about 5x faster than manual development, liberating my time for other activities.

---

## Project 2: A TOON Language Visual Studio Extension

I often build Visual Studio extensions, but developing a custom language service for the [TOON language](https://github.com/toon-format/toon) was a new challenge, involving a parser and tokenizer robust enough for real-world usage. To automate this, I provided Copilot with the TOON language specification and requested it to generate code suitable for a Visual Studio language service.

Copilot, via the Visual Studio Cloud Agent, quickly created an initial implementation, posting a pull request on GitHub within 20 minutes. Further refinements were made through agentic and manual workflows. Copilot also assisted with unit test generation, identified performance bottlenecks (using the Profiler Agent), and helped surface security improvements.

Continuous integration and delivery were set up automatically with GitHub Actions, and ultimately, the [ToonTokenizer](https://www.nuget.org/packages/ToonTokenizer/) NuGet package and the [extension](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.Toon) itself were published.

---

## Project 3: Designing Visual Studio Color Themes with Copilot

Colour theme development for Visual Studio was unfamiliar territory for me, especially when mapping colours accurately from screenshots to theme files. With Copilot's help, I extracted code and environment color tokens from Solarized themes and updated XML theme files. While iterative manual adjustments were necessary, Copilot accelerated the process considerably. Ultimately, I added six new color themes to my [Blue Steel extension](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.BlueSteel).

---

## Lessons Learned: Agentic Development in Practice

AI, and Copilot in particular, helped me start and rapidly progress on projects I would have otherwise left idle. The biggest benefits were:

- **Parallel workstreams**: While Copilot or the agent worked in the background, I could focus on other engineering tasks.
- **Reduced friction**: Prompt-based development broke tasks into manageable, iterative steps.
- **Extensibility and automation**: Integrations with GitHub Actions and NuGet streamlined deployment.
- **Security and quality**: Automated test and vulnerability detection raised code confidence.

Manual work and fine-tuning were still critical—AI isn't a replacement for expertise, but serves as a powerful productivity accelerator.

If you want to try agentic development or need a jump start on overdue work, I recommend leveraging Copilot and AI features in Visual Studio. Clone repos, tweak themes, and use the agent cloud—your future self will thank you.

This post appeared first on "Microsoft VisualStudio Blog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/how-ai-fixed-my-procrastination/)
