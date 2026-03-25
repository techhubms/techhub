---
tags:
- Agent Mode
- AI
- CRUD Operations
- Data Analytics Architecture
- Edit Item Definitions
- Extension Settings
- Fabric Extension
- Fabric Items
- Fabric MCP Server
- Fabric REST APIs
- GitHub Copilot
- GitHub Copilot Chat
- GitHub Issues
- Item Definitions
- Lakehouse
- MCP
- Medallion Architecture
- Microsoft Fabric
- ML
- News
- Notebooks
- OneLake
- Read Only Mode
- VS Code
- VS Code Extension
- VS Marketplace
- Workspace Folders
primary_section: github-copilot
date: 2026-03-25 10:30:00 +00:00
section_names:
- ai
- github-copilot
- ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/explore-edit-and-organize-item-definitions-and-fabric-mcp-server-in-vs-code/
feed_name: Microsoft Fabric Blog
title: Explore, edit, and organize Item definitions and Fabric MCP server in VS Code
author: Microsoft Fabric Blog
---

Microsoft Fabric Blog shares a roundup of recent updates to the Microsoft Fabric VS Code extension, including browsing workspace folders, viewing/editing item definitions, and using the Fabric MCP server with GitHub Copilot Chat (including an agent mode) for common Fabric tasks like REST API work and item CRUD operations.<!--excerpt_end-->

## Overview

This update covers recent improvements to the **Microsoft Fabric extension for Visual Studio Code**, plus a new **Fabric MCP server** (pre-release) that integrates with **GitHub Copilot Chat** to provide Fabric-specific tools and prompts.

> Note: The post also references Arun Ulag’s blog for a broader set of FabCon/SQLCon 2026 announcements: “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform”

## Browse workspace folders

Folders are now accessible inside your Fabric workspace view in VS Code, making it easier to inspect how Fabric content is organized without leaving the editor.

![Microsoft Fabric interface showing project/workspace navigation with folder and item listing](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-shows-a-microsoft-fabric-interface-with.png)

**Current limitation**

- You **cannot create items inside folders** from the Fabric extension yet.
- Improvements for working with folders are planned for future releases.

## View Fabric item definitions

You can now **view and edit item definitions** directly in VS Code.

- By default, item definitions open in **read-only** mode (editing is disabled).

## Edit Fabric item definitions

To enable editing:

1. Update the **Fabric extension settings** to allow editing.
2. Make changes to the item definition.
3. Save via **File > Save**.

Saved changes update the Fabric item in your workspace.

**Warning**: Incorrect values can break your Fabric item, so edit carefully.

![Fabric extension settings showing option to enable editing item definitions](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-shows-a-microsoft-fabric-interface-with-1.png)

## Fabric MCP server for GitHub Copilot Chat

A **Fabric MCP server** is available as a **pre-release** extension:

- Fabric MCP server (VS Marketplace): https://marketplace.visualstudio.com/items?itemName=fabric.vscode-fabric-mcp-server

If you have both the **Fabric extension** and the **GitHub Copilot Chat** extension installed, VS Code will prompt you to enable/install the Fabric MCP server if it isn’t already installed.

This enables Fabric-specific MCP tools inside Copilot Chat, providing a more tailored experience for working with Fabric artifacts.

![Prompt to install Fabric MCP server extension](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-shows-a-notification-prompt-in-microsoft.png)

### Example usage

The post shows an example prompt for Copilot Chat:

- Write a design document for implementing **Medallion architecture** in Fabric.

![Copilot Chat example for generating a design doc for Medallion architecture in Fabric](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-appears-to-be-a-screen-capture-of-an-int.png)

### Use Fabric agent mode

When developing in VS Code, you can switch to **Fabric agent mode** for a more tailored Copilot experience.

This is called out as useful for:

- Understanding an item definition
- Working with **Fabric REST APIs**
- Accessing Microsoft Fabric documentation
- Creating, updating, deleting, and listing items in your tenant

![UI showing Fabric agent mode configuration](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-image-displays-a-user-interface-for-an-e-fabri.png)

To see all available tools:

- Microsoft MCP tools on GitHub: https://github.com/microsoft/mcp

Feedback for the Fabric MCP server can be shared by filing issues in the repo.

### Example prompts (from the post)

| Category | Prompt |
| --- | --- |
| Perform CRUD operations | I need to test OneLake file operations. In Workspace1, create a “DataLakehouse” lakehouse. Upload a CSV file from <local path> with sample sales data to that directory. List the contents to verify the upload.  

OR  

I want to create and run a new notebook, called SampleNotebook in Workspace1 workspace. Add a markdown cell with the title “Sample Notebook”. Add a Python cell that prints “Hello, Fabric!”. |
| Generate specs and plan | Create a design document for data analytics architecture in Microsoft Fabric for my sales project using Medallion Architecture. Provide recommendations for each layer (Bronze, Silver, and Gold) on what item types to use for storing and processing the data. Also include a detailed execution plan. |

## Share feedback / get the extensions

- Report issues / suggestions: https://github.com/microsoft/vscode-fabric/issues
- Install Microsoft Fabric extension (VS Marketplace): https://marketplace.visualstudio.com/items?itemName=fabric.vscode-fabric


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/explore-edit-and-organize-item-definitions-and-fabric-mcp-server-in-vs-code/)

