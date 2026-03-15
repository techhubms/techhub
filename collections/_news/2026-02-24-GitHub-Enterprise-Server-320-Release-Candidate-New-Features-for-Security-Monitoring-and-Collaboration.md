---
external_url: https://github.blog/changelog/2026-02-24-github-enterprise-server-3-20-release-candidate-is-available
title: 'GitHub Enterprise Server 3.20 Release Candidate: New Features for Security, Monitoring, and Collaboration'
author: Allison
primary_section: devops
feed_name: The GitHub Blog
date: 2026-02-24 23:37:14 +00:00
tags:
- Advanced Security
- API
- Backup Service
- Code Security
- Collaboration
- Collaboration Tools
- Deployment
- DevOps
- Enterprise Teams
- GHES 3.20
- GitHub Enterprise Server
- Governance
- Immutable Releases
- News
- Pull Requests
- Release Management
- Secret Scanning
- Security
section_names:
- devops
- security
---
Allison provides an in-depth overview of the GitHub Enterprise Server 3.20 release candidate, breaking down new security, DevOps, and collaboration features for enterprise development teams.<!--excerpt_end-->

# GitHub Enterprise Server 3.20 Release Candidate: What’s New?

The GitHub Enterprise Server (GHES) 3.20 release candidate introduces significant advancements across deployment, security, monitoring, and team governance. Below are the major highlights described by Allison:

## 1. Improved Pull Request Merge Experience

- **Merge Efficiency:** The new merge experience streamlines workflows, grouping status checks by result and providing clear explanations for merge-time errors.
- **Accessibility:** Enhanced keyboard navigation and consistent focus/landmarks for improved usability.
- [Details and screenshots](https://github.blog/changelog/2025-03-04-improved-pull-request-merge-experience-is-now-generally-available/)

## 2. Immutable GitHub Releases

- **Release Immutability:** Assets are now locked post-publication, and release tags are protected from changes, helping protect against supply chain risks.
- **Compliance and Security:** This feature reduces the risk that release artifacts are altered after distribution.
- [Read the changelog](https://github.blog/changelog/2025-10-28-immutable-releases-are-now-generally-available/)

## 3. Expanded Secret Scanning & Security

- **Validity Checks & Policy:** Scanning now checks if discovered secrets are still active and allows expanded push protection.
- **Alert Assignment:** Collaborative alert assignment streamlines incident response at scale.
- **Enterprise Controls:** Delegated bypass management and expanded default secret coverage for pushes.
- **Continuous Improvement:** New and improved secret detectors.

## 4. Enterprise Governance Features

- **Enterprise Teams:** Create and manage teams across organizations for simplified governance.
- **Custom Roles:** Assign roles via API or UI at both team and user levels.
- **Preview Note:** This is a public preview feature—limitations may apply.
- [Enterprise team documentation](https://docs.github.com/enterprise-server@3.20/admin/concepts/enterprise-fundamentals/teams-in-an-enterprise#what-kind-of-team-should-i-use).

## 5. Advanced Security Policy and Alert Management

- **Enterprise Security Manager Role:** Supports centralized security policy and alert management for up to 15,000 organizations.
- **Applicability:** Currently in public preview for enterprises needing broad security oversight.

## 6. Managed Backup Service

- **General Availability:** The built-in backup service is now generally available, reducing the need for separate hosts/utilities.
- **Retirement Notice:** The standalone `backup-utils` package will be sunset starting with GHES 3.22.
- [Backup Service Documentation](https://docs.github.com/enterprise-server@3.20/admin/backing-up-and-restoring-your-instance/backup-service-for-github-enterprise-server/about-the-backup-service-for-github-enterprise-server)

## 7. How to Access and Provide Feedback

- **Release Candidates:** Early access allows users to test new features and provide feedback before general release.
- [Release Notes](https://docs.github.com/enterprise-server@3.20/admin/release-notes)
- [Download GHES 3.20 Release Candidate](https://enterprise.github.com/releases/3.20.0/download)
- [Contact Support](https://support.github.com/features/enterprise-administrators-server)

---

**For questions or feedback, visit the GitHub documentation or contact support for enterprise guidance.**

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-24-github-enterprise-server-3-20-release-candidate-is-available)
