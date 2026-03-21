---
tags:
- Bike Sharing Analytics
- Data Exploration
- DB Explorer
- Eventhouse
- FabCon
- Function Preview
- KQL
- Log Analytics
- Microsoft Fabric
- ML
- News
- Parameterized Functions
- Query Results Preview
- Real Time Intelligence
- Search And Sorting
- SQLCon
- Stored Functions
title: 'Instantly Run and Preview Functions in Microsoft Fabric Eventhouse: No Code Required'
date: 2026-03-20 13:00:00 +00:00
section_names:
- ml
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/instantly-run-and-preview-functions-in-microsoft-fabric-eventhouse-no-code-required/
primary_section: ml
---

Microsoft Fabric Blog announces a preview feature for Fabric Eventhouse that lets you inspect stored function definitions and run/preview results directly in DB Explorer, avoiding manual KQL and making it easier to test parameters, validate outputs, and explore large sets of functions.<!--excerpt_end-->

# Instantly Run and Preview Functions in Microsoft Fabric Eventhouse: No Code Required

*If you haven’t already, check out Arun Ulag’s hero blog “*[FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news)*” for a complete look at all of our FabCon and SQLCon announcements across both Fabric and our database offerings.*

Before, using an Eventhouse function required you to write KQL queries by hand. You had to enter the function name in a query set, format the parameters correctly, and execute it just to see the results. If you needed to view the function body or its metadata, you had to run another command separately.

That’s no longer necessary.

With the new **Preview Functions** capability in **Microsoft Fabric Eventhouse**, you can:

- Open the function definition
- Run the function
- Instantly preview its results

All without manual KQL, parameter guesswork, or extra commands.

## Why this matters

Eventhouse functions are effective, but day-to-day work gets harder as:

- Databases grow
- Teams transition
- You inherit functions written by other people

Instead of guessing what a function does (or writing a test query from scratch just to validate it), you can now:

- View the function definition instantly
- Run the function and preview results with a single click
- Test parameterized functions interactively
- Browse your function list with search and sorting

This is meant to remove friction from workflows like:

- Exploring unfamiliar logic
- Validating outputs before building reports
- Troubleshooting unexpected results

## How to preview a function

1. In **DB Explorer**, expand **Functions**.
2. Select a function.
   - A read-only version of the function opens.
3. Select **Preview results** to run the function and see output.
4. If the function has parameters:
   - Enter parameter values
   - Preview results based on your inputs

The preview shows **up to 100 records**.

![Database query interface showing recent system logs with timestamps, service names, operation names, and error codes](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/word-image-33973-1.png)

*Figure: Query results showing recent system logs, including timestamps, services, operations, and error codes filtered by a selected time range.*

## Explore all stored functions

You can view a complete list of stored functions, including:

- Folder
- Description
- Optional sorting

There’s also built-in search to help find specific functions, which helps with discovery and navigation in larger databases.

![Eventhouse analytics visuals and examples around bike sharing data: locations, availability, and utilization](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/word-image-33973-2.png)

*Figure: Visualizing bike sharing analytics in Eventhouse with instant run and preview—enabling rapid data exploration without writing code.*

## Conclusion

The new **Run & Preview Functions** feature in **Microsoft Fabric Eventhouse** lets you:

- Inspect function definitions
- Preview function results

Without writing KQL or manually handling parameters.

Learn more: [Create and edit stored functions](https://learn.microsoft.com/fabric/real-time-intelligence/create-functions)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/instantly-run-and-preview-functions-in-microsoft-fabric-eventhouse-no-code-required/)

