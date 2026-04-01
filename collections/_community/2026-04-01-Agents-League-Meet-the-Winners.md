---
date: 2026-04-01 07:00:00 +00:00
section_names:
- ai
- azure
- devops
- github-copilot
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/agents-league-meet-the-winners/ba-p/4507503
tags:
- Adaptive Cards
- Agent Architecture
- AI
- AI Agents
- Automated Testing
- Azure
- Azure AI Foundry
- Bicep
- Code Review
- Community
- DevOps
- GitHub Copilot
- Guardrails
- LLM Fallback Chain
- MCP
- MCP Server
- Microsoft Teams
- MIDI Export
- Multi Agent Systems
- OAuth Identity Passthrough
- OpenAI Responses API
- OpenTelemetry
- RAG
- SharePoint List Automation
- TypeScript
- VS Code Agent Mode
title: 'Agents League: Meet the Winners'
primary_section: github-copilot
author: aycabas
feed_name: Microsoft Tech Community
---

aycabas announces the Agents League category winners and highlights how each team built and shipped real AI-agent solutions—ranging from an MCP server inside GitHub Copilot to an enterprise multi-agent assistant deployed to Azure with Bicep and instrumented with OpenTelemetry.<!--excerpt_end-->

## Agents League: Meet the Winners

After weeks of reviewing 100+ submissions across three tracks, Agents League announced three category champions—each showing a different take on building AI agents with Microsoft developer tools.

## Creative Apps Winner: CodeSonify

[View project](https://github.com/microsoft/agentsleague/issues/36)

CodeSonify turns source code into music:

- Functions become ascending melodies
- Loops create rhythmic patterns
- Conditionals trigger chord changes
- Bugs produce dissonant sounds

Capabilities and implementation notes mentioned:

- Supports **7 programming languages** and **5 musical styles**
- Maps each language to its own **key signature**
- Uses **code complexity** to drive **tempo**

The team delivered three integrated experiences:

- A web app with **real-time visualization** and **one-click MIDI export**
- An **MCP server** that exposes **5 tools inside GitHub Copilot** in **VS Code Agent Mode**
- A **diff sonification engine** that lets you “hear” a code review (clean refactors sound harmonious; messy ones sound chaotic)

Engineering detail called out:

- The MIDI generator was built **from scratch in pure TypeScript**, with **zero external dependencies**
- Built entirely with **GitHub Copilot assistance**

## Reasoning Agents Winner: CertPrep Multi-Agent System

[View project](https://github.com/microsoft/agentsleague/issues/76)

CertPrep Multi-Agent System is a production-grade **8-agent** setup for personalized Microsoft certification exam prep.

Coverage:

- Supports **9 exam families**, including **AI-102**, **AZ-204**, **AZ-305**, and more

Agent responsibilities listed:

- Profile the learner
- Generate a week-by-week study schedule
- Curate learning paths
- Track readiness
- Run mock assessments
- Issue a **GO / CONDITIONAL GO / NOT YET** booking recommendation

Reliability and safety mechanisms mentioned:

- A **3-tier LLM fallback chain** so the system can run even without Azure credentials
- Pipeline completes in **under 1 second** in mock mode
- A **17-rule guardrail pipeline** validating agent boundaries
- **Largest Remainder algorithm** for study-time allocation so no domain is silently zeroed out
- **342 automated tests**

## Enterprise Agents Winner: Whatever AI Assistant (WAIA)

[View project](https://github.com/microsoft/agentsleague/issues/52)

WAIA is a production-ready multi-agent system for:

- **Microsoft 365 Copilot Chat**
- **Microsoft Teams**

How it works (as described):

- A workflow agent routes queries to specialized agents (HR, IT, or a fallback agent)
- Supports both **RAG-style Q&A** and **action automation**
- Includes IT ticket submission via a **SharePoint list**

Technical implementation highlights called out:

- Custom **MCP server** secured with **OAuth Identity Passthrough**
- **Streaming responses** via the **OpenAI Responses API**
- **Adaptive Cards** for human-in-the-loop approval flows
- Debug mode accessible directly from **Teams or Copilot**
- Full **OpenTelemetry** integration visible in the **Foundry** portal
- End-to-end automated deployment using **Bicep** so it can be deployed into any Azure environment

## Thank you

Agents League ends with a thank-you to everyone who submitted projects.

- Browse all submissions: https://github.com/microsoft/agentsleague/issues


[Read the entire article](https://techcommunity.microsoft.com/t5/microsoft-developer-community/agents-league-meet-the-winners/ba-p/4507503)

