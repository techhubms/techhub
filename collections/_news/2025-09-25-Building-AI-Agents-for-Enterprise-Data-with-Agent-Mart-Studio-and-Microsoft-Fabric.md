---
layout: post
title: Building AI Agents for Enterprise Data with Agent Mart Studio and Microsoft Fabric
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/27082/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-09-25 11:00:00 +00:00
permalink: /ai/news/Building-AI-Agents-for-Enterprise-Data-with-Agent-Mart-Studio-and-Microsoft-Fabric
tags:
- Agent Mart Studio
- AI Agents
- Business Process Automation
- Data Automation
- Data Integration
- Enterprise Data
- Lucid Data Hub
- Microsoft Fabric
- No Code AI
- OneLake
- Retail Automation
- Workflow Automation
section_names:
- ai
- azure
---
Authored by the Microsoft Fabric Blog team, this guide explains how Venu Amancha and Roy Hasson demonstrate using Agent Mart Studio on Microsoft Fabric to enable business users to create and deploy AI agents directly on enterprise data stored in OneLake.<!--excerpt_end-->

# Building AI Agents for Enterprise Data with Agent Mart Studio and Microsoft Fabric

**Coauthored by: Venu Amancha (Lucid Data Hub) and Roy Hasson (Microsoft)**

Lucid Data Hub has announced the integration of Agent Mart Studio with [Microsoft Fabric](https://www.microsoft.com/microsoft-fabric), providing business users a no-code environment to build and deploy AI agents powered by their own enterprise data in [OneLake](https://learn.microsoft.com/fabric/onelake/onelake-overview). This enables organizations to automate complex business processes with adaptive, secure, and industry-tailored AI agents.

## Addressing Enterprise Data Challenges

- Traditional enterprise data workflows are bottlenecked by technical complexity and reliance on engineering or data science teams.
- Non-technical users are often unable to extract timely, actionable insights from their data.
- Lucid's integration leverages Microsoft Fabric and the open OneLake platform to democratize AI agent creation, making it accessible via Agent Mart Studio's drag-and-drop, no-code interface.

## Real-World Use Case: Retail Out-of-Stock Automation

**Scenario:**
A retail company faces challenges tracking and responding to out-of-stock items. Previously, manual tracking, cross-department coordination, and lag in reporting would delay response.

**Solution with Agent Mart Studio:**

- Business analysts can use Agent Mart Studio in Fabric to automate out-of-stock detection and replenishment reporting, acting directly on near-real-time data from OneLake.
- Agents identify at-risk products, generate recommendations, and notify teams or trigger ordering actions—all without coding.
- Agents are customizable to match inventory policies, alert thresholds, and business rules per organization.

## Step-by-Step Guide: Building an Out-of-Stock AI Agent

### 1. Add the Agent Mart Studio Workload

- From Microsoft Fabric, add Agent Mart Studio to your Workspace(s). This unlocks the AI Agent Builder app.

### 2. Design Your Agent

- Launch Agent Mart Studio and use the visual interface to define business logic (e.g., notifying teams, triggering replenishments, or adjusting promotions).
- Add a new 'AI Agent' item and define its purpose, such as monitoring availability and triggering supplier recommendations.
- Select data tools: Agent Mart scans OneLake data and offers query blocks (e.g., inventory summary, sales, thresholds).
- Choose and customize business logic flows via provided templates. Extend with SQL or Python notebooks for advanced analysis or use LLMs for narrative reporting.
- Select actions: Send alerts (email, Teams), update Power BI dashboards, or invoke external services. Multiple parallel actions are supported.

### *Optional*: Prepare Contextual Data

- With Lucid Context Hub, users can model and annotate datasets in a Lakehouse, enabling quicker agent setup for large or complex enterprises.

### 3. Deploy and Test the Agent

- Activate the agent and choose built-in actions (e.g., updating dashboards, sending supplier reports, triggering restocks).
- Test using sample data and monitor results through Agent Mart Studio’s dashboards.
- Iterate quickly—refine replenishment rules and workflows based on real-time results.

### 4. Publish and Operate

- Publish your agent to make it live.
- Schedule or run agents on-demand, monitor execution logs, and deliver insights directly to business teams.

## Key Benefits

- **Empower Business Users:** Lowers technical barriers so business analysts can build and use sophisticated AI agents.
- **Automation and Adaptability:** Supports workflow automation across industries with customizable logic and secure, contextual guardrails.
- **Responsiveness:** Enables organizations to react faster using real-time data and automated actions, without lengthy engineering cycles.
- **Integration:** Works natively with Microsoft Fabric, OneLake, and can trigger actions in other Microsoft tools such as Power BI and Teams.

## Getting Started

Agent Mart Studio for Microsoft Fabric is available in preview. For more information, demo requests, and webinars, visit [Agent Mart Studio](https://bit.ly/agentmart-ai) or [Lucid Data Hub](https://bit.ly/luciddatahub).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/27082/)
