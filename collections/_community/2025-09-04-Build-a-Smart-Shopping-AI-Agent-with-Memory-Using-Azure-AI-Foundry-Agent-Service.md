---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/build-a-smart-shopping-ai-agent-with-memory-using-the-azure-ai/ba-p/4450348
title: Build a Smart Shopping AI Agent with Memory Using Azure AI Foundry Agent Service
author: Bobur_Umurzokov
feed_name: Microsoft Tech Community
date: 2025-09-04 07:00:00 +00:00
tags:
- Agent Development
- AI Agents
- API Integration
- Azure AI Foundry
- FunctionTool
- GPT 4o
- Memori
- Memory Persistence
- Microsoft Azure
- Multi Agent Systems
- OpenAI
- Persistent Storage
- Personalization
- Python
- Recommendation Systems
- SQLite
- AI
- Azure
- Coding
- Community
section_names:
- ai
- azure
- coding
primary_section: ai
---
Bobur Umurzokov showcases how to build a smart shopping assistant using Memori with the Azure AI Foundry Agent service, enabling AI agents to remember customer data and personalize interactions effectively.<!--excerpt_end-->

# Build a Smart Shopping AI Agent with Memory Using Azure AI Foundry Agent Service

## Introduction

Human intelligence relies on memory to learn, adapt, and make better decisions. In the same way, AI agents become far more helpful when they remember past user interactions, preferences, budgets, and conversations. This guide demonstrates how to architect an AI shopping assistant that leverages memory, using Memori as a memory system in combination with the Azure AI Foundry Agent Service.

## Why Memory Matters for AI Agents

Ordinary agents can split a customer journey into several steps (plan, search, API calls, parse, write), but without memory, they forget earlier context, repeat actions, or miss key personalization opportunities. By integrating persistent memory, agents become more efficient, consistent, and personalized.

## Smart Shopping Experience: System Overview

- **Learns customer preferences:** Remembers purchases and likes
- **Personalized recommendations:** Suggests items based on prior shopping history
- **Budget-awareness:** Considers the user's spending constraints
- **Cross-category intelligence:** Connects shopping behaviors across different product types
- **Contextual conversations:** Retains ongoing context for multi-step shopping journeys
- **Gift recommendations:** Suggests gifts informed by purchase history

A full demo, complete with code, can be tried [on GitHub](https://github.com/GibsonAI/memori/tree/main/demos/smart_shopping_assistant), and a [live demo](https://smart-shopping-ai-agent.lovable.app/) is also available.

## Technical Architecture

- **Core Microsoft Technology:** [Azure AI Foundry Agent Service](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/overview)
- **Memory Layer:** [Memori](https://memori.gibsonai.com/) open-source memory SDK
- **Database:** Local SQLite (or PostgreSQL/MySQL)

### Sample Python Integration (Code Excerpt)

```python
from azure.ai.agents.models import FunctionTool
from azure.ai.projects import AIProjectClient
from azure.identity import DefaultAzureCredential
from memori import Memori, create_memory_tool
import os, json

# Constants

DATABASE_PATH = "sqlite:///smart_shopping_memory.db"
NAMESPACE = "smart_shopping_assistant"

# Azure provider config

azure_provider = ProviderConfig.from_azure(
    api_key=os.environ["AZURE_OPENAI_API_KEY"],
    azure_endpoint=os.environ["AZURE_OPENAI_ENDPOINT"],
    azure_deployment=os.environ["AZURE_OPENAI_DEPLOYMENT_NAME"],
    api_version=os.environ["AZURE_OPENAI_API_VERSION"],
    model=os.environ["AZURE_OPENAI_DEPLOYMENT_NAME"],
)

# Initialize Memori

memory_system = Memori(
    database_connect=DATABASE_PATH,
    conscious_ingest=True,
    auto_ingest=True,
    verbose=False,
    provider_config=azure_provider,
    namespace=NAMESPACE,
)
memory_system.enable()

memory_tool = create_memory_tool(memory_system)

def search_memory(query: str) -> str:
    try:
        if not query.strip():
            return json.dumps({"error": "Please provide a search query"})
        result = memory_tool.execute(query=query.strip())
        memory_result = str(result) if result else "No relevant shopping history found"
        return json.dumps({
            "shopping_history": memory_result,
            "search_query": query,
            "timestamp": datetime.now().isoformat(),
        })
    except Exception as e:
        return json.dumps({"error": f"Memory search error: {str(e)}"})
```

## Deployment Steps

1. **Install Dependencies:**

    ```bash
    pip install memorisdk azure-ai-projects azure-identity python-dotenv
    ```

2. **Set Azure Environment Variables:**

    ```bash
    export PROJECT_ENDPOINT="https://your-project.eastus2.ai.azure.com"
    export AZURE_OPENAI_API_KEY="your-azure-openai-api-key-here"
    export AZURE_OPENAI_ENDPOINT="https://your-resource.openai.azure.com/"
    export AZURE_OPENAI_DEPLOYMENT_NAME="gpt-4o"
    export AZURE_OPENAI_API_VERSION="2024-12-01-preview"
    ```

3. **Run the Demo:**

    ```bash
    python smart_shopping_demo.py
    ```

The demo walks through realistic shopping conversations — such as helping a user shop for an iPhone, matching accessories, and respecting the remembered budget from previous sessions.

## How Memori Enhances the Agent

- **Structured memory:** Validates and manages preferences using Pydantic
- **Short-term vs. long-term memory:** Keeps only critical personalized context
- **Multi-agent memory:** Shares knowledge among several shopping assistants
- **Automatic recording:** Captures all customer-agent interactions
- **Multi-tenancy:** Isolates user data using namespaces

## Real-World Extensibility

Developers can enhance the demo to:

- Expand product catalogs and connect to real inventory APIs
- Add tools (like "track my order" or price alerts)
- Integrate payment and delivery APIs
- Provide consistent context across web, mobile, and voice channels

## Conclusion

AI agents with persistent memory deliver smarter, more relevant, and personal user experiences. The combination of Memori and Azure AI Foundry Agent Service empowers developers to create enterprise-ready solutions that adapt and learn with every interaction.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/build-a-smart-shopping-ai-agent-with-memory-using-the-azure-ai/ba-p/4450348)
