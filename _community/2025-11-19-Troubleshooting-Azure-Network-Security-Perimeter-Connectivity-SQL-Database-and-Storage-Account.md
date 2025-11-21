---
layout: "post"
title: "Troubleshooting Azure Network Security Perimeter Connectivity: SQL Database and Storage Account"
description: "This discussion by Antonio Buonaiuto explores a scenario where two resources, an Azure SQL Database and a Storage Account, are assigned to the same Network Security Perimeter (NSP) in Azure. Despite both resources being within the perimeter and public connectivity being denied, he encounters challenges with SQL accessing storage for bulk data operations. The post compares the behavior with Azure Key Vault, considers the impact of resource preview/GAs, and seeks community insights on expected NSP behavior and Azure networking implementation nuances."
author: "Antonio Buonaiuto"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-networking/what-would-be-the-expected-behavior-for-an-nsp/m-p/4471260#M748"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-19 08:57:53 +00:00
permalink: "/2025-11-19-Troubleshooting-Azure-Network-Security-Perimeter-Connectivity-SQL-Database-and-Storage-Account.html"
categories: ["Azure", "Security"]
tags: ["Azure", "Azure Networking", "Azure Platform", "Azure SQL Database", "Azure Storage Account", "Bulk Insert", "Community", "Data Security", "Firewall Rules", "GA", "Key Vault", "Network Security Perimeter", "NSP", "Preview Features", "Resource Connectivity", "Security"]
tags_normalized: ["azure", "azure networking", "azure platform", "azure sql database", "azure storage account", "bulk insert", "community", "data security", "firewall rules", "ga", "key vault", "network security perimeter", "nsp", "preview features", "resource connectivity", "security"]
---

Antonio Buonaiuto describes a networking issue in Azure where an SQL Database can't access a Storage Account within the same Network Security Perimeter, detailing his troubleshooting steps and raising questions about expected Azure NSP behavior.<!--excerpt_end-->

# Networking Troubles in Azure Network Security Perimeter (NSP)

Antonio Buonaiuto shares his experience using Azure's Network Security Perimeter (NSP) to secure communication between an Azure SQL Database and a Storage Account. Both resources were assigned to the same NSP, which should deny public connectivity and enforce perimeter-level access control.

## Problem Scenario

- **Setup**: Two resources in NSP—Azure SQL Database and a Storage Account.
- **Operation**: Using BULK INSERT in SQL to load data from `sample_data.csv` in the storage account.
- **Expected**: Resources within the same NSP should communicate directly.

## Observed Behavior

- Azure SQL **cannot access Storage Account** within the same NSP by default.
- To resolve, Antonio added:
  1. An **outbound rule** in NSP for SQL to reach the storage account FQDN.
  2. An **inbound rule** in NSP to permit SQL's public IP.
- After both rules, SQL could successfully perform data transfer.

## Comparison with Azure Key Vault

- Key Vault in the same NSP *does* communicate with Storage Account *without additional rules*.
- Key Vault (GA) works as expected, while Azure SQL (currently in Preview for NSP) does not.

## Community Questions

Antonio asks if the behavior is due to:

- Feature instability/unavailability for Azure SQL in NSP (since it's in Preview)
- Missing configuration
- Or if perhaps this is by design/expected in the current Azure implementation

## Lessons and Takeaways

- Azure security features in Preview may not behave identically to GA features.
- Even resources in the same NSP may require explicit connectivity rules depending on their current support and integration status.
- Key Vault's integration with NSP appears more seamless, possibly due to maturity and GA status.
- It is vital to consult Azure documentation, monitor preview feature announcements, and explicitly test security boundaries rather than rely on assumed behaviors.

## Discussion Prompt

Have others observed similar issues with Preview resources in Azure NSP? Is additional rule configuration typical for resources like Azure SQL, or is this an implementation gap being addressed?

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking/what-would-be-the-expected-behavior-for-an-nsp/m-p/4471260#M748)
