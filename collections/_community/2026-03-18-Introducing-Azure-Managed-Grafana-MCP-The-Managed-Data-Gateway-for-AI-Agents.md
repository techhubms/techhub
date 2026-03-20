---
tags:
- Access Controls
- AI
- AI Agents
- Application Insights
- Attack Surface Reduction
- Autonomous Remediation
- Azure
- Azure Managed Grafana
- Azure Monitor
- Azure RBAC
- Community
- Kusto
- Managed Identities
- MCP
- MCP Server
- Microsoft Foundry
- Observability
- Operational Data
- Production Diagnostics
- Remote MCP Endpoint
- Root Cause Analysis
- Security
- Telemetry
external_url: https://techcommunity.microsoft.com/t5/azure-observability-blog/introducing-azure-managed-grafana-mcp-the-managed-data-gateway/ba-p/4503619
primary_section: ai
section_names:
- ai
- azure
- security
title: 'Introducing Azure Managed Grafana MCP: The Managed Data Gateway for AI Agents'
feed_name: Microsoft Tech Community
author: aayodeji
date: 2026-03-18 23:30:40 +00:00
---

In this community post, aayodeji introduces Azure Managed Grafana MCP, a built-in managed MCP endpoint that lets AI agents query production telemetry (Azure Monitor, Application Insights, Kusto) using existing Azure RBAC and managed identities—without running a self-hosted MCP server.<!--excerpt_end-->

## Overview

Azure Managed Grafana MCP is a **fully managed, remote Model Context Protocol (MCP) server** built into every **Azure Managed Grafana** instance.

The goal is to let **AI agents** securely query real **production telemetry** that teams already send to Azure Managed Grafana—without requiring teams to deploy and operate a separate, self-hosted MCP server.

## The problem this is trying to solve

Teams experimenting with agents (including Foundry and coding agents) commonly hit a practical blocker: **agents are only as useful as the data they can reason over**, and production telemetry is often locked behind extra infrastructure and security work.

Today, enabling an agent to query systems like:

- Azure Monitor metrics and logs
- Application Insights
- Kusto

…often means:

- Deploying and operating a **self-hosted MCP server**
- Wiring identity and networking
- Maintaining additional runtime infrastructure

This adds friction and expands the security surface area.

## What Azure Managed Grafana MCP is

Azure Managed Grafana MCP is a **built-in, managed MCP endpoint** that allows agents to query enterprise telemetry and operational data **through Azure Managed Grafana**.

With this release, every Azure Managed Grafana instance includes a **fully managed, remote MCP server** that is *ready by default*.

## How it works (high level)

Instead of deploying your own MCP server, customers can:

1. Point their agent to the **Azure Managed Grafana MCP endpoint**
2. Grant the agent a **managed identity**
3. Start querying production data

Key point: **No containers. No extra infrastructure. No duplicated auth systems.**

Because many customers already connect data sources (Azure Monitor, Kusto, Application Insights) to Azure Managed Grafana, the MCP server can expose that telemetry to agents using the same controls teams already rely on.

## Core value propositions

### Zero infrastructure overhead

Azure Managed Grafana MCP is:

- Fully managed
- Enabled by default

It removes the need for:

- Self-hosted MCP servers
- Additional networking configuration

### Secure by design (inherits existing controls)

The post emphasizes that security is inherited from existing Azure practices:

- Uses **Azure RBAC**
- Supports **managed identities**
- Respects current **Azure Managed Grafana access controls**

This avoids duplicating authentication/authorization logic and aims to keep the security posture consistent with existing observability access patterns.

## Example enterprise scenarios enabled

By exposing production telemetry through MCP, the post calls out agent workflows such as:

- Root cause analysis using Application Insights
- Automated operational summaries
- Real-time diagnostics
- Cross-resource telemetry correlation
- Structured data access via Kusto

## Closing the loop: from telemetry to remediation

A key scenario described is “closing the loop” between observability and action, when agents have both:

- **Code context**
- **Live telemetry**

Example flow:

- Query Application Insights for production errors
- Identify recurring exception patterns
- Locate the source code emitting those errors
- Generate a fix and submit a pull request

## Designing for agents (not just dashboards)

The post argues that agents consume data differently than humans:

- Humans: sequential dashboard navigation, limited cognitive bandwidth, manual correlation
- Agents: parallel processing, iterative drill-downs, faster statistical pattern detection

Azure Managed Grafana MCP is positioned as enabling **agent-optimized tools**, such as aggregated failure views across many Application Insights instances.

It is also described as available as a **native tool within Microsoft Foundry**, so it can be connected to Foundry Agents.

## Looking ahead

Azure Managed Grafana MCP is positioned as a foundation for:

- Observability-driven autonomous agents
- Secure enterprise telemetry reasoning
- AI systems that detect, diagnose, and act

The post frames this as more than a visualization feature: “an infrastructure shift.”

## Reference documentation

- Configure an Azure Managed Grafana remote MCP server | Microsoft Learn: https://learn.microsoft.com/en-us/azure/managed-grafana/grafana-mcp-server


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-observability-blog/introducing-azure-managed-grafana-mcp-the-managed-data-gateway/ba-p/4503619)

