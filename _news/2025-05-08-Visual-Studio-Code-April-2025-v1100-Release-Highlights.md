---
layout: "post"
title: "Visual Studio Code April 2025 (v1.100) Release Highlights"
description: "The April 2025 release of Visual Studio Code (version 1.100) introduces a wealth of enhancements across AI-powered development, agent mode chat, GitHub integration, notebook tools, security, and accessibility. Key innovations include NES-powered code suggestions, custom prompt/instruction files for chat, improved agent mode with OpenAI and Anthropic models, advanced tools for Python and Jupyter, and extension signature verification for Linux. This release also brings updates to accessibility features, source control, container development, and extension authoring, supporting a streamlined and secure developer experience."
author: "Visual Studio Code Team"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/updates/v1_100"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2025-05-08 17:00:00 +00:00
permalink: "/2025-05-08-Visual-Studio-Code-April-2025-v1100-Release-Highlights.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot", "Security"]
tags: ["Accessibility", "Agent Mode", "AI", "AI Assisted Development", "Anthropic Claude", "Chat Instructions", "Coding", "Copilot Chat", "DevOps", "Extension Security", "Floating Windows", "GitHub Copilot", "GitHub Pull Requests", "JavaScript Debugging", "Jupyter Notebooks", "MCP Server", "News", "Next Edit Suggestions", "OpenAI GPT 4.1", "Prompt Files", "Python Tools", "Security", "Semantic Search", "Source Control", "TypeScript", "VS Code", "VS Code 1.100"]
tags_normalized: ["accessibility", "agent mode", "ai", "ai assisted development", "anthropic claude", "chat instructions", "coding", "copilot chat", "devops", "extension security", "floating windows", "github copilot", "github pull requests", "javascript debugging", "jupyter notebooks", "mcp server", "news", "next edit suggestions", "openai gpt 4dot1", "prompt files", "python tools", "security", "semantic search", "source control", "typescript", "vs code", "vs code 1dot100"]
---

Visual Studio Code Team delivers an in-depth summary of the April 2025 (v1.100) release, focusing on new AI chat workflows, Copilot-powered code editing, agent mode, developer security, and ecosystem improvements.<!--excerpt_end-->

# Visual Studio Code April 2025 (v1.100) Release Highlights

The Visual Studio Code Team presents the April 2025 (1.100) update, packed with significant enhancements for developers across AI, chat, coding productivity, security, and more.

## Key Updates

### AI and Copilot Enhancements

- **Next Edit Suggestions (NES):** Now enabled by default and powered by a new, faster model (e.g., OpenAI GPT-4.1, Anthropic Claude 3.7/3.5), providing code recommendations that align more closely with developer intent and recent edits.
- **Customizable Chat Experience:** Developers can now create and manage prompt and instruction files in markdown to fine-tune Copilot's chat behavior per project or file type. These files improve code generation by applying team conventions and workflows.
- **Agent Mode Improvements:** Faster responses and edits in agent mode, including AI-powered autofixes for introduced errors, optimized prompt caching, and support for various model backends with vision capabilities.
- **Repository Search and MCP Integration:** Search code in any accessible GitHub repo directly from chat using the `#githubRepo` tool. Expanded Model Context Protocol (MCP) integration includes Streamable HTTP transport, image output, and improved tool progress reporting.

### Coding and Editor Experience

- **Multi-Window and Floating Editors:** Improved multi-window workflows, compact/always-on-top floating window modes, and enhanced visibility settings.
- **Enhanced Source Control and Code Review:** Staged changes now feature in-editor diff highlights, and Copilot Chat prevents pre-release installations in stable builds to maintain reliability.
- **Notebook Enhancements:** Drag and drop cell outputs into chat, new agent tools for notebook cell execution and environment management, and persistent find/replace history.
- **Improved Accessibility:** Accessible chat actions, unique audio cues for events, and new options for tracking changes or merge conflicts.
- **Language Features:** Contextual browser support info for HTML/CSS, better syntax highlighting for .env files, expandable JavaScript/TypeScript hovers, and improved network debugging in Node.js.

### Security

- **Extension Signature Verification:** Mandatory on all platforms (Windows, macOS, Linux), with full validation for every extension installation except Linux ARM32.
- **Learn More on Malicious Extensions:** Detailed links in VS Code for understanding flagged or blocked extensions.
- **Agent Mode Security Tools:** Fine-grained control over model commands and auto-diagnostics after edits.

### Python, Jupyter, and Data Tools

- **Python Testing Explorer:** Branch coverage support with coverage.py ≥7.7.
- **Python Environments:** Quick creation of virtual environments, AI-powered code actions (e.g., string conversion), and chat extensions for information/package installs.
- **Jupyter/Notebook Integration:** Notebook-specific chat tools for running cells, listing packages, and more.

### Extension Authoring

- **Text Encoding API:** New stable APIs for encoding and decoding text documents.
- **ESM Module Support:** NodeJS extension host can now run ESM-based extensions.
- **MCP API:** New APIs for extensions to contribute MCP servers and annotate tools for richer chat experiences.

### Additional Notable Fixes and Community Contributions

Hundreds of community contributions and specific bug fixes (e.g., session logout, language server consistency, improved HTML/JS support) ensure improved stability and user satisfaction. See the full release notes for details and a list of contributors.

---

## Useful Resources

- [Release Notes](https://code.visualstudio.com/updates/v1_100)
- [Chat Customization Documentation](https://code.visualstudio.com/docs/copilot/copilot-customization)
- [Model Context Protocol Servers](https://code.visualstudio.com/docs/copilot/chat/mcp-servers)
- [GitHub Pull Requests Extension](https://marketplace.visualstudio.com/items?itemName=GitHub.vscode-pull-request-github)

---

This release continues VS Code’s commitment to empowering developers with AI-driven workflows, secure and extensible development environments, and seamless multi-language support.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_100)
