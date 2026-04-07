---
title: Cosmos DB Optimization
feed_name: John Savill's Technical Training
primary_section: azure
author: John Savill's Technical Training
external_url: https://www.youtube.com/watch?v=RbH5F_3w47E
date: 2026-04-06 11:02:11 +00:00
tags:
- Account Throughput Limit
- Autoscale
- Azure
- Azure Cloud
- Azure Cosmos DB
- Azure Pricing
- Capacity Planning
- Cosmos DB
- Cosmosdb
- Cost Optimization
- Data Model Design
- Document Size
- Free Tier
- High Cardinality
- Indexing
- Johnsavillstechnicaltraining
- Microsoft
- Microsoft Azure
- NoSQL
- Onboard to Azure
- Partition Key
- Partitions
- Provisioned Throughput
- Request Units
- Reserved Capacity
- RU/s
- RUs
- Serverless
- Videos
- Workload Patterns
section_names:
- azure
---

John Savill's Technical Training walks through practical ways to optimize Azure Cosmos DB cost and performance, focusing on throughput (RUs), service tiers, partitioning, and data model/design choices.<!--excerpt_end-->

## Cosmos DB Optimization

This session covers how to optimize Azure Cosmos DB deployments with a focus on cost/performance trade-offs. Topics include throughput models (Request Units), service options, and how data model and partitioning decisions directly affect RU consumption.

## Chapters covered (video outline)

- Introduction
- Service options and structure
- Request Units (RUs)
- Free SKU
- Provisioned throughput
  - Manual
  - Autoscale
- Configuration
- Account throughput limit
- Partitions
- What should I use?
- Optimizing design
  - Optimizing RU usage
  - Importance of the partition key
  - High cardinality
  - Document size
  - Global secondary index
  - Storage-heavy vs write-heavy patterns
- Summary

## Key optimization themes called out

### Throughput and pricing model choices

- Understanding and managing **Request Units (RUs)**
- Choosing between:
  - **Free SKU**
  - **Provisioned throughput**
    - **Manual** provisioning
    - **Autoscale** provisioning
- Reviewing pricing-related options such as **reservations**

### Partitioning and data model design

- How **partitions** impact scalability and performance
- Why the **partition key** is a critical design decision
- Using **high-cardinality** partition keys to avoid hot partitions
- Considering **document size** and its impact on throughput and storage
- Index considerations (including mention of a **global secondary index**)
- Thinking in terms of workload shape:
  - **Storage-heavy**
  - **Write-heavy**

## Links and resources

- Whiteboard diagram: https://github.com/johnthebrit/RandomStuff/raw/master/Whiteboards/CosmosDBOptimization.png
- Cosmos DB Request Units: https://learn.microsoft.com/azure/cosmos-db/request-units
- Cosmos DB Pricing: https://azure.microsoft.com/pricing/details/cosmos-db/standard-provisioned/
- Cosmos DB Reservations: https://learn.microsoft.com/azure/cosmos-db/reserved-capacity

## Additional learning resources from the author

- Recommended Learning Path for Azure: https://learn.onboardtoazure.com
- Certification Content Repository: https://github.com/johnthebrit/CertificationMaterials
- Weekly Azure Update playlist: https://youtube.com/playlist?list=PLlVtbbG169nEv7jSfOVmQGRp9wAoAM0Ks
- Azure Master Class playlist: https://youtube.com/playlist?list=PLlVtbbG169nG169nGccbp8VSpAozu3w9xSQJoY
- DevOps Master Class playlist: https://youtube.com/playlist?list=PLlVtbbG169nFr8RzQ4GIxUEznpNR53ERq
- PowerShell Master Class playlist: https://youtube.com/playlist?list=PLlVtbbG169nFq_hR7FcMYg32xsSAObuq8
- Certification Cram playlist: https://youtube.com/playlist?list=PLlVtbbG169nHz2qfLvPsAz9CnnXofhmcA
- Mentoring content playlist: https://youtube.com/playlist?list=PLlVtbbG169nGHxNkSWB0PjzZHwZ0BkXZZ
- FAQ: https://savilltech.com/faq

## Notes

- The video description notes subtitles can be enabled and auto-translated, with a demo: https://youtu.be/v5b53-PgEmI

