---
layout: "post"
title: "Visual Studio Code January 2026 Insiders Update (v1.109): Terminal and Chat Improvements"
description: "This update details new features and fixes in the Visual Studio Code Insiders January 2026 release (version 1.109). Highlights include terminal enhancements like kitty keyboard protocol support, win32-input-mode, advanced formatting sequences, improved chat view discoverability, import options, and enriched extension APIs. Developer workflows and compatibility see targeted improvements."
author: "Visual Studio Code Team"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code.visualstudio.com/updates/v1_109"
viewing_mode: "external"
feed_name: "Visual Studio Code Releases"
feed_url: "https://code.visualstudio.com/feed.xml"
date: 2026-02-04 17:00:00 +00:00
permalink: "/2026-02-04-Visual-Studio-Code-January-2026-Insiders-Update-v1109-Terminal-and-Chat-Improvements.html"
categories: ["Coding"]
tags: ["Chat View", "Coding", "Developer Tooling", "Extension API", "Kitty Keyboard Protocol", "MCP Apps", "Microsoft", "News", "npm Commands", "Quick Input", "Settings Editor", "SGR 221", "SGR 222", "Terminal", "VS Code", "Win32 Input Mode"]
tags_normalized: ["chat view", "coding", "developer tooling", "extension api", "kitty keyboard protocol", "mcp apps", "microsoft", "news", "npm commands", "quick input", "settings editor", "sgr 221", "sgr 222", "terminal", "vs code", "win32 input mode"]
---

Visual Studio Code Team shares the January 2026 Insiders update, covering robust terminal upgrades, chat session enhancements, new MCP Apps support, and improvements to extension APIs and settings.<!--excerpt_end-->

# Visual Studio Code January 2026 Insiders Release (v1.109)

_Last updated: January 11, 2026_

The Visual Studio Code (VS Code) Team has released the January 2026 Insiders build (version 1.109), introducing important features, bug fixes, and developer productivity enhancements. These notes summarize what's new:

## Terminal Enhancements

- **Kitty Keyboard Protocol (CSI u)**: The terminal now supports the advanced kitty keyboard protocol, unlocking more sophisticated keyboard input handling and access to previously unavailable key combinations. ([#286809](https://github.com/microsoft/vscode/issues/286809))
- **Win32-input-mode**: Enhanced compatibility for keyboard handling in Windows console applications, offering a smoother developer experience. ([#286896](https://github.com/microsoft/vscode/issues/286896))
- **SGR 221 and 222 Escape Sequences**: These sequences can now control bold and faint text attributes independently, giving users granular formatting options. ([#286810](https://github.com/microsoft/vscode/issues/286810))
- **Terminal Timeout Parameter**: The tool can now specify a `timeout` for running commands, helping to prevent unnecessarily long polling operations. ([#286598](https://github.com/microsoft/vscode/issues/286598))
- **Quick Suggestions Toolbar Logic**: The terminal’s suggest toolbar now hides the selection mode option if quick suggestions are disabled, making the user interface more intuitive. ([#286440](https://github.com/microsoft/vscode/issues/286440))

## Improved Chat and MCP Apps

- **Archived Chat Session Discoverability**: It's now easier to find and access archived conversations in the Chat view. ([#286815](https://github.com/microsoft/vscode/issues/286815))
- **Chat Session Import**: Users can import chat sessions directly into the Chat view, rather than being limited to opening them in editor tabs. ([#283954](https://github.com/microsoft/vscode/issues/283954))
- **MCP Apps Support**: VS Code now includes built-in support for MCP Apps, allowing servers to provide custom user interfaces for various tool invocations. ([#260218](https://github.com/microsoft/vscode/issues/260218))

## Settings and Extension APIs

- **Terminal Suggestion Settings**: The `terminal.integrated.suggest.quickSuggestions` option can be configured in the graphical Settings editor instead of manual JSON edits, streamlining customization. ([#286075](https://github.com/microsoft/vscode/issues/286075))
- **Enhanced Quick Input API**: Extension authors gain more flexibility, as the new API enables inline buttons after the input box – not just in the title bar. ([#221397](https://github.com/microsoft/vscode/issues/221397))
- **Overflow Buttons in Quick Input**: Quick input controls now support overflow buttons, offering secondary actions in an extra menu. ([#285213](https://github.com/microsoft/vscode/issues/285213))
- **NPM Commands Update**: The set of npm commands eligible for automatic execution has been refreshed for safety. ([#286463](https://github.com/microsoft/vscode/issues/286463))

## Additional Updates and Fixes

- **Web Browser Explorations**: Ongoing work is improving the built-in web browser using new rendering approaches to overcome iframe limitations. ([#277298](https://github.com/microsoft/vscode/issues/277298))
- **Auto Approval Transparency**: Fixed invisible auto-approval info for the fetch tool, improving workflow clarity. ([#282238](https://github.com/microsoft/vscode/issues/282238))
- **Commit and Issue Tracking**: Users can follow update progress in the [Commit log](https://github.com/Microsoft/vscode/commits/main) and peruse closed issues for this milestone [here](https://github.com/microsoft/vscode/issues?q=is%3Aissue+is%3Aclosed+milestone%3A%22January+2026%22).

## Try It Yourself

- To try these features, [download the VS Code Insiders build](https://code.visualstudio.com/insiders).
- For all updates and past release notes, visit [code.visualstudio.com/updates](https://code.visualstudio.com/updates).

**Happy Coding from the Visual Studio Code Team!**

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/updates/v1_109)
