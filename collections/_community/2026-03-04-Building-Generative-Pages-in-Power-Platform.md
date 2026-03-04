---
layout: "post"
title: "Building Generative Pages in Power Platform"
description: "This article by harshul05 provides a practical guide for developers and makers to create Generative Pages in Power Platform using AI-driven tools. It covers how natural language can be used to automatically generate responsive, data-connected UI in model-driven Power Apps, including setup requirements, design tips, step-by-step workflow, and known limitations compared to traditional development approaches."
author: "harshul05"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-generative-pages-in-power-platform/ba-p/4494062"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-03-04 08:00:00 +00:00
permalink: "/2026-03-04-Building-Generative-Pages-in-Power-Platform.html"
categories: ["AI"]
tags: ["AI", "AI Powered Development", "App Development", "Application Lifecycle Management", "Community", "Copilot", "Dataverse", "Generative Pages", "Microsoft", "Model Driven Apps", "Natural Language Interface", "Power Apps", "Power Fx", "Power Platform", "Prompt Engineering", "React Components", "UI Design"]
tags_normalized: ["ai", "ai powered development", "app development", "application lifecycle management", "community", "copilot", "dataverse", "generative pages", "microsoft", "model driven apps", "natural language interface", "power apps", "power fx", "power platform", "prompt engineering", "react components", "ui design"]
---

harshul05 explains how Generative Pages bring AI-powered page design to Power Platform, enabling developers to create model-driven app interfaces by describing requirements in natural language.<!--excerpt_end-->

# Building Generative Pages in Power Platform

Artificial Intelligence is now embedded across the Power Platform, and Generative Pages are one of its most significant innovations. Instead of manually arranging controls or writing Power Fx, creators can describe their requirements in plain language, and Power Apps converts those instructions into a responsive, data-enabled page.

## What Are Generative Pages?

Generative Pages provide an AI-powered method for building pages in model-driven apps:

- Makers describe the page in conversational or natural language.
- The platform selects React-based components and logic.
- Pages are auto-wired to Dataverse tables for data interaction.

**Example prompt:**
> "Design a card gallery that displays account names, images, email addresses, websites, and phone numbers."

## Key Features

- Page creation using natural language
- Native Dataverse integration (CRUD operations, up to 6 tables)
- React code generation with designer visibility
- Upload support for wireframes, screenshots, or sketches
- Conversational refinement through Copilot

## Getting Started: Prerequisites

1. Power Platform environment must be hosted in the US region
2. Copilot and AI features need to be enabled
3. Must be working within a model-driven app (not canvas apps)

## Step-by-Step Guide

### 1. Open the App Designer

Visit [make.powerapps.com](https://make.powerapps.com), sign in, and open your model-driven app in edit mode.

### 2. Start a New Generative Page

- In the designer, select **Add a page → Describe a page**
- This launches the AI-driven designer interface

### 3. Write Your Prompt

Describe the goal, layout, and functionality in detailed natural language.

- **Example:** "Create a responsive page with a three-column card layout showing account records. Include name, image, website, email, and phone. Add search and filtering. Optimize for mobile devices."

### 4. Connect Dataverse Tables

- Select **Add data → Add table**
- Choose up to six relevant tables (e.g., Account, Contact, Opportunity)

### 5. (Optional) Upload a Design Reference

- Attach a wireframe or mock-up to guide the layout

### 6. Generate and Refine

- Power Apps generates the page and React structure automatically
- Refine via conversational prompts (e.g., "Increase card size", "Enable dark mode")

### 7. Publish the Page

- Generative Pages must be published individually; publishing the app alone is not enough

## Tips for Better Prompts

- Provide detailed data and relationships ("Display active accounts only, sorted by last modified date")
- Be specific about layout across devices ("Three-column grid on desktop, one column on mobile")
- Use sketches/screenshots for more precise results
- Make gradual changes through iterations
- Plan for ALM if migrating between environments

## Known Limitations

- Only Dataverse data sources are supported (other connectors require virtual tables)
- Currently limited to US regions
- Pages must be individually published
- Direct editing of generated React code is not supported

## Generative Pages vs. Traditional Pages

| Category           | Generative Pages                 | Standard Pages (Forms/Views)         |
|--------------------|----------------------------------|--------------------------------------|
| Creation method    | AI-driven natural language       | Drag-and-drop/manual + Power Fx      |
| Technology         | Auto-generated React components  | Model-driven/canvas components       |
| Data sources       | Dataverse only                   | Dataverse + 3rd-party connectors     |
| Development speed  | Very fast                        | Moderate to slow                     |
| Customization      | Conversational AI refinements    | Full manual control                  |
| Performance        | Optimized React rendering        | Depends on implementation            |
| ALM support        | In development                   | Enterprise-ready/mature              |

### When to Use Each

- **Generative Pages:** Need rapid UI, Dataverse-centric, quick dashboards, modern look
- **Standard Pages:** Need advanced customization, non-Dataverse data, mature ALM, or granular control

## Conclusion

Generative Pages and AI-driven development significantly reduce interface creation time in model-driven Power Apps, letting makers focus more on optimization and user experience. While not a full replacement for classic, fully customizable approaches, they provide a powerful starting point and are expected to mature with broader regional and platform support.

## References

- [Generate a page using natural language with model-driven apps in Power Apps - Microsoft Learn](https://learn.microsoft.com/en-us/power-apps/maker/model-driven-apps/generative-pages)
- [Power Apps Developer Plan | Microsoft Power Platform](https://www.microsoft.com/en-us/power-platform/products/power-apps/free?msockid=28ff76106670665c0328602f675d6797)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/building-generative-pages-in-power-platform/ba-p/4494062)
