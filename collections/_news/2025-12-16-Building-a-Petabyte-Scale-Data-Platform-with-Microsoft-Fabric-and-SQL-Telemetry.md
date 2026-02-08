---
external_url: https://blog.fabric.microsoft.com/en-US/blog/sql-telemetry-intelligence-how-we-built-a-petabyte-scale-data-platform-with-fabric/
title: Building a Petabyte-Scale Data Platform with Microsoft Fabric and SQL Telemetry
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-12-16 10:00:00 +00:00
tags:
- AKS
- Anomaly Detection
- Autoscale Billing
- Azure Data Lake
- CI/CD
- Data Engineering
- Data Platform
- Data Quality
- Delta Lake
- GitOps
- KEDA
- Kimball Dimensional Modeling
- Lakehouse Architecture
- Microsoft Fabric
- OpenTelemetry
- Petabyte Scale
- Power BI
- SLA Monitoring
- Spark Streaming
- VS Code Devcontainer
- Azure
- DevOps
- ML
- News
- .NET
section_names:
- azure
- dotnet
- devops
- ml
primary_section: ml
---
In this deep technical blog, the Microsoft Fabric Blog team explains how they engineered a robust, Petabyte-scale data platform on Microsoft Fabric, focusing on real-time telemetry and modern data engineering practices.<!--excerpt_end-->

# SQL Telemetry & Intelligence – How we built a Petabyte-scale Data Platform with Fabric

## Overview

Over the past three years, the SQL Telemetry & Intelligence (T&I) Engineering team has developed a massive data lake (over 10 Petabytes) running on Microsoft Fabric. This platform ingests, processes, and analyzes real-time data from globally distributed SQL Server engines and control/data plane services, forming the analytics backbone for telemetry at scale.

## Architecture and Design

- **Lakehouse Architecture:** Employs a Medallion (bronze-silver-gold) layer pattern influenced by the Lambda architecture, with real-time streaming and optimized data compression (columnar Parquet).
- **Data Ingestion:** Utilizes OpenTelemetry for instrumenting services, Event Hub for control plane events, and Azure Data Explorer for time-series storage. Real-time data flows are mirrored into Delta Lake storage via high-throughput C#/Rust services running on AKS with KEDA.
- **ETL & Streaming:** Spark Streaming is used for schema enforcement, combining micro-batches, supporting low-latency processing and horizontal scalability. Transformations are managed via versioned samples and stateful/isolated checkpoints.
- **Datamodeling:** Adopts Kimball STAR schemas with SCD2 dimensions and transaction grain, focusing on idempotency and data integrity (primary/foreign key enforcement, broadcast joins, and periodic snapshot tables).
- **Semantic Layer:** Implements a DirectLake semantic model, leveraging Tabular Editor, Power BI Desktop, and DAX Studio for advanced analytics.

## Operational Tooling & DevOps Practices

- **Development Environment:** Teams use VSCode Devcontainer setups for local development with consistent dependencies (pinned Spark/JDK versions, extensions, etc.), streamlining build/test cycles. All code is tested locally before deploying to Fabric Spark via the VSCode Extension.
- **CI/CD Automation:** Full-stack environments are provisioned per engineer using GitOps manifests, wrapping the Fabric CLI, Fabric-cicd, and APIs for automated deployment. Version control extends even to workspace icons.
- **Legacy & Integration:** For regions lacking Fabric, Synapse Workspaces are maintained and an internal workspace-deployment app automates cross-platform environments.

## Data Quality and Analytics

- **Data Quality:** The team employs Deequ for robust data validation, anomaly detection, and data quality rules using DQDL. Integrity enforcement utilizes clustering and optimized query patterns (e.g., WHERE EXISTS over JOINs).
- **SLA Monitoring:** SLAs are defined via YAML, version-controlled, and integrated with Spark DataFrames for breach detection, automated alerting via Fabric Activator, and visualization in Power BI.
- **Testing:** Testing strategies involve parallelized execution and Spark configuration tuning to reduce runtimes by up to 67%.

## Scaling & Challenges

- **Scalability:** Autoscale billing in Spark allows handling variable workloads (8000 cores at peak). Backfilling and incremental view maintenance strategies provide resilience, minimize reprocessing, and protect checkpointed streaming jobs.
- **Incremental Processing:** Explores advanced optimization using SQL AST rewriting (with references to Linkedin Coral and research on DBSP) and features from Fabric’s Materialized Lake Views for cost-efficient refreshes.

## Future Directions

- Expansion into dbt-on-Fabric to enable self-serve analytics for developers.
- Broader use of AI/ML for anomaly detection and advanced KPI monitoring.

---

**Key Technical Highlights:**

- Real-world application of Spark Streaming and Kimball modeling at hyperscale.
- Heavy automation via DevOps pipelines, GitOps, and tailored deployment frameworks.
- Emphasis on reproducibility, regression-proofing, and efficient local/cloud development.
- Multi-layered quality enforcement (data, SLA, operational metrics).
- Deep integration of Microsoft and open-source tools (Fabric, AKS, Spark, Delta Lake, Deequ, Power BI).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/sql-telemetry-intelligence-how-we-built-a-petabyte-scale-data-platform-with-fabric/)
