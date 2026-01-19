---
layout: post
title: How Sitecore Built a Scalable Isolated SaaS Platform on Azure
author: Microsoft Events
canonical_url: https://www.youtube.com/watch?v=Ra8_emjhRD0
viewing_mode: internal
feed_name: Microsoft Events YouTube
feed_url: https://www.youtube.com/feeds/videos.xml?channel_id=UCrhJmfAGQ5K81XQ8_od1iTg
date: 2025-11-20 15:44:34 +00:00
permalink: /coding/videos/How-Sitecore-Built-a-Scalable-Isolated-SaaS-Platform-on-Azure
tags:
- API Management
- Autoscale Analysis
- Circuit Breakers
- Compliance
- Cosmos DB
- Cost Management
- Data Isolation
- DB Per Tenant
- Deployment Stamps
- Event Overflow
- Fleet Monitoring
- MSIgnite
- Multi Tenant SaaS
- Partition Key
- SaaS Design Patterns
- Sitecore
- TCO Optimization
- Unifyyourdataplatform
section_names:
- azure
- coding
- devops
---
This Microsoft Ignite session, delivered by Deborah Chen, Jeffrey Ilse, and Andrew Liu, showcases Sitecore's approach to building a scalable, secure, and cost-effective SaaS platform on Azure, with a focus on Cosmos DB architectural patterns.<!--excerpt_end-->

{% youtube Ra8_emjhRD0 %}

# How Sitecore Built a Scalable Isolated SaaS Platform on Azure

*Speakers: Deborah Chen, Jeffrey Ilse, Andrew Liu*

This advanced session from Microsoft Ignite 2025 explores Sitecore's transformation to a multi-tenant SaaS platform leveraging Azure services, particularly Cosmos DB. The key topics and practices discussed include:

## SaaS Transformation Journey

- Introduction to Sitecore's API-first commerce platform, OrderCloud
- Challenges of global deployment and benefits of compliance-driven regional architecture

## Data Tier Design Patterns

- Comparison of two Cosmos DB tenancy strategies:
  - **DB-per-Tenant Pooling**: Each tenant receives a separate database instance
  - **Partition-Key-per-Tenant**: Multiple tenants share a database, separated by partition keys
- Analysis of cost implications, TCO, and isolation provided by each model
- Security posture considerations when selecting a tenancy strategy
- Real-world trade-offs between cost, performance, and customer isolation

## Operational Best Practices

- Employing the bulkhead pattern to isolate regions and customers, improving reliability
- Managing event overflow and handling AI pipeline costs
- Integrating circuit breakers and comparing SaaS models for robust architecture

## Cost Optimization & Monitoring

- Using dashboards and query tools to monitor large-scale Cosmos DB fleets
- Identifying unused database accounts and pursuing cost-saving measures with autoscale analysis

## API Management and Custom Solutions

- Addressing API management questions and sharing the architectural rationale for custom-built integration

## Additional Resources

- Further explore Microsoft Fabric Data Engineering Plan: [ignite25-plans-MicrosoftFabricDataEngineeringPlan](https://aka.ms/ignite25-plans-MicrosoftFabricDataEngineeringPlan)
- Watch more sessions from Microsoft Ignite at [ignite.microsoft.com](https://ignite.microsoft.com)

---

**Key Takeaways:**

- Strategic choices in Cosmos DB multi-tenancy design significantly impact scalability, costs, and isolation.
- Use of deployment stamps and bulkhead patterns enhances platform reliability and regional compliance.
- Operational monitoring and cost controls are essential for managing large, multi-tenant SaaS solutions on Azure.
