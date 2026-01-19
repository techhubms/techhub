---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/talk-to-your-data-postgresql-gets-a-voice-in-vs-code/ba-p/4453695
title: Boost Productivity with the PostgreSQL Extension and GitHub Copilot in VS Code
author: TELAWRENCE
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-09-25 05:00:00 +00:00
tags:
- Azure Database For PostgreSQL
- Copilot Chat
- Database Development
- Developer Productivity
- Docker
- Extension Marketplace
- IntelliSense
- Microsoft Entra ID
- PostgreSQL
- Semantic Kernel
- SQL Optimization
- VS Code
section_names:
- ai
- azure
- coding
- github-copilot
---
TELAWRENCE demonstrates how to supercharge PostgreSQL development inside Visual Studio Code using Microsoft’s new extension and GitHub Copilot AI. Learn how to set up, connect, and leverage intelligent features to streamline your workflow.<!--excerpt_end-->

# Boost Productivity with the PostgreSQL Extension and GitHub Copilot in VS Code

## Introduction

Modern developer teams juggle multiple tools to build applications, but Microsoft's improved PostgreSQL extension for Visual Studio Code (VS Code) brings powerful, integrated database management and AI assistance to your development environment. This guide walks you through installation, connection setup, and advanced productivity features.

## 1. Installing the PostgreSQL Extension

- **Open VS Code** and visit the Extensions view (`Ctrl+Shift+X` or `⌘+Shift+X`).
- Search for **'PostgreSQL'** and install the official (Preview) extension from Microsoft (blue elephant icon).
- After installation, look for the elephant icon in the Activity Bar indicating success.

## 2. Connecting to a PostgreSQL Database

The extension supports:

- **Azure Database for PostgreSQL**
  - Use the 'Browse Azure' dialog to sign in via Microsoft Entra ID (formerly Azure AD).
  - Enjoy password-less, secure authentication and centralized identity management.
- **Local Databases using Docker**
  - Quickly spin up a PostgreSQL Docker instance directly from the extension.
- **Manual Connections**
  - Paste a standard connection string or manually enter host, port, user, and password. Credentials are encrypted locally.

## 3. Database Management in VS Code

Utilize the PostgreSQL Explorer for seamless management:

- **Browse and modify**: Navigate servers, databases, schemas, tables, and functions within the sidebar.
- **Edit and create**: Right-click to select rows, script tables, or change schema. Supports table creation, alteration, and deletes.
- **Fast search**: Use filters to locate tables or views easily.

## 4. Rich Query Editing & IntelliSense

- **SQL Query Editor**: Open a new query via right-click or palette commands.
- **IntelliSense for SQL**: Get real-time code completion, suggestions for table/column names, and JOIN conditions based on schema context.
- **Execute and Explore Results**:
  - Query results appear in an interactive grid, sortable and filterable.
  - Easily export to CSV, JSON, or Excel.
  - Maintain a query history for quick re-runs.

## 5. Advanced AI Features with GitHub Copilot

- **AI IntelliSense**: Get smarter SQL completions, error prevention, and syntax awareness thanks to underlying AI models.
- **Copilot Chat (`@pgsql`)**: Ask database questions or automate tasks in natural language. Example: `@pgsql how do I create a new table with a JSONB column indexed for search?`
- **Explain and Optimize**: Highlight queries and use Copilot to explain, refactor, or analyze performance, receiving tailored suggestions and natural language explanations.
- **Data insights integration**: Easily export data for ML/AI application scenarios, connect to notebooks, and leverage further Microsoft AI tooling. Upcoming features include vector search and deeper AI integration.

## 6. Security & Productivity Benefits

- **Secure Authentication**: Uses encrypted storage and integrates with Entra ID for password-less Azure connections.
- **Improved Workflow**: Drastically reduces context-switches, enabling developers to manage databases, edit queries, and access AI tools all within VS Code.
- **Community and Expert Validation**: Feedback from early adopters and industry architects underscores the fast prototyping and development gains.

## 7. Getting Started

- **Install now**: [VS Code Marketplace – PostgreSQL Extension](https://marketplace.visualstudio.com/items?itemName=ms-ossdata.vscode-pgsql)
- **Explore documentation**: Find quickstarts and advanced scenarios on Microsoft Learn.
- **Update regularly**: The extension is in public preview and features are evolving quickly.

---
**Happy coding and may your context switches be few!**

## Further Reading & References

- [Official documentation and quickstart on Microsoft Learn](https://learn.microsoft.com)
- [Azure Database for PostgreSQL](https://azure.microsoft.com/en-us/products/postgresql/)
- [Semantic Kernel](https://github.com/microsoft/semantic-kernel)

## About the Author

Written by TELAWRENCE, a contributor to the Microsoft Developer Community Blog. Quotes from industry experts and community feedback have been included to ensure practical, real-world insight.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/talk-to-your-data-postgresql-gets-a-voice-in-vs-code/ba-p/4453695)
