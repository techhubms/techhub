---
layout: post
title: Enterprise AI Controls and Agent Control Plane Public Preview for GitHub Copilot
author: Allison
canonical_url: https://github.blog/changelog/2025-10-28-enterprise-ai-controls-the-agent-control-plane-are-in-public-preview
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-10-28 15:24:07 +00:00
permalink: /github-copilot/news/Enterprise-AI-Controls-and-Agent-Control-Plane-Public-Preview-for-GitHub-Copilot
tags:
- Administration
- Agent Control Plane
- Agent Management
- AI Governance
- Audit Logs
- Copilot
- Custom Agents
- Enterprise Cloud
- Enterprise Management Tools
- Enterprise Roles
- Fine Grained Permissions
- GitHub Administration
- MCP Allowlist
- Policy Configuration
- Universe25
- VS Code
section_names:
- ai
- devops
- github-copilot
---
Allison announces the public preview of enterprise AI controls and an agent control plane for GitHub Copilot, empowering administrators with governance, auditability, and organizational configuration tools tailored for enterprises.<!--excerpt_end-->

# Enterprise AI Controls & Agent Control Plane: Public Preview

**Author:** Allison

GitHub is launching a new enterprise-level experience for managing AI systems and Copilot on GitHub Enterprise Cloud, now available to all enterprise customers. The newly introduced agent control plane provides a central suite of features tailored for organizational governance, administrative visibility, and operational control over custom agents and AI tools deployed at scale.

## Key Features of the Agent Control Plane

- **Centralized Administration:** Unified dashboard for all AI and Copilot-related admin tasks within GitHub Enterprise Cloud.
- **Custom Agent Management:** Configure and operationalize organization-defined custom agents. Use one-click push rules to protect standard file paths (`.github/agents/*.md`) and control agent assignment across your enterprise.
- **Agent Activity Insights:** View enterprise-wide sessions, filter activities by agent type and state, and navigate directly to affected repositories.
- **Detailed Audit Logging:** New audit log fields capture granular agent activity, including events like `pull_request.create` by Copilot, session state changes, and traceability to user and agent actors.
- **Permission and Access Controls:** Fine-grained permissions (FGPs) allow enterprises to delegate AI controls to specific teams and create custom roles, decentralizing oversight without broad administrative access.
- **MCP Enterprise Allowlist:** Centrally govern which Machine Connection Points (MCP) are permitted using registry URLs, with forthcoming support for VS Code and other development environments.

## Upcoming Enhancements

Soon, fine-grained permission (FGP) features for decentralized AI administration and further IDE support will become available, enabling even more customized enterprise governance strategies.

## Additional Resources

- [Documentation on AI controls and agent control plane](https://docs.github.com/copilot/concepts/agents/coding-agent/about-custom-agents?utm_source=changelog-docs-enterprise-controls&utm_medium=changelog&utm_campaign=universe25)
- [GitHub Community: Copilot Conversations](https://github.com/orgs/community/discussions/categories/copilot-conversations?utm_source=changelog-community-enterprise-controls&utm_medium=changelog&utm_campaign=universe25)

Enterprises are encouraged to begin experimenting with these capabilities to standardize their approach to Copilot and AI administration, enhance oversight, and meet scaling needs for advanced agentic development.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-28-enterprise-ai-controls-the-agent-control-plane-are-in-public-preview)
