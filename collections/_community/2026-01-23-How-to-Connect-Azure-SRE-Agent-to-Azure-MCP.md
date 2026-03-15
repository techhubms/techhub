---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-to-connect-azure-sre-agent-to-azure-mcp/ba-p/4488905
title: How to Connect Azure SRE Agent to Azure MCP
author: dbandaru
feed_name: Microsoft Tech Community
date: 2026-01-23 20:35:55 +00:00
tags:
- Access Control
- AI Integration
- Automation
- Azure MCP
- Azure Portal
- Azure Resources
- Azure Security
- Azure SRE Agent
- Cloud Infrastructure
- Identity Authentication
- Managed Identity
- MCP
- RBAC
- Resource Management
- Subagent Builder
- AI
- Azure
- DevOps
- Security
- Community
section_names:
- ai
- azure
- devops
- security
primary_section: ai
---
dbandaru presents a practical guide on connecting the Azure SRE Agent to Azure MCP, with step-by-step instructions for secure configuration, managed identity, and automation best practices.<!--excerpt_end-->

# How to Connect Azure SRE Agent to Azure MCP

Connecting the Azure SRE Agent to Azure MCP (Model Context Protocol) allows you to extend your agent's capabilities with pre-built Azure-native tools for resource management and automation. This guide covers setting up the connection, handling managed identity, implementing secure practices, and testing the configuration.

## Prerequisites

- An active Azure subscription
- An Azure SRE Agent deployed and accessible in your Azure environment

## Step 1: Add an MCP Connector Using the Portal UI

1. **Navigate to your SRE Agent resource on the Azure Portal.**
2. Under **Settings**, select **Connectors**.
3. Click **+ Add MCP Connector**.
4. In the configuration panel:
    - **Name:** Choose a descriptive name (e.g., "Azure MCP Server").
    - **Connection Type:** Select **stdio**. This enables communication between the SRE Agent and MCP server using standard input/output.
    - **Arguments:** Input the following:

      ```
      npx, -y, @azure/mcp, server, start
      ```

    - **Optional:** To customize tool availability:
      - Expose only tools in the subscription namespace: `--namespace, subscription`
      - Expose all tools without namespace wrapping: `--mode, all`

## Step 2: Configure Managed Identity

1. **Select a managed identity** in the Managed Identity dropdown. MCP will use this identity for API calls.
2. Set required environment variables:
   - `AZURE_CLIENT_ID`: The client ID of the managed identity.
   - `AZURE_TOKEN_CREDENTIALS`: Use `managedidentitycredential` to ensure only managed identity authentication.
3. **Assign appropriate Azure RBAC roles** to the managed identity (e.g., Reader access to subscriptions or resource groups for listing resources).
4. Optional: Review [Azure MCP documentation](https://github.com/Azure/azure-mcp) for additional environment variables.

## Step 3: Create a Subagent Using Subagent Builder

1. In the Subagent Builder, create a new subagent (e.g., "Azure Resource Manager").
2. Provide clear usage instructions for the MCP tools:

   ```
   You are an Azure resource management assistant. Use the Azure MCP tools to:
   - List Azure subscriptions the user has access to
   - Query resources across subscriptions
   - Retrieve resource details and configurations
   Always confirm the subscription context before performing operations.
   ```

3. Click **Choose tools** and add the configured MCP connector to the subagent's tools.

## Step 4: Test Your Configuration

1. Open the subagent playground.
2. Ask a question to trigger an MCP tool (e.g., "List my Azure subscriptions").
3. Verify that the tool call succeeds and the response contains the expected data.

## Security Considerations

### Managed Identity Access Control

- Azure MCP with SRE Agent relies on managed identity for downstream API access.
- **Users with access to the SRE Agent inherit the managed identity’s permissions**. This can lead to excessive privilege if not scoped correctly.

### Best Practices

1. **Least Privilege:** Grant only necessary permissions to managed identities.
2. **Scope Roles Narrowly:** Prefer resource group-level assignments over subscription-wide roles.
3. **Audit Regularly:** Review managed identity assignments to prevent privilege creep.
4. **Segregate Agents:** Use separate SRE Agents and identities for different groups/use cases.

## Summary

Following these steps, you can securely connect Azure SRE Agent to Azure MCP, automating Azure operations with managed identity authentication and minimizing security risks through role-based access control.

## Additional Resources

- [Azure MCP Documentation](https://github.com/Azure/azure-mcp)
- [Azure SRE Agent Documentation](https://learn.microsoft.com/azure/sre-agent)
- [Managed Identities for Azure Resources](https://learn.microsoft.com/azure/active-directory/managed-identities-azure-resources/)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-to-connect-azure-sre-agent-to-azure-mcp/ba-p/4488905)
