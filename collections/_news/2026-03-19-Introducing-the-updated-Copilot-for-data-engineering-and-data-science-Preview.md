---
feed_name: Microsoft Fabric Blog
section_names:
- ai
- ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/introducing-the-updated-copilot-for-data-engineering-and-data-science-preview/
title: Introducing the updated Copilot for data engineering and data science (Preview)
tags:
- /fix Command
- AI
- Apache Spark
- Code Generation
- Context Awareness
- Copilot in Fabric
- Data Engineering
- Data Quality
- Data Science
- Data Shuffles
- Debugging
- Diff Review
- Fabric Notebooks
- Join Optimization
- Lakehouse
- Microsoft Fabric
- ML
- News
- Notebook Workflows
- Performance Tuning
- Preview Feature
- Refactoring
- Root Cause Analysis
date: 2026-03-19 16:15:00 +00:00
primary_section: ai
author: Microsoft Fabric Blog
---

The Microsoft Fabric Blog (co-authors Jene Zhang, Jenny Jiang, and Qixiao Wang) introduces an updated Copilot experience in Fabric notebooks, focusing on context-aware help, performance guidance, and in-notebook diagnostics for Spark-based data engineering and data science workflows.<!--excerpt_end-->

# Introducing the updated Copilot for data engineering and data science (Preview)

*If you haven’t already, check out Arun Ulag’s hero blog “[FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news)” for a complete look at all of our FabCon and SQLCon announcements across both Fabric and our database offerings.*

Co-authors: Jene Zhang, Jenny Jiang, Qixiao Wang

## Context awareness built in and immediately accessible

Microsoft Fabric notebooks offer a Copilot feature that adapts to complex tasks, providing context-aware support as soon as you start using them.

Copilot no longer requires a session to be started or for you to re-explain what you’re working on. Instead, it automatically understands your:

- Workspace
- Attached Lakehouse
- Notebook structure
- Execution environment

This is meant to provide always-on, notebook-aware assistance across entire notebook workflows—helping you write larger blocks of code faster, understand complex notebooks, and make changes with more confidence while staying in control of what context is used.

### What this enables

Copilot has built-in awareness of your workspace and notebook context. It can understand:

- Notebook flow
- Attached Lakehouse schemas and tables
- Files
- Runtime state

It’s positioned to help with multi-step notebook work end-to-end (planning, generating, refining, and validating code) as your notebook evolves.

You can use Copilot to:

- **Develop faster**: Generate cohesive code across multiple steps, not just single cells.
- **Build complete workflows**: Create multi-step transformations and validation logic directly in your notebook.
- **Refactor with confidence**: Rewrite existing logic to reduce duplication and improve readability.
- **Understand at scale**: Summarize complex or unfamiliar notebooks quickly.
- **Debug and troubleshoot sooner**: Use *Fix with Copilot* to get an error summary and recommended fix, then iterate with full context.

The stated goal is fewer partial solutions, less back-and-forth editing, and faster iteration as notebooks grow in complexity.

## Understand performance as you build

Copilot in Fabric notebooks is also described as helping you identify potential performance issues earlier.

When prompted, Copilot can draw on your notebook’s structure, data, and execution environment to surface opportunities such as:

- More efficient join strategies
- Avoiding unnecessary data shuffles
- Refactoring logic into reusable functions
- Highlighting potential data quality issues

Unlike static suggestions, these recommendations are presented as grounded in runtime behavior (for example: data size, joins, and how the notebook runs).

## Diagnose code failures

When a cell execution fails (or a failed Spark job is detected), a **Fix with Copilot** button appears below the affected cell. Selecting it opens a Copilot panel that pulls in:

- The cell code
- Running context
- Spark execution details

This is intended to remove the need for manual copy/paste when debugging.

![Fix with Copilot button appearing below a failed notebook cell, opening Copilot to analyze the error and suggest a fix.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/fix-with-copilot-button-appearing-below-a-failed-n.jpeg)

*Figure: Fix with Copilot provides error summary and suggested fixes.*

Copilot then generates a diagnostic summary drawing from Microsoft Fabric documentation, Spark community knowledge, and other public resources, including:

- A clear summary of the error message
- A root cause analysis
- Actionable recommendations for fixes

If a fix requires code changes, Copilot can apply them automatically. You can review a diff of the changes and choose to accept or revert.

Beyond cell-level diagnostics, you can also trigger analysis using the **/Fix** command directly in Copilot chat, targeting a specific cell or the entire notebook.

## Getting started

Copilot is described as working alongside you as you write, understand, refine, and improve your notebook—helping you move from code to insight in Microsoft Fabric.

To learn more, see the documentation: [Copilot for Data Engineering and Data Science documentation](https://go.microsoft.com/fwlink/?linkid=2354708).


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/introducing-the-updated-copilot-for-data-engineering-and-data-science-preview/)

