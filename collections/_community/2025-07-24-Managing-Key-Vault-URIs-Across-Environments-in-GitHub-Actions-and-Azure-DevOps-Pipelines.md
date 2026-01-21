---
external_url: https://www.reddit.com/r/azuredevops/comments/1m886qz/pipeline_parameters/
title: Managing Key Vault URIs Across Environments in GitHub Actions and Azure DevOps Pipelines
author: Azure DevOps
feed_name: Reddit Azure DevOps
date: 2025-07-24 15:56:04 +00:00
tags:
- Access Policy
- ARM Templates
- Azure DevOps
- Azure Key Vault
- CI/CD
- Environment Variables
- GitHub Actions
- OIDC
- Pipeline Parameters
- Secrets Management
- Service Principal
- YAML Pipelines
section_names:
- azure
- devops
- security
---
Azure DevOps shares practical advice on securely managing Key Vault URIs in CI/CD pipelines across dev and prod environments, highlighting different approaches for Azure DevOps and GitHub Actions.<!--excerpt_end-->

# Managing Key Vault URIs in Multi-Environment Pipelines

**Author: Azure DevOps Community**

When working with CI/CD pipelines—whether in Azure DevOps or GitHub Actions—managing secure access to environment-specific Azure Key Vaults is a common challenge. Here’s how community experts recommend tackling this scenario:

## Problem Statement

You have a YAML pipeline that fetches secrets from a Key Vault URL. By default, your exported ARM template doesn’t specify the environment (dev, prod), and you’re unsure how to parameterize the Key Vault URI for different deployments, especially when transitioning from Azure DevOps to GitHub Actions.

## Key Recommendations

### 1. Parameterizing Environments

- **Azure DevOps Pipelines**:
  - Use [template parameters](https://learn.microsoft.com/en-us/azure/devops/pipelines/process/templates?view=azure-devops&pivots=templates-includes#add-parameters-to-job-stage-and-step-templates) to define different values for the Key Vault URL based on the environment.
  - Create distinct environments (Dev, Prod) and associate each with its own Key Vault.
  - Variable groups can be set up to retrieve secrets directly from Key Vault, or you can add a step at the start of your pipeline to fetch secrets dynamically—ensuring no sensitive data is stored directly in the pipeline definition.

- **GitHub Actions**:
  - While GitHub Actions doesn’t have direct YAML parameter support like Azure DevOps, you can achieve a similar effect using [`env`](https://docs.github.com/en/actions/learn-github-actions/environment-variables) variables and encrypted secrets scoped to environments.
  - Consider referencing environment-specific secrets and variables in your workflows, or use reusable workflows with inputs to toggle environments.

### 2. Authentication Considerations

- Use a service principal (SPN) with proper permissions to access the right Key Vault.
- Store the SPN’s `client_id`, `client_secret`, and other credentials as secrets.
- Wherever possible, prefer OIDC for authentication, which improves security and can remove the need for static client secrets.

### 3. Permissions & Policies

- Set appropriate access policies on each Key Vault for your pipelines’ service principals.

### 4. Further Reading & Resources

- Review Microsoft’s documentation linked above for Azure DevOps.
- For GitHub Actions, search for resources on secure deployment patterns (e.g., authenticating to Azure from GitHub Actions using OIDC or managed identities).
- Check out guides on configuring multi-environment workflows and secure secret access.

---

**References:**

- [Azure DevOps YAML Template Parameters](https://learn.microsoft.com/en-us/azure/devops/pipelines/process/templates?view=azure-devops&pivots=templates-includes#add-parameters-to-job-stage-and-step-templates)

---

By structuring your pipeline for environment-aware deployments, you improve both your security posture and operational flexibility.

This post appeared first on "Reddit Azure DevOps". [Read the entire article here](https://www.reddit.com/r/azuredevops/comments/1m886qz/pipeline_parameters/)
