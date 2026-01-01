---
layout: "post"
title: "Introducing Copilot Edits: AI-Powered Multi-File Editing in VS Code (Preview)"
description: "This news article introduces the preview of Copilot Edits for GitHub Copilot in Visual Studio Code. It details how developers can use natural language to ask Copilot for edits across multiple files, highlights new UI concepts for better control, and covers the dual-model architecture powering this AI-assisted code editing experience. The article is aimed at developers interested in integrating AI with their coding workflows and showcases real-world benefits and technical underpinnings."
author: "Isidor Nikolic"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/blogs/2024/11/12/introducing-copilot-edits"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2024-11-12 00:00:00 +00:00
permalink: "/2024-11-12-Introducing-Copilot-Edits-AI-Powered-Multi-File-Editing-in-VS-Code-Preview.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["AI", "AI in Code Review", "AI Powered Editing", "Claude 3.5 Sonnet", "Code Refactoring", "Coding", "Copilot Edits", "Developer Tools", "Extension Preview", "GitHub Copilot", "GPT 4o", "Natural Language Programming", "News", "O1 Preview", "Programming Productivity", "Speculative Decoding", "VS Code", "Working Set"]
tags_normalized: ["ai", "ai in code review", "ai powered editing", "claude 3dot5 sonnet", "code refactoring", "coding", "copilot edits", "developer tools", "extension preview", "github copilot", "gpt 4o", "natural language programming", "news", "o1 preview", "programming productivity", "speculative decoding", "vs code", "working set"]
---

Isidor Nikolic introduces Copilot Edits, now in preview, which adds conversational, AI-powered multi-file code editing to GitHub Copilot in VS Code. This article highlights the main features, how developers remain in control, and the technology behind the new experience.<!--excerpt_end-->

# Introducing Copilot Edits (Preview)

**Author: Isidor Nikolic**  
_Copilot Edits is now available in preview for all GitHub Copilot users in Visual Studio Code._

## What is Copilot Edits?

Copilot Edits is a newly introduced feature for GitHub Copilot in Visual Studio Code (VS Code) that enhances code editing by allowing developers to use natural language to request changes across multiple files. It combines the best aspects of Copilot Chat and Inline Chat, enabling iterative code modifications in a developer-centric UI.

## Key Features

### 1. Multi-File Editing

- Specify a Working Set of files in your workspace for editing.
- Use natural language prompts to describe the changes you want.
- Edits are made inline, and you can review and accept or discard each suggestion.

### 2. Iterative Development Flow

- Stay in the 'flow' of development by reviewing, accepting, and iterating on changes in real time.
- Easily run your code, verify tests, and undo or redo changes as needed.
- Supports adding files to the Working Set via drag and drop, keyboard commands, or by opening files in the editor.

### 3. Developer Control

- Edits are limited to the explicitly specified Working Set, unless Copilot proposes creating new files.
- Undo/redo functionality allows safe experimentation.
- Code-review style UI for granular feedback on AI-generated edits.

### 4. Accessibility and Flexibility

- Use voice commands for hands-free interaction.
- Fits workflows for both non-expert and advanced engineers alike.
- Enables quick prototyping or complex refactoring, even in unfamiliar languages.

## Under the Hood: The Technical Details

- Uses a dual-model architecture for efficient and accurate suggestions.
- Lets users choose the foundation model (e.g., GPT-4o, o1-preview, o1-mini, Claude 3.5 Sonnet).
- Employs a speculative decoding endpoint designed for fast application of file changes.
- Upcoming improvements planned for even faster performance and improved transitions from Copilot Chat.

## Availability and Getting Started

- Copilot Edits is now in preview and available to all GitHub Copilot users in VS Code.
- For the latest features, try VS Code Insiders and the pre-release Copilot Chat extension.
- Feedback is welcome—file issues or suggestions in the official [VS Code GitHub repository](https://github.com/microsoft/vscode/issues).

For a deeper dive, read the [official documentation](https://code.visualstudio.com/docs/copilot/chat/copilot-edits).

---

Copilot Edits helps developers iterate faster by integrating conversational AI into existing coding workflows. Whether quickly iterating on new ideas or orchestrating major codebase refactoring, Copilot Edits is designed to streamline the development experience with AI assistance—all while keeping you, the developer, in total control.

Happy coding!

_Isidor Nikolic_

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2024/11/12/introducing-copilot-edits)
