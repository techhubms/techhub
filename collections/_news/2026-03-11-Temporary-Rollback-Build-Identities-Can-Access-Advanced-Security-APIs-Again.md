---
layout: "post"
title: "Temporary Rollback: Build Identities Can Access Advanced Security APIs Again"
description: "This update from Laura Jiang covers the temporary rollback of API access restrictions for build identities in Azure DevOps Advanced Security. It explains the changes introduced in Sprint 269, why the decision was made, and what steps customers should take before the restrictions are re-enforced in April 2026, including migrating to service principals."
author: "Laura Jiang"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/devops/temporary-rollback-build-identities-can-access-advanced-security-read-alerts-again/"
viewing_mode: "external"
feed_name: "Microsoft DevOps Blog"
feed_url: "https://devblogs.microsoft.com/devops/feed/"
date: 2026-03-11 23:46:28 +00:00
permalink: "/2026-03-11-Temporary-Rollback-Build-Identities-Can-Access-Advanced-Security-APIs-Again.html"
categories: ["DevOps", "Security"]
tags: ["Advanced Security", "API Access", "Automation", "Azure DevOps", "Build Service Identities", "CI/CD", "DevOps", "License Management", "News", "Pipeline Identities", "Security", "Security Alerts", "Security Permission", "Service Principal", "Sprint 269", "Status Checks"]
tags_normalized: ["advanced security", "api access", "automation", "azure devops", "build service identities", "cislashcd", "devops", "license management", "news", "pipeline identities", "security", "security alerts", "security permission", "service principal", "sprint 269", "status checks"]
---

Laura Jiang details a temporary rollback in Azure DevOps allowing build identities to access Advanced Security APIs again. The post outlines the required actions for teams before the restrictions return in April 2026.<!--excerpt_end-->

# Temporary Rollback: Build Identities Can Access Advanced Security APIs Again

**Author:** Laura Jiang

If you use build service identities like `Project Collection Build Service` to call Advanced Security APIs, you may have encountered issues due to permission changes introduced in [Sprint 269](https://learn.microsoft.com/en-us/azure/devops/release-notes/2026/sprint-269-update#build-identity-access-restricted-for-advanced-security-apis). These restrictions were implemented to improve security, but unfortunately, early notice was not provided to customers relying on these identities for automation.

**Current Status:**

- The change has been **temporarily rolled back**. Build identities can again access Advanced Security APIs, but this is only until **April 15, 2026**. After that date, restrictions will be reinstated.

## What You Should Do

- **Action Required:**
  - **Migrate automations** to use a service principal with the "Advanced Security: Read alerts" permission.
  - Scope service principals narrowly for security.
  - Service principals not committing code will not consume an Advanced Security committer license.

## Upcoming Feature: Status Checks in Sprint 272

- New **status checks** will soon be introduced, allowing teams to gate on security posture natively during pull requests, without requiring API-driven alert mutations.
- Status checks support blocking merges if high or critical alerts are present.
- Note: Status checks may not yet cover every automation scenario.

## Next Steps & Feedback

- Take required actions by **April 15, 2026** to avoid workflow interruptions.
- For gaps in moving to a service principal or other feedback, [contact the team](https://aka.ms/ghazdo-feedback).

---

**References:**

- [Azure DevOps Blog Post](https://devblogs.microsoft.com/devops/temporary-rollback-build-identities-can-access-advanced-security-read-alerts-again/)
- [Sprint 269 Release Notes](https://learn.microsoft.com/en-us/azure/devops/release-notes/2026/sprint-269-update#build-identity-access-restricted-for-advanced-security-apis)

**Illustration:** [Status Checks Example](https://devblogs.microsoft.com/devops/wp-content/uploads/sites/6/2026/03/ado-status-checks.webp)

This post appeared first on "Microsoft DevOps Blog". [Read the entire article here](https://devblogs.microsoft.com/devops/temporary-rollback-build-identities-can-access-advanced-security-read-alerts-again/)
