---
layout: "post"
title: "Configuring Internal MCP Registries and Allowlists for Copilot in VS Code Insiders"
description: "This news post details how enterprise and organization administrators can configure Model Context Protocol (MCP) registries and test allowlist enforcement for GitHub Copilot across environments, focusing on VS Code Insiders. It covers registry hosting options, policy enforcement mechanisms, rollout timelines, and future enhancements for security and governance in Copilot deployments."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-09-12-internal-mcp-registry-and-allowlist-controls-for-vs-code-insiders"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-09-12 18:31:57 +00:00
permalink: "/news/2025-09-12-Configuring-Internal-MCP-Registries-and-Allowlists-for-Copilot-in-VS-Code-Insiders.html"
categories: ["AI", "DevOps", "GitHub Copilot"]
tags: ["AI", "Allowlist Enforcement", "Azure API Center", "Copilot", "Copilot Business", "Copilot Enterprise", "DevOps", "Eclipse", "Enterprise Policy", "GitHub Copilot", "Improvement", "JetBrains", "MCP Registry", "News", "Platform Governance", "Policy Governance", "Registry Only Policy", "VS", "VS Code", "VS Code Insiders", "Xcode"]
tags_normalized: ["ai", "allowlist enforcement", "azure api center", "copilot", "copilot business", "copilot enterprise", "devops", "eclipse", "enterprise policy", "github copilot", "improvement", "jetbrains", "mcp registry", "news", "platform governance", "policy governance", "registry only policy", "vs", "vs code", "vs code insiders", "xcode"]
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
