---
layout: "post"
title: "Dynamic Azure DevOps MCP Server for Seamless Context Switching in Claude Code"
description: "This post by OkAdministration2514 introduces a custom MCP server designed to enhance Claude Code workflows with Azure DevOps. The tool automates context switching for multiple Azure DevOps projects by detecting the active directory and updating authentication credentials on the fly. Features include local config management, robust security practices, and a comprehensive set of DevOps utilities, enabling developers and consultants to streamline multi-client solutions with improved efficiency, workflow, and project isolation."
author: "OkAdministration2514"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/azuredevops/comments/1m91urt/i_built_a_dynamic_azure_devops_mcp_server_for/"
viewing_mode: "external"
feed_name: "Reddit Azure DevOps"
feed_url: "https://www.reddit.com/r/azuredevops/.rss"
date: 2025-07-25 15:14:49 +00:00
permalink: "/2025-07-25-Dynamic-Azure-DevOps-MCP-Server-for-Seamless-Context-Switching-in-Claude-Code.html"
categories: ["Azure", "DevOps", "Coding", "Security"]
tags: [".azure Devops.json", "Authentication Context Switching", "Azure", "Azure DevOps", "Build Management", "Claude Code", "Coding", "Community", "Consulting Workflows", "Credential Isolation", "DevOps", "DevOps Automation", "Directory Detection", "GitHub Secret Scanning", "MCP SDK", "MCP Server", "Multi Project Workflow", "Node.js", "PAT Tokens", "Pipeline Automation", "Repository Management", "Security", "Security Best Practices", "TypeScript", "Workflow Optimization"]
tags_normalized: ["dotazure devopsdotjson", "authentication context switching", "azure", "azure devops", "build management", "claude code", "coding", "community", "consulting workflows", "credential isolation", "devops", "devops automation", "directory detection", "github secret scanning", "mcp sdk", "mcp server", "multi project workflow", "nodedotjs", "pat tokens", "pipeline automation", "repository management", "security", "security best practices", "typescript", "workflow optimization"]
---

OkAdministration2514 details how they built a dynamic MCP server that empowers Claude Code users to seamlessly switch Azure DevOps contexts by project directory, offering robust DevOps automation for multi-client work.<!--excerpt_end-->

# Dynamic Azure DevOps MCP Server for Seamless Context Switching in Claude Code

**Author:** OkAdministration2514

## TL;DR

Created a custom MCP server enabling Claude Code users to work across multiple Azure DevOps projects without manual re-authentication. The tool leverages smart directory detection and local config files to auto-switch contexts, significantly improving multi-client DevOps workflows.

## The Problem

When using Claude Code with Azure DevOps, developers working on multiple projects encounter static authentication limitations—typically restricted to a single Azure DevOps org or project at a time due to environment variable settings. Switching between projects requires restarts and config changes, causing workflow interruptions.

## The Solution: Dynamic Context Switching

The solution is `u/wangkanai/devops-mcp`, an MCP server that:

- Detects the active project directory automatically
- Uses project-specific `.azure-devops.json` files to store organization and PAT credentials
- Switches Azure DevOps authentication contexts instantly as you move directories, with no manual config edits or restarts required

### Key Features

- **Zero-Configuration Directory Switching:**
  - Simply changing directories updates your Azure DevOps context automatically
- **Comprehensive DevOps Tooling (8 total):**
  - Work item management (create/query with full metadata)
  - Repository and build management
  - Pipeline triggering/monitoring
  - Pull request operations
  - Dynamic context reporting
- **Security Focus:**
  - PAT tokens stored locally only
  - Credentials never committed to git
  - Credential isolation per project
  - Compliant with GitHub secret scanning policies

### How It Works

1. Each project directory contains a `.azure-devops.json` file with organization-specific credentials and metadata.
2. The MCP server detects directory changes and loads context from the respective config file.
3. Authentication is switched on-the-fly without restarts, maintaining a seamless workflow across multiple clients or projects.

#### Example Directory Usage

```bash
cd ~/projects/company-a   # Switches to Company A's Azure DevOps context
cd ~/projects/company-b   # Switches to Company B's Azure DevOps context
```

#### Example Config File (.azure-devops.json)

```json
{
  "organizationUrl": "https://dev.azure.com/your-org",
  "project": "YourProject",
  "pat": "your-pat-token",
  "description": "Project-specific Azure DevOps config"
}
```

### Installation

Quick setup via Claude Code:

```sh
claude mcp add devops-mcp -- npx u/wangkanai/devops-mcp
```

Add a `.azure-devops.json` for each project as shown above.

### Real-World Impact

- **90% Time Reduction** in context switching
- **Zero Authentication Errors** when moving between projects
- **Simplified Consulting and Multi-client Workflows**
- **Enhanced Security**: Credential isolation, local-only storage

### Tech Stack & Metrics

- **Node.js + TypeScript** with MCP SDK
- **>95% Test Coverage**
- **Sub-200ms Context Switching Overhead**
- **Production Ready**: Robust error handling & fallbacks

### Why This Matters for DevOps

This approach removes one of the main productivity bottlenecks in multi-organizational DevOps work, making context switching transparent and secure. Especially valuable for consultants and developers managing numerous client projects.

- **GitHub:** [wangkanai/devops-mcp](https://github.com/wangkanai/devops-mcp)
- **NPM:** `@wangkanai/devops-mcp`

Questions or need implementation help? Comment below!

This post appeared first on "Reddit Azure DevOps". [Read the entire article here](https://www.reddit.com/r/azuredevops/comments/1m91urt/i_built_a_dynamic_azure_devops_mcp_server_for/)
