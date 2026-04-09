---
section_names:
- ai
- azure
- github-copilot
primary_section: github-copilot
title: 'Next-Level SQL in VS Code: GitHub Copilot Custom Instructions, Plan Mode & Skills | Data Exposed'
feed_name: Microsoft Developer YouTube
external_url: https://www.youtube.com/watch?v=noEj1AhZUwE
author: Microsoft Developer
date: 2026-04-09 16:01:17 +00:00
tags:
- Agent Mode
- AI
- Azure
- Azure SQL
- Azure SQL Database
- Cloud Computing
- Copilot Debug Panel
- Custom Instructions
- Database Schema Design
- Dev
- Development
- GitHub Copilot
- Instruction Files
- LearnSQL
- LLM Request Payload
- Microsoft
- MSSQL Extension
- Plan Mode
- PRD
- Prompt Context
- Schema Designer
- Slash Commands
- SQL
- SQL Server
- T SQL
- Tech
- Technology
- Vector Search
- Videos
- VS Code
---

Microsoft Developer walks through using GitHub Copilot with the VS Code MSSQL extension to control Copilot context for T-SQL—covering custom instruction files, Plan Mode/Agent Mode for schema design, skills as slash commands, and inspecting Copilot’s debug payload while targeting SQL Server 2025 and Azure SQL Database.<!--excerpt_end-->

## Overview
This Data Exposed episode shows how to make **GitHub Copilot** produce **team-aligned T-SQL** in **VS Code** by controlling the context Copilot receives via the **MSSQL extension**.

The demo flow starts with a clean workspace (no context), then incrementally adds:

- A **custom instruction file** to teach your team’s T-SQL conventions
- **Plan Mode** to design a data model from a **PRD (Product Requirements Document)**
- **Agent Mode** + **Schema Designer** to generate/shape schema while connected to a real database
- A **skills file** that encodes your **vector search architecture** as a **slash command** so Copilot can generate targeted SQL without repeating lengthy prompts
- The **GitHub Copilot debug panel** to inspect what Copilot actually sends to the model (system prompt + injected context)

## What the episode demonstrates
### 1) Baseline: Copilot with zero context
- Start from a clean VS Code workspace
- Observe what Copilot generates for SQL when it has no knowledge of your team’s conventions

### 2) Add a custom instruction file for team T-SQL conventions
- Introduce a single instruction file that teaches Copilot how your team wants T-SQL written
- Goal: make “awesome SQL” become “awesome, but also consistent with our standards”

### 3) Use Plan Mode to design from a PRD
- Use **Plan Mode** to translate a product requirements document into a proposed data model

### 4) Use Agent Mode + Schema Designer against a real database
- Use **Agent Mode** to bring the planned model to life using **Schema Designer**
- Connect Copilot to your **actual database** so it can operate with real schema context

### 5) Create a skills file + slash command for your architecture
- Create a **skills file** that teaches Copilot your **vector search** approach
- Use it as a **slash command** so Copilot can generate relevant SQL quickly
- The episode calls out generating **SQL Server 2025 / Azure SQL Database T-SQL** without needing to restate the architecture in every prompt

### 6) Inspect Copilot’s debug payload
- Open the **GitHub Copilot debug panel**
- Review the full request payload sent to the LLM, including:
  - The **system prompt**
  - Any **injected context** (instructions/skills/schema context)

## Resources (from the video description)
- Install VS Code MSSQL extension: https://aka.ms/vscode-mssql
- Demos: https://aka.ms/vscode-mssql-demos
- Blogs: https://aka.ms/vscode-mssql-blogs
- Documentation: https://aka.ms/vscode-mssql-docs

## Episode timestamps (from the video description)
- 0:00 Introduction
- 1:33 Demo
- 8:45 Demo
- 17:16 Demo
- 18:33 Getting started

## Where to follow / watch more
- Data Exposed episodes: https://aka.ms/dataexposedyt
- Microsoft Azure SQL channel: https://aka.ms/msazuresqlyt
- Microsoft SQL Server channel: https://aka.ms/mssqlserveryt
- Microsoft Developer channel: https://aka.ms/microsoftdeveloperyt
- Twitter (Anna Hoffman): https://twitter.com/AnalyticAnna
- Twitter (AzureSQL): https://aka.ms/azuresqltw

