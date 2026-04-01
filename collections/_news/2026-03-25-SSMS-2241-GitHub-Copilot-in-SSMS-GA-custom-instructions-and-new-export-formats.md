---
tags:
- Agent Mode
- AI
- Copilot Chat
- Copilot Instructions.md
- Custom Instructions
- Database DevOps
- Developer Community Feedback
- Export To Excel
- Export To JSON
- Export To Markdown
- Export To XML
- GitHub Copilot
- GitHub Copilot in SSMS
- Group By Schema
- News
- Object Explorer
- Release Notes
- Results Grid Export
- Save With Encoding
- SQL Projects (preview)
- SQL Server Management Studio
- SSMS 22.4.1
primary_section: github-copilot
date: 2026-03-25 11:00:00 +00:00
section_names:
- ai
- github-copilot
external_url: https://blog.fabric.microsoft.com/en-US/blog/sql-server-management-studio-ssms-22-4-1-and-github-copilot-in-ssms-generally-available/
feed_name: Microsoft Fabric Blog
title: 'SSMS 22.4.1: GitHub Copilot in SSMS GA, custom instructions, and new export formats'
author: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces SSMS 22.4.1, bringing GitHub Copilot in SSMS to general availability with fixes, support for custom instructions via copilot-instructions.md, plus SSMS usability updates like new results-grid export formats and expanded “group objects by schema” in Object Explorer.<!--excerpt_end-->

## Overview

SSMS 22.4.1 is now available and includes general availability (GA) for **GitHub Copilot in SQL Server Management Studio (SSMS)**, plus several SSMS feature updates driven by community feedback.

- FabCon/SQLCon roundup (external): FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform
- GitHub Copilot in SSMS landing page: https://aka.ms/ssms-ghcp
- SSMS release notes (22.4.1): https://learn.microsoft.com/ssms/release-notes-22
- SSMS feedback hub: https://aka.ms/ssms-feedback

## GitHub Copilot in SSMS (GA)

GitHub Copilot was introduced in an SSMS 22 preview release “last fall” and refined with early adopter feedback. In **SSMS 22.4.1**, the Copilot integration includes:

- Numerous fixes for GitHub Copilot in SSMS
- Improved handling for query executions that:
  - returned no results, or
  - failed completely

### Custom instructions support (no solution/repository scenarios)

SSMS now supports **custom instructions** for GitHub Copilot when you are not working inside a solution/repository.

- Feature details: https://aka.ms/ssms-ghcp-custom-instructions
- Instructions file location: `copilot-instructions.md` in your **`%USERPROFILE%`** folder
- GitHub docs for configuring custom instructions: https://docs.github.com/copilot/how-tos/configure-custom-instructions

![Screenshot of the chat window for GitHub Copilot in SSMS, with the copilot-instructions.md file as a reference, highlighted by a red box.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-chat-window-for-github-copilot-i.png)

### Roadmap and what’s next

The SSMS roadmap has been updated and includes upcoming work such as **Agent mode**.

- SSMS roadmap: https://learn.microsoft.com/ssms/roadmap
- Provide feedback / upvote requests: https://aka.ms/ssms-feedback

## Additional export formats when saving the results grid

SSMS 22.4.1 adds four new **Save Results As...** formats for the results grid:

- Excel
- JSON
- XML
- Markdown

Previously, the available formats were **CSV** and **Text**.

### How to use

1. Run a query with **Results to Grid**.
2. Right-click the results grid.
3. Select **Save Results As...**
4. In **Save as type**, choose the format and select **Save**.
5. Optional: choose **Save with Encoding...** to select from available encodings.

![Screenshot of the right-click menu from the results grid in SSMS with Save Results As... highlighted.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-right-click-menu-from-the-result.png)

![Screenshot of the Save Grid Results dialog with new options available for file types, including JSON, XML, Excel and Markdown.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-save-grid-results-dialog-with-ne.png)

### Include column headers in exports

To export column headers, enable the option here:

- **Tools > Options > Query Results > Results to Grid**
- Check: **Include column headers when saving or dragging from the result grid**

![Screenshot of the Results to Grid options within Tools > Options. The option Include column headers when saving or dragging from the results grid is surrounded by a red box.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-results-to-grid-options-within-t.png)

## Group objects by schema in Object Explorer

SSMS previously introduced **group by schema** for **Fabric SQL databases**; this capability is now expanded “for other SQL offerings.”

### Enable grouping by schema

- Use the **group by schema** icon near the top of **Object Explorer**.
- Object Explorer reloads, and database objects appear grouped by owning schema.
- Default state: grouping by schema is **disabled**.

![Screenshot of Object Explorer in SSMS with the new Group By Schema button surrounded by a red box. Within the AdventureWorks2025 database, the objects are grouped by the different schemas including HumanResources, Person, Production, Purchasing, and Sales.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-object-explorer-in-ssms-with-the-new.png)

### Make “group by schema” the default

A new preference setting is available under:

- **Tools > Options > SQL Server Object Explorer > General**
- Check: **Group objects by Schema** (to enable it by default)

![Screenshot of the SQL Server Object Explorer options within Tools > Options. The option Group objects by Schema is surrounded by a red box.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-the-sql-server-object-explorer-optio.png)

## SQL Projects (Preview) and what’s next

SSMS 22.4.1 also adds **SQL Projects (Preview)**, a long-requested capability. The post notes SQL Projects will be covered separately:

- SQL Projects post: https://aka.ms/ssms-sqlproj-pupr

The team also calls out continued investment in **Database DevOps** support in upcoming SSMS releases and encourages users to provide feedback via Developer Community.


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/sql-server-management-studio-ssms-22-4-1-and-github-copilot-in-ssms-generally-available/)

