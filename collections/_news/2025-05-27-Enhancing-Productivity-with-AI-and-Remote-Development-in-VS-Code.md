---
external_url: https://code.visualstudio.com/blogs/2025/05/27/ai-and-remote
title: Enhancing Productivity with AI and Remote Development in VS Code
author: Brigit Murtaugh, Christof Marti, Josh Spicer, Olivia Guzzardo McVicker
feed_name: Visual Studio Code Releases
date: 2025-05-27 00:00:00 +00:00
tags:
- Agent Mode
- AI Integration
- Codespaces
- Copilot Chat
- Custom Instructions
- Dev Containers
- Developer Productivity
- Feature Configuration
- Python
- Remote Development
- Remote Tunnels
- SSH
- VS Code
- VS Code Extensions
- WSL
- AI
- Coding
- DevOps
- GitHub Copilot
- News
section_names:
- ai
- coding
- devops
- github-copilot
primary_section: github-copilot
---
Brigit Murtaugh, Christof Marti, Josh Spicer, and Olivia Guzzardo McVicker detail how AI—highlighting GitHub Copilot—supercharges remote development in VS Code, including tips for setup, custom instructions, and leveraging agent mode.<!--excerpt_end-->

# Enhance Productivity with AI and Remote Development in VS Code

**Authors:** Brigit Murtaugh, Christof Marti, Josh Spicer, Olivia Guzzardo McVicker  
Published: May 27, 2025

## Overview

This article explains how remote development features in Visual Studio Code (VS Code) can be combined with AI tools like GitHub Copilot to create a flexible and productive coding experience. Highlights include integrating Copilot into remote environments, customizing AI behavior with instruction files, leveraging agent mode, and safely managing autonomous tool execution.

## What is Remote Development in VS Code?

VS Code Remote Development provides several ways to code on remote environments:

- **Remote - SSH**: Open folders on a remote machine/VM using SSH.
- **Dev Containers**: Develop within Docker containers, with required tools pre-installed.
- **WSL**: Use Windows Subsystem for Linux for a Linux dev experience on Windows.
- **Remote - Tunnels**: Connect through secure tunnels without manual SSH configuration.
- **GitHub Codespaces**: Fully managed, cloud-hosted dev environments.

## AI in Remote VS Code Environments

Integrating AI with remote VS Code setups brings several benefits:

- **Automatic AI extension provisioning**: If you're using GitHub Copilot locally, it's automatically available when working remotely.
- **Enhanced chat context**: Remote workspace state and configuration become part of Copilot chat context.
- **Troubleshooting and setup assistance**: Use Copilot chat to resolve remote connection issues.
- **Custom instructions and chat participants**: Tailor the AI's understanding of your unique remote setup.
- **Agent mode autonomy**: Enable Copilot to run commands and manage your workspace with user-approved autonomy.

## Custom Instructions for Remote Development

To improve the quality and contextual accuracy of Copilot's assistance, use custom instructions:

- Describe the remote environment (e.g., dev container, VM, installed tools)
- Document language versions, extensions, and conventions
- Place custom instruction files in remote dev container configurations so Copilot tailors responses

**Example instruction:**

```
This is a dev container that includes `python3` and `pip3` pre-installed and available on the `PATH`, along with the Python language extensions for Python development.
```

You can add similar configuration steps via the `devcontainer.json` file or use built-in Python Templates.

## Dev Container Configuration Example

1. Start with the **Dev Containers: Add Dev Container Configuration Files...** command in VS Code.
2. Choose a template (such as Python 3) and build/connect.
3. The resulting environment inherits custom Copilot instructions for improved AI support.
4. Further enhance by merging instruction files or adding to `"github.copilot.chat.codeGeneration.instructions"` directly in your devcontainer config.

## Chat Participants for Problem Solving

VS Code chat supports domain-specific participants (like `@remote-ssh`). They help troubleshoot and interact with different parts of your environment—triggering context-aware help or executing commands across extensions.

- Type `@` in the chat panel to select a specialized participant.
- Use the **Diagnose with Copilot** feature to analyze connection problems rapidly.

## Agent Mode and Tool Approval

Agent mode can:

- Propose and execute code changes
- Run terminal commands in the remote environment
- Require explicit tool approval for safety

Administrators and developers can use the `chat.tools.autoApprove` setting to balance convenience and safety—it's recommended only in isolated environments like dev containers.

> **Warning:** Auto-approval allows Copilot to perform irreversible changes. Restrict this to non-production or sand-boxed environments to avoid accidental data loss.

## Additional Resources

- [Remote Development in VS Code Docs](https://code.visualstudio.com/docs/remote/remote-overview)
- [Dev Containers Extension Marketplace](https://marketplace.visualstudio.com/search?term=dev%20containers&target=VSCode&category=All%20categories&sortBy=Relevance)
- [Custom Instructions Guide](https://code.visualstudio.com/blogs/2025/03/26/custom-instructions)

## Conclusion

With integrated AI and remote development features, VS Code enables flexible, efficient, and secure coding from virtually anywhere. By configuring custom instructions, leveraging chat participants, and safely using agent mode, you can fully capitalize on Copilot’s capabilities both locally and in the cloud.

Happy (smart and remote) coding!

*Brigit, Christof, Josh, and Olivia*

This post appeared first on "Visual Studio Code Releases". [Read the entire article here](https://code.visualstudio.com/blogs/2025/05/27/ai-and-remote)
