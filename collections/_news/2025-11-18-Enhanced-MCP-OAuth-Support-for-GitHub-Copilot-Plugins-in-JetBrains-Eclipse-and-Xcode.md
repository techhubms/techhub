---
external_url: https://github.blog/changelog/2025-11-18-enhanced-mcp-oauth-support-for-github-copilot-in-jetbrains-eclipse-and-xcode
title: Enhanced MCP OAuth Support for GitHub Copilot Plugins in JetBrains, Eclipse, and Xcode
author: Allison
feed_name: The GitHub Blog
date: 2025-11-18 17:30:57 +00:00
tags:
- Authentication
- Copilot
- DCR
- Dynamic Client Registration
- Eclipse
- Enterprise Integration
- IDE Plugins
- Identity Provider
- Improvement
- JetBrains
- MCP
- OAuth
- OAuth 2.0
- OAuth 2.1
- Plugin Development
- Xcode
- AI
- GitHub Copilot
- News
section_names:
- ai
- github-copilot
primary_section: github-copilot
---
Allison explains the new enhanced authentication options in GitHub Copilot plugins for JetBrains, Eclipse, and Xcode, helping developers adopt secure OAuth provider integration in their preferred IDEs.<!--excerpt_end-->

# Enhanced MCP OAuth Support for GitHub Copilot Plugins in JetBrains, Eclipse, and Xcode

GitHub Copilot plugins for JetBrains, Eclipse, and Xcode have been updated to support enhanced authentication scenarios, giving developers more options for integrating various OAuth identity providers (IdPs).

## What's New

- **Broader OAuth Provider Support:** Plugins now work with a wider range of third-party IdPs, beyond the GitHub MCP server.
- **Standards-Based Integration:** OAuth 2.0 and OAuth 2.1 are now supported, leveraging industry standards for authentication.
- **Dynamic Client Registration (DCR):** The plugins use DCR for automatic client setup with IdPs that support it.
- **Fallback Mechanism:** If DCR is unavailable, the system falls back to a manual, client-credential workflow, letting administrators configure static client IDs and secrets.
- **Greater Flexibility:** This approach enables integration with enterprise IdPs or custom authentication services, preserving both flexibility and security/compliance requirements.

## How to Try the Enhanced Authentication

- Update your GitHub Copilot plugin or app to the latest version in JetBrains IDEs, Eclipse, or Xcode.
- Ensure you have a valid [Copilot license](https://github.com/features/copilot).
- Enhanced options will be available in your plugin's authentication workflow.

For specific plugin downloads or updates, visit:

- [GitHub Copilot for JetBrains IDEs](https://plugins.jetbrains.com/plugin/17718-github-copilot)
- [GitHub Copilot for Eclipse](https://marketplace.eclipse.org/content/github-copilot)
- [GitHub Copilot for Xcode](https://github.com/github/copilotforXcode/)

For more on MCP authentication and its standards, see the official [MCP Authorization Documentation](https://modelcontextprotocol.io/specification/2025-06-18/basic/authorization).

## Feedback and Support

The GitHub team invites feedback on these enhancements:

- [GitHub Copilot for JetBrains IDEs Feedback](https://github.com/microsoft/copilot-intellij-feedback/issues)
- [GitHub Copilot for Eclipse Feedback](https://github.com/microsoft/copilot-eclipse-feedback/issues)
- [GitHub Copilot for Xcode Feedback](https://github.com/github/CopilotForXcode/issues)

If you encounter issues or have improvement suggestions, please use the appropriate feedback channel above.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-18-enhanced-mcp-oauth-support-for-github-copilot-in-jetbrains-eclipse-and-xcode)
