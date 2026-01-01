---
layout: "post"
title: "Teaching the LLM Good Habits: How Azure MCP Uses Best-Practice Tools"
description: "Chris Harris explains how Azure MCP (Model Context Protocol) leverages built-in best-practice tools and system-prompt instructions to help large language models like GitHub Copilot in VS Code generate secure, production-grade Azure solutions. The article details how MCP's instruction mechanism guides LLM behavior, enforces coding and deployment best practices, and provides automatic access to code/linter tools for infrastructure and application code in Azure environments."
author: "Chris Harris"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/all-things-azure/teaching-the-llm-good-habits-how-azure-mcp-uses-best-practice-tools/"
viewing_mode: "external"
feed_name: "Microsoft All Things Azure Blog"
feed_url: "https://devblogs.microsoft.com/all-things-azure/feed/"
date: 2025-09-23 23:02:30 +00:00
permalink: "/2025-09-23-Teaching-the-LLM-Good-Habits-How-Azure-MCP-Uses-Best-Practice-Tools.html"
categories: ["AI", "Azure", "Coding", "DevOps"]
tags: ["AI", "All Things Azure", "Azure", "Azure Best Practices", "Azure MCP", "Bicep", "CI/CD", "Cloud Governance", "Coding", "DefaultAzureCredential", "DevOps", "IaC", "LLM", "MCP", "MCP (model Context Protocol)", "News", "SDK", "Terraform", "Thought Leadership", "VS Code"]
tags_normalized: ["ai", "all things azure", "azure", "azure best practices", "azure mcp", "bicep", "cislashcd", "cloud governance", "coding", "defaultazurecredential", "devops", "iac", "llm", "mcp", "mcp model context protocol", "news", "sdk", "terraform", "thought leadership", "vs code"]
---

Chris Harris discusses how Azure MCP integrates best-practice guidance for application and infrastructure code, showing how Azure MCP uses system prompt instructions to keep LLMs like GitHub Copilot in VS Code aligned with secure, production-ready Azure solutions.<!--excerpt_end-->

# Teaching the LLM Good Habits: How Azure MCP Uses Best-Practice Tools

**Author:** Chris Harris

## Why Best Practices Matter

Best practices are essential for reliability, security, and cost-effectiveness in Azure development and deployment. While often overlooked in the rush to deliver solutions, skipping them can result in fragile setups, spiraling costs, and major security issues. The Azure team addresses this by embedding best-practice guidance into tools, SDK usage, Infrastructure-as-Code (IaC), CI/CD, and governance mechanisms.

## The Role of Large Language Models (LLMs)

With the rise of AI-assisted development, it's become crucial for large language models (LLMs) to internalize these best practices, not just human developers. This ensures that AI suggestions are robust, secure, and follow Microsoft’s recommendations by default.

## What is Azure MCP?

[Azure MCP (Model Context Protocol)](https://aka.ms/azmcp) acts as a smart server providing LLMs with clear, updatable guidelines. MCP's special feature is the `instructions` property in its `InitializeResult`. When an MCP host (like VS Code with GitHub Copilot) connects, it receives a block of text with usage and tool invocation instructions. These instructions ensure the LLM knows exactly when and how to invoke best-practice tools.

## Example: How Instructions Guide LLM Behavior

When initializing, MCP can send something like:

```json
{
  "protocolVersion": "2025-06-18",
  "serverInfo": { "name": "azure-mcp-server", "version": "preview" },
  "capabilities": { "tools": {}, "resources": {}, "prompts": {} },
  "instructions": "Azure MCP server usage rules:\n- Use Azure Tools: When handling requests related to Azure, always use your tools.\n- Use Azure Code Gen Best Practices: When generating code for Azure, running terminal commands for Azure, or performing operations related to Azure, invoke your bestpractices tool if available. Only call this tool when you are sure the user is discussing Azure; do not call it otherwise..."
}
```

The `instructions` field, generated from `azure-rules.txt`, codifies best practices for the model to follow when generating code or infrastructure scripts for Azure.

## Best-Practice Tools Exposed by Azure MCP

- **Get Best Practices**: Delivers secure, production-grade patterns and guidance for SDK usage, authentication (like `DefaultAzureCredential`), retry/backoff recommendations, deployment strategies, and safe defaults. Call this before generating Azure app code, deployment scripts, or when referencing Azure SDKs.
- **Deploy IaC Rules**: Enforces rules and guardrails for Infrastructure-as-Code (Bicep, Terraform), covering naming, parameterization, role assignments, secure defaults, use of managed identities, enabling telemetry/diagnostics, and more. Use this for IaC authoring, editing, or reviewing resource definitions.

## In Practice: GitHub Copilot in VS Code

Currently, GitHub Copilot in VS Code is the main MCP host supporting direct usage of these instructions. When connected, Copilot will follow the encoded rules and tools in real-time, adding guardrails to code and pipeline suggestions and proactively enforcing best practices.

## Why This Matters

- **For MCP Builders:** You can embed expertise once and scale it across teams/projects. Updating `azure-rules.txt` ensures all LLM sessions benefit from the latest guidance instantly.
- **For Azure MCP Users:** Best practices are automatically enforced, reducing risk and time spent troubleshooting, and increasing reliability and security.

## Conclusion

Azure MCP’s pattern of injecting best-practice rules directly into LLM system prompts turns guidelines into actionable, in-the-moment guardrails for both application and infrastructure code. This approach is worth considering for anyone designing LLM integrations or looking to embed organizational expertise into AI-assisted workflows.

This post appeared first on "Microsoft All Things Azure Blog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/teaching-the-llm-good-habits-how-azure-mcp-uses-best-practice-tools/)
