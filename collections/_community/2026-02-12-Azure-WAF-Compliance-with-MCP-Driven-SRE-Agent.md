---
layout: "post"
title: "Azure WAF Compliance with MCP-Driven SRE Agent"
description: "This guide details how the Azure SRE Agent, powered by Model Context Protocol (MCP), enables continuous enforcement of Azure Well-Architected Framework (WAF) compliance. It automates resource discovery, cross-checks findings against both WAF pillars and organizational standards, and generates actionable remediation steps, all integrated into Azure governance and DevOps workflows."
author: "varghesejoji"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-waf-compliance-with-mcp-driven-sre-agent/ba-p/4494687"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-12 23:51:58 +00:00
permalink: "/2026-02-12-Azure-WAF-Compliance-with-MCP-Driven-SRE-Agent.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Automated Remediation", "Azure", "Azure ARM API", "Azure CLI", "Azure Compliance", "Azure DevOps", "Azure Governance", "Azure Policy", "Azure Security", "Azure SRE Agent", "Community", "Cost Optimization", "DevOps", "IaC", "Managed Identity", "MCP", "Operational Excellence", "Resource Discovery", "Security", "Terraform", "WAF Pillars", "Well Architected Framework"]
tags_normalized: ["automated remediation", "azure", "azure arm api", "azure cli", "azure compliance", "azure devops", "azure governance", "azure policy", "azure security", "azure sre agent", "community", "cost optimization", "devops", "iac", "managed identity", "mcp", "operational excellence", "resource discovery", "security", "terraform", "waf pillars", "well architected framework"]
---

varghesejoji explores how the Azure SRE Agent uses the Model Context Protocol to enforce Azure Well-Architected Framework compliance at scale, enabling security, finance, and platform teams to identify and remediate risks efficiently.<!--excerpt_end-->

# Azure WAF Compliance with MCP-Driven SRE Agent

## Overview

Managing Azure governance at scale is challenging. Teams often resort to manual reviews across multiple subscriptions, leading to missed security gaps and wasted effort. The Azure SRE Agent addresses these challenges with an MCP-powered, AI-driven engine that unifies discovery, compliance assessment, and remediation within one workflow.

## Key Capabilities

- **Autonomous Discovery**: Automatically inventories all Azure resources across subscriptions using Azure MCP (Model Context Protocol), collecting metadata (types, locations, tags, security settings) within seconds.
- **Multi-Pillar WAF Assessment**: Validates every resource against the five Azure Well-Architected Framework (WAF) pillars—Reliability, Security, Cost Optimization, Operational Excellence, and Performance.
- **Org Standards Enforcement**: Cross-checks WAF assessments with your organization's policies from a knowledge base (e.g., org-practices.md), escalating findings as critical, warning, or informational, based on real business needs.
- **Automated Remediation**: For each gap, generates Azure CLI commands, Terraform snippets, and Portal instructions with quantified impact in terms of risk, cost, and compliance.

## Architecture

The agent integrates Azure Resource Graph and ARM APIs via MCP, applies WAF checks, references your org’s standards, and then outputs prioritized remediation steps—all orchestrated via a YAML configuration and a local MCP server running in Node.js Stdio mode.

## Deployment Steps

1. **Install and Launch MCP Server:**
   - Use `npx` to launch `[azure]/mcp` in server mode (`start --mode all`) with managed identity credentials.
   - Set environment variables (AZURE_CLIENT_ID, AZURE_TOKEN_CREDENTIALS).
2. **Connect Azure SRE Agent:**
   - Upload organizational standards (org-practices.md) into the Knowledge Base.
   - Deploy the agent YAML and link both MCP and the Knowledge Base.
3. **Run Compliance Validation:**
   - Prompt the agent for a subscription/resource group assessment.
   - Integrate with operations for scheduled scans and ticket export (Azure DevOps, ServiceNow).

## Autonomous Compliance Workflow

### 1. Resource Discovery

- Inventories VMs, storage, NSGs, Key Vaults, App Services, etc., automatically (no manual IDs).
- Uses MCP tool calls (e.g., `list_virtual_machines()`).

### 2. WAF Assessment

- Checks each resource across all WAF pillars:
  - **Reliability**: Availability zones, backup, health probes
  - **Security**: NSG rules, Key Vault configurations, Storage public access, HTTPS enforcement
  - **Cost Optimization**: VM sizing, orphaned resources, proper tagging, storage tiering
  - **Operational Excellence**: Monitoring/diagnostics, logging, tags for IaC
  - **Performance**: Right-sizing, storage/account performance, scaling

### 3. Organization Cross-Check

- Cross-references WAF findings with org policies.
- Assigns critical/warning/info severity (e.g., open RDP NSG = critical).
- Provides evidence and links to org standards for each finding.

### 4. Automated Remediation

- Generates exact CLI, PowerShell, or Terraform steps for every gap.
- Quantifies impact (e.g., cost savings, compliance improvement, risk reduction).

## Remediation Roadmap Example

**Deploy Immediately (Critical):**

- Enable TLS 1.2 on SQL Server
- Remove RDP from 0.0.0.0/0
- Enable Key Vault Purge Protection

**Short-Term (High Risk, 3 Days):**

- Configure storage policies for cost
- Install diagnostics agents

**Medium- and Long-Term:**

- Implement secret rotation, Azure Policy enforcement, and regularly scheduled scans using Logic Apps.

## Real-World Results

- **8 critical findings, 11 warnings** across multiple Azure services
- Automated remediation in 6 minutes (vs. 4-6 hours manual)
- Direct linkage of findings to org policies
- Continuous reporting and backlog integration with Azure DevOps

## Key Benefits

- End-to-end Azure governance automation
- Security, risk, and cost visibility
- Easy integration and scheduling (ad-hoc, scheduled, event-driven)
- Actionable remediations with impact quantification

## Resources

- [Agent Configuration YAML](https://github.com/jvargh/sre-a/blob/main/databricks-srea/Azure_WAF_Compliance_Agent.yaml)
- [Org Best Practices](https://github.com/jvargh/sre-a/blob/main/databricks-srea/org-practices.md)
- [Azure SRE Agent Documentation](https://learn.microsoft.com/en-us/azure/sre-agent/)
- [Azure SRE Agent Blogs](https://techcommunity.microsoft.com/tag/azure%20sre%20agent?nodeId=board%3AAppsonAzureBlog)
- [MCP Specification](https://modelcontextprotocol.io/specification/)
- [Azure Well-Architected Framework](https://learn.microsoft.com/en-us/azure/well-architected/)

## Author

**varghesejoji** – [profile](https://techcommunity.microsoft.com/users/varghesejoji/1393158)

## Questions?

Open an issue on [GitHub](https://github.com/jvargh/sre-a/issues) or contact the Azure SRE team.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-waf-compliance-with-mcp-driven-sre-agent/ba-p/4494687)
