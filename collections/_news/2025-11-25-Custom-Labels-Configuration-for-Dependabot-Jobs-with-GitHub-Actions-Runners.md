---
external_url: https://github.blog/changelog/2025-11-25-custom-labels-configuration-option-for-dependabot-self-hosted-and-larger-github-hosted-actions-runners-now-generally-available-at-the-organization-level
title: Custom Labels Configuration for Dependabot Jobs with GitHub Actions Runners
author: Allison
viewing_mode: external
feed_name: The GitHub Blog
date: 2025-11-25 16:07:42 +00:00
tags:
- CI/CD
- Configuration
- Continuous Integration
- Custom Labels
- Dependabot
- GitHub Actions
- Improvement
- Job Queuing
- Kubernetes Runners
- Operational Governance
- Organization Scope
- Runner Groups
- Self Hosted Runners
- Supply Chain Security
- Workflow Management
section_names:
- devops
- security
---
Allison outlines the new ability for Dependabot jobs to use custom labels on self-hosted and larger GitHub-hosted Actions runners, enabling granular workload management for organizations and improved supply chain security.<!--excerpt_end-->

# Custom Labels Configuration for Dependabot Jobs with GitHub Actions Runners

Dependabot update jobs on GitHub can now target specific self-hosted and larger GitHub-hosted Actions runners using custom labels. Previously, all jobs required the fixed `dependabot` label, which introduced governance limitations in environments with restricted label usage, such as Kubernetes-based runner controllers.

## Key Enhancements

- **Custom Labels for Routing**: Organizations can now define any custom label for runners at the organization level, enabling precise targeting for Dependabot jobs.
- **Runner Group Scoping**: Optionally, jobs can be scoped by runner group name, adding an extra layer of granularity and control.
- **Operational Flexibility**: Reduces friction for setups that formerly needed a dedicated runner with only the `dependabot` label. Workloads can now be segmented according to security or performance needs.
- **Backward Compatibility**: Existing workflows using the `dependabot` label remain unchanged. The default label is still supported for backwards compatibility.

## Implications for Operations and Security

- **Granular Governance**: Custom labels allow finer workload segmentation and more secure or performant job routing, supporting complex organizational requirements.
- **Supply Chain Security**: Improved management of automation runners helps tighten control over dependency workflows, contributing to overall software supply chain security.
- **Configuration Best Practices**: If a specified label has no available runner online, Dependabot will queue the job until a matching runner becomes available. Be sure labels are spelled correctly to avoid configuration errors and job delays.

## Resources

- [Using custom labels with self-hosted runners](https://docs.github.com/actions/how-tos/manage-runners/self-hosted-runners/apply-labels)
- [Managing Dependabot on self-hosted runners](https://docs.github.com/code-security/dependabot/maintain-dependencies/managing-dependabot-on-self-hosted-runners)
- [GitHub Community announcements](https://github.com/orgs/community/discussions/categories/announcements)

## Summary

With this feature, GitHub enables better runner utilization, control, and security for organizations managing their CI/CD pipelines with Dependabot and self-hosted runners.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-25-custom-labels-configuration-option-for-dependabot-self-hosted-and-larger-github-hosted-actions-runners-now-generally-available-at-the-organization-level)
