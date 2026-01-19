---
layout: "post"
title: "Azure Boards Adds Custom Field Filters in Private Preview"
description: "This announcement introduces a limited private preview for Azure Boards, letting users filter backlogs and Kanban boards by additional custom fields. The post explains how teams can enhance board views, the limitations of the feature, and how to join the private preview program, aiming for early feedback before general release."
author: "Dan Hellem"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/devops/azure-boards-additional-field-filters-private-preview/"
viewing_mode: "external"
feed_name: "Microsoft DevOps Blog"
feed_url: "https://devblogs.microsoft.com/devops/feed/"
date: 2026-01-19 20:22:53 +00:00
permalink: "/2026-01-19-Azure-Boards-Adds-Custom-Field-Filters-in-Private-Preview.html"
categories: ["Azure", "DevOps"]
tags: ["Agile", "Azure", "Azure & Cloud", "Azure Boards", "Azure DevOps", "Backlog Management", "Customization", "DevOps", "Feature Announcement", "Field Filters", "Kanban", "News", "Private Preview", "Software Development Process", "Work Item Tracking"]
tags_normalized: ["agile", "azure", "azure and cloud", "azure boards", "azure devops", "backlog management", "customization", "devops", "feature announcement", "field filters", "kanban", "news", "private preview", "software development process", "work item tracking"]
---

Dan Hellem introduces an Azure Boards private preview that enables filtering by additional custom fields on backlog and Kanban boards, offering teams more control and flexibility.<!--excerpt_end-->

# Azure Boards Adds Custom Field Filters (Private Preview)

**Author:** Dan Hellem

Azure Boards is launching a limited private preview to let teams filter backlog and Kanban boards using additional custom fields—a highly requested enhancement from the developer community. This update aims to help teams better tailor their work views and focus on tasks that matter most.

## Key Features

- **Custom Field Filters:** Users can add any field already displayed on a backlog column or Kanban card to the filter options, in addition to the standard filters.
- **Real-Time Effect:** Changes made in filter settings immediately reflect in the filters control, enabling more focused board interactions.

![Azure Boards filter options example](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2026/01/blog-filters-1.webp)

## Limitations

- Large text fields are not available as filters.
- Fields that always contain unique values (e.g., `Stack Rank`) are excluded from being used as filters.
- Filterable fields must first be visible as a backlog column or card field.
- Filters are supported only on backlog and board pages (not on sprint backlogs, sprint boards, queries, or work items hub).
- When sharing a filtered URL, anyone who lacks the same field configuration will not see those filters applied.

## How to Join the Private Preview

- Email Dan Hellem at dahellem@microsoft.com with your organization name in the format `dev.azure.com/{organization}`.
- Enrollment is open until **February 6, 2026**.
- Once enabled, the feature is available to all users within the selected organization.

## Next Steps

Feedback from this private preview phase will help guide improvements before general availability. The Azure DevOps team looks forward to input from early adopters to refine the experience.

For more details, visit the original [Azure DevOps Blog post](https://devblogs.microsoft.com/devops/azure-boards-additional-field-filters-private-preview/).

This post appeared first on "Microsoft DevOps Blog". [Read the entire article here](https://devblogs.microsoft.com/devops/azure-boards-additional-field-filters-private-preview/)
