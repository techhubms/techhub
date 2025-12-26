---
layout: "post"
title: "Integrating Azure Databricks Genie with Copilot Studio and Microsoft Foundry: Public Preview"
description: "This blog post authored by AnaviNahar and co-authored by Toussaint Webb details the public preview of AI/BI Genie integration with Microsoft Copilot Studio and Microsoft Foundry, streamlining access to conversational analytics and trusted data insights within the Microsoft ecosystem. It explains new simplified workflows for connecting Genie spaces to Copilot Studio agents and Foundry agents, leveraging Microsoft platform connectors and authentication, thus enabling organizations to scale data-driven solutions without complex custom code integrations."
author: "AnaviNahar"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/azure-databricks-genie-integration-with-copilot-studio-and/ba-p/4471087"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-18 18:48:18 +00:00
permalink: "/community/2025-11-18-Integrating-Azure-Databricks-Genie-with-Copilot-Studio-and-Microsoft-Foundry-Public-Preview.html"
categories: ["AI", "Azure", "ML"]
tags: ["AI", "Azure", "Azure Databricks", "Community", "Conversational Analytics", "Copilot Studio", "Data Democratization", "Data Insights", "Enterprise Data", "Entra ID", "Genie", "Integration", "Machine Learning", "MCP", "Microsoft Foundry", "ML", "OAuth", "Power Apps", "Teams Integration"]
tags_normalized: ["ai", "azure", "azure databricks", "community", "conversational analytics", "copilot studio", "data democratization", "data insights", "enterprise data", "entra id", "genie", "integration", "machine learning", "mcp", "microsoft foundry", "ml", "oauth", "power apps", "teams integration"]
---

AnaviNahar, in collaboration with Toussaint Webb, outlines how teams can easily integrate Azure Databricks Genie spaces with Copilot Studio and Microsoft Foundry agents, unlocking scalable conversational analytics across Microsoft platforms.<!--excerpt_end-->

# Integrating Azure Databricks Genie with Copilot Studio and Microsoft Foundry: Public Preview

**Authors:** AnaviNahar (Microsoft), Toussaint Webb (Databricks)

## Overview

Microsoft and Databricks have announced the public preview of AI/BI Genie integration with [Copilot Studio](https://www.microsoft.com/en/microsoft-copilot/microsoft-copilot-studio?market=af) and [Microsoft Foundry](https://azure.microsoft.com/en-us/products/ai-foundry), enabling organizations to democratize trusted data and accelerate business insights via conversational analytics tools.

## What is Genie?

[AI/BI Genie](https://learn.microsoft.com/en-us/azure/databricks/genie/) is a tool that empowers users to ask natural language questions about their organization's data (e.g., "What is my revenue growth this month?"). Genie interprets intent, generates queries, and returns insights.

## Integration Challenges (Prior State)

Previously, integrating Genie into Microsoft platforms required custom code and complex API management, creating architectural overhead and limitations for scaling insights.

## What’s New: Seamless Connection Through Microsoft Ecosystem

- **Genie + Copilot Studio:**
  - Genie spaces can now be directly connected to Copilot Studio agents via MCP (Model Context Protocol).
  - This integration eliminates custom development, making Genie’s insights instantly available in Teams or M365 Copilot-enabled workflows.

- **Genie + Microsoft Foundry:**
  - Developers can add Genie spaces as tools in Foundry agents, streamlining secure access to enterprise data during agent development.

## Step-by-Step Integration Guide

### Genie + Copilot Studio Setup

1. Connect Azure Databricks workspace to Power Apps using OAuth or Microsoft Entra ID Service Principal.
2. Open Copilot Studio.
3. Choose (or create) a Copilot Studio agent.
4. Go to 'Tools,' click '+Add a tool,' search for "Azure Databricks Genie" (or find it under Model Context Protocol).
5. Select and configure Genie space connection.
6. Publish the agent to target channels (e.g., Microsoft Teams).

**Tip:** Clearly title and describe Genie spaces for better orchestration.

### Genie + Microsoft Foundry Portal Setup

1. Open Microsoft Foundry.
2. Navigate to the 'Discover' tab tool catalog.
3. Select 'Azure Databricks Genie.'
4. Configure connection and click 'Connect.'
5. Use Genie in chosen Foundry agent.

## Getting Started

- [Genie + Copilot Studio Technical Documentation](https://learn.microsoft.com/en-us/azure/databricks/integrations/msft-power-platform)
- [Genie + Microsoft Foundry Documentation](https://learn.microsoft.com/en-us/azure/databricks/integrations/azure-ai-foundry)
- [Azure Databricks Power Platform Connector Blog](https://www.databricks.com/blog/introducing-azure-databricks-power-platform-connector-unleashing-governed-real-time-data-power)

## Key Benefits

- Reduced integration complexity via Microsoft connectors.
- Real-time analytics and conversational data access available instantly in Microsoft Teams and Foundry agents.
- Secure, managed API connectivity maintained via MCP protocols.
- Democratizes actionable insights for technical and non-technical users.

## Version & Release

- Published Nov 18, 2025
- Genie Integration Version 1.0

## Author

- AnaviNahar ([Profile](https://techcommunity.microsoft.com/t5/s/gxcuf89792/images/dS04MDM5OTktQlh3V0lu?image-coordinates=0%2C64%2C670%2C735&amp;image-dimensions=50x50))
- Co-authored by Toussaint Webb (Databricks)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/azure-databricks-genie-integration-with-copilot-studio-and/ba-p/4471087)
