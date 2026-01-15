---
layout: post
title: Natural Language to Generate and Explain Pipeline Expressions with Copilot (Preview)
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/preview-natural-language-to-generate-and-explain-pipeline-expressions-with-copilot/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-11-21 10:30:00 +00:00
permalink: /ai/news/Natural-Language-to-Generate-and-Explain-Pipeline-Expressions-with-Copilot-Preview
tags:
- AI
- AI Productivity Tools
- Automated Code Generation
- Conversational AI
- Copilot Studio
- Data Engineering
- Data Factory
- Data Transformation
- Dynamic Expressions
- Machine Learning
- Microsoft Fabric
- ML
- Natural Language Processing
- News
- Pipeline Expression Builder
- Preview Features
section_names:
- ai
- ml
---
Microsoft Fabric Blog presents a new Copilot (Preview) feature for the Pipeline Expression Builder in Data Factory, allowing users to create and explain expressions through natural language, transforming data workflow efficiency.<!--excerpt_end-->

# Natural Language to Generate and Explain Pipeline Expressions with Copilot (Preview)

**Author:** Microsoft Fabric Blog

Copilot now makes it significantly easier to build pipeline expressions in Microsoft Fabric Data Factory. With this new (Preview) feature, developers and data engineers can write and understand complex dynamic expressions using plain, conversational language.

## What Does the Copilot Expression Builder Offer?

- **Natural Language Input:** Users can simply describe what they need, such as "Get the current date in UTC," and Copilot translates this intent into a valid pipeline expression for you.
- **Clarity and Explanation:** If you come across an unfamiliar expression, Copilot can explain it in accessible terms, removing the guesswork from pipeline management.
- **Interactive Controls:** Choose to accept, discard, or regenerate expressions Copilot provides, ensuring you're always in control.

## Why This Matters

Pipeline expressions allow for powerful, dynamic data movement and transformation, but have historically required memorizing complex syntax and extensive debugging. Copilot streamlines this process, saving time and minimizing errors—making this tooling accessible even to those new to Data Factory.

## Key Features

- **Generate Expressions by Describing Desired Logic:**
  - Example Prompt: “Get the current date and time in UTC. Convert this to Central Standard Time, format the result, and concatenate with a string.”
  - Copilot outputs the corresponding Data Factory expression automatically.
- **Explain What Expressions Do:**
  - Example Expression: `addDays(pipeline().TriggerTime, 7)`
  - Copilot explains: “This adds 7 days to the time when the pipeline was triggered.”
- **Productivity and Accessibility:**
  - Reduce manual coding and speed up pipeline development.
  - Minimize syntax-related errors and focus on business logic.

## Getting Started

1. **Open** the Pipeline Expression Builder for your activity in Data Factory.
2. **Select** the Copilot chat panel inline with your development view.
3. **Type** your requests or paste expressions for explanation.
4. **Review and Insert**: Copilot generates or explains the code, and you can place it directly into your pipeline step.

## Feedback and Further Resources

Explore the new feature and share your feedback with Microsoft. To learn more, check out the [Explain Pipeline Expressions with Copilot](https://aka.ms/CopilotPipelineExpression) documentation.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/preview-natural-language-to-generate-and-explain-pipeline-expressions-with-copilot/)
