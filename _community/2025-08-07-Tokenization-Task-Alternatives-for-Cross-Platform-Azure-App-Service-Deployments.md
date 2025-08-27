---
layout: "post"
title: "Tokenization Task Alternatives for Cross-Platform Azure App Service Deployments"
description: "This community discussion addresses challenges encountered when replacing the Solidify tokenization task in Azure DevOps pipelines. The main issues include the task's limitation to Windows agents and the requirement for installation on each runner. Participants share experiences and best practices for handling secrets and variable substitution across both Linux and Windows agents, aiming for a solution that avoids hardcoded secrets and agent-side installation. The conversation also touches on deployment philosophies, such as separation of build and deployment steps, and practical approaches for .NET 8 and Umbraco 13 deployments."
author: "YaMoef"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/azuredevops/comments/1mk7u14/better_solidify_tokenization_task/"
viewing_mode: "external"
feed_name: "Reddit Azure DevOps"
feed_url: "https://www.reddit.com/r/azuredevops/.rss"
date: 2025-08-07 18:24:01 +00:00
permalink: "/2025-08-07-Tokenization-Task-Alternatives-for-Cross-Platform-Azure-App-Service-Deployments.html"
categories: ["Azure", "Coding", "DevOps"]
tags: [".NET 8", "Azure", "Azure App Service", "Azure DevOps", "CI/CD", "Coding", "Community", "Cross Platform", "Deployment Pipelines", "DevOps", "Environment Variables", "Linux Agents", "Runtime Configuration", "Secrets Management", "Solidify", "Tokenization", "Umbraco 13", "Variable Substitution", "Windows Agents", "ZIP Deploy"]
tags_normalized: ["dotnet 8", "azure", "azure app service", "azure devops", "cislashcd", "coding", "community", "cross platform", "deployment pipelines", "devops", "environment variables", "linux agents", "runtime configuration", "secrets management", "solidify", "tokenization", "umbraco 13", "variable substitution", "windows agents", "zip deploy"]
---

YaMoef opens a discussion about the limitations of the Solidify tokenization task in Azure DevOps, focusing on multi-platform support and secure handling of secrets. Community responses offer practical deployment strategies.<!--excerpt_end-->

# Tokenization Task Alternatives for Cross-Platform Azure App Service Deployments

**Author:** YaMoef

## Problem Statement

YaMoef describes the use of the [Solidify tokenization](https://github.com/solidify/vsts-task-tokenize-in-archive) task to handle variable replacement in deployments to Azure App Services. The main limitations observed are:

- The Solidify task only operates on Windows agents
- It requires installation directly on every build agent
- Efforts to create a custom solution (including using AI tools) struggle with dynamically loading secrets without hardcoding them

## Community Discussion

**Key Issues Raised:**

- Need for a tokenization/variable substitution approach that works on both Linux and Windows
- Desire for a build pipeline step that doesn't require installing tools on the agent
- Safe and dynamic handling of secrets during deployment

**Best Practice Advice:**

1. **Separation of Build and Deployment:**
   - Build artifacts should be immutable; all configuration (including secrets) should be applied at deployment.
   - Similar to Docker workflows, use environment variables for runtime configuration instead of modifying artifacts in the pipeline.

2. **Using Environment Variables:**
   - Azure App Service supports configuring environment variables per deployment.
   - Substitute runtime settings using these variables rather than performing token substitution directly inside artifacts.

3. **.NET 8 and Umbraco 13 Context:**
   - For .NET-based applications, consider configuring variable substitution within the application itself or via platform features.
   - The need to preserve legacy workflows is acknowledged, especially for backwards compatibility.

**Sample Workflow:**

1. Build the .NET 8 / Umbraco 13 application → generate deployable artifact.
2. In the deployment pipeline, fetch configuration/secrets as environment variables from Azure (e.g., Key Vault or Azure DevOps variable groups).
3. Set these environment variables in App Service / deployment environment.
4. Deploy the unchanged artifact and rely on platform or application code to bind to these runtime configurations.

**Comments and Suggestions:**

- A participant notes: _"You shouldn't try to change the build artifact. Use mechanism to load runtime configuration separately."_
- Example given: using 'ZIP Deploy' to App Service, environment variables can safely manage configuration across tech stacks.
- Although moving away from tokenization is recommended, compatibility with existing workflows is sometimes necessary.

## Conclusion

While tokenization in pipelines (such as with Solidify) remains a legacy pattern for some teams, modern DevOps guidance encourages immutable artifacts with configuration injected only at deployment time. Azure platform supports this via environment variables and service integrations across both Linux and Windows App Service plans. For rolling migration, progressive improvement to this pattern is suggested, keeping backward compatibility in mind while planning eventual transition.

This post appeared first on "Reddit Azure DevOps". [Read the entire article here](https://www.reddit.com/r/azuredevops/comments/1mk7u14/better_solidify_tokenization_task/)
