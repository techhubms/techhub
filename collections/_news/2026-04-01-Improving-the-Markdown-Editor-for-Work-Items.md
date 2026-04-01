---
tags:
- '#devops'
- Azure
- Azure & Cloud
- Azure Boards
- Azure DevOps
- Community
- Developer Community Feedback
- DevOps
- Edit Mode
- Editing Workflow
- Feature Update
- Markdown
- Markdown Editor
- News
- Preview Mode
- Rich Text Fields
- Rollout Plan
- Usability
- UX
- Work Items
primary_section: azure
feed_name: Microsoft DevOps Blog
section_names:
- azure
- devops
title: Improving the Markdown Editor for Work Items
date: 2026-04-01 17:17:11 +00:00
external_url: https://devblogs.microsoft.com/devops/improving-the-markdown-editor-for-work-items/
author: Dan Hellem
---

Dan Hellem outlines an update to the Azure DevOps work item Markdown editor that reduces accidental edits by defaulting large text fields to preview mode and requiring an explicit action to enter edit mode.<!--excerpt_end-->

## Improving the Markdown Editor for Work Items

We introduced the Markdown editor in July 2025 to bring Markdown support to large text fields in Azure DevOps work items. Since then, we’ve received customer feedback highlighting friction in the editing experience—especially around switching in and out of edit mode.

Many users found the existing interaction model confusing and sometimes disruptive. For example, entering edit mode via actions like double-clicking could feel unexpected when someone was simply trying to read or review content.

## What’s changing

To improve usability, the work item Markdown editor now has a clearer distinction between **preview** and **edit** modes for large text fields:

- **Default behavior:** fields open in **preview mode** so you can read and interact with content without accidentally entering edit mode.
- **Entering edit mode:** when you want to change content, you explicitly click the **edit icon** at the top of the field.
- **Exiting edit mode:** after updates, you can exit edit mode and return to preview mode.

![Work item field showing the edit icon to enter Markdown editing](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2026/04/markdown-edit.webp)

![Work item field showing the done action to return to preview mode](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2026/04/markdown-done.webp)

## Why this matters

This change aligns with how most users expect Markdown editors to behave. The goal is to:

- reduce accidental edits
- make interactions more intentional
- make the experience more predictable and less disruptive

## Rollout plan

The improved experience has already been rolling out to a subset of customers for early feedback, which has been positive. Microsoft is now expanding the rollout to all customers over the next **two to three weeks**.

## References

- Original post: https://devblogs.microsoft.com/devops/improving-the-markdown-editor-for-work-items/
- Feedback thread: https://developercommunity.visualstudio.com/t/Markdown-editor-for-work-item-multi-line/10935496


[Read the entire article](https://devblogs.microsoft.com/devops/improving-the-markdown-editor-for-work-items/)

