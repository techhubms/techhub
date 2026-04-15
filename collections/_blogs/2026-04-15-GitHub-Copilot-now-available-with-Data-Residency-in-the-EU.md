---
feed_name: Jesse Houwing's Blog
primary_section: github-copilot
author: Jesse Houwing
tags:
- AI
- Blogs
- Copilot Data Residency
- Data Residency
- Data Sovereignty
- DevOps
- Enterprise Policy
- EU Region
- FedRAMP Compliance
- Generally Available Features
- Ghe.com
- GitHub
- GitHub Copilot
- GitHub Enterprise Cloud
- Inference Processing
- Organization Policy
- Preview Features
- Proxima
- US Region
date: 2026-04-15 08:17:51 +00:00
title: GitHub Copilot now available with Data Residency in the EU
section_names:
- ai
- devops
- github-copilot
external_url: https://jessehouwing.net/github-copilot-now-with-data-residency-in-europe/
---

Jesse Houwing summarizes GitHub’s update that GitHub Copilot can now keep inference processing and associated data within US or EU data residency regions, and shows the enterprise/org policy you must enable to restrict Copilot to data-resident models.<!--excerpt_end-->

# GitHub Copilot now available with Data Residency in the EU

With the current political climate there has been increased focus on **data residency** and **data sovereignty**, and more inquiries to migrate from **GitHub Enterprise Cloud** to the **Data Resident** version.

Previously, one of the gaps for GitHub data residency was that **Copilot data could still leave the region**. GitHub has now addressed that.

## Announcement

GitHub changelog entry:

[Copilot data residency in US + EU and FedRAMP compliance now available](https://github.blog/changelog/2026-04-13-copilot-data-residency-in-us-eu-and-fedramp-compliance-now-available/?ref=jessehouwing.net)

According to GitHub, **GitHub Copilot now supports data residency for US and EU regions**, ensuring that:

- **Inference processing** stays within your designated geography
- **Associated data** stays within your designated geography
- For US government customers, **FedRAMP compliance** is now available (as described in the changelog)

## Prerequisite: use the Data Resident version of GitHub Enterprise Cloud

To use this feature, you must already be using the **Data Resident version of GitHub Enterprise Cloud** in a supported region.

This environment is also referred to as:

- `ghe.com`
- GitHub’s internal codename **Proxima**

## Configure the policy to restrict Copilot to data-resident models

To ensure Copilot only uses **Data Resident models**, you must enable a policy at either the **Enterprise** or **Organization** level.

![Screenshot showing the GitHub policy “Restrict Copilot to Data Residency Models”](https://storage.ghost.io/c/13/29/1329ef25-4e8e-4ebc-be08-4f1135b51be8/content/images/2026/04/image-1.png)

Set **Restrict Copilot to Data Residency Models** to **Enabled everywhere** to ensure Copilot data is processed in your data residency region.

### Feature availability note

GitHub states that currently all **generally available** Copilot features are included. However:

- **New features** may take longer to arrive in data-residency environments
- This can happen when features are still in **preview** and not yet available for data residency

The author notes this has been the case with other GitHub features as well, so it should not be surprising.

[Read the entire article](https://jessehouwing.net/github-copilot-now-with-data-residency-in-europe/)

