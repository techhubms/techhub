---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/transition-to-azure-functions-v2-on-azure-container-apps/ba-p/4457258
title: 'Transition to Azure Functions V2 on Azure Container Apps: Migration Guide and Feature Overview'
author: DeepGanguly
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-10-14 04:39:47 +00:00
tags:
- Alerting
- Application Insights
- Azure Container Apps
- Azure Functions
- Azure Pipelines
- CI/CD
- Containerization
- Custom Domains
- DAPR Integration
- Deployment Best Practices
- DevOps Automation
- Diagnostics
- Feature Comparison
- Function App
- GitHub Actions
- Health Probes
- Metrics
- Microsoft.App RP
- Microsoft.Web RP
- Private Endpoints
- Scaling
- Secrets Management
- Serverless
- V1 To V2 Migration
section_names:
- azure
- coding
- devops
---
DeepGanguly explains the migration from Azure Functions V1 to V2 on Azure Container Apps, highlighting technical improvements, resource model benefits, and a step-by-step guide for deploying serverless functions in container environments.<!--excerpt_end-->

# Transition to Azure Functions V2 on Azure Container Apps

## Introduction

Azure Functions on Azure Container Apps now support two deployment models: V1 (Legacy Microsoft.Web RP Model) and the recommended V2 (Microsoft.App RP Model). V2 offers a native, feature-rich experience for hosting serverless workloads as containers on Azure.

## V1 Limitations

### Troubleshooting Restrictions

- No direct container access or real-time log viewing.
- Console and live output are restricted; diagnostics are indirect via Log Analytics and Application Insights.

### Portal and Management Experience

- Missing features: multi-revision, easy auth, health probes, custom domains.

### DAPR Integration Challenges

- .NET isolated functions may encounter compatibility issues with DAPR during builds due to dependency conflicts.

## Functions V2: Improved Model

Functions V2 leverages the Microsoft.App RP for direct container app creation with `kind=functionapp`:

- **Resource consolidation:** Eliminates proxy Function App resources for simplified management.
- **Native features:** Multi-revision traffic splitting, seamless authentication, private endpoints, metrics, alerting, CI/CD integrations (Azure Pipelines, GitHub Actions), health probes, custom domains, secrets management, and sidecar containers.
- **No code changes needed:** Existing function container images work with V2.

### Feature Enhancements

- [Multi-revision management](https://learn.microsoft.com/en-us/azure/container-apps/revisions-manage)
- [Easy Auth](https://learn.microsoft.com/en-us/azure/container-apps/authentication)
- [Private Endpoint](https://learn.microsoft.com/en-us/azure/container-apps/networking)
- [Metrics & Alerting](https://learn.microsoft.com/en-us/azure/container-apps/metrics)
- [Azure Pipelines](https://learn.microsoft.com/en-us/azure/container-apps/azure-pipelines) and [GitHub Actions](https://learn.microsoft.com/en-us/azure/container-apps/github-actions)
- [Health Probes](https://learn.microsoft.com/en-us/azure/container-apps/health-probes)
- [Custom Domains & Certificates](https://learn.microsoft.com/en-us/azure/container-apps/custom-domains-managed-certificates)
- [Scale Settings](https://learn.microsoft.com/en-us/azure/container-apps/scale-app)
- [Secrets Management](https://learn.microsoft.com/en-us/azure/container-apps/manage-secrets)
- [Sidecar Containers](https://learn.microsoft.com/en-us/azure/container-apps/containers)

## Unsupported Legacy Deployment Approach

Deploying function images as plain container apps (without kind=functionapp) via Microsoft.App RP is no longer advised:

- Not officially supported.
- No autoscale; scaling must be configured manually.
- Lacks key V2 features down the roadmap (e.g., listing functions, function keys).

**Recommendation:** Migrate to V2 for maximum native Azure Container Apps support.

## Migration Checklist: V1 to V2

### 1. Preparation

- Identify if currently running Functions V1 (Web RP) in Azure Container Apps.
- Retrieve your parent function container image.
- Record all configurations: environment variables, secrets, storage connections, networking settings.
- Review quotas (memory, CPU, instance limits) in Azure Container Apps; adjust as needed.

### 2. Create V2 Container App

- In Azure Portal, select “Optimize for Functions app” or use CLI (`az functionapp create`) to deploy your image.
- No code rebuild required — reuse your function container image.
- Apply all configuration/environment variables from your previous deployment.
- For detailed instructions, visit [Functions on Container Apps V2 documentation](https://learn.microsoft.com/en-us/azure/container-apps/functions-usage).

### 3. Validation

- Test function triggers (HTTP, Event Hub, Service Bus, etc.).
- Validate all integrations with databases, storage, and other Azure services.

### 4. DNS and Custom Domains

- V2 app has a new DNS name.
- Update DNS records and rebinding SSL/TLS certificates.
- Notify stakeholders about DNS and endpoint changes; verify traffic routing and endpoints.

### 5. Cutover

- Switch production traffic to V2.
- Monitor closely for errors or scaling issues.
- Communicate with users and team about the transition.

### 6. Cleanup

- Remove old V1 deployment.
- Update project/team documentation with details and lessons learned.

## Feedback & Support

- Submit feedback/issues at the [Azure Container Apps GitHub repo](https://github.com/microsoft/azure-container-apps/issues).
- Technical questions via [Azure Support Portal](https://azure.microsoft.com/en-us/support/options/).
- Community guidance in [GitHub Discussions](https://github.com/microsoft/azure-container-apps) and [Azure Community forums](https://azure.microsoft.com/en-us/support/community/).
- For enterprise needs, coordinate with your Microsoft account team.

---

_Updated: Oct 14, 2025_

## Author

**DeepGanguly**

Follow the [Apps on Azure Blog](https://techcommunity.microsoft.com/t5/apps-on-azure/bg-p/AppsonAzureBlog) for more updates.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/transition-to-azure-functions-v2-on-azure-container-apps/ba-p/4457258)
