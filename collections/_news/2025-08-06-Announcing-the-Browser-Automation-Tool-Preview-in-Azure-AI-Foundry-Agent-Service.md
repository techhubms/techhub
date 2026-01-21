---
external_url: https://devblogs.microsoft.com/foundry/announcing-the-browser-automation-tool-preview-in-azure-ai-foundry-agent-service/
title: Announcing the Browser Automation Tool (Preview) in Azure AI Foundry Agent Service
author: Linda Li
feed_name: Microsoft DevBlog
date: 2025-08-06 15:00:56 +00:00
tags:
- Agent Setup
- Agentic AI
- AI Agent
- AI Agents
- API Integration
- Automation Security
- Azure AI Foundry
- Azure SDK
- Azure Subscription
- Browser Automation Tool
- Cloud Browser Automation
- Code Sample
- Form Automation
- Isolated Execution
- Multi Turn AI Agents
- Natural Language Automation
- Natural Language Prompts
- Playwright
- Playwright Workspaces
- Python SDK
- Web UI Automation
section_names:
- ai
- azure
- coding
---
Linda Li introduces the new Browser Automation Tool (Preview) in Azure AI Foundry Agent Service, enabling developers to automate browser tasks via natural language using Playwright Workspaces in Azure.<!--excerpt_end-->

# Announcing the Browser Automation Tool (Preview) in Azure AI Foundry Agent Service

**Author:** Linda Li

## Overview

Microsoft has released the Browser Automation Tool (Preview) as the newest action tool in Azure AI Foundry Agent Service, accessible via API and SDK. This tool empowers developers to build AI agents capable of automated browser-based workflows—such as searching, form-filling, booking appointments, and navigating web applications—all through natural language prompts. The tool leverages [Playwright Workspaces](https://aka.ms/pww/docs/overview) to provide isolated, cloud-hosted browser sessions.

## Key Benefits

- **Natural Language to Automation:** Easily translate user intents into browser actions.
- **Realistic Web Interactions:** Handle multi-step tasks like bookings, form fills, and navigation, simulating real user interactions.
- **Isolated Execution:** Browser sessions run in Azure-hosted, sandboxed environments—no manual maintenance of VMs or browsers.
- **Multi-turn Conversations:** Agents can iteratively refine automation tasks in response to ongoing user input.
- **Robust Automation:** The tool analyzes DOM and accessibility trees for semantic interaction, going beyond pixel-based automation.
- **Diverse Use Cases:** Handle workflows such as reservations, product research, or updating data in web forms.

## Example Use Cases

- **Booking & Reservations:** Automate class sign-ups, appointments, or reservations through complex forms.
- **Product Discovery:** Perform product and review searches, returning structured summaries.
- **Web Form Submission:** Automate uploading documents or editing user profiles.
- **Customer Support Operations:** Fetch ticket or account data across multiple web services.

## How It Works

1. **User Query:** User provides a request in natural language, e.g., "Show me yoga classes this week at [site]".
2. **Session Provisioning:** Azure AI Foundry Agent Service connects to Playwright Workspaces, starting a sandboxed browser session in your Azure subscription.
3. **Agent Reasoning:** The agent parses page structures (DOM) to determine required actions.
4. **Action Execution:** Browser Automation Tool executes step-by-step actions within the session, reporting updates after each.
5. **Multi-turn Loop:** Agent and user iterate until the desired task is achieved.

This allows AI agents to deliver reliable automation, combining large language models (LLMs) with robust, programmatic web control.

## Security & Responsible Use

**Warning:** Browser automation carries potential security risks—unintended actions by the agent, or malicious web content, may jeopardize data or system integrity. You are responsible for safe use and should restrict agent access to low-privilege environments. See the [Transparency Note](https://learn.microsoft.com/en-us/azure/ai-foundry/responsible-ai/agents/transparency-note#enabling-autonomous-actions-with-or-without-human-input-through-action-tools) for guidance.

## Sample Python Code

Below is an excerpt showing how to configure and use the Browser Automation Tool with Azure AI Foundry SDK and Playwright Workspaces:

```python
from azure.identity import DefaultAzureCredential
from azure.ai.agents import AgentsClient
from azure.ai.projects import AIProjectClient

project_endpoint = "https://<your-ai-services-resource-name>.services.ai.azure.com/api/projects/<your-project-name>"
project_client = AIProjectClient(endpoint=project_endpoint, credential=DefaultAzureCredential())
playwright_connection = project_client.connections.get(name="YOUR_PLAYWRIGHT_CONNECTION_NAME")

with project_client:
    agent = project_client.agents.create_agent(
        model="YOUR_MODEL_NAME",
        name="my-agent",
        instructions="use the tool to respond",
        tools=[{
            "type": "browser_automation",
            "browser_automation": {
                "connection": {"id": playwright_connection.id}
            }}],
    )
    thread = project_client.agents.threads.create()
    message = project_client.agents.messages.create(thread_id=thread.id, role="user", content="YOUR_QUERY_TO_THE_AGENT")
    run = project_client.agents.runs.create_and_process(thread_id=thread.id, agent_id=agent.id)
    # Process run status, outputs, and clean up as appropriate
```

## Getting Started

**Prerequisites:**

- An Azure subscription with permissions to provision Playwright Workspaces and AI Foundry resources
- Python 3.8+ (compatible with the Azure AI Foundry SDK)

**Setup Steps:**

1. [Create a Playwright Workspace Resource](https://aka.ms/pww/docs/manage-workspaces)
2. [Generate an Access Token](https://aka.ms/pww/docs/manage-access-tokens)
3. Assign suitable roles to your project identity ([see guide](https://aka.ms/pww/docs/manage-workspace-access))
4. Connect Playwright Workspace to Foundry via the Azure AI Foundry portal, providing endpoints and access keys
5. Configure your agent using the SDK, following code samples provided above

## Further Learning

- [Azure AI Foundry documentation](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/tools/browser-automation)
- [SDK download](https://aka.ms/aifoundrysdk)
- [Azure AI Foundry Learn courses](https://aka.ms/CreateAgenticAISolutions)
- [GitHub forum](https://aka.ms/azureaifoundry/forum)
- [Discord community](https://aka.ms/azureaifoundry/discord)

Microsoft encourages developers to explore creative, secure web automation experiences using this preview technology.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/foundry/announcing-the-browser-automation-tool-preview-in-azure-ai-foundry-agent-service/)
