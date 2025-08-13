---
layout: "post"
title: "General Availability of Azure Monitor Auxiliary Logs, New Features, and Price Reduction"
description: "Microsoft announces major advancements to Azure Monitor's Auxiliary Logs—now in General Availability across all regions—including enhanced KQL support, new ingestion-time KQL transformations, broader summary rule use, expanded search job capabilities, and a significant cost reduction. These updates help organizations manage and analyze high-volume Azure logs more cost-effectively, with improved querying, flexible log ingestion, and streamlined data analysis for operational and security workloads. The new features align with Microsoft's evolving data lake and observability strategy, integrating with Sentinel Data Lake and facilitating robust analytics across logging tiers."
author: "AdiBiran"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-observability-blog/general-availability-of-auxiliary-logs-and-reduced-pricing/ba-p/4439460"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-05 15:06:19 +00:00
permalink: "/2025-08-05-General-Availability-of-Azure-Monitor-Auxiliary-Logs-New-Features-and-Price-Reduction.html"
categories: ["Azure", "ML", "Security"]
tags: ["Analytics Logs", "Auxiliary Logs", "Azure", "Azure Monitor", "Basic Logs", "Cost Optimization", "Data Collection Rules", "Data Lake", "Delta Parquet", "Ingestion Time Transformations", "KQL", "Log Ingestion", "Microsoft Security", "ML", "News", "Operational Monitoring", "Pricing Update", "Search Jobs", "Security", "Sentinel Data Lake", "Summary Rules", "Table Plans", "Workspace Rules"]
tags_normalized: ["analytics logs", "auxiliary logs", "azure", "azure monitor", "basic logs", "cost optimization", "data collection rules", "data lake", "delta parquet", "ingestion time transformations", "kql", "log ingestion", "microsoft security", "ml", "news", "operational monitoring", "pricing update", "search jobs", "security", "sentinel data lake", "summary rules", "table plans", "workspace rules"]
---

AdiBiran outlines Azure Monitor Auxiliary Logs' general availability, new features, and lower pricing, highlighting enhancements in querying, data transformations, and large-scale log management for Microsoft customers.<!--excerpt_end-->

# General Availability of Azure Monitor Auxiliary Logs and Reduced Pricing

![Azure Monitor Auxiliary Logs](https://techcommunity.microsoft.com/t5/s/gxcuf89792/images/bS00NDM5NDYwLXQ3OERnaQ?revision=1&amp;image-dimensions=2000x2000&amp;constrain-image=true)

Azure Monitor Logs is a foundational monitoring tool for hundreds of thousands of organizations running mission-critical Azure workloads. This news highlights the general availability and major improvements to **Auxiliary Logs**, a high-volume logging tier now accessible in all regions.

### What Are Auxiliary Logs?

Auxiliary Logs is designed for high-volume ingestion scenarios and works alongside Basic and Analytics Logs as part of Azure Monitor. Customers—including teams ingesting over a petabyte per day—benefit from scalable, cost-effective logging for detailed analysis and long-term storage.

Key updates include:

- **General Availability:** Auxiliary Logs are now fully supported, with broad regional presence and enhanced service capabilities.
- **Security Data:** Support now includes not just Custom Logs but also security data.
- **Table Support:** Additional tables will be supported soon (see [table plans](https://aka.ms/logsTablePlans)).
- **Price Reduction:** Significant price cuts now make Auxiliary Logs a more economical choice for high-volume data scenarios. Refer to [Azure Monitor pricing](https://aka.ms/logsPricingPage) for details.
- **Sentinel Data Lake Integration:** Logs can move between Auxiliary and Sentinel Data Lake without duplication, supporting modern data lake technology and operational/security analytics. [Sentinel Data Lake announcement](https://techcommunity.microsoft.com/blog/microsoft-security-blog/introducing-microsoft-sentinel-data-lake/4434280).

### Enhanced Query Capabilities

- **Expanded KQL Support:** Now, all KQL operators—including the lookup operator to Analytics tables—are supported in a single-table context.
- **Performance Boosts:** Built on Delta Parquet, providing improved encoding and partitioning for faster queries.
- **Extended Time Range:** Query any time period, not just the last 30 days.
- **Cost Estimation Preview:** Estimate query costs before executing, providing better budget management.

### Summary Rules: General Availability

Summary rules—newly in general availability—increase efficiency for summarizing high-ingestion streams, supporting robust analysis and dashboarding. Key benefits:

- Increased rule limits per workspace
- Retry functionality for incident-affected bins
- Broadened regional access
- Efficient summarization without losing raw data for investigations

Learn more: [Summary Rules Details](https://aka.ms/LogsSummaryRule)

### Search Jobs: More Power and Flexibility

Search jobs allow async scanning of massive data volumes, ingesting results into Analytics tables. Recent improvements include:

- Load up to 100 million records (coming soon)
- Streamlined user interface for search jobs
- Cost prediction prior to execution
- Increased concurrency and removed additional limits
- Support for all KQL operators on a single table with lookup operator to Analytics tables (rolling out soon)

More info: [Search Jobs](https://aka.ms/logssearchjob)

### KQL Transformations at Ingestion (Public Preview)

Now in public preview: KQL-based transformations for Auxiliary Logs at ingestion time. This brings Auxiliary Logs in line with other Azure Monitor tiers. Benefits:

- Apply KQL filters and data shaping as data is ingested
- Reduce storage costs by filtering noise
- Parse, split, and distribute log fields for optimized downstream usage
- Supports Data Collection Rules (DCRs) for declarative pipeline setup

Learn more:

- [Ingestion-Time Transformations](https://go.microsoft.com/fwlink/?linkid=2327607)

### Cost and Data Governance

Custom transformations incur log processing charges; check the [pricing page](https://aka.ms/logsPricingPage) for up-to-date information. These enhancements fit within Microsoft’s broader cloud data lake and observability evolution, aiming for unified operational and security insights on a common stack.

---

For organizations with heavy monitoring and security compliance needs, these upgrades and cost reductions position Auxiliary Logs as a flexible, scalable, and budget-friendly solution on Azure.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-observability-blog/general-availability-of-auxiliary-logs-and-reduced-pricing/ba-p/4439460)
