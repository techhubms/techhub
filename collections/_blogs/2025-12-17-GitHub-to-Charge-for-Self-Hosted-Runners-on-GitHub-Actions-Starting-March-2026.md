---
layout: "post"
title: "GitHub to Charge for Self-Hosted Runners on GitHub Actions Starting March 2026"
description: "This article details the significant pricing change announced by GitHub, which will begin charging {{DESCRIPTION}}.002 per minute for self-hosted runners used in GitHub Actions from March 1, 2026. The coverage includes developer reactions, competitive analysis, and planned improvements, as well as the impact on various users and workflow scenarios."
author: "Tim Anderson"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devclass.com/2025/12/17/github-to-charge-for-self-hosted-runners-from-march-2026/"
viewing_mode: "external"
feed_name: "DevClass"
feed_url: "https://devclass.com/feed/"
date: 2025-12-17 11:55:59 +00:00
permalink: "/blogs/2025-12-17-GitHub-to-Charge-for-Self-Hosted-Runners-on-GitHub-Actions-Starting-March-2026.html"
categories: ["DevOps"]
tags: ["Actions Data Stream", "Actions Runner Controller", "CI/CD", "Cloud Infrastructure", "Continuous Integration", "Depot", "Developer Tooling", "Development", "DevOps", "GitHub", "GitHub Actions", "GitHub Enterprise Server", "Microsoft", "Blogs", "Pricing Change", "Self Hosted Runners", "Workflow Automation"]
tags_normalized: ["actions data stream", "actions runner controller", "cislashcd", "cloud infrastructure", "continuous integration", "depot", "developer tooling", "development", "devops", "github", "github actions", "github enterprise server", "microsoft", "blogs", "pricing change", "self hosted runners", "workflow automation"]
---

Tim Anderson examines GitHub's decision to introduce per-minute fees for self-hosted runners used with GitHub Actions starting March 2026, and discusses the developer community's reactions and the wider DevOps ecosystem.<!--excerpt_end-->

# GitHub to Charge for Self-Hosted Runners on GitHub Actions Starting March 2026

**Author:** Tim Anderson

GitHub has announced that, beginning March 1, 2026, it will charge {{CONTENT}}.002 per minute for self-hosted runners used with GitHub Actions—a marked departure from the previous policy where self-hosted runners were free. Previously, developers could run Actions workflows on their own infrastructure without additional cost. This change comes as part of broader pricing adjustments that also reduce rates for GitHub-hosted runners by 20-39%.

## Background and Rationale

GitHub states that revenue from GitHub-hosted runners was previously subsidizing self-hosted runners. The company highlighted costs for “maintaining and evolving GitHub Actions,” which reportedly processes 71 million jobs daily. The change ostensibly aligns revenue with the underlying operational costs of the platform.

- **Pricing Details:**
  - {{CONTENT}}.002 per minute for self-hosted runners
  - No effect on GitHub Enterprise Server (on-premises)
  - GitHub Actions remain free for public repositories
  - Company claims 96% of customers will see no change to their bill; those impacted may actually pay less

## Developer Community Reaction

The reaction among developers has been predominantly negative, especially for teams regularly using self-hosted infrastructure. Concerns center on the principle of paying to run jobs on hardware already owned and operated by the user, with some calling the move “absolutely bananas.” Others point to the potentially high costs for teams with intensive CI usage, such as an estimated $140+ per month for heavy users.

Despite price reductions for many, some developers see this as GitHub focusing on new features and controversial UI or AI changes rather than core platform stability. Criticism also notes issues with the open source runner application, lack of contributions, and operational hiccups such as difficulty canceling jobs and slow configuration.

## Competitive Context

GitHub’s decision is partly attributed to pressure from rivals like Depot, which offers optimized runners at per-second billing—marketed at half the cost of GitHub’s own hosted option. Some projects, including Zig, have migrated away from GitHub citing Actions-related frustrations.

## Planned Platform Improvements

GitHub has promised to invest further in the Actions ecosystem alongside the pricing change, including:

- A new scale-set client for self-hosted runners
- Multi-label support in Actions Runner Controller for easier management
- Launch of Actions Data Stream, providing real-time workflow observability

Whether these investments will sway developer sentiment remains to be seen.

## Impacted Users

- **Users with public repositories:** No cost changes
- **Enterprise Server (on-premises) users:** No impact
- **Heavy CI users on self-hosted hardware:** Likely to see substantial new costs
- **Majority of smaller or average users:** Expected to see no change or a reduction in cost

## Conclusion

The new self-hosted runner fees mark a major shift in GitHub Actions pricing and could have ripple effects through the DevOps landscape. Teams relying on self-hosted workflows should begin preparing for the new fee structure and reassess the economics of their CI/CD infrastructure going forward.

This post appeared first on "DevClass". [Read the entire article here](https://devclass.com/2025/12/17/github-to-charge-for-self-hosted-runners-from-march-2026/)
