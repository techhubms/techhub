---
external_url: https://blog.fabric.microsoft.com/en-US/blog/locking-and-ddl-blocking-behavior-in-microsoft-fabric-data-warehouse-what-you-need-to-know/
title: Understanding Locking and DDL Blocking in Microsoft Fabric Data Warehouse
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-10-08 07:03:26 +00:00
tags:
- ACID Transactions
- Concurrency
- Data Engineering
- Data Warehouse
- Database Performance
- DDL Blocking
- DML Operations
- Locking
- Microsoft Fabric
- Schema Modification Lock
- Snapshot Isolation
- Sys.dm Tran Locks
- Sys.tables
- T SQL
- Workload Management
- Azure
- ML
- News
- Machine Learning
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog discusses locking behavior and DDL blocking in Microsoft Fabric Data Warehouse, offering practical advice for managing concurrency and minimizing blocking issues. Essential reading for data platform professionals.<!--excerpt_end-->

# Understanding Locking and DDL Blocking in Microsoft Fabric Data Warehouse

As Microsoft Fabric Data Warehouse adoption grows, it's critical for developers, architects, and data engineers to understand its concurrency and locking mechanisms. This article explores Fabric DW’s approach to locking, DDL blocking, their impact, and strategies to minimize issues.

## Locking Behavior in Fabric Data Warehouse

Microsoft Fabric DW implements ACID-compliant transactions using standard T-SQL commands—`BEGIN TRANSACTION`, `COMMIT`, and `ROLLBACK`—and enforces snapshot isolation. Locks are used to control concurrent access, especially during schema changes (DDL operations).

### Table-Level Lock Modes by Operation

| Statement Type        | Lock Taken            |
|----------------------|----------------------|
| SELECT               | Schema-Stability (Sch-S)      |
| INSERT/DELETE/UPDATE/MERGE/COPY INTO | Intent Exclusive (IX) |
| CREATE/ALTER/DROP/TRUNCATE TABLE, CTAS, CREATE TABLE AS CLONE | Schema-Modification (Sch-M) |

Fabric DW applies table-level locking for every operation, regardless of how many rows are accessed.

#### Trade-offs

- **Pros:** Predictable locking model, easy to reason about
- **Cons:** Blocking can occur in high-concurrency DDL scenarios

## DDL Blocking Behavior

Long-running transactions containing DDL operations can block concurrent sessions.

- DDLs acquire Sch-M locks on tables for the transaction duration
- These locks prevent concurrent DML (SELECT, INSERT, UPDATE, DELETE) on the same table
- An exclusive (X) lock is also held on system tables (sys.tables, sys.objects), blocking metadata queries
- Explicit transactions (`BEGIN TRAN`) including DDL can extend the lock duration and increase blocking risk

**Impact:**

- Schema evolution or automated schema migrations during peak activity can lead to workload disruption and slow user experience

## Best Practices

- Avoid long transactions that hold locks for extended periods
- Perform DDLs during maintenance or low-activity periods
- Avoid wrapping DDLs in explicit user transactions
- Monitor lock conflicts via `sys.dm_tran_locks`

## Upcoming Improvement: READPAST Table Hint

Microsoft Fabric DW is introducing support for the `READPAST` hint on metadata queries (e.g., querying `sys.tables`). This will let metadata queries skip rows currently locked with an X lock, improving system responsiveness and observability during DDL operations.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/locking-and-ddl-blocking-behavior-in-microsoft-fabric-data-warehouse-what-you-need-to-know/)
