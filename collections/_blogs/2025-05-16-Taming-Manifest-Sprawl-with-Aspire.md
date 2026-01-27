---
external_url: https://medium.com/@davidfowl/taming-manifest-sprawl-with-aspire-1ad938379433?source=rss-8163234c98f0------2
title: Taming Manifest Sprawl with Aspire
author: David Fowler
feed_name: David Fowler's Blog
date: 2025-05-16 15:15:55 +00:00
tags:
- .NET
- Aspire
- C#
- CI/CD
- Cloud Native
- Configuration Management
- Developer Experience
- Docker
- Docker Compose
- Kubernetes
- OpenTelemetry
- Service Modeling
- Software Development
section_names:
- coding
- devops
primary_section: coding
---
David Fowler explores how Aspire addresses 'manifest sprawl' by unifying application configuration and deployment manifests into a single, C#-coded model, streamlining workflows for developers.<!--excerpt_end-->

# Taming Manifest Sprawl with Aspire

*Written by David Fowler*

Manifest sprawl is a challenge familiar to many modern developers: a new service means juggling multiple configuration manifests—`docker-compose.yml` for local runs, Kubernetes YAML for production, and a `.env` file for secrets. A single missed line can cause frustrating errors that take hours or days to diagnose. And that's just the simple case, without factoring in multiple environments, separate secret stores, or CI/CD pipelines. This duplication complicates every deployment, code review, and onboarding session.

## Manifest Sprawl Hurts

- **Drift**: Local and production configs diverge easily, such as when someone forgets to update a port or environment variable.
- **Onboarding toil**: New developers often spend their first day getting containers wired up rather than writing code.
- **Review fatigue**: Pull requests become bogged down by boilerplate YAML that few read thoroughly.
- **Hidden coupling**: Important runtime dependencies are tracked externally—in wikis, or simply in the minds of senior engineers—instead of being version-controlled.
- **Under-modeled relationships**: When relationships aren’t represented in a unified way, human knowledge becomes the integration glue, resulting in late-breakage issues during delivery cycles.

## Aspire in a Nutshell

Aspire addresses these challenges by letting developers model the entire stack in a single **C# graph**. Every service, container, secret, and dependency is named explicitly:

```csharp
var db = builder.AddPostgres("db");
var api = builder.AddProject<Api>("api").WithReference(db);
```

> *Note*: The workloads can be any containerized application; C# is just employed as the modeling language for clarity and strongly-typed relationships.

## Dev Loop: One Command to Run

```bash
aspire run
```

- Boots up the full application stack on your local machine.
- Automatically manages ports, secrets, and connection strings.
- Launches a dashboard showing the live dependency graph, logs, and traces via OpenTelemetry built-in.

## Shipping: One Command to Deploy

```bash
aspire publish
```

- Outputs standard artifacts—plain Kubernetes YAML, Docker Compose files, or custom adapters that you’ve integrated.
- Artifact generation is deterministic, making it ideal for CI/CD environments.
- Wiring and observability remain consistent with the local run configuration.

## Why Aspire Sticks

- **No more drift**: Change a port once in your C# code, and both development and production environments update together.
- **Zero-setup onboarding**: Just clone the repo and run `aspire run` to start coding.
- **Observability built-in**: Logging and tracing (OpenTelemetry) are automatic, requiring no extra tooling or sidecars.
- **Pluggable architecture**: Easily add components (e.g., `aspire add redis`) or write custom publishers for in-house services and target any platform (ECS, Nomad, etc.).
- **No lock-in**: Generated artifacts are standard YAML and can be modified or discarded as needed.

If managing YAML files is working, you needn’t change. But if a cleaner, code-centric approach appeals, Aspire can collapse the sprawl and streamline your workflow.

*Author: David Fowler — Distinguished Engineer at Microsoft*

This post appeared first on "David Fowler's Blog". [Read the entire article here](https://medium.com/@davidfowl/taming-manifest-sprawl-with-aspire-1ad938379433?source=rss-8163234c98f0------2)
