---
layout: "post"
title: "Introducing the Microsoft Fabric Extensibility Toolkit"
description: "This announcement presents the Microsoft Fabric Extensibility Toolkit, a successor to the Workload Development Kit. The toolkit enables developers and organizations to rapidly create, integrate, and publish data applications into Microsoft Fabric workspaces. It streamlines authentication, state management, OneLake integration, and leverages Copilot-ready starter kits to accelerate development. Key capabilities include integrated authentication with Microsoft Entra ID, scalable metadata storage, OneLake data access, and samples for quick onboarding. Practical examples include the Package Installer and OneLake Item Explorer, both of which demonstrate extensibility and integration potential within Fabric."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/introducing-the-microsoft-fabric-extensibility-toolkit/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-09-22 15:30:00 +00:00
permalink: "/2025-09-22-Introducing-the-Microsoft-Fabric-Extensibility-Toolkit.html"
categories: ["AI", "Azure", "Coding", "ML"]
tags: ["AI", "API Integration", "Authentication", "Azure", "Coding", "Copilot", "Custom Data Applications", "Data Integration", "Developer Tools", "Fabric Extensibility Toolkit", "Fabric Workspace", "Front End Development", "Item Explorer", "Metadata Management", "Microsoft Entra ID", "Microsoft Fabric", "ML", "News", "OneLake", "Package Installer", "Power BI", "SaaS", "Spark", "Starter Kit", "Workload Development Kit"]
tags_normalized: ["ai", "api integration", "authentication", "azure", "coding", "copilot", "custom data applications", "data integration", "developer tools", "fabric extensibility toolkit", "fabric workspace", "front end development", "item explorer", "metadata management", "microsoft entra id", "microsoft fabric", "ml", "news", "onelake", "package installer", "power bi", "saas", "spark", "starter kit", "workload development kit"]
---

Microsoft Fabric Blog introduces the Fabric Extensibility Toolkit, showcasing how developers can build, integrate, and publish custom data applications directly within Microsoft Fabric, using streamlined APIs and Copilot-powered starter kits.<!--excerpt_end-->

# Introducing the Microsoft Fabric Extensibility Toolkit

The Microsoft Fabric Extensibility Toolkit is the next evolution of the Workload Development Kit. Designed to simplify and accelerate the development, integration, and deployment of data-centric applications within Microsoft Fabric, this toolkit helps organizations and developers deliver solutions faster with deep platform integration.

## Key Features and Capabilities

- **Publish Custom Data Apps:** Easily publish data applications into Fabric workspaces as first-class items, benefiting from native governance, security, and administration.
- **Integrated Authentication:** Securely access Fabric APIs via Microsoft Entra tokens for seamless authorization from the frontend.
- **Metadata and State Management:** Store item metadata securely and at scale using Fabric's services. See [documentation](https://review.learn.microsoft.com/fabric/extensibility-toolkit/how-to-store-item-definition).
- **OneLake Integration:** Use the toolkit to store and access data within OneLake, and create shortcuts to lakehouses or external stores such as Amazon S3.
- **Native Service Integration:** Incorporate Power BI reports, Spark jobs, and other services directly into your application using simplified API access.
- **Copilot-Ready Starter Kits:** Kickstart your development with Copilot-enabled starter kits and samples designed for modern programming approaches, reducing development time to hours or days.
- **Standardized Item Management:** Built-in controls for item creation, management, and deployment eliminate the need for custom flows.
- **Frontend-Focused Development:** Simplify the integration of UX with backend services or Microsoft APIs, including Fabric, Azure, and Office.

## Example Components

### Package Installer

- Installs preconfigured items into Fabric, can create new workspaces, update data, and schedule items. [GitHub Repository](https://github.com/microsoft/Microsoft-Fabric-tools-workload/tree/main/Workload/app/items/PackageInstallerItem)

### OneLake Item Explorer

- Opens Fabric items for OneLake content exploration, with support for creating, updating, deleting files, and managing shortcuts. [GitHub Repository](https://github.com/microsoft/Microsoft-Fabric-tools-workload/tree/main/Workload/app/items/OneLakeExplorerItem)

## Developer Onboarding

Developers can get started quickly using the [Starter Kit](https://aka.ms/fabric-extensibility-starter-kit), which includes Copilot-enabled samples and a 'Hello World' project. The toolkit supports building scalable, secure applications that are deeply integrated into the Fabric ecosystem, leveraging enterprise-grade SaaS capabilities and Fabric's infrastructure.

Join the [developer community](https://aka.ms/fabric-extensibility-toolkit-samples) and browse the samples repository for practical examples, reusable items, and integration guidance.

## Summary

The Fabric Extensibility Toolkit makes it possible to:

- Develop specialized data applications for Fabric
- Integrate deeply with platform features like authentication, data storage, and native services
- Utilize rapid development cycles with Copilot support and streamlined tooling
- Benefit from secure, scalable, and enterprise-ready deployment within Fabric

Whether you're building custom visualizations, streamlined data integrations, or enterprise-grade applications, the toolkit provides robust infrastructure to ensure rapid delivery and easy maintenance.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/introducing-the-microsoft-fabric-extensibility-toolkit/)
