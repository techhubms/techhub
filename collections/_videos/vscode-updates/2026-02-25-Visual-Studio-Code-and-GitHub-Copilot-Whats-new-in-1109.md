---
external_url: https://www.youtube.com/watch?v=FcYEOpsJD6g
title: Visual Studio Code and GitHub Copilot - What's new in 1.109
author: Fokko Veegens
feed_name: Fokko at Work
date: 2026-02-25 18:00:00 +00:00
tags:
- Claude Agent
- Developer Tools
- MCP Apps
- Mermaid Diagrams
- Plan Mode
- Productivity
- Terminal Sandboxing
- VS Code
- VS Code 1.109
- AI
- GitHub Copilot
- Videos
section_names:
- ai
- github-copilot
primary_section: github-copilot
---
Fokko Veegens covers the highlights of VS Code 1.109, including mermaid diagram rendering in chat responses, improved plan mode with interactive questions, Claude as an agent provider, MCP app support for richer visualizations, and terminal sandboxing for safer command execution.<!--excerpt_end-->

{% youtube FcYEOpsJD6g %}

## Full summary based on transcript

Visual Studio Code 1.109 brings several notable improvements to GitHub Copilot's capabilities, from visual diagram rendering to new agent providers and improved security. This highlights video covers the key features Fokko considers most relevant.

### Mermaid Diagrams in Chat Responses

Copilot can now render mermaid diagrams directly in chat output. Mermaid diagrams are text-based visualizations that support version control, live inside your repository, and render natively in markdown files like READMEs.

**Prerequisites:**

- Enable the "Render Mermaid Diagram" tool under Tools in chat (part of the Mermaid Chat Features extension, bundled with Copilot)
- Available from VS Code 1.109 onward, but requires manual activation

**Capabilities:**

- Ask Copilot to generate class diagrams, flowcharts, git branching diagrams, and other mermaid-supported diagram types
- Works with standard models like GPT-4.1 (no premium requests required)
- Diagrams render inline in the chat with interactive controls:
  - Hold Alt and scroll to zoom in/out
  - Pan while zoomed in (hold Alt and drag)
  - Right-click to reset pan and zoom
  - Right-click to copy diagram source (mermaid syntax)
- Insert the generated diagram directly into the open editor or at the cursor position

**Tip:** You can screenshot an existing diagram, add it as context, and ask Copilot to generate a mermaid version of it.

### Ask Questions Tool in Plan Mode

Plan mode now includes a structured four-step process: discovery, alignment, design, and refinement. The alignment step introduces an interactive questions feature where Copilot asks clarifying questions before generating a plan.

**How it works:**

1. **Discovery** - Copilot explores the codebase using sub-agents to understand the project structure
2. **Alignment** - Copilot presents structured questions with selectable options to clarify your intent
3. **Design** - Combines your answers with its analysis to create a detailed plan
4. **Refinement** - Adds verification data and documents decisions made during planning

**Question interface:**

- Questions appear with pre-defined options you can select
- Navigate between questions using arrow keys
- Enter custom answers when the available options don't fit
- Press Escape to skip questions
- Submitted answers are visible in the chat for review

**After planning:**

- Review and iterate on the generated plan
- Switch to agent mode to start implementation
- Optionally export the plan as a markdown specification file for use in a fresh chat session

**Configuration:**

- Setting: `chat.askQuestions.enabled`
- Currently experimental; enabled by default

### Claude Agent

VS Code 1.109 adds Claude as an agent provider alongside GitHub Copilot's built-in agents. This is an early implementation of the vision for VS Code as a multi-agent environment.

**Key details:**

- Select "Code" as the agent provider in the chat panel to use Claude
- Available modes: "Ask before edits" (comparable to ask mode), "Edit automatically" (agent mode), and plan mode
- Only Claude models are available when using the Claude agent (GPT models are not)
- Included in your existing Copilot license at no additional cost (current state, subject to change)
- Supports familiar features: file references with `#`, slash commands (slightly different set), and to-do list tracking

**Behavior:**

- Explores the codebase by searching for relevant files
- Creates and tracks to-do lists during implementation
- Shows file changes inline, similar to regular Copilot agent mode
- Uses the Claude SDK under the hood

**Note:** This is an early implementation and may have rough edges. Microsoft is actively seeking feedback to improve the experience.

### Support for MCP Apps

MCP (Model Context Protocol) apps extend the standard MCP server protocol with richer visualization and interactivity features. While MCP servers provide tool-based integration between Copilot and external APIs, MCP apps add visual components.

**Example demonstration:**

- A colleague-built MCP server can visualize any data with filtering, sorting, and export capabilities
- Select data in the editor (e.g., a list of flights from a controller) and ask Copilot to visualize it
- The MCP app renders an interactive table with:
  - Column filtering
  - CSV export
  - Rich data display

**Security considerations:**

- MCP servers require approval before execution (per-use, per-session, or always-allow options)
- Treat MCP servers like any software: review them, scan for vulnerabilities, and be cautious of unknown servers
- Rogue MCP servers could potentially exfiltrate data from your machine

**Compatibility:**

- MCP apps are a new protocol feature with varying support across tools
- Available in VS Code from 1.109; other tools (Windsurf, Cursor, Claude Code) support standard MCP servers but may not yet support the app protocol

### Terminal Sandboxing

Agent mode uses the terminal with the same permissions as the user, which poses security risks. Terminal sandboxing restricts what Copilot can do through the terminal.

**Configuration:**

- Setting: `chat.tools.terminal.sandbox.enabled` - Enable the sandbox
- Platform-specific file system settings: `chat.tools.terminal.sandbox.linuxFileSystem` and a corresponding Mac setting (Windows support not yet available)
- Configure in `settings.json` (not available through the visual settings editor)
- Network access restrictions via `allowedDomains` and `denyDomains` settings

**How it works:**

- Restricts the terminal to the current workspace directory only (cannot access files outside the workspace)
- Optionally restricts network access to specific domains
- Commands run without individual approval since they operate within the sandbox boundaries

**Alternatives for safe execution:**

- Use dev containers to isolate the development environment
- Use GitHub Codespaces for a fully disposable cloud environment (delete and recreate if something goes wrong)

**Platform support:**

- Fully supported on Linux and macOS
- Windows support is not yet available

[Watch the video on YouTube](https://www.youtube.com/watch?v=FcYEOpsJD6g)
