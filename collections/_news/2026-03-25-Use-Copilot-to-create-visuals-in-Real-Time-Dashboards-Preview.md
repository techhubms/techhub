---
primary_section: ai
tags:
- AI
- Business Intelligence Development
- Copilot
- Dashboard Editing
- Dashboard Tiles
- Data Exploration
- Data Visualization
- Fabric Capacity
- KQL
- Kusto Query Language
- Microsoft Fabric
- ML
- Natural Language Queries
- News
- Preview Feature
- Query Generation
- Real Time Dashboards
- Real Time Intelligence
title: Use Copilot to create visuals in Real-Time Dashboards (Preview)
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/use-copilot-to-create-visuals-in-real-time-dashboards-preview/
date: 2026-03-25 13:30:00 +00:00
feed_name: Microsoft Fabric Blog
section_names:
- ai
- ml
---

Microsoft Fabric Blog introduces (in preview) Copilot-assisted editing for Real-Time Dashboards in Microsoft Fabric, letting dashboard editors describe the insight they want in natural language while Copilot generates KQL, previews results, and suggests an appropriate visual.<!--excerpt_end-->

## Overview

Real-Time Dashboards in **Microsoft Fabric** can monitor live data and share insights with a team. A new **Copilot integration (Preview)** in the dashboard editing experience lets you create and modify dashboard visuals using **natural language**, without needing deep **Kusto Query Language (KQL)** expertise.

For a broader set of announcements, the post points readers to Arun Ulag’s roundup: [FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news).

## The challenge of creating dashboard visuals

Creating effective dashboard visuals typically requires:

- Understanding the data
- Knowing how to query the data

For many dashboard editors, the querying part is the blocker. Time gets spent looking up syntax, debugging query errors, and iterating on KQL before they can focus on presenting insights. This can slow dashboard development or create reliance on colleagues with KQL expertise.

## How Copilot helps dashboard editors

Copilot is available directly in the **visuals editing** experience.

When editing a dashboard visual (creating a new one or modifying an existing one):

- Open the **Copilot pane**
- Ask questions about the data source in **natural language**

Example prompts mentioned:

- “Show me the top 10 products by revenue in the last 30 days”
- “What’s the average response time by region?”

Copilot interprets the question and generates a **KQL query** to retrieve relevant data.

![New tile being created with the Copilot pane open for natural language questions](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/new-tile-being-created-copilot-pane-is-open-and.png)

*Figure: New tile added to a dashboard with Copilot assistant.*

![Real-time dashboard visual in edit mode after applying Copilot’s answer](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/real-time-dashboard-visual-in-edit-mode-after-copi.png)

*Figure: Real-time dashboard visual in edit mode after Copilot answer has been applied.*

### What Copilot returns

Copilot provides three outputs:

- **Suggested visual**: a visualization type appropriate for the returned data
- **Data table**: a preview of query results
- **KQL query**: the generated query used to retrieve the data

### How editors can iterate

- Accept Copilot’s suggestion as-is
- Refine with follow-up questions
- Edit the generated KQL directly for more control
- Use the dashboard’s no-code formatting options to customize the visual

## Use cases

### Quick visual creation

Describe the insight needed and let Copilot handle the query logic, including:

- Aggregations (e.g., “total sales by category”)
- Trends (e.g., “orders over the past week”)
- Comparisons (e.g., “top five regions by customer count”)

### Iterative refinement

Start broad and narrow down with follow-ups, for example:

- “Show me all error events”
- “Filter to critical errors only”
- “Group by error type”

Copilot maintains context so follow-ups build on prior requests.

### Enabling non-KQL experts

Business analysts and report creators who understand the data but don’t write KQL regularly can build dashboard visuals independently, reducing bottlenecks.

## Get started

To use Copilot when editing **Real-Time Dashboard** visuals, you need:

- A workspace with a **Microsoft Fabric-enabled capacity**
- A **Real-Time Dashboard** with at least one connected data source

Steps:

1. Open your dashboard
2. Switch to **Edit mode**
3. Select the **Copilot icon** when creating or editing a tile
4. Enter a natural language question and review:
   - Suggested visual
   - Data preview
   - Generated KQL

For step-by-step instructions, see: [Copilot-assisted real-time data exploration](https://learn.microsoft.com/fabric/real-time-intelligence/dashboard-explore-data)

## Next steps

- Learn more: [Create a Real-Time Dashboard](https://learn.microsoft.com/fabric/real-time-intelligence/dashboard-real-time-create)
- Submit feature ideas: [Microsoft Fabric Ideas forum](https://ideas.fabric.microsoft.com/)
- Provide feedback using the feedback option in the Copilot pane


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/use-copilot-to-create-visuals-in-real-time-dashboards-preview/)

