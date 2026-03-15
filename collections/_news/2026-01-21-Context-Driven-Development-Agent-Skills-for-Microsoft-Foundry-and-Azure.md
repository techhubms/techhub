---
external_url: https://devblogs.microsoft.com/all-things-azure/context-driven-development-agent-skills-for-microsoft-foundry-and-azure/
title: 'Context-Driven Development: Agent Skills for Microsoft Foundry and Azure'
author: Govind Kamtamneni
feed_name: Microsoft All Things Azure Blog
date: 2026-01-21 13:57:59 +00:00
tags:
- Agent Skills
- AI Agents
- All Things Azure
- AZD Deployment
- Azure AI
- Azure AI Search
- Context7
- Cosmos DB
- FastAPI
- LLMs
- MCP Server
- Microsoft Foundry
- Pydantic
- Python
- React Flow
- SDK Integration
- Software Architecture
- TypeScript
- AI
- Azure
- News
- .NET
section_names:
- ai
- azure
- dotnet
primary_section: ai
---
Govind Kamtamneni explains how agent skills for Microsoft Foundry and Azure enable modular, context-driven agent development in enterprise environments, providing practical steps and technical insights.<!--excerpt_end-->

# Context-Driven Development: Agent Skills for Microsoft Foundry and Azure

*Author: Govind Kamtamneni*

Enterprise AI workloads are increasingly adopting modular, greenfield microservices rather than large, monolithic codebases. In this environment, coding agents—powered by LLMs—can generate much of the necessary code. However, out-of-the-box agents often lack deep domain knowledge of specific SDKs and patterns. This is where agent skills come in: they inject context, specialization, and domain expertise into AI agents for work on Microsoft Foundry and Azure.

## What Are Agent Skills?

The [agent-skills GitHub repository](https://github.com/microsoft/agent-skills) provides a curated set of skills, prompts, agents, and MCP (Multi-Context Provider) configurations. These help agents understand and implement domain-specific coding patterns, including for Cosmos DB, Foundry IQ, Azure AI services, and various deployment workflows.

### Repository Contents

- **Skills**: Modular packages for domains like Cosmos DB, Foundry, AZD Deployment, FastAPI, and more
- **Prompts**: Reusable templates for reviews, component scaffolding, endpoints
- **Agents**: Persona definitions for backend, frontend, and planning roles
- **MCP Servers**: Integrations for GitHub, Playwright, Microsoft Docs, and Context7
- **Context7 Integration**: Indexed Foundry documentation at [context7.com/microsoft/agent-skills](https://context7.com/microsoft/agent-skills)

## Best Practices: Context Management and Skills Curation

- **Avoid context rot**: Only load skills relevant to your project to prevent the agent from becoming overwhelmed (high signal-to-noise is critical)
- **Limit MCP servers**: Only enable those tools truly needed for the current developer workflow

## Getting Started (Quick Start)

1. **Copy only required skills:**

   ```bash
   git clone https://github.com/microsoft/agent-skills.git
   cp -r agent-skills/.github/skills/cosmos-db-python-skill your-project/.github/skills/
   cp -r agent-skills/.github/skills/foundry-iq-python your-project/.github/skills/
   # Or use symlinks for multi-project setups
   ```

2. **Configure your agent:**
   - *GitHub Copilot*: Skills auto-discovered from `.github/skills/`
   - *Claude Code*: Reference `SKILL.md` in instructions
3. **Add MCP servers as needed:**
   - Copy configuration, remove unused servers for optimal performance
4. **(Optional) Use Context7 for latest Foundry docs**
   - Add the Context7 MCP server ([see documentation](https://context7.com/microsoft/agent-skills))

## Example Agent Skills Provided

| Skill                   | Description                                           |
|-------------------------|-------------------------------------------------------|
| azd-deployment          | Azure Developer CLI deployment to Container Apps      |
| azure-ai-search-python  | Patterns for Azure AI Search SDK (vector/hybrid, etc) |
| cosmos-db-python-skill  | Cosmos DB NoSQL patterns with Python/FastAPI          |
| foundry-iq-python       | Agentic retrieval with Foundry Agent Service          |
| pydantic-models         | Multi-model Pydantic v2 usage patterns                |
| react-flow-node         | TypeScript React Flow custom nodes                    |
| ...more in repo         |

## Scaling Recipes and Advanced Patterns

For larger codebases or when static skills are insufficient:

- Implement AST-based memory systems (hierarchical graph representations)
- Use knowledge graphs for component and dependency relationships

These help your agents maintain global context without exceeding their token window.

## Ongoing Work and Next Steps

- More idiomatic skills for Microsoft SDKs and frameworks will be added
- Automated skill quality testing using [copilot-sdk](https://github.com/github/copilot-sdk) and sample harnesses ([copilot-sdk-samples/skill-testing](https://github.com/microsoft-foundry/copilot-sdk-samples/tree/main/samples/skill-testing/sdk))

## Resources

- [Agent Skills Repository](https://github.com/microsoft/agent-skills)
- [Context7: Foundry Docs](https://context7.com/microsoft/agent-skills)
- [Copilot SDK](https://github.com/github/copilot-sdk)
- [Claude Code + Foundry Setup Guide](https://devblogs.microsoft.com/all-things-azure/claude-code-microsoft-foundry-enterprise-ai-coding-agent-setup/)
- [Microsoft Foundry Docs](https://learn.microsoft.com/en-us/azure/ai-foundry/)

This post appeared first on "Microsoft All Things Azure Blog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/context-driven-development-agent-skills-for-microsoft-foundry-and-azure/)
