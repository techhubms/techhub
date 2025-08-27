---
layout: "post"
title: "Introducing .NET Aspire Event Hub Live Explorer"
description: "LupusOnFire presents the Event Hub Live Explorer, a Blazor-based open-source tool for interacting with Azure Event Hubs during local development. It integrates with .NET Aspire dashboards, providing real-time message sending and reading capabilities to streamline event-driven workflows and diagnostics."
author: "LupusOnFire"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/dotnet/comments/1mgm401/introducing_net_aspire_event_hub_live_explorer/"
viewing_mode: "external"
feed_name: "Reddit DotNet"
feed_url: "https://www.reddit.com/r/dotnet/.rss"
date: 2025-08-03 15:28:59 +00:00
permalink: "/2025-08-03-Introducing-NET-Aspire-Event-Hub-Live-Explorer.html"
categories: ["Azure", "Coding", "DevOps"]
tags: [".NET", ".NET Aspire", "Azure", "Azure Event Hubs", "Blazor", "Coding", "Community", "Consumer Groups", "Dashboard Integration", "Developer Tooling", "DevOps", "Event Driven Workflows", "Local Development", "NuGet Package", "Open Source"]
tags_normalized: ["dotnet", "dotnet aspire", "azure", "azure event hubs", "blazor", "coding", "community", "consumer groups", "dashboard integration", "developer tooling", "devops", "event driven workflows", "local development", "nuget package", "open source"]
---

Authored by LupusOnFire, this article introduces the .NET Aspire Event Hub Live Explorer—an open-source Blazor tool for local development with Azure Event Hubs.<!--excerpt_end-->

## Introducing .NET Aspire Event Hub Live Explorer

**Author:** LupusOnFire

While migrating Azure projects to .NET Aspire to improve local development, LupusOnFire encountered the recurring need to publish events to locally emulated Azure Event Hubs in order to emulate and trigger event-driven workflows.

Because Azure Event Hub Explorer is unavailable for local environments, an initial workaround—a minimal console application—was functional but inefficient for frequent use. To address this, LupusOnFire developed Event Hub Live Explorer, an open-source, Blazor-based frontend for Azure Event Hubs.

**Repository:** [https://github.com/lupusbytes/event-hub-live-explorer](https://github.com/lupusbytes/event-hub-live-explorer)

### What is Event Hub Live Explorer?

Event Hub Live Explorer provides a streamlined UI for interacting with [Azure Event Hubs](https://learn.microsoft.com/en-us/azure/event-hubs/), supporting both sending and receiving events in real time. It is designed for:

- Local development
- Testing
- Diagnostics

Event Hub Live Explorer is built to integrate smoothly into .NET Aspire dashboards, enhancing the developer experience and reducing setup friction for event-driven system development.

### Features

- **Send messages** directly to Event Hubs
- **Read events** from multiple partitions in real time
- **Integrates with .NET Aspire**
- **Ideal for local development and testing** of event-based systems

---

## Usage in .NET Aspire

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

### Installation and Setup

1. **Add the NuGet package** to your Aspire AppHost project:

   ```bash
   dotnet add package LupusBytes.Aspire.Hosting.Azure.EventHubs.LiveExplorer
   ```

2. **Add Event Hub Live Explorer to your Aspire Dashboard:**

   ```csharp
   var explorer = builder.AddEventHubLiveExplorer();
   ```

3. **Reference an Event Hub:**

   ```csharp
   var eventHub = builder
       .AddAzureEventHubs("event-hub-namespace").RunAsEmulator()
       .AddEventHub("event-hub");
   explorer.WithReference(eventHub);
   ```

   This will enable the Explorer to connect using the `$Default` consumer group.

4. **Use a Different Consumer Group:**

   ```csharp
   var eventHubWithCustomConsumerGroup = eventHub.AddConsumerGroup("explorer");
   explorer.WithReference(eventHubWithCustomConsumerGroup);
   ```

5. **Add All Event Hubs Automatically:**
   Use the convenience method to reduce boilerplate by referencing all added Event Hubs and creating consumer groups if needed:

   ```csharp
   builder
     .AddAzureEventHubs("event-hub-namespace").RunAsEmulator()
     .AddEventHub("event-hub1")
     .AddEventHub("event-hub2")
     .AddEventHub("event-hub3")
     .AddEventHub("event-hub4");

   explorer.WithAutoReferences(consumerGroupName: "explorer");
   ```

   > **Note:**
   > This must be called after all desired Event Hubs have been added. Event Hubs added after this call will not be referenced.

### More Information

- For documentation, usage samples, and contribution guidelines, see the project repository: [https://github.com/lupusbytes/event-hub-live-explorer](https://github.com/lupusbytes/event-hub-live-explorer)

This post appeared first on "Reddit DotNet". [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1mgm401/introducing_net_aspire_event_hub_live_explorer/)
