---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/modernizing-java-ee-applications-to-jakarta-ee-with-github/ba-p/4486471
title: Modernizing Java EE Applications to Jakarta EE with GitHub Copilot App Modernization
author: ayangupta
feed_name: Microsoft Tech Community
date: 2026-01-17 12:52:07 +00:00
tags:
- API Compatibility
- App Modernization
- Automated Refactoring
- CI/CD
- Cloud Development
- Compilation Fixes
- CVEs
- Dependency Management
- Gradle
- IntelliJ IDEA
- Jakarta EE
- Java EE
- Java Migration
- Maven
- Microsoft Learn
- Namespace Migration
- OpenRewrite
- VS Code
section_names:
- ai
- devops
- github-copilot
---
ayangupta outlines how GitHub Copilot app modernization streamlines the migration of Java EE applications to Jakarta EE, providing automated analysis, code transformations, and workflow guidance for developers.<!--excerpt_end-->

# Modernizing Java EE Applications to Jakarta EE with GitHub Copilot App Modernization

Migrating from Java EE to Jakarta EE has become mandatory as the ecosystem adopts the new `jakarta.*` namespace, impacting servlets, persistence frameworks, messaging, and security. Manual migration is labor-intensive and error-prone, but GitHub Copilot app modernization can automate and accelerate this process for developers.

## Why Migrate Now?

- The entire Java enterprise ecosystem has shifted to Jakarta EE
- Frameworks and libraries expect `jakarta.*` imports instead of `javax.*`
- Staying up-to-date ensures compatibility, security, and access to ongoing support

## How Copilot App Modernization Assists

- **Project scans** detect Java EE libraries, outdated dependencies, and JDK constraints
- **Migration plans** specify required namespace changes, dependency updates, plugin modifications, and JDK settings
- **Automated transformations** rewrite imports, update versions, and fix API usage via OpenRewrite-powered rules
- **Build-fix cycles** automatically identify and resolve compilation issues post-migration
- **Security checks** surface CVEs and behavioral changes in updated dependencies

## Supported Migration Workflow

1. **Open your project in Visual Studio Code or IntelliJ IDEA** with Copilot app modernization enabled
2. **Full analysis**: Scans libraries, frameworks, server versions, and build configurations (Maven/Gradle)
3. **Migration plan generation**: Outlines changes from `javax.*` to `jakarta.*`, dependency alignments, and plugin updates
4. **Automated code transformation**: Applies refactoring and updates libraries and build files
5. **Build-fix iteration**: Repeats building and fixing until project compiles
6. **Security and behavior validation**: Highlights CVEs, API behavioral changes, and provides fix suggestions
7. **Developer review**: Run tests, review application behavior, update configs as needed

## Expected Output

- Updated source code with correct namespaces and dependencies
- Modernized build configuration (Maven/Gradle)
- Updated test suites and validation logic
- A summary file listing: all changes, dependency updates, CVE findings, and manual review items

## Developer Responsibilities

- Running application tests
- Reviewing for logic and behavior changes
- Verifying integration points (messaging, REST, persistence)
- Ensuring semantic and business correctness

## Resources

- [Quickstart: Upgrade a Java Project with GitHub Copilot App Modernization | Microsoft Learn](https://learn.microsoft.com/en-us/azure/developer/github-copilot-app-modernization/quickstart-upgrade)

By leveraging GitHub Copilot app modernization, development teams can focus less on manual migration work and more on validating complex logic and ensuring their applications remain secure, compatible, and maintainable.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/modernizing-java-ee-applications-to-jakarta-ee-with-github/ba-p/4486471)
