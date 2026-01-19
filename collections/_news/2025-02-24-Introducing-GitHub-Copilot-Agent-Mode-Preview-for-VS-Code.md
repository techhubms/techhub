---
external_url: https://code.visualstudio.com/blogs/2025/02/24/introducing-copilot-agent-mode
title: Introducing GitHub Copilot Agent Mode (Preview) for VS Code
author: Isidor Nikolic
viewing_mode: external
feed_name: Visual Studio Code Releases
date: 2025-02-24 00:00:00 +00:00
tags:
- Agent Mode
- AI Assisted Coding
- Autonomous Programming
- Claude Sonnet
- Code Analysis
- Copilot Edits
- Developer Productivity
- GPT 4o
- LLM
- MCP Servers
- Refactoring
- Terminal Commands
- VS Code
- Workspace Tools
section_names:
- ai
- coding
- github-copilot
---
Isidor Nikolic introduces GitHub Copilot agent mode for VS Code, an autonomous AI-powered peer programmer that assists with multi-step coding tasks, context analysis, and development productivity.<!--excerpt_end-->

# Introducing GitHub Copilot Agent Mode (Preview)

**Author: Isidor Nikolic**

GitHub Copilot agent mode is now available in Visual Studio Code (VS Code) Insiders and is coming soon to VS Code Stable. This feature marks a significant advancement in AI-assisted software development, allowing Copilot to function as an autonomous peer programmer directly within your editor.

## What is Copilot Agent Mode?

Copilot agent mode brings a new level of autonomy to AI programming assistance. With agent mode enabled, Copilot can:

- Analyze your codebase and relevant files
- Propose and apply multi-file edits
- Run terminal commands and tests
- Monitor terminal/test output, respond to compile and lint errors
- Iterate through corrections until tasks are complete

Agent mode is designed to help with complex workflows, like creating apps from scratch, cross-file refactoring, running and writing tests, migrating legacy code, integrating new libraries, and even generating documentation. Developers interact with it using the Copilot Edits view, selecting the "Agent" mode in VS Code.

## How to Use Agent Mode

1. Open Copilot Edits in VS Code Insiders.
2. Choose **Agent** from the mode dropdown.
3. Enter your prompt describing the coding task.
4. Optionally, guide context selection by referencing files with `#file`, drag-and-drop, or the **Add Files** button.

Agent mode will loop autonomously over several steps, modifying files, executing terminal commands, and iterating as needed to achieve your requested outcome. You maintain control with transparent tool invocation, approval-required terminal actions, and rich undo support (e.g., **Undo Last Edit**).

## How Agent Mode Works Internally

- On each request, Copilot sends a tailored prompt to the selected LLM (such as GPT-4o or Claude Sonnet), including:
  - Your query
  - Workspace structure summary
  - Machine context (OS, environment)
  - Tool descriptions/results
- Copilot employs a suite of tools—such as reading file content, proposing and applying code edits, executing terminal commands, and more.
- Tool invocation is documented in the UI; users can inspect each action and intervene.

Example tool description:

```json
{
  "name": "read_file",
  "description": "Read the contents of a file. You must specify the line range you're interested in, and if the file is larger, you will be given an outline of the rest of the file. If the file contents returned are insufficient for your task, you may call this tool again to retrieve more content.",
  "parameters": {
    "type": "object",
    "properties": {
      "filePath": { "description": "The absolute paths of the files to read.", "type": "string" },
      "startLineNumberBaseZero": { "type": "number", "description": "The line number to start reading from, 0-based." },
      "endLineNumberBaseZero": { "type": "number", "description": "The inclusive line number to end reading at, 0-based." }
    },
    "required": ["filePath", "startLineNumberBaseZero", "endLineNumberBaseZero"]
  }
}
```

## Developer Experience and Best Practices

- *Iterate with Copilot*: The UI is built for step-wise refinement; you can review and revert each edit.
- *Manage Context*: You can explicitly provide files or specifications to narrow Copilot’s focus.
- *Quota Considerations*: Agent mode may quickly use up your Copilot quota, so use for complex, open-ended work.
- *Self-Hosting Philosophy*: Microsoft’s VS Code team uses agent mode extensively across 200+ repos, and it has shown particular value in smaller projects and productivity tasks.
- *Model Support*: Currently supports LLMs like GPT-4o and Claude Sonnet. Further model tuning is planned.

## Roadmap and Feedback

Upcoming features include fine-grained undo, context UI improvements, notebook support, auto-approval for specific commands, better terminal UI, expanded tool extensibility (including MCP servers), and plans for broader rollout in VS Code Stable.

You’re encouraged to try the preview, review the [official docs](/docs/copilot/chat/chat-agent-mode), and provide feedback via [VS Code GitHub repo](http://github.com/microsoft/vscode/issues/).

---

**Happy coding!**

---

*For full documentation and updates, see: [Read the full article](https://code.visualstudio.com/blogs/2025/02/24/introducing-copilot-agent-mode)*

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2025/02/24/introducing-copilot-agent-mode)
