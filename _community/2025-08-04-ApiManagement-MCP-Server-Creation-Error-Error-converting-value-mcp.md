---
layout: "post"
title: "ApiManagement MCP Server Creation Error: 'Error converting value mcp'"
description: "A user encounters an error while attempting to expose their API as an MCP server using Azure API Management. The error “Error converting value 'mcp' to type 'Microsoft.Azure.ApiManagement.Management.ControlPath'” is discussed, with references to Microsoft documentation and requests for community insights on potential bugs or misconfigurations."
author: "Competitive-Ad-5081"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/AZURE/comments/1mhgbww/apimanagement_create_mcp_server_from_api_error/"
viewing_mode: "external"
feed_name: "Reddit Azure"
feed_url: "https://www.reddit.com/r/azure/.rss"
date: 2025-08-04 15:19:58 +00:00
permalink: "/2025-08-04-ApiManagement-MCP-Server-Creation-Error-Error-converting-value-mcp.html"
categories: ["Azure"]
tags: ["API Exposure", "API Gateway", "Azure", "Azure API Management", "Community", "ControlPath", "Error Handling", "Input Parsing", "MCP Server", "Microsoft Documentation", "Microsoft.Azure.ApiManagement", "REST API"]
tags_normalized: ["api exposure", "api gateway", "azure", "azure api management", "community", "controlpath", "error handling", "input parsing", "mcp server", "microsoft documentation", "microsoft dot azure dot apimanagement", "rest api"]
---

Authored by Competitive-Ad-5081, this post details a technical issue faced when exposing an API as an MCP server via Azure API Management and seeks advice from the community.<!--excerpt_end-->

## Problem Statement

A user is attempting to expose their API as a Managed Control Plane (MCP) server using Azure API Management (APIM), but encounters a parsing error during the setup process.

#### Error Message

- **Error converting value 'mcp' to type 'Microsoft.Azure.ApiManagement.Management.ControlPath'**, line 2, position 15.
- The request to create the MCP server failed, citing one or more incorrect field values.

#### Context and Setup

- The user enabled the "AI Gateway early update group" 4 days prior to posting.
- Deployment region: Eastern US
- APIM tier: Basic

#### Troubleshooting Steps

- The user is following official Microsoft documentation: [Export REST APIs as MCP servers](https://learn.microsoft.com/es-es/azure/api-management/export-rest-mcp-server)
- They are unsure if the error is due to a service bug or a configuration issue, and asks the community if anyone has successfully set up MCP exposure for APIs.

#### Request to the Community

- Seeking insights from other users who have exposed APIs as MCP servers.
- Asking whether the error may be the result of a current bug in Azure API Management or a misconfiguration in the process.

#### Visuals

- Screenshots are referenced, showing the error in the Azure portal and configuration UI.

---
**References:**  

- [Reddit Post](https://www.reddit.com/r/AZURE/comments/1mhgbww/apimanagement_create_mcp_server_from_api_error/)  
- [Microsoft Documentation](https://learn.microsoft.com/es-es/azure/api-management/export-rest-mcp-server)

**Submitted by:** [Competitive-Ad-5081](https://www.reddit.com/user/Competitive-Ad-5081)

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mhgbww/apimanagement_create_mcp_server_from_api_error/)
