---
external_url: https://devblogs.microsoft.com/foundry/announcing-azure-language-in-foundry-tools-for-deterministic-privacy-first-agents/
title: Announcing Azure Language in Foundry Tools for Deterministic, Privacy-First Agents
author: Xiaoying
feed_name: Microsoft AI Foundry Blog
date: 2025-11-20 16:00:12 +00:00
tags:
- AI Agent
- AI Agents
- AI Development
- AI Tools
- Authentication
- Azure AI
- Azure AI Foundry
- Azure AI Services
- Azure Language
- CLU
- Conversational Language Understanding
- Determinism
- Entity Extraction
- Foundry Tools
- Healthcare Entity Extraction
- Intent Detection
- Intent Routing
- Key Phrase Extraction
- Language Detection
- MCP
- MCP Server
- Microsoft Entra ID
- Microsoft Foundry
- MSIgnite
- Multi Turn Conversations
- Named Entity Recognition
- Pii Redaction
- Privacy
- Privacy Control
- Sentiment Analysis
- Slot Filling
- Text Summarization
section_names:
- ai
- azure
---
Xiaoying breaks down the new Azure Language features in Foundry Tools, highlighting privacy-first AI agents, advanced PII redaction, and deterministic intent routing for developers building enterprise solutions on Azure.<!--excerpt_end-->

# Announcing Azure Language in Foundry Tools for Deterministic, Privacy-First Agents

As the demand for robust and compliant AI agent architectures accelerates, Microsoft introduces new Azure Language capabilities within Foundry Tools. With a focus on privacy, security, and determinism, these tools equip developers to build agents that meet enterprise requirements for predictability and compliance.

## Key Enhancements

### Azure Language Remote MCP Server

- **Centralized Language AI Tools:** Access a suite of API-driven Language AI services (PII redaction, intent detection, entity extraction, question answering, and more) through a cloud-hosted Model Context Protocol (MCP) server, simplifying agent workflows.
- **Seamless Integration:** Native support for Foundry resource endpoints, two authentication methods (key-based and Microsoft Entra ID/RBAC), and straightforward addition via Foundry portal or agent playground.
- **No infrastructure management:** Cloud-hosted, fully managed, and agent-ready.

### Supported Language AI Capabilities

- **PII Redaction:** Protect privacy in text or native documents by detecting and masking personal identifiers.
- **Intent Detection (CLU):** Accurately route user queries by extracting intents and entities from conversational messages.
- **Exact Question Answering:** Retrieve grounded answers from configured knowledge bases, supporting compliance-heavy scenarios.
- **Named Entity Recognition & Healthcare Entity Extraction:** Identify people, organizations, medical entities, and financial terms.
- **Sentiment Analysis & Opinion Mining:** Monitor sentiment levels for customer support or stakeholder feedback workflows.
- **Text Summarization & Key Phrase Extraction:** Highlight actionable insights from meetings, documents, and conversations.
- **Language Detection:** Support multilingual workflows and onboarding for users in various locales.

### Example Agent Patterns

- **Privacy-Preserving AI Assistants:** Use PII Redaction for healthcare, claims, or sensitive HR workflows.
- **Case Routing:** Leverage Intent Detection and sentiment analysis to automate customer inquiries and escalation.
- **Enterprise Knowledge Assistants:** Deliver reliable, policy-grounded responses using knowledge-based QA.
- **Compliance & Operations:** Turn large documents into structured insights with NER, summarization, and key phrase extraction.
- **Multilingual HR Assistants:** Detect languages and extract relevant entities for global teams.
- **Meeting/Productivity Assistants:** Summarize key decisions and follow-ups from transcripts or documents.

## Getting Started

1. **Connect to MCP Endpoint**
    - Example: `https://{foundry-resource-name}.cognitiveservices.azure.com/language/mcp?api-version=2025-11-15-preview`
2. **Authenticate**
    - Use key from Foundry resource (for prototyping).
    - Use Microsoft Entra ID for RBAC in enterprise scenarios.
3. **Configure in Foundry Portal**
    - Add from Foundry Tools catalog, or directly to agents via agent playground.

## Integration in VS Code with GitHub Copilot

- Connect Azure Language MCP server in VS Code (using mcp.json and correct headers).
- Test redaction, intent, and other AI tools directly from your developer environment.

## Advanced Privacy & PII Redaction

- **Improved Model (2025-11-15-preview):** Supports more entity types (Airport, City, Geopolitical Entity, etc.) and enhanced accuracy.
- **Synthetic Replacement:** Mask PII with realistic synthetic values for safer sharing and analysis.
- **Analytics:** Confidence scores, entity offset, and detailed tags for granular review.
- **Customer Validation:** Real-world improvements with users like Nationwide Building Society (over 90% redaction accuracy).

## Deterministic Intent Routing with CLU

- **Multi-turn Understanding:** Accurately interpret full conversation context for better intent prediction.
- **Slot-Filling:** Map entities to intents, identify missing information, and enable smarter bot interactions for processes like flight booking.
- **Quick Deploy with LLM:** Streamlined deployment and playground testing for custom CLU models.

## Summary

With Azure Language in Foundry Tools, developers gain access to a scalable, privacy-first Language AI foundation built for modern agentic solutions. Whether you're automating customer service, compliance tasks, or knowledge workflows, these tools enable confident innovation on Azure.

**Learn More:**

- [Azure Language in Foundry Tools](https://aka.ms/azure-language)
- [Quickstart: Multi-turn conversational language understanding with CLU](https://learn.microsoft.com/en-us/azure/ai-services/language-service/conversational-language-understanding/how-to/quickstart-multi-turn-conversations)
- [Foundry Playground](https://ai.azure.com)
- [What's New in Azure Language](https://learn.microsoft.com/en-us/azure/ai-services/language-service/whats-new?tabs=csharp)

This post appeared first on "Microsoft AI Foundry Blog". [Read the entire article here](https://devblogs.microsoft.com/foundry/announcing-azure-language-in-foundry-tools-for-deterministic-privacy-first-agents/)
