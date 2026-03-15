---
external_url: https://www.reddit.com/r/azuredevops/comments/1m6btbu/web_app_service_wrong_version/
title: Fixing Node.js Version Mismatch in Azure Web App Service Deployment
author: Logical-Try6336
feed_name: Reddit Azure DevOps
date: 2025-07-22 11:41:42 +00:00
tags:
- Azure DevOps
- Azure Web App
- Container Deployment
- Dependency Management
- Environment Variables
- Linux Container
- Node.js
- Version Management
- YAML
- Yarn
- Azure
- DevOps
- Community
section_names:
- azure
- devops
primary_section: azure
---
Logical-Try6336 shares troubleshooting steps for fixing a Node.js version issue during Azure Web App Service deployment with Azure DevOps.<!--excerpt_end-->

Logical-Try6336 describes a deployment issue when hosting a web application in Azure via Azure DevOps pipelines. The application requires Node.js version 20, which is specified in the deployment YAML configuration, environment variables, and container settings. However, the deployed Azure container uses Node.js v18.17.1.

### Problem Details

- The application’s dependencies, managed with yarn, require Node.js >20.
- Locally, the application backend works as expected with the correct Node.js version.
- On Azure, after deployment with DevOps (which reports no errors), the application fails to start. Manual SSH is inaccessible, but running bash reveals the active Node.js version is still 18.17.1.
- Running `yarn install` manually in the container raises an explicit error stating the Node.js version is 18, while >20 is expected.

### Attempted Solutions

- The deployment YAML file specifies Node.js version 20.
- All relevant environment variables and container parameters have been configured for Node.js 20.
- Multiple troubleshooting sessions have been conducted with Microsoft Support, but the issue persists.

### Summary

The author seeks community input on enforcing the correct Node.js version (20) in Azure’s Web App Service container, given that deployment configuration does not reflect in the running environment, leading to dependency installation failures.

This post appeared first on "Reddit Azure DevOps". [Read the entire article here](https://www.reddit.com/r/azuredevops/comments/1m6btbu/web_app_service_wrong_version/)
