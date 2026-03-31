---
author: Mark Downie
feed_name: Microsoft VisualStudio Blog
section_names:
- ai
- dotnet
- github-copilot
- security
title: Visual Studio March Update – Build Your Own Custom Agents
date: 2026-03-31 16:00:18 +00:00
tags:
- .agent.md
- .NET
- Agent Skills
- Agent Skills Specification
- AI
- C#
- C++
- Copilot Agent Mode
- Custom Agents
- Debugging
- Debugging And Diagnostics
- Dependency Updates
- Enterprise Governance
- Find Symbol
- GitHub Copilot
- Instrumentation Profiling
- Language Aware Navigation
- Live Profiling
- LSP
- MCP
- MCP Allowlist
- Model Picker
- News
- NuGet Vulnerabilities
- Perf Tips
- Performance Profiling
- Profiling Agent
- Razor
- Security
- SKILL.md
- Solution Explorer
- Test Explorer
- Tool Calling
- TypeScript
- Visual Studio Insiders
- VS
primary_section: github-copilot
external_url: https://devblogs.microsoft.com/visualstudio/visual-studio-march-update-build-your-own-custom-agents/
---

Mark Downie walks through the Visual Studio March update features that let teams customize GitHub Copilot with repo-based custom agents, reusable agent skills, and a new language-aware find_symbol tool, plus Copilot-assisted profiling and NuGet vulnerability fixes inside the IDE.<!--excerpt_end-->

# Visual Studio March Update – Build Your Own Custom Agents

This month’s Visual Studio update adds new ways to customize GitHub Copilot, including custom agents, reusable agent skills, and a language-aware `find_symbol` tool for agent mode. It also introduces Copilot-powered profiling experiences and a workflow to fix NuGet vulnerabilities from Solution Explorer.

Download **Visual Studio 2026 Insiders** to try these features: https://visualstudio.microsoft.com/downloads/

## Build your own custom agents

Custom agents let Copilot follow your team’s standards and workflows—for example:

- Enforce coding standards
- Run your build pipeline
- Query internal documentation

Agents are defined as `*.agent.md` files in your repository. They support:

- Workspace awareness
- Code understanding
- Tool access
- Your preferred model
- MCP connections to external knowledge sources

### Where to place the agent file

- Put `*.agent.md` into `*.github/agents/*`
- The agent will then appear in the agent picker inside Visual Studio

![Visual Studio custom agents picker](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2026/03/vs18.4_custom-agents.webp)

### Notes and gotchas

- If you don’t specify a model, the agent uses whatever you selected in the model picker.
- Tool names can vary across GitHub Copilot platforms, so verify which tools are available in Visual Studio to ensure your agent works as expected.
- Community examples are available in the `awesome-copilot` repo: https://github.com/github/awesome-copilot

## Use agent skills

**Agent skills** provide reusable instruction sets. Visual Studio automatically discovers skills from several locations, including:

- Repo locations like `*.github/skills/*`
- User profile locations like `~/.copilot/skills/*`

Each skill:

- Lives in its own directory
- Includes a `SKILL.md` file
- Follows the Agent Skills specification: https://agentskills.io/specification

When a skill is activated, it appears in the chat UI so you can tell it’s being applied.

![Visual Studio agent skills UI](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2026/03/vs18.4_agent-skills.webp)

More community-shared skills: https://github.com/github/awesome-copilot

## Find_symbol tool for agent mode

Copilot’s agent mode now supports **language-aware symbol navigation** via the new `find_symbol` tool.

What it enables:

- Find all references to symbols across the project
- Access metadata such as type information, declarations, and scope
- Perform more reliable refactors (e.g., updating a parameter across call sites) because the agent can use your code’s structure instead of text pattern matching

![find_symbol tool in Visual Studio](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2026/03/vs18.4_find-symbol.webp)

Once enabled, Copilot will use the tool automatically when answering questions or suggesting changes.

![find_symbol example](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2026/03/vs18.4_find-symbol-example.webp)

### Language support

Supported languages include:

- C++
- C#
- Razor
- TypeScript
- Any language with a supported LSP extension installed

For best results, use AI models that support tool-calling. Model details: https://docs.github.com/copilot/reference/ai-models/model-comparison

## Enterprise MCP governance

Visual Studio now respects GitHub-managed allowlist policies for MCP server usage.

- Admins can specify which MCP servers are allowed within their organization.
- When allowlisting is enabled, only approved servers can be connected.
- Using an unauthorized MCP server results in an error explaining the restriction.

The goal is to help organizations control which MCP servers handle sensitive data and support compliance requirements.

## Profile tests with Copilot

Test Explorer now includes a **Profile with Copilot** command.

![Profile with Copilot in Test Explorer](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2026/03/vs18.4_profile-test-with-copilot.webp)

How it works:

- The Profiling Agent runs the selected test and analyzes performance.
- It combines CPU usage and instrumentation data to produce actionable insights.
- By default it uses **Instrumentation profiling**.
- Currently supported in **.NET**.

If you need deeper analysis, you can launch the selected test directly from the Copilot chat window and choose additional profiling tools.

## Perf tips powered by live profiling

Visual Studio now surfaces performance signals while debugging:

- Inline execution time and performance signals as you step through code
- A Perf Tip you can click to ask Copilot for optimization suggestions immediately

![Perf Tip powered by profiling agent](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2026/03/vs18.4_profiler-agent-perf-tip.webp)

The Profiler Agent captures runtime data during debugging automatically:

- Elapsed time
- CPU usage
- Memory behavior

Copilot uses that data to identify hot spots and suggest targeted fixes.

## Fix vulnerabilities with Copilot

Copilot can help fix **NuGet package vulnerabilities** from Solution Explorer.

Workflow:

- When a vulnerability is detected, Visual Studio shows a notification with a **Fix with GitHub Copilot** link.
- Copilot analyzes the vulnerability and recommends/implements targeted dependency updates.

![Fix with GitHub Copilot link for NuGet vulnerabilities](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2026/03/vs18.4_fix-with-copilot-link.webp)

## HTML rich copy/cut

Visual Studio now supports HTML clipboard format when copying/cutting code:

- Preserves syntax highlighting and formatting when pasting into HTML-based apps
- Enabled by default

To configure:

- **Tools > Options > Text Editor > Advanced**
- Toggle **Copy rich text on copy/cut**
- Set the max length

Resources:

- Visual Studio Hub: https://aka.ms/vshub

Source post: https://devblogs.microsoft.com/visualstudio/visual-studio-march-update-build-your-own-custom-agents/


[Read the entire article](https://devblogs.microsoft.com/visualstudio/visual-studio-march-update-build-your-own-custom-agents/)

