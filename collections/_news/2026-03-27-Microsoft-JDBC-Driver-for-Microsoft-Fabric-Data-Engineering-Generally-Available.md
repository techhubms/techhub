---
author: Microsoft Fabric Blog
section_names:
- ml
- security
feed_name: Microsoft Fabric Blog
primary_section: ml
date: 2026-03-27 09:00:00 +00:00
tags:
- Access Tokens
- Apache Spark
- ARRAY
- Asynchronous Prefetching
- Certificate Based Authentication
- Circuit Breaker
- Client Credentials
- Connection Pooling
- DBeaver
- DbVisualizer
- Fabric Data Engineering
- HikariCP
- HTTP Proxy
- Java 11
- Java 17
- Java 21
- JDBC
- Lakehouse
- Livy APIs
- MAP
- Microsoft Entra ID
- Microsoft Fabric
- Microsoft JDBC Driver
- ML
- News
- OAuth
- OneLake
- Power BI
- Proxy Support
- Retry Logic
- Security
- SOCKS Proxy
- Spark SQL
- Spark SQL Data Types
- STRUCT
- Tableau
external_url: https://blog.fabric.microsoft.com/en-US/blog/microsoft-jdbc-driver-for-microsoft-fabric-data-engineering-generally-available/
title: Microsoft JDBC Driver for Microsoft Fabric Data Engineering (Generally Available)
---

Microsoft Fabric Blog announces general availability of the Microsoft JDBC Driver for Fabric Data Engineering, a production-ready way to connect Java tools to Fabric Spark SQL lakehouse workloads with Entra ID auth, connection pooling, and resilience features.<!--excerpt_end-->

# Microsoft JDBC Driver for Microsoft Fabric Data Engineering (GA)

The Microsoft Fabric Blog announced the general availability of the **Microsoft JDBC Driver for Microsoft Fabric Data Engineering**, an enterprise-grade JDBC 4.2 driver that connects Java applications and BI tools to **Spark SQL workloads running in Microsoft Fabric** using **Fabric’s Livy APIs** as the execution layer.

## Why this matters

- **JDBC is a standard** way for Java applications (and many data tools) to connect to data platforms.
- Since **Apache Spark** is central to large-scale data processing, teams often need a reliable way to integrate Spark workloads with existing applications and tooling.
- This driver targets **mission-critical, enterprise-scale data engineering workloads** in Microsoft Fabric.

## Deep integration with Fabric Data Engineering

The driver is designed specifically for **Fabric Data Engineering** and integrates with Fabric capabilities such as:

- Native access to **Lakehouse data in OneLake**
- Executing jobs using **Fabric Environment items**
- Applying **custom Spark configurations** per workload
- Secure execution via **Microsoft Fabric Livy APIs**

## Key features

- **JDBC 4.2 compliant**
  - Works out-of-the-box with **Java 11, 17, and 21**
  - Intended to work with tools including **DbVisualizer**, **DBeaver**, **Tableau**, and **Power BI (via JDBC)**

- **Enterprise authentication (Microsoft Entra ID)**
  - Supports multiple flows:
    - Interactive browser
    - Client credentials
    - Certificate-based authentication
    - Access tokens

- **Connection pooling and throughput**
  - Built-in pooling with:
    - Health monitoring
    - Automatic recovery
  - Integration with **HikariCP**

- **Native Spark SQL support**
  - Execute Spark SQL statements directly
  - Supports Spark SQL types including complex types:
    - `ARRAY`
    - `MAP`
    - `STRUCT`

- **Performance and resilience**
  - Asynchronous result-set prefetching
  - Circuit breaker patterns
  - Automatic reconnection
  - Advanced retry logic

- **Enterprise networking and diagnostics**
  - Full **HTTP/SOCKS proxy** support
  - Configurable logging

## Quick demo (DbVisualizer)

The post includes an example showing how to set up the driver in **DbVisualizer** and run a **Spark SQL** query against lakehouse data.

![Animated view of the DbVisualizer main menu showing options like Edit, View, Database, Files, and Favorites, with a submenu open for creating a database connection, installing a demo database, and customizing the application.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/animated-view-of-the-dbvisualizer-main-menu-showin.gif)

Figure: The DbVisualizer main menu, highlighting options for managing databases, creating connections, installing a demo database, and customizing the application.

## Get started

Documentation: https://learn.microsoft.com/fabric/data-engineering/spark-jdbc-driver

## Notes from the announcement

The blog states Microsoft will continue investing in **performance, security, and developer experience** improvements as Fabric Data Engineering evolves, and invites users to adopt the GA release and share feedback.

[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/microsoft-jdbc-driver-for-microsoft-fabric-data-engineering-generally-available/)

