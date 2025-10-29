---
layout: "post"
title: "Simplifying Data Ingestion with Copy Job: Enhanced File Format Support in Microsoft Fabric Data Factory"
description: "This news article covers recent enhancements to the Copy job in Microsoft Fabric Data Factory, focusing on expanded file format support and improved CSV configuration options. It details how Copy job now supports ORC, Excel, Avro, and XML formats, in addition to more granular control over CSV data ingestion through new Quote, Escape, and Encoding settings. Readers will learn how these features streamline data movement, improve compatibility across sources, and ensure data fidelity for complex ingestion workflows in data engineering and analytics."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-more-file-formats-with-enhancements/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-10-29 11:00:00 +00:00
permalink: "/2025-10-29-Simplifying-Data-Ingestion-with-Copy-Job-Enhanced-File-Format-Support-in-Microsoft-Fabric-Data-Factory.html"
categories: ["Azure", "ML"]
tags: ["Avro", "Azure", "Bulk Copy", "Cloud Data", "Copy Job", "CSV", "Data Engineering", "Data Factory", "Data Ingestion", "Data Movement", "Encoding", "Excel", "File Formats", "Incremental Copy", "Integration", "Microsoft Fabric", "ML", "News", "ORC", "XML"]
tags_normalized: ["avro", "azure", "bulk copy", "cloud data", "copy job", "csv", "data engineering", "data factory", "data ingestion", "data movement", "encoding", "excel", "file formats", "incremental copy", "integration", "microsoft fabric", "ml", "news", "orc", "xml"]
---

The Microsoft Fabric Blog team explains new enhancements to Copy job in Microsoft Fabric Data Factory, including expanded support for file formats and improved CSV handling options, enabling more flexible and robust data ingestion.<!--excerpt_end-->

# Simplifying Data Ingestion with Copy Job: More File Formats and Enhancements

Copy job in Microsoft Fabric Data Factory serves as a versatile tool for moving data across clouds, on-premises systems, and services. With its intuitive interface and support for bulk copy, incremental copy, and change data capture (CDC) replication, Copy job enables a wide array of data movement scenarios suited for modern data engineering workflows.

## Expanded File Format Support

**Copy job** now includes native support for the following additional file formats:

- **ORC**
- **Excel**
- **Avro**
- **XML**

This enhancement empowers users to ingest and distribute data from a variety of structured and semi-structured sources, increasing compatibility and flexibility depending on analytical or business needs.

## Enhanced CSV Configuration Options

Working with CSV data is now more robust, thanks to new settings:

- **Quote Character** — Configure how quoted text is parsed.
- **Escape Character** — Handle embedded delimiters and quotes for accurate parsing.
- **Encoding Options** — Select from UTF-8, UTF-16, or regional encodings like BIG5, GB18030, and SHIFT-JIS, streamlining integration between global data sources/destinations.

These controls ensure precise parsing, data fidelity, and compatibility across diverse workflows.

## Key Capabilities

- Move data efficiently regardless of the source or target format
- Seamless handling of new and legacy file types
- Flexible CSV ingestion for global or complex data sources
- Maintain robust data pipelines between on-prem, cloud, and hybrid environments

## Next Steps & Further Resources

- [Microsoft Fabric Copy job documentation](https://learn.microsoft.com/fabric/data-factory/what-is-copy-job)
- [Fabric Ideas Community](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas/label-name/data%20factory%20%7C%20copy%20job)
- [Microsoft Fabric technical documentation](https://aka.ms/FabricBlog/docs)
- Join discussions in [the Fabric Community](https://community.fabric.microsoft.com/t5/Copy-job/bd-p/db_copyjob)

If you have questions or feedback, you are encouraged to comment or participate in the linked communities.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/simplifying-data-ingestion-with-copy-job-more-file-formats-with-enhancements/)
