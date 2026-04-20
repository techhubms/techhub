---
section_names:
- devops
- security
external_url: https://github.blog/changelog/2026-04-14-sbom-exports-are-now-computed-asynchronously
author: Allison
date: 2026-04-14 16:00:14 +00:00
tags:
- API Endpoints
- Asynchronous Operations
- Concurrency Limits
- Dependency Graph
- DevOps
- GitHub
- HTTP 201
- HTTP 302 Redirect
- Improvement
- Job Polling
- News
- Repository Insights
- REST API
- SBOM
- Security
- Software Bill Of Materials
- Supply Chain Security
- Timeouts
feed_name: The GitHub Blog
title: SBOM exports are now computed asynchronously
primary_section: devops
---

Allison explains how GitHub’s SBOM export flow moved to an asynchronous model in the Dependency Graph UI and REST API, removing hard timeouts and adding a generate/fetch pattern for reliably downloading SBOM reports from large repositories.<!--excerpt_end-->

# SBOM exports are now computed asynchronously

Software Bill of Materials (SBOM) exports from GitHub repository pages and new REST API endpoints are now **asynchronous operations**.

Previously, exporting an SBOM via either of these paths had a **hard-coded timeout of 10 seconds**:

- Repository **Dependency graph** page → **Export SBOM** button
- REST API: `GET /repos/{owner}/{repo}/dependency-graph/sbom`

This worked for most repositories, but **large repos with complex dependency trees** could take longer to process. Also, **multiple requests** could spawn **multiple independent back-end workers**, with no guarantee any of them would complete.

## Download SBOMs in your browser

GitHub now provides a new web experience that **polls for job completion** (and matching async API behavior) to eliminate timeouts.

Steps:

1. Go to a repository’s **Insights** tab
2. Click **Dependency Graph**
3. Click **Export SBOM**
4. When the file is ready, download it from that page

## API access to SBOMs

Two new endpoints work together to provide asynchronous access:

- `GET /repos/{owner}/{repo}/dependency-graph/sbom/generate-report`
  - Returns a URL that includes a unique identifier: `{sbom-uuid}`
  - This indicates the service has started generating the SBOM

- `GET /repos/{owner}/{repo}/dependency-graph/sbom/fetch-report/{sbom-uuid}`
  - Returns **201** until the report is ready
  - Once ready, it returns a **302 redirect** to the actual SBOM contents

## Limitations and caveats

- The SBOM export represents the repository state **at the time you initiate the request**.
- SBOMs are **not available for refs other than `HEAD`**.
- **Anonymous users** are limited to **one concurrent SBOM request per repository**.
- **Logged-in users** are not subject to that concurrency restriction.

Join the discussion in GitHub Community:

- [GitHub Community – Announcements](https://github.com/orgs/community/discussions/categories/announcements)

![GitHub Community announcement social image](https://github.com/user-attachments/assets/39e105e0-5405-4363-a8bf-9b72a3772279)


[Read the entire article](https://github.blog/changelog/2026-04-14-sbom-exports-are-now-computed-asynchronously)

