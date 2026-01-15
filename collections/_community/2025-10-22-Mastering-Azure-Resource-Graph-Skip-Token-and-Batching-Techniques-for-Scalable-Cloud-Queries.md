---
layout: post
title: 'Mastering Azure Resource Graph: Skip Token and Batching Techniques for Scalable Cloud Queries'
author: ankitankit
canonical_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/mastering-azure-queries-skip-token-and-batching-for-scale/ba-p/4463387
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-10-22 08:53:15 +00:00
permalink: /coding/community/Mastering-Azure-Resource-Graph-Skip-Token-and-Batching-Techniques-for-Scalable-Cloud-Queries
tags:
- API Pagination
- Automation
- Azure
- Azure API
- Azure Resource Graph
- Batching
- Cloud Inventory
- Coding
- Community
- DevOps
- DevOps Best Practices
- KQL
- Kusto Query Language
- Large Scale Query
- PowerShell
- Resource Management
- Search AzGraph
- Skip Token
section_names:
- azure
- coding
- devops
---
ankitankit delivers a thorough, actionable walkthrough for cloud engineers on scaling Azure inventory queries with PowerShell, demonstrating efficient use of Skip Token and Batching in Azure Resource Graph.<!--excerpt_end-->

# Mastering Azure Resource Graph: Skip Token and Batching Techniques for Scalable Cloud Queries

Managing massive Azure environments with standard queries can quickly grow overwhelming due to API limits and throttling. ankitankit's guide tackles these challenges head-on, teaching you how to leverage two essential techniques with PowerShell and the Azure Resource Graph (ARG): **Skip Token** for retrieving entire datasets and **Batching** for optimal performance.

## Table of Contents

1. Introduction: Why Standard Queries Struggle at Scale
2. Understanding Data Pagination in Cloud Queries
3. What Is a Skip Token?
    - The Concept
    - PowerShell Example Using Skip Token
4. What Is Batching?
    - The Concept
    - PowerShell Example Using Batching
5. Azure Resource Graph (ARG) Overview
    - Why ARG Uses Skip Token and Batching
    - Combined Example: Skip Token with Batching Across Subscriptions
6. Summary and References

---

## 1. Introduction: Why Standard Queries Don't Work at Scale

In large Azure environments, resource inventory queries are frequently bottlenecked by:

- **Result Limits (Pagination):** APIs limit single-query results (often 1,000 records), forcing you to fetch data in segments.
- **Efficiency Limits (Throttling):** Too many API calls result in reduced performance or even temporary blocks.

**Skip Token** helps retrieve every record, while **Batching** groups queries for speed and reduced risk of throttling.

---

## 2. Understanding Data Pagination in Cloud Queries

APIs rarely return endless streams of data. Instead, they enforce manageable page sizes. To get all resources, you must:

- Detect when the query returns a Skip Token
- Use that token in your follow-up API calls to fetch the subsequent data slices

---

## 3. What Is a Skip Token?

### The Concept

A **Skip Token** (or continuation token) is an API-generated value marking where one data page ends, so your next query continues smoothly where you left off. Azure returns a Skip Token when your results exceed their single-query cap—usually 1,000 items.

### PowerShell Example Using Skip Token

```powershell
# Define the query

$Query = "Resources | project name, type, location"
$PageSize = 1000

$AllResults = @()
$SkipToken = $null # Initialize the token

Write-Host "Starting ARG query..."
do {
    Write-Host "Fetching next page. (Token check: $($SkipToken -ne $null))"
    # 1. Execute the query with the -SkipToken parameter
    $ResultPage = Search-AzGraph -Query $Query -First $PageSize -SkipToken $SkipToken
    # 2. Add current page results
    $AllResults += $ResultPage.Data
    # 3. Get the skip token for next page
    $SkipToken = $ResultPage.SkipToken
    Write-Host " -> Items in this page: $($ResultPage.Data.Count). Total retrieved: $($AllResults.Count)"
} while ($SkipToken -ne $null)
Write-Host "Query finished. Total resources found: $($AllResults.Count)"
```

**Best Practice:** Use a do-while loop as above to ensure you retrieve every result page.

---

## 4. What Is Batching?

### The Concept

**Batching** compiles multiple sub-queries into a single API request. In ARG, you can submit up to 10 different KQL queries at once, often to different subscriptions or scopes. This cuts network overhead and helps you stay under API call rate limits.

| Feature       | Batching                        | Pagination (Skip Token)           |
|-------------- |---------------------------------|-----------------------------------|
| Goal          | Improve efficiency/speed         | Retrieve all data completely      |
| Input         | Multiple different queries       | Single query, with page marker    |
| Result        | One response, all grouped data   | Partial results, plus token       |

### PowerShell Example Using Batching

```powershell
# Define multiple queries to run together

$BatchQueries = @(
    @{ Query = "Resources | where type =~ 'Microsoft.Compute/virtualMachines'"; Subscriptions = "Subscription-A-ID" },
    @{ Query = "Resources | where type =~ 'Microsoft.Network/publicIPAddresses'"; Subscriptions = @("Subscription-B-ID", "Subscription-C-ID") }
)

Write-Host "Executing batch of $($BatchQueries.Count) queries..."
$BatchResults = Search-AzGraph -Batch $BatchQueries
Write-Host "Batch complete. Reviewing results..."
$VMCount = $BatchResults[0].Data.Count
$IPCount = $BatchResults[1].Data.Count
Write-Host "Query 1 (VMs) returned: $VMCount results."
Write-Host "Query 2 (IPs) returned: $IPCount results."
```

---

## 5. Azure Resource Graph (ARG) Overview

**Azure Resource Graph (ARG)** facilitates fast, cross-subscription property queries using Kusto Query Language (KQL). To help at scale, it supports Skip Token and Batching natively:

- Skip Token manages pagination when the dataset exceeds the per-query maximum.
- Batching submits up to 10 sub-queries in one call for efficiency.

### Combined Example: Skip Token and Batching Across Subscriptions

When querying multiple subscriptions with large datasets, combine batching with Skip Token logic to ensure both speed and data completeness:

```powershell
# Define subscriptions to query

$SubscriptionIDs = @("Sub-Alpha-ID", "Sub-Beta-ID")
$KQLQuery = "Resources | project id, name, type, subscriptionId"
$AllResults = @()

Write-Host "Starting batched query across $($SubscriptionIDs.Count) subscriptions..."

# Create initial batch

$CurrentBatch = $SubscriptionIDs | ForEach-Object {
  [PSCustomObject]@{ Query = $KQLQuery; Subscriptions = ---
layout: "post"
title: "Mastering Azure Resource Graph: Skip Token and Batching Techniques for Scalable Cloud Queries"
author: "ankitankit"
excerpt_separator: "<!--excerpt_end-->"
canonical_url: "https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/mastering-azure-queries-skip-token-and-batching-for-scale/ba-p/4463387"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: "2025-10-22 08:53:15 +00:00"
permalink: "2025-10-22-Mastering-Azure-Resource-Graph-Skip-Token-and-Batching-Techniques-for-Scalable-Cloud-Queries.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["API Pagination", "Automation", "Azure", "Azure API", "Azure Resource Graph", "Batching", "Cloud Inventory", "Coding", "Community", "DevOps", "DevOps Best Practices", "KQL", "Kusto Query Language", "Large Scale Query", "PowerShell", "Resource Management", "Search AzGraph", "Skip Token"]
tags_normalized: [["api pagination", "automation", "azure", "azure api", "azure resource graph", "batching", "cloud inventory", "coding", "community", "devops", "devops best practices", "kql", "kusto query language", "large scale query", "powershell", "resource management", "search azgraph", "skip token"]]
---

ankitankit delivers a thorough, actionable walkthrough for cloud engineers on scaling Azure inventory queries with PowerShell, demonstrating efficient use of Skip Token and Batching in Azure Resource Graph.<!--excerpt_end-->

{{CONTENT}}

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/mastering-azure-queries-skip-token-and-batching-for-scale/ba-p/4463387)
 }
}
$Cycle = 0
do {
    Write-Host "`n--- Running Batch Cycle: $($Cycle++) ---"
    $BatchResponse = Search-AzGraph -Batch $CurrentBatch -First 1000
    $NextBatch = @()
    $NewResultsInCycle = 0
    foreach ($Result in $BatchResponse) {
        $SubId = $Result.Subscriptions
        $AllResults += $Result.Data
        $NewResultsInCycle += $Result.Data.Count
        if ($Result.SkipToken) {
            Write-Host " ✅ Skip Token found for Subscription ID: $SubId. Preparing next page request."
            $NextBatch += [PSCustomObject]@{ Query = $KQLQuery; Subscriptions = $SubId; SkipToken = $Result.SkipToken }
        } else {
            Write-Host " 🛑 Query complete for Subscription ID: $SubId."
        }
    }
    Write-Host "Total new resources retrieved this cycle: $NewResultsInCycle"
    $CurrentBatch = $NextBatch
} while ($CurrentBatch.Count -gt 0)
Write-Host "`n--- Script Finished ---"
Write-Host "Final total resource count: $($AllResults.Count)"
```

**Tip:** Always check for returned Skip Tokens and build your next batch accordingly, so no data is missed.

---

## 6. Summary and References

**Goal:** Use Skip Token for data completeness and Batching for performance in Azure Resource Graph queries with PowerShell.

- **Skip Token:** Ensures complete result sets by paginating through large data.
- **Batching:** Groups up to 10 queries into a single call, reducing API and network overhead.
- **Combined Approach:** Delivers both accuracy and speed when querying complex Azure estates.

### References

- [Azure Resource Graph documentation](https://learn.microsoft.com/en-us/azure/governance/resource-graph/overview)
- [Search-AzGraph PowerShell reference](https://learn.microsoft.com/en-us/powershell/module/az.resourcegraph/search-azgraph?view=azps-14.5.0)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/mastering-azure-queries-skip-token-and-batching-for-scale/ba-p/4463387)
