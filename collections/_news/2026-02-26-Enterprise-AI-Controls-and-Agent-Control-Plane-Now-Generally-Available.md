---
layout: "post"
title: "Enterprise AI Controls and Agent Control Plane Now Generally Available"
description: "This announcement details the general availability of GitHub’s Enterprise AI Controls and agent control plane, providing enterprise administrators with enhanced governance, oversight, and management capabilities for Copilot and AI agents. Key features include granular audit logging, policy configuration, custom agent support, and programmatic management APIs—empowering organizations to safely scale AI adoption while maintaining control and compliance."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-26-enterprise-ai-controls-agent-control-plane-now-generally-available"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-26 20:10:23 +00:00
permalink: "/2026-02-26-Enterprise-AI-Controls-and-Agent-Control-Plane-Now-Generally-Available.html"
categories: ["AI", "GitHub Copilot"]
tags: ["Agent Control Plane", "AI", "AI Controls", "AI Governance", "API Integration", "Audit Logging", "Compliance", "Copilot", "Copilot Policies", "Custom Agents", "Enterprise Administration", "Enterprise AI Controls", "Enterprise Management Tools", "GitHub Copilot", "MCP Registry", "Microsoft GitHub", "News", "Role Based Access", "Session Activity"]
tags_normalized: ["agent control plane", "ai", "ai controls", "ai governance", "api integration", "audit logging", "compliance", "copilot", "copilot policies", "custom agents", "enterprise administration", "enterprise ai controls", "enterprise management tools", "github copilot", "mcp registry", "microsoft github", "news", "role based access", "session activity"]
---

Allison announces the general availability of GitHub’s Enterprise AI Controls and agent control plane. The update gives enterprise administrators enhanced governance, visibility, and policy management for Copilot and AI agents.<!--excerpt_end-->

# Enterprise AI Controls and Agent Control Plane Now Generally Available

**Author**: Allison  

GitHub has officially launched the general availability of its Enterprise AI Controls and agent control plane, introducing a new standard for enterprise governance over GitHub Copilot and custom AI agents. This suite of features provides enterprise administrators with unprecedented control, visibility, and configuration capabilities around AI usage in organizational codebases.

## Key Features and Capabilities

### Centralized AI Administration Workspace

- A consolidated view and top-level navigation for all AI system management tasks.
- Custom administrative roles with fine-grained permissions, allowing decentralized management of AI standards and adoption.
- Role-based access lets teams manage audit logs, agent activities, and AI Controls efficiently.

### Detailed Audit Logging & Agent Tracking

- Comprehensive audit logs track agent activity, including who agents are acting on behalf of.
- New event records (`agent_session.task`) display when sessions are started, completed, or incomplete.
- Cloud agent session activity for the past 24 hours is accessible, with an enhanced session record cap.
- Central MCP (Managed Control Plane) allowlist registry for streamlined agent and policy management.

### Custom Agent Standards and Protection

- Enterprises can set and enforce standards for custom agents, enabling agent roles tailored to organization needs.
- Built-in version control for custom agent configurations, utilizing `.github/agents/*.md` paths.
- 1-click push rules protect critical agent definitions at scale.

### Expanded Discovery and Policy Configuration

- Enhanced UI in the enterprise settings now dedicates an “AI Controls” tab for all policy and configuration needs.
- Administrators can programmatically define and manage custom agent configurations via APIs.
- Enhanced search, filtering, and policy application, including organization-level and third-party agent tracking.

### Operational Improvements

- Cloud agent session activity view goes beyond the prior 1,000 entry limit.
- More detailed and filterable agent discovery in the agents audit log.
- Support for MCP enterprise allowlists is still in preview but will scale better in future releases.

## Looking Forward

- Upcoming features will expand coverage of agent session activity in Copilot clients (e.g., VS Code, Copilot CLI).
- Future updates will deliver more granular API access and controls for enterprise policies and session management.
- Ongoing evolution in MCP governance for large-scale organizations.

Read more in the [official documentation](https://docs.github.com/copilot/concepts/agents/enterprise-management), and join the [community discussion](https://github.com/orgs/community/discussions/178247).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-26-enterprise-ai-controls-agent-control-plane-now-generally-available)
