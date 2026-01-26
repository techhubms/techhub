---
layout: "post"
title: "Giving Agents a Visual Voice: MCP Apps Support in VS Code"
description: "This article introduces MCP Apps, a major new extension for the Model Context Protocol (MCP) that enables AI coding agents in Visual Studio Code to render interactive UI components directly in agent conversations. It covers hands-on demos, integration details, and the collaborative benefits for developers, teams, and extension authors."
author: "Harald Kirschner, Connor Peet"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/blogs/2026/01/26/mcp-apps-support"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2026-01-26 00:00:00 +00:00
permalink: "/2026-01-26-Giving-Agents-a-Visual-Voice-MCP-Apps-Support-in-VS-Code.html"
categories: ["AI", "Coding"]
tags: ["AI", "AI Coding Agents", "Blog", "Code Profiling", "Coding", "Copilot Agents", "Developer Tools", "Feature Flags", "Flame Graphs", "Interactive UI", "MCP", "MCP Apps", "MCP Servers", "News", "Playwright", "Storybook", "VS Code", "VS Code Extensions", "Workflow Automation"]
tags_normalized: ["ai", "ai coding agents", "blog", "code profiling", "coding", "copilot agents", "developer tools", "feature flags", "flame graphs", "interactive ui", "mcp", "mcp apps", "mcp servers", "news", "playwright", "storybook", "vs code", "vs code extensions", "workflow automation"]
---

Harald Kirschner and Connor Peet present how VS Code now supports MCP Apps, giving AI coding agents the power to render interactive dashboards, forms, and visualizations in the editor.<!--excerpt_end-->

# Giving Agents a Visual Voice: MCP Apps Support in VS Code

*By Harald Kirschner and Connor Peet*

AI coding agents in Visual Studio Code (VS Code) are quickly becoming more capable and interactive. Traditionally, these agents could search your codebase, edit files, run commands, and summarize information using text alone. With the introduction of [MCP Apps](https://modelcontextprotocol.github.io/ext-apps/api/), AI agents can now display interactive user interfaces—such as dashboards, forms, and workflows—right within the agent chat panel in VS Code.

## What Are MCP Apps?

MCP Apps are the first official extension of the Model Context Protocol (MCP), allowing tool calls from agents to return interactive UI components. This moves agent interaction beyond plain text, enabling richer developer experiences:

- Drag-and-drop list ordering
- Interactive flame graph visualization for profiling
- Feature flag selectors with live environment status
- Multi-step workflows and custom forms

## Real-World Demos

- **List Reordering:** Agents can now show a drag-and-drop UI for sorting tasks, making the process visual and immediate instead of back-and-forth through text.
- **Performance Profiling:** Agents present interactive flame graphs, so developers can explore performance data visually and validate agent analysis.
- **Feature Flag Management:** Developers select feature flags within an interactive UI, seeing environment status and generating SDK code in a unified workflow.

## Ecosystem and Partner Adoption

- [Storybook](https://storybook.js.org) has integrated MCP Apps into their open source MCP server. Now, agents can render and preview Storybook components in the conversation without switching contexts.
- Quote from Storybook core contributor, Jeppe Reinhold: "Users can now preview Storybook stories directly in agent chat, eliminating the need to navigate between the chat and their Storybook to review changes."

## Why This Matters

By integrating MCP Apps into VS Code (currently in Insiders, rolling out to Stable soon), Microsoft is making VS Code the first major code editor to support MCP Apps natively. This change gives agents a "visual voice"—making collaboration richer and keeping developer workflows inside the editor.

## How to Get Started

- **Try It Now:** Install [VS Code Insiders](https://code.visualstudio.com/insiders/) to use MCP Apps.
- **Explore Demos:** [Demo playground](https://github.com/digitarald/mcp-apps-playground), [SDK and examples](https://github.com/modelcontextprotocol/ext-apps/).
- **Documentation:** [VS Code MCP guide](https://code.visualstudio.com/docs/copilot/customization/mcp-servers), [MCP developer guide](https://code.visualstudio.com/docs/copilot/guides/mcp-developer-guide).
- **Learn More:** See the [MCP Apps announcement](https://blog.modelcontextprotocol.io/posts/2026-01-26-mcp-apps/) and join the [VS Code livestream](https://youtube.com/live/HWmC3T5Wwqw) with the MCP team for demos and Q&A.

If you're developing MCP servers or AI-driven development tools, it's a great time to experiment with MCP Apps to make agent interactions more powerful and tangible.

---

*Happy Coding!*

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2026/01/26/mcp-apps-support)
