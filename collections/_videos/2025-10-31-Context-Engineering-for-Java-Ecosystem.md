---
layout: "post"
title: "Context Engineering for Java Ecosystem"
description: "In this episode, Ayan Gupta and Bruno Borges explore how context engineering can transform Java development workflows with GitHub Copilot in Visual Studio Code. The session demonstrates using custom instructions, chat modes, and prompt files to control AI code assistance, enabling project-wide standards and reducing repetitive prompting. Bruno covers how to set context for frameworks (including Spring and Hibernate), set up reusable prompts, and leverage 'planner' chat modes for architecture planning. Developers will learn practical strategies to fully harness Copilot's capabilities in large Java projects."
author: "Microsoft Developer"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.youtube.com/watch?v=kSElHY8MWwc"
viewing_mode: "internal"
feed_name: "Microsoft Developer YouTube"
feed_url: "https://www.youtube.com/feeds/videos.xml?channel_id=UCsMica-v34Irf9KVTh6xx-g"
date: 2025-10-31 16:01:27 +00:00
permalink: "/videos/2025-10-31-Context-Engineering-for-Java-Ecosystem.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["AI", "AI Assisted Development", "AIAssisted", "Bad Practice Analysis", "Chat Modes", "ChatModes", "Coding", "Context Engineering", "ContextEngineering", "Copilot Chat", "Custom Instructions", "CustomInstructions", "Developer Productivity", "DeveloperProductivity", "GitHub Copilot", "Hibernate", "Java", "Java 25", "Java25", "JavaDevelopment", "Planner Mode", "Prompt Engineering", "PromptEngineering", "Spring Framework", "Videos", "VS Code"]
tags_normalized: ["ai", "ai assisted development", "aiassisted", "bad practice analysis", "chat modes", "chatmodes", "coding", "context engineering", "contextengineering", "copilot chat", "custom instructions", "custominstructions", "developer productivity", "developerproductivity", "github copilot", "hibernate", "java", "java 25", "java25", "javadevelopment", "planner mode", "prompt engineering", "promptengineering", "spring framework", "videos", "vs code"]
---

Microsoft Developer, featuring Ayan Gupta and Bruno Borges, showcases advanced context engineering techniques in Java using GitHub Copilot and Visual Studio Code to streamline developer workflows and AI interactions.<!--excerpt_end-->

{% youtube kSElHY8MWwc %}

# Context Engineering for Java Ecosystem

**Presented by Ayan Gupta & Bruno Borges | Microsoft Developer**

## Introduction

Context is crucial when it comes to AI-assisted development. In this episode, Bruno Borges demonstrates how context engineering enables you to get the most out of GitHub Copilot in large Java projects. The right context—just like the right coffee brewing method—makes a huge difference in the quality of AI-generated assistance.

## What Is Context Engineering?

Context engineering is about strategically providing information so that AI tools like Copilot generate relevant, high-quality output. Instead of issuing repetitive prompts, developers can predefine standards and intentions, shaping Copilot's code suggestions project-wide.

## Three VS Code Features for Better AI Context

1. **Custom Instructions**  
   - Use copilot-instructions.md to set global or project standards (e.g., "always use Java 25 syntax" or "include humor").
   - Automatically applies guidance to Copilot’s output.

2. **Chat Modes**  
   - Create specialized chat modes like 'planner' that focus on tasks such as generating implementation plans or code architectures rather than code.
   - Helps teams plan features before coding them.

3. **Prompt Files**  
   - Save reusable prompts for common tasks.
   - Example shown: Quickly trigger a prompt to analyze code for bad practices without leaving VS Code or copy-pasting from old notes.

## Framework-Specific Instructions

- Use the instructions folder to provide context-aware guidance for specific frameworks (e.g., Hibernate, Spring) that activates only when needed based on the file being edited.

## Java 25 Language Features

- Demonstrations include how to guide Copilot to use the latest Java 25 features by including language spec info in your contextual instructions.

## Practical Workflow Benefits

- Eliminates repetitive prompting
- Improves code consistency and quality
- Ensures that Copilot's recommendations align with project standards

## Demonstrated Tasks

- Setting up a Java application in VS Code
- Exploring Java 25 new features
- Using custom chat modes for planning
- Detecting code issues via prompt files
- Adding project-specific instructions for frameworks and language specs

## Best Practices for Context Engineering

- Always define project-wide Copilot instructions at the start.
- Use chat modes to separate code writing from planning tasks.
- Store and reuse prompt files for consistent analysis and refactoring.
- Update instruction files as your tech stack evolves (e.g., adopting new Java versions).

## Additional Resources

- [Java and AI for Beginners](https://aka.ms/JavaAndAIForBeginners)

---

Harnessing context engineering will help you maximize AI assistance and streamline your Java development process in VS Code.
