We are thrilled to announce the release of [AutoGen v0.4](https://microsoft.github.io/autogen/stable/)! This new version represents a complete redesign of the AutoGen library, focusing on improved code quality, robustness, usability, and scalability of agentic workflows. This release sets the stage for a robust ecosystem to drive advances in agentic AI.

**Key Features:**

- **Asynchronous Messaging:** Agents now communicate through asynchronous messages, supporting both event-driven and request/response interaction patterns. 

- **Modular and Extensible:** Create easily customize systems with pluggable components, including custom agents, tools, memory, and models. Build proactive and long-running agents. 

- **Full type support:** Interfaces and extensive typing are enforced in the library, ensuring consistent and high-quality code and dependable APIs. 

- **Layered architecture:** With a new layered and modular approach users can target the level of abstraction that their scenario requires. 

- **Observability and Debugging:** Built-in tools for tracking, tracing, and debugging agent interactions and workflows, with support for OpenTelemetry. 

- **Scalable and Distributed:** Design complex, distributed agent networks that operate seamlessly across organizational boundaries. 

- **Cross-Language Support:** Interoperability between agents built in different programming languages, currently supporting Python and .NET. 

- **Built-in and community extensions**: Support for community extensions allows open-source developers to manage their own extensions to enhance the framework’s functionality. 

**Layered Architecture**

[![A screenshot of a computer AI-generated content may be incorrect.](https://devblogs.microsoft.com/autogen/wp-content/uploads/sites/86/2025/01/a-screenshot-of-a-computer-ai-generated-content-m-4-300x184.png)](https://devblogs.microsoft.com/autogen/wp-content/uploads/sites/86/2025/01/a-screenshot-of-a-computer-ai-generated-content-m-4.png)

Figure 1. The v0.4 update introduces a cohesive AutoGen ecosystem that includes the framework, developer tools, and applications. The framework’s layered architecture clearly defines each layer’s functionality. It supports both first-party and third-party applications and extensions.

**Framework:**

- **Core API:** The foundation layer offering a scalable, event-driven actor framework for creating agentic workflows 

- **AgentChat API:** Built on Core, offering a task-driven, high-level framework for building interactive agentic applications. It is a replacement for AutoGen v0.2. 

**Developer Tools:**

- **AutoGen Bench:** Benchmark agents by measuring and comparing performance across tasks and environments. 

- **AutoGen Studio:** Rebuilt on the v0.4 AgentChat API, this enhanced low-code interface enables rapid prototyping of AI agents. The tool features real-time agent updates, mid-execution control, message flow visualization, drag-and-drop builder and more. 

**Apps:**

- **Magentic-One:** A new generalist multi-agent application to solve open-ended web and file-based tasks across various domains. 

**Migrating to AutoGen v0.4**

Despite the new architecture of v0.4, the library offers a path to facilitate a smooth upgrade of applications built on v0.2. The AgentChat API maintains the same level of abstraction as v0.2, making it easy to migrate existing code to v0.4. For example, AgentChat offers an AssistantAgent and UserProxy agent with similar behaviors to those in v0.2. It also provides a team interface with implementations like RoundRobinGroupChat and SelectorGroupChat, which cover all the capabilities of the GroupChat class in v0.2.

For detailed guidance on how to migrate se the [migration guide](https://aka.ms/autogen-migrate).

**What’s Next?**

We will soon release a .NET version of v0.4!

Our roadmap includes built-in extensions and applications as well as fostering a community-driven ecosystem of extensions and applications—some of which are already being developed by users.

We’re certainly not stopping here! There’s plenty of exciting new features on the roadmap and we’re going to be releasing more frequently going forward.

We encourage you to engage with us on [AutoGen’s discord](https://discord.gg/HeEdc5nu) channel and share feedback on the official AutoGen [repository](https://github.com/microsoft/autogen) via GitHub Issues.

 
 

Category

## Author

![Friederike Niedtner](https://devblogs.microsoft.com/autogen/wp-content/uploads/sites/86/2024/11/selfie_square-96x96.jpg)

Principal TPM

I work with researchers and engineers to advance AI technology. Together we transform cutting-edge research into open-source software, empowering developers to create innovative AI applications. Through these efforts, we aim to redefine the possibilities of AI in the development community.