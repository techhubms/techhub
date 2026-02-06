---
external_url: https://blog.fabric.microsoft.com/en-US/blog/mssql-extension-for-vs-code-fabric-integration-public-preview/
title: MSSQL Extension for VS Code Adds Fabric Integration and Database Provisioning
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-09-23 11:00:00 +00:00
tags:
- Azure SQL
- Context Switching
- Database Provisioning
- Developer Tools
- Fabric Connectivity
- Microsoft Entra ID
- Microsoft Fabric
- Modern App Development
- MSSQL Extension
- Public Preview
- SQL Database
- SQL Development
- VS Code
- VS Code Extension
- Workspace Authentication
- Azure
- News
- .NET
section_names:
- azure
- dotnet
primary_section: dotnet
---
Microsoft Fabric Blog showcases new features by introducing Fabric Connectivity and SQL Database provisioning within the MSSQL extension for VS Code, empowering developers with seamless integration and modern SQL development tools.<!--excerpt_end-->

# MSSQL Extension for VS Code: Fabric Integration (Preview)

## Overview

Version 1.36.0 of the [MSSQL Extension for VS Code](http://aka.ms/vscode-mssql) brings Microsoft Fabric functionality directly to developers' local workflows. This release introduces two major features: Fabric Connectivity (Browse) and SQL Database in Fabric Provisioning, both available in Public Preview. The additions are designed to facilitate easier database management and application prototyping without leaving the Visual Studio Code environment.

## Fabric Connectivity (Preview)

- **Direct Workspace Access:** Users can now browse and connect to Fabric workspaces within VS Code using a dedicated Fabric connection type in the connection dialog.
- **Seamless Authentication:** Authentication leverages Microsoft Entra ID, offering secure, persistent sign-in.
- **Intuitive Workspace Browsing:** Workspaces are displayed in a tree view, supporting on-demand resource loading and real-time search for efficient navigation.

![Fabric browse experience in the MSSQL extension](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/09/fabric-connectivity-1024x649.png)

## SQL Database in Fabric Provisioning (Preview)

- **Simple Provisioning Flow:** Developers can authenticate, select or create a workspace, and provision a SQL database within minutes, right from the Deployments page.
- **Immediate Connection:** Newly created databases are automatically added to your connections for instant use.
- **Consistent Experience:** Provisioning aligns with other backend options, like local SQL Server containers, and offers clear guidance when capacity isn’t available.

![Fabric provisioning animated demo](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2025/09/fabric-provisioning.gif)

## Benefits for Developers

- Minimizes context switching by enabling Fabric integration within VS Code
- Accelerates application prototyping and database management
- Enhances security and convenience with persistent Entra ID authentication
- Offers a unified developer workflow for SQL Server, Azure SQL, and Fabric

## Get Involved

- Share feedback and ideas through [GitHub discussions](https://aka.ms/vscode-mssql-discussions)
- Submit new feature requests [here](https://aka.ms/vscode-mssql-feature-request)
- Report bugs [here](https://aka.ms/vscode-mssql-bug)
- [View Fabric Connectivity demo](https://aka.ms/vscode-mssql-fabric-browse-demo)
- [View Fabric provisioning demo](https://aka.ms/vscode-mssql-fabric-db-demo)
- [See the full playlist of demos](https://aka.ms/vscode-mssql-demos)

## Conclusion

These updates further solidify the MSSQL extension for VS Code as an accessible and efficient tool for SQL development across Microsoft database platforms and Fabric. With Fabric integration, developers can expect more cohesive workflows, enhanced productivity, and a consistent experience right in their code editor.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/mssql-extension-for-vs-code-fabric-integration-public-preview/)
