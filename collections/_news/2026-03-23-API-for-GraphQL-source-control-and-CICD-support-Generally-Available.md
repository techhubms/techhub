---
tags:
- API For GraphQL
- Artifact Versioning
- Azure DevOps
- Branching Strategy
- CI/CD
- Deployment Pipelines
- DevOps
- Environment Promotion
- Fabric Data Engineering
- Fabric Deployment Pipelines
- Git Integration
- Governance
- GraphQL
- Microsoft Fabric
- ML
- News
- Pull Requests
- Release Management
- Source Control
date: 2026-03-23 08:30:00 +00:00
primary_section: ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/api-for-graphql-source-control-and-ci-cd-support-generally-available/
title: API for GraphQL source control and CI/CD support (Generally Available)
author: Microsoft Fabric Blog
section_names:
- devops
- ml
feed_name: Microsoft Fabric Blog
---

Microsoft Fabric Blog announces general availability of source control and CI/CD support for the API for GraphQL in Fabric Data Engineering, covering Git versioning, pull-request workflows, and deployment pipelines for promoting GraphQL artifacts across environments.<!--excerpt_end-->

# API for GraphQL source control and CI/CD support (Generally Available)

*If you haven’t already, check out Arun Ulag’s hero blog “FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform” for a complete look at all of our FabCon and SQLCon announcements across both Fabric and our database offerings:* [FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news)

Source control and deployment capabilities for the [API for GraphQL](https://learn.microsoft.com/fabric/data-engineering/api-graphql-overview) in Microsoft Fabric Data Engineering are now available. This release targets improved reliability and performance for Fabric CI/CD and deployment pipelines, so you can:

- Version GraphQL artifacts in Git
- Collaborate through pull requests
- Promote changes across environments with consistent governance

## Features with CI/CD support

- **Version GraphQL artifacts in Git** to review changes and roll back when needed.
- **Release through Fabric deployment pipelines** to manage API for GraphQL items across environments.
- **Collaborate with pull requests and reviews** to apply branching and governance to API changes.

![Diagram showing how the development process works.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/diagram-showing-how-the-development-process-works-.png)

*Figure: Diagram showing how the development process works.*

## How it works

1. **Connect your workspace to Git** and choose a repository/branch for your GraphQL API artifacts.

![Screenshot of workspace settings Git integration window with workspace connected to main branch of repo.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-workspace-settings-git-integration-w.png)

*Figure: Enable Git integration on your workspace.*

2. **Develop and iterate** in your preferred branching model (feature branches, trunk-based, etc.).
3. **Review and merge** changes through pull requests to ensure quality and governance.
4. **Deploy through your pipeline** from ADO or using Fabric Deployment pipelines to promote approved changes across environments with consistent configuration.

## Next steps

- Follow the end-to-end tutorial for development lifecycle management in Fabric: https://learn.microsoft.com/fabric/cicd/cicd-tutorial?tabs=azure-devops%2Cmanual
- Enable source control for your GraphQL artifacts: https://learn.microsoft.com/fabric/data-engineering/graphql-source-control-and-deployment
- Set up a deployment pipeline to standardize releases across environments.


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/api-for-graphql-source-control-and-ci-cd-support-generally-available/)

