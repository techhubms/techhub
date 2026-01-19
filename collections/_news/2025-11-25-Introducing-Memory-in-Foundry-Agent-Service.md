---
layout: post
title: Introducing Memory in Foundry Agent Service
author: Lewis Liu, Paul Hsu, Takuto Higuchi
canonical_url: https://devblogs.microsoft.com/foundry/introducing-memory-in-foundry-agent-service/
viewing_mode: external
feed_name: Microsoft AI Foundry Blog
feed_url: https://devblogs.microsoft.com/foundry/feed/
date: 2025-11-25 16:00:48 +00:00
permalink: /ai/news/Introducing-Memory-in-Foundry-Agent-Service
tags:
- Agent Development
- Agent Factory
- Agent Memory
- Agent Service
- AI Agent
- AI Agents
- API Integration
- Contextual AI
- Conversational AI
- Gpt 4.1
- Hybrid Retrieval
- Long Term Memory
- Machine Learning
- MCP
- Memory
- Memory Store
- Microsoft Foundry
- MSIgnite
- Natural Language Processing
- Personalization
- Python SDK
- Stateful Agents
- Text Embedding 3 Small
- User Profile Extraction
section_names:
- ai
- azure
---
Lewis Liu, Paul Hsu, and Takuto Higuchi explain how Microsoft Foundry Agent Service’s new Memory feature helps developers build context-aware, personalized AI agents using long-term memory—all with easy integration and enterprise scalability.<!--excerpt_end-->

# Introducing Memory in Foundry Agent Service

Microsoft has announced the public preview of **Memory** in the Foundry Agent Service, a managed feature for adding long-term memory to conversational AI agents. Traditionally, most agents have been stateless and forgetful—each new conversation starts from scratch. Workarounds like embedding chat history into prompts or maintaining custom databases introduced complexity, latency, and often failed to provide truly context-aware interactions.

With the new Memory functionality, developers can now enable agents to store, retrieve, and manage chat summaries, user preferences, and critical context natively within the Foundry Agent Service. This means agents can remember important user facts and conversation details across sessions, devices, and workflows—delivering more personal and tailored experiences by default.

## Developer-First Experience

- **Rapid prototyping:** Start building with an out-of-the-box memory configuration directly through the Foundry portal.
- **Enterprise scalability:** Effortlessly expand to support millions of users and long-term memory entries.
- **Unified API:** Use a single, developer-friendly API, with flexible backend support and future BYO storage options.

## Technical Implementation Sample

Below is a Python example using the Foundry Agent Service SDK to create, update, and query a memory store for an agent:

```python
# Create a memory store

memory_store_name = "my_memory_store"
definition = MemoryStoreDefaultDefinition(
    chat_model="gpt-4.1",
    embedding_model="text-embedding-3-small",
    options=MemoryStoreDefaultOptions(
        user_profile_enabled=True,
        user_profile_details="Food preferences for a meal planning agent",
        chat_summary_enabled=True
    ),
)

memory_store = project_client.memory_stores.create(
    name=memory_store_name,
    description="Example memory store for conversations",
    definition=definition,
)

# Add a memory to the store

scope = "user123"
user_message = ResponsesUserMessageItemParam(
    content="I prefer dark roast coffee and usually drink it in the morning"
)
update_poller = project_client.memory_stores.begin_update_memories(
    name=memory_store.name,
    scope=scope,
    items=[user_message],
    update_delay=0,
)
update_result = update_poller.result()

# Retrieve memories

query_message = ResponsesUserMessageItemParam(content="What are my coffee preferences?")
search_response = project_client.memory_stores.search_memories(
    name=memory_store.name,
    scope=scope,
    items=[query_message],
    options=MemorySearchOptions(max_memories=5),
)
```

## Architecture and Quality Approaches

Memory management happens in several phases for efficient retrieval and storage:

1. **Extraction:** The system detects and extracts key data points (like user allergies or preferences) from interactions.
2. **Consolidation:** LLMs merge duplicate or similar entries, resolving conflicts for accurate long-term context.
3. **Smart Retrieval:** Agents use hybrid search to retrieve the right information at the start and throughout conversations, allowing for highly personalized and natural user experiences.
4. **Developer Customization:** Developers can control memory focus by specifying key user attributes (e.g., user_profile_details) for their particular application.

## Quality and Evaluation

Microsoft uses a rigorous, iterative approach to memory quality:

- Precise evaluation frameworks and curated datasets address real-world needs.
- End-to-end tests show Foundry’s memory-enabled agents outperform stateless models in recall, relevance, and personalization.
- Ongoing investment in research ensures continuous improvement.

## Getting Started

- [Start building with Foundry Portal](https://ai.azure.com/)
- [Read the memory documentation](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/concepts/agent-memory)
- [Review code samples](https://github.com/Azure/azure-sdk-for-python/tree/main/sdk/ai/azure-ai-projects/samples/memories)
- Explore Ignite 2025 sessions about agent development and orchestration.

Developers can seamlessly enable the memory feature in the portal or leverage APIs and SDKs for fine-grained control and advanced configurations.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/introducing-memory-in-foundry-agent-service/)
