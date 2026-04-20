---
title: 'Best of Both Worlds for Agentic Refactoring: GitHub Copilot + MicroVMs via Docker Sandbox'
external_url: https://devblogs.microsoft.com/all-things-azure/best-of-both-worlds-for-agentic-refactoring-github-copilot-microvms-via-docker-sandbox/
section_names:
- ai
- devops
- dotnet
- github-copilot
- security
author: cindywang
date: 2026-04-16 22:09:53 +00:00
feed_name: Microsoft All Things Azure Blog
primary_section: github-copilot
tags:
- .NET
- .NET Modernization
- /var/run/docker.sock
- Absolute Paths
- Agentic Coding
- Agentic Refactoring
- AI
- All Things Azure
- Async/await Refactoring
- Bidirectional Filesystem Sync
- CI Testing
- Containerization
- Data Exfiltration Prevention
- Deny All Egress
- Dependency Upgrades
- DevOps
- Docker Build
- Docker Compose
- Docker Desktop
- Docker Sandbox
- Docker Socket
- Fleet Execution
- GitHub Copilot
- GitHub Copilot CLI
- GitHub Enterprise Authentication
- HTTP/HTTPS Proxy
- Java Modernization
- Legacy Modernization
- MicroVMs
- Modernization
- News
- OCI Images
- Parallel Agents
- Platform Engineering
- Sandboxed Docker Daemon
- Security
- Supply Chain Security
- Technical Debt Reduction
- Thought Leadership
- Workspace Sync
- YOLO Mode
---

cindywang explains how GitHub Copilot agents can modernize legacy Java and .NET code inside Docker Sandbox microVMs, keeping host filesystem paths consistent while avoiding risky Docker socket mounts and tightening egress controls during dependency upgrades.<!--excerpt_end-->

## Overview

Legacy codebases often rely on hardcoded logic and build scripts that assume specific filesystem layouts, which makes modernization difficult in isolated environments. This article describes using **GitHub Copilot (agentic refactoring)** together with **Docker Sandbox** (microVM-based isolation) to modernize legacy applications while keeping developer workflows productive and reducing host security risk.

A key capability highlighted is **bidirectional workspace sync** that preserves **the same absolute paths** inside the sandbox as on the host. That consistency helps keep file references, build outputs, error messages, and IDE tooling aligned across the isolation boundary.

## Why Docker access is a pain point for coding agents

Agentic modernization workflows commonly need to:

- Build container images (`docker build`)
- Run integration tests inside containers
- Orchestrate multi-container stacks (`docker compose`)

In a typical container setup, enabling this often means mounting the host Docker socket:

- `/var/run/docker.sock`

That effectively gives the agent root-level access to the host Docker daemon, which is a serious security risk (e.g., enumerating containers, pulling sensitive images, or escaping the sandbox).

## Testing containerized legacy code in isolation

Docker Sandboxes are presented as a mitigation by providing:

- A **private Docker daemon** running inside a **microVM**
- Isolation from the host’s primary Docker environment

That lets an agent build and test containerized versions of legacy code without visibility into (or impact on) the host Docker daemon.

### Example workflow

```bash
$ docker sandbox run copilot ~/asset-manager-mod

Creating new sandbox ‘copilot-asset-manager-mod’…
...
✓ Created sandbox copilot-sandbox-2026-03-26-160741 in VM copilot-asset-manager-mod

Workspace: C:\Users\cindywang\asset-manager-mod

Agent: copilot

To connect to this sandbox, run:

docker sandbox run copilot-asset-manager-mod

Starting copilot agent in sandbox ‘copilot-asset-manager-mod’…

Authentication is being handled through delegation. Within the terminal, type /login to complete auth via GitHub or GitHub Enterprise.
```

![Docker Sandbox terminal output showing a Copilot sandbox being created](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2026/04/docker-sandbox.webp)

The article references a sample legacy Java app used for containerization and refactoring preparation:

- http://java-migration-copilot-samples/asset-manager%20at%20main%20·%20Azure-Samples/java-migration-copilot-samples

## Mitigating risk during dependency upgrades

Modernization often requires upgrading large numbers of dependencies, which increases **supply chain attack** risk (for example, pulling malicious packages from public registries).

Docker Sandboxes mitigate this with:

- HTTP/HTTPS filtering proxies
- A “smart deny-all” policy
- Allowing access to essential registries (e.g., npm, PyPI)
- Blocking access to local networks and cloud metadata endpoints to reduce data exfiltration risk during automated upgrades

![Docker Sandbox UI showing dependency management in an isolated environment](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2026/04/manage-dependency.webp)

## From periodic clean-ups to continuous modernization operations

The article argues that agentic modernization can shift from occasional refactoring efforts to continuous, high-throughput operations.

Examples of repetitive modernization tasks include:

- Updating deprecated APIs
- Converting callbacks to `async/await`
- Adding error handling

A common bottleneck is “approval fatigue” when agents require constant human permission prompts. The article suggests Docker Sandboxes enable running agents in “YOLO mode” (commands execute without prompts) because:

- The microVM provides a hard security boundary to protect the host from destructive commands (e.g., `rm -rf /`) and malicious code execution

It also mentions organizational impact:

- Fleets of agents can run mass refactoring across many repositories in parallel
- Autonomous agents merge ~60% more pull requests than those requiring constant supervision (as claimed in the article)
- At enterprise scale, a secure sandbox can reclaim developer time and reduce technical debt

### Example: parallel agent executions (“fleets”)

The article shows a Docker Desktop view of parallel executions:

![Docker Desktop showing parallel agent executions (fleet view 1)](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2026/04/fleet1.webp)

![Docker Desktop showing parallel agent executions (fleet view 2)](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2026/04/fleet2.webp)

![Docker Desktop showing parallel agent executions (fleet view 3)](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2026/04/fleet3.webp)

![Docker Desktop showing parallel agent executions (fleet view 4)](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2026/04/fleet4.webp)

![Docker Desktop showing parallel agent executions (fleet view 5)](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2026/04/fleet5.webp)

![Docker Desktop showing parallel agent executions (fleet view 6)](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2026/04/fleet6.webp)

![Docker Desktop showing parallel agent executions (fleet view 7)](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2026/04/fleet7.webp)

## Preserving absolute paths with workspace synchronization

A recurring issue with virtualized dev environments is mismatch between host and guest filesystem paths.

GitHub Copilot + Docker Sandboxes address this via:

- **Bidirectional workspace synchronization**
- Mounting the developer’s project directory directly into the sandbox
- Maintaining the **same absolute path** inside the microVM as on the host

This is positioned as important for:

- Build tool compatibility
- Consistent error messages and configuration paths
- IDE integration coherence

Example given:

- Host path `/Users/dev/my-project` on macOS is preserved for the agent inside the microVM

Using VS Code as an example:

![VS Code showing a synchronized filesystem view between host and sandbox](https://devblogs.microsoft.com/all-things-azure/wp-content/uploads/sites/83/2026/04/filesys.webp)


[Read the entire article](https://devblogs.microsoft.com/all-things-azure/best-of-both-worlds-for-agentic-refactoring-github-copilot-microvms-via-docker-sandbox/)

