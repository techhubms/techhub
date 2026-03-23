---
tags:
- Adventure Works
- AI
- Cross Selling Recommendations
- Data Agents
- Deterministic Traversal
- Enterprise AI
- Explainable AI
- Fabric Data Agent
- Fabric Graph
- GQL
- Graph Modeling
- Graph Query Language
- Graph RAG
- Knowledge Graph
- Microsoft Fabric
- ML
- Neurosymbolic AI
- News
- NL2GQL
- OneLake
- Recommendation Systems
- Retrieval Augmented Generation
- Symbolic AI
date: 2026-03-23 08:30:00 +00:00
primary_section: ai
external_url: https://blog.fabric.microsoft.com/en-US/blog/graph-powered-ai-reasoning-preview/
title: Graph-powered AI reasoning (Preview)
author: Microsoft Fabric Blog
section_names:
- ai
- ml
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog introduces a preview of graph-powered AI reasoning in Microsoft Fabric, using Fabric Data Agent plus Fabric Graph to translate natural language into Graph Query Language (NL2GQL) and run deterministic graph traversals (graph RAG) so enterprise users can inspect and validate the reasoning path behind an answer.<!--excerpt_end-->

# Graph-powered AI reasoning (Preview)

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a broader set of Fabric and database announcements.*

## Why enterprises are pushing for inspectable reasoning

Enterprises are increasingly using large language models (LLMs) for decision support—not just text generation. As a result, the *path* to the answer matters as much as the answer itself.

Key need: AI systems that can **reason, justify, and be inspected** in complex enterprise information environments (organizational, transactional, and behavioral relationships).

## What’s being introduced

Microsoft is previewing a **graph-powered AI reasoning harness** via **Fabric Data Agent + graph integration**.

The idea:

- Translate natural language intent into **Graph Query Language** using **NL2GQL**.
- Perform **graph-based retrieval augmented generation (graph RAG)** using **deterministic graph traversals**.
- Return answers grounded in **nodes, edges, and traversal paths**, enabling users to inspect the “why.”

## What is neurosymbolic AI?

**Neurosymbolic AI** combines:

- **Neural models** (e.g., LLMs) for language understanding and generation
- **Symbolic representations** (entities, relationships, rules) for step-by-step reasoning over structured knowledge

Why it matters:

- LLMs are strong at summarization and generation.
- Enterprises often need **relationship-aware reasoning** (e.g., tracing linked records) and explanations that can be **validated and trusted**.

Symbolic AI supports scenarios like:

- Operations management
- Risk and fraud analysis
- Compliance and investigations

Without symbolic grounding, enterprise AI agents struggle to **explain**, **justify**, or **govern** decisions reliably.

## Symbolic AI reasoning via graphs in Microsoft Fabric

Graphs make relationships first-class, enabling explicit multi-hop traversal without flattening relationships into complex joins.

In **Microsoft Fabric**, you can add **graph models or querysets** as knowledge sources in **Data Agent**:

- Fabric Graph overview: https://learn.microsoft.com/fabric/graph/overview
- Data agent concept: https://learn.microsoft.com/fabric/data-science/concept-data-agent

### How it works (high level)

Under the hood, an **NL2GQL service**:

- Interprets the user’s intent
- Validates it against the graph model
- Generates a **GQL** statement
- Executes deterministic traversals

Then the data agent responds with grounded answers based on the execution results.

When setting up a data agent, you can also add:

- Supported data sources
- Agent instructions
- Data source instructions
- Sample queries

These can expand the knowledge base and constrain or shape agent behavior.

## Example: Graph-powered AI reasoning for cross-selling recommendations

### Scenario

Adventure Works (multinational outdoor sportswear/equipment retailer) wants to scale cross-selling recommendations (“you might also like”) based on similar purchase patterns across product categories/subcategories.

With graph-powered agents, they aim to map connections across:

- Salespeople
- Customers
- Products
- Categories

…without relying on brittle similarity pipelines.

### Adventure Works’ revenue operations data

The company records revenue operations with these tables/columns:

| Orders | Customers | Product Subcategories |
| --- | --- | --- |
| • SalesOrderID | • CustomerID | • SubCategoryID |
| • OrderDate | • FirstName | • CategoryID |
| • DueDate | • LastName | • SubCategoryName |
| • ShipDate |  |  |
| • EmployeeID |  |  |
| • CustomerID |  |  |
| • TotalDue |  |  |
| • ProductID |  |  |
| • OrderQty |  |  |
| • UnitPrice |  |  |
| • UnitPriceDiscount |  |  |
| • LineTotal |  |  |

| Salespeople | Products | Product Categories |
| --- | --- | --- |
| • EmployeeID | • ProductID | • CategoryID |
| • FirstName | • ProductNumber | • CategoryName |
| • LastName | • ProductName |  |
| • JobTitle | • ModelName |  |
| • OrganizationLevel | • StandardCost |  |
| • Country | • ListPrice |  |
|  | • SubCategoryID |  |

### The challenge with SQL

The post argues this becomes complex in SQL because it requires:

1. Building a per-order-line “shopping context” set (PurchaseCountry, CategoryID, SubCategoryID, ProductID), complicated because `Country` is in the `Salespeople` table.
2. Computing customer-to-customer similarity (often self-joins) based on overlap in countries and categories/subcategories.
3. Aggregating products bought by similar customers, excluding what the target customer already purchased.

At scale, this becomes **pairwise comparison** territory and can require precomputation/materialized views and extra derived tables (e.g., customer×country, customer×subcategory, co-purchase matrices).

### Graph-powered agent approach

Adventure Works adopts **Fabric Graph** (Microsoft-managed, horizontally scalable, native graph data system). The post claims this reduces the need for significant engineering effort to build and maintain custom similarity pipelines/derived tables.

A business analyst creates a graph model from operational data in **OneLake** using Fabric Graph no-code UI.

![Graph model for Adventure Works' sales data. The "country" node type has been created from the corresponding column in the "salespeople" table.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/graph-model-for-adventure-works-sales-data-the.png)

Then they connect the graph to Fabric Data Agent and ask:

> Which products should be recommended together because they are frequently bought by similar customers who make purchases in the same countries and buy from the same categories or subcategories?

A recommendations output appears.

![Cross-selling recommendations based on the sales data.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/cross-selling-recommendations-based-on-the-sales-d.png)

### What the NL2GQL service does

To generate the recommendations, NL2GQL:

- Finds customers who placed multiple orders.
- Traverses the graph from orders to products, subcategories, and categories.
- Constrains similarity by shared country and category context.
- Counts frequently co-purchased product pairs.
- Ranks the strongest co-purchase signals per country.

The workflow is reflected in an inspectable GQL query trace.

![GQL query trace allows for greater explainability.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/gql-query-trace-allows-for-greater-explainability-.png)

The point: the answer is grounded in explicit traversal paths rather than probabilistic text generation, and evolving the question doesn’t force unreadable/brittle SQL.

## Conclusion

As AI systems move from answering questions to facilitating actions, enterprises need reasoning they can inspect, govern, and trust. Graph-powered AI reasoning is positioned as a foundation for agents operating over real-world relationship complexity.

## Learn more

- Create a graph model in Fabric: https://learn.microsoft.com/fabric/graph/tutorial-introduction
- Create a data agent in Fabric: https://learn.microsoft.com/fabric/data-science/how-to-create-data-agent


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/graph-powered-ai-reasoning-preview/)

