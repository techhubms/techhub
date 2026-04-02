---
primary_section: ai
feed_name: Microsoft News
section_names:
- ai
- devops
- ml
date: 2026-04-02 16:23:57 +00:00
external_url: https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/new-and-improved-multi-agent-orchestration-connected-experiences-and-faster-prompt-iteration/
author: stclarke
tags:
- Agent Orchestration
- Agent To Agent (a2a)
- AI
- AI Agents
- Anthropic Claude
- Apps SDK
- Azure DevOps Connector
- CI/CD
- Company News
- Content Moderation
- DevOps
- Evaluation Automation APIs
- Governance Controls
- Managed Models
- MCP (model Context Protocol)
- Meeting Transcripts
- Microsoft 365 Agents SDK
- Microsoft Copilot Studio
- Microsoft Fabric
- Microsoft Teams Meetings Agents
- ML
- Model Selection
- Multi Agent Systems
- News
- Open Agent Protocols
- OpenAI Models
- Prompt Builder
- Prompt Editor
- Prompt Engineering
- Retrieval Quality
- ServiceNow Connector
- Xai Grok
title: 'What’s new in Copilot Studio: Updates to multi-agent systems'
---

stclarke summarizes the latest Microsoft Copilot Studio updates, including generally available multi-agent orchestration (Fabric, Microsoft 365 Agents SDK, and A2A), an immersive Prompt Builder for faster iteration, and new governance and evaluation capabilities aimed at running agents more reliably in production.<!--excerpt_end-->

# What’s new in Copilot Studio: Updates to multi-agent systems

Learn what's new in Copilot Studio: multi-agent systems are now generally available, plus recent updates to the Prompt Editor and governance controls.

Microsoft Copilot Studio helps organizations build connected systems of agents (not isolated experiences) and manage how those agents behave in production.

## Agents that work together across your entire ecosystem

Scaling AI in an organization is less about building *one* useful agent and more about getting many agents—across teams and tools—to work together reliably.

Typical issue: different teams build separate agents (data, app, productivity). The moment a workflow needs knowledge/reasoning/actions across multiple systems, teams hit brittle handoffs and custom integration work, which slows adoption.

Copilot Studio is rolling several multi-agent capabilities to **general availability** over the next few weeks:

- Microsoft Fabric integration
- Microsoft 365 Agents SDK orchestration
- Agent-to-Agent (A2A) communication

### Multi-agent support for Microsoft Fabric

With multi-agent support, Copilot Studio agents can work with **Fabric agents** to reason over enterprise data and analytics at scale.

Goal: connect business-facing agent experiences to an existing data estate without turning each data-heavy scenario into a one-off engineering project.

![Agent creation screen in Copilot Studio with the heading “Connect an existing agent.” Under the heading are two options: Copilot Studio agents and Microsoft Fabric agents.](https://www.microsoft.com/en-us/microsoft-copilot/blog/wp-content/uploads/2024/08/Extend-Copilot-Studio-agent-Microsoft-Fabric-crop-1105px-1024x412.webp)

### Multi-agent support for the Microsoft 365 Agents SDK

Using the **Microsoft 365 Agents SDK**, teams can orchestrate Copilot Studio agents alongside agents built for Microsoft 365 experiences.

Instead of recreating logic across multiple agents (e.g., retrieving data, applying business rules, completing common tasks), teams can reuse and combine existing capabilities to build cross-app workflows with less duplication.

### Agent-to-Agent (A2A) support

With **A2A support**, Copilot Studio agents can communicate with and delegate work to other agents—first-party, second-party, or third-party—using an **open protocol**.

The point is interoperability: enterprise AI workflows often span multiple stacks, so the platform needs to participate in a broader ecosystem rather than being constrained to a single product boundary.

## The impact of multi-agent systems

Example: the **Ask Microsoft web agent** (“customer zero”). As site traffic and knowledge sources grew, a single-agent architecture created slower response times. The team upgraded to a modern architecture with generative orchestration and multi-agent coordination.

![Diagram of Microsoft’s Ask Microsoft web agent on Copilot Studio. Diagram shows customer using one agent, which connects to five expert sub-agents and external knowledge sources.](https://www.microsoft.com/en-us/microsoft-copilot/blog/wp-content/uploads/2024/08/Ask-Microsoft-web-agent-on-Copilot-Studio-1024x438.jpg)

Now multiple sub-agents cover areas like Microsoft Azure, Microsoft 365, pricing, and trials, with a main agent orchestrating fast multi-turn responses.

> Building a more advanced assistant with Copilot Studio has meaningfully raised the bar for our customer experience and enabled us to scale faster across products to deliver real business impact
>
> – *Alyse Muttera, Director of eCommerce Programs at Microsoft*

Another scenario: a bank with separate agents for mortgages vs account inquiries. Multi-agent orchestration coordinates specialized agents behind the scenes so the customer gets one seamless experience.

A customer example: ask about a mortgage payment and an account balance in the same interaction. The system returns a unified, context-aware answer without forcing the user to juggle multiple interfaces.

Customer example: Coca‑Cola Beverages Africa uses Copilot Studio agents and Microsoft Dynamics 365 to autonomously run planning cycles and automate workflows end to end, saving planners 1 to 1.5 hours per day.

Watch: Coca‑Cola Beverages Africa automates with Copilot Studio agents (YouTube)

These features will be fully available to all eligible customers as of **April 2026**.

## Build prompts faster while maintaining control

As agent experiences become more sophisticated, prompt quality and iteration speed matter more.

Historically, prompt iteration could feel disjointed: switch to a separate editor, make a change, test, repeat.

### Immersive Prompt Builder (generally available)

The new **immersive Prompt Builder** brings prompt editing directly into each agent’s **Tools** tab:

- update instructions
- switch models
- add inputs or knowledge
- test changes

All from one place, reducing context-switching during iteration.

![The immersive Prompt Builder inside a Helpdesk Agent. The user is building a prompt for the agent to extract customer information from a document.](https://www.microsoft.com/en-us/microsoft-copilot/blog/wp-content/uploads/2024/08/Immersive-prompt-builder-in-agent-tools-1920px-1024x576.webp)

Example scenario: clinical documentation support. Teams may need to refine instructions, swap a knowledge source, and test output against domain terminology that can trigger safeguards. Doing this in one workspace can reduce friction while tuning toward production readiness.

### More options for prompts: content moderation and model choice

Copilot Studio added **content moderation settings for prompts** (generally available in supported regions). Makers can control harmful-content sensitivity on managed models, including lowering sensitivity to support legitimate scenarios in regulated domains (healthcare, insurance, law enforcement) where defaults may be too restrictive.

The Prompt Tool also supports **Anthropic Claude Opus 4.6** and **Claude Sonnet 4.5** in paid experimental preview in the United States, providing more model choice for balancing performance, reasoning depth, and cost.

## What else is new and improved in Copilot Studio

Additional updates across automation, meetings, retrieval quality, and model support:

- **ServiceNow and Azure DevOps connector quality improvements** (generally available) to improve operational question handling and retrieval of tickets/work items.
- **Evaluation automation APIs** (generally available) through Microsoft Power Platform APIs and connectors to run evaluations programmatically and integrate quality checks into **CI/CD** workflows.
- **Agents for Microsoft Teams meetings** can access real-time meeting transcripts and group chat for in-meeting Q&A, surfacing relevant information, and tracking decisions/follow-ups.
- **Model context protocol (MCP) apps and Apps SDK support** expanded how agents connect to external work apps, aimed at making business-system integration easier.
- **Additional model support** in paid experimental preview, including **Grok 4.1 Fast**, **GPT-5.3 Thinking**, and **GPT-5.4 Instant**.

![Dropdown menu showing the latest OpenAI, Anthropic, and xAI models available to choose for Copilot Studio agents.](https://www.microsoft.com/en-us/microsoft-copilot/blog/wp-content/uploads/2024/08/Model-selection-CopilotStudio-V2.webp)

## Stay up to date

Microsoft notes more is coming in April 2026 across voice channels, workflows, and the building experience.

Key links:

- Build and manage agents with Copilot Studio: https://aka.ms/CopilotStudio
- Sign up for a free Copilot Studio trial: https://aka.ms/TryCopilotStudio
- What’s new in Microsoft Copilot Studio (Learn): https://learn.microsoft.com/en-us/microsoft-copilot-studio/whats-new
- Multi-agent support for Microsoft Fabric: https://learn.microsoft.com/en-us/microsoft-copilot-studio/add-agent-fabric-data-agent
- Multi-agent support for Microsoft 365 Agents SDK: https://learn.microsoft.com/en-us/microsoft-copilot-studio/add-agent-microsoft-365-agents-sdk-agent
- Agent-to-Agent (A2A) support: https://learn.microsoft.com/en-us/microsoft-copilot-studio/add-agent-agent-to-agent
- Add a prompt as a tool: https://learn.microsoft.com/microsoft-copilot-studio/nlu-prompt-node#add-a-prompt-as-a-tool-to-an-agent


[Read the entire article](https://www.microsoft.com/en-us/microsoft-copilot/blog/copilot-studio/new-and-improved-multi-agent-orchestration-connected-experiences-and-faster-prompt-iteration/)

