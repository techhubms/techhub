---
layout: "post"
title: "Reverting AzureRmWebAppDeployment Task Version in Azure Pipelines"
description: "This community post discusses whether you can roll back the AzureRmWebAppDeployment pipeline task from version 3.259.0 to an earlier version (such as 3.249.0) in Azure DevOps. It explores methods like forking and rebuilding tasks, pinning versions in YAML, and alternative approaches such as custom PowerShell or Python scripts, with concrete links and advice shared by the community."
author: "magielonczyk"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/azuredevops/comments/1mj53qd/azure_pipeline_task_reverting_to_old_one/"
viewing_mode: "external"
feed_name: "Reddit Azure DevOps"
feed_url: "https://www.reddit.com/r/azuredevops/.rss"
date: 2025-08-06 13:45:43 +00:00
permalink: "/2025-08-06-Reverting-AzureRmWebAppDeployment-Task-Version-in-Azure-Pipelines.html"
categories: ["Azure", "DevOps"]
tags: ["Azure", "Azure DevOps", "Azure Pipelines", "Azure Pipelines Tasks", "AzureRmWebAppDeployment", "CI/CD", "Community", "Custom Tasks", "Deployment Automation", "DevOps", "npm Packages", "Pipeline Tasks", "PowerShell", "Rollback Strategies", "Task Publishing", "Task Versioning", "Tfx", "YAML Pipelines"]
tags_normalized: ["azure", "azure devops", "azure pipelines", "azure pipelines tasks", "azurermwebappdeployment", "cislashcd", "community", "custom tasks", "deployment automation", "devops", "npm packages", "pipeline tasks", "powershell", "rollback strategies", "task publishing", "task versioning", "tfx", "yaml pipelines"]
---

magielonczyk and the community provide practical approaches to reverting an AzureRmWebAppDeployment task to an earlier version in Azure DevOps, including forking, publishing custom tasks, and YAML version pinning.<!--excerpt_end-->

# Reverting AzureRmWebAppDeployment Task Version in Azure Pipelines

**Author:** magielonczyk

## Question

Is it possible to revert the AzureRmWebAppDeployment (V4) pipeline task from version 3.259.0 to 3.249.0? The user references npm registry details and tfx usage:

- [Azure Pipelines Tasks - npm registry link](https://registry.npmjs.org/azure-pipelines-tasks-azure-arm-rest/)
- Using `tfx build tasks` with direct task API URL to a specific version.

## Community Insights and Replies

### Fork and Publish Approach

- You can fork the task repository, build your version, then publish it to your Azure DevOps organization.
- Afterwards, update your pipelines to reference the custom (older) task.

### Direct Version Pinning in YAML

- In YAML pipelines, you can specify a particular version directly:

  ```yaml
  - task: AzureRmWebAppDeployment@3.249.0
  ```

- As long as Microsoft hasn't removed the prior version, this will work for major version changes (not always possible with minor ones).

### Using tfx

- tfx can be used for managing and uploading specific task versions if you have access.

### Workarounds

- If reverting isn't possible or sustainable, another option is to use custom PowerShell or Python scripts instead of relying on changing Microsoft tasks. This can give more control and prevent future unexpected breaks.

### Reporting Issues

- If there are breaking changes or issues with the latest version, report them on the [Azure Pipelines Tasks GitHub repo](https://github.com/microsoft/azure-pipelines-tasks/issues) with details. The maintainers may assist or resolve the issue.

## Version Reference

The post included a detailed list of published task versions and their release dates for tracking available versions.

## Summary Steps

1. Try pinning the task version in YAML (`AzureRmWebAppDeployment@3.249.0`).
2. Fork, build, and publish the prior version if necessary.
3. Consider alternatives such as custom deployment scripts for stability.
4. Report incompatibilities or issues to Microsoft via GitHub.

This post appeared first on "Reddit Azure DevOps". [Read the entire article here](https://www.reddit.com/r/azuredevops/comments/1mj53qd/azure_pipeline_task_reverting_to_old_one/)
