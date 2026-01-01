---
layout: "post"
title: "Enterprise-Ready RBAC Model for Azure AI Landing Zone"
description: "This blueprint by Madhur_Shukla details a robust, enterprise-oriented RBAC framework for Azure-based AI Landing Zones. It covers best practices for access control, environment guardrails, role/persona mapping, custom roles, automation identities, governance, and operational security. The guidance is intended for technical teams deploying AI, ML, and data workloads on Azure."
author: "Madhur_Shukla"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-architecture-blog/access-governance-blueprint-for-ai-landing-zone/ba-p/4455910"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-09-25 18:29:20 +00:00
permalink: "/2025-09-25-Enterprise-Ready-RBAC-Model-for-Azure-AI-Landing-Zone.html"
categories: ["AI", "Azure", "DevOps", "ML", "Security"]
tags: ["Access Guardrails", "Access Patterns", "AI", "AKS", "Azure", "Azure AI", "Azure AI Foundry", "Azure AI Search", "Azure API Management", "Azure App Service", "Azure Container Apps", "Azure Landing Zone", "Azure Machine Learning", "Azure OpenAI", "Azure SQL", "CI/CD", "Community", "Cosmos DB", "Custom Roles", "Data Security", "DevOps", "Entra ID", "Governance", "Identity Architecture", "Least Privilege", "Managed Identities", "Microsoft Defender For Cloud", "ML", "MLOps", "PIM", "RBAC", "Role Based Access Control", "Security", "Segregation Of Duties", "Service Principals", "Zero Trust"]
tags_normalized: ["access guardrails", "access patterns", "ai", "aks", "azure", "azure ai", "azure ai foundry", "azure ai search", "azure api management", "azure app service", "azure container apps", "azure landing zone", "azure machine learning", "azure openai", "azure sql", "cislashcd", "community", "cosmos db", "custom roles", "data security", "devops", "entra id", "governance", "identity architecture", "least privilege", "managed identities", "microsoft defender for cloud", "ml", "mlops", "pim", "rbac", "role based access control", "security", "segregation of duties", "service principals", "zero trust"]
---

Madhur_Shukla presents a comprehensive, technical RBAC governance framework for Azure AI Landing Zones, covering access models, personas, automation, and key security guardrails for enterprise ML/AI workloads.<!--excerpt_end-->

# Enterprise-Ready RBAC Model for Azure AI Landing Zone

**Author:** Madhur_Shukla

## Overview

This blueprint defines an enterprise-ready access governance and RBAC (Role-Based Access Control) model for Azure AI Landing Zones. It focuses on secure enablement of AI, ML, and data services while following best practices such as least privilege, segregation of duties, and strict environment guardrails.

---

## Governance Principles

- **Least Privilege:** Only required permissions are granted per role.
- **Segregation of Duties (SoD):** Distinct permissions for building, deploying, operating, and securing workloads.
- **Environment Guardrails:**
  - **Dev:** Open for experimentation.
  - **Nonprod:** Controlled integration and validation.
  - **Prod:** No human write access; CI/CD-only with PIM Just-in-Time access as exception.
- **Identity Strategy:**
  - Managed Identities for workloads.
  - Service Principals for pipeline automation.
  - PIM for human elevation.
  - Break-glass accounts for emergencies only.

### Key Benefits

- Aligns security to enterprise and regulator needs.
- Defines accountability by persona and environment.
- Maintains consistent control-plane and data-plane access.
- Enforces operational guardrails (cost, compliance, and security).

---

## Covered Azure Services

- Azure Machine Learning (Workspaces)
- Azure AI Search
- Azure AI Services / OpenAI
- Azure Kubernetes Service (AKS)
- Azure App Service / Functions
- Azure API Management
- Azure Container Apps
- Azure Cosmos DB
- Azure SQL (DB and Managed Instance)
- MySQL/PostgreSQL Flexible Server
- AI Foundry
- Storage Accounts

---

## Key Personas and Access Postures

| Persona                   | Responsibilities                                               | Access Posture                                                      |
|--------------------------|---------------------------------------------------------------|----------------------------------------------------------------------|
| Data Scientist (DS)      | Data exploration, model training, notebooks, pipelines        | Contributor (Dev), Reader (Prod), limited (Nonprod)                  |
| ML Engineer (MLE)        | Pipeline productionization, compute management, MLOps          | Contributor (Dev/Nonprod), Reader (Prod, CI/CD-only)                 |
| AI Engineer (AIE)        | AI apps/services, integrating models                           | Contributor (lower env), Reader (Prod, pipeline for deploys)         |
| ML Operator              | Run and monitor ML workloads, health, rollback                | Limited contributor (lower env), Monitor/Trigger (Prod)              |
| MLOps / DevOps           | CI/CD, infra as code, promotions                              | Contributor (pipeline identities), Reader/Monitor (human)            |
| Operations               | Incident response, observability                              | Monitor Contributor (Prod), Viewer (others)                          |
| Subscription Owner       | Billing, policy, governance                                   | Owner (prefer automation)                                            |
| Security & Compliance    | Policy, threat protection, compliance                         | Security Reader/TI (no data-plane write)                             |

---

## Environment Guardrails

| Environment | Purpose                        | Human Access                        | Pipeline Access         | Guardrails                                 |
|-------------|-------------------------------|-------------------------------------|------------------------|---------------------------------------------|
| Dev         | Experimentation, rapid change | DS/MLE/AIE: Contributor             | Full (IaC encouraged)  | No PII, quota limits, dev SKUs              |
| Nonprod     | Integration, validation       | MLE/AIE: Contributor; DS: Reader    | Full (gates/approval)  | Policies, private endpoints, CMK as needed   |
| Prod        | Production workloads          | Reader/Monitor only                 | Full (mandatory)       | PIM JIT, break-glass, Defender, alerting     |

---

## Persona vs Environment Access Matrix

- **Contributor = 🟢, Reader = 🟡, Restricted = 🔴**

| Persona           | Dev   | Nonprod    | Prod      |
|-------------------|-------|------------|-----------|
| Data Scientist    | 🟢     | 🟡         | 🔴        |
| ML Engineer       | 🟢     | 🟢         | 🔴        |
| AI Engineer       | 🟢     | 🟢         | 🔴        |
| ML Operator       | 🟡     | 🟡         | 🟡        |
| MLOps             | 🟡     | 🟡         | 🟢 (CI/CD only) |
| Operations        | 🔴     | 🟡         | 🟢        |
| Subscription Owner| 🟢     | 🟢         | 🟢        |

---

## Service-by-Service Example Role Assignments

- **Azure ML:** Data Scientist: Contributor (Dev), Reader (Prod)
- **AI Foundry/OpenAI:** AIE: Contributor (Dev), CI/CD/pipeline: Contributor (Prod)
- **AI Search:** MLE: Search Service Contributor (Dev/Nonprod), Ops: Data Reader (Prod)
- **AKS:** Cluster ops: Contributor (platform), Developers: Cluster User Role + K8s RBAC
- **Cosmos DB:** App/MI: Data Reader/Contributor (RBAC), Ops: Data Reader (Prod)
- **App Service/API Mgmt:** App teams: Contributor (Dev/Nonprod), Operators: Reader + CI/CD MI (Prod)

---

## Custom Role Example: AzureML Limited Compute Operator

```json
{
  "Name": "AzureML Limited Compute Operator",
  "IsCustom": true,
  "Description": "Operate ML compute (start/stop/attach/detach) without workspace admin actions.",
  "Actions": [
    "Microsoft.MachineLearningServices/workspaces/read",
    "Microsoft.MachineLearningServices/workspaces/computes/*/read",
    "Microsoft.MachineLearningServices/workspaces/computes/*/action"
  ],
  "NotActions": [
    "Microsoft.MachineLearningServices/workspaces/delete",
    "Microsoft.MachineLearningServices/workspaces/update"
  ],
  "DataActions": [],
  "NotDataActions": []
}
```

---

## Identities and Automation

- **Managed Identity:** For workloads (AML jobs, Functions, Container Apps) in prod.
- **Service Principal:** For pipeline deployments. Rotate secrets, use FIC when possible.
- **Break-glass Accounts:** For emergencies only. Excluded from strong CA/MFA for controlled use.
- **PIM:** For human elevation, time-bound with approval and justification.

---

## Entra ID Group Naming Guidance

- Short and long naming conventions for assignable roles (e.g., `ai-ds-dev`, `grp-ai-ds-<project>-dev`).
- Recommended to group by persona and environment for clarity and scale.

---

## Governance Workflows

- Access requests via ITSM/Access Packages (Entra ID).
- Pipeline/GitHub Actions/Terraform for deployments (approval gates, automation).
- PIM JIT elevation and strict recertification for human roles.

---

## Further Reading

- [Azure AI Foundry RBAC Concepts](https://learn.microsoft.com/en-us/azure/ai-foundry/concepts/rbac-azure-ai-foundry)
- [Azure ML IAM and Roles](https://learn.microsoft.com/en-us/azure/machine-learning/how-to-assign-roles)
- [Azure AI Search Security RBAC](https://learn.microsoft.com/en-us/azure/search/search-security-rbac)
- [Azure Cosmos DB Data Plane RBAC](https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/how-to-grant-data-plane-access)

---

## Changelog

- September 25, 2025 – Initial Release [Madhur_Shukla]

---

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/access-governance-blueprint-for-ai-landing-zone/ba-p/4455910)
