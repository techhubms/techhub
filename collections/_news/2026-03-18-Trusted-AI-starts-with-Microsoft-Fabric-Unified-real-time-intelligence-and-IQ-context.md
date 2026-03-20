---
title: 'Trusted AI starts with Microsoft Fabric: Unified real-time intelligence and IQ context'
primary_section: ai
tags:
- 3D Visualization
- Activator
- AI
- Anomaly Detection
- Business Events
- Content Based Routing
- Digital Twin
- Event Driven Architecture
- Eventhouse
- Eventstream
- Fabric Graph
- Fabric IQ
- General Availability
- Geospatial Analytics
- GQL
- Maps
- Microsoft Fabric
- Microsoft IQ
- ML
- News
- NVIDIA Omniverse
- OneLake
- Ontology
- Physical AI
- Private Preview
- Real Time Dashboards
- Real Time Hub
- Real Time Intelligence
- SQL Operator
- Streaming Analytics
date: 2026-03-18 05:46:22 +00:00
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/trusted-ai-starts-with-microsoft-fabric-unified-real-time-intelligence-and-iq-context/
section_names:
- ai
- ml
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog outlines how Microsoft Fabric Real-Time Intelligence and Fabric IQ aim to provide a shared semantic/ontology layer for real-time operations, enabling AI agents to reason over consistent business context. It also announces a Microsoft–NVIDIA Omniverse integration (private preview in April) and highlights new/updated Fabric capabilities like Maps (GA), Business Events, Graph, ontology improvements, and SQL Operator in Eventstream.<!--excerpt_end-->

# Trusted AI starts with Microsoft Fabric: Unified real-time intelligence and IQ context

*Source: Microsoft Fabric Blog*

Modern operations increasingly need decisions in minutes or seconds. This post argues that **speed isn’t enough**: without a shared semantic model, teams and AI systems interpret the same signals differently, leading to fragmented insights and misaligned actions.

The announcement frames **Microsoft Fabric** as an “operational foundation” for real-time decision loops across digital systems and physical environments, built around:

- **OneLake** (unified data estate)
- **Real-Time Intelligence** (stream → analyze → model → visualize → act)
- **Fabric IQ** (shared business context/ontology)
- **AI agents** (reasoning and autonomous actions with guardrails)

## Microsoft and NVIDIA partnership: Fabric + Omniverse for Physical AI

Microsoft announces a strategic partnership with **NVIDIA** to integrate **Microsoft Fabric** with **NVIDIA Omniverse** libraries to support “Physical AI” scenarios (operations in airports, logistics hubs, factories).

Example customer mentioned:

- **Vanderlande** is using Fabric + Omniverse to build an **AI-powered 3D Airport Digital Twin** via **Vanderlande’s OpenAir Platform**, spanning airside, terminal, and baggage operations.

The solution is described as three layers:

- **Fabric Real-Time Intelligence**: real-time streaming and analytics
- **Fabric IQ**: shared business semantics/context
- **NVIDIA Omniverse Libraries**: 3D visualization and physical simulation

### Preview detail

The first step is planned for **private preview in April**, enabling teams to **embed Omniverse 3D scenes into Fabric Real-Time Dashboards**, with:

- **Bidirectional cross-highlighting and filtering** between dashboards and 3D scenes

![Animated GIF illustrating a strategic partnership between Microsoft and NVIDIA to deliver an integrated platform for Physical AI. It highlights Fabric Real-Time Intelligence, Fabric IQ, and NVIDIA Omniverse Libraries.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/animated-gif-illustrating-a-strategic-partnership.gif)

## Microsoft Fabric as an operational foundation

The post describes Fabric as a platform for a continuous operational loop (observe → analyze → decide → act), emphasizing that **shared context** is necessary for trust—especially when decisions affect real-world systems.

![Animated GIF showing the continuous Observe → Analyze → Decide → Act cycle for the real-time intelligence loop.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/animated-gif-showing-the-continuous-observe-greaterana.gif)

## OneLake: unified data foundation

**OneLake** is positioned as the core unified data lake, intended to remove silos via:

- Mirroring
- Cross-cloud shortcuts
- “Hundreds of connectors”

Goal: ensure people, applications, and AI agents operate from the same trusted data foundation.

![Animated GIF showing how OneLake unifies data in Microsoft Fabric.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/animated-gif-showing-how-onelake-unifies-data-in-m.gif)

## Real-Time Intelligence: time, space, and relationships

Real-Time Intelligence is described as a lifecycle:

- **Stream → Analyze → Model → Visualize → Act**

It emphasizes three dimensions to model operations:

- Time
- Space (including geospatial)
- Relationships

Key components listed:

- **Real-Time Hub**: discover/manage streaming data
- **Eventstream**: event ingestion via connectors
- **Eventhouse**: time-series storage/analysis at scale
- **Real-Time Dashboards**: live visualization with sub-second refresh
- **Activator**: detect patterns and trigger responses
- **Anomaly Detector**: machine-learning-based detection of unusual patterns

![Animated GIF illustrating components and benefits of Real-Time Intelligence in Microsoft Fabric.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/animated-gif-illustrating-components-and-benefits.gif)

## Fabric IQ: shared business context via ontology

**Fabric IQ** is presented as the semantic layer that defines business entities (examples given: customers, locations, assets, transactions, teams) and binds them to historical + real-time data.

This ontology is positioned as the basis for consistent interpretation by:

- People
- Applications
- AI agents

A customer quote highlights using Fabric IQ for a **data center ontology** in energy management, enabling “agentic AI” while keeping final decisions with operators.

![Animated GIF showing how Microsoft IQ unifies the act–decide–analyze–observe loop with a semantic foundation.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/animated-gif-showing-how-microsoft-iq-unifies-the.gif)

The post also frames Fabric IQ within a broader **Microsoft IQ** concept:

- **Work IQ**: communications/documents/flow of work
- **Fabric IQ**: live operational state and operational actions
- **Foundry IQ**: curated institutional knowledge

## AI agents in Fabric

Agents are described as operating on the same shared context as humans, with guardrails for safety/compliance.

Two types mentioned:

- **Data agents**: conversational Q&A grounded in ontology + real-time state
- **Operations agents**: continuous monitoring and autonomous actions within defined boundaries

## Announced updates

### Maps (Generally Available)

Maps are now **generally available**, adding native **geospatial intelligence** into Fabric’s real-time operating model.

- Blog: [Maps in Microsoft Fabric blog](https://aka.ms/maps-ga-blog)

![Animated GIF showing Maps in Microsoft Fabric.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/Animated_GIF_v02_medium.gif)

### Business Events

**Business Events** brings event-driven architecture to Fabric at the semantic level.

- Define and detect business-level events like “customer at risk,” “shipment delayed,” “equipment needs maintenance.”
- Trigger workflows/alerts/AI agents when conditions match.

(The post references a “Business Events blog” for details but does not provide a direct URL in the provided text.)

### Graph improvements

**Fabric Graph** updates mentioned:

- Support for “billions scale graphs”
- Deeper **Fabric Data Agent** integration
- Expanded **GQL** capabilities (including shortest path queries)
- UX improvements for modeling/querying/exploration

Graph is said to work directly on data in **OneLake**, avoiding copies to separate graph databases.

![Animated GIF showing a Fabric Graph visualization of entities and relationships.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/animated-gif-showing-a-graph-visualization-with-in.gif)

### Ontology improvements

Several improvements are mentioned around operating the shared model at scale and strengthening governance/security foundations.

- More info: [Fabric IQ Ontology blog](https://aka.ms/FabricIQOntologyblog)

### SQL Operator in Eventstream

**SQL Operator** brings SQL to real-time stream processing in **Eventstream**, including an evolution toward:

- Content-based routing
- Fan-out to destinations like **Eventhouse**, **Lakehouse**, **Activator**, or downstream streams
- Testing support and a single maintainable SQL surface

Timing called out:

- New capabilities available in **early April**

Details:

- [Fabric Eventstream SQL Operator blog](https://blog.fabric.microsoft.com/blog/fabric-eventstream-sql-operator-your-tool-kit-to-real-time-data-processing-in-fabric-real-time-intelligence)

### Additional roadmap items mentioned

- Multivariate anomaly detection
- Real-Time Dashboard performance/interactivity
- Expanded Eventstream connectors
- Operations agent enhancements
- Data agent + Graph query integration
- More geospatial analytics

## Getting started resources

- Tutorials: [Real-Time Intelligence](https://aka.ms/realtimetutorial) | [Ontology](https://aka.ms/ontology-tutorial)
- Forums: [Real-Time Intelligence](https://aka.ms/realtimeforum) | [IQ](https://aka.ms/fabric-iq-forum)
- Ideas: [Real-Time Intelligence](https://aka.ms/realtimeideas) | [IQ](https://aka.ms/fabric-iq-ideas)
- Docs: [Real-Time Intelligence](https://aka.ms/realtimedocs) | [IQ](https://aka.ms/fabric-iq-overview)
- Learning path: [Real-Time Intelligence](https://aka.ms/realtimelearningpath)
- Certification: [Real-Time Intelligence](https://aka.ms/realtimeskill)
- Blogs: [Real-Time Intelligence](https://aka.ms/realtimeblogs) | [IQ](https://aka.ms/fabric-iq-blogs)
- Release plans: [Real-Time Intelligence](https://aka.ms/realtimereleaseplan) | [IQ](https://aka.ms/fabric-iq-release)
- LinkedIn: [Follow on LinkedIn](https://aka.ms/realtimelinkedin)
- YouTube: [Watch on YouTube](https://aka.ms/fabric-youtube)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/trusted-ai-starts-with-microsoft-fabric-unified-real-time-intelligence-and-iq-context/)

