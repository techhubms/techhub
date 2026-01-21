---
external_url: https://www.reddit.com/r/azuredevops/comments/1mbguxq/how_to_only_allow_prs_if_pipelines_x_y_both_run/
title: Enforcing PR Branch Policies with Multiple Required Pipelines in Azure DevOps
author: panzerbjrn
feed_name: Reddit Azure DevOps
date: 2025-07-28 13:56:17 +00:00
tags:
- Azure DevOps
- Azure Pipelines
- Azure Repos
- Branch Policies
- Build Definitions
- Build Validation
- CI/CD
- Inner Source
- Pipeline Configuration
- Pipeline Requirements
- PR Approval
- PR Automation
- Pull Requests
- Terraform
- YAML Pipelines
section_names:
- azure
- devops
---
Panzerbjrn and other community members discuss how to enforce that PRs in Azure DevOps are only merged if specific pipelines (X & Y) both succeed. The thread includes setup guidance and clarifies differences between classic and YAML pipelines.<!--excerpt_end-->

# Enforcing Multiple Pipeline Requirements for PRs in Azure DevOps

**Author:** panzerbjrn (community discussion)

## Scenario

You want pull requests (PRs) targeting a specific branch to be automatically rejected unless *both* of two specific Azure DevOps pipelines ("X" and "Y") run successfully (green). The goal is to prevent merging untested code and avoid subsequent rollbacks.

These pipelines are defined as YAML (not classic) and work with Terraform. The scenario involves internal build agents and Azure DevOps-hosted repos and pipelines.

## Solution Steps

1. **Go to Azure DevOps Repo Settings**
   - Navigate to your repository and open the settings panel.
2. **Configure Branch Policies**
   - Select the target branch for which you want to enforce policies (such as `main` or `develop`).
3. **Add Build Validation for Each Pipeline**
   - Under **Branch policies**, find the **Build validation** section.
   - Click the `+` button to add a build validation policy for *each* required pipeline (e.g., once for Pipeline X, once for Pipeline Y).
   - Each will require successful completion before a PR can be merged.
4. **PR Workflow**
   - When a contributor opens a PR, both pipelines are triggered on the PR branch.
   - If *both* pass (success or warnings), the PR can be completed.
   - If *either* fails, merging is blocked, ensuring code quality.

## Additional Notes

- If using YAML pipelines, ensure each has a clear `pr` trigger so they run on PR creation.
- Some confusion may arise regarding "Build Definitions"—these are more relevant for classic pipelines, but YAML pipelines can still be attached via branch policy by referencing the pipeline name.
- If you do *not* see your YAML pipelines available under build validation, ensure the pipeline exists and is configured to trigger on PR events.
- Internal agents or hosted agents are both supported.

## Common Issues

- **Pipeline Not Listed:** Sometimes YAML pipelines may not appear when adding build validation. Double-check your YAML triggers and that the pipeline has run at least once.
- **Classic vs YAML:** "Build Definitions" in older documentation often refer to classic pipelines, but YAML is preferred for modern scenarios.

## Useful References

- [Azure DevOps Docs: Branch Policies](https://learn.microsoft.com/en-us/azure/devops/repos/git/branch-policies)
- [Azure DevOps Docs: YAML Pipelines and PR triggers](https://learn.microsoft.com/en-us/azure/devops/pipelines/repos/github-pr-triggers)

## Summary

Setting up multiple build validation policies on your target branch ensures only PRs passing all pipeline checks can be merged. This enforces quality and consistency for your codebase.

*If you run into issues with YAML pipelines not showing as options, make sure they have been executed at least once and have appropriate triggers configured.*

This post appeared first on "Reddit Azure DevOps". [Read the entire article here](https://www.reddit.com/r/azuredevops/comments/1mbguxq/how_to_only_allow_prs_if_pipelines_x_y_both_run/)
