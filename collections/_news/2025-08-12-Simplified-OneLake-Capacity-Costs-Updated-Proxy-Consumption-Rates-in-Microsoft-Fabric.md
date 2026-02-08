---
external_url: https://blog.fabric.microsoft.com/en-US/blog/onelake-costs-simplified-lowering-capacity-utilization-when-accessing-onelake/
title: 'Simplified OneLake Capacity Costs: Updated Proxy Consumption Rates in Microsoft Fabric'
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-08-12 10:00:00 +00:00
tags:
- ADLS
- Analytics
- Azure Data Lake Storage
- Billing
- Capacity Utilization
- Cost Optimization
- CU Seconds
- Data Engineering
- Data Lake
- Data Storage
- Lakehouse
- Microsoft Fabric
- OneLake
- Proxy Access
- Redirect Access
- Warehouse
- Azure
- ML
- News
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog details a new update that simplifies OneLake capacity utilization costs, authored by the Microsoft Fabric team. The change aligns proxy and redirect transaction rates, improving cost management and billing predictability for organizations using OneLake.<!--excerpt_end-->

# Simplified OneLake Capacity Costs: Updated Proxy Consumption Rates in Microsoft Fabric

**Author: Microsoft Fabric Blog**

Microsoft Fabric has announced a significant update to how OneLake’s capacity utilization is managed and billed. This change aims to streamline the management of Fabric capacity and provide organizations with more predictable cost structures for their data workloads.

## What Has Changed?

- The **consumption rate of OneLake transactions via proxy now matches the rate for transactions via redirect**.
- Previously, proxy access incurred higher consumption rates (measured in Capacity Units, or CUs) compared to redirect access.
- As of this update, both access paths now share the same, lower CU rate, making access path choice irrelevant from a cost perspective.

## Why the Change Matters

- Users no longer need to optimize or worry about whether workloads access OneLake through proxy or redirect—both are now equally cost-effective.
- This update simplifies how organizations calculate, allocate, and forecast their Microsoft Fabric capacity spending.
- OneLake is designed to be an open, extensible foundation for data estates, supporting both structured and unstructured data.

## Operational Rate Breakdown

The new unified consumption rates for common OneLake operations are as follows:

| Operation Type     | Redirect CU Rate | Previous Proxy CU Rate | New Proxy CU Rate |
|--------------------|------------------|----------------------|-------------------|
| Read (per 4 MB, per 10,000 ops) | 104 CU seconds  | 306 CU seconds         | **104 CU seconds**  |
| Write (per 4 MB, per 10,000 ops) | 1626 CU seconds | 2650 CU seconds        | **1626 CU seconds** |
| Iterative Read (per 10,000 ops)  | 1626 CU seconds | 4798 CU seconds        | **1626 CU seconds** |
| Other Operations (per 10,000 ops)| 104 CU seconds  | 306 CU seconds         | **104 CU seconds**  |

These rates are in effect immediately, impacting all read, write, and other data operations initiated via OneLake, regardless of the access method.

## Practical Impact

- Storage in OneLake remains billed on a pay-as-you-go model (per GB stored), similar to Azure Data Lake Storage (ADLS) or Amazon S3.
- Fabric items (lakehouse, warehouse, etc.) automatically store data in OneLake.
- Capacity Units (CUs) are deducted based on the number and type of operations performed.
- Users can now confidently connect to OneLake using Fabric, Azure Databricks, Snowflake, and other compatible tools, knowing that costs are consistent and predictable.

## Additional Resources

- [OneLake consumption – Microsoft Fabric | Microsoft Learn](https://learn.microsoft.com/fabric/onelake/onelake-consumption)
- [OneLake capacity consumption example – Microsoft Fabric | Microsoft Learn](https://learn.microsoft.com/fabric/onelake/onelake-capacity-consumption)
- [OneLake shortcuts and cross-capacity data access](https://blog.fabric.microsoft.com/blog/use-onelake-shortcuts-to-access-data-across-capacities-even-when-the-producing-capacity-is-paused?ft=Elizabeth%20Oldag:author)

## Conclusion

This announcement reinforces Microsoft Fabric’s commitment to openness and simplicity, making OneLake a central, reliable platform for managing enterprise data at scale. Organizations benefit from clearer billing and easier management of data capacity, helping them focus on building robust analytics solutions.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/onelake-costs-simplified-lowering-capacity-utilization-when-accessing-onelake/)
