---
section_names:
- ml
primary_section: ml
title: Use ANY_VALUE() for simpler grouping of results in Fabric Data Warehouse (Generally Available)
date: 2026-04-06 14:00:00 +00:00
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/use-any_value-for-simpler-grouping-of-results-in-fabric-data-warehouse-generally-available/
tags:
- Aggregate Functions
- Analytic Functions
- ANY VALUE
- Dimensional Attributes
- Fabric Data Warehouse
- Functionally Dependent Columns
- GROUP BY
- Microsoft Fabric
- ML
- News
- Performance Tuning
- Query Design
- Readability
- SQL Aggregation
- T SQL
- Transact SQL
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces general availability of ANY_VALUE() in Fabric Data Warehouse, explaining how this T-SQL aggregate helps keep GROUP BY clauses focused on the true grouping key while still returning functionally dependent descriptive columns like city, state, and country.<!--excerpt_end-->

# Use ANY_VALUE() for simpler grouping of results in Fabric Data Warehouse (Generally Available)

Microsoft Fabric Data Warehouse now supports the **ANY_VALUE()** aggregate. It’s useful when you want to group by a key but still return descriptive columns that are functionally the same for every row in the group.

## What is ANY_VALUE()?

**ANY_VALUE()** is an aggregate or analytic function that returns an arbitrary value from each group or partition.

It’s designed for scenarios where you:

- Aggregate values by a key (for example, **GeographyID**)
- Want to include descriptive attributes (for example, **City**, **State**, **Country**) that don’t change within each group

When a column is truly constant within the group (functionally dependent on the grouping key), **ANY_VALUE()** makes your intent explicit: group by the key and bring along the descriptive columns.

## Example scenario

Suppose you want to calculate total revenue from taxi trips for each geographic area by grouping on **GeographyID**. You also want to include **City**, **State**, and **Country** in the output.

Because those descriptive columns are functionally dependent on **GeographyID** (they don’t vary within the group), you can project them via **ANY_VALUE()** without adding them to the **GROUP BY** clause or using less clear workarounds.

![SQL query showing total revenue by geography. Query returns the values for city, state, and country that are same within the groups, so any value can be returned to the client.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/sql-query-showing-total-revenue-by-geography-quer.png)

*Figure: Using ANY_VALUE() to project descriptive columns while aggregating trips by GeographyID.*

## When to use it (and when not to)

Use **ANY_VALUE()** when the chosen column is functionally dependent on the grouping key (for example, *City* is constant for a given *GeographyID*).

If values can legitimately vary within the group, choose an aggregate that matches the business rule instead, such as:

- **MIN()**
- **MAX()**
- A windowing approach

## Benefits

- **Cleaner GROUP BY:** Group only by what defines the aggregation (for example, *GeographyID*) instead of repeating descriptive columns. Unnecessary columns in **GROUP BY** can also cause performance issues.
- **Clearer intent:** Descriptive columns are “along for the ride,” not part of the grouping logic.
- **Avoid unnecessary workarounds:** You don’t need to use **MIN()**/**MAX()** just to pick a representative value when the value is already constant within the group. It also reduces confusion for future readers who might otherwise misinterpret why a query is taking a “max city” or similar.

## References

- ANY_VALUE documentation: [ANY_VALUE (Transact-SQL)](https://learn.microsoft.com/sql/t-sql/functions/any-value-transact-sql)

## Conclusion

**ANY_VALUE()** helps keep aggregation queries focused on what defines the group while still returning the descriptive fields a report or dashboard needs. In Fabric Data Warehouse, it can simplify query text, reduce GROUP BY noise, and make queries easier to maintain.


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/use-any_value-for-simpler-grouping-of-results-in-fabric-data-warehouse-generally-available/)

