---
layout: "post"
title: "Debugging UI with AI: GitHub Copilot Agent Mode Meets MCP Servers"
description: "Chris Reddington explores using GitHub Copilot agent mode in combination with the Playwright MCP server to debug and troubleshoot UI issues in a Next.js project. The article highlights best practices for providing clear requirements to AI tools and offers practical tips for real-world agentic workflows."
author: "Chris Reddington"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/ai-and-ml/github-copilot/debugging-ui-with-ai-github-copilot-agent-mode-meets-mcp-servers/"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/feed/"
date: 2025-07-22 21:58:35 +00:00
permalink: "/2025-07-22-Debugging-UI-with-AI-GitHub-Copilot-Agent-Mode-Meets-MCP-Servers.html"
categories: ["AI", "Coding", "DevOps", "GitHub Copilot"]
tags: ["Agent Mode", "Agentic AI", "AI", "AI & ML", "AI Workflows", "Automation", "Coding", "Custom Instructions", "Developer Productivity", "DevOps", "End To End Testing", "GitHub Copilot", "MCP", "MCP Server", "News", "Next.js", "Playwright", "Rubber Duck Thursdays", "TypeScript", "UI Debugging", "Visual Studio Code"]
tags_normalized: ["agent mode", "agentic ai", "ai", "ai ml", "ai workflows", "automation", "coding", "custom instructions", "developer productivity", "devops", "end to end testing", "github copilot", "mcp", "mcp server", "news", "next dot js", "playwright", "rubber duck thursdays", "typescript", "ui debugging", "visual studio code"]
---

In this article, Chris Reddington details his experience using GitHub Copilot agent mode alongside the Playwright MCP server for efficient UI debugging in a Next.js application, and shares practical tips for leveraging agentic tools with clear requirements.<!--excerpt_end-->

# Debugging UI with AI: GitHub Copilot Agent Mode Meets MCP Servers

**Author:** Chris Reddington

---

If you’ve ever dusted off an old project and thought, “How did I leave things in such a mess?”, you’re not alone. On my latest **Rubber Duck Thursdays** stream, I revisited my OctoArcade Next.js app and encountered several persistent UI issues. This time, I experimented with a combination that felt almost magical: letting GitHub Copilot agent mode and the Playwright MCP (Model Context Protocol) server collaboratively identify and fix UI bugs.

## Revisiting OctoArcade and Surfacing Bugs

Firing up OctoArcade—a collection of GitHub-themed mini-games built with Next.js and TypeScript—I quickly noticed bugs not addressed during the addition of a new game. In one session, we tackled:

- **Navigation header overlapping content**: Copilot agent mode and Playwright MCP visually identified and fixed a global header issue.
- **Bonus fixes:** Addressed additional UI issues like unintended canvas-footer gaps revealed during testing.
- **Results:** Issue resolutions with minimal manual intervention, solving problems I had previously struggled to diagnose and correct.

## Setting Up Copilot Custom Instructions

My environment in VS Code Insiders included updated Copilot custom instruction files (`.github/copilot-instructions.md`, `*.instructions.md`). Keeping these files current is vital, as they provide Copilot agents with context about repository structure, coding styles, and workflow expectations. The agent uses this information to propose relevant and accurate changes.

Tip: When instructions might be outdated, I’ll prompt Copilot to review them:

```markdown
Based on the #codebase, please can you update the custom instructions file for accuracy? Please make sure to keep the structure (i.e. headings etc.) as-is. Thanks!
```

I also instruct Copilot to update documentation files when major changes are made, ensuring documentation stays current with the codebase.

## Agentic Debugging with Playwright MCP

[Playwright MCP server](https://github.com/microsoft/playwright) is a robust tool for end-to-end UI testing. As an MCP server, it allows AI tools (like Copilot agent mode or Copilot coding agent) to:

- Load web pages
- Simulate user actions (clicks, navigation)
- Inspect rendered layouts

Getting started is as easy as adding to your MCP configuration:

```json
{
  "mcpServers": {
    "playwright": {
      "command": "npx",
      "args": ["@playwright/mcp@latest"]
    }
  }
}
```

After starting the server, Copilot accesses new tools for browser interaction:

- `browser_snapshot` — Accessibility snapshots
- `browser_navigate` — URL navigation
- `browser_click`, `browser_type`, `browser_hover` — Element interaction
- `browser_resize`, `browser_take_screenshot` — Responsive and visual documentation

Full details can be found in the [Playwright MCP server README](https://github.com/microsoft/playwright-mcp?tab=readme-ov-file#tools).

## The Debugging Journey: Real-Time Fixes & Key Lessons

### 1. Explicit Problem Description Fuels Agentic Workflows

An issue: game content starting behind the navigation bar, particularly on pages rendering games like OctoPong or OctoSnap. To kick off Copilot agent mode with Playwright, I wrote detailed prompts:

> I have spotted a UI error—main content "starts" behind the navigation bar, especially in games like OctoSnap, OctoPong, and OctoBrickBreaker. Can you use Playwright, inspect the pages when a game is loaded, and investigate?

The agent initially inspected configuration pages but missed issues found during gameplay, requiring more targeted prompts. The experience underscored a crucial point: the more precise your requirements, the better Copilot’s outputs—much like communicating with a human collaborator.

Copilot, leveraging Playwright, iteratively navigated, diagnosed, and proposed solutions, suggesting architectural improvements like passing context from the root layout for a unified navigation bar. I observed as Copilot made fixes, reran the app, and handled linting errors with minimal guidance.

### 2. Iterative Refinement of Requirements

Bug fixing, especially within UI, is rarely linear. Fixes often introduce unintended side effects, like disappearing game paddles or new layout gaps. The prompts and responses show how iteration and explicit requirements guide the AI toward viable solutions:

- Initial prompt: fix layout gap, leading to missing paddles due to a container height issue.
- Subsequent prompt: restore visibility, which reintroduced the spacing issue.
- Final refined requirements (ensuring game is playable, covers space, and does not exceed the viewport): achieved the intended results.

Key reflection: **The most challenging part was specifying requirements, not the technical task itself.**

## Practical Tips for Copilot Agent Mode & MCP Servers

- **Keep custom instructions current:** They inform the agent’s understanding of your project.
- **Power Copilot with MCP servers:** Playwright MCP makes UI inspection and interactive testing possible from within agent workflows.
- **State requirements explicitly:** List must-haves, behaviors, and edge cases clearly.
- **Iterate and commit frequently:** Small incremental changes are easier to test and review.

## Conclusion: Progress Beats Perfection

This real-world debugging session reinforced two key takeaways:

1. **Agentic tools like Copilot and Playwright MCP accelerate UI troubleshooting**—especially when given the right context.
2. **Specifying clear requirements is challenging and essential**. Iteration and feedback are fundamental for both learning and problem-solving.

### Actionable Steps

1. [Update Copilot custom instruction files](https://docs.github.com/en/copilot/how-tos/custom-instructions/adding-repository-custom-instructions-for-github-copilot) before agentic workflows.
2. [Install and start Playwright MCP server](https://github.com/microsoft/playwright-mcp?tab=readme-ov-file#getting-started) in VS Code to enable browser UI testing.
3. Describe issues and features clearly in Copilot agent mode chats.
4. Allow Copilot to propose/apply fixes, but always review changes.
5. Refine requirements and repeat as necessary.
6. Commit frequently to enable easy rollback and granular iteration.

Let us know how you’re using Copilot agent mode and MCP servers in your development workflows! For more insights, join us on the next [Rubber Duck Thursdays stream](https://gh.io/rubberduckthursdays).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/github-copilot/debugging-ui-with-ai-github-copilot-agent-mode-meets-mcp-servers/)
