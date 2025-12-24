---
layout: "post"
title: "Exposing Lakehouse Materialized Views via GraphQL APIs in Microsoft Fabric"
description: "This guide details how to quickly expose data from Microsoft Fabric Lakehouse using Materialized Lake Views and the API for GraphQL. Learn how to set up a Lakehouse, create high-performance materialized views with Spark SQL, and provide developers with fast, flexible API access using GraphQL—all managed within the Microsoft Fabric platform."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/exposing-lakehouse-materialized-views-to-applications-in-minutes-with-graphql-apis-in-microsoft-fabric/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-12-02 14:00:00 +00:00
permalink: "/news/2025-12-02-Exposing-Lakehouse-Materialized-Views-via-GraphQL-APIs-in-Microsoft-Fabric.html"
categories: ["Azure", "Coding", "ML"]
tags: ["Analytics", "API Development", "Azure", "Coding", "Data Engineering", "Data Integration", "Data Platform", "Fabric Runtime", "GraphQL API", "Lakehouse", "Materialized View", "Microsoft Fabric", "ML", "News", "Retail Data", "Schema Support", "Spark SQL"]
tags_normalized: ["analytics", "api development", "azure", "coding", "data engineering", "data integration", "data platform", "fabric runtime", "graphql api", "lakehouse", "materialized view", "microsoft fabric", "ml", "news", "retail data", "schema support", "spark sql"]
---

Microsoft Fabric Blog presents a step-by-step tutorial on how to expose Lakehouse Materialized Views to applications in minutes using GraphQL APIs, combining performance and developer flexibility.<!--excerpt_end-->

# Exposing Lakehouse Materialized Views via GraphQL APIs in Microsoft Fabric

In today’s data-driven world, efficiently surfacing data through modern APIs is essential. Microsoft Fabric provides a unified approach to this by combining Materialized Lake Views with its API for GraphQL, enabling high-performance, developer-friendly analytics solutions.

## Why Use Materialized Lake Views and GraphQL?

- **Materialized Lake Views**: Pre-compute and store complex analytics results for faster queries.
- **GraphQL APIs**: Allow developers to query only the precise data they require, resulting in flexible, efficient integrations.
- **Microsoft Fabric Integration**: Offers a streamlined experience, removing the need for external API gateways or complex connectors.

## Step-by-Step Guide

### 1. Create a Lakehouse with Schema Support

- Go to your Fabric workspace.
- Choose **+ New** and select **Lakehouse**.
- Name your Lakehouse (e.g., 'RetailAnalytics') and enable **Lakehouse schemas (Preview)** during creation.
- Use **Start with sample data** > **Retail Data Model from Wide World Importers**.
- The Lakehouse will be populated with retail-oriented tables (e.g., dimension tables and fact_sale).

> **Note:** Schema support is a preview feature and can't be disabled once enabled.

### 2. Create a Materialized Lake View

- Access **Manage materialized lake views** in your Lakehouse.
- Create a new notebook (using Fabric Runtime 1.3 or later).
- Use Spark SQL to define an aggregated sales metrics view. Example code:

```sql
CREATE MATERIALIZED LAKE VIEW IF NOT EXISTS dbo.SalesByStockItemAndCity COMMENT "Sales metrics aggregated by stock item and city" AS
SELECT si.StockItem, si.Brand, si.Color, c.City, c.StateProvince, c.Country, COUNT(DISTINCT fs.SaleKey) as TotalSales, SUM(fs.Quantity) as TotalQuantity, SUM(fs.TotalExcludingTax) as TotalRevenue, SUM(fs.Profit) as TotalProfit, AVG(fs.UnitPrice) as AverageUnitPrice
FROM dbo.fact_sale fs
INNER JOIN dbo.dimension_stock_item si ON fs.StockItemKey = si.StockItemKey
INNER JOIN dbo.dimension_city c ON fs.CityKey = c.CityKey
GROUP BY si.StockItem, si.Brand, si.Color, c.City, c.StateProvince, c.Country;
```

- Run the notebook cell and verify the view in the Lakehouse explorer.

### 3. Schedule and Monitor View Refreshes

- View relationships and set up scheduled (e.g., hourly or daily) refreshes.
- Fabric will intelligently skip unnecessary recomputations and provides lineage tracking for visibility.

### 4. Create and Configure a GraphQL API

- In the workspace, choose **+ New > API for GraphQL**.
- Name your API (e.g., 'RetailDataAPI').
- Select your Lakehouse and the desired materialized view as the data source.
- Fabric automatically generates the corresponding GraphQL schema.

### 5. Query Data with GraphQL

- Use the built-in API explorer to create and test queries.
- Example query to fetch top products in a region:

```graphql
query TopProductsByRegion {
  salesbystockitemandcities(
    filter: { Country: { eq: "United States" }, TotalRevenue: { gt: 50000 } }
    orderBy: { TotalProfit: DESC }
    first: 10
  ) {
    items {
      StockItem
      Brand
      Color
      City
      StateProvince
      TotalSales
      TotalQuantity
      TotalRevenue
      TotalProfit
      AverageUnitPrice
    }
  }
}
```

- You can customize the fields, filters, and sort patterns as needed without additional endpoint development.

## Benefits

- **Performance**: Pre-computation and efficient APIs deliver rapid analytics access.
- **Developer Experience**: Flexibility and self-service, all managed within Fabric.
- **Simplicity**: No external gateways or redundant infrastructures needed.

## Additional Resources

- [Microsoft Fabric GraphQL Documentation](https://learn.microsoft.com/fabric/data-engineering/get-started-api-graphql)
- [Materialized Views Guide](https://learn.microsoft.com/fabric/data-engineering/materialized-lake-views/get-started-with-materialized-lake-views)
- [Fabric Ideas Community Page](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas)

> *Ready to experiment with your own datasets? Explore more ways to combine the analytical power of Microsoft Fabric Lakehouse with the flexibility of GraphQL APIs.*

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/exposing-lakehouse-materialized-views-to-applications-in-minutes-with-graphql-apis-in-microsoft-fabric/)
