---
external_url: https://github.blog/changelog/2025-08-26-dependabot-can-now-exclude-automatic-pull-requests-for-manifests-in-selected-subdirectories
title: Suppress Dependabot PRs in Specific Subdirectories with `exclude-paths`
author: Allison
feed_name: The GitHub Blog
date: 2025-08-26 17:53:23 +00:00
tags:
- Automation
- Change Management
- CI/CD
- Configuration
- Dependabot
- Dependabot.yml
- Dependency Management
- Exclude Paths
- GitHub
- Monorepo
- Pull Requests
- Repository Management
- Software Maintenance
- Subdirectory
- DevOps
- News
section_names:
- devops
primary_section: devops
---
Allison explains how GitHub's Dependabot now supports excluding dependency manifests in chosen subdirectories, making it easier for teams to control update noise in monorepos and refine DevOps automation.<!--excerpt_end-->

# Suppress Dependabot PRs in Specific Subdirectories with `exclude-paths`

Dependabot's latest update introduces the `exclude-paths` option in your `dependabot.yml` file, letting you exclude certain folders or files—either explicitly or via glob patterns—so that dependency manifests in those paths are ignored.

## Key Features

- **Exclude Directories or Files:** Prevents dependency update pull requests for manifests in designated subdirectories.
- **Glob Pattern Support:** Specify complex exclusion rules using patterns like `*` and `**`.
- **Per-Ecosystem Tailoring:** Use exclusion rules in each `updates` block to fine-tune behavior for different package managers or languages.
- **Works for Large Repositories:** Especially helpful for monorepos with multiple project types, sample code, test fixtures, or experimental sections.

## Practical Impact

Prior to this feature, users had to list out every included directory or accept unwanted PR noise. Now, you can:

- Use a single, broad directory pattern (e.g., `directory: "/"`)
- Exclude areas like `examples/`, `samples/`, and archived code
- Minimize distraction and manual triage of unnecessary PRs

## Usage Tips

- Exclusions are evaluated before parsing manifests, so ignored paths are not even scanned
- Exclusion does not replace dependency-level `ignore` rules
- For overlapping `updates` blocks, each block's exclusions apply independently
- Grouping only applies to included (not excluded) manifests

## Security & Compliance Considerations

Excluding directories means updates for dependencies present only in those locations will not be surfaced. Ensure that paths you exclude are non-production or intentionally unmanaged to avoid missing important security updates.

## Rollout

- **GitHub.com:** Available immediately
- **GitHub Enterprise Server (GHES):** Shipping in version 3.19

For more details, refer to [Dependabot documentation](https://docs.github.com/code-security/dependabot/working-with-dependabot/dependabot-options-reference#exclude-paths-).

Join the [Dependabot Community discussion](https://github.com/dependabot/dependabot-core/issues/4364) to share feedback and questions.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-26-dependabot-can-now-exclude-automatic-pull-requests-for-manifests-in-selected-subdirectories)
