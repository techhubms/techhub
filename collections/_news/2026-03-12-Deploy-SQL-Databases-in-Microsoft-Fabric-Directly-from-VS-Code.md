---
external_url: https://blog.fabric.microsoft.com/en-US/blog/deploy-sql-databases-in-fabric-from-vs-code-no-more-context-switching/
title: Deploy SQL Databases in Microsoft Fabric Directly from VS Code
author: Microsoft Fabric Blog
primary_section: ml
feed_name: Microsoft Fabric Blog
date: 2026-03-12 11:00:00 +00:00
tags:
- Application Development
- Azure
- Data Engineering
- Database Automation
- DevOps
- DevOps Workflow
- Extension
- Git Integration
- Item Templates
- Microsoft Fabric
- ML
- News
- Publish Dialog
- Schema Deployment
- Source Control
- SQL Database Projects
- T SQL
- Version Control
- VS Code
- .NET
section_names:
- azure
- dotnet
- devops
- ml
---
Microsoft Fabric Blog details how VS Code users can now deploy SQL database schema changes directly to Fabric, streamlining data engineering workflows. Author Microsoft Fabric Blog explains new publish dialog and item templates, improving productivity for database and application development teams.<!--excerpt_end-->

# Deploy SQL Databases in Microsoft Fabric Directly from VS Code

Author: Microsoft Fabric Blog

## Overview

Managing SQL databases in Microsoft Fabric is now more streamlined thanks to new features in the SQL Database Projects extension for VS Code. Developers can deploy schema changes, preview deployment scripts, and leverage prebuilt item templates—all from within their code editor, removing the need to context switch between tools.

## Key Features

### 1. Publish Dialog

- **Browse Fabric workspaces and select target database**: No longer search for server names or manually copy connection strings.
- **Preview deployment script**: Review the exact T-SQL before deploying, minimizing risk of unexpected changes.
- **Configure deployment options**: Control behaviors such as object deletion for safer, more controlled releases.
- **Production confidence**: Especially helpful for high-risk changes and production environments.
- **Demo Video**: [Publish dialog walkthrough](https://blog.fabric.microsoft.com/en-us/blog/deploy-sql-databases-in-fabric-from-vs-code-no-more-context-switching/)

### 2. Item Templates

- **Faster object creation**: Easily add new tables, views, stored procedures, or functions with preconfigured code generic to your team standards.
- **Consistency**: Templates enforce structuring, naming conventions, security patterns, or audit columns, promoting best practices.
- **Copilot Support**: Copilot agent and code completions in VS Code help scaffold and complete your SQL objects.

### Workflow Improvements

- **Application-like workflow for databases**: Author schema changes, version them in Git, review in pull requests, and deploy—all in VS Code.
- **Source Control Integration**: Fabric SQL databases now support Git integration, enabling peer review of database changes.
- **Local Testing**: Option to deploy to a local SQL Server 2025 container before pushing to Fabric.
- **Auditability and Team Standards**: Review and enforce consistent structure and standards across database objects.

## Why It Matters

The update aligns database development with modern application development workflows, allowing consistent source control and deployment processes without jumping between tools. This reduces friction, increases productivity, and diminishes the risk of deployment mistakes.

## Getting Started

- Features are available in the latest SQL Database Projects extension for VS Code.
- Documentation:
  - [Publish dialog](https://learn.microsoft.com/sql/tools/visual-studio-code-extensions/sql-database-projects/publish-database-project?view=sql-server-ver17)
  - [Item templates](https://learn.microsoft.com/sql/tools/visual-studio-code-extensions/sql-database-projects/add-item-templates?view=sql-server-ver17)
- Demo: [Source control with Fabric SQL databases](https://www.youtube.com/watch?v=6bBrrPY0H_Y)

## Roadmap

Microsoft is investing in future improvements:

- Enhanced code analysis and quality checks
- Advanced IntelliSense and productivity tooling
- AI-assisted database development
- For updates, see [vscode-mssql roadmap](https://github.com/microsoft/vscode-mssql/wiki/roadmap) and [DacFx roadmap](https://github.com/microsoft/DacFx/wiki)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/deploy-sql-databases-in-fabric-from-vs-code-no-more-context-switching/)
