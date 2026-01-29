---
external_url: https://github.blog/changelog/2025-11-18-internal-mcp-registry-and-allowlist-controls-for-vs-code-stable-in-public-preview
title: Internal MCP Registry and Allowlist Controls for Copilot in VS Code and Visual Studio
author: Allison
feed_name: The GitHub Blog
date: 2025-11-18 16:39:20 +00:00
tags:
- Allowlist Policy
- Azure API Center
- Configuration
- Copilot
- Copilot Business
- Copilot Enterprise
- Developer Tools
- Enforcement
- Enterprise Governance
- Improvement
- Internal Registry
- MCP Registry
- MCP Server
- MCP Specification
- Platform Governance
- Policy Management
- Remote Server
- Security Policy
- VS
- VS Code
- AI
- DevOps
- GitHub Copilot
- News
section_names:
- ai
- devops
- github-copilot
primary_section: github-copilot
---
Allison explains how GitHub Copilot enterprise customers can now configure MCP registries and enforce allowlist policies in VS Code, with early support in Visual Studio. This marks a key step in platform governance for organizations.<!--excerpt_end-->

# Internal MCP Registry and Allowlist Controls for Copilot in VS Code and Visual Studio

GitHub Copilot has introduced new governance controls for enterprise and organization administrators, allowing the configuration of internal MCP (Model Context Protocol) registries and the enforcement of allowlist policies. These features are now publicly available in VS Code Stable and partially supported in Visual Studio, giving enterprises more control over the AI coding assistant’s server access and compliance.

## Key Features

- **MCP Registries:** Administrators can upload a registry URL to GitHub Copilot policies, making approved MCP servers easily discoverable and installable in compatible host applications.
- **Allowlist Enforcement:** With the registry-only policy, any MCP server not listed in the registry is blocked from use in VS Code at runtime.
- **Governance Messaging:** Developers see clear messages indicating which organization or enterprise is enforcing registry restrictions.

## Registry Setup Approaches

- **Azure API Center:** Use Microsoft’s managed service for dynamic registry management and enterprise governance.
- **Self-hosted Options:** Fork the open-source MCP Registry or create a custom implementation that complies with specification v0.1 and proper CORS support.

## VS Code vs Visual Studio Support

- **VS Code Stable:**
  - Registry servers display for discovery in MCP servers panel.
  - Registry-only enforcement actively blocks non-listed servers.
  - Full messaging for policy status and organization enforcement.
- **Visual Studio (latest preview):**
  - Registry discovery is available, but enforcement is coming in a future release.
  - All servers are currently allowed, regardless of policy.

## Policy Options

- **Allow all (default):** Registry servers are recommendations but not enforced.
- **Registry only:** Only listed servers are permitted, and others are blocked.
- **Local vs Remote Servers:** Remote server enforcement uses both name and URL for validation, while local enforcement only checks name (potentially editable by the user). Enterprises with strict requirements should limit to remote server usage.

## Getting Started

- Available to Copilot Business and Copilot Enterprise customers.
- Documentation and setup guidance can be found here: [Configure an MCP registry for your organization or enterprise](https://docs.github.com/copilot/how-tos/administer-copilot/manage-mcp-usage/configure-mcp-server-access).
- See [Support surfaces for MCP usage in your company](https://docs.github.com/en/copilot/concepts/mcp-management#supported-surfaces) for updated host application support status.

## Summary

This update enables large-scale governance and compliance features for Copilot users across organizations, improving visibility and security. Visual Studio will offer further allowlist enforcement capabilities in upcoming releases.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-18-internal-mcp-registry-and-allowlist-controls-for-vs-code-stable-in-public-preview)
