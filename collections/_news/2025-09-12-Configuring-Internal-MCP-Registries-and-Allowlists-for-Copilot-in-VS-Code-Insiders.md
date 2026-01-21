---
external_url: https://github.blog/changelog/2025-09-12-internal-mcp-registry-and-allowlist-controls-for-vs-code-insiders
title: Configuring Internal MCP Registries and Allowlists for Copilot in VS Code Insiders
author: Allison
feed_name: The GitHub Blog
date: 2025-09-12 18:31:57 +00:00
tags:
- Allowlist Enforcement
- Azure API Center
- Copilot
- Copilot Business
- Copilot Enterprise
- Eclipse
- Enterprise Policy
- Improvement
- JetBrains
- MCP Registry
- Platform Governance
- Policy Governance
- Registry Only Policy
- VS
- VS Code
- VS Code Insiders
- Xcode
section_names:
- ai
- devops
- github-copilot
---
Allison explains how to set up and test MCP registries and allowlist enforcement for GitHub Copilot in VS Code Insiders, outlining enterprise governance features, setup steps, and rollout plans.<!--excerpt_end-->

# Internal MCP Registry and Allowlist Controls for VS Code Insiders

Enterprise and organization administrators can now manage how Model Context Protocol (MCP) servers are accessed via Copilot, starting with VS Code Insiders. This capability enables tighter control over which MCP servers are used, enhancing security and compliance across development environments.

## Understanding MCP Registries and Allowlists

- **MCP Registries**: These are catalogs of approved MCP servers. Administrators upload a registry URL to their Copilot policies page, making these servers discoverable and installable for users.
- **Allowlisting**: Pairing registries with a 'Registry only' policy ensures only servers in the registry are usable, blocking all others at runtime. This is like having a strict approved vendor list for Copilot add-ins.

## Registry Hosting Options

- **Azure API Center**: Leverage Microsoft's [Azure API Center](https://learn.microsoft.com/azure/api-center/overview) for dynamic, enterprise-grade registry management.
- **Static Hosting**: Host a [specification-compliant JSON file](https://spec.modelcontextprotocol.io/specification/registry/) via GitHub Pages, S3, or any HTTPS endpoint.

## Current and Upcoming Rollout

- **VS Code Insiders**: Supports both registry display and strict runtime enforcement.
- **VS Code Stable**: Currently shows registries but does not enforce policies—full enforcement is coming soon.
- **Future Integration**: October will bring enforcement and stricter server validation to VS Code Stable, Visual Studio, and later to Copilot agents for JetBrains, Eclipse, and Xcode.

## Policy Options

- **Allow all (default)**: Registry entries serve as recommendations; any MCP server is usable.
- **Registry only**: Only allow registry-listed servers; others are blocked with a clear policy message.
- **Known Limitation**: Enforcement currently only validates server IDs; stricter validation (including command paths, arguments, environment variables) arrives in October.

## Getting Started

- Features are live for Copilot Business and Copilot Enterprise customers.
- Enterprise policies trump organization-level rules if both apply.
- Begin testing in VS Code Insiders to prep for broader Copilot environment support.
- Full setup instructions and registry format details: [Configure MCP server access for your organization or enterprise](https://gh.io/mcp-registry-allow-lists).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-12-internal-mcp-registry-and-allowlist-controls-for-vs-code-insiders)
