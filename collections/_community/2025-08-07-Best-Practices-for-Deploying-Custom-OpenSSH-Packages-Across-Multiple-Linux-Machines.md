---
external_url: https://www.reddit.com/r/devops/comments/1mk0byh/installing_packages_not_available_in_linux_repos/
title: Best Practices for Deploying Custom OpenSSH Packages Across Multiple Linux Machines
author: antonioefx
feed_name: Reddit DevOps
date: 2025-08-07 13:37:34 +00:00
tags:
- Almalinux
- Artifactory
- Chef
- Configuration Management
- Deployment Pipeline
- DevOps Automation
- Docker Build Environment
- Fleet Operations
- FPM
- Internal Yum Repository
- Linux Packaging
- Nexus
- OpenSSH
- RPM
- DevOps
- Community
section_names:
- devops
primary_section: devops
---
antonioefx and community members outline practical steps for managing custom OpenSSH deployments on multiple Linux servers, emphasizing custom RPM creation, repository setup, and integrating with Chef for automation.<!--excerpt_end-->

# Best Practices for Deploying Custom OpenSSH Packages Across Multiple Linux Machines

Managing the deployment of newer versions of software like OpenSSH can be challenging when your Linux distribution repositories (e.g., Almalinux) do not offer the latest releases. This thread, initiated by antonioefx, explores several effective strategies for installing and maintaining custom packages at scale.

## Problem Statement

Deploying the latest OpenSSH on several Linux machines is cumbersome when repos lack up-to-date packages. Manual compilation on each host is inefficient as the fleet grows. The community discusses alternative approaches that streamline this process.

## Recommended Approach

### 1. Build a Custom RPM Package

- Use the official spec file as a starting point.
- Perform compilation in a clean, reproducible build environment (such as Docker) to ensure consistency.
- Tools like [FPM](https://fpm.readthedocs.io/) or native `rpmbuild` can be used to create an RPM out of your custom OpenSSH build.

### 2. Establish an Internal Repository

- Host your RPMs on a simple internal Yum repository, which could be served by Nginx, Nexus, or Artifactory.
- This gives you central control, versioning, and makes package delivery more efficient across all hosts.

### 3. Automate Deployment with Configuration Management (Chef)

- Update your Chef recipes to reference your internal repository rather than the system package manager. This allows you to automate upgrades and installations reliably across multiple servers.
- Testing the package in a staging environment before wide deployment is recommended for stability.

## Benefits

- **Version control:** You control which OpenSSH version each machine receives.
- **Auditability and consistency:** Builds and deployments can be tracked, and the same artifacts are delivered everywhere.
- **Efficient automation:** Leveraging a single build to deploy to many servers saves time and reduces risk.

## Community Tips

- Avoid compiling on every node—it wastes resources and time.
- Consider modern repo management tools (Nexus, Artifactory) for large fleets.
- Treat this as a mini deployment pipeline: build once, test, roll out.

For more insights on deployment strategies and automation, newsletters like NoFluffWisdom were mentioned for further tips.

This post appeared first on "Reddit DevOps". [Read the entire article here](https://www.reddit.com/r/devops/comments/1mk0byh/installing_packages_not_available_in_linux_repos/)
