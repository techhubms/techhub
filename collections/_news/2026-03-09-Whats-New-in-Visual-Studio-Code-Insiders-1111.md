---
external_url: https://code.visualstudio.com/updates/v1_111
title: What's New in Visual Studio Code Insiders 1.111
author: Visual Studio Code Team
primary_section: github-copilot
feed_name: Visual Studio Code Releases
date: 2026-03-09 17:00:00 +00:00
tags:
- Accessibility
- Agent Plugins
- Agent Sessions
- AI
- AI CLI
- Chatbots
- Copilot Chat
- Copilot CLI
- Custom Agents
- Extensions
- GitHub Copilot
- Keyboard Shortcuts
- Localization
- News
- Observability
- OpenTelemetry
- Release Notes
- Source Control
- Telemetry
- VS Code
- VS Code Insiders
- .NET
section_names:
- ai
- dotnet
- github-copilot
---
Visual Studio Code Team summarizes the new features in VS Code Insiders 1.111, including improvements to Copilot integration, source control, agent plugins, and accessibility.<!--excerpt_end-->

# What's New in Visual Studio Code Insiders 1.111

The Visual Studio Code Team has released the 1.111 update for the Insiders build, bringing a variety of new features, enhancements, and bug fixes for developers who want early access to the latest tools.

## Release Highlights

- **Recursive Instruction File Discovery**: VS Code now recursively searches for `*.instructions.md` files in subdirectories under `.github/instructions/`, aligning with the behavior of Copilot CLI and GitHub Copilot agents.
- **Source Control Enhancements**: You can now copy names of items such as stash or branch directly from the Source Control Repositories view via the context menu.
- **Agent Plugin Improvements**: Custom agent frontmatter now supports agent-scoped chat hooks, and a new `/troubleshoot` slash command allows easier debugging and inspection for agent customizations.
- **Terminal Profiles Update**: AI CLI terminal profiles get a dedicated group in the terminal dropdown, making them easier to find.
- **MCP Apps File Downloads**: MCP Apps now support downloading files directly.
- **Keyboard Shortcut Addition**: `Ctrl+F5` in the integrated browser now refreshes the page without opening the debugger.
- **Copilot Chat Observability**: Added OpenTelemetry instrumentation for Copilot Chat, offering improved traceability and performance insights.
- **Chat UI Improvements**: Chat tips are now displayed conditionally and won’t repeat once dismissed within the same session; tables in chat improve horizontal scrolling and column width support.
- **Go to Definition for Localization**: You can Go to Definition on localization placeholders in `package.json` files to jump directly to the associated resource in `package.nls.json`.
- **Accessibility**: Adjustments have been made to respect chat accessibility settings, with reduced unintended speech for ongoing progress.
- **Theme Customization**: Token customization now supports relative values for font sizes and font weights.
- **Other Notable Changes**: Multi-page UIs for tools now have improved button layouts, terminal session isolation supports more granular scopes, and CJK punctuation characters render with improved consistency.

## Useful Links

- [VS Code February events](https://aka.ms/vscode/events)
- [Release notes online](https://code.visualstudio.com/updates)
- [Insiders build download](https://code.visualstudio.com/insiders)
- [Commit log](https://github.com/Microsoft/vscode/commits/main)
- [Closed issues for 1.111.0](https://github.com/microsoft/vscode/issues?q=is:issue+is:closed+milestone:%221.111.0%22)

Please note that some release notes were generated using GitHub Copilot.

## Daily Updates (March 1–5, 2026)

- **March 5**: Recursive Copilot instruction folder search, improved source control context menus, agent hook enhancements, `/troubleshoot` chat command, and additional CLI session isolation options.
- **March 4**: AI CLI terminal profiles group refinement.
- **March 3**: MCP Apps file downloads, improved browser refresh shortcut.
- **March 2**: OpenTelemetry for Copilot Chat, chat tip display rules, Go to Definition for localization placeholders, agent plugin Extension view improvements.
- **March 1**: Markdown table rendering enhancements in chat.
- (Late February) Additional accessibility and theme customization updates completed.

## How to Get Involved

Try the new features in VS Code Insiders and let the team know your feedback. For full change details, review the [commit log](https://github.com/Microsoft/vscode/commits/main) and [closed issues list](https://github.com/microsoft/vscode/issues?q=is:issue+is:closed+milestone:%221.111.0%22).

Stay tuned for further updates as the Insiders build continues to evolve.

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_111)
