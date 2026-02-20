---
external_url: https://blog.fabric.microsoft.com/en-US/blog/no-more-excuses-ai-powered-assistants-are-in-ssms-vs-code-and-fabric/
title: 'AI-Powered Assistants in SSMS, VS Code, and Fabric: GitHub Copilot for SQL Developers'
author: Microsoft Fabric Blog
primary_section: github-copilot
feed_name: Microsoft Fabric Blog
date: 2026-02-17 12:00:00 +00:00
tags:
- Agent Mode
- AI
- AI Powered Coding
- Azure
- Azure SQL
- Database Chat Assistant
- Database Tools
- Fabric SQL
- GitHub Copilot
- Microsoft Fabric
- MSSQL Extension
- News
- Query Optimization
- SQL Development
- SQL Server Management Studio
- SSMS
- T SQL
- VS Code
- .NET
section_names:
- ai
- azure
- dotnet
- github-copilot
---
Microsoft Fabric Blog details how GitHub Copilot brings AI-powered coding and query optimization to SQL developers directly in SSMS, VS Code, and Fabric, offering chat-driven help, inline completions, and security-focused workflows.<!--excerpt_end-->

# AI-Powered Assistants in SSMS, VS Code, and Fabric: GitHub Copilot for SQL Developers

*Author: Microsoft Fabric Blog*

Whether you’re new to T-SQL or a seasoned pro, writing and managing SQL has its tedious moments. Thankfully, AI-powered assistants are now available wherever you write SQL—across SQL Server Management Studio (SSMS), Visual Studio Code (VS Code), and the Microsoft Fabric portal. This article walks through the capabilities, setup, and practical advantages of GitHub Copilot for SQL development.

## Where Can You Use AI-Powered SQL Assistance?

- **SQL Server Management Studio (SSMS)**
- **Visual Studio Code with the MSSQL Extension**
- **Microsoft Fabric portal Query Editor**

### GitHub Copilot in SSMS

In SSMS 22, GitHub Copilot is available as a preview feature:

- **Inline code completions:** As you type, AI autocompletes T-SQL snippets and queries.
- **Chat and menu integration:** Chat directly with Copilot to generate queries, explain code, or optimize slow queries. You can even attach execution plans and receive actionable recommendations.
- **Streamlined workflows:** Tasks like documenting code, understanding inherited stored procedures, or troubleshooting performance become much faster with Copilot support.

Watch the [official Copilot in SSMS demo playlist](https://www.youtube.com/playlist?list=PL3EZ3A8mHh0wew8yf4hodr3PQDxDh7sLt) for walkthroughs.

### GitHub Copilot in VS Code (MSSQL Extension)

Copilot integration in the MSSQL extension for VS Code offers:

- **Chat and inline suggestions** grounded in your connected database context
- **Agent Mode**, which automates multi-step database workflows (such as schema scaffolding or query debugging)
- **Direct database operations:** Issue queries or generate scripts in chat mode, with every action requiring your explicit approval
- **Slash commands and workflow presets** for common tasks

Explore Copilot demos with the [MSSQL extension GitHub Copilot playlist](https://www.youtube.com/playlist?list=PL3EZ3A8mHh0xv6HXxjln5U6JwqSDrh2wG).

### Copilot in Microsoft Fabric SQL

- **Seamless Copilot features:** Copilot in SSMS and VS Code work directly with Fabric SQL databases.
- **Query Editor integration:** Fabric’s browser-based Query Editor also has built-in Copilot features for code actions, chat assistance, and optimization.
- **Consistent AI experience:** Whether in desktop tools or the Fabric portal, users get the same AI-powered guidance.

Azure Copilot is also available in the Azure portal for Azure SQL resources.

## Security, Privacy, and Common Questions

- **No prompt data is used for model training.**
- **Destructive commands** (e.g., writes) require explicit user approval. SSMS Copilot is currently "Ask mode" (read-only); Agent mode for writes is coming soon and will require confirmation. VS Code supports Agent mode with the same safeguards.
- **Prompts are encrypted**. For full details, refer to the [GitHub Copilot Trust Center](https://copilot.github.trust.page/) and [Fabric privacy/security docs](https://learn.microsoft.com/fabric/fundamentals/copilot-privacy-security).

## Get Started

- **SSMS:** Download [SQL Server Management Studio 22](https://learn.microsoft.com/ssms/install/install) and enable the Copilot workload via the Visual Studio Installer.
- **VS Code:** Install the [MSSQL extension](https://aka.ms/vscode-mssql) and sign in with your Copilot subscription.
- **Fabric:** Connect to your SQL database in Fabric from SSMS, VS Code, or use Copilot directly in the Fabric portal's Query Editor.

For advanced learning, join [SQLCon](https://sqlcon.us/) or subscribe to the [Azure SQL YouTube channel](https://aka.ms/AzureSQLYT).

## Conclusion

GitHub Copilot and related AI assistants make T-SQL development more productive and approachable for every SQL environment supported by Microsoft—SSMS, VS Code, and Fabric. Both new and experienced developers can leverage Copilot to accelerate writing, debugging, and optimizing SQL, with strong safeguards for privacy and control.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/no-more-excuses-ai-powered-assistants-are-in-ssms-vs-code-and-fabric/)
