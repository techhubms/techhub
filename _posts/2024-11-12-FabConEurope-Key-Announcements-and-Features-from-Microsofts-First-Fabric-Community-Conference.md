---
layout: "post"
title: "#FabConEurope: Key Announcements and Features from Microsoft's First Fabric Community Conference"
description: "Samu Niemelä summarizes major updates and new features introduced at Europe's first Microsoft Fabric Community Conference. Highlights include network security enhancements, user experience updates, data warehouse improvements, developments in data engineering and science, Data Factory advancements, and real-time intelligence features."
author: "Samu Niemelä"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://zure.com/blog/fabconeurope-onsite-reporting-from-microsoft-fabrics-first-community-conference/"
viewing_mode: "external"
feed_name: "Zure Data & AI Blog"
feed_url: "https://zure.com/category/blog/feed/?tag=dataai"
date: 2024-11-12 06:28:18 +00:00
permalink: "/2024-11-12-FabConEurope-Key-Announcements-and-Features-from-Microsofts-First-Fabric-Community-Conference.html"
categories: ["Azure", "ML"]
tags: ["AI", "AI Skills", "Azure", "Conference", "Data & AI", "Data Engineering", "Data Factory", "Data Platform", "Data Warehouse", "Event Streaming", "Fabric Community Conference", "Fabric Runtime", "Microsoft Fabric", "ML", "Network Security", "Posts", "Real Time Analytics", "T SQL Notebooks"]
tags_normalized: ["ai", "ai skills", "azure", "conference", "data and ai", "data engineering", "data factory", "data platform", "data warehouse", "event streaming", "fabric community conference", "fabric runtime", "microsoft fabric", "ml", "network security", "posts", "real time analytics", "t sql notebooks"]
---

Authored by Samu Niemelä, this overview details the major innovations and updates unveiled at the first Microsoft Fabric Community Conference in Europe, focusing on improvements in data, analytics, and Azure-integrated capabilities.<!--excerpt_end-->

# #FabConEurope – Onsite Reporting from Microsoft Fabric’s First Community Conference

**Published:** 12.11.2024  
**Author:** Samu Niemelä, Data & AI Domain Lead at Zure

---

Europe’s first Microsoft Fabric Community Conference in Stockholm was a landmark event, gathering data professionals and Microsoft experts for three days of announcements and deep dives into the Microsoft Fabric platform. Samu Niemelä, Data Domain Lead at Zure, attended and presents a detailed overview of the key takeaways and exciting new features.

## Key Takeaways and Announcements

### Platform Innovations

- **Network Security for All Capacities:** Previously reserved for higher-capacity (F64+) environments, essential security features such as trusted workspace access through firewalls are now available across all Fabric capacity levels. This democratizes security controls for organizations regardless of environment size.
- **User Interface Overhaul:** The portal experience, previously segmented into six different roles, will be streamlined into just two: 'developer' and 'analytics.' This simplification aims to improve usability and reduce confusion when navigating the Fabric environment.

### Data Warehouse Enhancements

- **Performance Focus:** New features such as data clustering, result set caching, and extended T-SQL support (including removal of VARCHAR column limit and nested CTEs) are rolling out in 2024, further enhancing Fabric’s data warehousing capabilities.
- **Database Migration Service (Private Preview):** Enables migration of database structures and data from external sources into Fabric Warehouses, offering smoother transitions for organizations.
- **T-SQL Notebook Support:** Notebooks now support T-SQL in public preview, unifying code handling (Python and T-SQL) and improving version control capabilities for teams.

### Data Engineering Updates

- **Fabric Runtime 1.3 (GA):** The new runtime—default for new notebooks—delivers performance and efficiency improvements, including support for Delta Lake 3.2 and the Native Execution Engine.
- **High-Concurrency Mode for Notebooks in Pipelines:** Reduces Spark pool spin-up time by enabling reuse of sessions across notebooks, boosting pipeline performance and efficiency.

### Data Science Developments

- **AI Skills (Preview):** A new Q&A chatbot experience lets users ask data-driven questions on selected Fabric tables, with the ability to supply predefined queries for tailored responses. Integration with Teams shows potential for collaborative analytics.

### Data Factory Advancements

- **Copy Job Task (Public Preview):** Streamlines data ingestion via Fabric workspaces, supporting both full and incremental loading with a user-friendly interface. Primed for quick ad-hoc and test scenarios.
- **Invoke Remote Pipelines:** Public preview support for orchestrating Azure Data Factory and Synapse pipelines directly from within Fabric, unifying monitoring and management across hybrid solutions.

### Real-Time Intelligence

- **Streaming and Usability Improvements:** Updates to the Real-Time Hub UI, easier management for KQL databases, and the addition of connectors (like SQL Server on VM CDC and Apache Kafka) enhance real-time analytics capabilities.
- **Query Acceleration with Lakehouse and KQL:** Sneak peek at features allowing delta tables in lakehouses to be joined with KQL databases, enabling powerful, mixed analytical and streaming scenarios in the future.

## Impressions from the Conference

Microsoft’s commitment to the Fabric ecosystem was evident, from the scale of the event to the on-site presence of numerous experts. Attendees left not only with knowledge, but also valuable networking connections and branded swag. Samu noted particular excitement around the enhanced security posture, unification of roles in the portal, and cross-platform pipeline orchestration.

## About the Author

**Samu Niemelä** is Data & AI Domain Lead at Zure, with a decade of experience in data modeling, report design, and solution architecture atop Azure, guiding clients to maximize value from data.

---

### Related Reading

- [Heidi’s first weeks as a Data Consultant at Zure](https://zure.com/blog/heidis-first-weeks-as-a-data-consultant-at-zure/)
- [Translytical Fabric (Power BI write-back)](https://zure.com/blog/translytical-fabric-ie-power-bi-write-back/)
- [Microsoft Ignite 2024: Key Updates from Microsoft Fabric Perspective](https://zure.com/blog/microsoft-ignite-2024-key-updates-for-fabric/)
- [AI Security Posture Management: Considerations](https://zure.com/blog/ai-security-posture-management-what-it-is/)

For more updates, visit the [Zure Blog](https://zure.com/news-blog-events/).

---

*Contact Samu Niemelä at [samu.niemela@zure.com](mailto:samu.niemela@zure.com) for more information or inquiries about data solutions and Microsoft Fabric insights.*

This post appeared first on "Zure Data & AI Blog". [Read the entire article here](https://zure.com/blog/fabconeurope-onsite-reporting-from-microsoft-fabrics-first-community-conference/)
