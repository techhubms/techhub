---
layout: "post"
title: "PIM for Groups: Missing \"Permanently Eligible\" Option When Assigning Roles"
description: "The author describes an issue encountered while setting up Azure Privileged Identity Management (PIM) for Groups. They were previously able to designate users as \"permanently eligible\" for specific admin roles, but this option is now missing in a newly created group. They are seeking insight into possible causes."
author: "velkkor"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/AZURE/comments/1mhegiy/pim_for_group_no_permanently_eligible_option/"
viewing_mode: "external"
feed_name: "Reddit Azure"
feed_url: "https://www.reddit.com/r/azure/.rss"
date: 2025-08-04 14:08:20 +00:00
permalink: "/2025-08-04-PIM-for-Groups-Missing-Permanently-Eligible-Option-When-Assigning-Roles.html"
categories: ["Azure"]
tags: ["Access Control", "Azure", "Community", "Exchange Recipient Administrator", "Groups", "Identity Management", "Microsoft Entra", "Permanently Eligible", "PIM", "Privileged Identity Management", "Role Assignments", "SharePoint Administrator", "User Administrator"]
tags_normalized: ["access control", "azure", "community", "exchange recipient administrator", "groups", "identity management", "microsoft entra", "permanently eligible", "pim", "privileged identity management", "role assignments", "sharepoint administrator", "user administrator"]
---

velkkor is troubleshooting an issue in Azure PIM where the "permanently eligible" option for group administrator roles is missing when assigning users, despite it being available previously.<!--excerpt_end-->

## Issue Description

The author is implementing Azure Privileged Identity Management (PIM) for managing Groups within Microsoft Entra. A few weeks prior, they created a group, assigning roles such as User Administrator, Exchange Recipient Administrator, and SharePoint Administrator. At that time, when adding users as eligible for these roles, the interface provided an option to make the assignments "permanently eligible."

However, in a recent attempt to set up another test group—this time with just User Administrator and Exchange Recipient Administrator roles—the author discovered that the "permanently eligible" checkbox or option is missing. They are unable to assign users as permanently eligible for these roles in the new group.

## Troubleshooting Considerations

- The discrepancy occurs between two different group setups, each with similar but not identical configurations.
- The author has not changed roles drastically, but the presence or absence of SharePoint Administrator as a role is the main difference noticed.
- They are unsure whether this issue is caused by overlap with users or roles assigned in other groups, changes in Azure PIM policy, or other factors.

## Possible Factors to Investigate

- **Group Creation Method**: Whether the two groups were created with the exact same settings.
- **Role Combinations**: If certain combinations of roles enable or hide the permanently eligible option due to policy or licensing constraints.
- **Policy or Feature Updates**: Whether Microsoft recently changed how "permanently eligible" assignments function in PIM for Groups.
- **User or Group Overlap**: If users or the group itself have conflicting assignments elsewhere.
- **Licensing or Permissions**: Whether the Azure subscription or tenant configuration affects PIM capabilities or eligibility options.

## What to Check Next

1. Review documentation for recent changes to PIM for Groups around eligible assignments.
2. Compare all group creation and role assignment steps for consistency.
3. Ensure you have sufficient permissions and licenses for PIM permanent eligibility.
4. Test with a new group and different role combinations to observe if the option reappears.
5. Consider escalating to Microsoft support or the Azure community if the issue persists or appears to be a feature regression.

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mhegiy/pim_for_group_no_permanently_eligible_option/)
