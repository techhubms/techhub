---
layout: "post"
title: "Assign Issues to GitHub Copilot Using the API"
description: "This news update covers how developers can now assign issues directly to GitHub Copilot using either GraphQL or the REST API. It explains the required mutations and endpoints, repository and branch configuration options, and additional features such as custom instructions and agents. Key links are provided for further documentation and community support."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-12-03-assign-issues-to-copilot-using-the-api"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-12-03 19:21:25 +00:00
permalink: "/2025-12-03-Assign-Issues-to-GitHub-Copilot-Using-the-API.html"
categories: ["AI", "DevOps", "GitHub Copilot"]
tags: ["Addassigneestoassignable Mutation", "AI", "API Integration", "Branch Management", "Copilot", "Copilot Assignment", "Createissue Mutation", "Custom Agents", "Custom Instructions", "DevOps", "DevOps Automation", "GitHub Copilot", "GraphQL API", "Improvement", "Issue Assignment", "News", "Projects & Issues", "Replaceactorsforassignable Mutation", "Repository Configuration", "REST API", "Updateissue Mutation"]
tags_normalized: ["addassigneestoassignable mutation", "ai", "api integration", "branch management", "copilot", "copilot assignment", "createissue mutation", "custom agents", "custom instructions", "devops", "devops automation", "github copilot", "graphql api", "improvement", "issue assignment", "news", "projects and issues", "replaceactorsforassignable mutation", "repository configuration", "rest api", "updateissue mutation"]
---

Allison reports that developers can now assign issues to GitHub Copilot via both GraphQL and REST APIs, providing flexible automation options through clear API documentation.<!--excerpt_end-->

# Assign Issues to GitHub Copilot Using the API

Developers can now streamline their project management by assigning issues directly to GitHub Copilot through both GraphQL and REST API endpoints. This enhancement allows for greater automation and integration with Copilot's capabilities in repository workflows.

## Supported APIs and Mutations

### GraphQL Support

- `updateIssue`: Update issue details and assignments. ([Docs](https://docs.github.com/graphql/reference/mutations#updateissue))
- `createIssue`: Create new issues and assign them immediately. ([Docs](https://docs.github.com/graphql/reference/mutations#createissue))
- `addAssigneesToAssignable`: Add Copilot as an assignee to existing issues. ([Docs](https://docs.github.com/graphql/reference/mutations#addassigneestoassignable))
- `replaceActorsForAssignable`: Replace current assignees with Copilot. ([Docs](https://docs.github.com/graphql/reference/mutations#replaceactorsforassignable))
- All requests require the header: `GraphQL-Features: issues_copilot_assignment_api_support`

### REST API Support

- [Add assignees to an issue](https://docs.github.com/rest/issues/assignees?apiVersion=2022-11-28#add-assignees-to-an-issue): Assign issues through a REST endpoint.
- [Create an issue](https://docs.github.com/rest/issues/issues?apiVersion=2022-11-28#create-an-issue): Create and automatically assign issues to Copilot.
- [Update an issue](https://docs.github.com/rest/issues/issues?apiVersion=2022-11-28#update-an-issue): Change assignments using REST API.

## Configuration Options

- Target repository and base branch selection for precise issue assignment workflows.
- Custom instructions and agent setups to extend Copilot's functionality.

## Community and Documentation

- [Community discussion](https://github.com/orgs/community/discussions/173575) for feedback and collaboration.
- [Documentation](https://docs.github.com/copilot/how-tos/use-copilot-agents/coding-agent/create-a-pr) for step-by-step guides on Copilot assignment via API.

Developers can leverage these features to automate project management, customize workflows, and integrate Copilot more deeply into team processes.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-12-03-assign-issues-to-copilot-using-the-api)
