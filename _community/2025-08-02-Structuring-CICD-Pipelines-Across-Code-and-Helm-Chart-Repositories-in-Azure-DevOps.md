---
layout: "post"
title: "Structuring CI/CD Pipelines Across Code and Helm Chart Repositories in Azure DevOps"
description: "This community discussion explores best practices for implementing CI/CD pipelines in an Azure DevOps environment, where application code and Helm charts are stored in separate repositories. It covers key tools like Azure DevOps Pipelines, ACR, AKS, and Helm, shares real-world pipeline example snippets, workflow design strategies, and tackles challenges with artifact handoffs and automation between repos. The conversation includes community tips and warns about common anti-patterns."
author: "machosalade"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/azuredevops/comments/1mfmw05/how_to_structure_cicd_pipelines_across_two_repos/"
viewing_mode: "external"
feed_name: "Reddit Azure DevOps"
feed_url: "https://www.reddit.com/r/azuredevops/.rss"
date: 2025-08-02 10:03:38 +00:00
permalink: "/2025-08-02-Structuring-CICD-Pipelines-Across-Code-and-Helm-Chart-Repositories-in-Azure-DevOps.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["AKS", "Artifact Handoff", "Azure", "Azure Container Registry", "Azure DevOps", "Build Automation", "CI/CD", "Coding", "Community", "DevOps", "Docker", "Helm", "IaC", "Mono Repo", "Multi Repo CI/CD", "Pipelines", "Repository Structure", "YAML Pipelines"]
tags_normalized: ["aks", "artifact handoff", "azure", "azure container registry", "azure devops", "build automation", "cislashcd", "coding", "community", "devops", "docker", "helm", "iac", "mono repo", "multi repo cislashcd", "pipelines", "repository structure", "yaml pipelines"]
---

machosalade and others discuss best practices for wiring up CI/CD pipelines using Azure DevOps, ACR, AKS, and Helm, including sample pipeline YAML and repo structure recommendations.<!--excerpt_end-->

# Structuring CI/CD Pipelines Across Two Repositories in Azure DevOps

**Author:** machosalade

This discussion centers on building a clear CI/CD approach using Azure DevOps, Azure Container Registry (ACR), Azure Kubernetes Service (AKS), and Helm charts, with code and charts stored separately in two repos. It highlights community experience and practical advice for effective automation, artifact handoff, and avoiding pitfalls.

## Scenario Overview

- **repoA**: Service source code. Pipeline builds Docker image and pushes to ACR (`image:date-buildNumber` tag)
- **repoB**: Helm charts mono-repo. Pipeline versions and packages charts, pushes them to ACR as OCI artifacts

### Objective

Implement clean, automated CI/CD flow for deployment to AKS, given the separate repos for code and Helm charts.

## Pipeline Options Discussed

### Option 1: Release Pipeline in repoA

- Build and push Docker image to ACR as part of the pipeline
- After manual approval, pull the latest Helm chart from ACR and run `helm upgrade` on AKS (set new image tag via `--set` argument)

### Option 2: Release Pipeline in repoB

- All `helm upgrade` logic lives here
- Challenge: repoB must know which image tag was just built in repoA
- Need for cross-repo communication to automate the flow (e.g., artifact metadata handoff)

## Solutions and Community Insights

- **Azure DevOps does not provide out-of-the-box triggers across repos. Workarounds involve: **
  - Use pipeline resources to trigger repoB's pipeline after repoA's completes, passing artifact information via variables or files
  - Checking out repoB in repoA's pipeline, programmatically updating the relevant Helm values file with the new image tag
  - Example includes installing tools like yq for YAML manipulation, and committing changes to trigger a downstream pipeline in repoB (though this is controversial)

- **Alternative Approaches:**
  - Using external events to trigger builds (GitHub's `repository_dispatch` is mentioned as a point of inspiration)
  - Tagging rather than committing to trigger downstream pipelines, to avoid anti-patterns

## Example Pipeline Snippet from Discussion

```yaml
# Sample: Updating Helm values file in repoB within repoA pipeline

i.e. updating image tag after Docker push

resources:
  repositories:
    - repository: HelmChartsRepo
      type: git
      name: YourProject/repoB
      ref: main

steps:
- task: Docker@2
  displayName: 'Build and Push Docker Image to ACR'
  inputs:
    command: 'buildAndPush'
    repository: '$(imageName)'
    dockerfile: '**/Dockerfile'
    containerRegistry: 'YourAcrServiceConnection'
    tags: '$(imageTag)'

- checkout: self
  persistCredentials: true

- checkout: HelmChartsRepo
  persistCredentials: true

- bash: |
    git config --global user.email "pipeline@azuredevops.com"
    git config --global user.name "Azure DevOps Pipeline"
    cd $(Pipeline.Workspace)/repoB
    sudo wget https://github.com/mikefarah/yq/releases/latest/download/yq_linux_amd64 -O /usr/bin/yq && sudo chmod +x /usr/bin/yq
    yq -i '.image.tag = "$(imageTag)"' ./charts/my-app/values.yaml
    git add .
    git commit -m "Update image tag for $(imageName) to $(imageTag) [skip ci]"
    git push
  displayName: 'Update and Push Helm Values'
```

## Pitfalls and Cautions

- Committing to a repo directly from pipelines is flagged as an anti-pattern by community members
- Prefer solutions that pass artifacts or metadata rather than auto-committing to a code repo as part of deploy
- Triggers and communication between pipelines often require custom workarounds in multi-repo setups

## Key Takeaways

- There is no single "right" way, but prioritize separation of concerns, traceability, and minimizing technical debt
- Artifact-based handoff (e.g., image tag files, pipeline resources) is cleaner than auto-commits to code
- Automation can be achieved, but design trade-offs must be considered

## Further Discussion

- Alternatives like GitHub Actions for event-based triggers may offer more flexibility
- Developers are encouraged to weigh pros/cons of different coupling strategies between code and chart repos

---
**For full thread and implementation details, see the original discussion by machosalade and community responders.**

This post appeared first on "Reddit Azure DevOps". [Read the entire article here](https://www.reddit.com/r/azuredevops/comments/1mfmw05/how_to_structure_cicd_pipelines_across_two_repos/)
