---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/from-quot-maybe-next-quarter-quot-to-quot-running-before-lunch/ba-p/4495736
title: From "Maybe Next Quarter" to "Running Before Lunch" on Container Apps - Modernizing Legacy .NET App
author: Jan-Kalis
primary_section: github-copilot
feed_name: Microsoft Tech Community
date: 2026-02-18 23:19:26 +00:00
tags:
- .NET 10
- Active Directory Authentication
- AI
- App Modernization
- ASP.NET Core
- AZD
- Azure
- Azure Container Apps
- Azure SQL
- Bicep
- CI/CD
- Cloud Deployment
- Community
- Development Automation
- DevOps
- Dockerfile
- EF Core
- GitHub Copilot
- GitHub Copilot App Modernization
- IaC
- Key Vault
- Managed Identity
- OpenTelemetry
- Program.cs
- Security
- VS
- .NET
section_names:
- ai
- azure
- dotnet
- devops
- github-copilot
- security
---
Jan-Kalis explains how GitHub Copilot's app modernization agent enabled rapid migration of a classic ASP.NET MVC app to Azure Container Apps with improved security and process automation.<!--excerpt_end-->

# From "Maybe Next Quarter" to "Running Before Lunch" on Container Apps - Modernizing Legacy .NET App

Jan-Kalis shares the journey of modernizing the well-known MVC Music Store application from ASP.NET MVC 5 to a modern, secure .NET 10 application running in Azure. This post illustrates both the technical depth and the practical acceleration offered by GitHub Copilot's app modernization tools.

## Background

The MVC Music Store started as a .NET Framework 4.8 app using Entity Framework 6. The primary modernization goals were:

- Eliminate vulnerabilities and cleartext credentials
- Enable managed identity and passwordless authentication
- Containerize and deploy with automated Azure infrastructure

## The Initial Roadblock

Legacy technology constraints emerged immediately, such as:

- Entity Framework on .NET Framework didn't support Azure.Identity or DefaultAzureCredential
- Modernizing required comprehensive codebase upgrades: data layer, identity, startup pipeline, and views
- Manual rewriting was estimated at a week of developer time

## Transition from Assessment to End-to-End Automation

Prior to agent-assisted modernization, GitHub Copilot could assess modernization steps but not automate them. The new Copilot "app modernization" agent moved from assessment to fully automating migrations, drastically compressing timelines.

## The Modernization Process

### Phase 1: Assessment

- Analysis of code for framework gaps, dependencies (EF6, MVC 5), security issues (plain connection strings), and API shifts
- Mapping requirement to move to EF Core and ASP.NET Core for secure authentication

### Phase 2: Code & Dependency Updates

**Project structure:**

- Migrated to SDK-style csproj targeting net10.0
- Replaced Global.asax with Program.cs (minimal hosting)
- Switched to NuGet PackageReference

**Data layer:**

- EF6 to EF Core conversion using Microsoft.EntityFrameworkCore.SqlServer
- Migration of DbContext and seeding pattern to MigrateAsync()

**Identity:**

- ASP.NET Membership replaced with ASP.NET Core Identity
- Cookie authentication streamlined

**Security:**

- Azure.Identity and DefaultAzureCredential in Program.cs
- Azure Key Vault integration via Azure.Extensions.AspNetCore.Configuration.Secrets
- No passwords in any configuration — only managed identity

**Views:**

- All Razor views adapted to ASP.NET Core conventions and tag helpers

### Phase 3: Local Testing

- Application builds and runs locally or in dev containers
- Full functional and security parity with, or improvements over, the original
- Key Vault gracefully bypasses if KeyVaultName is unset

### Phase 4: Infrastructure and Deployment (AZD UP)

- auto-generated azure.yaml points to Dockerfile for Azure Container Apps
- Bicep templates provide end-to-end IaaC: managed identities, Azure SQL (with AAD-only authentication), Key Vault integration, Application Insights, Log Analytics, Container Registry, secrets injection
- One command: `azd up` provisions resources, deploys containers, seeds the database, and secures all secrets

## Drastic Time & Capability Improvements

|               | Early 2025         | Now                       |
|---------------|--------------------|---------------------------|
| Assessment    | Available          | Available                 |
| Automation    | Semi-manual        | ✅ Fully automated agent   |
| Infra as Code | Semi-manual        | ✅ Bicep & AZD generated   |
| Time Required | Weeks              | ✅ Hours                   |

## Who Can Benefit

Organizations with legacy .NET Framework applications can now:

- Realistically scope migrations that previously seemed infeasible
- Trust that Copilot app modernization covers code, infra, testing, and more
- Remove most manual credentials in favor of managed identities and Key Vault

## Key Takeaways

- Modernization is now a developer-scale activity — not just for specialized teams
- Security, maintainability, and automation are drastically improved
- The backlog of "impossible" migrations may only be a lunch break away

## Next Steps

1. Start app modernization for .NET or Java with [GitHub Copilot app modernization](https://aka.ms/ghcp-appmod)
2. Open your application in Visual Studio or VS Code
3. Deploy to Azure Container Apps: [https://aka.ms/aca/start](https://aka.ms/aca/start)

---

**Author:** Jan-Kalis

*Original inspiration: transforming the MVC Music Store for the cloud-native era using secure, automated, developer-friendly workflows.*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/from-quot-maybe-next-quarter-quot-to-quot-running-before-lunch/ba-p/4495736)
