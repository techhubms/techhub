---
external_url: https://devblogs.microsoft.com/all-things-azure/azure-devops-to-github-migration-playbook-unlocking-agentic-devops/
title: 'Azure DevOps to GitHub Migration Playbook: A Step-by-Step Guide for Agentic DevOps'
author: Philippe Didiergeorges
feed_name: Microsoft All Things Azure Blog
date: 2025-11-20 11:03:44 +00:00
tags:
- Agentic DevOps
- AI Developer Tools
- All Things Azure
- Automation
- Azure Boards
- Azure DevOps
- Azure Pipelines
- CI/CD
- Developer Productivity
- DevOps Integration
- GHE
- GitHub
- GitHub CLI
- GitHub Enterprise
- Hybrid Cloud
- Migration
- Personal Access Token
- Repository Management
- Source Code Migration
- AI
- Azure
- Coding
- DevOps
- GitHub Copilot
- News
section_names:
- ai
- azure
- coding
- devops
- github-copilot
primary_section: github-copilot
---
Philippe Didiergeorges delivers a thorough migration playbook for moving from Azure DevOps to GitHub, demonstrating how to combine both platforms while unlocking AI-driven developer productivity with GitHub Copilot.<!--excerpt_end-->

# Azure DevOps to GitHub Migration Playbook: Unlocking Agentic DevOps

_By Philippe Didiergeorges_

## Overview

In today's Microsoft ecosystem, Azure DevOps and GitHub Enterprise both offer robust SDLC platforms. This playbook guides you through a migration strategy that leverages the best of both: maintaining enterprise features from Azure DevOps and harnessing GitHub’s AI and open-source strengths, especially with GitHub Copilot.

---

## Why Consider Migration?

**Azure DevOps** offers advanced enterprise features:

- Azure Boards for project management
- Azure Pipelines for build and release automation
- Azure Test Plans for quality and tracking

**GitHub Enterprise** stands out with:

- AI-native development platform features
- GitHub Copilot for AI-powered coding, reviews, and automation
- GitHub Actions for flexible CI/CD
- Advanced Security and rich app integrations

**Hybrid Approach:** Move source code to GitHub to leverage AI, while retaining Azure Boards and Pipelines. With unified Microsoft licensing, users can seamlessly access both platforms.

---

## Migration Prerequisites

- Access to both Azure DevOps and GitHub from your machine
- Administrator rights and proper permissions on both platforms
- Personal Access Tokens (PAT) for authentication on Azure DevOps and GitHub
- Installation of [GitHub CLI](https://cli.github.com/manual/installation) and the `azdo2gh` extension

---

## Step-by-Step Migration

### 1. Set Environment Variables

```powershell
$env:ADO_PAT="YOUR_AZURE_DEVOPS_PAT"
$env:GH_PAT="YOUR_GITHUB_PAT"
```

### 2. Install Azure Pipelines Application on GitHub

- Install [Azure Pipelines from the Marketplace](https://github.com/marketplace/azure-pipelines) to connect your pipelines post-migration.

### 3. Organization Inventory

- Generate inventory using the extension:

```bash
gh ado2gh inventory-report --ado-org <your-org>
```

- Review generated CSVs for orgs, pipelines, repos, and projects.

### 4. Migration Script Generation

```bash
gh ado2gh generate-script \
  --ado-org <your-org> \
  --ado-team-project <project> \
  --github-org <org> \
  --all
```

- Customization options include repository selection, naming conventions, team roles, and more.

### 5. Analyze and Customize the Script

- Ensure environment variables are set
- Verify Azure Pipelines application is installed
- Adjust repository/team names to match conventions
- Use GitHub Copilot for batch modifications if desired

### 6. Execute the Migration Script

```powershell
.\migrate.ps1
```

- Monitors migration progress, preserves commit history, branches, tags, and PR metadata
- Error handling ensures continued progress even if some repositories fail

---

## Post-Migration Steps

### 7. Manage "Mannequins" (Unmapped Users)

- Extract mannequin list, update with actual usernames, and trigger the remapping process

### 8. Update Teams and Communications

- Inform teams of new repository locations and update procedures
- Instructions for updating local remotes:

```bash
cd path/to/your/repo
git remote set-url origin https://github.com/YOUR_ORG/YOUR_REPO.git
git remote -v
```

### 9. Validation and Testing

- Test workflows, pipelines, code reviews, and access
- Explore new features like Copilot, Advanced Security, and validate pipeline integrations

---

## Key Takeaways

- Hybrid strategy enables use of both Azure DevOps' robust tracking and GitHub's AI-powered capabilities
- Preparation and communication are critical for a smooth migration
- Unified licensing between platforms reduces friction and cost
- Automation ensures a reliable and auditable migration process

---

## Resources and Further Reading

- [Migration Playbook Blog Post](https://devblogs.microsoft.com/all-things-azure/azure-devops-to-github-migration-playbook-unlocking-agentic-devops/)
- [Build Session: Making Azure DevOps and GitHub greater than the sum of their parts](https://build.microsoft.com/en-US/sessions/BRK110)
- [GitHub Migration Guide](https://docs.github.com/en/migrations)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [GitHub Learning Pathways](https://resources.github.com/learn/pathways/)

---

Happy coding with GitHub Copilot and your new hybrid DevOps platform!

This post appeared first on "Microsoft All Things Azure Blog". [Read the entire article here](https://devblogs.microsoft.com/all-things-azure/azure-devops-to-github-migration-playbook-unlocking-agentic-devops/)
