---
external_url: https://devblogs.microsoft.com/devops/azure-boards-additional-field-filters-private-preview/
title: Azure Boards Adds Custom Field Filters in Private Preview
author: Dan Hellem
feed_name: Microsoft DevOps Blog
date: 2026-01-19 20:22:53 +00:00
tags:
- Agile
- Azure & Cloud
- Azure Boards
- Azure DevOps
- Backlog Management
- Customization
- Feature Announcement
- Field Filters
- Kanban
- Private Preview
- Software Development Process
- Work Item Tracking
section_names:
- azure
- devops
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
