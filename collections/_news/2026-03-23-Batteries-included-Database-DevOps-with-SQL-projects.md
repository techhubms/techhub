---
tags:
- .NET Build
- Azure
- Azure DevOps
- Azure SQL Database
- BACPAC
- Branching And Merging
- CI/CD
- DACPAC
- Database DevOps
- DevOps
- GitHub
- Microsoft Fabric
- Microsoft.Build.Sql
- ML
- MSSQL Extension
- News
- Post Deployment Scripts
- Pre Deployment Scripts
- Pull Requests
- Schema Compare
- Source Control
- SQL Code Analysis
- SQL Database in Fabric
- SQL Projects
- SQL Server
- SQL Server Management Studio
- SQLCMD Variables
- SqlPackage
- SSMS 22.4
- VS Code
date: 2026-03-23 09:00:00 +00:00
primary_section: ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/batteries-included-database-devops-with-sql-projects/
title: 'Batteries included: Database DevOps with SQL projects'
author: Microsoft Fabric Blog
section_names:
- azure
- devops
- ml
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces new milestones for SQL projects across SSMS, VS Code, and SQL database in Fabric, focused on bringing database schema into source control and CI/CD with build validation, schema compare, and controlled deployments.<!--excerpt_end-->

# Batteries included: Database DevOps with SQL projects

*If you haven’t already, see Arun Ulag’s post “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for broader announcements across Fabric and Microsoft database offerings.*

Database changes often stay manual even when application code is fully automated. SQL projects aim to fix that by making database schema a first-class artifact in:

- Source control
- CI/CD pipelines
- Collaborative pull-request workflows

Microsoft announced milestones across the SQL ecosystem:

- **SQL projects in SQL Server Management Studio (SSMS) (Preview)**
- **Expanded SQL projects experience in VS Code (GA)**: publish dialog + richer schema compare
- **SQL database in Fabric CI/CD updates**: **pre-deployment** and **post-deployment** script support

This applies whether you run:

- SQL Server on-premises
- Azure SQL
- SQL database in Fabric

## Understanding Database DevOps with SQL projects

SQL projects provide a structured way to manage:

- Database schemas
- Deployment scripts
- Related database artifacts

Key ideas:

- Store database code in **version control**
- Support team collaboration on database changes
- Enable **continuous integration** and **continuous deployment** that includes databases

A SQL project supports both:

- **Declarative** schema definition (desired end state)
- **Migration-based** approaches (incremental scripts), while still providing validation and deployment tooling

![SQL database project comprised of database settings, pre-deployment script, database obejcts, and post-deployment script. Arrow to right showing build creates a dacpac box.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/sql-database-project-comprised-of-database-setting.png)

Building a SQL project:

- Validates syntax and object relationships
- Produces a **DACPAC** (compiled database schema)
- Deployment compares DACPAC schema vs target schema and generates a deployment plan to align the target

## Streamlined SQL projects: Fabric CI/CD and SQL database in Fabric

From the **Fabric portal**, you can connect a **SQL database in Fabric** to:

- GitHub repository
- Azure DevOps repository

Fabric will:

- Create a **SQL project** with the latest database schema
- Commit it to your repo

Because SQL projects support both **.sql files**, **.dacpac**, and generated deployment scripts, Fabric can support:

- **Branching and merging** database changes alongside code
- Creating a new development environment by deploying from the SQL project
- Reviewing schema impact through PRs in GitHub/Azure DevOps

You can apply changes:

- Directly from the Fabric portal
- Or generate a deployment script for manual review via SQL projects tools

![Query editor in Fabric screenshot with menu on a Shared query open and "Set as Post-deployment Script" option highlighted.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/query-editor-in-fabric-screenshot-with-menu-on-a-s.png)

### Pre-deployment and post-deployment scripts in Fabric

Fabric CI/CD for SQL database in Fabric now supports:

- **Pre-deployment scripts**
- **Post-deployment scripts**

These run during:

- Updating from source control
- Branching
- Deployment pipelines

How to add them:

1. Create a T-SQL script in the Fabric editor
2. Set it as a **Shared Query**
3. In the **Shared Queries** folder, mark it as pre- or post-deployment

Notes:

- These scripts match the pre/post deployment scripts used in SQL projects tooling
- Compatible with the wider ecosystem, including **SqlPackage CLI** and automation environments

Documentation: https://learn.microsoft.com/fabric/database/sql/source-control

## SQL Projects in SSMS: Database DevOps workload (Preview)

With **SSMS 22.4**, Microsoft introduced **SQL projects** in SSMS via the new **“Database DevOps” workload (Preview)**.

The preview includes core workflows:

- Create
- Build
- Publish

SSMS uses the same project backing as VS Code:

- **Microsoft.Build.Sql** projects

That enables cross-tool workflows (SSMS ↔ VS Code) with the same project format.

![SSMS installer with AI Assistance and Database DevOps (Preview) selected for install.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/ssms-installer-with-ai-assistance-and-database-dev.png)

### Working with a SQL project in SSMS

Workflow described:

- Clone the repository that was generated/updated from SQL database in Fabric
- Open the SQL project in SSMS
- Edit schema objects as **.sql files** in Solution Explorer
- Build and publish back to:
  - SQL database in Fabric
  - Or other target databases

SSMS project properties provide configuration for:

- **SQL code analysis rules** (code quality feedback during build)

Target platform behavior:

- New SQL projects default to **SQL Server 2025** as the target platform
- You can change the target platform to validate compatibility for:
  - Different SQL Server versions
  - Azure SQL Database
  - SQL database in Fabric

SQLCMD variables:

- SSMS project properties include **SQLCMD variables**
- Useful for environment-specific values without maintaining separate object versions
- Example mentioned: parameterizing an endpoint for `sp_invoke_external_rest_endpoint`

![SQL project properties dialog in SSMS with SQL Code Analysis tab selected. Design Rules, General, Naming Rules, and Performance Rules are available for settings.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/sql-project-properties-dialog-in-ssms-with-sql-cod.png)

Planned future SSMS improvements include:

- Creating and updating projects directly from databases
- Graphical schema compare interface

### Bringing an existing database into source control

Two options mentioned:

- VS Code **mssql extension**: “Create project from database”
- **SqlPackage CLI**: extract schema objects into `.sql` files

```sql
dotnet tool install -g microsoft.sqlpackage

sqlpackage /action:extract /sourceconnectionstring:"<connection string>" /targetfile:"databasefiles" /p:extracttarget=schemaobjecttype
```

![Solution explorer in SSMS with a SQL project for AdventureWorks open. Folders for stored procedure and table definitions are open.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/solution-explorer-in-ssms-with-a-sql-project-for-a.png)

More about SSMS 22.4.1: https://aka.ms/ssms-2241-blog

## Enhancing SQL Projects in VS Code: Publish dialog and schema compare

The **MSSQL extension for VS Code** includes support for:

- Microsoft.Build.Sql projects
- Schema compare
- Schema designer
- GitHub Copilot integrations (mentioned as an integration, not a focus)

A wizard for “Data-tier applications” was added:

- **DACPAC** and **BACPAC** wizard for creating/deploying packages

DACPAC vs BACPAC:

- **DACPAC**: iterative deployment / upgrade an existing database to new object versions
- **BACPAC**: snapshot of schema + data at a point in time (deploy to create a full copy)

![A SQL project in VS Code ready to publish with the Advanced Publish Options open.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/a-sql-project-in-vs-code-ready-to-publish-with-the.png)

### VS Code SQL Projects publish dialog (GA)

The publish dialog is now generally available and aims to streamline deploying a `.sqlproj` to a target Microsoft SQL database.

It provides:

- Connection selection
- Advanced property selection
- A preview of the equivalent **SqlPackage command** (helpful for reusing in CI/CD)
- Optional **deployment script generation** for review before applying

## Successfully leveraging SQL projects in your organization

SQL projects are intended to be adopted incrementally:

- **Start with source control**
  - Commit schema to Git
  - Get change history, diffs, PR visibility, single source of truth
  - For SQL database in Fabric: connect the database to a repo from the Fabric portal

- **Add build validation**
  - Add a CI build step using `dotnet build`
  - Catch syntax errors, broken references, code analysis violations early (on PR)

- **Automate deployments**
  - Use **SqlPackage CLI** or publish dialogs in VS Code/SSMS
  - Script-first review for confidence, then apply
  - Automate for non-prod environments when speed matters

- **Enforce standards**
  - Add custom code analysis rules
  - Enforce naming, performance, security policies
  - Run on every build

Entry points:

- Documentation: https://aka.ms/sqlprojects
- Public roadmap: https://aka.ms/sqlprojects-roadmap

Feedback is requested via the roadmap page.

[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/batteries-included-database-devops-with-sql-projects/)

