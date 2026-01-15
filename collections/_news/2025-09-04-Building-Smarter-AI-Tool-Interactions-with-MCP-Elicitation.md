---
layout: post
title: Building Smarter AI Tool Interactions with MCP Elicitation
author: Chris Reddington
canonical_url: https://github.blog/ai-and-ml/github-copilot/building-smarter-interactions-with-mcp-elicitation-from-clunky-tool-calls-to-seamless-user-experiences/
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/feed/
date: 2025-09-04 16:00:00 +00:00
permalink: /github-copilot/news/Building-Smarter-AI-Tool-Interactions-with-MCP-Elicitation
tags:
- AI
- AI & ML
- AI Tool Integration
- Code Refactoring
- Coding
- Developer Experience
- Elicitation
- Game Development
- Generative AI
- GitHub Copilot
- MCP
- MCP Server
- News
- Rubber Duck Thursdays
- Schema Driven Prompts
- Tool Naming
- Turn Based Games
- TypeScript
- User Experience
- VS Code
section_names:
- ai
- coding
- github-copilot
---
Chris Reddington details how to build smarter AI tool interactions using elicitation in MCP servers, focusing on GitHub Copilot and developer experience improvements.<!--excerpt_end-->

# Building Smarter AI Tool Interactions with MCP Elicitation

_Authored by Chris Reddington_

## Overview

This article examines how implementing elicitation in a Model Context Protocol (MCP) server can transform the way AI tools like GitHub Copilot interact with users. By enabling the server to request missing information dynamically, developers can create more intuitive and seamless tool experiences, particularly for turn-based games such as tic-tac-toe or rock, paper, scissors.

## Key Concepts

- **Elicitation**: A technique where, if all required information isn't available, the AI (via the MCP server) interactively asks the user for missing data before proceeding. This avoids default assumptions and promotes better contextual understanding.
- **MCP Server**: Handles AI interactions, tool calls, and user prompts. It can consolidate tool logic, enforce DRY code principles, and now supports elicitation based on the 2025 MCP revision.

## Implementation Walkthrough

1. **Identifying Missing Parameters**: When a user requests a game, the server checks which parameters (e.g., difficulty, player name) are missing using targeted TypeScript handlers.
2. **Dynamic Prompting**: The server presents schema-driven prompts to request only the missing information, using UI dialogs in Visual Studio Code via Copilot Chat.
3. **Unified Tools**: Refactoring reduced tool sprawl by merging similar functions, e.g., from `create-tic-tac-toe-game` and `create-tic-tac-toe-game-interactive` to a single `create-game` tool.
4. **Iterative Development**: Initial implementations triggered elicitation every time, but were improved to elicit only necessary information based on the user’s initial input.

## Developer Learnings

- **Tool Naming Is Critical**: Ambiguous or duplicative tool names can confuse AI agents and users alike. Merging tools and using distinct names is essential.
- **User Experience Matters**: Elicitation can enhance the user experience, but careful design is required to avoid unnecessary friction.
- **Iterative Improvements**: Test with real users (as done in Rubber Duck Thursdays streams) and refine logic (e.g., property schema alignment, better request parsing).

## How To Try MCP Elicitation Yourself

- **Fork the Sample Repo**: [gh.io/rdt-blog/game-mcp](https://gh.io/rdt-blog/game-mcp)
- **Set Up a GitHub Codespace** and build the demo MCP server
- **Interact through Copilot or the provided web app** to experience elicitation-based tool calls

## Visual Example

Screenshots in the article show Copilot Chat in VS Code prompting users for preferences not initially provided, such as player name and game difficulty.

## Final Thoughts

Elicitation is an important advance for context-aware, developer-centric AI tools. It enables tools like Copilot to deliver better experiences by gathering the required information before proceeding—no more clunky defaults or misunderstood prompts. Consolidating tools, enhancing UI, and focusing on clear user interaction patterns leads to more robust and user-focused AI integrations.

Join the next [Rubber Duck Thursdays stream](https://gh.io/rubberduckthursdays) for more insights on AI and developer experience, and check out the [guide to building your first MCP server](https://github.blog/ai-and-ml/github-copilot/building-your-first-mcp-server-how-to-extend-ai-tools-with-custom-capabilities?utm_campaign=rdt-blog-devrel&utm_source=blog&utm_medium=turnbased-mcp-blog-1).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/github-copilot/building-smarter-interactions-with-mcp-elicitation-from-clunky-tool-calls-to-seamless-user-experiences/)
