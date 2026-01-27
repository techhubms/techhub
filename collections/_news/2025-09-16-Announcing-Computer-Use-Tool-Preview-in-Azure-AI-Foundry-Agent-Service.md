---
external_url: https://devblogs.microsoft.com/foundry/announcing-computer-use-tool-preview-in-azure-ai-foundry-agent-service/
title: Announcing Computer Use Tool (Preview) in Azure AI Foundry Agent Service
author: Linda Li
feed_name: Microsoft AI Foundry Blog
date: 2025-09-16 15:00:45 +00:00
tags:
- Agent Service
- Automation
- Azure AI Foundry
- Browser Automation
- Computer Use Tool
- Desktop Automation
- Enterprise Security
- Human in The Loop
- Legacy Integration
- Microsoft
- Natural Language AI
- Operational Copilots
- Pixel Based Reasoning
- Prompt Injection
- REST API
- SDK
section_names:
- ai
- azure
primary_section: ai
---
Linda Li announces the preview availability of the Computer Use tool in Azure AI Foundry Agent Service, offering developers new automation and agent capabilities integrated with advanced security features.<!--excerpt_end-->

# Announcing Computer Use Tool (Preview) in Azure AI Foundry Agent Service

## Overview

The Computer Use tool is now available in preview as part of Azure AI Foundry Agent Service. This tool provides feature parity with the Azure OpenAI Responses API, plus seamless integration into Foundry agent runtime and enterprise-grade security. Developers can build agents that not only reason over text, call APIs, and retrieve knowledge, but also interact with computer systems using natural language.

Accessible via REST API and SDK, developers can embed Computer Use into their applications, automation workflows, and pipelines.

## Key Use Cases

- **Web & desktop automation:** Agents can fill forms, upload/download artifacts, and interact with apps lacking APIs.
- **Operational copilots:** Automate support workflows such as ticket triage or dashboard management across multiple enterprise applications.
- **Legacy integration:** Interface with older desktop apps by simulating keystrokes and mouse clicks.
- **Human-in-the-loop workflows:** Require user approval for sensitive or high-risk operations before execution.

## How It Works

- **Action loop:** The agent requests an action (e.g., click, type, screenshot) from the computer-use-preview model.
- **Execution environment:** The requested action runs in a controlled environment (browser or desktop) with a screenshot taken after execution.
- **Screenshot feedback:** The screenshot is sent back to the agent for the next step.
- **Pixel-based reasoning:** Instead of parsing HTML/DOM, Computer Use interprets raw pixels, adapting to dynamic or unfamiliar user interfaces.
- **Safety checks:** Instructions are monitored for potential risks (malicious or sensitive actions), requiring human acknowledgment for such cases.

|                | Browser Automation                         | Computer Use                              |
|----------------|--------------------------------------------|-------------------------------------------|
| Model support  | Most Foundry Agent Models                  | computer-use-preview model                |
| Visualization  | No                                         | Yes                                       |
| Screen Parsing | DOM/HTML/XML                               | Raw pixel screenshots                     |
| Actions        | API-driven                                 | Virtual keyboard and mouse                |
| Multi-step     | Yes                                        | Yes                                       |
| Interfaces     | Browser                                    | Browser + desktop                         |
| Resource setup | BYO Playwright resource and key management | No external resource (VM recommendations) |

**Note:** Due to the powerful capabilities, running agents on low-privilege, non-sensitive virtual machines is strongly recommended.

## Security and Responsible Use

Computer Use involves security and privacy risks, including prompt injection. Developers should consult the [Azure OpenAI transparency note](https://learn.microsoft.com/en-us/azure/ai-foundry/responsible-ai/openai/transparency-note?tabs=image%22) for risk guidelines, intended uses, and required mitigations.

## Sample Code

A code sample is provided for building and operating agents with the Computer Use tool, demonstrating usage of the preview SDK and API for setting up execution environments, message threads, and screenshot feedback. The code outlines project setup, environment variables, and safe execution strategies:

```python
# Import libraries and set up environment

from azure.ai.projects import AIProjectClient
from azure.ai.agents.models import ComputerUseTool
from azure.identity import DefaultAzureCredential

project_client = AIProjectClient(endpoint=os.environ["PROJECT_ENDPOINT"], credential=DefaultAzureCredential())
environment = os.environ.get("COMPUTER_USE_ENVIRONMENT", "windows")
computer_use = ComputerUseTool(display_width=1026, display_height=769, environment=environment)

with project_client:
    agents_client = project_client.agents
    agent = agents_client.create_agent(
        model=os.environ["MODEL_DEPLOYMENT_NAME"],
        name="my-agent-computer-use",
        instructions="You are an automation assistant. Use computer_use_preview to interact with the screen.",
        tools=computer_use.definitions,
    )
```

The full sample demonstrates how to:

- Configure the project and agent
- Handle messages and runs in a thread
- Provide and process screenshots as feedback for each agent action
- Safely operate the agent and manage run steps

## Getting Started

1. **Request access:** Apply for computer-use-preview in regions (East US 2, Sweden Central, South India) via [this link](https://aka.ms/oai/cuaaccess).
2. **Create deployments:** Deploy the tool in your Azure OpenAI resource after approval.
3. **Configure your agent:** Use the REST API or SDK to specify execution environments and set up the action loop.
4. **Apply best practices:** Always run Computer Use agents on low-privilege, isolated machines.

**Useful Links:**

- [Computer Use tool in Azure AI Foundry](https://learn.microsoft.com/en-us/azure/ai-foundry/agents/how-to/tools/computer-use)
- [Transparency notes for action tools](https://learn.microsoft.com/en-us/azure/ai-foundry/responsible-ai/agents/transparency-note)

With these resources, developers can quickly embed advanced automation and agent capabilities into enterprise AI applications, combining strong security with flexible integration and oversight.

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/announcing-computer-use-tool-preview-in-azure-ai-foundry-agent-service/)
