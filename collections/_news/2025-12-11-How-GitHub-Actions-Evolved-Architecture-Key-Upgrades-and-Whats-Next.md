---
external_url: https://github.blog/news-insights/product-news/lets-talk-about-github-actions/
title: 'How GitHub Actions Evolved: Architecture, Key Upgrades & What’s Next'
author: Ben De St Paer-Gotch
feed_name: The GitHub Blog
date: 2025-12-11 17:40:52 +00:00
tags:
- Architecture
- Arm64 Hosted Runners
- Automation
- Caching
- CI/CD
- Community Feedback
- Custom Images
- Developer Experience
- GitHub Actions
- Macos 15
- News & Insights
- Performance Metrics
- Pipeline Scalability
- Product
- Product Feedback
- Reusable Workflows
- Windows
- Workflow Templates
- YAML Anchors
section_names:
- devops
primary_section: devops
---
Ben De St Paer-Gotch details how GitHub Actions was re-engineered to boost reliability and scalability, introduces several highly requested upgrades, and previews features planned for 2026—inviting community feedback throughout.<!--excerpt_end-->

# How GitHub Actions Evolved: Architecture, Key Upgrades & What’s Next

GitHub Actions has rapidly expanded its role in CI/CD and developer automation since its launch in 2018. In 2025 alone, developers clocked 11.5 billion Actions minutes in public and open source projects—a 35% increase from the previous year.

## Rebuilding the Core Backend Architecture

Facing exponential growth, the GitHub Actions team recognized the need to modernize its legacy backend services. In early 2024, core platform elements were re-architected to provide:

- Greater uptime and resilience
- Improved performance
- Deeper visibility into developer experience
- Capacity to scale 10x beyond previous usage

This new architecture, rolled out in August, now supports over 71 million jobs per day. Enterprises can launch 7x more jobs per minute compared to the old framework. Despite some slowed feature releases, the overhaul was necessary for sustainable growth and to unlock future enhancements.

## Community-Requested Improvements Shipped in 2025

With the infrastructure upgrade complete, focus shifted to delivering critical developer-requested features:

### YAML Anchors

Support for YAML anchors allows developers to define configuration fragments once and reuse them across jobs, dramatically reducing duplication in complex workflows.
> See: [Learn more about YAML anchors](https://docs.github.com/en/actions/reference/workflows-and-actions/reusing-workflow-configurations#yaml-anchors-and-aliases)

### Non-Public Workflow Templates

Organizations can now create and share private workflow templates for consistent, reliable CI patterns across teams—no more manual copy-pasting between repositories.
> See: [Learn more about workflow templates](https://docs.github.com/en/actions/concepts/workflows-and-actions/reusing-workflow-configurations#workflow-templates)

### Deeply Nested Reusable Workflows

Enhanced limits enable up to 10 levels of reusable workflow nesting and 50 workflow calls per run, giving teams more flexibility for large-scale, modular pipelines.
> See: [Learn more about reusable workflows](https://docs.github.com/en/actions/how-tos/reuse-automations/reuse-workflows)

### Larger Caches

Projects can now exceed the previous 10GB cache limit, alleviating build delays for large codebases and complex dependencies.
> See: [Learn more about managing cache storage](https://docs.github.com/en/actions/using-workflows/caching-dependencies-to-speed-up-workflows)

### Expanded Workflow Dispatch Inputs

The number of workflow dispatch inputs has increased from 10 to 25, supporting richer self-service automation and parameterized runs.
> See: [Learn more about workflow dispatch](https://docs.github.com/en/actions/how-tos/manage-workflow-runs/manually-run-a-workflow)

### Additional Platform Improvements

Further releases include:

- arm64-hosted runners (now available for public repositories)
- macOS 15 and Windows 2025 images
- Actions Performance Metrics (GA)
- Custom image support (public preview)

## Roadmap: Planned Features for Early 2026

GitHub Actions continues to respond to community demands with the following features scheduled for roll-out:

1. Timezone support for scheduled jobs
2. Returning run IDs from workflow dispatch
3. A case function for expressions
4. UX improvements (faster load times, job list filtering, enhanced rendering for large workflows)
5. Parallel steps—targeted for mid-2026—enabling concurrent workflow execution

The team also pledges to raise open source quality standards and keep releases transparent.

## Community Participation: Shape the Future

Ben De St Paer-Gotch encourages developers to:

- Vote for priority items in community discussions
- Participate in the new [community feedback discussion post](https://github.com/orgs/community/discussions/181437)
- Stay informed via the [GitHub Actions changelog](https://github.blog/changelog/?label=actions)

Persistent improvement relies on community feedback. The roadmap reflects an ongoing commitment to building flexible, scalable, and reliable CI/CD for all GitHub users.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/news-insights/product-news/lets-talk-about-github-actions/)
