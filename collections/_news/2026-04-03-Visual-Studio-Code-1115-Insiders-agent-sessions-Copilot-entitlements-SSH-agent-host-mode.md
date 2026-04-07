---
title: 'Visual Studio Code 1.115 (Insiders): agent sessions, Copilot entitlements, SSH agent host mode'
primary_section: github-copilot
author: Visual Studio Code Team
external_url: https://code.visualstudio.com/updates/v1_115
section_names:
- ai
- devops
- dotnet
- github-copilot
date: 2026-04-03 17:16:30 +00:00
tags:
- .NET
- Agent Host Protocol
- Agent Sessions
- Agentic Development
- AI
- Background Terminals
- Code Serve Web
- Developer Tooling
- DevOps
- GitHub Copilot
- Go To File
- IDE
- Integrated Browser
- Minimap
- News
- Quick Pick
- Remote SSH
- Telemetry
- Terminal Integration
- Test Coverage
- VS Code
- VS Code CLI
- VS Code Insiders
feed_name: Visual Studio Code Releases
---

The Visual Studio Code Team summarizes what’s new in VS Code 1.115 (Insiders), including updates to Copilot-related agent sessions, terminal automation, and remote SSH connections for agent hosting.<!--excerpt_end-->

# Visual Studio Code 1.115 (Insiders)

*Last updated: April 3, 2026*

Welcome to the 1.115 release of Visual Studio Code.

## GitHub Copilot / agentic development

- Explore Agentic Development: Join a GitHub Copilot Dev Day near you: https://aka.ms/githubcopilotdevdays
- Copilot status bar entitlements and usage information is available in Sessions. ([#306462](https://github.com/microsoft/vscode/issues/306462))

## Agent sessions, chat, and browser integration

- Track and restore file edits in agent sessions, including diffs, undo/redo, and state restoration for customizations made during a session. ([#305332](https://github.com/microsoft/vscode/issues/305332))
- Chat can track and link to browser tabs opened or interacted with during a session, so agents can reference open web pages. ([#306537](https://github.com/microsoft/vscode/issues/306537))
- Rename agent sessions from the client side via a new client-to-server rename capability in the Agent Host Protocol. ([#305318](https://github.com/microsoft/vscode/issues/305318))

## Terminal improvements

- Background terminals automatically notify the agent when commands complete, including the exit code and terminal output. Prompts for input in background terminals are also detected and surfaced. ([#307201](https://github.com/microsoft/vscode/issues/307201))
- The `send_to_terminal` tool lets agents send commands to background terminals with user confirmation, addressing cases where `run_in_terminal` with a timeout would move terminals to the background and make them read-only. ([#305909](https://github.com/microsoft/vscode/issues/305909))
- Paste files (for example, images) into the terminal using Ctrl+V, drag-and-drop, and right-click paste. ([#301603](https://github.com/microsoft/vscode/issues/301603))

## Remote development / SSH

- Connect to remote machines over SSH, automatically installing the CLI and starting it in agent host mode. ([#306196](https://github.com/microsoft/vscode/issues/306196))

## Editor and UI updates

- Show test coverage indicators in the minimap. ([#258961](https://github.com/microsoft/vscode/issues/258961))
- Pinch-to-zoom gestures on Mac in the integrated browser. ([#307267](https://github.com/microsoft/vscode/issues/307267))
- Open web pages now show their favicons in the **Go to File** quick pick list, matching favicon display on browser tab labels. ([#299792](https://github.com/microsoft/vscode/issues/299792))

## CLI updates

- Update `code serve-web` CLI command to support `--disable-telemetry`, `--default-folder`, and `--default-workspace` options. ([#192230](https://github.com/microsoft/vscode/issues/192230))

## Links

- Full release notes: https://code.visualstudio.com/updates/v1_115
- VS Code on LinkedIn: https://www.linkedin.com/showcase/vs-code
- VS Code on X: https://go.microsoft.com/fwlink/?LinkID=533687
- VS Code on Bluesky: https://bsky.app/profile/vscode.dev


[Read the entire article](https://code.visualstudio.com/updates/v1_115)

