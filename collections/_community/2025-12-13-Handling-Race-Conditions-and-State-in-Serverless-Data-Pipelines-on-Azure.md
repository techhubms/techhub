---
external_url: https://techcommunity.microsoft.com/t5/azure-architecture/design-pattern-handling-race-conditions-and-state-in-serverless/m-p/4477664#M820
title: Handling Race Conditions and State in Serverless Data Pipelines on Azure
author: Chameseddine
feed_name: Microsoft Tech Community
date: 2025-12-13 12:22:51 +00:00
tags:
- Azure Data Factory
- Azure Durable Functions
- Azure Functions
- Cloud Architecture
- Data Engineering
- Distributed Systems
- Durable Entities
- ETL
- Fan Out/Fan in Pattern
- Parquet
- Race Condition
- Sequential ID Generation
- Serverless
- State Management
- Table Storage
- Workflow Design
section_names:
- azure
- coding
- ml
---
Chameseddine discusses hands-on solutions for managing state and avoiding race conditions in Azure serverless data pipelines, sharing lessons learned and design patterns from a real-world ETL challenge.<!--excerpt_end-->

# Handling Race Conditions and State in Serverless Data Pipelines on Azure

**Author:** Chameseddine

Recently, I faced a data engineering challenge involving nearly 2 million Parquet records that needed to be ingested, transformed, and separated into distinct entities—using only Azure Functions, Azure Data Factory (ADF), and Azure Storage.

## The Core Problem

- **Requirement:** Generate globally unique, sequential IDs for certain columns, under a 2-hour processing window.
- **Constraint:** Need for parallel processing to meet SLAs, but parallelism risks breaking sequential ID generation (race conditions on counters).

## Architecture Patterns Tested

1. **Sequential Processing with Azure Data Factory (ADF)**
   - Pros: Simple and safe for state consistency
   - Cons: Failed the 2-hour limit due to processing time

2. **Parallel Processing with External Locking / e-Tags on Table Storage**
   - Pros: Can enable parallelism
   - Cons: Complex implementation and persistent issues with insert consistency/race conditions

3. **Fan-Out/Fan-In Pattern with Azure Durable Functions & Durable Entities**
   - Pros: Durable Entities act as stateful actors—track and update unique counters in memory across concurrent executions
   - Transformation workloads run in parallel, while ID state remains consistent
   - Result: Solved race conditions without compromising performance

## Why Durable Entities?

Durable Entities in Azure Functions maintain in-memory state, letting multiple parallel transformations update unique, sequential IDs safely and efficiently. This approach removed the need for complicated locking logic or slower sequential execution.

## Lessons & Trade-offs

- **Performance:** Durable Entities support parallelization without race conditions
- **Simplicity:** Avoids external locking mechanics
- **Scalability:** Scales with workload, up to the throughput limits of Durable Functions
- For external database sequence generation, you'd lose some of the benefits of serverless architecture and increase operational complexity

Read the full breakdown and implementation details [here](https://medium.com/@yahiachames/data-ingestion-pipeline-a-data-engineers-dilemma-and-azure-solutions-7c4b36f11351).

## Community Question

Has anyone else used Durable Entities or a similar serverless actor pattern for ETL workloads? Do you prefer external sequences for ID generation, or have you solved race conditions a different way when forced to stay inside the Azure serverless toolset?

---

*Thanks for reading! — Chameseddine*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture/design-pattern-handling-race-conditions-and-state-in-serverless/m-p/4477664#M820)
