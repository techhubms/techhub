---
layout: "post"
title: "Dependabot Adds Version Update Support for vcpkg"
description: "This news item announces that Dependabot now supports automatic version updates for vcpkg, Microsoft's open-source C/C++ package manager. Teams using vcpkg can take advantage of Dependabot to keep dependencies up-to-date, enhancing security and maintainability in C and C++ projects. Instructions are provided for configuring this integration within GitHub workflows."
author: "Allison"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/changelog/2025-08-12-dependabot-version-updates-now-support-vcpkg"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/changelog/feed/"
date: 2025-08-12 16:22:46 +00:00
permalink: "/2025-08-12-Dependabot-Adds-Version-Update-Support-for-vcpkg.html"
categories: ["Coding", "DevOps"]
tags: ["C++", "Coding", "Continuous Integration", "Dependabot", "Dependency Updates", "DevOps", "DevOps Automation", "GitHub", "Manifest Mode", "Microsoft", "News", "Open Source Tools", "Package Management", "Vcpkg"]
tags_normalized: ["c", "coding", "continuous integration", "dependabot", "dependency updates", "devops", "devops automation", "github", "manifest mode", "microsoft", "news", "open source tools", "package management", "vcpkg"]
---

Allison details the new Dependabot support for automatic version updates in vcpkg, enabling C/C++ projects to maintain secure and current dependencies within GitHub workflows.<!--excerpt_end-->

# Dependabot Adds Version Update Support for vcpkg

Dependabot now supports automatic version updates for [vcpkg](https://vcpkg.io/), Microsoft's free C/C++ package manager. This new integration allows teams to keep their C and C++ project dependencies automatically updated, improving project security and maintainability.

## How it works

- **Automatic Monitoring:** Once enabled, Dependabot will monitor your project's [`vcpkg.json` manifest files](https://learn.microsoft.com/vcpkg/concepts/manifest-mode).
- **Version Updates:** Dependabot generates pull requests that update the `builtin-baseline` commit hash to keep dependencies in sync with the latest versions from the [vcpkg port repository](https://github.com/microsoft/vcpkg).
- **Scope:** This feature applies specifically to version updates (not security updates).

## Getting Started

1. **Configure Dependabot:**
   - Add a vcpkg configuration entry to your project's `.github/dependabot.yml` file to enable monitoring and updating of vcpkg dependencies.
   - Refer to the [Dependabot options reference](https://docs.github.com/code-security/dependabot/working-with-dependabot/dependabot-options-reference) for supported configuration options.

## Additional Resources

- [Dependabot version updates now support vcpkg (GitHub Blog)](https://github.blog/changelog/2025-08-12-dependabot-version-updates-now-support-vcpkg)
- [Learn more about vcpkg](https://vcpkg.io)
- [Dependabot options reference](https://docs.github.com/code-security/dependabot/working-with-dependabot/dependabot-options-reference)
- [Engage with the Dependabot open source community](https://github.com/dependabot/dependabot-core/issues/7644)

Teams using C and C++ with vcpkg can now benefit from greater automation in managing their dependencies within GitHub repositories.

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/changelog/2025-08-12-dependabot-version-updates-now-support-vcpkg)
