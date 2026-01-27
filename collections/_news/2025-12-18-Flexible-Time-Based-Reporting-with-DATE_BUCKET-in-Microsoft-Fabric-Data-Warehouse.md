---
external_url: https://blog.fabric.microsoft.com/en-US/blog/date_bucket-function-in-fabric-data-warehouse/
title: Flexible Time-Based Reporting with DATE_BUCKET() in Microsoft Fabric Data Warehouse
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-12-18 09:00:00 +00:00
tags:
- Custom Aggregation
- Data Analytics
- Data Grouping
- Data Warehouse
- DATE BUCKET
- Date Functions
- Interval Bucketing
- Microsoft Fabric
- Reporting
- Sales Order Analysis
- SQL Functions
- T SQL
- Time Based Reporting
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog explains how the new DATE_BUCKET() function in Fabric Data Warehouse empowers data professionals to efficiently group and report on date-based data using custom intervals.<!--excerpt_end-->

# Flexible Time-Based Reporting with DATE_BUCKET() in Microsoft Fabric Data Warehouse

The Microsoft Fabric Data Warehouse offers a powerful, expressive SQL language to create flexible analytics solutions. A recent enhancement is the new `DATE_BUCKET()` function, now generally available, which revolutionizes how you approach time-based reporting.

## Why Is Time-Based Reporting Important?

In analytics, grouping data by date—such as by year, quarter, or month—is one of the most common tasks. Historically, T-SQL provides functions like `DATEPART()`, `YEAR()`, `MONTH()`, and `WEEK()` for extracting parts of a date. For example:

```sql
SELECT DATEPART(QUARTER, OrderDate) AS [Quarter], COUNT(*) AS [Number of orders]
FROM SalesOrders
GROUP BY DATEPART(QUARTER, OrderDate)
ORDER BY DATEPART(QUARTER, OrderDate)
```

However, when you need to aggregate data by custom intervals (like every 2 months, every 3 weeks, or every 5 minutes), traditional functions fall short. Building these groupings used to require complex date arithmetic or custom logic.

## Enter: The DATE_BUCKET() Function

The new `DATE_BUCKET()` function makes it easy to group data by any time interval, returning the first date of each 'bucket'. The syntax is:

```sql
DATE_BUCKET(unit, length, datetime)
```

- **unit**: The time unit (e.g., WEEK, MONTH, MINUTE)
- **length**: The number of units per bucket
- **datetime**: The date or datetime field to bucket

For example, to group sales orders into 2-month periods:

```sql
SELECT DATE_BUCKET(MONTH, 2, OrderDate) AS PeriodStart, COUNT(*) AS NumberOfOrders
FROM SalesOrders
GROUP BY DATE_BUCKET(MONTH, 2, OrderDate)
ORDER BY PeriodStart
```

This query will produce a table where each row represents a 2-month interval and the associated order count.

To group by 3-week intervals, simply change the parameters:

```sql
SELECT DATE_BUCKET(WEEK, 3, OrderDate) AS PeriodStart, COUNT(*) AS NumberOfOrders
FROM SalesOrders
GROUP BY DATE_BUCKET(WEEK, 3, OrderDate)
ORDER BY PeriodStart
```

## Visual Examples

- **2-month intervals:**

![A T‑SQL query that groups number of sales orders into consecutive 2‑month intervals. Each row shows the interval start date and the count of orders.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/12/image-21-1024x343.png)

- **3-week intervals:**

![A T‑SQL query that groups sales orders into consecutive 3‑week intervals. Each row shows the interval start date and the count of orders.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/12/image-22-1024x440.png)

Using `DATE_BUCKET()`, you can quickly adapt your queries as reporting requirements change—just adjust the bucket size.

## Key Benefits

- **Customizable:** Any interval—days, weeks, months, minutes, etc.
- **Simplifies Queries:** Reduces complexity for groupings
- **Versatile:** Handles a wide variety of reporting scenarios

## Conclusion

`DATE_BUCKET()` is a valuable addition for anyone creating reports or analytics in Microsoft Fabric Data Warehouse. By letting you bucket data over custom periods, it empowers you to spot new trends and build more flexible dashboards.

For further details, refer to the [Fabric Data Warehouse documentation](https://learn.microsoft.com/sql/t-sql/functions/date-bucket-transact-sql?view=fabric).

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/date_bucket-function-in-fabric-data-warehouse/)
