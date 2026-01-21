---
external_url: https://blog.fabric.microsoft.com/en-US/blog/microsoft-jdbc-driver-for-microsoft-fabric-data-engineering-preview/
title: Enterprise-Grade JDBC Driver for Microsoft Fabric Data Engineering Preview
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-11-25 11:00:00 +00:00
tags:
- Authentication
- Azure Entra ID
- BI Tools
- Certificate Authentication
- Circuit Breaker
- Connection Pooling
- Data Engineering
- DBeaver
- DbVisualizer
- HikariCP
- Java
- JDBC Driver
- Lakehouse
- Logging
- Microsoft Fabric
- OneLake
- Power BI
- Preview Release
- Proxy Support
- Spark SQL
- Tableau
section_names:
- azure
- ml
---
Microsoft Fabric Blog details the new JDBC Driver for Fabric Data Engineering, empowering developers and data engineers with secure, high-performance Spark SQL connectivity.<!--excerpt_end-->

# Microsoft JDBC Driver for Microsoft Fabric Data Engineering (Preview)

JDBC (Java Database Connectivity) is a widely adopted standard that enables client applications to connect to and work with data from databases and big data platforms.

## Overview

The Microsoft JDBC Driver for Microsoft Fabric Data Engineering (Preview) brings robust, secure, and flexible Spark SQL connectivity to your Java applications and BI tools, using Microsoft Fabric's Livy APIs.

### Why It Matters

With the increasing adoption of Apache Spark for scalable data engineering and analytics, seamless integration into enterprise platforms is essential. This driver empowers developers, data engineers, and administrators to connect, query, and manage Spark workloads in Microsoft Fabric using familiar JDBC tools.

## Deep Integration with Fabric

- **Lakehouse Support:** Access data directly in OneLake.
- **Environment Items:** Use environment items during job execution.
- **Dynamic Spark Configuration:** Tailor Spark jobs to unique requirements.

![Fabric JDBC Driver Example in DbVisualizer](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/11/a-screenshot-of-a-computer-ai-generated-content-m-2.gif)

## Key Features

- **JDBC 4.2 Compliance:** Out-of-the-box compatibility with Java 11, 17, 21. Supports Tableau, Power BI (via JDBC connector), DBeaver, DbVisualizer, and more.
- **Enterprise-Grade Authentication:** Multiple Azure Entra ID (Azure Active Directory) flows including interactive browser, client credentials, certificate, and access token authentication.
- **Connection Pooling:** Built-in pooling, health monitoring, automatic recovery, HikariCP integration for high-throughput needs.
- **Spark SQL Support:** Full support for Spark SQL queries and all Spark SQL data types, including ARRAY, MAP, STRUCT.
- **Performance & Resilience:** Asynchronous result prefetching, circuit breaker pattern, auto-reconnect, advanced retry logic.
- **Proxy & Logging:** HTTP/SOCKS proxy support, customizable logging for enterprise environments.

## Getting Started

To download and learn more, view the official [Spark connector for Microsoft Fabric Data Warehouse](https://aka.ms/fabricspark-jdbc) documentation.

## Conclusion

The Microsoft JDBC Driver for Fabric Data Engineering streamlines Spark-powered data engineering in Microsoft Fabric, supporting secure integration, performance, and reliability for both developers and data engineers.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/microsoft-jdbc-driver-for-microsoft-fabric-data-engineering-preview/)
