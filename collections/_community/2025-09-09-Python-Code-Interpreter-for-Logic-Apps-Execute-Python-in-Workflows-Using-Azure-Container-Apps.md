---
layout: "post"
title: "Python Code Interpreter for Logic Apps: Execute Python in Workflows Using Azure Container Apps"
description: "This community post by DivSwa introduces the new Python Code Interpreter capability for Logic Apps, enabling developers to execute Python code directly within Logic Apps workflows. Powered by Azure Container Apps, this feature lets users leverage natural language to analyze structured data, clean and transform files like CSV or JSON, and receive insights—from summaries to visualizations—without writing manual scripts. The post explains setup, security, real-world automation use cases, and provides links for getting started."
author: "DivSwa"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcement-python-code-interpreter-in-logic-apps-is-now-in/ba-p/4452239"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-09-09 18:10:15 +00:00
permalink: "/community/2025-09-09-Python-Code-Interpreter-for-Logic-Apps-Execute-Python-in-Workflows-Using-Azure-Container-Apps.html"
categories: ["AI", "Azure", "Coding"]
tags: ["Agent Loop", "AI", "AI Agents", "Azure", "Azure Container Apps", "Business Intelligence", "Code Execution", "Coding", "Community", "CSV Analysis", "Data Analysis", "Data Transformation", "Logic Apps", "Natural Language Processing", "Python", "Python Code Interpreter", "Visualization", "Workflow Automation"]
tags_normalized: ["agent loop", "ai", "ai agents", "azure", "azure container apps", "business intelligence", "code execution", "coding", "community", "csv analysis", "data analysis", "data transformation", "logic apps", "natural language processing", "python", "python code interpreter", "visualization", "workflow automation"]
---

DivSwa details how developers can now use Python Code Interpreter in Logic Apps workflows, powered by Azure Container Apps, to automate structured data analysis using natural language prompts.<!--excerpt_end-->

# Python Code Interpreter for Logic Apps: Execute Python in Workflows Using Azure Container Apps

**Author:** DivSwa  
**Published:** Sep 09, 2025  
**Version:** 2.0

## Overview

As AI agents mature, they are expected to process more than simple text—they must analyze large, structured datasets (like CSV, Excel, or JSON files), reason about patterns, and even perform custom computations from natural language instructions. The new Python Code Interpreter for Logic Apps brings this capability into Logic Apps workflows, eliminating manual scripting and making data-driven automation accessible—even for non-developers.

## Why Is This Important?

Businesses struggle with fragmented, large data—often in formats like CSV or spreadsheets. Transforming this raw data into actionable insights typically requires data cleaning, custom logic, and visualization, which is labor-intensive and error-prone, especially for those without data engineering skills.

## What’s New: Python Code Execution in Logic Apps

Microsoft now offers Python code execution within Logic Apps, powered by Azure Container Apps (ACA) session pools. Developers (or the Logic Apps agent loop itself via LLMs) can:

- Use natural language prompts to define tasks
- Generate and execute Python code automatically
- Analyze uploaded datasets (CSV, JSON, etc.)
- Get back computations, visualizations, forecasts, and summaries—all within the existing workflow

This mirrors advanced data analysis tools, letting users type "Find the top 5 products by revenue" or "Forecast demand by region" and receive instant results.

## Typical Use Cases

- **Sales/Marketing:** Upload sales spreadsheets and instantly analyze top performers, regional splits, or forecast trends
- **Finance:** Auto-process expense reports, spot anomalies, produce quarterly breakdowns
- **Operations:** Clean large log files, find reliability issues, obtain actionable operational insights
- **Business Data Exploration:** Ask complex questions like “Which region had the highest YoY growth?” with zero code

## Security and Architecture Details

- Code execution uses Azure Container Apps session pool. These sessions are isolated with Hyper-V boundaries and can be configured with network isolation—ensuring data does not leave your defined boundaries.
- Developers can author their own Python code or let the AI agent generate it.
- Datasets are uploaded securely to the session and referenced in Python code.

## Getting Started

1. In Logic Apps, add the new action for executing Python code.
2. Connect to an Azure Container Apps session pool.
3. Author your code, or use natural language for the agent to generate it.
4. Optionally upload your dataset to the session (e.g., CSV/Excel/JSON).
5. Run your workflow and get instant insights.

For complete setup steps, see the official documentation: [Python Code Interpreter in Logic Apps (MS Learn)](https://learn.microsoft.com/azure/logic-apps/connectors/code-interpreter-python-container-apps-session)

Questions or feedback? [Contact the team](http://aka.ms/la/feedback).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcement-python-code-interpreter-in-logic-apps-is-now-in/ba-p/4452239)
