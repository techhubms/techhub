---
layout: post
title: Building a Snapshot Backup Catalog with SQL Server 2025 and Pure Storage FlashArray
author: Microsoft Developer
canonical_url: https://www.youtube.com/watch?v=gFOduTsR3kE
viewing_mode: internal
feed_name: Microsoft Developer YouTube
feed_url: https://www.youtube.com/feeds/videos.xml?channel_id=UCsMica-v34Irf9KVTh6xx-g
date: 2025-09-11 16:01:15 +00:00
permalink: /azure/videos/Building-a-Snapshot-Backup-Catalog-with-SQL-Server-2025-and-Pure-Storage-FlashArray
tags:
- Azure SQL
- Backup Catalog
- Cloud Computing
- Cloud Replication
- Database Backup
- Database Management
- Dev
- Development
- FlashArray
- Metadata Tagging
- Microsoft
- Pure Storage
- REST API
- Snapshot Backup
- Sp Invoke External REST Endpoint
- SQL Server
- Storage Snapshots
- T SQL
- Tech
- Technology
section_names:
- azure
---
Microsoft Developer presents how SQL Server 2025 integrates with Pure Storage FlashArray to create a self-documenting, metadata-rich backup snapshot catalog, covering automation and metadata embedding directly from T-SQL.<!--excerpt_end-->

{% youtube gFOduTsR3kE %}

# Building a Snapshot Backup Catalog with SQL Server 2025 and Pure Storage FlashArray

Learn how to use SQL Server 2025’s new native REST API features alongside Pure Storage FlashArray’s advanced snapshot tagging capabilities to revolutionize backup management and cataloging.

## Key Takeaways

- **Automated Snapshot Creation**: Leverage SQL Server 2025's `sp_invoke_external_rest_endpoint` stored procedure to initiate and manage snapshot creation directly from T-SQL without needing external backup scripts or tools.
- **Metadata-Rich Snapshots**: Embed detailed metadata, such as database name, SQL instance, backup timestamp, backup type, and backup URL, directly into each snapshot. This ensures every backup snapshot is self-describing and easy to manage.
- **Queryable Backup Catalog**: Treat snapshots as searchable data assets. You can query your backup catalog by database, instance, time range, or any metadata tag combinations, providing quick insights and improving backup discoverability.
- **Consistent and Portable Backups**: By tagging snapshots as copyable, all embedded metadata remains intact even when replicating snapshots across arrays, data centers, or to the cloud, enhancing backup portability and compliance.

## How It Works

1. **Integrate SQL Server 2025 with FlashArray**
   - Use the new `sp_invoke_external_rest_endpoint` T-SQL procedure to call the FlashArray REST API.
   - Automate snapshot creation and apply metadata tags from within SQL Server, improving consistency and transparency.
2. **Embed Metadata for Self-Documentation**
   - Metadata (database, instance, timestamp, backup type) is added to each snapshot at the time of creation.
   - Snapshots are searchable and filterable by any combination of these metadata attributes.
3. **Enable Snapshot Portability**
   - Protection Group Tags travel with the snapshot, ensuring metadata is not lost when snapshots are moved or replicated between storage environments.

## Practical Demo Highlights

- **Real-World Scenario**: Step-by-step demonstration of snapshot creation, tagging, and querying with SQL Server 2025 and FlashArray.
- **T-SQL Automation**: Sample scripts show how to invoke REST APIs and manage tags from SQL statements.
- **Querying and Cataloging**: Examples of using T-SQL to search and catalog snapshots based on tags and metadata, making backup management streamlined.

## Additional Resources

- [Connect with the presenters on Twitter](https://twitter.com/AnalyticAnna)
- [Azure SQL Twitter](https://aka.ms/azuresqltw)
- [Data Exposed YouTube Series](https://aka.ms/dataexposedyt)
- [Microsoft Azure SQL Channel](https://aka.ms/msazuresqlyt)
- [Microsoft SQL Server Channel](https://aka.ms/mssqlserveryt)

---

Discover how SQL Server 2025 and Pure Storage FlashArray can make backup management smarter, more reliable, and adaptable across your environments.
