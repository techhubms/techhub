---
author: TulikaC
primary_section: azure
tags:
- App Service For Linux
- Azure
- Azure App Service
- Azure DevOps
- CI/CD
- Community
- Deployment Logs
- Deployments Experience
- DevOps
- Drag And Drop Deployment
- GitHub Actions
- Kudu
- Runtime Logs
- SCM Site
- Server Side Build
- Skip Build
- Web App Deployment
- Zip Deployment
section_names:
- azure
- devops
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/a-simpler-way-to-deploy-your-code-to-azure-app-service-for-linux/ba-p/4510240
feed_name: Microsoft Tech Community
title: A simpler way to deploy your code to Azure App Service for Linux
date: 2026-04-10 09:48:40 +00:00
---

TulikaC introduces a new drag-and-drop zip deployment experience for Azure App Service for Linux via the Kudu/SCM site, including optional server-side builds, deployment phase tracking, and access to deployment and runtime logs.<!--excerpt_end-->

## Overview

Azure App Service for Linux now includes a new **Deployments** experience in the **Kudu/SCM** site designed to make it quicker to get code running on a web app.

## How to access the new deployment experience

1. Open the Kudu/SCM site for your app:

```text
<sitename>.scm.azurewebsites.net
```

2. Open the new **Deployments** experience.

## Deploy using a zip file

- Upload your app by **dragging and dropping a zip file** that contains your code.
- After upload, App Service shows the **contents of the zip**, so you can verify what you’re about to deploy.

## Build options

- If your application is already built and ready to run, you can **skip server-side build**.
- Otherwise, App Service can run the **build step** for you.

## Deploy and monitor progress

- Select **Deploy** to start the deployment.
- The experience shows progress through phases such as:
  - Upload
  - Build
  - Deployment
- You can also view **deployment logs** to see what’s happening during the process.

## Post-deployment verification

- After a successful deployment, you can view **runtime logs** to confirm the app has started successfully.

## When to use this vs CI/CD

- This workflow is positioned as ideal for getting started and quickly moving from code to a running app.
- For production workloads and teams that need repeatable releases, you’d typically use automated **CI/CD pipelines**, for example:
  - **GitHub Actions**
  - **Azure DevOps**

## Publication info

- Published: Apr 10, 2026
- Version: 1.0


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/a-simpler-way-to-deploy-your-code-to-azure-app-service-for-linux/ba-p/4510240)

