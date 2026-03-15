---
external_url: https://devblogs.microsoft.com/devops/whats-new-with-azure-repos/
title: Recent and Upcoming Improvements in Azure Repos
author: Dan Hellem
feed_name: Microsoft DevOps Blog
date: 2026-01-16 19:34:04 +00:00
tags:
- Azure & Cloud
- Azure DevOps
- Azure Repos
- Branching
- Code Review
- Collaboration
- Git Policy
- GitHub Copilot
- GitHub Copilot Integration
- MCP Server
- Notification Improvements
- Pull Requests
- Repository Management
- REST API
- TFVC
- VS Code
- Azure
- DevOps
- News
- .NET
section_names:
- azure
- dotnet
- devops
primary_section: dotnet
---
Dan Hellem outlines the latest updates and upcoming features in Azure Repos, offering practical guidance on streamlined repository management for developers and DevOps professionals.<!--excerpt_end-->

# Recent and Upcoming Improvements in Azure Repos

**Author:** Dan Hellem

Azure Repos has recently seen a range of updates designed to help developers and teams work more efficiently. This summary covers key completed changes, current rollouts, and a preview of what’s coming soon.

## Breaking Change: Disabling Obsolete TFVC Check-In Policies

- Old-style TFVC check-in policies will be disabled if not yet migrated; migration to the new storage format is mandatory for continued policy enforcement.
- Policies affected include: Build (require last build success), Work Item linkage, and Changeset Comments.
- See the [detailed migration guide](https://devblogs.microsoft.com/devops/tfvc-policies-storage-updates) for step-by-step instructions.

## Improved Comment Navigation from Pull Request Links

- Direct links to pull request comments now focus reliably, even for PRs with heavy comment traffic.
- This improves reviewer efficiency and reduces loss of context during feedback cycles.

## Pull Request Notification Improvements

- Low-value email notifications (such as draft state changes and auto-complete updates) have been removed to reduce inbox noise.
- Remaining notifications are more concise and action-focused, making it easier to follow key changes during code reviews.

## Pull Request Templates for Multi-Level Branches

- Pull request templates now support nested folder structures that match multi-level branching (e.g., `feature/foo/december`).
- Azure DevOps automatically applies the most specific template available based on the branch hierarchy, reducing duplication and aligning with branching conventions.
- [Learn about pull request templates](https://learn.microsoft.com/en-us/azure/devops/repos/git/pull-request-templates?view=azure-devops).

## Azure DevOps MCP Server

- The [Azure DevOps MCP Server](https://github.com/microsoft/azure-devops-mcp) enables local interaction with repositories, branches, commits, and pull requests.
- Works with tools like VS Code and GitHub Copilot, allowing metadata inspection and workflow automation outside the Azure DevOps UI.
- Supports scenarios like listing branches, exploring commit history, and integrating with intelligent development tools.

## Coming Soon

### Improved Git Policy Configuration API

- Upcoming changes will make it easier to retrieve all policies applying to a repository (across branches and refs), reducing unnecessary API calls.
- Intended to improve the scalability and performance of services managing policies at scale.
- [Documentation for the new API](https://learn.microsoft.com/en-us/rest/api/azure/devops/git/policy-configurations/get?view=azure-devops-rest-7.2).

### Additional Pull Request Improvements

- Planned features include: highlighting PRs with outstanding comments, resetting PRs to "ready for review" state, and filtering by tags.
- Many improvements are community-driven.
- Continuous improvements aim to streamline code review processes.

## Further Resources

- [Azure DevOps Roadmap](https://learn.microsoft.com/en-us/azure/devops/release-notes/features-timeline)
- [Azure DevOps Community](https://developercommunity.visualstudio.com/AzureDevOps)

These updates demonstrate Microsoft's ongoing commitment to refining the developer and DevOps experience within Azure Repos. Stay current by following roadmap updates and sprint notes.

This post appeared first on "Microsoft DevOps Blog". [Read the entire article here](https://devblogs.microsoft.com/devops/whats-new-with-azure-repos/)
