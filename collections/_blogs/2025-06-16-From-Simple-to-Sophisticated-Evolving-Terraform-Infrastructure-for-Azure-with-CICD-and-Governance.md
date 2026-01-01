---
layout: "post"
title: "From Simple to Sophisticated: Evolving Terraform Infrastructure for Azure with CI/CD and Governance"
description: "Hidde de Smet details a practical evolution of Terraform infrastructure on Azure, guiding teams from basic scripts to modular, automated, and governed solutions. The blog covers modularization, naming standards, environment strategies, CI/CD pipelines, testing, policy as code, cost management, and monitoring, offering actionable lessons and a phased roadmap."
author: "Hidde de Smet"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://hiddedesmet.com/terraform-evolution"
viewing_mode: "external"
feed_name: "Hidde de Smet's Blog"
feed_url: "https://hiddedesmet.com/feed.xml"
date: 2025-06-16 05:00:00 +00:00
permalink: "/2025-06-16-From-Simple-to-Sophisticated-Evolving-Terraform-Infrastructure-for-Azure-with-CICD-and-Governance.html"
categories: ["Azure", "DevOps"]
tags: ["Automation", "Azure", "Blogs", "CI/CD", "Cost Management", "DevOps", "Drift Detection", "Environment Separation", "GitHub", "GitHub Actions", "Governance", "IaC", "Modules", "Naming Conventions", "Policy as Code", "Terraform", "Terratest", "Testing"]
tags_normalized: ["automation", "azure", "blogs", "cislashcd", "cost management", "devops", "drift detection", "environment separation", "github", "github actions", "governance", "iac", "modules", "naming conventions", "policy as code", "terraform", "terratest", "testing"]
---

In this comprehensive guide, Hidde de Smet documents the step-by-step evolution of Terraform infrastructure for Azure. The post provides real-world insights and actionable patterns for teams modernizing their infrastructure-as-code, from basic setup to advanced automation and governance.<!--excerpt_end-->

# From Simple to Sophisticated: Terraform Infrastructure Evolution

**By Hidde de Smet**

This post documents a practical journey in evolving Terraform-based Azure infrastructure from basic, single-file deployments to a modular, automated, and governed state. The guide emphasizes not just technical steps, but also the rationale—enabling teams to improve their own practices incrementally.

---

## Table of Contents

1. The Starting Point: Simple but Limited
2. Evolution Phase 1: Breaking Down the Monolith
3. Evolution Phase 2: Standardization and Governance
   - Naming Conventions and Environment Separation
   - Validation and Comprehensive Tagging
   - Workspace vs. Environment Separation Strategies
4. Evolution Phase 3: Automation and CI/CD
5. Evolution Phase 4: Comprehensive Testing
6. The Advanced Features: Beyond the Basics
   - Policy as Code
   - Cost Management Foundation
   - Infrastructure Monitoring Tools
   - Alternative Approaches
7. Key Lessons Learned
8. What’s Next: The Roadmap Ahead
9. Getting Started: Your Evolution Path
10. Conclusion: Evolution Over Revolution

---

## The Starting Point: Simple but Limited

**Version 0.1.0 - The Basic Foundation**

- Monolithic `main.tf` containing all Azure resources:
    - Resource Group
    - Virtual Network and Subnet
    - Network Security Group
    - Storage Account and Container
    - App Service Plan and Linux Web App
    - Key Vault
- Pain Points:
    - One massive file, no modularity or reusability
    - Manual, error-prone deployment
    - No standardized naming or documentation
    - Scaling and collaboration are difficult

---

## Evolution Phase 1: Breaking Down the Monolith

**Version 0.2.0 – Modular Architecture**

- Split resources into logical, reusable modules:
    - `modules/network` (VNet, Subnet, NSG)
    - `modules/storage` (Storage Account, Containers)
    - `modules/webapp` (App Service Plan & Web App)
    - `modules/keyvault` (Key Vault)

**Key Improvements:**

- Reusability across environments
- Maintainability (easier debugging & updates)
- Collaboration (parallel work)
- Module-level testing

**Lesson:** Start with logical module boundaries, even for small projects. Modularization saves refactoring effort as needs grow.

---

## Evolution Phase 2: Standardization and Governance

### Version 0.3.0 – Naming Conventions and Environment Separation

- Added a naming module for consistent Azure resource naming, aligned with Azure CAF abbreviations
- Introduced dedicated `dev.tfvars` & `prod.tfvars` for environments
- Used Terraform workspaces for simple state separation

**Example naming module logic:**

```hcl
locals {
  resource_type_abbreviations = { resource_group = "rg", ... }
  resource_group_name = var.resource_group != "" ? var.resource_group : "${var.prefix}-${local.resource_type_abbreviations.resource_group}-${var.environment}-${var.suffix}"
}
```

### Version 0.4.0 – Validation and Comprehensive Tagging

- **Validation module:** Ensured names meet Azure constraints (length, allowed characters, variations per resource type)
- **Tagging module:** Standardized tags (`Environment`, `Owner`, `Cost Center`, etc.), automated metadata tracking (created date, Terraform version)

**Impact:**

- Moved from ad-hoc deployments to auditable, compliant infrastructure
- Enabled automation, cost tracking, and troubleshooting

### Workspace vs Environment Separation

- **Terraform Workspaces:** Shared state, easy setup, but increased risk and limited environment isolation
- **Separate Directories (Recommended):**
    - Full state isolation
    - Siloed configs for CI/CD and security
    - Slight code duplication, but safer for enterprise

**Lesson:** Start with workspaces for simplicity, migrate to separate directories/backends as needs grow.

---

## Evolution Phase 3: Automation and CI/CD

**Version 0.5.0 – GitHub Actions Integration**

- Implemented CI/CD with environment-specific protection rules
- Branch-based strategy:
    - `develop` branch → deploys to development
    - `main` branch → deploys to production
    - Feature branches → PR validations only
- Integrated GitHub environment protection & Azure Service Principal auth
- Automated plan & apply, manual dispatch for emergencies

**Workflow Sample:**

```yaml
name: 'Terraform Deploy'
on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main, develop]
  workflow_dispatch:
    ...
jobs:
  terraform-check:
    runs-on: ubuntu-latest
    steps:
    - name: Checkout
      uses: actions/checkout@v4
    - name: Setup Terraform
      uses: hashicorp/setup-terraform@v3
      with:
        terraform_version: ${{ env.TF_VERSION }}
    ...
```

**Results:**

- Manual deployment time cut from 20+ minutes to seconds
- Reduced errors, improved approval/audit process

---

## Evolution Phase 4: Comprehensive Testing

**Version 0.6.0 – Terratest Implementation**

- Developed a test suite using Terratest:
    1. Validation tests (syntax/config)
    2. Module tests (logic in isolation)
    3. Infrastructure tests (end-to-end, takes longer)
    4. Naming convention tests
- Created Makefile targets for standardized workflows:

```makefile
make test            # Quick validation
make test-all        # Full suite
make test-modules    # Test individual modules
make test-infrastructure # Full deployment tests
```

**Example:**

```go
func TestNamingConventions(t *testing.T) {
    terraformOptions := &terraform.Options{ ... }
    terraform.InitAndApply(t, terraformOptions)
    resourceGroupName := terraform.Output(t, terraformOptions, "resource_group_name")
    assert.Contains(t, resourceGroupName, "test-rg-dev-001")
    ...
}
```

**Impact:**

- Early issue detection (e.g., Azure storage container naming compliance)
- Production risks caught before infra deployment

---

## The Advanced Features: Beyond the Basics

### Current State: Enterprise-Ready Infrastructure

- **Policy as Code:** OPA (Open Policy Agent) for security/tagging enforcement, validated via Python script or direct OPA/Checkov integration

  ```rego
  deny[msg] { ... }
  ```

- **Cost Management Foundation:** Infracost integration for cost estimation, reporting, and optimization tracking
- **Monitoring Tools:**
    - Drift detection comparing state vs. actual
    - Automated notifications & markdown reporting

### Alternative Approaches

- Use native Terraform validation (`terraform plan`, `terraform validate`)
- Checkov for security scanning as a single binary/tool
- Infracost CLI or Azure CLI for cost estimation
- Shell scripts for drift detection (`terraform plan -detailed-exitcode`)
- All tools orchestrated via GitHub Actions or Makefile

### Documentation

- Architecture Decision Records (ADR) for design decisions
- Automated Terraform diagrams & comprehensive module docs

---

## Key Lessons Learned

1. **Start simple, evolve systematically:**
   - Focus on stepwise maturity, not big-bang redesigns
   - Learn at each stage and keep infra operational
2. **Governance is not optional:**
   - Enforce naming, tags, validation from early stages
   - Enables tracking, security, and simplification
3. **Test your infrastructure code:**
   - Find and fix issues before production
   - Ensures confidence and repeatability
4. **Automate early and often:**
   - CI/CD saves time and reduces errors
   - Consistent, secure, auditable deployments
5. **Comprehensive documentation:**
   - Accelerates onboarding and teamwork
   - Preserves decisions and upgrades rationale

---

## What’s Next: The Roadmap Ahead

**Short-term Goals:**

- Automate policy validation and drift detection in CI
- Integrate Infracost cost estimation in PR workflows
- Improve monitoring and alerting

**Long-term Vision:**

- Fully automated policy enforcement
- Real-time optimization and alerts
- Self-healing infra with drift remediation
- Advanced security scanning (Checkov, tfsec)

---

## Getting Started: Your Evolution Path

1. **Phase 1:** Basic functionality, plan for modules
2. **Phase 2:** Naming conventions and basic governance
3. **Phase 3:** Validation and comprehensive tagging
4. **Phase 4:** CI/CD automation with branch controls
5. **Phase 5:** Full testing with Terratest
6. **Phase 6:** Advanced features—policies, cost monitoring, drift detection

---

## Conclusion: Evolution Over Revolution

A systematic, phased migration—from a simple script to a robust platform—enabled:

- Maintaining infrastructure during transformation
- Gradual team skill growth
- Value delivery at every step
- A future-proof foundation for continued maturity

---

**Author:** Hidde de Smet, Azure Solution Architect, specializing in cloud design and management using Scrum and DevOps methodologies.

This post appeared first on "Hidde de Smet's Blog". [Read the entire article here](https://hiddedesmet.com/terraform-evolution)
