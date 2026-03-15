---
external_url: https://github.blog/changelog/2025-12-16-c-code-editing-tools-for-github-copilot-in-public-preview
title: C++ Code Editing Tools for GitHub Copilot in Visual Studio 2026 Insiders Public Preview
author: Allison
feed_name: The GitHub Blog
date: 2025-12-16 18:28:30 +00:00
tags:
- Agent Mode
- C++
- Class Hierarchies
- Code Editing
- Code Refactoring
- Copilot
- Copilot Chat
- Developer Tools
- Function Call Tracing
- Multi File Support
- Public Preview
- Semantic Analysis
- Symbol Awareness
- Tool Integration
- VS
- AI
- GitHub Copilot
- News
- .NET
section_names:
- ai
- dotnet
- github-copilot
primary_section: github-copilot
---
Allison details new C++ code editing tools for GitHub Copilot in Visual Studio 2026 Insiders, enabling deep code analysis and multi-file editing to improve developer workflows.<!--excerpt_end-->

# C++ Code Editing Tools for GitHub Copilot in Visual Studio 2026 Insiders

GitHub Copilot now offers powerful C++ code editing tools as part of the Visual Studio 2026 Insiders public preview. These enhancements provide developers with:

- **Deep Symbol Awareness**: Copilot can view all references for any symbol, understand type, declaration, scope, and access rich metadata across large codebases.
- **Multi-file Editing Support**: Edit and refactor code spanning multiple files reliably.
- **Class and Inheritance Visualization**: Visualize class inheritance hierarchies to better understand object relationships.
- **Function Call Chain Tracing**: Follow function call chains for improved code comprehension.

## Getting Started

1. Open your C++ project in Visual Studio 2026 Insiders.
2. Enable the C++ code editing tools via **Tools > Options > GitHub > Copilot**.
   - You may need to reopen your solution after enabling the tools.
3. Launch Copilot Chat and activate the specific C++ tools using the **Tools** icon.

![GitHub Copilot C++ tools in Visual Studio agent mode](https://github.com/user-attachments/assets/19c78848-466f-4bac-be64-08347ef1eb68)

Once configured, GitHub Copilot leverages this toolset to provide more context-aware code completions and suggestions. For example, it can help update all instances of a function when parameters change, assisting with large code refactors and feature additions. The tools also support symbol information retrieval and advanced code insights to streamline complex changes.

![C++ tools fetching symbol information and references](https://github.com/user-attachments/assets/3b98c50c-38a6-4d35-88de-15c394cb1c1c)

**Tip**: For best results, pick a model optimized for tool calling and provide explicit symbols in prompts to receive precise suggestions from Copilot.

## Feedback

Your input is welcome—use the Visual Studio feedback icon to report bugs or suggest improvements to the C++ Copilot experience.

![Visual Studio feedback menu](https://github.com/user-attachments/assets/b9738f4b-7bfd-47a6-8c0d-9b8e911f847d)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-16-c-code-editing-tools-for-github-copilot-in-public-preview)
