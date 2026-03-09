---
layout: "post"
title: "Network Configuration Update for Copilot Coding Agent Now Active"
description: "This update from GitHub announces that new network configuration requirements for the Copilot coding agent are now enforced as of February 27, 2026. The changes specifically affect teams operating Copilot coding agent on self-hosted or Azure private network runners. As a result, teams must update network allowlists to support subscription-based routing endpoints for different Copilot plans."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-03-02-network-configuration-changes-for-copilot-coding-agent-now-in-effect"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-03-02 20:42:32 +00:00
permalink: "/2026-03-02-Network-Configuration-Update-for-Copilot-Coding-Agent-Now-Active.html"
categories: ["AI", "GitHub Copilot"]
tags: ["AI", "Business", "Copilot", "Copilot Coding Agent", "Enterprise", "GitHub Copilot", "Improvement", "Network Allowlist", "Network Configuration", "News", "Private Networking", "Routing Endpoints", "Self Hosted Runners", "Subscription Plans"]
tags_normalized: ["ai", "business", "copilot", "copilot coding agent", "enterprise", "github copilot", "improvement", "network allowlist", "network configuration", "news", "private networking", "routing endpoints", "self hosted runners", "subscription plans"]
---

Allison details recent changes now in effect for the Copilot coding agent's network configuration, impacting teams using self-hosted or Azure private network runners.<!--excerpt_end-->

# Network Configuration Update: Copilot Coding Agent

As of February 27, 2026, new network configuration requirements for the Copilot coding agent are officially in effect. This impacts teams that deploy Copilot coding agent on:

- **Self-hosted runners**
- **Larger runners with Azure private networking**

## Key Changes

- Copilot coding agent now uses **subscription-based network routing**.
- The agent connects to hosts specific to a user's Copilot plan:
  - Copilot Business: `api.business.githubcopilot.com`
  - Copilot Enterprise: `api.enterprise.githubcopilot.com`
  - Copilot Pro/Pro+: `api.individual.githubcopilot.com`

## Required Actions

- Teams must ensure their network allows outbound connections to these new endpoints, depending on users' Copilot plans.
- If previously whitelisted, you may **remove `api.githubcopilot.com`** from your allowlist for Copilot coding agent traffic.

## Who Is Affected?

- Only teams using Copilot coding agent on **self-hosted or Azure private networking runners**.
- Standard users or teams on GitHub-hosted runners without private networking are *not impacted*.

## Reference Links

- [Details on Copilot coding agent](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent)
- [Configuring private networking for GitHub-hosted runners](https://docs.github.com/enterprise-cloud@latest/admin/configuring-settings/configuring-private-networking-for-hosted-compute-products/configuring-private-networking-for-github-hosted-runners-in-your-enterprise)
- [Subscription-based network routing management](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/administer-copilot/manage-for-enterprise/manage-access/manage-network-access)

**Author:** Allison

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-03-02-network-configuration-changes-for-copilot-coding-agent-now-in-effect)
