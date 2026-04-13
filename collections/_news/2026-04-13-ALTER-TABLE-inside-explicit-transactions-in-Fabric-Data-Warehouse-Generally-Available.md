---
feed_name: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/alter-table-inside-explicit-transactions-in-fabric-data-warehouse-generally-available/
primary_section: ml
tags:
- ALTER TABLE
- BEGIN TRAN
- CI/CD Pipelines
- COMMIT
- Constraints
- Database Migrations
- DDL Operations
- Distributed Temporary Tables
- Explicit Transactions
- Fabric Data Warehouse
- Foreign Key
- Microsoft Fabric
- ML
- News
- NOT ENFORCED Constraints
- Primary Key
- Rollback
- Schema Evolution
- Snapshot Isolation
- T SQL
- Transactional DDL
- Unique Constraint
author: Microsoft Fabric Blog
date: 2026-04-13 11:00:00 +00:00
title: ALTER TABLE inside explicit transactions in Fabric Data Warehouse (Generally Available)
section_names:
- ml
---

Microsoft Fabric Blog announces that Fabric Data Warehouse now supports running certain ALTER TABLE operations inside explicit transactions, enabling atomic schema changes with automatic rollback for safer deployments.<!--excerpt_end-->

## Overview

Schema evolution is a normal part of running analytics platforms: adding columns, dropping unused fields, and changing constraints—often through controlled deployment pipelines.

Fabric Data Warehouse previously supported transactional execution for table-focused DDL such as:

- `CREATE TABLE`
- `DROP TABLE`
- `TRUNCATE TABLE`
- CTAS (Create Table As Select)
- `sp_rename`

With this release, `ALTER TABLE` joins that set, enabling atomic schema evolution.

## A new approach to schema changes

Previously, the supported `ALTER TABLE` statements in Fabric Data Warehouse ran outside explicit user transactions.

If you attempted to include `ALTER TABLE` inside a `BEGIN TRAN` / `COMMIT` block, it failed with:

> Transaction failed because this DDL statement is not allowed inside a snapshot isolation transaction.

This created common enterprise problems:

- **No atomicity for schema evolution**: multi-step migrations could fail mid-way, leaving partial schema updates.
- **Brittle CI/CD pipelines**: combining `ALTER TABLE` with other DDL/DML in one atomic deployment wasn’t possible.
- **Manual recovery**: failed deployments required diagnosing and fixing schema drift.

Reference: https://learn.microsoft.com/en-us/fabric/data-warehouse/tsql-surface-area

## What’s new: `ALTER TABLE` inside `BEGIN TRAN`

Supported `ALTER TABLE` operations can now execute inside an explicit user-defined transaction in Fabric Data Warehouse.

You can now run patterns like:

```sql
BEGIN TRAN;

ALTER TABLE <table_name> ADD <column_name> <type>;

ALTER TABLE <table_name> DROP COLUMN <column_name>;

COMMIT;
```

Fabric processes these schema changes as a single atomic operation.

- If any statement in the transaction fails, all schema changes are automatically rolled back.
- The warehouse is left in a consistent, predictable state.

Learn more: https://learn.microsoft.com/sql/t-sql/statements/alter-table-transact-sql?view=fabric#syntax-for-warehouse-in-fabric

## Why this matters

### Atomic, all-or-nothing schema changes

Multiple `ALTER TABLE` statements can be grouped and committed together, avoiding partial updates.

### Stronger correctness guarantees

Schema evolution now follows the same transactional semantics as data changes, aligned with Fabric DW’s snapshot isolation and rollback model.

### Safer enterprise deployments

CI/CD pipelines become simpler and more reliable because failures automatically roll back without manual intervention.

## Supported scenarios

Fabric Data Warehouse supports executing the following inside an explicit transaction:

- `ALTER TABLE` add nullable columns
- `ALTER TABLE` drop columns
- `ALTER TABLE` add or drop **NOT ENFORCED** constraints:
  - `PRIMARY KEY`
  - `UNIQUE`
  - `FOREIGN KEY`
- Execute multiple `ALTER TABLE` statements atomically
- Alter distributed temporary tables

If any operation fails, the entire transaction is rolled back automatically.

### Current limitations

Some operations remain unsupported and will still produce clear error messages, including:

- Adding non-nullable columns
- `ALTER COLUMN`
- Alter on non-distributed temp tables

For the current list of supported features and limitations:

- https://learn.microsoft.com/fabric/data-warehouse/transactions#limitations

## Summary

Transactional DDL is a core requirement for enterprise data platforms. By allowing `ALTER TABLE` inside explicit user transactions, Fabric Data Warehouse enables safer, more predictable schema evolution workflows for mission-critical analytics on Microsoft Fabric.

[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/alter-table-inside-explicit-transactions-in-fabric-data-warehouse-generally-available/)

