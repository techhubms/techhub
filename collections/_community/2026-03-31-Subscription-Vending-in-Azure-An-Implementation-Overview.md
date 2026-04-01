---
author: abhilashasr
date: 2026-03-31 07:48:15 +00:00
section_names:
- azure
- devops
- security
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/subscription-vending-in-azure-an-implementation-overview/ba-p/4506350
primary_section: azure
tags:
- AVM
- Azure
- Azure Landing Zone
- Azure RBAC
- Azure Verified Modules
- Billing Scope
- Cloud Adoption Framework
- Community
- Custom Roles
- Deployment Pipeline
- DevOps
- Enterprise Agreement
- GitFlow
- Governance
- IaC
- JSON Parameters
- Management Groups
- Request Pipeline
- Resource Provider Registration
- Role Assignments
- Security
- Source Control
- Subscription Alias
- Subscription Budgets
- Subscription Democratization
- Subscription Vending
- Terraform
- Workload Isolation
- YAML Parameters
title: 'Subscription Vending in Azure: An Implementation Overview'
feed_name: Microsoft Tech Community
---

abhilashasr outlines how “subscription vending” in Azure Landing Zone uses automation and Infrastructure as Code to create governed Azure subscriptions at scale, including management group placement, billing alignment, budgets, provider registration, and RBAC setup.<!--excerpt_end-->

## Subscription Vending in Azure: An Implementation Overview

Subscription vending enables the creation of multiple Azure subscriptions using code, treating **subscriptions as the foundational unit for workload management** (instead of using resource groups as the primary boundary).

![Diagram of subscription vending](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/landing-zone/design-area/media/subscription-vending-high-res.png#lightbox)

This approach applies the concept of **subscription democratization** within the **Azure Landing Zone (ALZ)** model:

- Subscriptions become the main organizational boundary.
- It’s intended to help environments scale while strengthening regulation, governance, and security controls.
- Teams can move faster on migrations or new deployments while still working within guardrails.

## Subscription Vending Implementation Guidance

Subscription vending is typically implemented via automation and usually includes:

- Collecting subscription request data
- Initiating platform automation
- Creating subscriptions using Infrastructure as Code (IaC)

There are multiple ways to implement the automation. One example approach described is **GitFlow**.

### Example GitFlow-based flow

In the example model:

- Subscription request data is collected via a data collection tool.
- The request data is stored in a **JSON or YAML** parameter file.
- After approval, platform automation is triggered using:
  - a request pipeline
  - source control
  - a deployment pipeline
- IaC modules are used to create the subscription.

![Diagram of subscription vending GitFlow](https://learn.microsoft.com/en-us/azure/cloud-adoption-framework/ready/landing-zone/design-area/media/subscription-vending-high-res.png#lightbox)

### Implementation steps

- A data collection tool is used to gather subscription request information.
- Once the subscription request is approved, platform automation is initiated through the request pipeline, source control, and deployment pipeline.

To standardize foundational structure across environments, automation is implemented using **Infrastructure as Code**, enabling new subscriptions to be deployed with minimal effort.

## Resources Deployed During Subscription Creation

As a best practice, the following are deployed as part of subscription creation:

- **Management Group**
  - Created based on the organization’s design and structure.

- **Subscription**
  - Created using code according to design requirements.
  - During creation:
    - Billing account details are configured to align with the billing scope.
    - A **subscription alias** is added.
  - After creation:
    - The subscription is associated with the appropriate management group.
  - Lifecycle management notes:
    - Renaming or cancelling subscriptions can also be managed.
    - Cancelling a subscription through Terraform can deactivate it.
    - The subscription can be reenabled within **90 days**.
    - After **90 days**, the subscription is permanently deleted.

- **Budget**
  - Subscription budgets can be defined based on required thresholds.

- **Resource Provider Registration**
  - Required resource providers are enabled by default to allow necessary REST operations for resource deployment.

- **Identity Management**
  - Required role assignments (including custom roles) can be applied at the subscription or scoped level.
  - **Custom RBAC roles** can be created if prebuilt roles don’t meet requirements, and assigned at the subscription level.

## Additional notes

- A **subscription alias** in Azure is a resource type used to create a new subscription, typically under an **Enterprise Agreement (EA)** billing model.
- An alias enables creation of new subscriptions, but **cannot** be used to update existing subscriptions.

## Azure Verified Modules (AVM)

Azure provides **Azure Verified Modules (AVM)** for the resources involved in subscription creation:

- AVM modules are intended to standardize implementation and follow best practices.
- The reference implementation is available through the AVM pattern for subscription vending.

Link: Azure Verified Modules (AVM): https://azure.github.io/Azure-Verified-Modules/

## Source metadata

- Updated: Mar 30, 2026
- Version: 1.0
- Author: abhilashasr


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/subscription-vending-in-azure-an-implementation-overview/ba-p/4506350)

