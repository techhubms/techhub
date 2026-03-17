---
author: Allison
tags:
- Backup Service
- Backup Utils Retirement
- Code Scanning
- Collaboration Tools
- Commit Metadata Rules
- Credential Leaks
- DevOps
- Enterprise Roles
- Enterprise Security Manager
- Enterprise Teams
- GHES 3.20
- GitHub Advanced Security
- GitHub Enterprise Server
- Immutable Releases
- Merge Experience
- News
- Pull Requests
- Push Protection
- Release Assets
- Rulesets
- Secret Scanning
- Security
- Status Checks
- Supply Chain Security
- Validity Checks
title: GitHub Enterprise Server 3.20 is now generally available
external_url: https://github.blog/changelog/2026-03-17-github-enterprise-server-3-20-is-now-generally-available
date: 2026-03-17 18:17:55 +00:00
feed_name: The GitHub Blog
section_names:
- devops
- security
primary_section: devops
---

Allison summarizes what’s new in GitHub Enterprise Server (GHES) 3.20, focusing on PR merge UX improvements, stronger release and secret-scanning protections, new enterprise governance controls, and a generally available built-in backup service for enterprise deployments.<!--excerpt_end-->

# GitHub Enterprise Server 3.20 is now generally available

GitHub Enterprise Server (GHES) 3.20 adds improvements across deployment operations, monitoring, code security, and enterprise policy/governance.

## Highlights in GHES 3.20

## Improved pull request merge experience (GA)

The improved merge experience on the pull request page is now generally available and aims to make it easier to understand PR state and merge faster.

Key changes:

- **Status checks are grouped by status**, with failing checks listed first.
- Checks are **ordered using natural sorting**.
- When **commit metadata rules** fail, **merge-time errors** explain what needs fixing.
- Accessibility improvements: **consistent keyboard navigation, focus, and landmarks**.

More details (including screenshots):

- Improved pull request merge experience: https://github.blog/changelog/2025-03-04-improved-pull-request-merge-experience-is-now-generally-available/

## Immutable GitHub releases (release assets + tags)

GitHub releases now support **immutability**:

- Release assets can’t be **added, modified, or deleted** after publication.
- The **release tag** is protected from being **moved or deleted**.

This is positioned as a defense against **supply chain attacks** on distributed artifacts.

More info:

- Immutable releases changelog: https://github.blog/changelog/2025-10-28-immutable-releases-are-now-generally-available/

Limitations:

- **Release attestations aren’t supported on GHES yet**; they’re **only available on GitHub.com**.

## Secret scanning improvements

Secret scanning includes multiple updates intended to help teams prevent and respond to credential leaks at scale:

- **Validity checks** show whether detected secrets are still active.
- Enterprise admins can expose the feature to repo admins via the **Management Console**.
- **Push protection delegated bypass controls** can be managed at the **enterprise** level.
- **Alert assignment** is supported for collaboration.
- **Push protection** expands default coverage to block additional secret types.
- Additional **new detectors**, plus improvements to existing ones.

## Enterprise teams and governance (public preview)

Enterprise owners can create and manage **enterprise teams** to simplify enterprise-wide governance.

Capabilities include:

- Manage enterprise teams via **API** or **enterprise settings UI**.
- Assign enterprise teams to organizations.
- Create and assign **custom enterprise roles**.
- Assign roles to teams and users.
- Org/repo owners can assign roles to enterprise teams within their scope.
- Enterprise teams can be added to **ruleset bypass lists**.

Notes:

- There are **product limitations**.
- Feature is in **public preview** and subject to change.

Docs:

- Teams in an enterprise (GHES 3.20): https://docs.github.com/enterprise-server@3.20/admin/concepts/enterprise-fundamentals/teams-in-an-enterprise#what-kind-of-team-should-i-use

## GitHub Advanced Security: Enterprise Security Manager role (public preview)

For GitHub Advanced Security (including **code scanning** and **secret scanning**), GHES 3.20 adds an `Enterprise Security Manager` role aimed at simplifying enterprise-wide security policy and alert management.

Limitations:

- Supported only for enterprises with up to **15,000 organizations**.
- Feature is in **public preview**.

## Backup service (GA) and backup-utils retirement

The built-in **backup service** moves from public preview to **generally available** in GHES 3.20.

What’s new/important:

- Managed, built-in service intended as an alternative to GHES backup utilities.
- Does **not require a separate host** for backup software.

Docs:

- Backup service (GHES 3.20): https://docs.github.com/enterprise-server@3.20/admin/backing-up-and-restoring-your-instance/backup-service-for-github-enterprise-server/about-the-backup-service-for-github-enterprise-server

Retirement notice:

- `backup-utils` (https://github.com/github/backup-utils/) will be retired starting in **GHES 3.22**.

## Links

- GHES 3.20 release notes: https://docs.github.com/enterprise-server@3.20/admin/release-notes
- Download GHES 3.20: https://enterprise.github.com/releases/3.20.0/download
- Enterprise Server support: https://support.github.com/features/enterprise-administrators-server
- Community discussion: https://github.com/orgs/community/discussions/189803


[Read the entire article](https://github.blog/changelog/2026-03-17-github-enterprise-server-3-20-is-now-generally-available)

