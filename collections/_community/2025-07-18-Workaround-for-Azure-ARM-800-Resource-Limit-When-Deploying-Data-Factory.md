---
external_url: https://www.reddit.com/r/azuredevops/comments/1m30btw/workaround_for_azure_arm_800_resource_limit_while/
title: Workaround for Azure ARM 800 Resource Limit When Deploying Data Factory
author: jessred99
feed_name: Reddit Azure DevOps
date: 2025-07-18 11:55:43 +00:00
tags:
- ARM Deployment
- ARM Template
- Azure Data Factory
- Azure DevOps
- Ci/cd
- Continuous Delivery
- Data Factory Pipeline
- Deployment
- IaC
- Resource Limits
- Azure
- DevOps
- ML
- Community
- Machine Learning
section_names:
- azure
- devops
- ml
primary_section: ml
---
jessred99 raises a challenge in deploying Azure Data Factory via Azure DevOps CI/CD pipelines, encountering the ARM 800 resource limit.<!--excerpt_end-->

### Summary

jessred99 details an issue in deploying Azure Data Factory (ADF) via Azure DevOps CI/CD pipelines, specifically hitting the Azure Resource Manager (ARM) template limit of 800 resources per deployment. The approach involves exporting ARM templates using the ADF utility (`npm run build export`), which now results in ARM templates containing more than the allowed 800 resources. This constraint blocks deployment efforts.

The author asks whether it's possible to overcome this limitation by splitting the deployment into multiple, smaller ARM templates, thus staying below the resource count restriction per template.

### Key Points

- **Problem**: ARM deployment fails due to exceeding the 800-resource limit when deploying ADF via Azure DevOps.
- **Process**: ARM templates are generated using ADF utilities within a CI/CD pipeline.
- **Questions Raised**: Can one define more than 800 resources using multiple, smaller ARM templates, and orchestrate their deployment?

### Discussion

- **Azure ARM 800 Resource Limit**: Azure restricts ARM template deployments to a maximum of 800 resources per deployment operation.
- **Typical Workarounds**:
  - Split resources into separate ARM templates based on logical groupings (e.g., pipelines, datasets, linked services) and deploy them sequentially.
  - Use nested or linked templates to control deployment granularity.
  - Refactor the CI/CD pipeline to deploy individual components in smaller batches.
- **Considerations**:
  - Dependency management between resources needs careful attention.
  - Automation scripts or Azure DevOps pipelines might require additional coding to handle multiple deployment steps.

### Practical Advice

- Break up the Data Factory deployment into smaller resource groups and deploy in multiple steps using ARM linked templates or by scripting sequential deployments in your pipeline.
- Review Microsoft documentation or community solutions for edge cases related to dependencies and orchestration within Data Factory deployments.

### References

- [Azure Resource Manager template limits](https://docs.microsoft.com/azure/azure-resource-manager/management/azure-subscription-service-limits)
- [Deploy Azure Data Factory with CI/CD](https://docs.microsoft.com/azure/data-factory/continuous-integration-deployment)

This post appeared first on "Reddit Azure DevOps". [Read the entire article here](https://www.reddit.com/r/azuredevops/comments/1m30btw/workaround_for_azure_arm_800_resource_limit_while/)
