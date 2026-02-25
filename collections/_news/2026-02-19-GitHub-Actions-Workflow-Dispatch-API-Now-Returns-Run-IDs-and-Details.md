---
layout: "post"
title: "GitHub Actions Workflow Dispatch API Now Returns Run IDs and Details"
description: "This news update highlights an improvement to the GitHub Actions workflow dispatch API. Developers can now receive metadata, including run IDs and workflow URLs, by using a new optional parameter. This enhancement allows better mapping between API requests and workflow runs, minimizing the need for polling or custom tracking."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2026-02-19-workflow-dispatch-api-now-returns-run-ids"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2026-02-19 22:39:37 +00:00
permalink: "/2026-02-19-GitHub-Actions-Workflow-Dispatch-API-Now-Returns-Run-IDs-and-Details.html"
categories: ["DevOps"]
tags: ["Actions", "API", "API Endpoint", "API Improvement", "Automation", "CI/CD", "Client Apps", "DevOps", "Gh Workflow Run", "GitHub Actions", "GitHub CLI", "Improvement", "News", "Release Notes", "Run ID", "Workflow Automation", "Workflow Dispatch", "Workflow Metadata"]
tags_normalized: ["actions", "api", "api endpoint", "api improvement", "automation", "cislashcd", "client apps", "devops", "gh workflow run", "github actions", "github cli", "improvement", "news", "release notes", "run id", "workflow automation", "workflow dispatch", "workflow metadata"]
---

Allison provides an update on a new feature for GitHub Actions: the workflow dispatch API now returns run IDs and details, streamlining workflow tracking for developers and DevOps teams.<!--excerpt_end-->

# GitHub Actions Workflow Dispatch API Now Returns Run IDs and Details

GitHub has introduced an important enhancement to the [GitHub Actions workflow dispatch API endpoint](https://docs.github.com/rest/actions/workflows#create-a-workflow-dispatch-event). When you trigger a workflow using this endpoint, you now have the option to receive detailed metadata in the response, helping you easily map each API request to the corresponding workflow run.

**What's New:**

- **Previous behavior:** The endpoint returned only a `204 No Content` status.
- **New behavior:** By passing a new optional boolean parameter, `return_run_details`, the API returns a `200 OK` response that includes the workflow run ID, API URL, and workflow URL.
- If the parameter is not included, the default `204 No Content` response remains.

**GitHub CLI Support:**

- As of [GitHub CLI v2.87.0](https://github.com/cli/cli/releases/tag/v2.87.0), running `gh workflow run` will now return the URL of the created run and the command to view that run (`gh run view`).
- The CLI defaults `return_run_details` to `true`, streamlining access to workflow metadata.

**Why This Matters:**

- Developers can quickly and reliably track which workflow runs originated from their API requests.
- This eliminates the need for additional polling or building custom mapping solutions.

**Further Reading:**

- Explore full details in the [GitHub Actions documentation](https://docs.github.com/actions/using-workflows/events-that-trigger-workflows#workflow_dispatch).

---

*Posted by Allison on The GitHub Blog.*

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2026-02-19-workflow-dispatch-api-now-returns-run-ids)
