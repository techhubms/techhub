---
layout: "post"
title: "Microsoft Expands Copilot Features in Visual Studio Code 1.102"
description: "This article examines the major Copilot-centric update to Visual Studio Code (VS Code) version 1.102, detailing new AI-driven capabilities, the general availability of Model Context Protocol (MCP), enhanced Copilot Chat integration, custom instruction support, and emerging developer concerns over AI feature dominance. It discusses technical specifics and implications for developer workflow."
author: "Tim Anderson"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devclass.com/2025/07/14/microsoft-shovels-extra-copilot-features-into-vs-code-amid-dev-complaints-of-more-ai-bloat/"
viewing_mode: "external"
feed_name: "DevClass"
feed_url: "https://devclass.com/feed/"
date: 2025-07-14 16:58:30 +00:00
permalink: "/blogs/2025-07-14-Microsoft-Expands-Copilot-Features-in-Visual-Studio-Code-1102.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["AI", "AI Agent Safety", "AI Bloat", "AI Coding Tools", "AI/ML", "CLI Tools", "Coding", "Copilot", "Copilot Chat", "Custom Instructions", "Developer Experience", "Development", "GitHub Copilot", "IDE Integrations", "LLM Evaluation", "MCP", "MCP Servers", "Microsoft", "Posts", "Terminal Command Auto Approval", "VS Code", "VS Code 1.102"]
tags_normalized: ["ai", "ai agent safety", "ai bloat", "ai coding tools", "aislashml", "cli tools", "coding", "copilot", "copilot chat", "custom instructions", "developer experience", "development", "github copilot", "ide integrations", "llm evaluation", "mcp", "mcp servers", "microsoft", "posts", "terminal command auto approval", "vs code", "vs code 1dot102"]
---

Tim Anderson reports on Microsoft's latest update to Visual Studio Code, focusing on new Copilot and AI-related features, developer reactions, and potential impacts on coding workflows.<!--excerpt_end-->

# Microsoft Expands Copilot Features in Visual Studio Code 1.102

**Author:** Tim Anderson

## Overview

Microsoft has released Visual Studio Code (VS Code) version 1.102, significantly expanding its integration with Copilot and AI-centered capabilities. This update brings the general availability of Model Context Protocol (MCP) support and introduces several new features that deeply embed AI into the developer experience. The majority of headline features in this release are Copilot-related, with only one non-AI addition—middle-click editor scrolling.

## Key New Features

- **MCP Support (General Availability):**
  - A dedicated "MCP Servers" section now appears in the Extensions view, listing MCP servers and offering a curated install experience (currently with 37 servers).
  - The elicitation feature allows servers to prompt developers directly through their clients.

- **Copilot Chat and Custom Instructions:**
  - Developers can define custom instructions in markdown for Copilot to follow as part of every prompt, including project-specific rules or coding conventions.
  - Copilot Chat can analyze codebases and generate new instructions reflecting existing structure and best practices.
  - Tasks such as pull request prompts can now be specified in VS Code settings for more nuanced AI interaction.

- **Terminal Command Auto-Approval (Experimental):**
  - AI agents can auto-approve a whitelist of "safe" terminal commands (e.g., `cd`, `ls`), though the allow list is currently empty.
  - A UI for managing allowed and denied commands is planned, and Microsoft is exploring having LLMs evaluate command safety.
  - The feature aims to balance productivity benefits with safety, especially as agentic AI automates portions of the workflow.

## Community Feedback and Discussion

- Some developers have voiced concerns about "AI bloat"—the increasing proportion of AI-driven features in VS Code. (Example: “Yay, more AI bloat,” said one Reddit user).
- Microsoft responds by highlighting strong usage telemetry for Copilot features versus some other core editor functions.
- Non-AI features such as Live Share have reportedly received less attention, triggering stability issues in the new release.

## Broader Trends

- There is momentum behind CLI-based AI coding and alternative AI-driven IDEs, such as Cursor (a VS Code fork) and Claude Code.
- Microsoft positions VS Code as the "open source AI editor," with a move to open source some AI features, including Copilot Chat (client-side).
- Developer debate continues over whether VS Code should decouple its base editor from the expanding Copilot suite.

## Implications for Developers

- Expect stronger integration between VS Code, Copilot, and AI-assisted workflows.
- The ability to enforce project or team conventions using AI may streamline code quality and consistency.
- AI-driven command execution and agentic features present both safety and productivity trade-offs.

---
**References:**  

- [VS Code 1.102 Release Notes](https://code.visualstudio.com/updates/v1_102)  
- [Microsoft's FAQ on AI Usage](https://code.visualstudio.com/docs/supporting/FAQ)  
- [Related GitHub Issue](https://github.com/microsoft/vscode/issues/255805)

For ongoing news and insights, visit [DevClass Developer News](https://devclass.com/).

This post appeared first on "DevClass". [Read the entire article here](https://devclass.com/2025/07/14/microsoft-shovels-extra-copilot-features-into-vs-code-amid-dev-complaints-of-more-ai-bloat/)
