---
external_url: https://devblogs.microsoft.com/dotnet/your-migrations-source-of-truth-the-modernization-assessment/
author: Jeffrey Fritz
primary_section: github-copilot
section_names:
- ai
- azure
- devops
- dotnet
- github-copilot
- security
title: 'Your Migration’s Source of Truth: The Modernization Assessment'
date: 2026-04-07 18:00:00 +00:00
feed_name: Microsoft .NET Blog
tags:
- .NET
- .NET Framework To .NET
- Agentic Tooling
- AI
- AKS
- Application Modernization
- AppMod
- ASP.NET To ASP.NET Core
- Assess Plan Execute
- Azure
- Azure App Service
- Azure Container Apps
- Azure Landing Zone
- Bicep
- Cloud Readiness
- Containerization
- CVE Findings
- DevOps
- Dockerfile Generation
- Git Diff Review
- GitHub Copilot
- GitHub Copilot Modernization
- IaC
- ISO 5055
- Java Upgrade
- Migration Plan
- Modernization
- Modernization Assessment
- Modernize CLI
- Multi Repo Assessment
- News
- OpenJDK 11 To 21
- Security
- Spring Boot 2 To 3
- Terraform
- VS Code Extension
---

Jeffrey Fritz walks through GitHub Copilot’s application modernization assessment report, showing how it drives planning and execution for migrating .NET or Java apps to Azure, including issue triage (cloud readiness, upgrades, security), target compute comparisons (App Service/AKS/Container Apps), and downstream IaC and deployment outputs.<!--excerpt_end-->

# Your Migration’s Source of Truth: The Modernization Assessment

Jeffrey Fritz shares what he learned using **GitHub Copilot’s application modernization** capabilities (for **.NET and Java**) and argues that the **assessment document** is the foundation for the entire workflow.

The tool is positioned as more than code suggestions: it’s an **agentic Assess → Plan → Execute** experience that can:

- Analyze your codebase
- Generate an assessment report
- Build a migration plan
- Provision **Azure** infrastructure to run the modernized app

![Assess → Plan → Execute workflow diagram](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAqEAAACYAQMAAADuhqJRAAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAJElEQVRoge3BAQ0AAADCoPdPbQ43oAAAAAAAAAAAAAAAAODHADMQAAEKql8bAAAAAElFTkSuQmCC)

At each phase, the tooling generates documents with findings and recommendations, and you can provide feedback/corrections so Copilot can adjust in later phases.

## Tooling Surfaces

The workflow ships as:

- A **VS Code extension** (generally available for both .NET and Java) to run assessments, migrate dependencies, containerize apps, and deploy to Azure.
- A **Modernization CLI** (public preview) for terminal workflows and **multi-repo batch scenarios**.

For this post, Fritz uses **VS Code** on a single project.

## Why the Assessment Document Matters

Fritz describes the assessment report as the highest-leverage artifact because it determines what happens downstream:

- What gets upgraded
- What Azure resources get provisioned
- How the app is deployed

He notes that everything downstream (IaC, containerization, deployment manifests) takes its cues from the assessment findings.

## Two Paths In: Recommended vs Custom Assessment

The VS Code extension offers two ways to kick off an assessment.

## Path 1: Recommended Assessment (Fast Start)

A “quick read” path with no manual configuration.

![VS Code modernization pane - Recommended Assessment option](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAABJYAAALGAQMAAAAtB4n9AAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAfUlEQVR4nO3BMQEAAADCoPVPbQlPoAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgJcBmocAAbjNCH8AAAAASUVORK5CYII=)

Steps Fritz lists:

1. Open the **GitHub Copilot modernization** pane in VS Code.
2. Select **Start Assessment** (or **Open Assessment Dashboard**) from Quickstart.
3. Choose **Recommended Assessment**.
4. Pick one or more domains from the curated list:
   - **Java/.NET Upgrade**
   - **Cloud Readiness**
   - **Security**

Results show up in an interactive dashboard, with preconfigured settings per domain.

## Path 2: Custom Assessment (Targeted)

For cases where you know your target (example given: **AKS on Linux with containerization**), custom assessment allows explicit configuration.

You select **Custom Assessment** and configure:

![Custom Assessment configuration dialog](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAABJMAAALHAQMAAAAAcpEcAAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAfElEQVR4nO3BAQ0AAADCoPdPbQ8HFAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA8GybGwAB51URAwAAAABJRU5ErkJggg==)

| Setting | Options |
| --- | --- |
| **Assessment Domains** | Java/.NET Upgrade, Cloud Readiness, Security (pick one or combine all three) |
| **Analysis Coverage** | *Issue only* (just problems), *Issues & Technologies* (problems + inventory), *Issues, Technologies & Dependencies* (full picture) |
| **Target Compute** | [Azure App Service](https://learn.microsoft.com/azure/app-service/overview), [Azure Kubernetes Service (AKS)](https://learn.microsoft.com/azure/aks/what-is-aks), [Azure Container Apps (ACA)](https://learn.microsoft.com/azure/container-apps/overview) (pick multiple to compare) |
| **Target OS** | Linux or Windows |
| **Containerization** | Enable/disable containerization analysis |

When multiple Azure targets are selected, the dashboard supports switching targets to compare recommendations and blockers.

## Upgrade Paths the Assessment Covers

The assessment can also analyze framework/runtime upgrade paths and provide issue detection + remediation guidance.

- **Java:** OpenJDK 11 → 17 → 21, Spring Boot 2.x → 3.x
- **.NET:** .NET Framework → .NET 10, ASP.NET → ASP.NET Core
  - See the [.NET porting guide](https://learn.microsoft.com/dotnet/core/porting/)
  - See [ASP.NET Core migration guidance](https://learn.microsoft.com/aspnet/core/migration/proper-to-2x/)

Fritz highlights that each upgrade path has its own detection rules (e.g., API removals between JDK versions, or ASP.NET patterns without direct ASP.NET Core equivalents).

> **CLI note:** For many apps across multiple repos, the Modernize CLI supports `modernize assess --multi-repo` using a `repos.json` manifest to clone repos and generate per-app plus aggregated reports.

## The Assessment Document

Fritz’s focus: understand where the report lives, how it’s structured, and how to act on it.

## Where It Lives

Assessment reports are stored under:

```bash
.github/modernize/assessment/
```

Each run generates an **independent report** (history is preserved; earlier reports aren’t overwritten).

## Report Structure

The report has a header plus four analytical tabs.

![Top of the assessment report](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAABOMAAAKUAQMAAAB8IW7dAAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAfElEQVR4nO3BMQEAAADCoPVPbQZ/oAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAPgOXZwABrXoY+wAAAABJRU5ErkJggg==)

### Application Information (Snapshot)

The top of the report captures the current app state:

- **Runtime version detected** (Java or .NET)
- **Frameworks** (e.g., Spring Boot, ASP.NET MVC, WCF)
- **Build tools** (e.g., Maven, Gradle, MSBuild)
- **Project structure** (modules/solution structure)
- **Target Azure service** (the compute target(s) selected)

### Issue Summary (Dashboard)

A high-level readiness view with issues categorized by domain and shown with criticality percentages.

![Issue Summary dashboard showing criticality breakdown by domain](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAABOIAAAMdAQMAAAAMK0tHAAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAkUlEQVR4nO3BgQAAAADDoPlTX+AIVQEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAPAPr9QAB70+oTQAAAABJRU5ErkJggg==)

If multiple Azure compute targets are configured, you can switch between them and compare how many mandatory issues each target has.

### Issues Tab (Actionable Detail)

Issues are organized by domain:

- **Cloud Readiness**: Azure dependencies, migration blockers, platform incompatibilities
- **Java/.NET Upgrade**: JDK/framework issues, deprecated APIs, removed features
- **Security**: CVE findings, ISO 5055 compliance violations

Each issue has a criticality level:

| Level | Meaning |
| --- | --- |
| 🔴 **Mandatory** | Must fix or migration fails (hard blockers) |
| 🟡 **Potential** | Might matter depending on deployment scenario (needs judgment) |
| 🟢 **Optional** | Recommended improvements, not blockers |

When you expand an issue, the report includes:

- **Affected files and line numbers** (clickable navigation)
- **Detailed description** (what/why)
- **Known solutions** (concrete remediation steps)
- **Supporting documentation links** (migration guides, API docs, security advisories)

![Expanded issue showing affected files, description, and remediation steps](data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAABNwAAAJKAQMAAADN7co3AAAAA1BMVEXW1taWrGEgAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAcElEQVR4nO3BMQEAAADCoPVPbQ0PoAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAXg1ncQABuEdtDgAAAABJRU5ErkJggg==)

### Report Operations (Share, Import, Compare)

The tooling supports collaboration workflows:

- **Export and share**: teammates can import and see the same dashboard without re-running the assessment.
- **Import reports** from:
  - [AppCAT CLI](https://learn.microsoft.com/azure/migrate/appcat/dotnet) (`report.json`)
  - Dr. Migrate (app context files)
  - Previously exported reports
- **Import UX**:
  - Select **Import** on the assessment reports page, or
  - Use `Ctrl+Shift+P` → **Import Assessment Report**
- **Compare assessments**: run multiple assessments (e.g., AKS vs Container Apps) and compare issue counts/blockers.
- **Track history**: independent reports create a timeline as you fix issues and re-run.

### Key Insight

The assessment isn’t a one-off report; it’s the input to downstream automation:

- Infrastructure planning (what Azure resources are needed)
- IaC generation (Bicep/Terraform)
- Containerization decisions
- Deployment target selection (based on cloud readiness findings per compute option)

## From Assessment to Azure: Planning and Execution in VS Code

The assessment tells you where you stand; planning/execution connect that to deployment.

## Phase 1: Infrastructure Preparation

From the Copilot Chat pane, use the `modernize-azure-dotnet` agent to create a migration plan. Inputs can include:

- Application source code
- Assessment reports (the primary bridge)
- Architecture diagrams
- Compliance/security requirements (docs or natural language)

Example prompts:

> “Create Azure infrastructure based on the assessment report, following our compliance policies in docs/security-requirements.md”

> “Create an Azure landing zone tailored to my application’s architecture and requirements”

Outputs:

| File | Location | Purpose |
| --- | --- | --- |
| **Plan file** | `.github/modernize/{plan-name}/plan.md` | Strategy, proposed architecture, resource list |
| **Task list** | `.github/modernize/{plan-name}/tasks.json` | Tasks the agent will perform during execution |

The infrastructure plan can cover **Azure Landing Zone** foundations (networking, identity, governance, security) and generate IaC as **[Bicep](https://learn.microsoft.com/azure/azure-resource-manager/bicep/overview)** or **[Terraform](https://learn.microsoft.com/azure/developer/terraform/overview)**.

Fritz emphasizes review and control:

- Both files are editable in VS Code
- Execute from the modernization pane
- Review generated changes via Git diff

> **CLI note:** The same planning workflow is available via `modernize plan create` and `modernize plan execute`.

## Phase 2: Containerization and Deployment

A separate plan covers containerization and deployment.

Example prompt:

> “Containerize and deploy my app to Azure, subscription: <sub-id>, resource group: <rg-name>”

This phase can generate:

- Dockerfile tailored to the app
- Container image build validation
- Deployment manifests for the chosen Azure target (AKS / Container Apps / App Service)
- Reusable deployment scripts

You can also split it up (containerize-only, or deploy an already-containerized app).

## Independence and Human Control

Two design principles Fritz highlights:

- **Phases are independent** (prepare infra now, deploy later; or skip infra if it already exists)
- **Human-in-the-loop** with editable plans and Git-tracked changes (review diffs, revert, audit)

## Summary

The post frames the assessment report as the “source of truth” for Copilot modernization:

- It drives the migration plan and downstream automation
- It supports comparison across Azure compute targets
- It enables collaboration via export/import
- It connects directly to IaC, containerization, and deployment workflows inside VS Code

## Get Started

- GitHub Copilot application modernization overview: https://learn.microsoft.com/azure/developer/github-copilot-app-modernization/overview
- Install the VS Code extension: https://aka.ms/ghcp-appmod/vscode-ext

Fritz closes by asking readers about their experience using Copilot modernization tools to migrate .NET/Java apps to Azure.

[Read the entire article](https://devblogs.microsoft.com/dotnet/your-migrations-source-of-truth-the-modernization-assessment/)

