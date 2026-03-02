---
layout: "post"
title: "Keep Your GitHub Action Examples in Sync with Actions Example Checker"
description: "Jesse Houwing presents a practical solution for keeping GitHub Actions documentation up to date with implementation changes. The blog introduces the Actions Example Checker—a tool and corresponding GitHub Action—to automatically validate usage examples in markdown files against action.yaml schemas, reducing drift between documentation and code."
author: "Jesse Houwing"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://jessehouwing.net/keep-your-examples-in-sync-with-your-action/"
viewing_mode: "external"
feed_name: "Jesse Houwing's Blog"
feed_url: "https://jessehouwing.net/rss/"
date: 2026-03-02 09:53:49 +00:00
permalink: "/2026-03-02-Keep-Your-GitHub-Action-Examples-in-Sync-with-Actions-Example-Checker.html"
categories: ["DevOps"]
tags: ["Action.yaml", "Actions Example Checker", "Blogs", "CI/CD", "Continuous Integration", "DevOps", "Documentation Automation", "Example Checker", "GitHub", "GitHub Actions", "GitHub Marketplace", "Open Source Tooling", "Pull Request Validation", "Schema Validation", "Workflow Automation", "YAML"]
tags_normalized: ["actiondotyaml", "actions example checker", "blogs", "cislashcd", "continuous integration", "devops", "documentation automation", "example checker", "github", "github actions", "github marketplace", "open source tooling", "pull request validation", "schema validation", "workflow automation", "yaml"]
---

Jesse Houwing introduces the Actions Example Checker, a GitHub Action that validates documentation examples against action.yaml schemas to help developers keep their documentation in sync with actual implementations.<!--excerpt_end-->

# Keep Your GitHub Action Examples in Sync with Actions Example Checker

*By Jesse Houwing*

While publishing new GitHub Actions, I noticed how easy it was to let my documentation fall out of sync with the actual implementation. Updating examples was often forgotten as actions evolved, leading to confusion and outdated references.

To tackle this, I developed a simple script—which quickly grew into a standalone GitHub Action: [Actions Example Checker](https://github.com/marketplace/actions/actions-example-checker?ref=jessehouwing.net).

## What Is Actions Example Checker?

Actions Example Checker is a GitHub Action that:

- Scans your repository for all `action.yaml` files and extracts relevant metadata
- Searches all markdown files and collects YAML code blocks representing action usage
- Checks these examples against the defined schema in `action.yaml` (or an extended `action.schema.yaml`), reporting mismatches or outdated examples
- Optionally uses an extended schema to validate inputs like allowed values, datatypes, or regex patterns

This approach prevents documentation drift, ensuring your users always see correct and valid usage samples.

## How to Integrate Example Checking in Your Workflow

Add the following workflow to your repository:

```yaml
name: Validate Documentation
on:
  pull_request:
  push:
    branches: [main]
  workflow_dispatch:
permissions: {}
jobs:
  validate-docs:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@de0fac2e4500dabe0009e67214ff5f5447ce83dd # v6
      with:
        sparse-checkout: |
          **/action.yaml
          **/action.yml
          **/action.schema.yaml
          **/action.schema.yml
          **/*.md
        sparse-checkout-cone-mode: false
    - uses: jessehouwing/actions-example-checker@ea0821955c7ecf2c98e5b184054ba31486914a7b # v0.0.4
      with:
        token: ${{ secrets.GITHUB_TOKEN }}
```

## Sample Schemas

You can view real-world schema validation examples in these repositories:

- [actions-example-checker/action.schema.yaml](https://github.com/jessehouwing/actions-example-checker/blob/main/action.schema.example.yml?ref=jessehouwing.net)
- [actions-semver-checker/action.schema.yaml](https://github.com/jessehouwing/actions-semver-checker/blob/main/action.schema.yml?ref=jessehouwing.net)
- [actions-dependency-submission/action.schema.yaml](https://github.com/jessehouwing/actions-dependency-submission/blob/main/action.schema.yaml?ref=jessehouwing.net)
- [azdo-marketplace/action.schema.yaml](https://github.com/jessehouwing/azdo-marketplace/blob/main/action.schema.yaml?ref=jessehouwing.net)

These can serve as references when creating your own schema files.

## Conclusion

Automating the checking of documentation examples against actual Action schemas not only saves time but also improves reliability and user confidence in your GitHub Actions. If you find this tool useful, feel free to reach out and share your feedback!

This post appeared first on "Jesse Houwing's Blog". [Read the entire article here](https://jessehouwing.net/keep-your-examples-in-sync-with-your-action/)
