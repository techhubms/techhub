---
tags:
- .NET
- ADO.NET
- ADO.NET Provider
- Apache Spark
- Azure CLI Authentication
- Certificate Authentication
- Client Credentials
- Connection Pooling
- DbCommand
- DbConnection
- DbDataReader
- Fabric Data Engineering
- Lakehouse
- Livy API
- Microsoft Entra ID
- Microsoft Fabric
- ML
- News
- OAuth
- OneLake
- Parameterized Queries
- Security
- Session Reuse
- Spark SQL
primary_section: ml
date: 2026-04-06 09:00:00 +00:00
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/microsoft-ado-net-driver-for-microsoft-fabric-data-engineering-preview/
section_names:
- dotnet
- ml
- security
title: Microsoft ADO.NET Driver for Microsoft Fabric Data Engineering (Preview)
---

Microsoft Fabric Blog announces a preview ADO.NET driver that lets .NET apps connect to Microsoft Fabric Data Engineering Spark workloads via Livy APIs, using familiar ADO.NET abstractions plus Entra ID authentication, pooling, session reuse, and Spark SQL support.<!--excerpt_end-->

## Overview

ADO.NET is a common data access technology in the .NET ecosystem for connecting applications to databases and big data platforms.

The **Microsoft ADO.NET Driver for Microsoft Fabric Data Engineering (Preview)** enables .NET applications to **connect, query, and manage Apache Spark workloads in Microsoft Fabric** using standard ADO.NET patterns. The driver is **built on Microsoft Fabric’s Livy APIs** and provides Spark SQL connectivity using familiar ADO.NET abstractions.

Key ADO.NET abstractions supported:

- `DbConnection`
- `DbCommand`
- `DbDataReader`

## Why this matters

As organizations rely more on **Apache Spark** for scalable data engineering and analytics, this driver aims to make Spark connectivity from .NET more straightforward while aligning with enterprise requirements.

The driver is designed specifically for **Fabric Data Engineering**, with:

- Deep integration with **lakehouse** data access in **OneLake**
- Ability to use an **environment item** during job execution
- Support for different **Spark configurations** based on workload needs

## Example

![Screenshot of a C# code snippet demonstrating asynchronous connection to Microsoft Fabric using Spark.Livy.AdoNet library, executing a SQL query, and printing results to console. Code includes connection string setup with placeholders, command execution, and output of "Connected successfully!" and "Hello from Fabric!" in terminal below.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-c-code-snippet-demonstrating-asyn.png)

*Figure: Example of using the ADO.NET driver.*

## Key features

- **ADO.NET compliant**: Implements core ADO.NET abstractions:
  - `DbConnection`, `DbCommand`, `DbDataReader`, `DbParameter`, `DbProviderFactory`
- **Microsoft Entra ID authentication**: Multiple auth flows, including:
  - Azure CLI
  - Interactive browser
  - Client credentials
  - Certificate-based
  - Access token authentication
- **Spark SQL native query support**:
  - Direct execution of Spark SQL
  - Parameterized queries
- **Comprehensive data type support**:
  - Supports Spark SQL data types, including complex types: `ARRAY`, `MAP`, `STRUCT`
- **Connection pooling**: Built-in pooling for performance
- **Session reuse**: Reduces Spark session startup latency
- **Async prefetch**: Background loading for large result sets
- **Auto-reconnect**: Automatic session recovery after connection failures

## Next steps

To download and learn more, see the official documentation:

- Microsoft ADO.NET Driver for Microsoft Fabric Data Engineering: https://learn.microsoft.com/fabric/data-engineering/spark-adonet-driver


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/microsoft-ado-net-driver-for-microsoft-fabric-data-engineering-preview/)

