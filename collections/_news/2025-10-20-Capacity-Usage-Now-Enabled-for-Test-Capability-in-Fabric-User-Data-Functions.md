---
external_url: https://blog.fabric.microsoft.com/en-US/blog/capacity-usage-enabled-date-for-test-capability-in-user-data-functions/
title: Capacity Usage Now Enabled for Test Capability in Fabric User Data Functions
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
date: 2025-10-20 12:00:00 +00:00
tags:
- Capacity Metrics
- Data Architecture
- Data Engineering
- Develop Mode
- Fabric Integration
- Fabric User Data Functions
- General Availability
- Microsoft Fabric
- Power BI Integration
- Python Functions
- Service Limits
- Testing
- Azure
- Machine Learning
- News
section_names:
- azure
- ml
primary_section: ml
---
Microsoft Fabric Blog announces that, effective October 27th, testing User Data Functions in Develop mode will count towards Fabric capacity. The post by the Microsoft Fabric Blog team covers session mechanics, usage metrics, and general availability changes.<!--excerpt_end-->

# Capacity Usage Now Enabled for Test Capability in Fabric User Data Functions

**Author:** Microsoft Fabric Blog

## Overview

User Data Functions (UDFs) in Microsoft Fabric allow you to create and execute managed Python functions across Fabric's data ecosystem. These functions can be embedded into a range of integrations, including Fabric data sources, Power BI reports, Pipelines, and Notebooks, helping to tailor business logic within your data architecture.

## Test Capability Usage Update

As of **October 27th**, when you test UDFs using the Test capability in Develop mode, that activity will now consume your Fabric capacity. This change accompanies the general availability (GA) of User Data Functions.

### How the Test Capability Works

- Test directly from your browser without publishing functions
- Initiates a session for modifying and running code instantly
- Sessions have a 15-minute timeout, extended with activity
- Session resource consumption is reported as 'User Data Functions Portal Test' operations in the Capacity Metrics application

### Previous Approach

- Testing in preview mode before GA (since August 19th) did **not** consume any Fabric capacity, even though metrics were reported

### What's New at GA

- Starting October 27th, the time spent testing UDFs **does** consume your Fabric capacity (no change in rate, just the tracking model)
- The operation continues to appear in the usual Capacity Metrics application view
- No changes to operation names or metrics reporting, just billing for usage
- Enables better visibility and cost management of test activity by tracking consumption

## Related Resources

- [User Data Functions Portal Test operation](https://learn.microsoft.com/fabric/enterprise/fabric-operations#fabric-user-data-functions)
- [Validating UDFs with the Test Capability](https://learn.microsoft.com/fabric/data-engineering/user-data-functions/test-user-data-functions)
- [Service details and limitations](https://learn.microsoft.com/fabric/data-engineering/user-data-functions/user-data-functions-service-limits)
- [General Availability announcement for Fabric User Data Functions](https://blog.fabric.microsoft.com/blog/announcing-fabric-user-data-functions-now-in-general-availability/)

## Summary

This change means test iterations in Develop mode now have measurable impact on your Fabric tenant's available capacity. Current implementation details, rates, and metrics reporting are unchanged, but organizations and developers should factor this resource use into their planning when rapidly testing and iterating on UDFs.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/capacity-usage-enabled-date-for-test-capability-in-user-data-functions/)
