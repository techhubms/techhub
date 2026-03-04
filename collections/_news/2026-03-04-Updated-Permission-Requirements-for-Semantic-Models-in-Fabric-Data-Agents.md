---
layout: "post"
title: "Updated Permission Requirements for Semantic Models in Fabric Data Agents"
description: "This announcement details changes to the permission model for interacting with semantic models in Microsoft Fabric data agents. Previously, both creators and consumers needed member-level workspace access and build permission to use semantic models. The updated model now allows interaction with just read permission, simplifying access while maintaining security."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/update-to-required-permissions-for-semantic-models-in-fabric-data-agents/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2026-03-04 09:00:00 +00:00
permalink: "/2026-03-04-Updated-Permission-Requirements-for-Semantic-Models-in-Fabric-Data-Agents.html"
categories: ["ML"]
tags: ["Access Control", "AI", "BI Development", "Build Permission", "Data Agents", "Data Modeling", "Data Security", "Microsoft Fabric", "ML", "News", "Permissions", "Prep For AI", "Read Permission", "Semantic Models", "Workspace Roles"]
tags_normalized: ["access control", "ai", "bi development", "build permission", "data agents", "data modeling", "data security", "microsoft fabric", "ml", "news", "permissions", "prep for ai", "read permission", "semantic models", "workspace roles"]
---

The Microsoft Fabric Blog team explains the updated, simplified permissions required to use semantic models in Fabric data agents, making it easier for users with read access to interact with these models.<!--excerpt_end-->

# Updated Permission Requirements for Semantic Models in Fabric Data Agents

Microsoft Fabric has updated the permissions required to work with [semantic models](https://aka.ms/Fabric/Data-Agent-SemanticModel-PrepForAI) through Fabric data agents. Previously, both creators and consumers needed **Member access** to the workspace and **Build permission** on the semantic model to add or use it within a data agent. This meant that, whether creating or simply asking questions of a data agent, users had to have both levels of access.

**Key changes in the update:**

- **Read Permission Suffices:**
  - Users now only need **Read permission** on the semantic model to interact with it via a data agent.
  - Workspace access is no longer required for adding a semantic model to a data agent or querying it through a data agent.
- **Scope of Change:**
  - This change applies exclusively to interactions that occur via the data agent experience.
  - If you intend to modify the semantic model itself or use features like [Prep for AI](https://aka.ms/PBI/SemanticModel-PrepForAI), you still need **Write permission** on the model.
- **Objective:**
  - The update is designed to simplify the permission model for end users and make collaboration around data agents and semantic models more accessible without compromising security for modifications or advanced features.

## What does this mean for developers and data professionals?

- **Easier Collaboration:** Teams can now grant read access to a wider audience, enabling more users to interact with semantic models via data agents without unnecessary workspace permissions.
- **Security Maintenance:** Editing and advanced features remain protected by requiring write or higher permissions.
- **Improved Usability:** Reducing the permission requirements may improve organizational agility and reduce permission management overhead.

_For ongoing management or modification of the semantic model or use of advanced features, continue to use the appropriate permissions as before._

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/update-to-required-permissions-for-semantic-models-in-fabric-data-agents/)
