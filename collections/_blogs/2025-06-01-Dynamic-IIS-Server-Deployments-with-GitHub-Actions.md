---
layout: post
title: Dynamic IIS Server Deployments with GitHub Actions
author: Michiel van Oudheusden
canonical_url: https://www.mindbyte.nl/2025/06/02/dynamic-iis-server-deployments-github-actions.html
viewing_mode: external
feed_name: Michiel van Oudheusden's Blog
feed_url: https://mindbyte.nl/feed.xml
date: 2025-06-01 22:00:00 +00:00
permalink: /devops/blogs/Dynamic-IIS-Server-Deployments-with-GitHub-Actions
tags:
- Automation
- AWS
- Cloud Migration
- Deployment
- GitHub Actions
- Iis
- Self Hosted Runners
- Web Application
- Windows
- Windows Server
section_names:
- devops
---
Michiel van Oudheusden explores a practical approach to deploying web apps across multiple IIS servers using GitHub Actions, focusing on scalable DevOps strategies for traditional cloud infrastructure.<!--excerpt_end-->

## Dynamic IIS Server Deployments with GitHub Actions

**Author:** Michiel van Oudheusden

### Introduction

Migrating legacy or traditional application architectures to the cloud can present unique challenges—especially when modern orchestration (like containers or Kubernetes) isn’t feasible. In many scenarios, organizations must deploy and maintain web applications across multiple Windows IIS servers, often behind a cloud load balancer (such as AWS Elastic Load Balancer).

### The Problem

In the described project, the core challenge was:
> How to execute the same GitHub Actions deployment workflow to every IIS server in a way that is both scalable and maintainable?

While DevOps conversations today often focus on containerization and orchestration with Kubernetes, many teams work with established setups involving Windows servers and IIS.

### Architectural Context

- **Infrastructure:**
  - Application infrastructure hosted in cloud (specifically AWS).
  - Utilization of Windows servers running IIS.
  - Multiple IIS servers deployed behind an AWS load balancer.
- **Deployment:**
  - GitHub Actions used for build and deployment automation.
  - Need for consistent deployment across all servers in the farm.

### The Approach

1. **GitHub Actions Self-hosted Runners**:
   - Install GitHub Actions runners directly on each IIS server.
   - Configure the deployment workflow to trigger on each runner in the server pool.
2. **Scalability & Maintainability**:
   - Standardize the runner setup using scripts or configuration management tools.
   - Automate agent lifecycle (registration/unregistration) when servers are added or removed.
   - Implement monitoring for deployment status across all agents.
3. **Workflow Design**:
   - Define a GitHub Actions workflow that:
     - Packages the application code.
     - Deploys the artifacts to each IIS server via the self-hosted runner.
     - Runs health checks or post-deployment tests.

### Key Observations

- GitHub Actions, typically associated with cloud-native and containerized environments, can be adapted for traditional, stateful server deployments.
- Self-hosted runners provide fine-grained control for scenarios where cloud-native agents (such as GitHub-hosted runners) lack direct access to private infrastructure.
- This approach supports incremental migration to modern infrastructure over time, while preserving investment in established platforms.

### Considerations & Best Practices

- Secure the pipeline and runners, especially with sensitive credentials or keys on Windows servers.
- Keep runner environments consistent with automation or baseline images.
- Monitor the health and connectivity of all deployment targets.

### Conclusion

While containerization and modern orchestration technologies are increasingly prevalent, many organizations continue to depend on proven, “old-school” architectures for critical workloads. Leveraging tools like GitHub Actions with self-hosted runners allows for scalable and manageable deployments on IIS, bridging the gap between classic infrastructures and modern DevOps best practices.

This post appeared first on "Michiel van Oudheusden's Blog". [Read the entire article here](https://www.mindbyte.nl/2025/06/02/dynamic-iis-server-deployments-github-actions.html)
