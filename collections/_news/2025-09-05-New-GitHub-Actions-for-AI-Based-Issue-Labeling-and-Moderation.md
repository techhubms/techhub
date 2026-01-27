---
external_url: https://github.blog/changelog/2025-09-05-github-actions-ai-labeler-and-moderator-with-the-github-models-inference-api
title: New GitHub Actions for AI-Based Issue Labeling and Moderation
author: Allison
feed_name: The GitHub Blog
date: 2025-09-05 22:38:54 +00:00
tags:
- AI Models
- Automation
- Community Health
- Developer Tools
- GitHub Actions
- GitHub Models Inference API
- Issue Triage
- Moderation
- Open Source
- Spam Detection
- Workflow Automation
section_names:
- ai
- devops
primary_section: ai
---
Allison highlights two new GitHub Actions for open source maintainers: an AI assessment comment labeler and an AI moderator, both using the GitHub Models inference API to automate triage and moderation.<!--excerpt_end-->

# New GitHub Actions for AI-Based Issue Labeling and Moderation

GitHub has introduced two powerful GitHub Actions to assist open source maintainers with automating issue triage and moderation through the [GitHub Models inference API](https://docs.github.com/rest/models?apiVersion=2022-11-28):

## AI Assessment Comment Labeler

- **Purpose:** Automate the triage of issues by applying trigger labels and running AI assessments.
- **How it works:**
  - Configurable to act on specific labels and issue templates.
  - Runs multiple AI prompts in parallel, generating standard labels such as `ai:bug-review:ready for review`.
  - These labels streamline filtering, searching, and automating project workflows.
  - Option to suppress comments for seamless integration into existing automation pipelines.

## AI Moderator

- **Purpose:** Automatically scan issues and comments for spam, link spam, or AI-generated content.
- **Key Features:**
  - Auto-label and optionally minimize flagged content.
  - Supports custom moderation prompts for flexible enforcement.

## Shared Features

- Both actions use your workflow's `GITHUB_TOKEN` with `models: read` permissions, eliminating the need for extra API keys.
- Prompts are customizable—maintainers can use multiple prompt files for assessments and tailor moderation prompts.
- Designed to integrate with existing workflows for reduced manual workload and improved open source community health.

## Additional Resources

- Explore the [AI inference action reference template](https://github.com/actions/ai-inference) for custom automation.
- Read the full [models documentation](https://docs.github.com/github-models).
- Join [community discussions](https://github.com/orgs/community/discussions/categories/models?discussions_q=is%3Aopen+category%3AModels+) for questions and feedback.

These actions aim to make open source maintenance more efficient by leveraging AI-powered automation within GitHub workflows.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-05-github-actions-ai-labeler-and-moderator-with-the-github-models-inference-api)
