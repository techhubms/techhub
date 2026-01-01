---
layout: "post"
title: "Introducing Sub-Issues: Enhancing Issue Management on GitHub"
description: "Shaun Wong explores the development and impact of GitHub's new sub-issues feature. The post outlines how sub-issues were built, their technical design, the role of internal feedback, and the benefits for collaborative project management and progress tracking."
author: "Shaun Wong"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/engineering/architecture-optimization/introducing-sub-issues-enhancing-issue-management-on-github/"
viewing_mode: "external"
feed_name: "GitHub Engineering Blog"
feed_url: "https://github.blog/engineering/feed/"
date: 2025-04-11 22:33:30 +00:00
permalink: "/2025-04-11-Introducing-Sub-Issues-Enhancing-Issue-Management-on-GitHub.html"
categories: ["DevOps"]
tags: ["Accessibility", "Architecture", "Architecture & Optimization", "Beta Feedback", "Development Process", "DevOps", "Dogfooding", "Engineering", "GitHub Issues", "GraphQL", "Hierarchical Tasks", "How GitHub Builds GitHub", "Issue Tracking", "MySQL", "News", "Project Management", "React", "Software Engineering", "Sub Issues", "Workflow Optimization"]
tags_normalized: ["accessibility", "architecture", "architecture and optimization", "beta feedback", "development process", "devops", "dogfooding", "engineering", "github issues", "graphql", "hierarchical tasks", "how github builds github", "issue tracking", "mysql", "news", "project management", "react", "software engineering", "sub issues", "workflow optimization"]
---

Written by Shaun Wong, this post details the development of GitHub's sub-issues feature, from technical implementation to organizational benefits, offering insight into enhanced project and issue management practices.<!--excerpt_end-->

# Introducing Sub-Issues: Enhancing Issue Management on GitHub

*Authored by Shaun Wong*

## Overview

Recently, GitHub launched sub-issues—a feature aimed at improving the management of complex issues by allowing users to break larger tasks into smaller, more manageable items. This post discusses the development journey of sub-issues, technical integration, and how GitHub’s teams used the feature to build and refine it, ultimately enhancing collaborative project management.

---

## What Are Sub-Issues?

Sub-issues allow users to divide a single issue into hierarchical lists of sub-tasks. This structure helps teams track progress, visualize dependencies, and maintain organization in larger projects. For example, work that spans multiple repositories can be decomposed into discrete sub-issues, making tracking and reviewing more efficient. In practice, sub-issues help keep pull requests concise and reviews more manageable.

> *"Breaking this task into discrete sub-issues makes it easier to track progress and more clearly define the work I need to do."*

![A screenshot showing a list of sub-issues on GitHub.](https://github.blog/wp-content/uploads/2025/04/sub-issues.png?resize=1024%2C388)

---

## A Brief History

GitHub Issues are a longstanding tool for project management, supporting bug tracking and feature planning. Over time, enhancements such as labels, milestones, and task lists have improved their utility. As projects became more complex, the need to represent and organize hierarchical tasks within issues grew. Sub-issues were conceived to address this, aiming to integrate seamlessly with the existing GitHub Issues experience while ensuring simplicity and clarity.

---

## Building Sub-Issues

The development of sub-issues began with a new hierarchical structure for tasks. Instead of modifying existing task lists, GitHub implemented:

- **Nested Task Support:** Tasks can now be nested within tasks, creating a hierarchy.
- **Data Modeling:** Relationships between parent and child issues are recorded in a dedicated sub-issues table, ensuring hierarchical links are maintained.
- **Progress Tracking:** Completion status of sub-issues is rolled up in a separate table, enabling efficient progress visualization (e.g., Issue X is automatically updated when Issue Y is completed).
- **Data Management:** MySQL tables were used to straightforwardly represent sub-issue relationships, aiding performance and easier integration with environments like GitHub Enterprise Server and Cloud.
- **APIs:** Sub-issues are accessible via GraphQL endpoints, allowing integration with the new Issues experience and leveraging existing and new React components for list views and UI rendering.
- **User Experience:** The implementation focused on intuitive controls and accessibility, achieved by collaborating with designers and component teams.

---

## Using Sub-Issues in Practice

GitHub applied a "dogfooding" approach, using sub-issues extensively within its internal teams during development. This enabled:

- Early identification of usability improvements and performance optimizations.
- Validation of the feature’s real-world utility in managing large, complex projects.
- Fine-tuning of issue hierarchy management, resulting in a simpler user experience.

Feedback indicated that sub-issues increased visibility, improved control, and helped prevent important tasks from being overlooked, especially in multi-phase or cross-repository initiatives.

---

## Gathering Early Feedback

Collaboration with beta testers and early adopters was critical to refining sub-issues. Key lessons included:

- Deciding on metadata display: Initially, only issue titles were shown in sub-issues lists. Later, issue numbers and repository names were included for clarity if the issue belonged to another repository.
- Improving UI: Filters such as `has:sub-issues-progress` and `has:parent-issue` evolved from user discussions, enhancing filter syntax and usability.
- Iterative feature development: By managing the sub-issues feature with sub-issues, the team could rapidly spot friction points, applying fixes and enhancements quickly.

These practices will inform future GitHub feature development, emphasizing early stakeholder involvement and practical hands-on usage.

---

## Call to Action

Sub-issues are designed to break down complex work into manageable parts, helping users track dependencies, view progress, and manage cross-repository collaboration more efficiently. GitHub encourages users to try out the feature, provide feedback, and join discussions to shape its continued evolution.

- [Community Discussion on Sub-Issues](https://github.com/orgs/community/discussions/154148)

Thank you to the GitHub community for contributing to the platform’s ongoing improvement and innovation.

---
**References**:

- [Introducing sub-issues: Enhancing issue management on GitHub - GitHub Blog](https://github.blog/engineering/architecture-optimization/introducing-sub-issues-enhancing-issue-management-on-github/)
- [Changelog](https://github.blog/changelog/)
- [Docs on GitHub Issues](https://docs.github.com/issues)
- [Evolving GitHub Issues Public Preview](https://github.blog/changelog/2025-01-12-evolving-github-issues-public-preview/)

---

This post appeared first on "GitHub Engineering Blog". [Read the entire article here](https://github.blog/engineering/architecture-optimization/introducing-sub-issues-enhancing-issue-management-on-github/)
