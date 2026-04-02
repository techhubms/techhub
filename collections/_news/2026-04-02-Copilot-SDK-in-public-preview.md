---
tags:
- .NET
- Agent Runtime
- Agentic Capabilities
- AI
- Anthropic
- Approval Handlers
- Azure AI Foundry
- Blob Attachments
- BYOK
- Copilot
- Copilot CLI
- Custom Agents
- Custom Tools
- Distributed Tracing
- GitHub Copilot
- GitHub Copilot SDK
- Go
- Java
- Multi Turn Sessions
- News
- Node.js
- OpenAI
- OpenTelemetry
- Permission Framework
- Python
- Streaming Responses
- Tool Invocation
- TypeScript
- W3C Trace Context
author: Allison
date: 2026-04-02 21:26:37 +00:00
primary_section: github-copilot
section_names:
- ai
- dotnet
- github-copilot
external_url: https://github.blog/changelog/2026-04-02-copilot-sdk-in-public-preview
feed_name: The GitHub Blog
title: Copilot SDK in public preview
---

Allison announces the public preview of the GitHub Copilot SDK, outlining how developers can embed Copilot’s agent runtime into apps and workflows with built-in tools like streaming, permissions, and OpenTelemetry, plus BYOK support.<!--excerpt_end-->

# Copilot SDK in public preview

The [GitHub Copilot SDK](https://github.com/github/copilot-sdk) is now available in public preview. It provides building blocks to embed Copilot’s agentic capabilities into your own applications, workflows, and platform services.

The SDK exposes the same production-tested agent runtime that powers GitHub Copilot cloud agent and Copilot CLI. Instead of building your own AI orchestration layer, it includes tool invocation, streaming, file operations, and multi-turn sessions out of the box.

## Now available in five languages

- **Node.js / TypeScript**: `npm install @github/copilot-sdk`
- **Python**: `pip install github-copilot-sdk`
- **Go**: `go get github.com/github/copilot-sdk/go`
- **.NET**: `dotnet add package GitHub.Copilot.SDK`
- **Java**: Newly available to install via Maven.

## Key capabilities

- **Custom tools and agents**: Define domain-specific tools with handlers and let the agent decide when to invoke them. Build custom agents with tailored instructions.
- **Fine-grained system prompt customization**: Customize parts of the Copilot system prompt using `replace`, `append`, `prepend`, or dynamic `transform` callbacks (without rewriting the entire prompt).
- **Streaming and real-time responses**: Stream responses token-by-token.
- **Blob attachments**: Send images, screenshots, and binary data inline without writing to disk.
- **OpenTelemetry support**: Built-in distributed tracing with W3C trace context propagation across all SDKs.
- **Permission framework**: Gate sensitive operations with approval handlers, or mark read-only tools to skip permissions.
- **Bring Your Own Key (BYOK)**: Use your own API keys for OpenAI, Azure AI Foundry, or Anthropic.

## Get started

- Availability: The SDK is available to all Copilot and non-Copilot subscribers, including Copilot Free for personal use and BYOK for enterprises.
- Quotas: Each prompt counts toward premium request quota for Copilot subscribers.
- Links:
  - Getting started guide: [https://github.com/github/copilot-sdk](https://github.com/github/copilot-sdk)
  - Community discussion category: [https://github.com/orgs/community/discussions/categories/copilot](https://github.com/orgs/community/discussions/categories/copilot)


[Read the entire article](https://github.blog/changelog/2026-04-02-copilot-sdk-in-public-preview)

