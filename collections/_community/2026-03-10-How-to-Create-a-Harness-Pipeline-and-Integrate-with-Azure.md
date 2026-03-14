---
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/how-to-create-a-harness-pipeline-and-integrate-with-azure/ba-p/4499862
title: How to Create a Harness Pipeline and Integrate with Azure
author: gjayadev
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-03-10 09:06:01 +00:00
tags:
- AKS
- Automation
- Azure
- Azure Connector
- Azure Key Vault
- Azure Pipeline
- CI/CD
- Cloud Deployment
- Community
- Continuous Delivery
- Continuous Integration
- Delegate
- DevOps
- Harness
- IaC
- Kubernetes
- Pipeline Orchestration
- Secrets Manager
section_names:
- azure
- devops
---
gjayadev provides a hands-on tutorial for building CI/CD pipelines in Harness and integrating them with Microsoft Azure, highlighting pipeline setup, security, and automation best practices.<!--excerpt_end-->

# How to Create a Harness Pipeline and Integrate with Azure

Author: **gjayadev**

## Overview

Building reliable, automated CI/CD pipelines is critical for modern cloud-based applications. This step-by-step guide demonstrates how to use Harness for pipeline orchestration while securely integrating with Microsoft Azure to accelerate deployments, enforce governance, and boost security.

## What You'll Learn

- Core components of Harness required for Azure integration
- How to securely authenticate and interact with Azure resources
- Building and running a CI pipeline with best practices for scalability and security

---

## Key Concepts

### What is Harness?

Harness is a modern CI/CD platform offering:

- Continuous Integration (CI)
- Continuous Delivery (CD)
- Feature Flags
- Cloud Cost Management
- Security Testing
- Chaos Engineering

Its Delegate-based execution model allows you to securely run deployments in your own environment—ensuring secrets and credentials are kept inside your infrastructure.

### Harness Core Components for Azure

- **Harness Delegate:** Lightweight agent, deployed within your Azure environment (VM, AKS, VNet), handles tasks, deployments, and resource management.
- **Azure Connector:** Secure configuration holding credentials and endpoints, enables authentication and communication with Azure services (VMs, Storage, AKS, etc.).
- **Secrets Manager:** Handles encrypted storage and retrieval of sensitive values (e.g., Azure service principal credentials, API keys). Can work with Azure Key Vault or other vaults.
- **Organizational Hierarchy:** Structure for organizing accounts, orgs, and projects, and sharing resources like connectors and delegates.

## How Harness Connects to Azure

Harness integrates with Azure using several secure connectors and agents:

- **Azure Connector** for authenticating to your Azure Subscription using Client ID, Tenant ID, and related credentials
- **Kubernetes Connector** for AKS deployments
- **Secrets Manager** (optionally Azure Key Vault) for strong secret management
- **Harness Delegate** to execute all deployment tasks and manage infrastructure securely

## Step-by-Step Pipeline Creation in Harness with Azure

This section outlines how to create a new CI pipeline in Harness and integrate it with Azure services and resources.

### 1. Create a New Pipeline

- In the Harness UI, navigate to your project
- Click **Pipelines → New Pipeline**
- Enter a meaningful pipeline name and start building

### 2. Add a CI Stage

- Click **Add Stage**, select **CI**
- Choose your infrastructure type (Build for trial, or as needed)
- Give the stage a descriptive name (e.g., Build, Script Execution)

### 3. Select Build Infrastructure

- Open the **Infrastructure** tab in your CI stage
- Choose where pipeline tasks should run: Cloud, Kubernetes, Local, or VM
- For Azure, select **Kubernetes** (using AKS)
- Pick your Kubernetes Connector, select Namespace, and save

### 4. Add Execution Steps

- Move to the **Execution** tab and click **Add Step**
- Choose step types for your pipeline:
  - **Run Step:** Executes shell or PowerShell scripts
  - **Build & Push:** Builds and pushes container images
  - **Plugin Step:** Uses pre-defined plugins
  - **Test Step:** Runs automated tests
- For a simple setup, start with **Run Step** (e.g., echo "Running basic CI pipeline in Harness")
- Configure Docker images (e.g., alpine, ubuntu, or mcr.microsoft.com/azure-powershell)

### 5. Configure Environment Variables and Secrets

- In each Run Step, add any environment variables your scripts require
- Map sensitive values from Harness Secrets Manager for secure use of credentials and keys

### 6. Add Multiple Steps

- Design your automation workflow by adding steps for tasks like:
  - Unit tests
  - Linting
  - Artifact upload
  - Notifications

### 7. Save and Run the Pipeline

- Save your pipeline and hit **Run**
- Harness spin up the appropriate pod/VM, pulls your configured image, executes each step, and provides real-time logs
- Success or failure is displayed for transparency and troubleshooting

---

## Best Practices & Tips

- Always use Azure Connectors and Secrets Manager for credential handling; never hardcode secrets
- Deploy the Harness Delegate within your Azure network for optimal security and performance
- Take advantage of organizational hierarchy for resource sharing and governance
- Use Kubernetes connectors for AKS deployments; leverage Azure Key Vault for high-security scenarios

## Further Reading

- [Harness Documentation](https://developer.harness.io/docs/platform/get-started/key-concepts)
- [Azure Infrastructure Blog](https://techcommunity.microsoft.com/t5/s/gxcuf89792/m_assets/avatars/default/avatar-12.svg?image-dimensions=50x50)

---

**Author:** gjayadev  
Microsoft Tech Community Contributor

## Version History

Updated Mar 10, 2026  |  Version 1.0

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/how-to-create-a-harness-pipeline-and-integrate-with-azure/ba-p/4499862)
