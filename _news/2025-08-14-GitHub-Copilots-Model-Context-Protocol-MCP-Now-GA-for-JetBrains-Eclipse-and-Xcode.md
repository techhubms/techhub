---
layout: "post"
title: "GitHub Copilot's Model Context Protocol (MCP) Now GA for JetBrains, Eclipse, and Xcode"
description: "This news update announces the general availability of Model Context Protocol (MCP) support with GitHub Copilot for JetBrains, Eclipse, and Xcode IDEs. MCP allows Copilot to connect with external tools and data sources, enhancing context-aware coding. The article details setup requirements, admin policy management, and how developers can begin using this feature."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-13-model-context-protocol-mcp-support-for-jetbrains-eclipse-and-xcode-is-now-generally-available"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-14 10:35:58 +00:00
permalink: "/2025-08-14-GitHub-Copilots-Model-Context-Protocol-MCP-Now-GA-for-JetBrains-Eclipse-and-Xcode.html"
categories: ["AI", "GitHub Copilot"]
tags: ["AI", "Coding Intelligence", "Contextual AI", "Copilot Plugin", "Copilot Policy", "Developer Tools", "Eclipse", "Enterprise Policy", "GitHub Copilot", "IDE Integration", "JetBrains", "MCP", "Model Context Protocol", "News", "OAuth Authentication", "PAT Authentication", "Plugin Installation", "Programming Productivity", "Xcode"]
tags_normalized: ["ai", "coding intelligence", "contextual ai", "copilot plugin", "copilot policy", "developer tools", "eclipse", "enterprise policy", "github copilot", "ide integration", "jetbrains", "mcp", "model context protocol", "news", "oauth authentication", "pat authentication", "plugin installation", "programming productivity", "xcode"]
---

Allison reports on the general availability of Model Context Protocol (MCP) support for GitHub Copilot in JetBrains, Eclipse, and Xcode, explaining how it enhances Copilot’s integration with external tools and provides steps for both developers and administrators.<!--excerpt_end-->

# GitHub Copilot's Model Context Protocol (MCP) Support Goes GA for JetBrains, Eclipse, and Xcode

**Author:** Allison

Model Context Protocol (MCP) support with GitHub Copilot is now generally available for JetBrains, Eclipse, and Xcode! MCP enables GitHub Copilot to integrate with a wider range of external tools and data sources, increasing the assistant's contextual awareness and coding capability across development environments.

## What Is MCP?

MCP (Model Context Protocol) lets Copilot connect to both local and remote servers that follow the MCP specification. Once a connection is established, Copilot's agent mode can interact with tools exposed by these servers, helping with broader development tasks.

**Sample Use Cases:**

- Remotely create GitHub issues
- Check repository history
- Search GitHub from your IDE

## Supported IDEs and Setup Requirements

MCP support is now available in:

- **JetBrains IDEs** ([Download Copilot plugin](https://plugins.jetbrains.com/plugin/17718-github-copilot))
- **Eclipse** ([Plugin marketplace](https://marketplace.eclipse.org/content/github-copilot))
- **Xcode** ([GitHub Copilot for Xcode](https://github.com/github/CopilotForXcode))

To get started, make sure you have:

1. Latest GitHub Copilot plugin for your IDE
2. Valid [Copilot license](https://github.com/features/copilot)
3. Access to configure MCP servers (can be local or remote; connect using PAT or OAuth authentication)

Follow the step-by-step setup guides:

- [Configure MCP servers in JetBrains](https://aka.ms/copilot-jb-mcp)
- [Configure MCP servers in Eclipse](https://aka.ms/copilot-ecl-mcp)
- [Configure MCP servers in Xcode](https://aka.ms/copilot-xd-mcp)

## Organization and Enterprise Policy Controls

MCP access within Copilot is regulated by organizational and enterprise policies. The feature is **disabled by default** and must be [enabled by an admin](https://docs.github.com/copilot/how-tos/administer-copilot/manage-for-organization/manage-policies). This ensures organizations can control how external integrations are managed for security and compliance.

## Share Your Feedback

Feedback and discussions are encouraged through the following channels:

- [Copilot for JetBrains feedback](https://github.com/microsoft/copilot-intellij-feedback/issues)
- [Copilot for Eclipse feedback](https://github.com/orgs/community/discussions/151288)
- [Copilot for Xcode feedback](https://github.com/github/CopilotForXcode/issues)

MCP support in GitHub Copilot represents another step towards deeper, tool-assisted development workflows—making code suggestions and automating tasks more context-aware for developers using multiple IDEs.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-13-model-context-protocol-mcp-support-for-jetbrains-eclipse-and-xcode-is-now-generally-available)
