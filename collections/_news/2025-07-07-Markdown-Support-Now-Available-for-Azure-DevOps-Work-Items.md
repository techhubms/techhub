---
external_url: https://devblogs.microsoft.com/devops/markdown-support-arrives-for-work-items/
title: Markdown Support Now Available for Azure DevOps Work Items
author: Dan Hellem
feed_name: Microsoft DevBlog
date: 2025-07-07 19:32:46 +00:00
tags:
- Agile
- Azure & Cloud
- Azure Cloud
- Azure DevOps
- Editor
- HTML Conversion
- Large Text Fields
- Markdown
- Release Process
- REST API
- User Feedback
- Work Items
section_names:
- azure
- devops
---
Dan Hellem introduces general availability of Markdown support for large text fields in Azure DevOps work items, detailing opt-in features, conversion steps, REST API usage, and release process.<!--excerpt_end-->

# Markdown Support Now Available for Azure DevOps Work Items

**Author:** Dan Hellem  
**Source:** [Azure DevOps Blog](https://devblogs.microsoft.com/devops/markdown-support-arrives-for-work-items/)

After several months in private preview and numerous bug fixes, Markdown support in large text fields for Azure DevOps work items is now generally available.

---

## 🦄 How it Works

### Default Behavior

- By default, existing and new work items use the HTML editor for large text fields.
- Users now have the option to **opt-in** and use the Markdown editor for individual work items and fields.

### Existing Work Items

1. **Access the Editor:** Open a work item and click into a large text field (e.g., Description).
2. **HTML to Markdown Option:** The field uses the HTML editor initially, but shows an option to convert to Markdown.
    - A best-effort conversion is performed.
    - More complex HTML formatting may result in less accurate conversions. Intricate formatting is better maintained in HTML.
3. **One-way Conversion:**
    > **Important**: Once converted to Markdown, you **cannot revert** the field back to HTML.
4. **Markdown Editing:** After conversion, the field displays and edits in Markdown format.
5. **Preview Available:** Users can toggle a **preview mode** to view rendered Markdown in real time.

### New Work Items

- Experience is similar to existing items, except there is no data to convert.
- **Sticky Preference:** If Markdown is chosen for a new work item, future items will default to Markdown for that user.
- Users can choose HTML for a field only if it is empty.
- Once a field uses Markdown, the HTML editor is not available.
- Pasting HTML into the Markdown editor triggers an automatic conversion attempt.

---

## 🧑‍💻 Using REST APIs

- When creating work items via the REST API, **HTML** remains the default format for large text fields.
- To set a field to Markdown, add an operation in your patch document:

```json
{ "op": "add", "path": "/fields/System.Description", "value": "# some markdown text" },
{ "op": "add", "path": "/multilineFieldsFormat/System.Description", "value": "Markdown" }
```

- The `/multilineFieldsFormat` property applies to any large text field, including custom fields.

**Key Rules:**

- Default format is HTML.
- Markdown setting is **irreversible** (cannot switch back to HTML).
- Applies to large text fields: Description, Repro Steps, Acceptance Criteria, and custom large text fields.

---

## 🚀 Release Process

- The Markdown support rollout began with a select group of customers.
- The deployment is done in five stages ("rings") to monitor and ensure a smooth rollout.
- Each ring takes a few days before moving to the next, with the entire rollout expected to take 4-5 weeks for all organizations.
- Enhanced checks have been implemented for quality and stability.

---

## 💬 Feedback

- Users are encouraged to provide feedback or report issues by [submitting a ticket](https://developercommunity.visualstudio.com/AzureDevOps).
- The Azure DevOps team monitors and responds to reports actively.

---

For the original announcement and more details, visit the [Azure DevOps Blog](https://devblogs.microsoft.com/devops/markdown-support-arrives-for-work-items/).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/devops/markdown-support-arrives-for-work-items/)
