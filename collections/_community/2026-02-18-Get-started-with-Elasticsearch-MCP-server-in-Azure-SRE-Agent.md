---
layout: "post"
title: "Get started with Elasticsearch MCP server in Azure SRE Agent"
description: "This community post provides a practical, detailed walkthrough for integrating the Elasticsearch MCP server with Azure SRE Agent. It covers step-by-step instructions on connecting Azure SRE Agent to Elastic Cloud or self-hosted Elasticsearch (version 9.2.0+), including authentication setup, connector configuration, subagent creation, tool assignment, and testing. The post emphasizes safe practices for querying observability data, troubleshooting steps, and highlights the operational capabilities available to Azure SREs through this integration."
author: "dbandaru"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/get-started-with-elasticsearch-mcp-server-in-azure-sre-agent/ba-p/4492896"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-02-18 16:41:52 +00:00
permalink: "/2026-02-18-Get-started-with-Elasticsearch-MCP-server-in-Azure-SRE-Agent.html"
categories: ["Azure", "DevOps"]
tags: ["Agent Builder MCP", "API Key Authentication", "Azure", "Azure SRE Agent", "Community", "DevOps", "DevOps Integration", "Elastic Cloud", "Elasticsearch", "ESQL", "Incident Diagnosis", "Index Management", "Kibana", "Metrics Monitoring", "Observability", "Query DSL", "Streaming HTTP", "Subagent Configuration"]
tags_normalized: ["agent builder mcp", "api key authentication", "azure", "azure sre agent", "community", "devops", "devops integration", "elastic cloud", "elasticsearch", "esql", "incident diagnosis", "index management", "kibana", "metrics monitoring", "observability", "query dsl", "streaming http", "subagent configuration"]
---

dbandaru explains how to integrate Elasticsearch MCP server with Azure SRE Agent, providing detailed steps for secure authentication, connector setup, tool assignment, and best practices for querying and monitoring Elasticsearch clusters.<!--excerpt_end-->

# Get Started with Elasticsearch MCP Server in Azure SRE Agent

The Elasticsearch MCP server allows your Azure SRE Agent to interact directly with Elasticsearch clusters via natural language. This enables conversational querying of logs, metrics analysis, cluster health checks, and troubleshooting—streamlining observability tasks for Site Reliability Engineers.

---

## Key Capabilities

- **Search**: Execute search queries using Elasticsearch Query DSL
- **ES|QL**: Run ES|QL queries for advanced data exploration
- **Mappings**: Retrieve field mappings for indices
- **Cluster Health**: Check shard status and overall cluster health
- **Index Management**: List and explore available indices

---

## Prerequisites

- Azure SRE Agent resource setup ([sre.azure.com](https://sre.azure.com))
- Elasticsearch cluster (Elastic Cloud or self-hosted, v9.2.0+)
- Kibana with Agent Builder enabled (Elastic 9.2.0+ or Serverless)
- API key with at least read permissions to your indices

---

## Step 1: Get your Elasticsearch Credentials

1. Log in to Elastic Cloud or your Kibana instance ([`https://{your-kibana-url}`])
2. Go to **Management > API Keys**
3. Click **Create API key**, provide a relevant name (e.g., `azure-sre-agent-mcp`)
4. Set minimum required permissions (read access)
5. Create and securely copy the API key
6. Note your Kibana URL for later configuration (e.g., `https://my-deployment.kb.us-east-1.aws.elastic.cloud`)

---

## Step 2: Add the MCP Connector

1. Access your Azure SRE Agent at [sre.azure.com](https://sre.azure.com)
2. Select your agent and find **Builder > Connectors**
3. Click **Add connector** and choose **MCP Server (User provided connector)**
4. Configure fields as follows:

   - **Name**: `elasticsearch-mcp`
   - **Connection type**: Streamable-HTTP
   - **URL**: `https://{KIBANA_URL}/api/agent_builder/mcp`
   - **Authentication method**: Custom headers
   - **Header name**: `Authorization`
   - **Header value**: `ApiKey {your-api-key}`

5. Review and add the connector.

**Equivalent mcp.json configuration:**

```json
{
  "mcpServers": {
    "elasticsearch-mcp": {
      "url": "https://{KIBANA_URL}/api/agent_builder/mcp",
      "transport": "streamable-http",
      "headers": {
        "Authorization": "ApiKey {your-api-key}"
      }
    }
  }
}
```

---

## Step 3: Create a Subagent and Add Tools

1. Go to **Builder > Subagent builder** in Azure SRE Agent
2. Click **+ Create** and switch to **YAML** tab
3. Paste a configuration like the following:

```yaml
api_version: azuresre.ai/v1
kind: AgentConfiguration
spec:
  name: Elasticsearch
  system_prompt: >
    Goal: Provide Azure SREs a unified interface to retrieve observability data via ES|QL...
    (See your content for the full provided YAML example.)
  tools:
    - Elasticsearch_platform_core_execute_esql
    - Elasticsearch_platform_core_generate_esql
    - Elasticsearch_platform_core_get_document_by_id
    - Elasticsearch_platform_core_get_index_mapping
    - Elasticsearch_platform_core_index_explorer
    - Elasticsearch_platform_core_list_indices
    - Elasticsearch_platform_core_search
handoff_description: ...
agent_type: Autonomous
enable_skills: false
```

1. Save the subagent.
2. In the subagent editor, navigate to the **Tools** tab and **+ Add tools** like `list_indices`, `get_mappings`, `search`, `esql`, `get_shards`.
3. Attach the tools, then save these changes.

> **Tip:** Attaching tools is mandatory for your subagent to interact with Elasticsearch via the MCP server.

---

## Step 4: Test the Integration

Open a new chat with your Azure SRE Agent and try prompts such as:

- *"List all indices in my Elasticsearch cluster"*
- *"What are the mappings for the logs-* index?"*
- *"Search for errors in the last hour across all logs indices"*
- *"Run an ES|QL query to find the top 10 error types"*
- *"Show me shard information for my cluster"*

---

## Available Tools and Their Uses

- **list_indices**: List all Elasticsearch indices
- **get_mappings**: Retrieve field mappings
- **search**: Perform standard search queries
- **esql**: Run advanced ES|QL queries
- **get_shards**: Fetch shard status for indices

---

## Troubleshooting and Resources

| Issue                                | Solution                                                      |
|--------------------------------------|---------------------------------------------------------------|
| Subagent missing Elasticsearch tools | Add tools post-subagent creation                              |
| Connection refused                   | Verify Kibana URL, access, and HTTPS usage                    |
| 401 Unauthorized                     | Check API key validity and permissions                        |
| 403 Forbidden                        | Ensure Agent Builder is enabled                               |
| Tools not appearing                  | Refresh after connector addition                              |
| SSL/TLS errors                       | Confirm Kibana URL uses HTTPS                                 |

---

## Related Resources

- [Elasticsearch MCP Server (GitHub)](https://github.com/elastic/mcp-server-elasticsearch)
- [Elastic Agent Builder Documentation](https://ela.st/agent-builder-docs)
- [Elastic Agent Builder MCP Endpoint](https://ela.st/agent-builder-mcp)
- [MCP Integration Overview (Microsoft)](https://learn.microsoft.com/azure/sre-agent/mcp-integration)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/get-started-with-elasticsearch-mcp-server-in-azure-sre-agent/ba-p/4492896)
