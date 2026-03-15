---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/mcp-driven-azure-sre-for-databricks/ba-p/4494630
title: Automate Databricks Compliance and Incident Response with Azure SRE Agent and Model Context Protocol
author: varghesejoji
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-02-12 16:56:21 +00:00
tags:
- AI
- AI Operations
- Alert Integration
- Azure
- Azure Databricks
- Azure SRE Agent
- Best Practices
- Community
- Compliance Automation
- Container Apps
- DevOps
- FastMCP
- Governance
- Incident Response
- MCP
- Operations Automation
- REST API
- Root Cause Analysis
- Security
- Self Healing
section_names:
- ai
- azure
- devops
- security
---
varghesejoji demonstrates how Azure SRE Agent and the Model Context Protocol can automate compliance and incident response for Azure Databricks, providing step-by-step deployment guidance and real-world operational scenarios.<!--excerpt_end-->

# Automate Databricks Compliance and Incident Response with Azure SRE Agent and MCP

## Overview

Azure SRE Agent is an AI-driven operations assistant designed for incident response, compliance, and governance in cloud environments. By leveraging the Model Context Protocol (MCP), it connects seamlessly to Azure Databricks, enabling:

- **Proactive compliance checks** against best practices
- **Automated incident investigation and root cause analysis**
- Integration with service management and alerting platforms (PagerDuty/ServiceNow)

## Architecture

The solution is architected with the following components:

- **Azure SRE Agent** orchestrates knowledge prompts and operational skills.
- **MCP Server** (FastMCP) containerized in Azure Container Apps exposes Databricks REST API endpoints.
- **Databricks Workspace** is the operational target for compliance and troubleshooting workflows.

The agent issues high-level intents, which are converted into MCP tool calls (e.g., list_clusters, get_job), and the MCP server relays these as REST API requests to Databricks. Structured results, evidence, and remediation steps are automatically aggregated.

## Deployment Steps

1. **Deploy the MCP Server**
   - Run FastMCP as a container in Azure Container Apps
   - Configure connection settings for your Databricks workspace
   - Reference deployment instructions: [GitHub repository](https://github.com/jvargh/sre-a/tree/main/databricks-srea)
2. **Configure the Azure SRE Agent**
   - Create an MCP connector (streamable-http transport)
   - Upload the Best Practices document as a Knowledge Base: [AZURE_DATABRICKS_BEST_PRACTICES.md](https://github.com/jvargh/sre-a/blob/main/databricks-srea/AZURE_DATABRICKS_BEST_PRACTICES.md)
   - Define Ops Skills by adding operational runbooks: [DATABRICKS_OPS_RUNBOOK_SKILL.md](https://github.com/jvargh/sre-a/blob/main/databricks-srea/DATABRICKS_OPS_RUNBOOK_SKILL.md)
   - Deploy the agent YAML for integrated orchestration: [Databricks_MCP_Agent.yaml](https://github.com/jvargh/sre-a/blob/main/databricks-srea/Databricks_MCP_Agent.yaml)
3. **Integrate with Alerting**
   - Connect incident management tools like PagerDuty or ServiceNow via webhook
   - Enable auto-remediation for common issues

## Proactive Compliance: Best Practices Validation

**Example Prompt:**
> @Databricks_MCP_Agent: Validate the Databricks workspace for best practices compliance and provide a summary, detailed findings, and concrete remediation steps.

**Agent Actions:**

- Audits compute clusters and Unity Catalog configurations
- Reviews job setups and governance via REST API
- Cross-references configuration with uploaded Knowledge Base
- Provides an actionable compliance report and remediation plan

**Benefits:**

- Drastically reduce review time (5 min vs 2-3 hours)
- Consistent validation across all workspaces
- Actionable outputs with concrete code or configuration fixes

## Reactive Incident Response

### Example 1: Job Failure (Non-Zero Exit Code)

- Diagnoses root cause from failing job and run history
- Analyzes error logs (e.g., sys.exit(1) in notebook code)
- Provides job IDs, evidence, and specific remediation guidance (add retry policy, fix code errors)
- Typical resolution reduced to 3-5 minutes

### Example 2: Task Notebook Exception

- Detects recurring job failures due to notebook errors (timeouts, null partitions, etc.)
- Retrieves run history and code excerpts to pinpoint error source
- Details concrete fixes (exception handling, retry logic)
- Cuts investigation time from 45+ minutes to under 10 minutes

## Operational Impact

| Metric                        | Before     | After     | Improvement |
|-------------------------------|------------|-----------|-------------|
| Compliance review time        | 2–3 hours  | 5 min     | 95%         |
| Job failure investigation     | 30–45 min  | 3–8 min   | 85%         |
| On-call alerts (per shift)    | 4–6        | 1–2       | 70%         |

## Key Benefits

- **Continuous compliance monitoring**
- **Automated best practice validation**
- **Faster incident response with evidence-based analysis**
- **Self-healing operational workflows**
- **Simple deployments via containerized FastMCP**

## Resources

- [Deployment Guide](https://github.com/jvargh/sre-a/blob/main/databricks-srea/QUICKSTART_INSTALL.md)
- [Subagent Configuration YAML](https://github.com/jvargh/sre-a/blob/main/databricks-srea/Databricks_MCP_Agent.yaml)
- [Best Practices Knowledge Base](https://github.com/jvargh/sre-a/blob/main/databricks-srea/AZURE_DATABRICKS_BEST_PRACTICES.md)
- [Ops Skill Runbook](https://github.com/jvargh/sre-a/blob/main/databricks-srea/DATABRICKS_OPS_RUNBOOK_SKILL.md)
- [Validation Script](https://github.com/jvargh/sre-a/blob/main/databricks-srea/mcp_validate.py)
- [Azure SRE Agent Documentation](https://learn.microsoft.com/en-us/azure/sre-agent/)
- [Model Context Protocol Specification](https://modelcontextprotocol.io/specification/)
- [Blogs & Support](https://techcommunity.microsoft.com/tag/azure%20sre%20agent?nodeId=board%3AAppsonAzureBlog)

**Questions?** Open an issue on [GitHub](https://github.com/jvargh/sre-a/tree/main/databricks-srea) or contact the Azure SRE team.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/mcp-driven-azure-sre-for-databricks/ba-p/4494630)
