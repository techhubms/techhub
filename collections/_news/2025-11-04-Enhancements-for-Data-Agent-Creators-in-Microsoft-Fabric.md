---
layout: "post"
title: "Enhancements for Data Agent Creators in Microsoft Fabric"
description: "This post details significant new features for Data Agent creators in Microsoft Fabric, focusing on improved debugging tools, higher configuration limits, enhanced SDK validation, markdown support for instructions, and a flexible multi-tasking workflow. These upgrades help developers iterate quickly, author clear logic, and deliver more reliable data-driven agents."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/creator-improvements-in-the-data-agent/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2025-11-04 11:00:00 +00:00
permalink: "/news/2025-11-04-Enhancements-for-Data-Agent-Creators-in-Microsoft-Fabric.html"
categories: ["AI", "Azure", "ML"]
tags: ["Agent Authoring", "Agent Configuration", "AI", "Azure", "Data Agent", "Data Science", "Debugging", "Development Tools", "Example Queries", "Few Shot Learning", "KQL", "Markdown", "Microsoft Fabric", "ML", "Multi Tasking", "News", "SDK", "SQL Validation"]
tags_normalized: ["agent authoring", "agent configuration", "ai", "azure", "data agent", "data science", "debugging", "development tools", "example queries", "few shot learning", "kql", "markdown", "microsoft fabric", "ml", "multi tasking", "news", "sdk", "sql validation"]
---

The Microsoft Fabric Blog introduces powerful improvements for Data Agent creators, helping developers refine, debug, and structure their agents more efficiently using enhanced SDK tools and markdown-based configuration.<!--excerpt_end-->

# Creator Improvements in the Data Agent

Microsoft Fabric Blog announced a suite of enhancements for Data Agent creators, aiming to streamline the process of authoring, debugging, and iterating on intelligent data agents within the Fabric ecosystem.

## New Debugging Tools

- **Run Steps View:** Lets creators inspect which example queries were retrieved and used for a user's question. This visibility ensures that the right examples are applied or highlights areas for more targeted few-shot questions.
- **Diagnostic Summary:** Allows downloading detailed traces of agent reasoning, exposing internal logic not visible in the basic run step view. Useful for in-depth troubleshooting or sharing context with support teams.

## Higher Limits for Configurations

- Data source instructions for eventhouses: Increased from 5,000 to 15,000 characters.
- SQL and KQL example queries: Expanded from 1,000 to 5,000 characters.
- These new limits let developers provide richer context and more complex logic for agents.

## Fabric Data Agent SDK Enhancements

- New tools in the SDK, like `evaluate_few_shot_examples()`, let creators validate natural language/SQL pairs, summarizing which examples aligned well or failed.
- Convert results into DataFrames—enabling easy review and further iteration.
- This validation process supports continuous agent improvement and more accurate SQL generation.

### Example Code

```python
result = evaluate_few_shot_examples(
    examples,
    llm_client=llm_client,
    model_name=model_name,
    batch_size=20,
    use_fabric_llm=True
)
success_df = cases_to_dataframe(result.success_cases)
failure_df = cases_to_dataframe(result.failure_cases)
display(success_df)
display(failure_df)
```

For a full example, visit the [Data Agent Example Queries](https://learn.microsoft.com/fabric/data-science/data-agent-example-queries) documentation.

## Markdown Editor for Instructions

- Data Agent and data source instructions now support Markdown, making documentation clear and easily maintained.
- Markdown empowers creators to add structured lists, headings, tables, and logic clarifications for better agent behavior documentation.
- Templates and best practices are available in the [official documentation](https://learn.microsoft.com/fabric/data-science/data-agent-configurations).

## Improved Multi-Tasking Workflow

- Developers can now chat with and configure their Data Agent in the same session, without losing schema or context.
- Seamless switching between chat and configuration accelerates iteration and ensures context retention.

## Impact for Developers

These updates make it easier and quicker for developers to create, debug, and iterate on data agents—leading to more intelligent, reliable, and understandable agents powered by Microsoft Fabric. For more, see the official [configuration guide](https://learn.microsoft.com/fabric/data-science/data-agent-configurations).

---

*Author: Microsoft Fabric Blog*

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/creator-improvements-in-the-data-agent/)
