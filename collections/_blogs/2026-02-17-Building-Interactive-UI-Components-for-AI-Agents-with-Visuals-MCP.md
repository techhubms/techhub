---
layout: "post"
title: "Building Interactive UI Components for AI Agents with Visuals MCP"
description: "This article by Harald Binkle introduces Visuals MCP, an open-source Model Context Protocol (MCP) server that enables AI agents (like those powered by GitHub Copilot) to deliver rich, interactive UI components such as tables, lists, trees, and images directly inside developer tools like VS Code. The post walks through the architecture, technology stack (TypeScript, Node.js, React, Vite), how each visual is a standalone React app, and how these components are served and rendered in response to agent tool calls. Installation options include both npm and a dedicated VS Code extension. Readers will learn how Visuals MCP bridges the gap between AI assistant capabilities and interactive developer experiences."
author: "Harald Binkle"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://harrybin.de/posts/visuals-mcp-server/"
viewing_mode: "external"
feed_name: "Harald Binkle's blog"
feed_url: "https://harrybin.de/rss.xml"
date: 2026-02-17 21:23:00 +00:00
permalink: "/2026-02-17-Building-Interactive-UI-Components-for-AI-Agents-with-Visuals-MCP.html"
categories: ["AI", "Coding", "GitHub Copilot"]
tags: ["AI", "AI Assistants", "Blogs", "Coding", "Data Visualization", "Developer Tools", "GitHub Copilot", "Interactive UI", "LLM Integration", "MCP", "MCP Server", "Node.js", "Open Source", "React", "Tailwind CSS", "TypeScript", "Visuals MCP", "Vite", "VS Code Extension"]
tags_normalized: ["ai", "ai assistants", "blogs", "coding", "data visualization", "developer tools", "github copilot", "interactive ui", "llm integration", "mcp", "mcp server", "nodedotjs", "open source", "react", "tailwind css", "typescript", "visuals mcp", "vite", "vs code extension"]
---

Harald Binkle explores how to extend AI agents with Visuals MCP, letting tools like GitHub Copilot render interactive tables, lists, and images inside VS Code using React, TypeScript, and a flexible MCP server.<!--excerpt_end-->

# Visuals MCP: Giving Your AI Agent a Face

**Author: Harald Binkle | Posted: February 17, 2026**

The Model Context Protocol (MCP) is transforming AI integration in developer workflows. With Visuals MCP, you can empower your AI agents—such as those using GitHub Copilot—to deliver interactive UI components directly inside your IDE or editor, moving far beyond plain text responses.

## The Problem: Text-Only AI Feels Limiting

Traditional AI integrations often return static tables or lists as markdown. This restricts interaction, requiring manual scrolling and re-querying for any updates or filtering.

## Visuals MCP: Interactive UIs for AI Agents

Visuals MCP solves this by letting AI agents return instructions to render specific UI components—like sortable tables, hierarchical trees, master-detail views, image grids, and more. Host tools (such as VS Code or Claude Desktop) can then render these using fully-featured React apps, making your interaction with AI both dynamic and productive.

### Key Technology Stack

- **TypeScript** and **Node.js** for the server
- **React 19** with **Vite** for fast, modular frontend apps
- **Tailwind CSS** for styling, **@tanstack/react-table** for data grids
- Each UI is a separate, self-contained React app bundled as a single HTML file

### Architecture Overview

- Every visual (table, tree, list, image, etc.) is its own independent React application
- Vite builds each app to a standalone HTML resource, served by the Node.js MCP server
- On tool invocation, the server sends the appropriate UI and data instructions to the host
- The React app receives input and theme context using `useApp` from `@modelcontextprotocol/ext-apps/react`, enabling dynamic rendering and state updates

### Example: Displaying a Table

When your agent is prompted to “display a table of users,” the following occurs:

1. The AI receives the user request and calls the `display_table` tool defined by Visuals MCP
2. The server responds with a JSON structure describing the table (columns, rows, etc.)
3. The host loads the React-based table app, which renders the interactive grid—complete with sorting, filtering, theming, and state feedback support

### Developer Experience

- Simple to integrate via npm or as a turn-key VS Code extension
- Each MCP tool maps to a specific UI, bundled via Vite and served by resource URI
- The extension layer auto-registers the MCP server, requiring no manual config for most users

### Features

- **Interactive Tables**: Fully sortable/filterable data grids
- **Tree Views**: Hierarchical data navigation
- **Master-Detail Views**: Efficient data inspection
- **Interactive Lists**: Reorder and select items with feedback
- **Image Previews**: Inline display of image assets with metadata

### Installation Options

- **Power user**: Run via `npx` for full control (works everywhere MCP is supported—including Claude Desktop)
- **VS Code Extension**: Install from the VS Code Marketplace for instant setup; no config files necessary

### Conclusion

Visuals MCP demonstrates a leap forward for AI agent UX—enabling practical, rich interfaces that amplify productivity. Whether building custom tooling or just seeking a better AI chat experience, Visuals MCP opens up new UI possibilities for AI-integrated developer tools.

Learn more and get started: [Visuals MCP on GitHub](https://github.com/harrybin/visuals-mcp)

This post appeared first on "Harald Binkle's blog". [Read the entire article here](https://harrybin.de/posts/visuals-mcp-server/)
