---
tags:
- AI
- AI Agent Workflows
- Bulk Remove Labels API
- Bulk Set Labels API
- Compliance Automation
- Get Item API
- Governance
- Information Classification
- Information Protection
- Label Inheritance
- List Items API
- Microsoft Fabric
- Microsoft Purview
- ML
- News
- Protection Policies
- Public APIs
- REST API
- Security
- Sensitivity Labels
- Update Item API
- Workspace Inventory
external_url: https://blog.fabric.microsoft.com/en-US/blog/sensitivity-labels-in-fabric-for-public-apis-generally-available/
primary_section: ai
feed_name: Microsoft Fabric Blog
date: 2026-03-31 08:00:00 +00:00
title: Sensitivity labels in Fabric for public APIs (Generally Available)
section_names:
- ai
- ml
- security
author: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces general availability of sensitivity labels in Microsoft Fabric Public APIs, explaining how teams can retrieve label metadata in core item APIs and use it for governance automation, workspace classification monitoring, and AI/agent safety guardrails.<!--excerpt_end-->

# Sensitivity labels in Fabric for public APIs (Generally Available)

To support advanced governance and automation scenarios, **Sensitivity Labels are now exposed through Microsoft Fabric Public APIs**. This enables developers, partners, and platform teams to programmatically get and manage sensitivity labels as part of information classification and AI workflows.

## Sensitivity labels in Fabric

In Fabric, Sensitivity Labels (from **Microsoft Purview Information Protection**) help **classify and protect** sensitive data across all data assets. These labels prevent unauthorized access, support compliance, and make it easier to identify sensitive information.

Because Fabric uses the same unified sensitivity labels as Microsoft 365, **protection remains consistent across platforms**. The label stays attached to the item’s data throughout its lifecycle in Fabric (for example, via inheritance) and persists when exported to Microsoft 365.

Sensitivity labels and default labeling behavior are governed by organizational label policies defined in Microsoft Purview, enabling each organization to enforce its own information classification standards across Fabric.

More details: [Sensitivity Labels in Fabric and Power BI](https://learn.microsoft.com/fabric/governance/protected-sensitivity-labels)

### Sensitivity label definition

![Menu showing the selection of a sensitivity label and protection scope, with ‘Highly Confidential’ chosen in Microsoft Fabric.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/menu-showing-the-selection-of-a-sensitivity-label.png)

*Figure: Selecting a sensitivity label for a Fabric item.*

### Protection Policy

Organizations can use protection policies to ensure that sensitive data in Fabric is also protected according to sensitivity label—combining classification and protection.

- Documentation: [Protection policies overview](https://learn.microsoft.com/fabric/governance/protection-policies-overview)

![Screenshot of review and finish page in protection policy configuration.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-review-and-finish-page-in-protection.png)

*Figure: Review and confirm protection policy settings before creating the policy.*

## Sensitivity label availability for public API

Microsoft Fabric now supports Sensitivity Labels across key Public API operations.

### Available capabilities

- [List Items](https://learn.microsoft.com/rest/api/fabric/core/items/list-items?tabs=HTTP)
  - Response now includes the **Sensitivity Label Id** for each item.
- [Get Item](https://learn.microsoft.com/rest/api/fabric/core/items/get-item?tabs=HTTP)
  - Response includes the item’s **Sensitivity Label Id**.
- [Update Item](https://learn.microsoft.com/rest/api/fabric/core/items/update-item?tabs=HTTP)
  - Response includes the **Sensitivity Label Id**.

> Note: labels can be retrieved programmatically but updating labels via this API is not supported.

### Related label management APIs

- [Bulk Set Labels](https://learn.microsoft.com/rest/api/fabric/admin/labels/bulk-set-labels?tabs=HTTP) – Apply sensitivity labels to items (admin).
- [Bulk Remove Labels](https://learn.microsoft.com/rest/api/fabric/admin/labels/bulk-remove-labels?tabs=HTTP) – Remove sensitivity labels from items (admin).

These changes let you access sensitivity label metadata without additional per-item metadata queries, which is especially useful for large-scale automation.

## Use cases

## AI and agent workflows: using sensitivity labels as a safety control

### Scenario

An AI agent (or workflow automation) searches Fabric items to answer a user question. The organization requires the agent to detect and enforce policies for specific sensitivity label types. If content is tagged with sensitive labels (for example: Confidential, Highly Confidential, Restricted), the agent must:

- exclude it, or
- require additional approval, or
- route results through a secure handling process.

### Why the new API behavior matters

When the agent discovers items using **List Items**, it immediately receives the **Sensitivity Label Id** for each candidate. The label ID can act as a real-time guardrail signal in the AI workflow (filter, escalate, redact, or require approval) without extra API calls per item.

- **List Items**: discover items and read Sensitivity Label Id in one pass.
- **Get Item**: verify the sensitivity label for a specific artifact before using it in an AI step.

## Workspace classification monitoring and inventory visibility

### Scenario

A platform or governance team needs a programmatic inventory of Fabric items within a workspace (or across workspaces) to assess data security posture.

Using **List Items** (with Sensitivity Label Id included), automation can perform classification analysis such as:

- identifying unlabeled items
- detecting items labeled below a required classification level
- analyzing label distribution across the workspace

Results can feed compliance dashboards, trigger remediation workflows, or enforce organizational labeling standards.

### Why the new API behavior matters

Previously, sensitivity label information required additional metadata queries per item. Now the **Sensitivity Label Id** is included directly in the **List Items** response, enabling efficient large-scale classification monitoring in a single pass and reducing API overhead.

### APIs involved

- **List Items**: retrieve all items with Sensitivity Label Id.
- **Bulk Set Labels**: remediate unlabeled or misclassified artifacts.

## Conclusion and further resources

Sensitivity labels in Microsoft Fabric public APIs help teams incorporate data classification into automated, governance-aware workflows. By exposing label metadata through core APIs, organizations can better enforce compliance across analytics and AI scenarios.

Further reading:

- [Protection policies in Microsoft Fabric](https://learn.microsoft.com/fabric/governance/protection-policies-overview)
- [Create and manage protection policies for Fabric](https://learn.microsoft.com/fabric/governance/protection-policies-create)
- [Apply sensitivity labels to Fabric items](https://learn.microsoft.com/fabric/fundamentals/apply-sensitivity-labels)
- [Public APIs](https://learn.microsoft.com/rest/api/fabric/core/items/list-items?tabs=HTTP)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/sensitivity-labels-in-fabric-for-public-apis-generally-available/)

