---
external_url: https://devclass.com/2025/12/11/vs-code-update-brings-agent-overload-typescript-7-preview-and-the-end-of-intellicode/
title: VS Code Update Introduces Agent HQ, TypeScript 7 Preview, and Deprecates IntelliCode
author: Tim Anderson
feed_name: DevClass
date: 2025-12-11 15:20:03 +00:00
tags:
- Agent HQ
- Agentci AI Foundation
- Agentic AI Foundation
- AI Agents
- AI Assisted Development
- AI/ML
- Code Completion
- Development
- Go Language Service
- IntelliCode
- JavaScript
- Microsoft
- Prompt Injection
- TypeScript
- TypeScript 7
- VS Code
- VS Code Extensions
- AI
- GitHub Copilot
- Blogs
- .NET
section_names:
- ai
- dotnet
- github-copilot
primary_section: github-copilot
---
Tim Anderson reports on the latest VS Code update, highlighting Agent HQ for agentic AI integration, TypeScript 7 support, and the move from IntelliCode to GitHub Copilot for AI-driven code completion.<!--excerpt_end-->

# VS Code Update: Agent HQ, TypeScript 7, and IntelliCode Deprecation

*Author: Tim Anderson*

## Overview

Microsoft has released an update to Visual Studio Code (VS Code), introducing Agent HQ, significant improvements for agentic AI-assisted development, a preview of TypeScript 7, and announcing the end of IntelliCode in favor of GitHub Copilot.

## Key Features

### Agent HQ & Agentic Coding

- **Agent HQ**: A new feature described as a central location for managing coding agents within VS Code.
- **Agent Sessions View**: Although now disabled by default, it showcases the ability to run multiple background and cloud-based agents simultaneously.
- **Customization**: Developers can customize, move, and share agents across organizations and workspaces.
- **Security Note**: The "YOLO" (You Only Live Once) setting, though disabled by default, poses security risks by turning off manual approval – a potential vulnerability.
- **Industry Movement**: Agent HQ is positioned to integrate agents from multiple vendors, keeping VS Code competitive with similar agent-focused tools from Google and JetBrains.

### AI and Productivity Impact

- AI agents aim to enhance developer productivity with automated assistance but introduce new security considerations like prompt injection and the reliability of generative AI.

### TypeScript 7 Preview

- **Native Compiler & Language Service**: TypeScript 7 includes a new implementation in Go, promising better performance, faster load times, and lower memory usage.
- **Experimental Support**: Developers can opt into TypeScript 7 features if they configure the required Go components.
- **Build Improvements**: VS Code’s build scripts are now fully in TypeScript, aided by Node.js 22.18+.

### IntelliCode Deprecation and Copilot Migration

- **IntelliCode End-of-Life**: IntelliCode, which offered free AI-driven code completions, is being deprecated.
- **GitHub Copilot**: Now the default for AI-assisted completions in VS Code, free for up to 2,000 completions/month, then subscription-based.
- **Developer Feedback**: Some developers express concern over the commercial motivations for the migration to Copilot.

## Security and Best Practices

- Features like YOLO highlight the trade-offs between productivity and security, especially when manual approval mechanisms are disabled.
- Developers are encouraged to remain vigilant about prompt injection risks and safeguard their coding environments accordingly.

## Conclusion

This VS Code update reflects Microsoft’s strategy to integrate more advanced AI features for developers, centralize agent management, and streamline code completion under GitHub Copilot. The move from IntelliCode to Copilot marks a notable commercial shift, while new TypeScript 7 tooling highlights progress toward higher performance and modern language support.

**Further Reading and References:**

- [VS Code Release Notes](https://code.visualstudio.com/updates/v1_107)
- [GitHub Blog on Agent HQ](https://github.blog/news-insights/company-news/welcome-home-agents/)
- [TypeScript 7 Preview](https://devblogs.microsoft.com/typescript/progress-on-typescript-7-december-2025/)
- [IntelliCode Deprecation Discussion](https://github.com/microsoft/vscode-dotnettools/issues/2537)

This post appeared first on "DevClass". [Read the entire article here](https://devclass.com/2025/12/11/vs-code-update-brings-agent-overload-typescript-7-preview-and-the-end-of-intellicode/)
