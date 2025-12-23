---
name: RSS Feed Configuration Template
about: Add new RSS feeds to the automated content processing workflow
title: Add new RSS feed
labels: 'rss, automation'
assignees: ''
---

## Required Information

Please fill out the following information:

**Feed Source Name:** [e.g., "Microsoft DevBlog"]
**RSS Feed URL:** [e.g., "https://example.com/feed.xml"]
**Content Type/Output Directory:** [News / Blog / Video / Community / Events]

## Explanation for RSS feed automation

RSS feeds are automatically processed by GitHub Actions to create new content on the site. The system downloads RSS content, converts it to markdown articles, and places them in the appropriate collection directory based on the content type.

## Instructions for GitHub Copilot

**CRITICAL**: Follow the steps listed in `.github/prompts/new-rss-feed.prompt.md` to process this request.
**CRITICAL**: If you have not read them, fetch `AGENTS.md` and use these instructions as well.
