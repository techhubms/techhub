---
layout: "post"
title: "Fabric User Data Functions Now Generally Available"
description: "This official announcement highlights the general availability of User Data Functions in Microsoft Fabric. The content covers the benefits and key features such as customization with Python, reusability in data architectures, integration with Fabric data sources, development tool support, OpenAPI specification generation, and the newest enhancements for developers using Power BI, Data Pipelines, and Notebooks."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/announcing-fabric-user-data-functions-now-in-general-availability/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-09-24 11:00:00 +00:00
permalink: "/2025-09-24-Fabric-User-Data-Functions-Now-Generally-Available.html"
categories: ["Azure", "Coding", "ML"]
tags: ["API Integration", "Async Functions", "Azure", "Coding", "Custom Functions", "Data Engineering", "Data Pipelines", "Fabric Lakehouse", "Fabric SQL Database", "Microsoft Fabric", "ML", "News", "Notebooks", "OpenAPI", "Pandas", "Power BI", "Python", "User Data Functions", "VS Code"]
tags_normalized: ["api integration", "async functions", "azure", "coding", "custom functions", "data engineering", "data pipelines", "fabric lakehouse", "fabric sql database", "microsoft fabric", "ml", "news", "notebooks", "openapi", "pandas", "power bi", "python", "user data functions", "vs code"]
---

Microsoft Fabric Blog details the general availability of User Data Functions, allowing developers and data engineers to build, customize, and deploy reusable business logic across the Fabric platform.<!--excerpt_end-->

# Fabric User Data Functions Now Generally Available

The Microsoft Fabric team announces that User Data Functions are now generally available, empowering users to embed business logic directly into Fabric solutions. This feature has proven valuable through its preview, being used for diverse scenarios in data engineering, analytics, and translytical applications.

## Benefits of User Data Functions

- **Reusability:** Functions can be shared and integrated across different areas of the Fabric ecosystem.
- **Customization:** Write custom functions in Python, leveraging the full ecosystem of PyPI libraries.
- **Encapsulation:** Abstract complex procedures into straightforward function calls, making your solutions more maintainable.
- **External Connectivity:** Functions can interact with external systems and also be invoked from outside Fabric using public endpoints.

## Core Features

- **Flexible Development Tools:** Craft, edit, and test functions using the in-browser portal or the VS Code extensions, both of which are now generally available.
- **Comprehensive Integration:** Easily connect with a range of Fabric data sources, such as Fabric SQL Database, Fabric Warehouse, Fabric Lakehouse, and mirrored DBs.
- **Flexible Execution:** Run your functions from Power BI reports, Data Pipelines, Notebooks, or external client applications using public endpoints.
- **Development Enhancements:**
  - Test and validate with Develop mode
  - Generate OpenAPI specifications for your functions
  - Full support for async functions and data handling with Pandas

## Release Updates

- User Data Functions are now available for all Microsoft Fabric tenants without requiring special admin configuration.
- Expanded geographic region support ensures more organizations can leverage these capabilities globally.
- The Visual Studio Code extensions for Fabric and User Data Functions are now generally available for streamlined development and deployment workflows.
- Recent feature rollouts include:
  - **Develop mode** for enhanced testing
  - **OpenAPI code generation** for easier API integration
  - **Async and Pandas dataframe/series support**

## Billing and Operations

Testing capabilities in Develop mode are universally available for all tenants with audit billing enabled. For billing implications, refer to the official [Fabric operations documentation](https://learn.microsoft.com/fabric/enterprise/fabric-operations#fabric-user-data-functions).

## Learn More and Get Started

- [Getting started guide for User Data Functions](https://learn.microsoft.com/fabric/data-engineering/user-data-functions/create-user-data-functions-portal)
- [Comprehensive documentation](https://learn.microsoft.com/fabric/data-engineering/user-data-functions/user-data-functions-overview)
- [Service details and limitations](https://learn.microsoft.com/fabric/data-engineering/user-data-functions/user-data-functions-service-limits)

---

By embracing User Data Functions, developers and data engineers can build scalable, maintainable solutions that deeply integrate with the Microsoft Fabric ecosystem, leveraging the full power of Python and the broader open-source stack.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/announcing-fabric-user-data-functions-now-in-general-availability/)
