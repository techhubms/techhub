---
external_url: https://zure.com/blog/how-reusing-wcf-sql-xml-simplified-our-biztalk-to-azure-migration
title: How Reusing WCF-SQL XML Simplified Our BizTalk to Azure Migration
author: tim.dhaeyer@zure.com (Tim D'haeyer)
feed_name: Zure Data & AI Blog
date: 2025-10-30 12:50:02 +00:00
tags:
- Azure Functions
- Azure Key Vault
- BizTalk Migration
- Enterprise Integration
- Legacy System Modernization
- LINQ To XML
- Logic Apps
- SAP Integration
- SQL Server
- Staging Database
- Stored Procedures
- User Defined Table Types
- WCF SQL
- XML Integration
- XSLT Mapping
- Azure
- Coding
- DevOps
- Blogs
section_names:
- azure
- coding
- devops
primary_section: coding
---
Tim D'haeyer shares his experience migrating BizTalk flows to Azure by reusing WCF-SQL XML, enabling a more streamlined integration with SAP and SQL Server. His approach, detailed in this article, helped accelerate migration while preserving reliability.<!--excerpt_end-->

# How Reusing WCF-SQL XML Simplified Our BizTalk to Azure Migration

Migrating complex BizTalk flows to Azure can present numerous challenges, especially when dealing with intricate integrations and legacy systems. In this article, Tim D'haeyer explains how reusing the existing WCF-SQL XML from BizTalk enabled a significantly smoother migration to Azure, particularly for an integration scenario between SAP and an external Sales Force Automation (SFA) system relying on SQL staging databases.

## Project Context and Challenge

- Established BizTalk integrations utilized the WCF-SQL adapter, significant XSLT mapping, and connections to SAP RFCs.
- The SFA system required heavy SQL usage, including user-defined table types with upsert operations into staging tables.
- Tight deadlines and minimal testing bandwidth necessitated creative migration strategies.

## Rethinking the Integration Approach

- SQL connectors in Logic Apps do not handle user-defined table types well.
- Decision: Replace built-in SQL connector flows with a custom Azure Function for executing SQL statements directly (including on-prem databases).
- The obstacle: Migrating extensive BizTalk XSLT mapping and logic into the new Azure-based approach seemed daunting.

## Leveraging Existing XML for Migration

### Key Insight

Tim realized that by preserving and reusing the XML schemas and payloads already processed by BizTalk, it was possible to avoid reimplementing complex mapping logic. The XML passed to BizTalk can be parsed and interpreted in Azure, saving development time and reducing risk.

### Technical Walkthrough

- **Stored Procedures Extraction**: Used LINQ to XML to identify and extract stored procedure calls from the root XML node.
- **Parameter Handling**: Iterated each procedure’s parameters, distinguishing between simple types and user-defined table types (with appropriate namespace checks).
- **User-Defined Table Types**: Queried the SQL server to dynamically build a `DataTable` matching the expected structure for user-defined types, then populated rows from XML elements.

#### Code Highlights

```csharp
// Extract stored procedure elements
var spElements = xmlDoc.Root?
    .Elements()
    .Where(e => e.Name.Namespace.NamespaceName.Equals("http://schemas.microsoft.com/Sql/2008/05/Procedures/dbo"))
    .ToList();

// Loop through each stored procedure and handle parametersoreach (var spElement in spElements) {
    foreach (var parameterElement in spElement.Elements()) {
        string parameterName = parameterElement.Name.LocalName;
        var tableTypeElement = parameterElement.Elements()
            .FirstOrDefault(e => e.Name.Namespace.NamespaceName.Equals("http://schemas.microsoft.com/Sql/2008/05/Types/TableTypes/dbo"));

        if (tableTypeElement != null) {
            string tableTypeName = tableTypeElement.Name.LocalName;
            DataTable dataTable = await BuildDataTableFromTableType(...);
            sqlStatement.Parameters.Add(parameterName, dataTable);
        } else {
            sqlStatement.Parameters.Add(parameterName, parameterElement.Value);
        }
    }
}
```

- **Dynamic Schema Discovery**: Ran a SQL query to extract column definitions for each user-defined table type, dynamically constructing a matching C# `DataTable` to supply to SQL commands.
- **Row Handling and Type Conversion**: Iterated XML rows, populated `DataTable` with type conversion and null/empty checks.

## Azure Function Design and Security

- The new solution wrapped all this logic in an HTTP-triggered Azure Function, callable from Logic Apps.
- Connection-string selection was parameterized. Secrets (connection strings) are securely retrieved from Azure Key Vault.
- The XML payload could be passed as Base64 or via blob reference for flexibility.

## Scalability and Future Evolution

- Considering constraints around HTTP timeouts, an asynchronous Azure Service Bus-triggered function is noted as a potential enhancement for long-running jobs.
- The approach is tailored for migrations rather than net-new integrations, making it a strategic shortcut where maintenance, speed, and simplicity matter most.

## Conclusion

By reusing BizTalk’s XML schemas and adapting them for processing within an Azure Function, the migration process avoided unnecessary rewrites and preserved integration reliability. This method offered an expedient path away from legacy BizTalk infrastructure—demonstrating practical tactics for similar enterprise modernization initiatives.

---

*Article authored by Tim D'haeyer, Azure Consultant at Zure.*

This post appeared first on "Zure Data & AI Blog". [Read the entire article here](https://zure.com/blog/how-reusing-wcf-sql-xml-simplified-our-biztalk-to-azure-migration)
