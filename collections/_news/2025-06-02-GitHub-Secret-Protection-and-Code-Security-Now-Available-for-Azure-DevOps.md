---
layout: "post"
title: "GitHub Secret Protection and Code Security Now Available for Azure DevOps"
description: "Laura Jiang details the release of GitHub Secret Protection and GitHub Code Security as standalone products for Azure DevOps, outlining features, pricing, and guidance for administrators on enabling these security solutions within their organizations."
author: "Laura Jiang"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/devops/github-secret-protection-and-github-code-security-for-azure-devops/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/devops/feed/"
date: 2025-06-02 14:30:55 +00:00
permalink: "/news/2025-06-02-GitHub-Secret-Protection-and-Code-Security-Now-Available-for-Azure-DevOps.html"
categories: ["Azure", "DevOps", "Security"]
tags: ["Azure", "Azure DevOps", "Code Security", "CodeQL", "Dependency Alerts", "DevOps", "Enterprise Security", "GitHub", "GitHub Advanced Security", "News", "Pricing", "Repository Protection", "Secret Protection", "Secret Scanning", "Security", "Security Overview"]
tags_normalized: ["azure", "azure devops", "code security", "codeql", "dependency alerts", "devops", "enterprise security", "github", "github advanced security", "news", "pricing", "repository protection", "secret protection", "secret scanning", "security", "security overview"]
---

Laura Jiang introduces the new standalone GitHub Secret Protection and GitHub Code Security products for Azure DevOps, highlighting their features, pricing structure, and steps for implementation within organizations.<!--excerpt_end-->

## GitHub Secret Protection and Code Security for Azure DevOps

**Author:** Laura Jiang

Following recent changes to GitHub Advanced Security, GitHub is launching standalone security solutions—GitHub Secret Protection and GitHub Code Security—for Azure DevOps. Enterprises now gain the flexibility to choose and enable advanced security measures tailored to their repositories.

### GitHub Secret Protection for Azure DevOps

- **Pricing:** $19 per active committer per month
- **Features:**
  - **Push Protection:** Helps prevent secret leaks proactively.
  - **Secret Scanning Alerts:** Notifies organizations of existing secret exposures before they can be exploited.
  - **Security Overview:** Offers detailed insights into an organization’s risk posture and security status.

### GitHub Code Security for Azure DevOps

- **Pricing:** $30 per active committer per month
- **Features:**
  - **Dependency Alerts:** Detects vulnerabilities within open-source dependencies used by your codebase.
  - **CodeQL Scanning:** Analyzes code directly to identify potential vulnerabilities.
  - **Third-Party Tool Findings:** Integrates security findings from third-party security solutions.
  - **Security Overview:** Summarizes organizational risk and protection status in a unified dashboard.

### Getting Started

The rollout for these features will occur during the week of launch. Once enabled in your organization, Azure DevOps administrators can activate Secret Protection and Code Security through **Repositories** settings at the organization, project, or repository level. Visual guidance is available in the [repository settings documentation](https://learn.microsoft.com/azure/devops/repos/security/configure-github-advanced-security-features).

#### Cost Estimation

To estimate costs, visit the [Azure pricing page for Azure DevOps Services](https://azure.microsoft.com/en-us/pricing/details/devops/azure-devops-services/). Look for the GitHub Advanced Security section for details on standalone feature pricing.

#### Migration and Support

For current Advanced Security users, existing services remain unaffected. To transition to the new standalone Secret Protection and Code Security subscriptions, submit a support ticket via the **Azure Portal** for the GitHub Advanced Security for Azure DevOps service. Specify “Billing migration from bundled to standalone products” and list associated Azure subscriptions to migrate.

#### Additional Resources

- Learn how to [enable Secret Protection and Code Security](https://learn.microsoft.com/azure/devops/repos/security/configure-github-advanced-security-features) and see an overview of included features.
- For product feedback, contact the team directly or post on the [Developer Community](https://developercommunity.visualstudio.com/AzureDevOps).

**Reference Links:**

- [GitHub Advanced Security on GitHub](https://github.blog/changelog/2025-03-04-introducing-github-secret-protection-and-github-code-security/)
- [Azure DevOps Blog](https://devblogs.microsoft.com/devops/github-secret-protection-and-github-code-security-for-azure-devops/)

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/devops/github-secret-protection-and-github-code-security-for-azure-devops/)
