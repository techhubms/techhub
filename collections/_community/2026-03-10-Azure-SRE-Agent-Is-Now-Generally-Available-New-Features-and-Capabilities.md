---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-is-generally-available-what-s-new/ba-p/4500779
title: 'Azure SRE Agent Is Now Generally Available: New Features and Capabilities'
author: dchelupati
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-03-10 14:09:34 +00:00
tags:
- Agent Automation
- AI
- Azure
- Azure Container Apps
- Azure SRE Agent
- Community
- DevOps
- DevOps Tools
- ICM
- Incident Management
- MCP Connectors
- Monitoring
- Operational Intelligence
- PagerDuty
- Plugin Marketplace
- Python Integration
- Root Cause Analysis
- Serverless
- ServiceNow
- Site Reliability Engineering
- SRE
section_names:
- ai
- azure
- devops
---
dchelupati introduces the Azure SRE Agent’s general availability, describing its new operational and automation features for Site Reliability Engineering on Azure, including integration, workflow orchestration, and deep diagnostic capabilities.<!--excerpt_end-->

# Azure SRE Agent Is Now Generally Available: New Features and Capabilities

_Authored by dchelupati_

The Azure SRE Agent has reached general availability, following months of testing across Microsoft teams and with early customers. The SRE Agent is designed to improve operational reliability by automating investigations, simplifying onboarding, and building deep context across your systems.

## Highlights of the GA Release

- **Redesigned Onboarding:** The agent now guides you through connecting your code, logs, incidents, Azure resources, and knowledge files, making it useful from the first day.
- **Continuous Context Building:** Persistent access to your logs, source code, and prior investigations enables the agent to build operational expertise, remember historic issues, and offer insights proactively.
- **Automated Investigation:** Schedule regular diagnostics or let the agent pick up incidents automatically through integrations with platforms like ICM, PagerDuty, and ServiceNow.
- **Root Cause Analysis:** Learns from previous incidents, tracking errors to the code that caused them. The agent gets smarter with each usage.
- **Ecosystem Workflow Automation:** Connects to external and internal systems via MCP connectors, orchestrates workflows across Azure and your monitoring and ticketing platforms.
- **Custom Tooling:** Supports integration with any HTTP API and allows extension via custom Python scripts.
- **Skills and Plugins:** Domain-specific knowledge can be added as custom skills, or use plugins from the Plugin Marketplace for new capabilities.

## Useful Resources

- [Create your agent](https://aka.ms/sreagent-portal)
- [Official Documentation](https://aka.ms/sreagent/newdocs)
- [Getting Started Guide](https://aka.ms/sreagent/newdocs/get-started)
- [Pricing](https://azure.microsoft.com/en-us/pricing/details/sre-agent/)
- [Feedback & Issues](https://github.com/microsoft/sre-agent/issues)
- [Sample Implementations](https://github.com/microsoft/sre-agent/tree/main/samples)
- [Video Tutorials](https://www.youtube.com/@AzureSREAgent)

For further insights:

- [GA Announcement](https://aka.ms/sreagent/ga)
- [The Agent That Investigates Itself](https://aka.ms/sreagent/blogs/sre4sre)
- [Deep Context Blog](https://aka.ms/sreagent/blogs/deepcontextblog)
- [Onboarding Blog](https://aka.ms/sreagent/blogs/onboardingtosrea)

## Core Capabilities

- **Proactive and reactive investigations**: Reducing time to resolution (MTTR) by automating error detection and diagnosis.
- **Integrated platform connectors**: Connect Azure, monitoring, and incident systems under one roof.
- **Custom skills and plugin system**: Domain tailoring for unique operational requirements.
- **Support for Python integration and HTTP APIs**: Extensibility for specialized workflows.

This is a foundational GA release, with more innovations planned. Teams looking to streamline site reliability and operational automation on Azure are encouraged to explore the SRE Agent and contribute feedback as adoption grows.

_Last updated Mar 10, 2026. Version 1.0. Authored by dchelupati._

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-is-generally-available-what-s-new/ba-p/4500779)
