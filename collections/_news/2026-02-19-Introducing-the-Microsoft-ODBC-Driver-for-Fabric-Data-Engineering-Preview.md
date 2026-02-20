---
layout: "post"
title: "Introducing the Microsoft ODBC Driver for Fabric Data Engineering (Preview)"
description: "This news article introduces the Microsoft ODBC Driver for Microsoft Fabric Data Engineering (Preview), an enterprise-grade connector that provides secure, reliable Spark SQL connectivity within Microsoft Fabric. It highlights key features, integration with lakehouse and OneLake, authentication options, and support for .NET, Python, and other ODBC-compatible applications."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/microsoft-odbc-driver-for-microsoft-fabric-data-engineering-preview/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2026-02-19 17:00:00 +00:00
permalink: "/2026-02-19-Introducing-the-Microsoft-ODBC-Driver-for-Fabric-Data-Engineering-Preview.html"
categories: ["Azure", "ML"]
tags: [".NET", "Authentication", "Azure", "BI Tools", "Data Connectivity", "Data Engineering", "Data Integration", "Enterprise Security", "Lakehouse", "Microsoft Entra ID", "Microsoft Fabric", "ML", "News", "ODBC Driver", "OneLake", "Python", "Session Management", "Spark SQL"]
tags_normalized: ["dotnet", "authentication", "azure", "bi tools", "data connectivity", "data engineering", "data integration", "enterprise security", "lakehouse", "microsoft entra id", "microsoft fabric", "ml", "news", "odbc driver", "onelake", "python", "session management", "spark sql"]
---

Microsoft Fabric Blog announces the Microsoft ODBC Driver for Fabric Data Engineering (Preview), detailing its secure Spark SQL connectivity for developers, data engineers, and administrators.<!--excerpt_end-->

# Microsoft ODBC Driver for Microsoft Fabric Data Engineering (Preview)

ODBC (Open Database Connectivity) is a standardized protocol enabling client applications to access and interact with databases and big data platforms. The newly released Microsoft ODBC Driver for Microsoft Fabric Data Engineering (Preview) is an enterprise-grade connector providing reliable, secure, and flexible Spark SQL connectivity for .NET, Python, and other ODBC-compatible applications as well as business intelligence tools, powered by Microsoft Fabric’s Livy APIs.

## Why this matters

As organizations increasingly depend on Apache Spark for large-scale data engineering and analytics, seamless integration with enterprise platforms like Microsoft Fabric becomes even more vital. The ODBC driver empowers developers, data engineers, and administrators to connect, query, and manage Spark workloads in Microsoft Fabric using a familiar ODBC interface, boosting productivity and enabling reliability.

![Animated GIF demonstrating ODBC Data Source Administrator with a Fabric Spark data source.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/02/Microsoft-ODBC-Driver.gif)

*Figure 1: Getting started using the ODBC driver.*

The driver offers deep integration, including:

- Access to lakehouse data in OneLake
- Environment item specification during job execution
- Custom Spark configuration based on project needs

## Key Features

- **ODBC 3.x Compliant:** Supports the complete ODBC 3.x specification
- **Microsoft Entra ID Authentication:** Flexible authentication options: Azure CLI, interactive, client credentials, certificate-based, and access token methods
- **Spark SQL Query Support:** Directly execute Spark SQL statements
- **Comprehensive Data Type Support:** Full support for Spark SQL data types, including complex types like ARRAY, MAP, and STRUCT
- **Session Reuse:** Built-in session management for better performance
- **Large Table Support:** Efficient handling of large result sets with configurable page sizes
- **Async Prefetch:** Improved performance with background data loading
- **Proxy Support:** HTTP proxy configuration for enterprise scenarios
- **Multi-Database Lakehouse Support:** Connect to specific databases within a lakehouse

The driver is designed to facilitate Spark-powered data engineering projects with the security, reliability, and performance needed for enterprise scenarios.

## Next Steps

You are encouraged to try the ODBC driver, provide feedback, and explore new analytics and integration options in Microsoft Fabric.

For more details or to download the driver, see the official documentation: [Microsoft ODBC Driver for Microsoft Fabric Data Engineering](https://learn.microsoft.com/fabric/data-engineering/spark-odbc-driver).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/microsoft-odbc-driver-for-microsoft-fabric-data-engineering-preview/)
