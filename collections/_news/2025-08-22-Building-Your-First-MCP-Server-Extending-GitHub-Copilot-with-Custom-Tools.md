---
layout: post
title: 'Building Your First MCP Server: Extending GitHub Copilot with Custom Tools'
author: Chris Reddington
canonical_url: https://github.blog/ai-and-ml/github-copilot/building-your-first-mcp-server-how-to-extend-ai-tools-with-custom-capabilities/
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/feed/
date: 2025-08-22 16:52:27 +00:00
permalink: /github-copilot/news/Building-Your-First-MCP-Server-Extending-GitHub-Copilot-with-Custom-Tools
tags:
- AI & ML
- AI Extensibility
- AI Integration
- APIs
- Custom API
- Developer Tools
- Game Server
- MCP
- MCP Server
- MCP Tools
- Next.js
- Open Source
- Prompt Engineering
- Rock Paper Scissors
- Rubber Duck Thursdays
- SDK
- Tic Tac Toe
- TypeScript
- VS Code
section_names:
- ai
- coding
- github-copilot
---
Chris Reddington demonstrates how to extend GitHub Copilot with custom tools using the Model Context Protocol (MCP), sharing lessons learned by building a turn-based game MCP server in TypeScript.<!--excerpt_end-->

# Building Your First MCP Server: Extending GitHub Copilot with Custom Tools

**Author:** Chris Reddington  

Ever wanted to give GitHub Copilot access to external systems, or enable it to take actions beyond code suggestions? The [Model Context Protocol (MCP)](https://modelcontextprotocol.io/) makes this possible, providing a standard interface to extend AI tools like Copilot with custom resources, actions, and prompts.

## Overview: Why MCP?

AI developer tools like Copilot traditionally can’t access private data, perform custom actions, or interact with real-time APIs. MCP introduces a standardized way to bridge these gaps, allowing developers to:

- Register custom MCP servers for Copilot and VS Code
- Expose tools (actions), resources (state/data), and prompts (templates) to Copilot
- Consistently extend AI capabilities with pluggable, reusable components

## Example Project: Turn-Based Game MCP Server

Chris Reddington demonstrates MCP with a practical project: a TypeScript monorepo implementing a web frontend (Next.js), a backend API, and a custom MCP server to enable Copilot to play turn-based games (Tic-Tac-Toe and Rock Paper Scissors).

### Key Architecture Components

- **Next.js Web App:** Game UI for user moves
- **API Routes:** Handle game state and backend logic
- **MCP Server:** Implements AI-facing tools for making moves, fetching game state, etc.
- **Shared Libraries:** Game logic reused across server and client

You can find the sample code here: [github-samples/turn-based-game-mcp](https://github.com/github-samples/turn-based-game-mcp)

### Adding MCP Server to VS Code

Developers register their MCP server in `.vscode/mcp.json`. Example configuration:

```json
{
  "servers": {
    "playwright": { "command": "npx", "args": [ "@playwright/mcp@latest" ] },
    "turn-based-games": { "command": "node", "args": ["dist/index.js"], "cwd": "./mcp-server" }
  }
}
```

Once configured, Copilot discovers new capabilities and tools provided by the custom MCP server.

## The Three Building Blocks of MCP

- **Tools**: Actions the AI can take (e.g., `play_tic_tac_toe`, `create_rock_paper_scissors_game`). These expose backend logic to Copilot.
- **Resources**: Contextual data accessible via custom URIs (e.g., `game://tic-tac-toe/{Game-ID}` for retrieving game state).
- **Prompts**: Predefined interaction or strategy guides for quick user access within Copilot and VS Code.

## Workflow Example: Copilot Playing a Game

1. Register the MCP server in VS Code.
2. Interact with Copilot, which discovers available tools.
3. Ask Copilot to start or play a game.
4. Copilot invokes the relevant MCP tool; the MCP server computes Game AI moves and returns results.
5. Copilot presents results to the user, and the game continues.

## Security and Real-World Considerations

- **Authentication:** Real MCP servers often require OAuth or tokens for private access, though this demo keeps auth simple.
- **Open Source Trust:** Always review the code and publisher before running third-party MCP servers.
- **SDKs:** MCP supports multiple languages for server implementation (TypeScript, Python, Go, etc.).

## What Developers Can Learn

- How MCP extends Copilot with powerful, reusable functionality
- The value of decoupling tools, context, and prompts for AI integrations
- Patterns for building and sharing your own MCP servers for custom developer workflows

## Useful Links

- [GitHub MCP Server Implementation](https://github.com/github/github-mcp-server)
- [Turn-Based Game MCP Example](https://github.com/github-samples/turn-based-game-mcp)
- [Model Context Protocol Documentation](https://modelcontextprotocol.io/docs/learn/)

MCP lowers the barrier to creating deeply integrated developer tools, making AI assistants truly useful for specific, custom workflows beyond code completion.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/github-copilot/building-your-first-mcp-server-how-to-extend-ai-tools-with-custom-capabilities/)
