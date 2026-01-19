---
layout: post
title: Introducing Knowledge Agent in SharePoint (Public Preview)
author: Dellenny
canonical_url: https://dellenny.com/introducing-knowledge-agent-in-sharepoint-public-preview/
viewing_mode: external
feed_name: Dellenny's Blog
feed_url: https://dellenny.com/feed/
date: 2025-09-28 12:33:12 +00:00
permalink: /ai/blogs/Introducing-Knowledge-Agent-in-SharePoint-Public-Preview
tags:
- AI in SharePoint
- Automation Rules
- Content Automation
- Copilot
- Document Summarization
- Knowledge Agent
- M365 Copilot
- Metadata Management
- Microsoft 365
- Natural Language Query
- PowerShell
- Public Preview
- Set SPOTenant
- SharePoint
- Site Governance
section_names:
- ai
---
Dellenny introduces Microsoft's Knowledge Agent for SharePoint, detailing its AI content organization features, setup steps, and strategies for maximizing the intelligent capabilities within modern SharePoint environments.<!--excerpt_end-->

# Introducing Knowledge Agent in SharePoint (Public Preview)

Dellenny outlines the major elements and setup of the newly released Knowledge Agent for SharePoint, highlighting its AI-driven approach to content management and usability improvements for organizations.

## Overview

In September 2025, Microsoft launched Knowledge Agent as a public preview in SharePoint. Knowledge Agent is an AI-powered feature designed to make SharePoint content more organized, accessible, and intelligent—serving both users and AI tools such as Copilot.

### What is Knowledge Agent?

- **AI Layer for SharePoint**: Enriches files and pages by extracting structured metadata and summarizing content.
- **Integrated with Copilot**: Enhances Copilot’s ability to deliver accurate, relevant answers by improving the underlying content quality.
- **Advanced Features**: Goes beyond simple file inspection by using metadata extraction, summarization workflows, and context-aware actions.

### Core Features

- **Ask a Question**: Ask natural-language questions about documents, pages, or sites.
- **Summarize File/Page**: Quickly generate summaries of long content.
- **Compare Files**: Analyze and find differences/similarities between documents.
- **Organize This Library**: Get AI-driven recommendations for metadata columns and tagging.
- **Set Up Rules**: Use natural language to automate certain SharePoint tasks.
- **Create New View**: Generate new library views based on metadata.
- **Improve This Site**: Suggestions for fixing broken links and maintaining content health.

### How to Enable Knowledge Agent

- **Prerequisites**: Requires Microsoft 365 Copilot license and admin setup.
- **Administration**: Enable via PowerShell using `Set-SPOTenant -KnowledgeAgentScope`.
  - `AllSites`: Enable everywhere
  - `ExcludeSelectedSites`: Enable everywhere except specific sites
  - `NoSites`: Disable feature
  - Example:

    ```powershell
    Connect-SPOService https://yourtenant-admin.sharepoint.com
    Set-SPOTenant -KnowledgeAgentScope AllSites
    ```

  - Site-level controls will become available from November 1, 2025; full release planned for 2026.

### How It Appears

- After activation, users with the proper Copilot license and site permissions see a floating Knowledge Agent button within SharePoint. Permissions vary: owners and members get broad access; consumers (read-only users) mostly get Q&A capabilities.

### Recommended Strategies for Early Adopters

1. Start with a pilot on select sites or libraries.
2. Define clear governance for review and approval of agent-generated suggestions.
3. Use taxonomy and naming standards for consistency.
4. Train users on how to interact with Knowledge Agent and monitor its outputs.
5. Regularly review AI-generated changes to ensure accuracy.
6. Anticipate exceptions where the feature may underperform.
7. Stay updated, as Microsoft will iterate continuously during the preview.

### Why This Matters

- **AI Requires Quality Data**: Clean, well-tagged content improves all AI results.
- **Content Hygiene**: Easier maintenance, fewer outdated files, and improved governance.
- **Empowering Non-Experts**: Broadens utility for business users and admins.
- **Intelligent SharePoint**: Moves SharePoint from passive storage to an active, intelligent platform.

### Cautions

- Treat Knowledge Agent as beta software: monitor outputs, enforce oversight, and train users to catch edge cases. Expect improvements ahead of general availability in 2026.

For more details, see [Introducing Knowledge Agent in SharePoint (Public Preview)](https://dellenny.com/introducing-knowledge-agent-in-sharepoint-public-preview/).

---

*Author: Dellenny*

This post appeared first on "Dellenny's Blog". [Read the entire article here](https://dellenny.com/introducing-knowledge-agent-in-sharepoint-public-preview/)
