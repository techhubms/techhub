---
primary_section: ml
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
title: Support for default arguments in Fabric User data functions
date: 2026-03-24 10:00:00 +00:00
section_names:
- ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/support-for-default-arguments-in-fabric-user-data-functions/
tags:
- Data Pipelines
- Data Quality
- Datetime
- Default Arguments
- Fabric Data Engineering
- Function Signatures
- ISO 8601
- JSON Serialization
- Microsoft Fabric
- ML
- News
- Pandas DataFrame
- Pandas Series
- Product Reviews
- Python
- Support Tickets
- Telemetry
- UDF
- User Data Functions
---

Microsoft Fabric Blog announces that Fabric User Data Functions (UDF) now support default argument values, explaining supported JSON-serializable types, key limitations, and a practical Python example for adding triage tags in a data quality pipeline.<!--excerpt_end-->

# Support for default arguments in Fabric User Data Functions

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a broader look at the FabCon and SQLCon announcements across Fabric and Microsoft’s database offerings:* https://aka.ms/FabCon-SQLCon-2026-news

The [Fabric user data functions (UDF)](https://learn.microsoft.com/fabric/data-engineering/user-data-functions/user-data-functions-overview) programmability model now supports defining functions with **default argument values**.

This lets you omit parameters when calling a function; missing parameters automatically use their defaults. The goal is cleaner, more concise calls so you can focus on function logic.

## How to write functions with default values

You can define default arguments in Fabric user data functions to make code easier to call and maintain.

Default values support common **JSON-serializable types**, including:

- Strings
- Booleans
- Numbers (`int`, `float`)
- Arrays/lists
- Objects/dictionaries

## Syntax

```python
@udf.function()

def function_name(param1: type = value1, param2: type = value2, listparam: list | None = None, …) -> output_type:
    # function body
```

## Input types supported

The following input types support passing default values.

| JSON type | Python data type |
| --- | --- |
| String | `str` |
| Date/time string | `datetime` (for example, ISO 8601 input parsed to `datetime`) |
| Boolean | `bool` |
| Number | `int`, `float` |
| Array | `list` (e.g., `list[int]`) |
| Object | `dict` |
| Object or array of objects | `pandas.DataFrame`, `pandas.Series` |

## Limitations

- The default value **must be JSON serializable**.
  - Nested lists such as `[1, 2, [3]]` are permitted.
  - Nested sets or tuples are **not supported**.
- For list or dictionary defaults:
  - Prefer using `None` in the signature and assigning the real default inside the function.
- For date/time defaults passed as strings:
  - Use a consistent format (for example, **ISO 8601**).

## Example scenario: Data quality triage tags for a support or reviews pipeline

Records from sources like customer feedback, product reviews, support tickets, telemetry anomalies, or compliance events are ingested into Fabric. Often, records lack metadata such as tags, severity, or review status, which downstream processes need.

This UDF normalizes the dataset by retaining existing tags or marking untagged records as “new” and “unreviewed” for triage.

```python
import fabric.functions as fn

udf = fn.UserDataFunctions()

@udf.function()

def add_tags(record: dict, default_tags: list | None = None) -> dict:
    """
    Adds default tags to a record if no tags exist.

    Args:
        record (dict): Input row, e.g. {"id": 1, "tags": ["priority"]}.
        default_tags (list): Tags applied when record has no tags.

    Returns:
        dict: Record with a final_tags list added.
    """

    if default_tags is None:
        default_tags = ["new", "unreviewed"]

    tags = record.get("tags") or default_tags

    return {**record, "final_tags": tags}
```

### Output when you run the function

If you call `add_tags({"id": 1})` (omitting `default_tags`), the function applies `["new", "unreviewed"]` and returns a record with `final_tags` set.

## Get started

- Learn more: [User data functions programming model](https://learn.microsoft.com/fabric/data-engineering/user-data-functions/python-programming-model)
- Share feedback and ideas: [Fabric Ideas](https://community.fabric.microsoft.com/t5/Fabric-Ideas/idb-p/fbc_ideas)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/support-for-default-arguments-in-fabric-user-data-functions/)

