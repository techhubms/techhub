---
layout: "post"
title: "Azure Tag Best Practice: Staging Tags Before Server Onboarding"
description: "The author seeks advice on whether Azure allows users to predefine and stage resource tags and patching schedules before servers are onboarded, aiming for efficient organization and management during infrastructure expansion. The post discusses current Azure tagging capabilities and asks about best practices and potential limitations."
author: "jyoungii"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/AZURE/comments/1mbrt66/azure_tag_best_practice/"
viewing_mode: "external"
feed_name: "Reddit Azure"
feed_url: "https://www.reddit.com/r/azure/.rss"
date: 2025-07-28 20:43:49 +00:00
permalink: "/2025-07-28-Azure-Tag-Best-Practice-Staging-Tags-Before-Server-Onboarding.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Arc", "Best Practices", "Community", "Maintenance Configurations", "Onboarding", "Patching", "POC", "Resource Management", "Resource Tagging", "Schedules", "Security"]
tags_normalized: ["azure", "azure arc", "best practices", "community", "maintenance configurations", "onboarding", "patching", "poc", "resource management", "resource tagging", "schedules", "security"]
---

Author jyoungii asks the community about best practices for staging and managing Azure tags for patching and onboarding servers, specifically in the context of Azure Arc.<!--excerpt_end-->

## Azure Tag Best Practice: Staging Tags Before Server Onboarding

**Author:** jyoungii

### Summary

The author is conducting a proof-of-concept (POC) with Azure Arc, utilizing Azure resource tags for maintenance configurations. They seek best practices or official documentation regarding the ability to stage or predefine tags, particularly for patching, before servers are onboarded.

---

### Main Questions and Context

- **POC with Azure Arc:** The setup involves using Azure tags for various maintenance tasks, including scheduling and patching.
- **Tag Management Plan:** There is already a plan in place for how to leverage tags effectively.
- **Key Question:** Can tags and associated schedules (like patching windows) be created and managed in Azure *in advance*, before any servers are actually onboarded?
    - The requester wants to create a specific 'Patching' tag, including all relevant keys and even maintenance schedules, so that when servers are onboarded, they can be simply tagged accordingly.
    - They haven’t found a way in the Azure Portal to stage tags or manage them globally before resources exist.
- **Current Understanding:** The author notes that tags seem to be created only when actually assigned to a resource, and wonders whether there’s a way to establish them as reusable sets without having resources already present.

---

### Community Call to Action

- **Seeking Guidance:** The author requests recommendations on Azure tag management best practices, especially with regard to pre-staging tags for systematic patching as infrastructure is incrementally onboarded.
- **Desired Outcome:** Finding a method or feature that allows proactive tag creation, enabling more streamlined and organized patching workflows as new resources are introduced.

---

### Azure Tagging Current Limitations (as implied)

- **Tags in Azure are Resource-Scoped:** Typically, tags must be assigned to resources or resource groups; you cannot manage a central tag catalog out-of-the-box.
- **No Native Tag Pre-Staging Tool:** The request highlights the potential need for better tag governance and predefinition, suggesting a limitation (as of the discussion date) in Azure’s tagging system for pre-creation or cataloging ahead of resource onboarding.
- **Workarounds May Exist:** Often, these patterns are solved with policy, automation, or naming conventions, but not through a dedicated central tag management feature.

---

### References

- [Reddit Discussion Link](https://www.reddit.com/r/AZURE/comments/1mbrt66/azure_tag_best_practice/)
- [Azure Tags Documentation](https://learn.microsoft.com/en-us/azure/azure-resource-manager/management/tag-resources)

---

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mbrt66/azure_tag_best_practice/)
