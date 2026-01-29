---
external_url: https://github.blog/ai-and-ml/github-copilot/a-step-by-step-guide-to-modernizing-java-projects-with-github-copilot-agent-mode/
title: 'Modernizing Java Projects with GitHub Copilot Agent Mode: Step-by-Step Guide'
author: Andrea Griffiths
feed_name: The GitHub Blog
date: 2025-09-23 13:00:00 +00:00
tags:
- Agent Mode
- AI & ML
- AI Assisted Development
- AKS
- App Modernization
- Application Insights
- Automated Migration
- Cloud Deployment
- Continuous Integration
- CVE Scanning
- Dependency Upgrades
- Gradle
- Java
- Log Analytics
- Managed Identity
- Maven
- Microsoft Entra ID
- OpenRewrite
- Spring Security
- VS Code
- AI
- Azure
- DevOps
- GitHub Copilot
- Security
- News
section_names:
- ai
- azure
- devops
- github-copilot
- security
primary_section: github-copilot
---
Andrea Griffiths details how to modernize legacy Java projects by leveraging GitHub Copilot Agent Mode, automated upgrades, secure migrations, and direct Azure cloud deployments.<!--excerpt_end-->

# Modernizing Java Projects with GitHub Copilot Agent Mode: Step-by-Step Guide

Andrea Griffiths walks you through an actionable process for bringing legacy Java applications up to date, leveraging AI-driven tools from GitHub and Microsoft to automate upgrades, resolve security issues, and migrate to the cloud with confidence.

## Why Modernize Java Projects?

Migrating legacy Java projects to the cloud or updating runtimes can be difficult due to outdated dependencies, deprecated APIs, and lingering security vulnerabilities. Automation helps to cut manual effort and reduce errors.

## Tooling Required

- **Visual Studio Code**
- **GitHub Copilot license** (Pro or above, with access to Copilot coding agent)
- **GitHub Copilot App Modernization – Upgrade for Java extension**
- A Git-based legacy Java project using Maven or Gradle (JDK 8+)

## Process Overview

1. **Analyze Your Java Codebase**
   - Start an agent session in VS Code with the Java upgrade extension. GitHub Copilot scans your code, looks for outdated/unsafe dependencies, build tool config (Gradle/Maven), deprecated APIs, and proposes a customizable upgrade plan.
2. **Apply Automated Upgrades**
   - Approve the upgrade plan. Copilot uses tools like OpenRewrite to update code, automatically fixing build issues. It iterates until all tests pass and produces a change summary (updated dependencies, APIs, commit history, to-do list).
3. **Security Hardening & CVE Scan**
   - The CVE scanner highlights dependency security issues and suggests updated, secure replacements.
4. **Cloud-Readiness Assessment & Azure Migration**
   - Launch an application assessment for Azure compatibility. Modify the deployment target (e.g., Azure Kubernetes Service) in `assessment-config.yaml`. Issues such as on-premise authentication are flagged, with actionable fixes such as migrating to Microsoft Entra ID (formerly Azure AD).
5. **Migration to Microsoft Entra ID**
   - Copilot creates a migration plan: updates dependencies, adds configuration in `application.properties`, sets up Spring Security integration for Azure, and documents changes.
6. **Validate and Deploy**
   - Run your test suite with Maven or Gradle. If migration/deployment tasks are assigned, Copilot can help provision Azure infrastructure, enable monitoring (Application Insights, Log Analytics), and use managed identities for secure deployments.

## Sample Code Change (Before/After Upgrade)

```java
// Before (deprecated constructor)
View view = this.resolver.resolveViewName("intro", new Locale("EN"));

// After Java 21 upgrade
View view = this.resolver.resolveViewName("intro", Locale.of("EN"));
```

## Azure Integration Highlights

- **Choose deployment target:** AKS, Azure App Service, Container Apps, etc.
- **Automatic infrastructure provisioning**
- **End-to-end visibility:** Deployment records, auto-scaling, monitoring, and enhanced test coverage.

## Security: Automated CVE Scanning

- Every dependency update or migration triggers a vulnerability scan.
- The Copilot agent suggests corrections or secure alternatives as needed.

## Final Steps and Author Credits

With Copilot Agent Mode and these extensions, you can:

- Analyze
- Plan
- Upgrade
- Remediate
- Migrate
- Secure
- Deploy

—all in a streamlined workflow for both Java and .NET projects. Andrea Griffiths also credits collaborators Sandra Ahlgrimm and Nick Zhu for their technical support.

## Additional Resources

- [Upgrade Java projects using GitHub Copilot App Modernization](https://marketplace.visualstudio.com/items?itemName=vscjava.vscode-java-upgrade)
- [Documentation on .NET app upgrades](https://marketplace.visualstudio.com/items?itemName=ms-appmod.dotnet-modernization)
- [GitHub Copilot appmod landing](https://aka.ms/ghcp-appmod/Java)
- [Agent Mode in GitHub Copilot explained](https://github.blog/developer-skills/github/less-todo-more-done-the-difference-between-coding-agent-and-agent-mode-in-github-copilot/)

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/github-copilot/a-step-by-step-guide-to-modernizing-java-projects-with-github-copilot-agent-mode/)
