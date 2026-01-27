---
external_url: https://www.reddit.com/r/azuredevops/comments/1malfn3/best_practice_for_yaml_pipelines/
title: Migrating Classic Azure DevOps Pipelines to YAML for Multi-Repo Apps
author: DrippinInSuccess
feed_name: Reddit Azure DevOps
date: 2025-07-27 12:47:44 +00:00
tags:
- .NET 8
- Angular
- Automation
- Azure App Service
- Azure DevOps
- CI/CD
- Classic Pipelines
- Cross Repository Pipelines
- Deployment Strategies
- Multi Environment Deployment
- Parameter Files
- Pipeline Templates
- Playwright Tests
- Release Pipeline
- YAML Pipelines
section_names:
- azure
- coding
- devops
primary_section: coding
---
DrippinInSuccess discusses with the community about migrating classic Azure DevOps build and release pipelines to YAML pipelines for an Angular and .NET 8 application. Multiple approaches, tips, and strategies are shared by experienced engineers.<!--excerpt_end-->

# Migrating Classic Azure DevOps Pipelines to YAML for Multi-Repo Apps

**Author:** DrippinInSuccess and community members

## Context

The author maintains an application split into two repositories: one for the Angular UI and another for a .NET 8 API. The deployment pipeline targets three environments: QA, PreProd, and Prod, using classic Azure DevOps pipelines—a build pipeline per component/environment and a single release pipeline coordinating artifacts and deploying to corresponding Azure App Service environments. There's also a Playwright test repo for regression testing before production deployment.

## Migration Goal

Migrate from classic pipelines to YAML-based Azure DevOps pipelines while maintaining environment separation, artifact deployments, and regression gates.

## Main Questions

- How to structure YAML pipelines with multiple repositories?
- How to coordinate deployment and testing (Playwright) across the UI and API?
- What is considered best practice for parameterization and environment configuration?

## Community Insights

### Multiple YAML Pipelines

- You can declare multiple YAML pipeline files per repository.
- There’s no need for a separate pipeline per environment; conditions and parameters can manage environment differences.
- Use conditional logic to apply variable sets, templates, or override steps based on the branch or pipeline parameters.

### Using Templates

- Create reusable templates for building and deploying both the UI and API.
- Templates (for building, deploying) can be stored in a shared repository and referenced as resources in your pipelines.

```yaml
# Example snippet for a template reference

stages:
  - template: build-ui.yaml
    parameters:
      environment: 'qa'
```

### Parameter Files for Environment Configuration

- Use convention-based parameter files (e.g., `myapiname.parameters.dev.json`), and select the appropriate file per environment dynamically in the pipeline.
- Reference parameters using YAML expressions:

```yaml
parameters:
  - name: environment
    type: string
    default: 'dev'

steps:
  - task: SomeTask@
    inputs:
      parametersFile: myapiname.parameters.${{ parameters.environment }}.json
```

### Pipelines Across Multiple Repositories

- It's common to have build pipelines in separate repositories (for UI and API), and coordinate deployments by linking pipelines through artifacts or resources.
- Central deployment YAMLs can consume artifacts from multiple repositories by declaring them as pipeline resources.

### Regression Testing Integration

- Regression (Playwright) tests can be run as a specific stage in the pipeline, acting as a gate prior to deployment to production.

### Key Takeaways and Best Practices

- Favor templates and parameterized pipelines over redundant per-environment YAML files.
- Use environment-based conditions and variables for environment-specific logic.
- Store shared logic in a dedicated templates repository for easy maintenance and upgrades across services.
- Cross-repository deployments can be handled by using pipeline resources and artifact sharing.

## Community Wisdom

- Avoid unnecessary pipeline duplication—use conditions and templates.
- Shared templates (build/deploy) significantly streamline maintenance for apps/services deployed to many environments.
- Name environment-specific parameter files consistently to reduce manual configuration.

## Summary

Migrating classic Azure DevOps pipelines to YAML can dramatically improve maintainability, especially for multi-repo/multi-env scenarios. Use templates for build and deploy logic, parameter files for environment configuration, and leverage pipeline resource features for cross-repository artifact coordination.

This post appeared first on "Reddit Azure DevOps". [Read the entire article here](https://www.reddit.com/r/azuredevops/comments/1malfn3/best_practice_for_yaml_pipelines/)
