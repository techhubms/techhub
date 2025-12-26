---
layout: "post"
title: "Streamlining Enterprise Governance with GitHub Organization Custom Properties"
description: "This news item introduces organization custom properties in GitHub, now available in public preview. Enterprise administrators can use these properties to efficiently add and manage metadata across organizations, improving ruleset targeting for compliance, security, and operational needs. The post details what custom properties are, how they work, their configuration options, and the potential for better security and governance through automation. Limitations, best practices, and links to further discussion and feedback opportunities are provided."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-10-23-organization-custom-properties-are-now-available-in-public-preview"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-10-23 16:58:12 +00:00
permalink: "/news/2025-10-23-Streamlining-Enterprise-Governance-with-GitHub-Organization-Custom-Properties.html"
categories: ["DevOps"]
tags: ["Configuration", "Custom Properties", "DevOps", "Enterprise Administration", "Enterprise Management Tools", "GitHub", "Governance", "Improvement", "Metadata", "News", "Organization Management", "Platform Governance", "Platform Management", "Policy Automation", "Public Preview", "Rulesets", "Security Compliance", "Targeting"]
tags_normalized: ["configuration", "custom properties", "devops", "enterprise administration", "enterprise management tools", "github", "governance", "improvement", "metadata", "news", "organization management", "platform governance", "platform management", "policy automation", "public preview", "rulesets", "security compliance", "targeting"]
---

Allison shares news about GitHub's new organization custom properties, helping enterprise administrators manage organizational metadata for better policy targeting and compliance automation.<!--excerpt_end-->

# Streamlining Enterprise Governance with GitHub Organization Custom Properties

Organization custom properties in GitHub, now available in public preview, enable enterprise administrators to assign and manage metadata across organizations for more precise ruleset targeting.

## What Are Organization Custom Properties?

Similar to custom properties for repositories, organization custom properties let you define and assign metadata (such as department, compliance requirements, or geographic region) to each organization within your enterprise. This enables the creation of targeted policies tailored to various business needs.

**Benefits include:**

- **Time savings and fewer errors:** Automate the application of rulesets so the right policies are applied to the right organizations without manual configuration.
- **Improved security and compliance:** Ensure organizations comply with the correct security and compliance requirements through automated rules.
- **Flexible control:** Adjust policies to meet each organization's unique needs across your enterprise.

## How Organization Custom Properties Work

Enterprise administrators define custom properties at the enterprise level. For each property, assign values to individual organizations. Supported property types include:

- **Single-select:** Choose one value from a predefined list.
- **Multi-select:** Choose multiple values from a list.
- **Text:** Input free-form text.
- **True/false:** Set a boolean value.

**Configuration limits:**

- Up to 100 property definitions per enterprise.
- Allowed value lists can include up to 200 items.
- Property names: Only `a-z`, `A-Z`, `0-9`, `_`, `-`, `---

layout: "post"
title: "Streamlining Enterprise Governance with GitHub Organization Custom Properties"
description: "This news item introduces organization custom properties in GitHub, now available in public preview. Enterprise administrators can use these properties to efficiently add and manage metadata across organizations, improving ruleset targeting for compliance, security, and operational needs. The post details what custom properties are, how they work, their configuration options, and the potential for better security and governance through automation. Limitations, best practices, and links to further discussion and feedback opportunities are provided."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-10-23-organization-custom-properties-are-now-available-in-public-preview"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: https://github.blog/changelog/feed/
date: 2025-10-23 16:58:12 +00:00
permalink: "2025-10-23-Streamlining-Enterprise-Governance-with-GitHub-Organization-Custom-Properties.html"
categories: ["DevOps"]
tags: ["Configuration", "Custom Properties", "DevOps", "Enterprise Administration", "Enterprise Management Tools", "GitHub", "Governance", "Improvement", "Metadata", "News", "Organization Management", "Platform Governance", "Platform Management", "Policy Automation", "Public Preview", "Rulesets", "Security Compliance", "Targeting"]
tags_normalized: [["configuration", "custom properties", "devops", "enterprise administration", "enterprise management tools", "github", "governance", "improvement", "metadata", "news", "organization management", "platform governance", "platform management", "policy automation", "public preview", "rulesets", "security compliance", "targeting"]]
---

Allison shares news about GitHub's new organization custom properties, helping enterprise administrators manage organizational metadata for better policy targeting and compliance automation.<!--excerpt_end-->

, and `#` allowed.

- Property values: Any printable ASCII character except `"`.

## Getting Started

Enterprise admins can access the new feature from the organization's custom properties section in their enterprise settings. Get involved in the [GitHub Community discussion](https://gh.io/org-properties-discussion) to share feedback during the public preview.

---

**Screenshots included in the original announcement:**

- Example of the "Regional Compliance" code ruleset settings, showing rule evaluation status and target organizations by property.
- Example settings for a "region" custom property, displaying its attributes and options.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-10-23-organization-custom-properties-are-now-available-in-public-preview)
