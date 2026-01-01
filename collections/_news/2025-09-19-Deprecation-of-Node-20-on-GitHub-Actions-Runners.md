---
layout: "post"
title: "Deprecation of Node 20 on GitHub Actions Runners"
description: "This announcement details the planned end-of-life for Node20 support on GitHub Actions runners, migration timing to Node24, changes in runner compatibility, operating system requirements, and specific update steps for both maintainers and users of GitHub Actions. It highlights required actions, environmental variable options, and critical deadlines for continued support."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-09-19-deprecation-of-node-20-on-github-actions-runners"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-09-19 18:29:59 +00:00
permalink: "/2025-09-19-Deprecation-of-Node-20-on-GitHub-Actions-Runners.html"
categories: ["DevOps"]
tags: ["Actions", "Actions Maintenance", "ARM32", "Continuous Integration", "Deprecation", "DevOps", "GitHub Actions", "Macos", "News", "Node20", "Node24", "Operating System Compatibility", "Retired", "Runner", "Self Hosted Runner", "Version Migration", "Workflow"]
tags_normalized: ["actions", "actions maintenance", "arm32", "continuous integration", "deprecation", "devops", "github actions", "macos", "news", "node20", "node24", "operating system compatibility", "retired", "runner", "self hosted runner", "version migration", "workflow"]
---

Allison summarizes the end-of-life timeline for Node20 on GitHub Actions runners and outlines migration steps for maintainers and users as Node24 becomes the new default.<!--excerpt_end-->

# Deprecation of Node 20 on GitHub Actions Runners

Node20 will reach end-of-life (EOL) in April 2026. GitHub has begun deprecating Node20 support for GitHub Actions, with a plan to migrate all Actions to Node24 in the fall of 2025.

## Timeline and Key Milestones

- **GitHub Actions Runner v2.328.0** now supports both Node20 (default) and Node24.
- **Testing Node24**: Set `FORCE_JAVASCRIPT_ACTIONS_TO_NODE24=true` in your workflow environment or runner to test Node24 in advance.
- **Default Switch Date**: Starting March 4th, 2026, runners will use Node24 by default.
- **Opt-out Option**: To continue using Node20 after March 4th, 2026, set `ACTIONS_ALLOW_USE_UNSECURE_NODE_VERSION=true` in your workflow, but this will only function until Node20 is fully removed after the summer 2026 runner upgrade.

## Removal of Operating System Support with Node24

- **macOS Support**: Node24 is incompatible with macOS 13.4 and earlier versions.
- **ARM32 Support**: Node24 does not officially support ARM32, so self-hosted runners on ARM32 will cease to be supported post-Node20 deprecation.
- **Refer to Documentation**: See [official documentation](https://docs.github.com/en/actions/creating-actions/metadata-syntax-for-github-actions#runs-for-javascript-actions) for supported OS versions and architectures.

## Recommended Actions

### For Actions Maintainers

- Update your actions to target Node24 instead of Node20.
- See [Actions configuration settings](https://docs.github.com/en/actions/creating-actions/metadata-syntax-for-github-actions#runs-for-javascript-actions) for details.

### For Actions Users

- Update your workflows to use the latest versions of actions that utilize Node24.
- Guidance can be found in [Using versions for Actions](https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions#example-using-versioned-actions).

## Join the Discussion

- For questions or concerns, join the [GitHub Community discussions](https://github.com/orgs/community/discussions/categories/announcements).

Stay updated on deprecation milestones to ensure your workflows remain secure and compatible.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-09-19-deprecation-of-node-20-on-github-actions-runners)
