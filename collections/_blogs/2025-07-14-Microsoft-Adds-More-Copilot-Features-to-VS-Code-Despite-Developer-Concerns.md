---
layout: "post"
title: "Microsoft Adds More Copilot Features to VS Code Despite Developer Concerns"
description: "This article examines the latest Visual Studio Code (VS Code) update (v1.102), highlighting Microsoft's significant addition of Copilot and AI-driven features, the technical details of Model Context Protocol (MCP) support, newly introduced custom instruction capabilities, developments in AI command automation, and the broader impact of these changes on developer workflows and community sentiment."
author: "DevClass.com"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.devclass.com/ai-ml/2025/07/14/microsoft-shovels-extra-copilot-features-into-vs-code-amid-dev-complaints-of-more-ai-bloat/101147"
viewing_mode: "external"
feed_name: "DevClass"
feed_url: "https://devclass.com/feed/"
date: 2025-07-14 16:58:30 +00:00
permalink: "/2025-07-14-Microsoft-Adds-More-Copilot-Features-to-VS-Code-Despite-Developer-Concerns.html"
categories: ["AI", "GitHub Copilot"]
tags: ["AI", "AI Development", "AI Productivity", "Auto Approval Terminal Commands", "Blogs", "Copilot Chat", "Custom Instructions", "Developer Tools", "GitHub Copilot", "Large Language Models", "MCP", "Microsoft", "VS Code", "VS Code Extensions"]
tags_normalized: ["ai", "ai development", "ai productivity", "auto approval terminal commands", "blogs", "copilot chat", "custom instructions", "developer tools", "github copilot", "large language models", "mcp", "microsoft", "vs code", "vs code extensions"]
---

DevClass.com reports on Microsoft's VS Code v1.102 release, where Tim Anderson details the bundling of new Copilot and AI features, technical innovations like MCP support, custom instructions, and reactions from the developer community.<!--excerpt_end-->

# Microsoft Adds More Copilot Features to VS Code Despite Developer Concerns

**Author:** DevClass.com (Tim Anderson)

Microsoft has released Visual Studio Code (VS Code) version 1.102 with a major focus on expanding Copilot and AI-driven capabilities. Out of the nine key updates, eight are linked to Copilot, underlining a continued bet on integrating AI into developer tools.

## Major Features in VS Code v1.102

- **Model Context Protocol (MCP):**
  - MCP support is now generally available.
  - VS Code introduces a new "MCP Servers" section in the Extensions view, listing 37 MCP servers at launch, accessible via install buttons.
  - MCP allows servers to solicit developer input directly from clients, improving collaborative, AI-powered workflows.

- **Custom Instructions for Copilot:**
  - Developers can now define custom instructions in markdown to be automatically respected by Copilot when generating suggestions.
  - Instructions are included in every prompt to Copilot, ensuring team rules or conventions are followed.
  - Beyond code, task instructions (e.g., for pull requests) can be configured within VS Code settings.
  - Copilot Chat can generate these instructions by analyzing codebases for structure and patterns.

- **AI Automation of Terminal Commands:**
  - Experimental feature enables automatic approval for "safe" terminal commands executed by AI agents (e.g., `cd`, `ls`).
  - "Safe" and "dangerous" command lists are maintained, with plans for UI-based customization and LLM-evaluated safety reviews.

## Developer Community Response

Some developers question the increasing dominance of AI features, referring to it as "more AI bloat." Microsoft's counterpoint, backed by telemetry data, is that AI features are now more widely used by VS Code users than legacy features like debugging or testing.

Issues in non-AI features, like Live Share, have drawn attention to resource allocation priorities. The article notes that fixes for Live Share introduced new errors, highlighting a perceived shift in focus towards AI-centric development at the expense of other features.

## Industry Context

A growing interest in CLI tools for AI coding is also evident. Alternative editors adopting AI capabilities, such as Cursor’s fork of VS Code, signal a larger trend in the ecosystem.

Microsoft has expressed its intent for VS Code to become “the open source AI editor.” Efforts to open source more Copilot features, including the chat client, reinforce this direction. Some developers advocate for the option to decouple Copilot for leaner, non-AI workflows, but the trend currently favors tighter AI integration.

## References

- Official [VS Code v1.102 Release Notes](https://code.visualstudio.com/updates/v1_102)
- Community discussions on Reddit, Hacker News, and GitHub Issues
- Microsoft's Copilot usage FAQ

## Conclusion

The latest VS Code release further embeds AI—especially GitHub Copilot—into the developer workflow, emphasizing automation, customization, and integration. Reactions are mixed, reflecting both excitement for AI-powered productivity and concern over possible feature bloat.

This post appeared first on "DevClass". [Read the entire article here](https://www.devclass.com/ai-ml/2025/07/14/microsoft-shovels-extra-copilot-features-into-vs-code-amid-dev-complaints-of-more-ai-bloat/101147)
