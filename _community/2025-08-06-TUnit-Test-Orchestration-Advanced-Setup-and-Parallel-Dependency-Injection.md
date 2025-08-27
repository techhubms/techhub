---
layout: "post"
title: "TUnit Test Orchestration: Advanced Setup and Parallel Dependency Injection"
description: "This community post by thomhurst introduces a new TUnit feature for orchestrating complex test setups in .NET projects. It highlights nested property injection with data sources, parallel initialisation of Docker-based dependencies (like SQL Server, Redis, RabbitMQ), and intelligent object reuse, allowing for efficient and maintainable integration tests without third-party dependencies."
author: "thomhurst"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/csharp/comments/1mjgiuq/tunit_test_orchestration/"
viewing_mode: "external"
feed_name: "Reddit CSharp"
feed_url: "https://www.reddit.com/r/csharp/.rss"
date: 2025-08-06 20:59:12 +00:00
permalink: "/2025-08-06-TUnit-Test-Orchestration-Advanced-Setup-and-Parallel-Dependency-Injection.html"
categories: ["Coding"]
tags: [".NET", "C#", "ClassDataSource", "Coding", "Community", "Dependency Injection", "Docker", "Integration Testing", "Parallel Initialization", "PerTestSession", "RabbitMQ", "Redis", "ReSharper", "Rider", "SQL Server", "Test Orchestration", "TestContainers", "Testing Framework", "TUnit", "VS"]
tags_normalized: ["dotnet", "csharp", "classdatasource", "coding", "community", "dependency injection", "docker", "integration testing", "parallel initialization", "pertestsession", "rabbitmq", "redis", "resharper", "rider", "sql server", "test orchestration", "testcontainers", "testing framework", "tunit", "vs"]
---

thomhurst presents a major enhancement to the TUnit testing framework, enabling .NET developers to orchestrate complex integrations—like Docker-based SQL Server, Redis, and RabbitMQ—using powerful nested property injection and parallel initialisation.<!--excerpt_end-->

# TUnit Test Orchestration: Advanced Setup and Parallel Dependency Injection

**Contributor:** thomhurst

TUnit has released a new feature designed to simplify the orchestration of complex test environments. This update is particularly valuable for developers who need to spin up setups involving multiple components—such as WebApps, Docker networks, Redis, message buses, and SQL databases—without connecting to real third-party services.

## Key Features

- **Nested Property Injection**: Properties created via data source attributes can themselves have injected dependencies. This process happens recursively, allowing deeply nested and configurable test setups.
- **Object Reuse with ClassDataSource**: Using `ClassDataSource(Shared = PerTestSession)`, TUnit enables smart reuse of expensive resources within a test session, minimizing redundant initialisation.
- **Intelligent Orchestration**: TUnit determines the proper initialisation order for services, ensuring dependencies are ready when needed. Parallel initialisation occurs when properties are at the same level with no dependencies between them.
- **Less Boilerplate**: Developers can focus on testing rather than setup/teardown logic, keeping code modular and adhering to the Single Responsibility Principle.

## Example Scenario

Suppose your integration test needs:

- A WebApp connected to
  - Docker Network
  - Redis
  - Message Bus
  - SQL Database
- Extra Docker containers for UI inspection
- All running **in-memory**, such as with [TestContainers](https://testcontainers.com/)

TUnit's orchestration features allow you to define all these dependencies declaratively. The framework manages the lifecycle and dependencies, ensuring services like SQL Server, Redis, and RabbitMQ start in parallel (when possible) under one test session.

**Read the full example:** [Complex Test Infrastructure with TUnit](https://tunit.dev/docs/examples/complex-test-infrastructure)

## Community Discussion Highlights

- **Parallel Initialisation**: Dependencies at the same level (i.e., not dependent on each other) are started in parallel, improving efficiency.
- **Tooling Support**: TUnit tests work with Visual Studio Test Explorer and `dotnet test`. Detection in ReSharper may require checking settings for the "testing platform"; Rider supports it out of the box.
- **Feedback Welcome**: thomhurst encourages the community to share thoughts and suggestions on these new orchestration capabilities.

---

For more information or to contribute feedback, visit the [official documentation](https://tunit.dev/docs/examples/complex-test-infrastructure).

This post appeared first on "Reddit CSharp". [Read the entire article here](https://www.reddit.com/r/csharp/comments/1mjgiuq/tunit_test_orchestration/)
