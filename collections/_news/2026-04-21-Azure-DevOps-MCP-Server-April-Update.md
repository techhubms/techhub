---
title: Azure DevOps MCP Server April Update
primary_section: ai
tags:
- '#devops'
- AI
- Annotations
- Authentication
- Azure & Cloud
- Azure Boards
- Azure DevOps
- Community
- Destructive Tools
- DevOps
- Elicitations
- GitHub Copilot
- LLMs
- Local MCP Server
- MCP
- MCP Apps
- MCP Server
- Mcp.json
- News
- OpenWorld Tools
- Personal Access Token
- Public Preview
- Read Only Tools
- Remote MCP Server
- Telemetry
- Tooling
- Wiki
- WIQL
- Work Items
feed_name: Microsoft DevOps Blog
section_names:
- ai
- devops
external_url: https://devblogs.microsoft.com/devops/azure-devops-mcp-server-april-update/
author: Dan Hellem
date: 2026-04-21 14:12:23 +00:00
---

Dan Hellem summarizes April changes to the Azure DevOps MCP Server, covering new WIQL work item querying, remote-server tool consolidation and annotations, plus local-server updates like PAT authentication and experimental MCP Apps for packaging repeatable workflows.<!--excerpt_end-->

## Overview

This update covers improvements across both **local** and **remote** Azure DevOps **MCP Servers**, including new tooling for querying work items, safety metadata (annotations), toolset consolidation, authentication improvements, and an experimental “MCP Apps” workflow feature.

## Query work items with WIQL

A new tool is available:

- `wit_query_by_wiql`: construct and run **WIQL** queries for work items.

Remote MCP availability:

- For the **remote MCP**, the tool is currently limited to users with the **Insiders** feature enabled to maintain reliability and performance.
- As usage telemetry is gathered and performance is validated, the plan is to make it broadly available.

Learn more: https://learn.microsoft.com/en-us/azure/devops/mcp-server/remote-mcp-server?view=azure-devops#insiders

## Remote MCP Server

### Annotations

**MCP Annotations** are metadata tags intended to help LLMs use external tools safely and effectively by describing behavior, context, and risk.

The update implements annotations for:

- **read-only** tools
- **destructive** tools
- **openWorld** tools

The goal is clearer signaling of how tools operate for safer, more reliable interactions.

### Missing tools

There are still feature gaps between local and remote MCP servers.

Recently added remote support:

- `repo_get_file_content`
- `repo_list_directory`
- `repo_vote_pull_request`

More tools are expected in the coming weeks.

### Tool restructuring

Because Azure DevOps has a large surface area, the update starts consolidating related tools so clients/LLMs can work with a smaller, more focused set.

With the remote MCP Server in **public preview**, the team is making incremental improvements starting with wiki tools.

Wiki tool consolidation:

| New tool | Type | Actions / scope | Replaces |
| --- | --- | --- | --- |
| `wiki` | Read-only | `get_page`, `list_pages`, `list_wikis`, `get_wiki` | `wiki_get_page`, `wiki_get_page_content`, `wiki_list_pages`, `wiki_list_wikis`, `wiki_get_wiki` |
| `wiki_upsert_page` | Write | Single operation, no action parameter | `wiki_create_or_update_page` |
| `search_wiki` | Search |  | `search_wiki` |

Documentation updates will continue here: https://learn.microsoft.com/en-us/azure/devops/mcp-server/remote-mcp-server

## Local MCP Server

### Personal Access Token (PAT) support

**Personal access tokens** are now supported for authentication. This is intended to simplify integration with external services and clients such as **GitHub Copilot**.

Learn more: https://github.com/microsoft/azure-devops-mcp/blob/main/docs/GETTINGSTARTED.md#-personal-access-token-pat

### Elicitations

**Elicitations** are guided prompts designed to ensure required information is provided when performing tasks.

Example:

- Since most operations require a **project**, elicitation support was added for project selection across:
  - core
  - work
  - work items

Rollout note:

- Because demand hasn’t been strong yet, elicitations are being tested via a limited rollout to evaluate effectiveness.
- Feedback is requested via issues/comments.

### MCP Apps (Experimental)

**MCP Apps** are an experimental feature to package and execute common workflows directly within the MCP Server environment.

Why it matters:

- Reduces manual chaining of multiple tools
- Provides a more structured and repeatable workflow
- Reduces setup time and helps maintain consistency

Example tool:

- `mcp_app_my_work_item`: a self-contained work item experience to:
  - view assigned work items
  - filter results
  - open and edit work items

Image: [mcp blog 1 image](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2026/04/mcp-blog-1.webp)

How to try it:

1. Use the `mcp-apps-poc` branch:
   - https://github.com/microsoft/azure-devops-mcp/tree/mcp-apps-poc
2. Update `mcp.json` to include the `mcp-apps` domain:

```json
{
  "servers": {
    "ado": {
      "type": "stdio",
      "command": "mcp-server-azuredevops",
      "args": ["contsoso", "-d", "core", "work", "work-items", "mcp-apps"]
    }
  }
}
```

Feedback is requested to decide whether MCP Apps should move into the main local/remote MCP servers.

## Feedback

More updates are planned. Feedback can be provided by commenting on the post or creating an issue:

- https://github.com/microsoft/azure-devops-mcp/issues


[Read the entire article](https://devblogs.microsoft.com/devops/azure-devops-mcp-server-april-update/)

