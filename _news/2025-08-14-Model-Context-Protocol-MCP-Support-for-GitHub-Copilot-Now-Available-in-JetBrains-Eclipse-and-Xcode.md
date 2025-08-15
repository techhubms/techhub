---
layout: "post"
title: "Model Context Protocol (MCP) Support for GitHub Copilot Now Available in JetBrains, Eclipse, and Xcode"
description: "This announcement details the general availability of Model Context Protocol (MCP) support in GitHub Copilot for JetBrains, Eclipse, and Xcode. MCP enables Copilot to connect with external tools and data sources via compliant servers, providing enhanced contextual awareness and extended coding intelligence. The post covers integration requirements, authentication methods, enterprise policy management, and setup links for each supported IDE."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-13-model-context-protocol-mcp-support-for-jetbrains-eclipse-and-xcode-is-now-generally-available"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-14 10:35:58 +00:00
permalink: "/2025-08-14-Model-Context-Protocol-MCP-Support-for-GitHub-Copilot-Now-Available-in-JetBrains-Eclipse-and-Xcode.html"
categories: ["AI", "GitHub Copilot"]
tags: ["Agent Mode", "AI", "Contextual Coding", "Copilot Plugin", "Developer Tools", "Eclipse", "Enterprise Policy", "External Tool Integration", "GitHub Copilot", "IDE Integration", "JetBrains", "MCP", "Model Context Protocol", "News", "OAuth Authentication", "PAT Authentication", "Programming Productivity", "Remote MCP Server", "Xcode"]
tags_normalized: ["agent mode", "ai", "contextual coding", "copilot plugin", "developer tools", "eclipse", "enterprise policy", "external tool integration", "github copilot", "ide integration", "jetbrains", "mcp", "model context protocol", "news", "oauth authentication", "pat authentication", "programming productivity", "remote mcp server", "xcode"]
---

Allison announces the general availability of Model Context Protocol (MCP) support for GitHub Copilot in JetBrains, Eclipse, and Xcode, highlighting expanded integration options and contextual coding capabilities.<!--excerpt_end-->

# Model Context Protocol (MCP) Support for GitHub Copilot Now Available in JetBrains, Eclipse, and Xcode

The GitHub Copilot team has announced that Model Context Protocol (MCP) support is now generally available for JetBrains, Eclipse, and Xcode IDEs. This update empowers GitHub Copilot to connect with a broad range of external tools and data sources, offering deeper contextual awareness for developers and improving coding intelligence.

## What Is MCP?

MCP is a protocol specification that allows GitHub Copilot to integrate with both local and remote servers acting as context providers. Using MCP, Copilot operates in agent mode and can perform expanded tasks, such as:

- Connecting to remote or local MCP servers
- Creating GitHub issues from within the IDE
- Checking repository history
- Searching GitHub repositories and more

Connections to MCP servers require either Personal Access Token (PAT) or OAuth authentication methods for security.

## Supported IDEs and Getting Started

MCP is supported in the following environments:

- **JetBrains IDEs** ([Copilot plugin](https://plugins.jetbrains.com/plugin/17718-github-copilot))
- **Eclipse** ([Marketplace plugin](https://marketplace.eclipse.org/content/github-copilot))
- **Xcode** ([CopilotForXcode project](https://github.com/github/CopilotForXcode))

To use MCP:

1. Install the latest GitHub Copilot plugin for your IDE.
2. Ensure you have a valid [Copilot license](https://github.com/features/copilot).
3. Follow the official setup guides:
   - [Configure MCP servers in JetBrains IDEs](https://aka.ms/copilot-jb-mcp)
   - [Configure MCP servers in Eclipse](https://aka.ms/copilot-ecl-mcp)
   - [Configure MCP servers in Xcode](https://aka.ms/copilot-xd-mcp)

## Enterprise and Organization Policy

For enterprise and organization administrators, MCP access is managed via the Copilot policy settings within the MCP server. Note that MCP is **disabled by default** and must be enabled by an administrator. [Official documentation on enabling MCP policy](https://docs.github.com/copilot/how-tos/administer-copilot/manage-for-organization/manage-policies) is available.

## Feedback

Developers are encouraged to provide feedback and share their experience:

- [GitHub Copilot for JetBrains IDEs Feedback](https://github.com/microsoft/copilot-intellij-feedback/issues)
- [GitHub Copilot for Eclipse Feedback](https://github.com/orgs/community/discussions/151288)
- [GitHub Copilot for Xcode Feedback](https://github.com/github/CopilotForXcode/issues)

MCP integration marks a significant improvement in the flexibility and contextual power of GitHub Copilot across popular development environments.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-13-model-context-protocol-mcp-support-for-jetbrains-eclipse-and-xcode-is-now-generally-available)
