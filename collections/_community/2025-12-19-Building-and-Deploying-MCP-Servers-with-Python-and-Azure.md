---
layout: post
title: Building and Deploying MCP Servers with Python and Azure
author: Pamela_Fox
canonical_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/learn-how-to-build-mcp-servers-with-python-and-azure/ba-p/4479402
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-12-19 15:13:45 +00:00
permalink: /ai/community/Building-and-Deploying-MCP-Servers-with-Python-and-Azure
tags:
- API Security
- Application Insights
- Authentication
- Azure Container Apps
- Azure Functions
- Cloud Deployment
- Docker
- FastMCP
- Langchain
- MCP Server
- Microsoft Agent Framework
- Microsoft Entra ID
- OAuth
- OpenTelemetry
- Python
section_names:
- ai
- azure
- coding
- security
---
Pamela Fox presents a detailed series on building, deploying, and securing MCP servers with Python and Azure, covering FastMCP, cloud deployment, and authentication using Microsoft Entra.<!--excerpt_end-->

# Building and Deploying MCP Servers with Python and Azure

Pamela Fox's three-part livestream series, **Python + MCP**, guides developers through building Model Context Protocol (MCP) servers in Python, deploying them to Azure, and implementing secure authentication. All accompanying materials—including presentation slides, video sessions, and open-source code samples—are provided for extended learning.

## Series Overview

- **Build MCP servers in Python using FastMCP**
- **Deploy servers on Azure** (Container Apps and Azure Functions)
- **Add authentication** leveraging Microsoft Entra as the OAuth provider

### Resources

- [All session video recordings](https://www.youtube.com/watch?v=_mUuhOwv9PY), [second session](https://www.youtube.com/watch?v=gL3WYfXAiWI), [third session](https://www.youtube.com/watch?v=_Redi3ChzFA)
- [Presentation slides](https://aka.ms/pythonmcp/slides/servers), [deploying](https://aka.ms/pythonmcp/slides/deploying), [auth](https://aka.ms/pythonai/slides/auth)
- [Code examples (python-mcp-demos)](https://github.com/Azure-Samples/python-openai-demos)
- [Functions hosting sample (mcp-sdk-functions-hosting-python)](https://github.com/Azure-Samples/mcp-sdk-functions-hosting-python)
- [Spanish-language resources](https://aka.ms/pythonmcp/recursos)

## Part 1: Building MCP Servers with FastMCP

- Introduction to **MCP (Model Context Protocol)**: An open protocol for extending AI agents and chatbots with enhanced, custom functionality.
- Guide to building an MCP server in Python using the FastMCP SDK.
- Integrate with chatbots like **GitHub Copilot** in VS Code using resources, tool callbacks, and prompts.
- Examples of connecting with frameworks like **Langchain** and **Microsoft agent-framework**.
- [Session slides](https://aka.ms/pythonmcp/slides/servers) | [Code repository](https://github.com/Azure-Samples/python-openai-demos)

## Part 2: Deploying MCP Servers on Azure

- Steps for containerizing an MCP server using Docker.
- Deploying containers to **Azure Container Apps**.
- Observability using **OpenTelemetry**, **Azure Application Insights**, and **Logfire**.
- Private networking for MCP servers via Azure virtual networks.
- [Session slides](https://aka.ms/pythonmcp/slides/deploying) | [Code examples](https://github.com/Azure-Samples/python-openai-demos)

## Part 3: Adding Authentication with Microsoft Entra

- Implementing API key-based authentication for basic security.
- OAuth2 integration for user-specific data, exploring advanced flows such as PRM and DCR/CIMD for MCP.
- Demonstrating full authentication flows with KeyCloak (open-source identity provider), and implementing an OAuth proxy pattern with Microsoft Entra.
- [Session slides](https://aka.ms/pythonai/slides/auth) | [Code repositories](https://github.com/Azure-Samples/python-openai-demos), [Functions examples](https://github.com/Azure-Samples/mcp-sdk-functions-hosting-python)

## Additional Information

- Weekly office hours via Foundry Discord for community Q&A: [Join here](http://aka.ms/aipython/oh)
- Materials available for classroom and self-guided learning

---
*Author: Pamela Fox*

---

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/learn-how-to-build-mcp-servers-with-python-and-azure/ba-p/4479402)
