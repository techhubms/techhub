---
section_names:
- ai
- ml
- security
external_url: https://blog.fabric.microsoft.com/en-US/blog/whats-next-for-fabric-iq-ontology-the-operational-context-that-powers-your-ai-agents-preview/
title: 'What’s next for Fabric IQ Ontology: The operational context that powers your AI agents (Preview)'
date: 2026-03-24 11:30:00 +00:00
author: Microsoft Fabric Blog
tags:
- Access Control
- AI
- AI Agents
- Automation
- Azure Private Link
- Data Agents
- Fabric Activator
- Fabric IQ
- Governance
- MCP
- MCP Endpoints
- Microsoft Fabric
- ML
- Network Isolation
- News
- OneLake
- Ontology
- Operational Context
- Permissions Management
- Preview Features
- Rules Engine
- Security
- Semantic Layer
- Workspaces
feed_name: Microsoft Fabric Blog
primary_section: ai
---

Microsoft Fabric Blog outlines upcoming preview capabilities for Fabric IQ Ontology, focusing on using ontologies as business context for AI agents, adding rules/actions for automation, and strengthening governance with permissions and Azure Private Link.<!--excerpt_end-->

# What’s next for Fabric IQ Ontology: The operational context that powers your AI agents (Preview)

*If you haven’t already, check out Arun Ulag’s hero blog “[FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news)” for a complete look at all of our FabCon and SQLCon announcements across both Fabric and our database offerings.*

Since introducing Fabric IQ, there’s been growing momentum around how organizations are using **ontologies** to establish a shared semantic foundation for **analytics and AI**. By modeling business concepts and relationships in the ontology, teams can move beyond fragmented data views and enable AI experiences that reason, automate, and act with confidence.

At **FabCon Atlanta**, Microsoft shared a preview of what’s coming next: new Fabric IQ capabilities that aim to make ontologies **more expressive, more connected, and more reliable** as the foundation for AI-powered experiences across **Microsoft Fabric**.

## Why Fabric IQ and Ontology matter

AI is only as good as its understanding of your business. While **Microsoft Fabric** and **OneLake** unify where data lives, the *meaning* of that data can remain scattered across systems, tools, and teams—leading to inconsistent definitions, brittle integrations, and AI outputs that are hard to trust.

Fabric IQ’s core concept is the **Ontology item**, which connects **data, processes, rules, and actions** into a unified semantic layer. By binding real-world data to business entities and relationships, ontologies can turn raw tables and events into business-ready concepts that both people and AI can interpret consistently.

## What’s coming next

## Highlights for FabCon Atlanta – Embedding rules and actions into the Ontology

**Rules** are intended to transform live business context in the ontology into operationalized outcomes. The idea is that the ontology can automatically initiate business processes through alerts and automated actions, within a context-aware environment.

This is positioned as enabling automation **without switching tools or writing custom code**, helping streamline workflows.

- Documentation: [Rules in ontology (Preview) (with Fabric Activator)](https://aka.ms/ontology-rules)

![Screenshot showing a Fabric IQ rule creation UI for inventory threshold alerts](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/Screenshot-2026-03-24-174159-1.png)

*Figure: Create a rule to get automatically notified when inventory drops below your threshold.*

## Enterprise-grade security and governance

As ontologies become mission-critical, the post highlights enterprise controls for security and governance.

### Enhanced access control and permissions management

Fabric IQ supports **sharing and permissions management** for Ontology items. Using Fabric’s permissions experience, you can grant granular permissions such as:

- Read
- Edit
- Reshare

You can also view who has access to Ontology items within a workspace, to support secure collaboration and governance.

### Private network access with Azure Private Link

Fabric IQ supports **Azure Private Link** integration to enable **network isolation** at:

- Tenant level
- Workspace level

The stated goal is for connectivity to ontology items to flow through secured endpoints so traffic doesn’t traverse the public internet.

The post also mentions **cross-workspace validation** to allow isolated workspace artifacts to safely invoke artifacts from non-isolated workspaces.

## Powering AI Agents with business context

At Ignite, Microsoft introduced **data agents** that leverage Ontology for consistent business definitions and relationships. The post says this approach is being expanded so more agents can use Ontology as business context for scenarios beyond Q&A.

- Tutorial: [Ontology (preview) tutorial part 4: Create data agent](https://aka.ms/ontology-data-agent)

## Ontology integration with Operations Agent (Coming Soon)

The post says an **Operations Agent** will be able to use Ontology as a knowledge source to inform playbooks. By using entity types, relationships, rules, and actions defined in the ontology, agents can evaluate conditions and take actions aligned with business logic.

- Reference link provided in the post: “What is ontology (preview)?” (SharePoint draft link)

### Ontology MCP endpoints (Coming Soon)

Fabric IQ Ontology is planned to be exposed through public **Model Context Protocol (MCP)** endpoints. The stated intent is to make the ontology accessible to a broader agent ecosystem so agents can ground reasoning/actions in a unified semantic layer.

- Reference link provided in the post: “What is ontology (preview)?” (SharePoint draft link)

## Learn more, engage, and stay updated

- Submit ideas and vote: [Fabric Ideas Portal](https://aka.ms/fabric-ideas)
- Ask questions: [Community Forum (IQ forums)](https://community.fabric.microsoft.com/t5/IQ-forums/ct-p/fabriciq)
- Explore: [Fabric Blog](https://blog.fabric.microsoft.com/) and [Fabric IQ documentation](https://learn.microsoft.com/fabric/iq/)
- Watch demos: [Fabric YouTube channel](https://youtube.com/@MicrosoftFabric)
- Follow updates: [Fabric release plan](https://roadmap.fabric.microsoft.com/?product=iq)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/whats-next-for-fabric-iq-ontology-the-operational-context-that-powers-your-ai-agents-preview/)

