---
external_url: https://techcommunity.microsoft.com/t5/azure/missing-equivalent-for-python-memorysearchtool-and/m-p/4494284#M22429
title: 'Agent Memory Abstractions in Azure AI Foundry: Python vs C# SDKs'
author: goranobradovic
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-02-11 08:54:07 +00:00
tags:
- Agent Service
- AgentMemorySettings
- AI
- Automatic Memory
- Azure
- Azure AI Foundry
- Azure.AI.Projects
- C# SDK
- Community
- Long Term Memory
- Managed Memory
- MemorySearchTool
- NuGet
- Preview Features
- Python SDK
- SDK Parity
section_names:
- ai
- azure
---
goranobradovic raises important questions for teams working with Azure AI Foundry Agent Service—specifically regarding the lack of high-level automatic agent memory abstractions in the C# SDK compared to Python.<!--excerpt_end-->

# Agent Memory Abstractions in Azure AI Foundry: Python vs C# SDKs

**Author:** goranobradovic

This post discusses the current state and potential roadmap for managed long-term memory ("Automatic User Memory") in Azure AI Foundry Agent Service, comparing the Python and C# SDKs.

## Context

- **Python SDK**: Offers a straightforward way to attach a `MemorySearchTool` to agents and leverage `AgentMemorySettings`. This enables automatic extraction, consolidation, and retrieval of agent memory without manual CRUD operations.
- **C# SDK (`Azure.AI.Projects`)**: As of version 1.2.0-beta.5, only a low-level `MemoryStoreClient` is available, which seems to require developers to perform manual CRUD operations for memory management. There is no obvious high-level abstraction for automatic agent memory integration.

## Questions Raised

1. **Is there a high-level AgentMemorySearchTool or similar in the C# SDK?**
   - As per current documentation and public repos, such an abstraction does **not** appear to be present. Instead, developers must interact with low-level APIs to manage memory.

2. **Is automatic agent memory on the .NET SDK's immediate roadmap?**
   - No confirmed public roadmap is detailed in the referenced docs or repositories. For the latest status, it is advisable to monitor the [Azure.AI.Projects GitHub repo](https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/ai/Azure.AI.Projects) and submit an issue or feature request for official guidance.

3. **Are there samples for "automatic" memory usage in C#?**
   - At present, there are no public samples replicating the agent memory orchestration provided by the Python SDK. Current C# integration requires building custom logic or interfacing with REST APIs directly.

## Guidance

Until feature parity is achieved, .NET developers using Azure AI Foundry Agent Service should:

- Use `MemoryStoreClient` for manual memory operations.
- Track GitHub issues and changelogs for the `Azure.AI.Projects` SDK.
- Raise or upvote requests for high-level agent memory support if this capability is critical for your solutions.

Developers interested in automatic fact extraction and storage may need to implement a custom memory orchestration layer in .NET for the time being.

### References

- [Automatic User Memory in Azure AI Foundry (Python)](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/memory-usage?view=foundry&tabs=python)
- [Azure.AI.Projects C# SDK Documentation](https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/ai/Azure.AI.Projects#memory-store-operations)

---

_For latest information, watch the official documentation and the Azure SDK GitHub repositories. Community input may also accelerate prioritization for these features._

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure/missing-equivalent-for-python-memorysearchtool-and/m-p/4494284#M22429)
