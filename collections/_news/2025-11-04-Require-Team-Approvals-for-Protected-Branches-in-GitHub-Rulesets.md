---
layout: "post"
title: "Require Team Approvals for Protected Branches in GitHub Rulesets"
description: "GitHub has introduced a new feature that allows repository administrators to mandate approvals from specific teams before changes can be merged into protected branches. This enhancement gives organizations granular control over workflows, enabling stricter policies based on file paths or critical code areas. Unlike CODEOWNERS, which assigns code responsibility, rulesets focus on enforcing review policies, offering enterprise-wide consistency and scalability. This update is especially useful for enforcing higher review standards on sensitive branches and improving governance."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-11-03-required-review-by-specific-teams-now-available-in-rulesets"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-11-04 00:47:20 +00:00
permalink: "/2025-11-04-Require-Team-Approvals-for-Protected-Branches-in-GitHub-Rulesets.html"
categories: ["DevOps"]
tags: ["Branch Protection", "Code Review", "CODEOWNERS", "Collaboration Tools", "DevOps", "Enterprise Collaboration", "GitHub", "Improvement", "News", "Platform Governance", "Policy Enforcement", "Release Management", "Repository Governance", "Rulesets", "Team Approvals", "Version Control", "Workflow Automation"]
tags_normalized: ["branch protection", "code review", "codeowners", "collaboration tools", "devops", "enterprise collaboration", "github", "improvement", "news", "platform governance", "policy enforcement", "release management", "repository governance", "rulesets", "team approvals", "version control", "workflow automation"]
---

Allison details GitHub's new ruleset feature requiring team approvals for protected branches, providing organizations with more control over code review and release workflows.<!--excerpt_end-->

# Require Team Approvals for Protected Branches in GitHub Rulesets

GitHub has rolled out an update allowing you to require approvals from specific teams before merging changes into protected branches. This feature helps organizations enforce stricter review policies and maintain better control over critical branches and code paths.

## What's New

- **Team Approvals**: Mandate reviews from designated teams for merges on protected branches.
- **Branch Protection**: Enforce consistent policies across various repositories or organization-wide.
- **Granular Controls**: Target specific files, folders, or patterns and set custom requirements for team approvals.

![Screenshot of the required reviewer rule dialog with a team selected as reviewer](https://github.com/user-attachments/assets/039259c4-3aca-4850-9579-b57550f3c8b3)

## How This Differs from CODEOWNERS

- **CODEOWNERS** is primarily used for defining responsibility over files and directories, auto-assigning reviews, and maintaining transparency.
- **Rulesets** focus on **enforcement**, letting you require a set number of approvals from chosen teams. Rulesets make it simple to protect sensitive or production branches and ensure critical code changes get the necessary scrutiny.
- These rules augment, but don’t replace, CODEOWNERS. Both can be used together for a flexible and secure workflow.

## Benefits

- Consistent enforcement of code review policies across projects
- Fine-grained control over who approves what
- Supports scaling policies enterprise-wide
- Enhances governance, especially for regulated or critical applications

## Resources

- [GitHub Documentation: Rulesets](https://docs.github.com/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/about-rulesets)
- [GitHub Community Discussion](https://gh.io/required-reviewer-feedback)

## Summary

This update provides teams with powerful tools to control merge policies, especially for sensitive code areas and critical branches. By leveraging team-based rulesets, organizations can enforce higher standards and transparently document review requirements, significantly improving overall DevOps and code governance.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-03-required-review-by-specific-teams-now-available-in-rulesets)
