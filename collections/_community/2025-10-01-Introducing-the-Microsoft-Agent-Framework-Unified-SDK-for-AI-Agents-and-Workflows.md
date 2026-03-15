---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/introducing-the-microsoft-agent-framework/ba-p/4458377
title: 'Introducing the Microsoft Agent Framework: Unified SDK for AI Agents and Workflows'
author: Lee_Stott
feed_name: Microsoft Tech Community
date: 2025-10-01 14:14:57 +00:00
tags:
- .NET
- A2A SDK
- Agent Orchestration
- Agentic AI
- AutoGen
- Azure OpenAI
- Context Providers
- Foundry SDK
- Graph Based Workflows
- Human in The Loop
- LLM Integration
- M365 Copilot Agents
- MCP SDK
- Microsoft Agent Framework
- Middleware
- Migration Guides
- Multi Agent Systems
- Python
- Semantic Kernel
- State Management
- Workflow Automation
- AI
- Azure
- Community
section_names:
- ai
- azure
- dotnet
primary_section: ai
---
Lee Stott presents the Microsoft Agent Framework, guiding developers through its unified, open-source SDK for agentic AI apps with .NET and Python support, multi-agent orchestration, and advanced workflow capabilities.<!--excerpt_end-->

# Introducing the Microsoft Agent Framework: Unified SDK for AI Agents and Workflows

The Microsoft Agent Framework is an open-source SDK designed for developers aiming to build intelligent, multi-agent applications in .NET or Python. It delivers a unified foundation by merging the best of Semantic Kernel’s enterprise features with AutoGen’s research-oriented abstractions and introduces advanced capabilities for agent orchestration and workflow design.

## Key Features

- **Multi-Agent Orchestration:** Simplifies building systems with multiple autonomous agents using both .NET and Python.
- **Integrated Best-of-Breed:** Combines AutoGen’s orchestration simplicity and Semantic Kernel’s strong features like thread-based state management, type safety, and telemetry.
- **Graph-Based Workflows:** Define complex, modular workflows with routing, conditional logic, checkpointing, and support for human-in-the-loop interventions.
- **Open and Extensible:** Developers can extend with middleware, memory context providers, and custom integrations.
- **Supports Major Providers:** Out-of-the-box support for Azure OpenAI, OpenAI, and Azure AI.
- **Backward Compatibility:** Migration guides ensure a smooth upgrade path from existing Semantic Kernel or AutoGen-based solutions.

## Installation and Getting Started

- **Python:**

  ```shell
  pip install agent-framework
  ```

- **.NET:**

  ```shell
  dotnet add package Microsoft.Agents.AI
  ```

## Integration Ecosystem

- Works seamlessly with Foundry SDK, MCP SDK, A2A SDK, and integrates with M365 Copilot Agents.
- Rich library of declarative agent manifests and code samples.
- Learning resources and community involvement provided through Microsoft Learn modules, GitHub repositories, and the Azure AI Foundry Discord server.

## Workflow and Agent Design

- **AI Agents**: Enable dynamic, decision-making components that process inputs, leverage state management, call tools or MCP servers, and generate responses.
- **Workflows**: Structure complex processing using type-based routing, orchestration patterns (sequential, concurrent, hand-off), and checkpoints for reliability and flexibility.

### Example Use Cases

- Building enterprise customer support bots
- Automating research agent tasks
- Orchestrating code generation or educational AI systems
- Integrating human oversight into process loops

## Migration and Compatibility

Developers using Semantic Kernel or AutoGen will find migration guides and backward compatibility built in. Community input is welcome with active feedback and contributions on GitHub.

## Important Notes

- Agent Framework is currently in public preview; developers are encouraged to participate in testing and to be mindful of data handling and compliance when integrating third-party agents or services.
- Documentation, downloads, and migration resources available via official [Microsoft resources](https://aka.ms/AgentFramework).

## Community and Support Resources

- [Microsoft Learn Modules](https://learn.microsoft.com/training/paths/develop-ai-agents-on-azure/)
- [AI Agents for Beginners GitHub](https://github.com/microsoft/ai-agents-for-beginners)
- [Azure AI Foundry Discord](https://aka.ms/foundry/discord)
- [Microsoft Agent Framework GitHub](https://github.com/microsoft/agent-framework)

Get started by downloading the SDK, exploring documentation, and joining the growing developer community building the next generation of AI agentic workflows.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/introducing-the-microsoft-agent-framework/ba-p/4458377)
