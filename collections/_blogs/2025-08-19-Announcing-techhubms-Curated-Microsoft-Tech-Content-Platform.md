---
layout: "post"
title: "Announcing tech.hub.ms: Curated Microsoft Tech Content Platform"
description: "Reinier van Maanen introduces tech.hub.ms, a new platform that aggregates, categorizes, and curates technical content from across the Microsoft ecosystem. The site covers Azure, AI, ML, GitHub Copilot, .NET, Security, and DevOps, offering AI-powered tagging, regular roundups, and a transparent process for contributors. Reinier details why the site was created, its underlying technology, community involvement, and plans for future enhancement."
author: "Reinier van Maanen"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://r-vm.com/new-website-tech-hub-ms.html"
viewing_mode: "external"
feed_name: "Reinier van Maanen's blog"
feed_url: "https://r-vm.com/feed.xml"
date: 2025-08-19 00:00:00 +00:00
permalink: "/2025-08-19-Announcing-techhubms-Curated-Microsoft-Tech-Content-Platform.html"
categories: ["AI", "Azure", "DevOps", "GitHub Copilot"]
tags: [".NET", "AI", "AI Powered Tagging", "Azure", "Azure App Services", "Azure Static Web Apps", "Blogs", "Curated Content", "Developer Tools", "DevOps", "GitHub Copilot", "GitHub Pages", "Jekyll", "Machine Learning", "Microsoft", "Microsoft Tech Hub", "OpenAI", "Personalization", "Playwright", "Site Launch", "Tech News", "Weekly Roundup"]
tags_normalized: ["dotnet", "ai", "ai powered tagging", "azure", "azure app services", "azure static web apps", "blogs", "curated content", "developer tools", "devops", "github copilot", "github pages", "jekyll", "machine learning", "microsoft", "microsoft tech hub", "openai", "personalization", "playwright", "site launch", "tech news", "weekly roundup"]
---

Reinier van Maanen shares the launch of tech.hub.ms, a Microsoft tech news aggregator with AI-driven categorization, built in collaboration with Rob Bos and Xebia colleagues.<!--excerpt_end-->

# Announcing tech.hub.ms — Curated Microsoft Tech Content Platform

**Author:** Reinier van Maanen

## Overview

[tech.hub.ms](https://tech.hub.ms) is a new platform dedicated to aggregating and curating high-quality technical content in the Microsoft ecosystem. The project, spearheaded by Reinier van Maanen alongside Rob Bos and other Xebia colleagues, aims to provide an organized, filterable hub for topics such as Azure, AI, ML, GitHub Copilot, .NET, Security, and DevOps.

## Features

- **Curated Content:** The site collects news, blogs, videos, and community articles focused on Microsoft developer and engineering topics.
- **AI-Powered Categorization:** Custom AI routines categorize, tag, and summarize incoming articles, helping users find relevant information efficiently.
- **Weekly Roundups:** Regular highlights summarize the week’s most significant developments across all focus areas, enhancing knowledge discovery.
- **Contributor-Friendly:** Contributions through Pull Requests are encouraged via [the GitHub repo](https://github.com/techhubms/techhub).

## Topics Covered

- Azure
- Artificial Intelligence (AI) and Machine Learning (ML)
- GitHub Copilot
- .NET ecosystem
- Security (with a Microsoft focus)
- DevOps

## How It Works

tech.hub.ms operates as an advanced RSS scraper with integrated AI for content annotation. It aggregates feeds from approximately 70 sources, extracting as much detail as possible. The content is categorized, tagged, and summarized, with outbound links always referencing the original publishers to maintain author visibility and support.

Shortcut URLs like [ai.hub.ms](https://ai.hub.ms) for AI or [ghc.hub.ms](https://ghc.hub.ms) for GitHub Copilot help users jump directly to specific topic areas.

## Community and Contributors

The site is primarily developed and maintained by Reinier, with Rob Bos and other Xebia colleagues contributing substantial content (notably around GitHub Copilot). A list of direct contributors includes [Rob Bos](https://github.com/rajbos), [Fokko Veegens](https://github.com/FokkoVeegens), [Liuba Gonta](https://github.com/liubchigo), and [Randy Pagels](https://github.com/PagelsR).

## Roadmap and Future Plans

- Move to a dynamic site architecture for improved flexibility
- More complete NLWeb support
- Personalized roundups driven by user prompts
- Podcast support for personalized roundups
- Expanded AI and MCP agent coverage
- Better search and tagging functionality
- LinkedIn and event integration
- Reintroduction of Reddit content via more robust scraping

## Technical Stack

- Jekyll for site-building
- Dev containers for development
- GitHub Pages for hosting (with plans to migrate to Azure Static Web Apps and Azure App Services due to scaling)
- Playwright used for scraping and headless browser automation
- Content analysis via GitHub Models and Azure OpenAI for robust summarization and rate-limiting mitigation

## Planned Blog Topics

- Building the platform architecture with Jekyll, dev containers, and GitHub Pages
- Experiences leveraging GitHub Copilot and Azure OpenAI for development
- Solving CI/CD and scaling challenges with Azure infrastructure
- Data handling and automation for weekly roundups
- Evolving from static hosting to full cloud deployment

---

Find out more at [tech.hub.ms](https://tech.hub.ms), or contribute to the project on [GitHub](https://github.com/techhubms/techhub).

This post appeared first on "Reinier van Maanen's blog". [Read the entire article here](https://r-vm.com/new-website-tech-hub-ms.html)
