---
layout: "post"
title: "Network Configuration Updates for Copilot Coding Agent with GitHub Actions and Azure"
description: "This news update outlines upcoming network configuration changes affecting the GitHub Copilot coding agent, particularly when used with self-hosted or Azure private network runners. It details the new routing based on Copilot subscription plans and provides guidance for repository administrators on required action steps by the February 27, 2026 deadline."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-13-network-configuration-changes-for-copilot-coding-agent"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-13 15:37:05 +00:00
permalink: "/2026-02-13-Network-Configuration-Updates-for-Copilot-Coding-Agent-with-GitHub-Actions-and-Azure.html"
categories: ["AI", "Azure", "DevOps", "GitHub Copilot"]
tags: ["AI", "Azure", "Azure Private Networking", "Continuous Integration", "Copilot", "Copilot Business", "Copilot Coding Agent", "Copilot Enterprise", "Copilot Pro", "DevOps", "GitHub Actions", "GitHub Copilot", "Improvement", "Network Configuration", "News", "Self Hosted Runners", "Subscription Based Routing"]
tags_normalized: ["ai", "azure", "azure private networking", "continuous integration", "copilot", "copilot business", "copilot coding agent", "copilot enterprise", "copilot pro", "devops", "github actions", "github copilot", "improvement", "network configuration", "news", "self hosted runners", "subscription based routing"]
---

Allison explains the forthcoming network configuration changes impacting the GitHub Copilot coding agent, with a focus on workflows using self-hosted or Azure-based runners. Key dates and practical migration steps are covered in this important update.<!--excerpt_end-->

# Network Configuration Changes for Copilot Coding Agent

_Authored by Allison_

## Overview

The Copilot coding agent is GitHub’s asynchronous, autonomous agent designed to perform delegated development tasks in the background. It operates in its own isolated environment, powered by GitHub Actions, often culminating in automatically opening pull requests for your review.

## What is Changing?

Beginning **February 27, 2026 (00:00 UTC)**, the network configuration supporting the Copilot coding agent will be updated. These changes are especially relevant if your team:

- Has configured the Copilot coding agent to run on [self-hosted runners](https://docs.github.com/actions/concepts/runners/self-hosted-runners)
- Uses larger GitHub-hosted runners with [Azure private networking](https://docs.github.com/enterprise-cloud@latest/admin/configuring-settings/configuring-private-networking-for-hosted-compute-products/configuring-private-networking-for-github-hosted-runners-in-your-enterprise)

If your org fits these criteria and has run the agent in this way in the past 60 days, you will receive an email with a list of possibly-affected repositories.

## Details of the Network Update

Currently, Copilot coding agent connects to `api.githubcopilot.com` for AI inference and logging. After February 27, 2026, the routing will be based on the subscription plan of the user initiating the agent task:

- **Copilot Business**: `api.business.githubcopilot.com`
- **Copilot Enterprise**: `api.enterprise.githubcopilot.com`
- **Copilot Pro and Pro+**: `api.individual.githubcopilot.com`

See [subscription-based network routing](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/administer-copilot/manage-for-enterprise/manage-access/manage-network-access) for administrative details.

## What Should You Do?

1. _Identify Affected Repositories_: Look for `.github/workflows/copilot-setup-steps.yml` in your repositories. Check the `runs-on` value to confirm which runners are used. If this file isn't present, the repository is not impacted.
2. _Review and Update Network Configuration_: Ensure outbound network access is allowed to the new hosts above. You may need multiple domains if users have different Copilot plans.
3. _Deadline_: All changes should be made before February 27, 2026, to avoid Copilot coding agent task failure due to communication issues with GitHub.

## Reference Links

- [Copilot coding agent documentation](https://docs.github.com/copilot/concepts/agents/coding-agent/about-coding-agent)
- [GitHub Actions: self-hosted runners](https://docs.github.com/actions/concepts/runners/self-hosted-runners)
- [Azure private networking for runners](https://docs.github.com/enterprise-cloud@latest/admin/configuring-settings/configuring-private-networking-for-hosted-compute-products/configuring-private-networking-for-github-hosted-runners-in-your-enterprise)
- [Subscription-based Copilot routing](https://docs.github.com/enterprise-cloud@latest/copilot/how-tos/administer-copilot/manage-for-enterprise/manage-access/manage-network-access)

## Summary

If your organization uses Copilot coding agent on self-hosted or Azure private network runners, it's essential to review your network rules and update host allow-lists by the deadline. Ignoring these changes may cause your automated Copilot tasks to fail.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-13-network-configuration-changes-for-copilot-coding-agent)
