---
external_url: https://devblogs.microsoft.com/semantic-kernel/build-ai-agents-with-github-copilot-sdk-and-microsoft-agent-framework/
title: Build AI Agents with GitHub Copilot SDK and Microsoft Agent Framework
author: Dmytro Struk
primary_section: github-copilot
feed_name: Microsoft Semantic Kernel Blog
date: 2026-01-27 21:37:26 +00:00
tags:
- .NET
- Agent Abstraction
- Agent Framework
- AI
- AI Agents
- Azure OpenAI
- Custom Tools
- File Operations
- Function Calling
- GitHub Copilot
- GitHub Copilot SDK
- MCP
- Microsoft Agent Framework
- Multi Agent Workflow
- News
- Orchestrators
- Python
- Session Management
- Shell Commands
- Streaming Responses
section_names:
- ai
- dotnet
- github-copilot
---
Dmytro Struk explains the new integration between Microsoft Agent Framework and the GitHub Copilot SDK, showing developers how to build AI agents in .NET and Python that leverage Copilot’s powerful features.<!--excerpt_end-->

# Build AI Agents with GitHub Copilot SDK and Microsoft Agent Framework

Microsoft has announced that the Agent Framework now integrates seamlessly with the [GitHub Copilot SDK](https://github.com/github/copilot-sdk), making it easier for developers to build AI agents that are powered by Copilot's capabilities. This integration is available in both .NET and Python, allowing for broad cross-platform support.

## Key Features of the Integration

- **Consistent Agent Abstraction**: Developers can implement GitHub Copilot agents using the same interface (`AIAgent` in .NET, `BaseAgent` in Python) as other frameworks, making agents interchangeable and composable.
- **Multi-Agent Workflows**: Use Copilot agents with other provider agents (like Azure OpenAI, Anthropic, OpenAI) in sequential, concurrent, handoff, or group chat workflows.
- **Ecosystem Integration**: Tap into the Agent Framework ecosystem, benefiting from declarative agent definitions, built-in orchestrators, and support for function tools, streaming, and session management.

## Installation

- **.NET**:

  ```shell
  dotnet add package Microsoft.Agents.AI.GithubCopilot --prerelease
  ```

- **Python**:

  ```shell
  pip install agent-framework-github-copilot --pre
  ```

## Getting Started: Creating a Copilot Agent

### .NET Example

```csharp
using GitHub.Copilot.SDK;
using Microsoft.Agents.AI;

await using CopilotClient copilotClient = new();
await copilotClient.StartAsync();
AIAgent agent = copilotClient.AsAIAgent();
Console.WriteLine(await agent.RunAsync("What is Microsoft Agent Framework?"));
```

### Python Example

```python
from agent_framework.github import GithubCopilotAgent

async def main():
    agent = GithubCopilotAgent(default_options={"instructions": "You are a helpful assistant."})
    async with agent:
        result = await agent.run("What is Microsoft Agent Framework?")
        print(result)
```

## Extending Agents with Custom Function Tools

- You can supply custom function tools for domain-specific enhancements. Example tools show both C# and Python approaches to adding weather data capabilities to an agent.

## Advanced Features

- **Streaming Responses**: Stream output as Copilot generates it, enhancing UX.
- **Multi-Turn Conversations**: Agents can maintain conversation threads, supporting context-aware dialogue.
- **Permission Handling**: Developers can grant or deny agent requests for sensitive actions (shell, file, URLs) by implementing permission handlers.

## Integrations: MCP Servers and More

- Agents can connect to local or remote MCP (Model Context Protocol) servers for extended capabilities, such as file system or external API access.

## Multi-Agent Workflows Example

- Demonstrates combining Azure OpenAI and GitHub Copilot agents in a sequential workflow for content creation and review tasks, all orchestrated within the Agent Framework.

## More Resources

- [GitHub Copilot SDK](https://github.com/github/copilot-sdk)
- [Microsoft Agent Framework on GitHub](https://github.com/microsoft/agent-framework)
- [Getting Started Tutorials](https://learn.microsoft.com/agent-framework/tutorials/overview)

## Summary

This integration allows developers to efficiently create advanced, flexible AI agent systems by leveraging both GitHub Copilot and the Microsoft Agent Framework features. Full code samples and best practices are provided for .NET and Python scenarios.

For questions or further discussion, join the [Microsoft Agent Framework community discussions](https://github.com/microsoft/agent-framework/discussions).

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/build-ai-agents-with-github-copilot-sdk-and-microsoft-agent-framework/)
