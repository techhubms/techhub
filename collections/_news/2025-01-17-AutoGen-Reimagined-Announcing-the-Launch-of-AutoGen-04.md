---
external_url: https://devblogs.microsoft.com/autogen/autogen-reimagined-launching-autogen-0-4/
title: 'AutoGen Reimagined: Announcing the Launch of AutoGen 0.4'
author: Friederike Niedtner
feed_name: Microsoft DevBlog
date: 2025-01-17 23:39:51 +00:00
tags:
- .NET
- Agentic Workflows
- Asynchronous Messaging
- Autogen
- AutoGen Studio
- Cross Language Support
- Developer Tools
- Layered Architecture
- Magentic One
- Modular AI
- New Release
- OpenTelemetry
- Python
section_names:
- ai
---
Friederike Niedtner presents the release of AutoGen v0.4, a major update focusing on robust, scalable agentic AI workflows with improved architecture, developer tools, and cross-language support. Learn about its new features, migration guidance, and roadmap.<!--excerpt_end-->

## AutoGen Reimagined: Launching AutoGen 0.4

**Author:** Friederike Niedtner

We are thrilled to announce the release of [AutoGen v0.4](https://microsoft.github.io/autogen/stable/)! This release represents a comprehensive redesign of the AutoGen library, placing emphasis on code quality, robustness, usability, and the scalability of agentic AI workflows. Version 0.4 paves the way for a vibrant ecosystem to accelerate innovation in agentic AI systems.

### Key Features

- **Asynchronous Messaging:** Agents now use asynchronous messages, supporting both event-driven and request/response communication patterns.
- **Modular and Extensible:** The framework is structured for customization. Developers can add pluggable components such as custom agents, tools, memory structures, or models, enabling the creation of proactive or long-running agents.
- **Full Type Support:** The enforcement of interfaces and extensive typing guarantees consistent, dependable APIs and high code quality.
- **Layered Architecture:** AutoGen v0.4 introduces a modular, layered structure, letting users select the abstraction level best suited to their scenarios.
- **Observability and Debugging:** Integrated tools for tracking, tracing, and debugging agent workflows, including support for OpenTelemetry.
- **Scalable and Distributed:** Enables the design of complex, distributed agent networks that function seamlessly across organizational boundaries.
- **Cross-Language Support:** Agents built in Python and .NET can now interoperate, with support for additional languages expected in the future.
- **Built-in and Community Extensions:** Framework supports both native and community-driven extensions to further expand its capabilities.

### Layered Architecture Overview

![AutoGen Layered Architecture Screenshot](https://devblogs.microsoft.com/autogen/wp-content/uploads/sites/86/2025/01/a-screenshot-of-a-computer-ai-generated-content-m-4.png)

**Figure 1:** The v0.4 update introduces a cohesive AutoGen ecosystem including the framework, developer tools, and apps. The newly defined layers clarify modular boundaries and enable support for both first- and third-party applications and extensions.

#### Framework Components

- **Core API:** The foundation, featuring a scalable, event-driven actor framework for orchestrating agentic workflows.
- **AgentChat API:** Built on the Core, this high-level API streamlines the creation of interactive, task-focused agent applications, replacing the prior AutoGen v0.2 agent communication model.

#### Developer Tools

- **AutoGen Bench:** A benchmarking tool for evaluating agent performance across tasks and environments.
- **AutoGen Studio:** An enhanced, low-code interface rebuilt using the v0.4 AgentChat API. Features include real-time agent updates, mid-execution control, a drag-and-drop builder, and visualization of message flows to boost rapid agent prototyping.

#### Apps

- **Magentic-One:** An open-ended, generalist multi-agent app that can tackle broad web and file-based tasks across different domains.

### Migrating to AutoGen v0.4

Transitioning from v0.2 to v0.4 is streamlined. The AgentChat API maintains the abstraction level found in v0.2, allowing existing applications to be upgraded with minimal friction. For instance, v0.4 provides AssistantAgent and UserProxy agents, mirroring v0.2 behaviors, and group interaction implementations like RoundRobinGroupChat and SelectorGroupChat, covering all functionality of the previous GroupChat class.

For comprehensive migration guidance, see the official [migration guide](https://aka.ms/autogen-migrate).

### What’s Next?

- A dedicated .NET version of v0.4 is on the horizon.
- The roadmap includes more built-in extensions and applications, alongside fostering a collaborative, community-driven extension ecosystem.
- Users are encouraged to contribute, share feedback via [AutoGen’s Discord](https://discord.gg/HeEdc5nu) and file issues or suggestions on the [official GitHub repository](https://github.com/microsoft/autogen).

The team looks forward to a stream of frequent updates and invites the community to help shape the future of agentic AI with AutoGen.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/autogen/autogen-reimagined-launching-autogen-0-4/)
