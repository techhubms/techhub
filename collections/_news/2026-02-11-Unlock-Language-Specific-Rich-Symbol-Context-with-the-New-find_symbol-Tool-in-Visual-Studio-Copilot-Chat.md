---
layout: "post"
title: "Unlock Language-Specific Rich Symbol Context with the New find_symbol Tool in Visual Studio Copilot Chat"
description: "This news update introduces the find_symbol tool for Agent Mode in Visual Studio Copilot Chat. It enables symbol-level code navigation and reasoning using enterprise-grade language services in supported languages, helping developers refactor, enhance, and explore large codebases more efficiently."
author: "Sinem Akinci, Hannah Hong (SHE/HER)"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/visualstudio/unlock-language-specific-rich-symbol-context-using-new-find_symbol-tool/"
viewing_mode: "external"
feed_name: "Microsoft VisualStudio Blog"
feed_url: "https://devblogs.microsoft.com/visualstudio/feed/"
date: 2026-02-11 15:29:15 +00:00
permalink: "/2026-02-11-Unlock-Language-Specific-Rich-Symbol-Context-with-the-New-find_symbol-Tool-in-Visual-Studio-Copilot-Chat.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["Agent Mode", "AI", "API Refactoring", "C#", "C++", "Code Navigation", "Codebase Management", "Coding", "Copilot Chat", "Developer Productivity", "Find Symbol", "GitHub Copilot", "IDE Tools", "Language Server Protocol", "News", "Refactoring", "TypeScript", "VS"]
tags_normalized: ["agent mode", "ai", "api refactoring", "csharp", "cplusplus", "code navigation", "codebase management", "coding", "copilot chat", "developer productivity", "find symbol", "github copilot", "ide tools", "language server protocol", "news", "refactoring", "typescript", "vs"]
---

Sinem Akinci and Hannah Hong present the new find_symbol tool for Agent Mode in Visual Studio Copilot Chat, helping developers improve code navigation and refactoring with language-aware symbol analysis.<!--excerpt_end-->

# Unlock Language-Specific Rich Symbol Context with the New find_symbol Tool in Visual Studio Copilot Chat

Refactoring at scale in large codebases is traditionally slow and susceptible to errors, relying on manual searches and repeated edits across many files. Modern developer workflows require fast, precise tools to help understand and update code reliably.

The Visual Studio Copilot Chat Agent Mode now addresses this need with the introduction of the **find_symbol** tool. This feature brings rich, language-aware symbol information—such as function, class, interface, and variable definitions—to Copilot-powered workflows, leveraging enterprise-grade language services found in modern IDEs.

## What Is find_symbol?

The find_symbol tool allows the Copilot Agent Mode to:

- Find all references to a specific symbol throughout a codebase
- Understand detailed symbol metadata, including type, declaration, implementation, and scope

This symbol-level awareness goes beyond traditional file or text search by enabling accurate code reasoning and robust refactorings for developers working in large projects.

## Language and Tool Support

- Available now in the [Visual Studio 2026 Insiders version 18.4](https://visualstudio.microsoft.com/insiders/)
- Supports languages like C++, C#, Razor, TypeScript, and any others compatible with Language Server Protocol (LSP) extensions
- Works with AI models that support tool-calling (see [GitHub Docs: AI model comparison](https://docs.github.com/copilot/reference/ai-models/model-comparison))

## Example Scenarios

### 1. Adding Functionality to Existing Code

When applications evolve, you may need to enhance existing functions (e.g., adding new logging or metrics) without disrupting old behavior. find_symbol helps agents quickly locate all references to a function or method, ensuring that changes are thorough and consistent across the codebase.

### 2. API Refactoring

Strengthening or changing an API demands a complete map of all usages. find_symbol provides this insight, distinguishing call paths and helping the agent suggest safe, accurate updates with minimal risk of breaking downstream code.

Concrete demonstrations are provided using [bullet3](https://github.com/bulletphysics/bullet3/), an open-source C++ physics simulation engine, to showcase how the tool works in practice.

## Community and Feedback

The Visual Studio team encourages developers to share their experiences and suggestions:

- Report bugs: [Report a Problem](https://learn.microsoft.com/visualstudio/ide/how-to-report-a-problem-with-visual-studio?view=vs-2022)
- Suggest features: [Developer Community](https://developercommunity.microsoft.com/VisualStudio/suggest)

Stay up to date through official Visual Studio channels on [YouTube](https://www.youtube.com/@visualstudio), [X](https://x.com/VisualStudio), [LinkedIn](https://www.linkedin.com/showcase/microsoft-visual-studio/), [Twitch](https://www.twitch.tv/visualstudio), and [Microsoft Learn](https://learn.microsoft.com/en-us/visualstudio/?view=vs-2022).

---

By empowering Copilot Agent Mode with language-aware code understanding, find_symbol enables more robust refactoring and navigation in complex projects—helping developers achieve greater productivity and confidence when evolving their codebases.

This post appeared first on "Microsoft VisualStudio Blog". [Read the entire article here](https://devblogs.microsoft.com/visualstudio/unlock-language-specific-rich-symbol-context-using-new-find_symbol-tool/)
