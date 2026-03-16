---
primary_section: devops
author: Kedasha Kerr
date: 2026-03-16 16:00:00 +00:00
section_names:
- devops
external_url: https://github.blog/developer-skills/github/github-for-beginners-getting-started-with-github-actions/
title: 'GitHub for Beginners: Getting started with GitHub Actions'
feed_name: The GitHub Blog
tags:
- Actions/checkout
- Automation
- CI/CD
- Developer Skills
- DevOps
- Events
- GitHub
- GitHub Actions
- GitHub CLI
- GitHub For Beginners
- GitHub Marketplace
- GitHub Skills
- GitHub TOKEN
- Hosted Runners
- Issues
- Jobs
- Labels
- News
- Permissions
- Secrets
- Self Hosted Runners
- Steps
- Trigger Events
- Ubuntu Latest
- Workflow
- YAML
---

Kedasha Kerr walks beginners through GitHub Actions by explaining core workflow concepts (events, runners, jobs, steps) and building a simple CI-style automation that labels newly opened issues using a YAML workflow and the GitHub CLI.<!--excerpt_end-->

# GitHub for Beginners: Getting started with GitHub Actions

Welcome back to the **GitHub for Beginners** series (season 3). This episode focuses on **GitHub Actions** and walks you through creating your first automated workflow.

If you prefer video, the GitHub for Beginners episodes are available on YouTube: https://gh.io/gfb

## What are GitHub Actions?

**GitHub Actions** is a CI/CD and automation platform built into GitHub. You define automation in **YAML workflow files** stored in your repository.

Common uses include:

- Running vulnerability scans
- Running tests
- Creating releases
- Reminding a team about updates

Workflows are triggered by GitHub events (pushes, pull requests, schedules, etc.) and run in a virtual environment.

## How workflows work

Key terms:

- **Event**: The activity that triggers a workflow (push, pull request, issue opened, etc.)
- **Hosted runners**: Virtual machines that run workflow jobs (GitHub-hosted, or you can use self-hosted runners)
- **Jobs**: Groups of steps that run on the same runner
  - Each step is either a shell command or a prebuilt action from GitHub Marketplace: https://github.com/marketplace

High-level flow:

1. An event happens in the repo.
2. GitHub triggers the workflow.
3. GitHub provisions one or more runners.
4. Jobs execute step-by-step until completion.

For extra practice, the post points to the “Hello GitHub Actions” skill exercise:

- https://github.com/skills/hello-github-actions

## Looking at a repository’s Actions

1. Navigate to the sample repository.
2. Fork it.
3. Select the **Actions** tab.
4. Select **New workflow**.
5. Choose any suggested workflow and click **Configure**.

A workflow template in YAML includes three main sections:

- **Name**: What the workflow is called
- **On**: What triggers the workflow
- **Jobs**: The work that runs when triggered

## Building a workflow: automatically label new issues

You’ll build a workflow that labels new issues when they’re opened.

1. Clone the sample repository locally.
2. Switch to the `action-start` branch.
3. Go to the `.github/workflows` directory in the repo.
4. Create a new workflow file named `label-new-issue.yml`.

### 1) Add a workflow name

Add this at the top of the file:

```yaml
Name: Label New Issues
```

### 2) Define the trigger

GitHub lists all available workflow triggers in its docs (the post links to the events-that-trigger-workflows reference):

- https://docs.github.com/en/actions/reference/workflows-and-actions/events-that-trigger-workflows

For this example, trigger on an issue being opened:

```yaml
on:
  issues:
    types: [opened]
```

### 3) Add a job, runner, and permissions

GitHub provides hosted runners for Ubuntu, Windows, and macOS. This job runs on `ubuntu-latest`.

It also sets permissions so the workflow can read repo contents and write to issues (to add a label):

```yaml
jobs:
  label-issues:
    runs-on: ubuntu-latest

    permissions:
      issues: write
      contents: read
```

### 4) Add steps (checkout + label using GitHub CLI)

This workflow:

- Checks out the repository
- Adds a `triage` label to the newly opened issue

```yaml
steps:
  - name: Checkout repository
    uses: actions/checkout@v6

  - name: Add triage label
    env:
      GH_TOKEN: ${{ secrets.GITHUB_TOKEN }}
      ISSUE_NUMBER: ${{ github.event.issue.number }}
      LABEL: "triage"
    run: gh issue edit "$ISSUE_NUMBER" --add-label "$LABEL"
```

Notes from the post:

- `uses` pulls in a prebuilt action from GitHub Marketplace.
- `run` executes shell commands/scripts.
- The `env` section defines environment variables.
- Because this uses the GitHub CLI, it needs auth via `GH_TOKEN`.
  - GitHub provides a temporary token automatically as `GITHUB_TOKEN`, accessible via `secrets.GITHUB_TOKEN`.
- The label (here, `triage`) must already exist in the repository.

## Testing your workflow

1. Push your changes.
2. Merge into the `main` branch.

If you need help pushing code, the post links an earlier episode:

- https://github.blog/developer-skills/github/beginners-guide-to-github-adding-code-to-your-repository

To test:

1. In the repo UI, go to **Issues**.
2. Click **New issue**.
3. Create an issue with a test title/description.
4. After creating it, you should see the `triage` label added within seconds.

## Reviewing current workflows

To inspect runs and debug:

1. Go to the **Actions** tab.
2. Select **Label New Issues** on the left.
3. Open the latest workflow run (for the test issue).
4. Select the `label-issues` job to see step-by-step output.

Additional workflow management options:

- Re-run runs via **Re-run all jobs**.
- Disable a workflow:
  - In **Actions**, select the workflow, click the three dots (**…**) near the search box, and choose **Disable workflow**.

The post also notes you can view deployments, runners, metrics, performance, and caches from the Actions UI.

## What’s next?

Examples of what GitHub Actions can automate beyond labeling issues:

- Publish packages
- Greet new contributors
- Build and test code
- Run security checks

Further docs:

- GitHub Actions documentation: https://docs.github.com/actions
- GitHub Skills exercises: https://learn.github.com/skills#automate-workflows-with-github-actions


[Read the entire article](https://github.blog/developer-skills/github/github-for-beginners-getting-started-with-github-actions/)

