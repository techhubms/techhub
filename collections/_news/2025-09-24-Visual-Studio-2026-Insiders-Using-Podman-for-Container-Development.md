---
layout: "post"
title: "Visual Studio 2026 Insiders: Using Podman for Container Development"
description: "This announcement details the new integration of Podman, an open-source, rootless container engine, into Visual Studio 2026 Insiders. It explores the benefits of daemonless and secure container workflows, compares Podman to Docker, and provides step-by-step guidance on using Podman for containerized application development directly within Visual Studio."
author: "Matt Hernandez"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/blog/visual-studio-2026-insiders-using-podman-for-container-development"
viewing_mode: "external"
feed_name: "Microsoft Blog"
feed_url: "https://devblogs.microsoft.com/feed"
date: 2025-09-24 17:30:30 +00:00
permalink: "/news/2025-09-24-Visual-Studio-2026-Insiders-Using-Podman-for-Container-Development.html"
categories: ["Coding", "DevOps"]
tags: ["Cloud Native", "Coding", "Container Development", "Container Engine", "Container Runtime", "Debugging", "Developer Tools", "DevOps", "Docker Alternative", "IDE", "Image Management", "Integrated Terminal", "News", "Open Source", "Podman", "Uncategorized", "VS", "Windows Development"]
tags_normalized: ["cloud native", "coding", "container development", "container engine", "container runtime", "debugging", "developer tools", "devops", "docker alternative", "ide", "image management", "integrated terminal", "news", "open source", "podman", "uncategorized", "vs", "windows development"]
---

Matt Hernandez introduces Podman integration in Visual Studio 2026 Insiders, showing how developers can create, run, and debug containers securely within the Visual Studio environment.<!--excerpt_end-->

# Visual Studio 2026 Insiders: Using Podman for Container Development

## Introduction

Podman is an open-source container engine ideal for developers who require a daemonless and rootless solution. Unlike Docker, Podman runs containers without a central service and without requiring elevated privileges, improving security and flexibility, especially in enterprise and cloud-native environments.

---

## Podman Integration in Visual Studio 2026 Insiders

With the latest Visual Studio 2026 Insiders release, Microsoft introduces support for Podman in the container development workflow. Historically, Visual Studio’s container tooling focused on Docker, but now developers can choose Podman, preserving familiar workflows while benefiting from Podman's unique advantages.

### Key Benefits

- **Daemonless and rootless architecture**: No central daemon, enhancing security
- **Docker CLI Compatibility**: Transition from Docker with minimal changes
- **Enhanced developer choice**: Select between Docker and Podman as your container runtime

![Podman Desktop Screenshot](https://devblogs.microsoft.com/wp-content/uploads/2025/09/podman_desktop.png)

## Using Podman in Visual Studio 2026 Insiders

### Getting Started

1. Install the latest [Visual Studio 2026 Insiders](http://aka.ms/vs/insiders).
2. Install and configure [Podman Desktop](https://podman-desktop.io/).
3. Open Visual Studio. It will auto-detect available Podman containers.
4. Interact with containers via:
   - Solution Explorer
   - Integrated terminal (Podman commands)
   - Built-in debugging tools

### Configuration

- If both Docker and Podman are installed, select your runtime in Visual Studio:
  - **Tools** > **Options** > **Container Tools** > **General** > **Container Runtime**
  - Choose **Podman** from the dropdown

![Podman CTW Screenshot](https://devblogs.microsoft.com/wp-content/uploads/2025/09/podman_CTW.png)

## Workflow Highlights

- Create, run, debug, and manage containers from within Visual Studio
- Familiar UI and experience for developers used to Docker
- Manage images and view logs inside the IDE

## Forward-Looking Statement

This addition reflects Microsoft’s focus on supporting open-source development. As containerization maintains its relevance in software architecture, Visual Studio’s Podman integration offers developers greater flexibility, enhanced security, and a modern development experience.

For feedback and issues, participate on [this GitHub issue](https://github.com/microsoft/DockerTools/issues/459).

This post appeared first on "Microsoft Blog". [Read the entire article here](https://devblogs.microsoft.com/blog/visual-studio-2026-insiders-using-podman-for-container-development)
