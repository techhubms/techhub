---
external_url: https://blog.fabric.microsoft.com/en-US/blog/concurrency-control-and-conflict-resolution-in-microsoft-fabric-data-warehouse/
title: Resolving Write Conflicts in Microsoft Fabric Data Warehouse
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-10-08 07:00:00 +00:00
tags:
- ACID Transactions
- Atomic Operations
- Compaction Preemption
- Concurrency Control
- Conflict Resolution
- Data Warehouse
- Error Handling
- ETL Pipelines
- Microsoft Fabric
- Snapshot Isolation
- T SQL
- Table Level Locking
- Transaction Management
- Write Conflicts
- Azure
- ML
- News
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog addresses how write-write conflicts occur in Fabric Data Warehouse and shares best practices and new system features to help data professionals manage concurrency and improve workflow reliability.<!--excerpt_end-->

# Resolving Write Conflicts in Microsoft Fabric Data Warehouse

Fabric Data Warehouse (DW) supports ACID-compliant transactions using standard T-SQL commands such as `BEGIN TRANSACTION`, `COMMIT`, and `ROLLBACK`. It employs Snapshot Isolation (SI) as its exclusive concurrency control model, ensuring:

- **Atomic Operations:** All actions within a transaction succeed or fail as a unit.
- **Consistent Snapshots:** Transactions operate on a consistent view of data as it existed at their start, mirroring SQL Server’s SI behavior.
- **Optimistic Concurrency:** Write-write conflicts are detected at commit time, not during execution, reducing lock contention but requiring rollback and retry logic when conflicts occur.

## Understanding Write-Write Conflicts

A write-write conflict (or update conflict) arises when multiple concurrent transactions attempt to change the same table with operations like `UPDATE`, `DELETE`, `MERGE`, or `TRUNCATE`. Under SI, only the first transaction to commit will succeed; others will be rolled back with a conflict error.

**Common error codes include:**

- Error 24556: Snapshot isolation transaction aborted due to update conflict.
- Error 24706: Snapshot isolation transaction aborted due to update conflict on row modified or deleted by another transaction.

When you see these errors, the solution is generally to retry the operation.

### Which Operations Can Cause Write-Write Conflicts?

- UPDATE
- DELETE
- MERGE (affects same table, even for different rows)
- TRUNCATE

MERGE operations are subject to conflict detection even if only appending new rows. If any concurrent DML transaction targets the same table, conflicts can occur.

## Causes of Write-Write Conflicts

There are two primary sources:

1. **User-Induced Conflicts**:
   - Multiple users or processes modify the same table at the same time, often in ETL jobs or batch updates.
2. **System-Induced Conflicts (Compaction)**:
   - Fabric DW may perform background compaction tasks to optimize data storage. These may compete with user transactions, causing conflicts if both attempt to modify the same table concurrently.

**Example:**
If a user update coincides with background compaction on the same table and compaction commits first, the user’s transaction will fail with a write-write conflict error.

## Best Practices to Avoid Write-Write Conflicts

1. **Avoid Concurrent DML Operations:** Try to prevent multiple operations (`UPDATE`, `DELETE`, `MERGE`) from targeting the same table at the same time.
2. **Implement Retry Logic:** Since SI detects conflicts at commit, retrying failed DML operations is recommended.
   - Implement retries in stored procedures, ETL pipelines, or integration tools such as Qlik Replicate.
   - Especially effective for single-statement transactions.

## Fabric Data Warehouse In-Product Capability: Compaction Preemption

To improve reliability, Microsoft introduced **Compaction Preemption** in October 2025. This feature aims to prevent conflicts between user transactions and background compaction by:

- Allowing user transactions to take a shared lock on a sub-resource.
- Requiring compaction to check for this lock both before starting and before commit.
- Compaction aborts if conflict is possible, thereby never blocking user operations.

Limitations: This feature only avoids conflicts between compaction and user operations. User-initiated write-write conflicts remain possible.

## Future Directions

Microsoft continues to improve concurrency by researching finer conflict detection, such as file-level or row-level granularity, beyond the current table-level approach.

---

## Additional Resources

- [Understanding Locking and DDL Blocking in Microsoft Fabric Data Warehouse](https://aka.ms/LockingandDDLBlockingBehaviorinMicrosoftFabricDW)
- [Data Compaction documentation](https://learn.microsoft.com/fabric/data-warehouse/guidelines-warehouse-performance#data-compaction)

## Summary

Understanding write-write conflicts and their resolution in Microsoft Fabric Data Warehouse enables developers and data engineers to design more robust and high-throughput pipelines. Adopting retry logic and leveraging new features like Compaction Preemption are key strategies to improve reliability in concurrent environments.

---

**Author:** Microsoft Fabric Blog

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/concurrency-control-and-conflict-resolution-in-microsoft-fabric-data-warehouse/)
