---
date: 2026-03-31 17:34:41 +00:00
title: Dependabot now supports Xcode projects using SwiftPM with .xcodeproj manifests
feed_name: The GitHub Blog
primary_section: devops
tags:
- .xcodeproj
- .xcworkspace
- Dependabot
- Dependabot.yml
- Dependency Updates
- DevOps
- GitHub
- GitHub Blog
- GitHub Enterprise Server 3.22
- Improvement
- Ios
- Macos
- News
- Package.resolved
- Project.pbxproj
- Pull Requests
- Security
- Supply Chain Security
- Swift Package Manager
- SwiftPM
- Xcode
external_url: https://github.blog/changelog/2026-03-31-dependabot-now-supports-xcode-projects-using-swiftpm-with-xcodeproj-manifests
author: Allison
section_names:
- devops
- security
---

Allison announces that Dependabot can now detect and update Swift Package Manager dependencies in Xcode-managed projects (including .xcodeproj/.xcworkspace setups), enabling automated PR-based updates even when no top-level Package.swift manifest exists.<!--excerpt_end-->

## Summary

Dependabot can now detect and update Swift Package Manager (SwiftPM) dependencies for Xcode projects that manage packages through `.xcodeproj` bundles, even when there is no top-level `Package.swift` file. This change is generally available and targets a common Xcode workflow where dependency data lives inside Xcode project/workspace bundles.

## What changed

Many iOS and macOS apps store SwiftPM dependency information in Xcode-managed files inside project/workspace bundles:

- Version rules in `project.pbxproj`
- Resolved dependency pins in `Package.resolved`
  - Often nested inside `.xcodeproj` or `.xcworkspace` bundles

Previously, Dependabot required a top-level `Package.swift` manifest to discover and update Swift dependencies, which meant a lot of Xcode-managed SwiftPM projects couldn’t use Dependabot for automated updates.

## What Dependabot will do now

With this release, Dependabot will:

- Automatically discover Xcode-managed `Package.resolved` files in:
  - `.xcodeproj` bundle layouts
  - `.xcworkspace` bundle layouts
- Parse dependency version rules from `project.pbxproj` so it respects the version constraints configured in Xcode
- Open pull requests that update the relevant `Package.resolved` file(s) in place

## How it behaves in your repo

- If your repository contains an `.xcodeproj` bundle with a `Package.resolved` file, Dependabot will pick it up automatically on the next scheduled run.
- You can add or adjust your `dependabot.yml` to customize update behavior for the `swift` package ecosystem.

## Availability

- Available now for GitHub.com (cloud)
- Planned support in GitHub Enterprise Server **3.22**

## References

- Most-requested enhancement discussion: https://github.com/dependabot/dependabot-core/issues/7694
- Supported ecosystems documentation: https://docs.github.com/code-security/reference/supply-chain-security/supported-ecosystems-and-repositories#supported-ecosystems-maintained-by-github


[Read the entire article](https://github.blog/changelog/2026-03-31-dependabot-now-supports-xcode-projects-using-swiftpm-with-xcodeproj-manifests)

