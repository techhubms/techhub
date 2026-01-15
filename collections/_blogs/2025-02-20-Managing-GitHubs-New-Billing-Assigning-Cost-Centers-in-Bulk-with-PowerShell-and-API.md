---
layout: post
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: Jesse Houwing
canonical_url: https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/
viewing_mode: external
feed_name: Jesse Houwing's Blog
feed_url: https://jessehouwing.net/rss/
date: 2025-02-20 14:10:17 +00:00
permalink: /devops/blogs/Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API
tags:
- API
- Automation
- Billing
- Blogs
- Cost Centers
- DevOps
- Enterprise
- GitHub
- GitHub Enterprise
- Organizational Management
- PowerShell
- Resource Assignment
section_names:
- devops
---
In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

# Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API

*By Jesse Houwing*

## Overview

With GitHub's rollout of their new Billing experience for enterprise customers, it is now possible to assign costs to defined cost centers. This allows organizations to more precisely track and manage expenditure across repositories, users, organizations, and the enterprise as a whole. Jesse Houwing shares an in-depth look at configuring and automating these assignments, particularly at scale.

## What Are Cost Centers in GitHub?

Cost centers enable the allocation of costs to specific organizational resources. Available resource categories include:

- **Repositories**: Charges for action minutes, Codespaces, LFS storage, etc.
- **Users**: Costs like enterprise seat licenses, Copilot, Advanced Security.
- **Organizations**: Costs from unassigned repositories, packages storage, network traffic.
- **Enterprise**: Costs for resources not assigned to a specific cost center.

By linking these resources to cost centers, organizations can distribute their GitHub bills according to their internal accounting structures.

## Assigning Resources

Linking organizations and repositories can be accomplished via the Cost Center UI in the GitHub Enterprise Admin portal:

![Assigning Organizations and Repositories](https://jessehouwing.net/content/images/2025/02/image-2.png)

However, **user assignments** require use of the GitHub API. This is especially challenging for large enterprises with hundreds of members.

### Key API Constraints

- **Batch Limit**: You can only assign users in batches of 50 (GitHub Support confirmed).
- **Reassignment**: A user can only be assigned to one cost center at a time; to change cost centers, two operations are needed.
- **Eventual Consistency**: API operations may take a few seconds to reflect.
- **API Rate Limits**: Excessive individual calls risk hitting rate limits; batching is critical for efficiency.

## PowerShell Automation for Bulk Assignment

To streamline mass user assignments, Jesse developed a PowerShell wrapper to interact with the API efficiently. The script minimizes unnecessary API calls by checking existing assignments and batches user modifications.

### Example PowerShell Function

```powershell
function Update-CostCenterResources {
  param(
    [Parameter(Mandatory=$true)] [string[]]$Handles,
    [Parameter(Mandatory=$true)] [ValidateSet('Add','Delete')] [string]$Action,
    [Parameter(Mandatory=$true)] $CostCenter,
    [Parameter(Mandatory=$true)] [string]$Enterprise
  )

  switch ($Action) {
    'Add' {
      $method = 'POST'
      $Handles = $Handles | Where-Object {
        $handle = ---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)

        return (($costCenter.resources | ?{ ---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
.type -eq "User" } | ?{---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
.name -eq $handle }).Count -eq 0)
      }
    }
    'Delete' {
      $method = 'DELETE'
      $Handles = $Handles | Where-Object {
        $handle = ---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)

        return (($costCenter.resources | ?{ ---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
.type -eq "User" } | ?{---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
.name -eq $handle }).Count -gt 0)
      }
    }
  }

  # Batch API calls for efficiency
  $count = 0
  do {
    $batch = $Handles | Select-Object -Skip $count -First 50
    $count += $batch.Count
    if ($batch.Count -gt 0) {
      $body = @{ users = [string[]]$batch }
      ---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
 = ($body | ConvertTo-Json) |
        gh api --method $method /enterprises/$Enterprise/settings/billing/cost-centers/$($CostCenter.id)/resource --input -
    }
  } while ($batch.Count -gt 0)
}
```

**Usage Example:**

```powershell
$enterprise = "xebia"
$costCenters = (invoke-gh -fromJson -- api /enterprises/$enterprise/settings/billing/cost-centers).costCenters
$costCenterNL = $costCenters | ?{ ---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
.name -eq "Netherlands" }
$handles = @("jessehouwing", "jessehouwing-demo")

# Remove users from other cost centers

$costCenters | ?{ ---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
.id -ne $costCenterNL.id } |
  ?{ ---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
 | ?{ $.resources | ?{ ---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
.type -eq "User" -and ---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
.name -in $handles } } } |
  %{ Update-CostCenterResources -handles $handles -action "Delete" -CostCenter ---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
 -Enterprise $enterprise }

# Assign users to the new cost center

Update-CostCenterResources -handles $handles -action "Add" -CostCenter $costCenterNL -Enterprise $enterprise
```

## Querying Current Assignments

To check a user's current cost center:

```powershell
$handle = "jessehouwing"
$enterprise = "xebia"

# Using Cost Center API

$costCenters = (gh api /enterprises/$enterprise/settings/billing/cost-centers | ConvertFrom-Json).costCenters
$currentCostCenter = $costCenters | ?{ ---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
.resources | ?{ ---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
.type -eq "User" -and ---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
.name -eq $handle } }

# Using Assigned Seats API

$enterpriseUsers = gh api https://api.github.com/enterprises/$enterprise/consumed-licenses --jq '.users[]' --paginate | ConvertFrom-Json
$currentCostCenter = ($enterpriseUsers | ?{ ---
layout: "post"
title: "Managing GitHub's New Billing: Assigning Cost Centers in Bulk with PowerShell and API"
author: "Jesse Houwing"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: https://jessehouwing.net/rss/
date: "2025-02-20 14:10:17 +00:00"
permalink: "2025-02-20-Managing-GitHubs-New-Billing-Assigning-Cost-Centers-in-Bulk-with-PowerShell-and-API.html"
categories: ["DevOps"]
tags: ["API", "Automation", "Billing", "Cost Centers", "DevOps", "Enterprise", "GitHub", "GitHub Enterprise", "Organizational Management", "Blogs", "PowerShell", "Resource Assignment"]
tags_normalized: [["api", "automation", "billing", "cost centers", "devops", "enterprise", "github", "github enterprise", "organizational management", "blogs", "powershell", "resource assignment"]]
---

In this guide, Jesse Houwing discusses how to organize GitHub enterprise billing using new cost center functionality, focusing on automation and best practices for large teams.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
.github_com_login -eq $handle }).github_com_cost_center
```

By integrating these API calls with organizational metadata (such as from Azure EntraID), bulk assignment is streamlined.

## Results and Limitations

When cost centers were assigned (in this example, on January 9th), all subsequent costs were properly allocated, as visualized here:

![Cost Distribution Post-Assignment](https://jessehouwing.net/content/images/2025/02/image-5.png)

However, it is currently **not possible to retroactively assign costs** prior to the date of assignment.

## Conclusion

The new cost center functionality in GitHub's billing is a powerful tool for enterprise cost management, though scaling assignments requires API automation due to current UI limitations. Combining the API with scripting (e.g., via PowerShell) enables efficient, repeatable cost center management, even in organizations with hundreds of members.

---

*Author: Jesse Houwing*

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/githubs-new-billing-assigning-cost-centers-in-bulk/)
