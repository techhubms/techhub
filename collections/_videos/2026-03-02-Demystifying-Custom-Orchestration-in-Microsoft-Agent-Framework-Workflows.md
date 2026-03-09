---
external_url: https://www.youtube.com/watch?v=2BB9-kWb1Tc
title: Demystifying Custom Orchestration in Microsoft Agent Framework Workflows
author: dotnet
primary_section: dotnet
feed_name: DotNet YouTube
date: 2026-03-02 21:58:03 +00:00
tags:
- .NET
- Automation Patterns
- Concurrent Workflows
- Custom Orchestration
- Demo
- Developer
- Developer Community
- Developer Session
- Domain Specific Logic
- Dotnet10
- Dotnetdeveloper
- Edges
- Executors
- Graph Based Design
- Microsoft
- Microsoft Agent Framework
- Microsoftagentframework
- Sequential Workflows
- Software Developer
- Software Development
- Videos
- Workflow Orchestration
section_names:
- dotnet
---
Presented by dotnet, this session explains how software developers can use Microsoft Agent Framework's low-level primitives and composable workflow patterns for greater flexibility and control in system orchestration.<!--excerpt_end-->

{% youtube 2BB9-kWb1Tc %}

# Demystifying Workflows with Microsoft Agent Framework

*Presented by: dotnet*

## Overview

This session focuses on reclaiming control over how workflows are orchestrated within the Microsoft Agent Framework. Rather than relying solely on abstract, high-level shortcuts, developers are encouraged to utilize low-level primitives such as *executors* and *edges*.

## Key Concepts

- **Executors and Edges:**
  - Executors are type-safe processing nodes.
  - Edges connect these nodes in a directed graph, explicitly managing data and decision routes.
- **Designing Custom Orchestration:**
  - Compose workflows using graphs instead of settling for fixed flow templates.
  - Build hybrids that combine sequential, concurrent, group chat, and handoff behaviors.

## Workflow Patterns

- **High-Level Shortcuts:**
  - Patterns like sequential, concurrent, group chat, handoff, or magnetic orchestration are provided for rapid workflow design.
  - These patterns are composable, making it quick to get started while maintaining flexibility.
- **Customizing for Control:**
  - When specialized routing or domain-specific logic is needed, developers can interact directly with underlying executors and edges.
  - Facilitates the implementation of hybrid or domain-optimized processes that exceed the limits of standard patterns.

## Takeaways

- You are not forced to choose between opaque high-level orchestration (“magic”) and fragile, handcrafted logic.
- Use high-level orchestration for speed and convenience, then selectively drop to low-level primitives for custom needs.
- Enables the mixing of various orchestration behaviors in a single, coherent workflow system.

## Use Cases

- Scenarios requiring custom routing decisions based on real-time data.
- Building modular workflow components to support evolving business logic.
- Integrating multiple communication modalities in a workflow, such as group chat handoffs.

## Conclusion

This approach empowers developers to make orchestration both flexible and robust, ensuring workflows built on Microsoft Agent Framework can evolve and scale as needed.
