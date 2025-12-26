---
layout: "post"
title: "Azure Logic Apps Designer Major Redesign: Phase I Public Preview"
description: "This post introduces the Phase I release of a redesigned Azure Logic Apps designer, now in Public Preview for Standard workflows. It highlights faster onboarding, unified workflow views, draft mode with auto-save, enhanced search, workflow documentation features, improved monitoring, and a new debugging timeline. The roadmap includes future improvements and encourages user feedback to shape upcoming development."
author: "travisvu"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-integration-services-blog/moving-the-logic-apps-designer-forward/ba-p/4469835"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-11-18 08:24:37 +00:00
permalink: "/community/2025-11-18-Azure-Logic-Apps-Designer-Major-Redesign-Phase-I-Public-Preview.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["Auto Save", "Azure", "Azure Integration", "Azure Logic Apps", "Coding", "Community", "Debugging", "Developer Tools", "Development Loop", "DevOps", "Draft Mode", "Logic Apps Designer", "Monitoring", "Public Preview", "Run History", "Standard Workflows", "Unified Canvas", "VS Code", "Workflow Automation"]
tags_normalized: ["auto save", "azure", "azure integration", "azure logic apps", "coding", "community", "debugging", "developer tools", "development loop", "devops", "draft mode", "logic apps designer", "monitoring", "public preview", "run history", "standard workflows", "unified canvas", "vs code", "workflow automation"]
---

travisvu details the Phase I public preview of the new Azure Logic Apps designer, covering key improvements such as draft mode, unified tool views, and developer-focused workflow enhancements.<!--excerpt_end-->

# Azure Logic Apps Designer Major Redesign: Phase I Public Preview

**Author:** travisvu

The Azure Logic Apps team has launched a significant redesign of the Logic Apps designer, now available in Public Preview for Standard workflows. This marks the first phase of a multi-stage improvement plan focusing on developer productivity, usability, and workflow management.

## Key Phase I Improvements

- **Faster Onboarding**: Begin building workflows immediately without prior choices between stateful, stateless, or agentic modes. This upfront selection will soon become entirely dynamic.

- **Unified Workflow View**: The designer now features all core tooling in a single interface. Easily switch between run history, code, and visual editor, and modify settings without leaving your workflow context.

- **Draft Mode & Auto-Save**: Your workflow is auto-saved as a draft every 10 seconds. Changes remain local until you explicitly publish, preventing accidental production releases and reducing risk of lost work. Publishing is now a separate step; auto-save will never restart your running app, only publishing will.

- **Smarter Search Experience**: Connector search and workflow browsing are streamlined, with forthcoming backend search support for faster, more relevant results and minimal upfront data downloads.

- **Workflow Documentation with Notes**: Add sticky notes, markdown annotations, even embed YouTube videos directly within workflows for on-canvas logic explanation and clarity.

- **Integrated Monitoring and Run History**: The run history page is consolidated with the designer for immediate feedback and fast iteration. View both draft and published runs side-by-side, serving the differing needs of developers and operators.

- **Enhanced Timeline Debugging**: Introduces a left-hand, hierarchical execution view so each workflow action is traceable in execution order, streamlining debugging of complex runs.

## Roadmap & Upcoming Phases

- **Phase II: Reimagining the Canvas**: Introduction of new workflow shortcuts and easier modification patterns.
- **Phase III: Unified Experiences**: Consistency between Logic Apps in VS Code, Consumption, and Standard SKUs.
- **Additional Improvements**: Streamlined connection management, improved onboarding, and eventually dynamic switching between key workflow options (stateful/stateless/agentic).

## Feedback & Participation

Your feedback is highly encouraged—it directly shapes ongoing releases and priorities. Share your experience through the feedback button in the Azure Portal designer, join discussions on GitHub or community forums, or comment on related blog posts.

## Useful Links

- [Azure Logic Apps GitHub UX feedback](https://github.com/Azure/LogicAppsUX)

*Updated: Nov 18, 2025*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/moving-the-logic-apps-designer-forward/ba-p/4469835)
