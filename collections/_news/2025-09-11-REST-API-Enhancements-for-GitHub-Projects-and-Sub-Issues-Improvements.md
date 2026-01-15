---
layout: post
title: REST API Enhancements for GitHub Projects and Sub-Issues Improvements
author: Allison
canonical_url: https://github.blog/changelog/2025-09-11-a-rest-api-for-github-projects-sub-issues-improvements-and-more
viewing_mode: external
feed_name: The GitHub Blog
feed_url: https://github.blog/changelog/feed/
date: 2025-09-11 18:42:31 +00:00
permalink: /devops/news/REST-API-Enhancements-for-GitHub-Projects-and-Sub-Issues-Improvements
tags:
- API Integration
- Automation
- Collaboration
- Collaboration Tools
- Cross Organization Issues
- Development Workflow
- DevOps
- GitHub
- GitHub Notifications
- Issue Management
- Microsoft Teams
- Milestones
- News
- Project Management
- Projects & Issues
- Projects API
- REST API
- Sub Issues
section_names:
- devops
---
Allison details GitHub's latest updates, including a REST API for Projects, improved sub-issue handling, and the renaming of the GitHub for Microsoft Teams app to GitHub Notifications.<!--excerpt_end-->

# REST API Enhancements for GitHub Projects, Sub-Issues Improvements, and More

GitHub has introduced several major updates aimed at improving project management and collaboration:

## REST API for GitHub Projects

Developers can now manage their projects programmatically through the Projects REST API. Key capabilities include:

- Listing and retrieving project details, including fields and items
- Adding and removing issues or pull requests from a project
- Updating project item field values

For complete API capabilities and usage examples, refer to [GitHub's Projects REST API documentation](https://docs.github.com/rest/projects).

## Sub-Issues Improvements

- **Inheritance:** Sub-issues now automatically inherit the Project and Milestone of their parent issue by default
- **Cross-organization Sub-issues:** You can now create sub-issues that belong to a different organization than the parent issue
- **Parent Issue Lookup:** The API now includes an endpoint to fetch the parent issue for a given sub-issue. See the [API documentation](https://docs.github.com/en/rest/issues/sub-issues?apiVersion=2022-11-28#get-parent-issue) for more information

These changes aim to make it easier to break down and track work across complex, multi-organization setups.

## Sticky Sidebar for Issues

A new sticky sidebar on GitHub Issues keeps key details like assignees, labels, types, milestones, and projects visible as you scroll through long discussion threads. This quality-of-life update makes management more efficient without having to repeatedly scroll to the top.

## GitHub for Microsoft Teams App Renamed

The GitHub for Microsoft Teams app is now called **GitHub Notifications**. The change clarifies the app’s role in delivering real-time GitHub notifications directly into Microsoft Teams. All core functionalities remain unchanged; users should now mention the app as `@GitHub Notifications` instead of `@GitHub`.

## Feedback and Learning

- Share feedback through the [GitHub Community](https://github.com/orgs/community/discussions/categories/projects-and-issues) or use the **Give feedback** menu in Projects.
- Learn more about project planning at [GitHub Issues and Projects](https://github.com/features/issues), view updates on the [roadmap](https://github.com/orgs/github/projects/4247/views/1), and read [the documentation](https://docs.github.com/issues).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-11-a-rest-api-for-github-projects-sub-issues-improvements-and-more)
