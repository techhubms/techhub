---
layout: "post"
title: "How We Use AI Agents for COBOL Migration and Mainframe Modernization"
description: "This detailed post by jkordick and gkaleta explores using Microsoft AI agent technologies, including Semantic Kernel, to modernize COBOL and mainframe applications. The authors describe a structured migration approach, leveraging AI-powered modular agents to analyze, transform, and convert legacy COBOL systems to modern Java or .NET platforms, with bank industry case studies."
author: "jkordick, gkaleta"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/all-things-azure/how-we-use-ai-agents-for-cobol-migration-and-mainframe-modernization/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/all-things-azure/feed/"
date: 2025-07-09 17:10:30 +00:00
permalink: "/news/2025-07-09-How-We-Use-AI-Agents-for-COBOL-Migration-and-Mainframe-Modernization.html"
categories: ["AI", "Azure"]
tags: ["Agent Orchestration", "Agents", "AI", "AI Agents", "All Things Azure", "App Development", "AutoGen", "Azure", "Bankdata", "Cloud Native", "COBOL Modernization", "Code Analysis", "Code Transformation", "Dependency Mapping", "Developer Productivity", "Java Quarkus", "Legacy Systems", "Mainframe Migration", "Microsoft Azure", "News", "OpenAI", "Semantic Kernel"]
tags_normalized: ["agent orchestration", "agents", "ai", "ai agents", "all things azure", "app development", "autogen", "azure", "bankdata", "cloud native", "cobol modernization", "code analysis", "code transformation", "dependency mapping", "developer productivity", "java quarkus", "legacy systems", "mainframe migration", "microsoft azure", "news", "openai", "semantic kernel"]
---

In this comprehensive post, authors jkordick and gkaleta delve into how Microsoft’s AI agentic technologies, notably Semantic Kernel, support migrating legacy COBOL mainframe applications to modern cloud architectures. The article features technical deep dives and insights from real-world modernization projects.<!--excerpt_end-->

# How We Use AI Agents for COBOL Migration and Mainframe Modernization

## Introduction

Legacy modernization is a persistent challenge for organizations looking to reduce technical debt and adopt cloud-native solutions. Among the most complex modernization scenarios are mainframe systems, particularly those written in COBOL and PL/1. These continue to power core banking, insurance, and government services, yet face hurdles such as declining expertise, ballooning maintenance costs, and massive codebases.

Organizations increasingly seek autonomy over these migrations, favoring approaches that let them retain control over intellectual property and project direction—moving away from dependence on specialized third parties or global system integrators (GSIs).

### Case Study: Bankdata

[Bankdata](https://www.bankdata.dk/) is a collective of Danish banks providing IT solutions to its members, making up over 30% of Denmark's banking market. Their software stack spans from 1960s-era mainframes to modern cloud platforms, but over 70 million lines of COBOL remain on their mainframes. While much of this works well where it is, components that require re-platforming have proved costly and difficult to move.

## Understanding COBOL Modernizability

Not all COBOL modules are equal—many are tightly coupled to mainframe-specific properties like batch performance, I/O, JCL orchestration, or strict SLAs. Such non-functional dependencies complicate straightforward migration. Thus, part of the modernization challenge is not just code transformation, but thoughtful analysis of what *can* and *should* be modernized and how.

## Rethinking COBOL Modernization with Agentic AI

The authors leveraged new AI advancements—large and small language models, and the rise of AI agents—to approach COBOL migration. Early experiments with GPT-4 and GitHub Copilot highlighted substantial challenges, such as token window limitations and poor COBOL comprehension, resulting in both insightful ideas and hallucinated output.

Reflecting on these difficulties shaped a structured orchestration process dividing migration into clear steps:

### 1. Preparation

- **Reverse engineering**: Extract business logic from code, documentation, and SMEs.
- **Preparing code for AI**: Stripping superfluous comments/information.
- **Translation**: Adapting code and comments to the language understood by AI (COBOL in Danish, in Bankdata's case).

### 2. Enrichment

- **Adding meaningful comments**: To bolster AI comprehension.
- **Identifying recurring/deterministic structures**: Better agent orchestration and chunking.
- **Documenting temporary results**: Structured markdown aids continuity for both agents and humans.

### 3. Automation Aids

- **Flow analysis/visualization**: Using tools like Mermaid for visualization.
- **Test generation**: Leveraging existing or generating new tests.
- **Utility function isolation**: Early removal of logic better handled as libraries.

## The COBOL Agentic Migration Factory (CAMF)

Building on these learnings, the team developed the COBOL Agentic Migration Factory (CAMF) using Microsoft’s [AutoGen](https://github.com/microsoft/autogen) framework. The first iteration comprised three worker agents:

- **COBOL Expert**: Analyzes COBOL structure and logic.
- **Java Expert**: Converts to Java Quarkus code.
- **Test Expert**: Builds test suites for generated code.

These agents collaborate in chat-based orchestration, controlled via logs and interfaces for performance evaluation.

### Learnings from Real-World Data

- *Context windows*: Too much context caused loss of coherence; concise, targeted input produced higher-quality results.
- *Call-chain complexity*: Accurately preserving call-chain dependencies was non-trivial even at moderate depths.
- *Test structures*: Deterministic tests proved vital for agent output validation.

To further refine output, strategies included pre-processing with Graph RAG, improved chunking, enhanced planners, and combining these tactics.

## Introducing Semantic Kernel for Orchestration

Moving beyond initial experiments, the team adopted Microsoft’s Semantic Kernel for more robust agent orchestration. This enabled multi-agent coordination to migrate COBOL code to Java or .NET, facilitating analysis, transformation, dependency mapping, and reporting across specialized agents.

### CAMF Architecture and Workflow

1. **COBOLAnalyzerAgent**: Scans for structure, variables, logic, embedded SQL, and dependencies.
2. **JavaConverterAgent**: Generates modern Java (Quarkus) code from structured analysis data, focusing on idiomatic design and best practices.
3. **DependencyMapperAgent**: Identifies relationships between programs and copybooks, generates Mermaid diagrams, calculates module-level complexity scores, and recommends microservice boundaries.

Sample system prompt guidelines are provided for consistent conversions, emphasizing modern Java constructs (classes, methods, proper types, idiomatic flow, comprehensive comments, and Quarkus compatibility).

#### Visualization & Analysis

The DependencyMapperAgent not only visualizes dependencies, but also analyzes data flow, circular dependencies, legacy patterns, and modularity improvements, feeding these insights to both COBOL and Java agents for more informed transformations.

#### Handling Large Legacy Bases

The system supports batch analysis, high token limits, and retry logic, making enterprise-scale modernization feasible and trackable. It also produces chat logs, metrics, code, and diagrams.

## End-to-End Workflow Example

- COBOL files and copybooks are discovered.
- COBOLAnalyzerAgent extracts structures and logic.
- DependencyMapperAgent maps all relationships and metrics.
- JavaConverterAgent transforms programs into Java, considering dependencies and architectural insights.
- Reports and visualizations are generated.

## Open Source and Customization

Due to Bankdata's code sensitivity, detailed reports were not published, but the open-source framework is available at [aka.ms/cobol](http://aka.ms/cobol). Each agent persona can be tailored to specific modernization requirements for other legacy environments.

## Conclusion

AI-powered agent orchestration, anchored in Microsoft technologies such as Semantic Kernel and AutoGen, presents a new approach to large-scale mainframe modernization. The methodology prioritizes both code and architecture transformation, providing transparency, customization, and extensibility for organizations modernizing legacy systems.

---

- **Project Team:** Julia Kordick (MSFT), Gustav Kaleta (MSFT), Omar Alhajj (Bankdata), Michael Munch (Bankdata), Morten Lilbæk Pedersen (Bankdata), Michael Lind Mortensen (Bankdata)

Read more, contribute, or experiment with the framework at [aka.ms/cobol](http://aka.ms/cobol).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/how-we-use-ai-agents-for-cobol-migration-and-mainframe-modernization/)
