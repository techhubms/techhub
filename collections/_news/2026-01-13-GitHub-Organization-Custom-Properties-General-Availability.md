---
layout: "post"
title: "GitHub Organization Custom Properties: General Availability"
description: "This announcement introduces the general availability of organization custom properties for GitHub Enterprise Cloud administrators. Custom properties allow tagging organizations with metadata, enabling automated rule application across business units, geographic regions, or compliance frameworks. The release increases consistency and scalability in enterprise GitHub management."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-01-13-organization-custom-properties-now-generally-available"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-01-13 20:48:28 +00:00
permalink: "/2026-01-13-GitHub-Organization-Custom-Properties-General-Availability.html"
categories: ["DevOps"]
tags: ["Automation", "Compliance", "Configuration Management", "Custom Properties", "DevOps", "Enterprise Administration", "Enterprise Management Tools", "Enterprise Policy", "GitHub Enterprise Cloud", "Improvement", "Metadata Tagging", "News", "Organization Management", "Platform Governance", "Rulesets"]
tags_normalized: ["automation", "compliance", "configuration management", "custom properties", "devops", "enterprise administration", "enterprise management tools", "enterprise policy", "github enterprise cloud", "improvement", "metadata tagging", "news", "organization management", "platform governance", "rulesets"]
---

Allison introduces the general availability of organization custom properties for GitHub Enterprise Cloud, helping administrators manage enterprise governance and policy automation more efficiently.<!--excerpt_end-->

# GitHub Organization Custom Properties: General Availability

Organization custom properties are now available on GitHub Enterprise Cloud, offering administrators a flexible way to manage organizations through detailed metadata tagging.

## Why Organization Custom Properties?

- **Scalable Ruleset Application:** Apply enterprise policies flexibly by tagging organizations with properties such as `region: EMEA`, `compliance: SOC2`, or `business-unit: Platform`.
- **Automatic Policy Targeting:** New organizations matching set criteria are brought under compliance automatically, reducing manual work and configuration drift.
- **Adaptable Structure:** Works for any enterprise setup—whether by team, department, region, or compliance requirement.

**Example:**
To enforce stricter code review for EU organizations handling customer data, define a ruleset targeting `region: EU` and `data-classification: customer-data`. Any matching org, old or new, is governed by this ruleset immediately.

## How It Works

- **Property Types:**
  - Single-select (one value from list)
  - Multi-select (multiple values from list)
  - Text (free-form entry)
  - True/False (boolean)
  - URL
- **Assignment:**
  - Define custom properties at the enterprise level
  - Assign property values to each organization
  - Set rulesets to target orgs by custom property combinations

![Screenshot illustrating region custom property settings in GitHub Enterprise Cloud.](https://github.com/user-attachments/assets/f802250c-45ed-4532-8d49-1bc7333ccc9b)

![Screenshot showing a compliance code ruleset targeting organizations based on region and compliance.](https://github.com/user-attachments/assets/19aee9db-45e6-43b9-b956-c85b58b0bdec)

## Getting Started

- **Availability:** For all GitHub Enterprise Cloud customers
- **Access:** In the organization section of enterprise settings
- **Documentation:** [Managing custom properties for organizations in your enterprise](https://gh.io/org-properties-docs)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-01-13-organization-custom-properties-now-generally-available)
