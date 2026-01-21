---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-expanding-observability-and-multi-cloud/ba-p/4472719
title: 'Azure SRE Agent: Enhancing Observability and Multi-Cloud Incident Management'
author: dbandaru
feed_name: Microsoft Tech Community
date: 2025-11-24 23:34:13 +00:00
tags:
- Automation
- Azure Monitor
- Azure SRE Agent
- Cross Platform
- Datadog
- Diagnostics
- Dynatrace
- Hybrid Cloud
- Incident Management
- MCP
- MCP Server
- Multi Cloud
- NeuBird
- New Relic
- Observability
- Operational Excellence
- PagerDuty
- Remediation
section_names:
- azure
- devops
---
dbandaru presents an in-depth look at Azure SRE Agent, detailing its integrations with Datadog, New Relic, Dynatrace, and plans for multi-agent collaboration to strengthen observability and incident management across hybrid and multi-cloud environments.<!--excerpt_end-->

# Azure SRE Agent: Enhancing Observability and Multi-Cloud Incident Management

Azure SRE Agent is fast becoming a central tool for operational excellence and incident management across Microsoft Azure and hybrid environments. In this article, dbandaru explores key partnerships, technical advances, and the broader vision for automated, resilient operations through deep platform integration.

## Integrations with Observability Platforms

Azure SRE Agent now provides native integration with leading observability platforms through Model Context Protocol (MCP) Servers:

- **Datadog**: Customers can bring Datadog MCP Server capabilities directly into Azure SRE Agent, making it possible to centralize logs, metrics, and analysis from Datadog’s observability suite. See Datadog’s [Azure Native Marketplace offering](https://marketplace.microsoft.com/en-US/product/saas/datadog1591740804488.dd_liftr_v2?tab=Overview) for details.

- **New Relic**: When an alert triggers in New Relic, Azure SRE Agent can invoke the corresponding MCP Server, leveraging advanced tools for entity/account management, deep workflows for monitoring, performance analysis, and instant remediation. Learn about New Relic’s integration at [Marketplace](https://marketplace.microsoft.com/en-US/product/saas/newrelicinc1635200720692.newrelic_liftr_payg?tab=Overview).

- **Dynatrace**: The Dynatrace integration connects Azure’s infrastructure management to Dynatrace’s AI-powered observability platform (Davis AI engine), enabling cross-cloud incident detection, root cause analysis, and remediation. Details are available at [Marketplace](https://marketplace.microsoft.com/en-US/product/saas/dynatrace.dynatrace_portal_integration?tab=Overview).

These integrations are delivered via Azure SRE Agent’s MCP connectors, enabling customers to:

- Bridge the agent to MCP servers for dynamic discovery and use of platform-specific observability and remediation tools
- Build custom sub-agents using observability resources from Datadog, New Relic, and Dynatrace
- Unlock cross-platform telemetry analysis and automate resolution scenarios

## Multi-Agent Collaboration for Resilience

The roadmap extends beyond MCP integrations:

- **PagerDuty**: PagerDuty’s PD Advance SRE Agent brings AI-driven incident triage by analyzing diagnostic logs, incidents, and runbooks. Azure SRE Agent and PagerDuty SRE Agent can collaborate on incident triage by sharing context, leveraging historical patterns, and surfacing remediations using Azure diagnostics.

- **NeuBird**: NeuBird’s Hawkeye AI autonomously investigates and resolves incidents across hybrid and multi-cloud environments by linking to telemetry sources like Azure Monitor, Prometheus, and GitHub. This agent-to-agent scenario expands multi-cloud management’s reach and enables real-time collaborative incident diagnosis and remediation. Sign up for the [private preview](https://neubird.ai/neubird-x-microsoft-azure-ai-sre-private-preview/).

These technical collaborations are paving the way for an agentic ecosystem focused on proactive, collaborative site reliability engineering.

## Why This Matters

Organizations running distributed cloud-native apps require sophisticated, integrated SRE solutions that operate effectively across clouds. Azure SRE Agent, by integrating with industry-leading platforms and building multi-agent workflows, offers:

- Automated remediation across platforms
- Centralized observability and diagnostics
- Proactive and scalable multi-cloud incident management
- The foundation for future innovation in operational resilience

## Additional Resources

- [SRE Day: Multi-Agent Collaboration Demo](https://www.youtube.com/watch?v=B2JKQX6haYc)
- [Azure Marketplace: Datadog](https://marketplace.microsoft.com/en-US/product/saas/datadog1591740804488.dd_liftr_v2?tab=Overview)
- [Azure Marketplace: New Relic](https://marketplace.microsoft.com/en-US/product/saas/newrelicinc1635200720692.newrelic_liftr_payg?tab=Overview)
- [Azure Marketplace: Dynatrace](https://marketplace.microsoft.com/en-US/product/saas/dynatrace.dynatrace_portal_integration?tab=Overview)
- [Azure Marketplace: NeuBird](https://marketplace.microsoft.com/en-us/product/saas/neubirdinc1733351251104.hawkeye)

---

*Author: dbandaru*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/azure-sre-agent-expanding-observability-and-multi-cloud/ba-p/4472719)
