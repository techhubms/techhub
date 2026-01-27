---
external_url: https://www.reddit.com/r/azuredevops/comments/1me1ts0/devops_backlog_questions/
title: 'Questions About Azure DevOps Backlogs: Closed Work Items & Iteration Filtering'
author: Ozzy_Lemmy_is_god
feed_name: Reddit Azure DevOps
date: 2025-07-31 13:38:24 +00:00
tags:
- ADO Backlog
- Azure Boards
- Azure DevOps
- Board Configuration
- Closed Work Items
- Epics
- Hierarchy View
- Iteration Path
- Team Configuration
- User Stories
- Work Item Visibility
section_names:
- azure
- devops
primary_section: azure
---
Ozzy_Lemmy_is_god raises two detailed questions around Azure DevOps backlog functionality and iteration filtering, delving into changes in closed work item visibility and team-based backlog configuration.<!--excerpt_end-->

# DEVOPS Backlog Questions: Azure DevOps Backlog Visibility and Iteration Filtering

Author: Ozzy_Lemmy_is_god

---

## 1. Visibility of Closed Work Items in Azure DevOps Backlogs

**Summary:**

The author has noticed that in recent Azure DevOps (ADO) updates, the backlog's behavior for viewing closed work items (Epics, Features, User Stories) has changed:

- **Previous Behavior:**
  - Closed items could still appear on their respective board levels (Epics, Features, User Stories) as long as the "Show closed work items" option was enabled.

- **Current Behavior:**
  - Closed items now disappear entirely from those board levels once closed, unless their parent or child work items remain open *and* the relevant options (like "Parents" or "Completed child Items") are enabled.

- **Limitation:**
  - The only way to find a closed Feature is by toggling to Board View or Sprint View, which does not provide the desired full hierarchy view in the backlog.

**Main Question:**

- Is there a way to restore the old behavior—i.e., configure the backlog to show closed work items directly on Epics, Features, or User Story levels, as before?

**Visual Reference:**

- ![DEVOPS backlog questions screenshot](https://preview.redd.it/devops-backlog-questions-v0-gku3pun0q7gf1.png?width=215&format=png&auto=webp&s=8112e7ce42323ed71ba96ca1188dad905a5b9b2d)

---

## 2. Iteration Path Filtering per Team in Azure DevOps Backlogs

**Scenario:**

- A team has been assigned a specific iteration in the "team configuration for iterations" (without additional permissions).
- An Epic is assigned to that iteration. It contains Features—some in the same iteration, some in others.

**Expectation:**

- When viewing the backlog (set to the Epic level and filtered by the team's iteration path), only the Epic and Features in that iteration should be visible to the team.

**Observed Behavior:**

- The backlog displays the full hierarchy: the Epic, all its Features (even those from different iterations), contrary to the expectation that only items within the assigned iteration appear.

**Question:**

- Does the backlog's mechanism for fetching child work items override the team's iteration configuration? In other words, why are Features outside the assigned iteration still shown, and can this be changed?

---

## Closing

The author thanks the community for help and seeks clarification or workarounds regarding both aspects of Azure DevOps backlog configuration and visibility.

This post appeared first on "Reddit Azure DevOps". [Read the entire article here](https://www.reddit.com/r/azuredevops/comments/1me1ts0/devops_backlog_questions/)
