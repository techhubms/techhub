---
external_url: https://devblogs.microsoft.com/devops/azure-developer-cli-azure-container-apps-dev-to-prod-deployment-with-layered-infrastructure/
title: 'Azure Developer CLI: Azure Container Apps Dev-to-Prod Deployment with Layered Infrastructure'
author: PuiChee (PC) Chan
feed_name: Microsoft DevOps Blog
date: 2025-11-04 22:45:46 +00:00
tags:
- '#azure'
- '#devops'
- Application Insights
- Azd
- Azure & Cloud
- Azure App Service
- Azure Container Apps
- Azure Container Registry
- Azure Developer CLI
- Azure Developer CLI (azd)
- CI/CD
- Container Deployment
- Flask
- GitHub
- GitHub Actions
- IaC
- Key Vault
- Layered Infrastructure
- Managed Identity
- Python
- VNET
section_names:
- azure
- coding
- devops
primary_section: coding
---
PuiChee (PC) Chan shares a practical guide on using Azure Developer CLI v1.20.0 and Azure Container Apps to enable build-once, deploy-everywhere patterns with layered infrastructure and robust CI/CD pipelines.<!--excerpt_end-->

# Azure Developer CLI: Azure Container Apps Dev-to-Prod Deployment with Layered Infrastructure

Author: PuiChee (PC) Chan

## Overview

This guide demonstrates how to implement build-once, deploy-everywhere patterns with Azure Container Apps utilizing new features introduced in Azure Developer CLI (azd) v1.20.0. It focuses on enabling seamless dev-to-prod workflows, layered infrastructure, and reusing container images across environments with secure separation of concerns.

Key topics include:

- Layered infrastructure deployments
- Azure Developer CLI's 'azd publish' and 'azd deploy --from-package'
- Container Registry sharing
- Real-world CI/CD guidance with GitHub Actions

## Problem: Simplifying Container Deployments to Multiple Environments

Deploying containers in production often requires:

- Building the container only once but deploying it to multiple isolated environments
- Security controls around which image versions reach production
- Environment-specific configuration without redundant builds

Previously, azd used a single-step deployment that bundled build, push, and deploy, which was not ideal for production needs.

## Solution: Azd v1.20.0 Layered Infrastructure & Separated Container Operations

### Features Introduced

- **Separated Container Operations**: Use `azd publish` to build and push to ACR; then use `azd deploy --from-package` to deploy specific images per environment.
- **Layered Infrastructure (Alpha Feature)**: Provision infrastructure in layers, allowing shared resources (e.g., ACR) to serve all environments, while environment-specific resources remain isolated.

### Layered Infrastructure Example

The approach uses layers for:

1. **Foundation** (container environment & managed identity)
2. **Shared ACR** (single registry for all environments)
3. **ACR Role Assignment** (appropriate permissions per environment)
4. **Container App** (application deployment)

Each layer passes required outputs to the next, ensuring correct dependency management.

#### Sample Structure

- **Shared resources**: Azure Container Registry (stores all app images)
- **Development environment**: Resource group, managed identity, storage, Key Vault, Application Insights
- **Production environment**: VNET, enhanced security/configuration, private endpoints

## Hands-on Guide

### Prerequisites

- Azure Developer CLI v1.20.0+
- Docker

### Steps

1. **Clone Sample Repository**

   ```sh
   azd init -t https://github.com/puicchan/azd-dev-prod-aca-storage
   ```

2. **Set Up Development Environment**

   ```sh
   azd config set alpha.layers on
   azd env new myapp-dev
   azd env set AZURE_ENV_TYPE dev
   azd up
   ```

3. **Prepare Production Infrastructure**

   ```sh
   azd env new myapp-prod
   azd env set AZURE_ENV_TYPE prod
   azd env set ACR_RESOURCE_GROUP_NAME rg-shared-acr-resource-group-name
   azd env set AZURE_CONTAINER_REGISTRY_ENDPOINT shared-acr-endpoint
   azd provision
   ```

   > Use `azd provision` for infra setup only before production. Infrastructure changes in prod should go through approval workflows.

4. **Configure CI/CD Pipeline**

   ```sh
   azd env select myapp-dev
   azd pipeline config
   # Follow prompts, selecting GitHub for the pipeline provider
   ```

   - The pipeline process:
     - **Build** (single container image in ACR)
     - **Deploy to Dev**
     - **Deploy to Prod** (same container image, no rebuild)

## How GitHub Actions Workflow Operates

- **Build job**: Builds & publishes container to ACR
- **Deploy-Dev job**: Deploys built image to development
- **Deploy-Prod job**: Deploys same image to production (using shared ACR)

The pipeline passes the name of the built container image between jobs, ensuring the same artifact is used in all environments.

See full workflow in the [azure-dev.yml](https://github.com/puicchan/azd-dev-prod-aca-storage/blob/main/.github/workflows/azure-dev.yml).

## Infrastructure and Security Notes

- Separation of environments prevents leakage between dev and prod
- Managed identities are used for secure resource access (no secrets in app code)
- Layer output management automates propagation of resource IDs and endpoints
- For private production, enable `internal: true` and use a reverse proxy for access

## Conclusion

With Azure Developer CLI v1.20.0 and support for layered infrastructure, Azure Container Apps can now adopt robust, production-grade deploy patterns. This approach supports scalable, maintainable solution architectures, and reinforces best practices for CI/CD in Azure environments.

For full details, sample code, and guidance, refer to the official [Azure Developer CLI documentation](https://aka.ms/azd) and the provided [GitHub repository](https://github.com/puicchan/azd-dev-prod-aca-storage).

---

*Questions or feedback? [Join the discussion](https://github.com/azure/azure-dev/discussions/5447).*

This post appeared first on "Microsoft DevOps Blog". [Read the entire article here](https://devblogs.microsoft.com/devops/azure-developer-cli-azure-container-apps-dev-to-prod-deployment-with-layered-infrastructure/)
