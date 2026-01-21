---
external_url: https://github.blog/changelog/2025-11-18-unified-code-to-cloud-artifact-risk-visibility-with-microsoft-defender-for-cloud-now-in-public-preview
title: Unified Code-to-Cloud Artifact Risk Visibility with Microsoft Defender for Cloud in GitHub
author: Allison
feed_name: The GitHub Blog
date: 2025-11-18 17:30:32 +00:00
tags:
- Application Security
- AppSec
- Artifact Metadata API
- Azure Container Registry
- CI/CD
- Code Scanning
- Dependabot
- DevSecOps
- GitHub Advanced Security
- Linked Artifacts
- Microsoft Defender For Cloud
- Production Context
- Runtime Risk
- Security Campaigns
- Supply Chain Security
section_names:
- azure
- devops
- security
---
Allison shares how new GitHub features, integrated with Microsoft Defender for Cloud, deliver end-to-end artifact provenance, runtime risk awareness, and targeted security remediation for code-to-cloud workflows.<!--excerpt_end-->

# Unified Code-to-Cloud Artifact Risk Visibility with Microsoft Defender for Cloud in GitHub

**Author:** Allison

## Overview

GitHub has launched powerful new features to give security and development teams deep, production-aware risk visibility for software artifacts. By connecting code, build artifacts, and runtime environments—with integration from Microsoft Defender for Cloud—teams can now trace, monitor, and remediate vulnerabilities that impact real deployments.

---

## Linked Artifacts and Full Traceability

- **Artifacts** built using GitHub Actions are now visible as "linked artifacts," even if they're stored externally (e.g., Azure Container Registry, JFrog Artifactory).
- GitHub displays metadata for each linked artifact:
  - Which repository created the artifact
  - The specific package registry storing it
  - Deployment destinations
- Artifacts are automatically mapped to their source code repositories, supporting end-to-end provenance.
- The new [Artifact Metadata APIs](https://docs.github.com/rest/orgs/artifact-metadata?apiVersion=2022-11-28) enable CI/CD tools to programmatically update storage and deployment records, keeping views accurate and current.

---

## Runtime Risk Context: Integration with Microsoft Defender for Cloud

- **Microsoft Defender for Cloud** enriches GitHub with runtime risk details for deployed artifacts, identifying if they:
  - Are internet-exposed
  - Handle sensitive data
- Defender for Cloud provides a native integration using the Deployment Record API, automatically syncing risk data without workflow changes.
- This gives both security and development teams shared visibility into the production risk profile of every artifact.

---

## Production-Aware Security Campaigns

- Teams can now filter and prioritize alerts in GitHub Advanced Security based on production context:
  - **Registry filters** with `artifact-registry-url:`
  - **Deployment status** using `has:deployment`
  - **Runtime risk** using `runtime-risk:` (e.g., internet-exposed or sensitive-data)
- Targeted remediation campaigns can be launched, focusing on artifacts that impact actual production systems.
- Issues can be assigned directly to Copilot coding agent, which generates draft pull requests with proposed fixes and explanations, reducing manual triage.

![Campaign creation flow with new production context filters](https://github.com/user-attachments/assets/090c0ba9-385c-4514-bbd1-5c45ae681950)

---

## Getting Started

- Update build and CI workflows to send artifact metadata using the [Artifact Metadata API](https://docs.github.com/rest/orgs/artifact-metadata?apiVersion=2022-11-28), unless you’re using Microsoft Defender for Cloud (which syncs automatically).
- Use new production context filters in GitHub Advanced Security views to focus remediation on deployed and exposed artifacts.
- Assign critical issues to Copilot coding agent for automated fix proposals.

---

## Collaboration and Visibility

These integrations bridge the gap between development, AppSec, and cloud security teams, making risk management more actionable and aligned with real deployments. For community support, open a topic in [GitHub Community discussions](https://github.com/orgs/community/discussions).

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-11-18-unified-code-to-cloud-artifact-risk-visibility-with-microsoft-defender-for-cloud-now-in-public-preview)
