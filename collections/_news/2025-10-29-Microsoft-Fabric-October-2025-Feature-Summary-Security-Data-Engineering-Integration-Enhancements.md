---
external_url: https://blog.fabric.microsoft.com/en-US/blog/fabric-october-2025feature-summary/
title: 'Microsoft Fabric October 2025 Feature Summary: Security, Data Engineering, Integration Enhancements'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-10-29 09:00:00 +00:00
tags:
- Adaptive Target File Size
- Concurrency
- Data Agent
- Data Engineering
- Data Factory
- Data Warehouse
- Delta Lake
- Eventstream
- Export Query Results
- Graph Data Management
- Lakehouse
- Microsoft Fabric
- OneLake
- Outbound Access Protection
- Power BI
- Python
- Real Time Intelligence
- Spark
- SQL
- Workspace Level Private Link
- Azure
- Machine Learning
- Security
- News
section_names:
- azure
- ml
- security
primary_section: ml
---
Microsoft Fabric Blog provides a detailed summary of October 2025 platform updates, focusing on enhanced security, smarter data engineering, new integrations, and workflow improvements for data professionals.<!--excerpt_end-->

# Fabric October 2025 Feature Summary

This October 2025 update for Microsoft Fabric introduces a comprehensive range of improvements across the platform, empowering data engineers, analysts, and IT professionals with new capabilities for security, efficiency, and collaboration.

## Events & Announcements

- **Fabric Data Days:** Two months of community-driven learning and networking for data and AI professionals, beginning November 4th.
- **FabCon 2026:** Annual community event focused on Power BI, Microsoft Fabric SQL, Real-Time Intelligence, and more.

## Platform Enhancements

### Enhanced Security

- **Outbound Access Protection for Spark (GA):** Tightens governance of outbound Spark connections, reducing risk of data exfiltration and expanding granular controls for organizations.
- **Workspace-Level Private Link (GA):** Enables secure, private access to Fabric workspaces from internal networks, eliminating public internet exposure.

### OneLake Security & Collaboration

- **ReadWrite Access in OneLake:** Data owners can assign precise read/write permissions, supporting collaborative workflows without sacrificing governance. Access is managed using Spark notebooks, OneLake File Explorer, or APIs and enforces least privilege.

### Data Engineering Innovation

- **Adaptive Target File Size:** File size targets in Delta Tables adjust dynamically as table size changes, optimizing for performance, parallelism, and data skipping.
- **Fast Optimize & Auto Compaction:** Automated compaction optimizes storage efficiency, reduces write amplification, and minimizes performance dips between maintenance cycles.
- **Spark Connector for SQL Databases (Preview):** Native Spark-to-SQL connector with PySpark and Scala support, seamless authentication, and built-in security enforcement.
- **Spark Executor Rolling Logs:** Simplifies log management for long or large Spark jobs by segmenting logs hourly for easier troubleshooting.

### Improved Developer Experience

- **Keyboard Shortcuts & Focus Mode:** Enhanced UI multitasking and distraction-free editing for developers, similar to professional IDEs.
- **Table Deep Links in Lakehouse Explorer:** Direct URLs to specific tables facilitate fast preview and collaboration.
- **Fabric Connection with Notebook:** Streamlines external data access and credential management in connected notebooks.

### Data Agent and Science Updates

- **Data Agent Integration in Lakehouse:** Integrate and manage Data Agents directly within Lakehouse, streamlining analytics workflows and enabling AI-driven insights.
- **Markdown Editor for Data Agent Creators:** Compose agent instructions in Markdown to improve clarity and LLM understanding.
- **New Authoring Experience:** Split Data/Setup tabs, integration with CI/CD, and schema-exploration enhance agent creation and management.

### Data Warehouse Advancements

- **Ingesting JSONL Files:** OPENROWSET in T-SQL supports reading and transforming JSON Lines data at scale.
- **Data Source Option for OPENROWSET:** Enables the use of relative file paths and simplifies access to Lakehouse files.
- **Improved Concurrency – Compaction Preemption:** System awareness of active workloads helps avoid write-write conflicts and query failures during compaction.

### Real-time Intelligence

- **Native Graph Engine:** Horizontally scalable graph data management and analytics inspired by LinkedIn’s system, foundational for agent-driven intelligence.
- **Eventstream:** New CDC support for MongoDB, schema validation for EventHub sources, and pause/resume controls for derived streams improve real-time pipeline flexibility and governance.

### Data Factory & Power BI Enhancements

- **Copy Job:** Now supports ORC, Excel, Avro, and XML formats as well as advanced CSV parsing features for broader interoperability.
- **Variable Libraries in Dataflow Gen2:** Gateway support and improved editor integration streamline solution authoring.
- **Export Query Results (Preview):** Power BI Desktop users can export data directly to Fabric destinations, supporting seamless cross-product data movement and workflow continuity.

## References & Further Reading

- [Fabric Data Days](https://aka.ms/fabricdatadays)
- [FabCon Registration](https://aka.ms/fabcon)
- [Fabric Documentation and Blogs](https://learn.microsoft.com/fabric/)

This release continues Microsoft Fabric’s vision for a unified, intelligent, and secure analytics platform.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/fabric-october-2025feature-summary/)
