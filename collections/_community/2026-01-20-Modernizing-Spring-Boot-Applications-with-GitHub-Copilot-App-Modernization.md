---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/modernizing-spring-boot-applications-with-github-copilot-app/ba-p/4486466
title: Modernizing Spring Boot Applications with GitHub Copilot App Modernization
author: ayangupta
feed_name: Microsoft Tech Community
date: 2026-01-20 02:38:47 +00:00
tags:
- App Modernization
- Automated Refactoring
- Build Tools
- CVE Validation
- Dependency Management
- IntelliJ IDEA
- Jakarta EE
- Java
- Java Development Kit
- JUnit 5
- Microsoft Learn
- Migration
- Plugin Updates
- Spring Boot
- Spring Framework
- VS Code
section_names:
- ai
- coding
- github-copilot
primary_section: github-copilot
---
ayangupta details how GitHub Copilot app modernization automates upgrading Spring Boot 2.x applications to 3.x, providing a guided path for code migration, dependency updates, and Java platform compatibility.<!--excerpt_end-->

# Modernizing Spring Boot Applications with GitHub Copilot App Modernization

Upgrading Spring Boot applications from 2.x to 3.x is a major undertaking due to framework, dependency, and Jakarta namespace changes. This migration offers better support for modern Java, performance improvements, and ensures long-term compatibility, but also introduces breaking changes and complex dependency shifts.

GitHub Copilot app modernization is designed to help streamline and automate this process:

## Supported Upgrade Path

- Compatible with upgrades to Spring Boot 3.5
- Updates underlying Spring Framework libraries to 6.x
- Migrates code from `javax.*` to `jakarta.*`
- Aligns dependencies and plugins to Spring Boot 3.x requirements
- Adjusts build configurations for required JDK levels
- Validates dependency updates and surfaces CVE-related security issues

## Key Features of GitHub Copilot App Modernization

### Project Analysis

- Identifies your current Spring Boot version
- Flags incompatible libraries, starters, and plugins
- Highlights `javax.*` imports for migration
- Assesses build and JDK configuration for upgrade readiness

### Upgrade Plan Generation

- Produces an upgrade blueprint:
  - Outlines the new parent and dependency versions
  - Lists required namespace changes
  - Recommends plugin and JDK configuration updates
- Gives developers the chance to review and tweak before changes are applied

### Automated Transformations

- Updates build parent to Spring Boot 3.5.x
- Rewrites imports from `javax.*` to `jakarta.*`
- Updates dependencies and manages BOMs
- Refactors deprecated or removed APIs
- Brings tests (e.g., JUnit 5) and plugins up to date

### Build & Fix Iterations

- Automatically builds the project
- Captures and reports failures
- Suggests and applies incremental fixes
- Repeats the process until project compiles successfully

### Security & Behavior Validation

- Checks for new or changed CVEs in dependencies
- Highlights potential behavior changes post-upgrade
- Provides options for remediating surfaced issues

## Outcome and Developer Responsibilities

Post-upgrade, developers can expect:

- Up-to-date Maven/Gradle build files referencing Spring Boot 3.x and Spring Framework 6.x
- Code rewritten to use Jakarta APIs
- Latest versions for core dependencies and plugins
- Upgrade summary file for traceability

However, manual validation remains essential. Developers should:

- Run all tests to verify correctness
- Inspect custom filters and security configurations
- Review integration points and unique web customizations
- Ensure intended behavior across various runtime scenarios

## Additional Resources

- [Quickstart: Upgrade a Java Project with GitHub Copilot App Modernization](https://learn.microsoft.com/en-us/azure/developer/github-copilot-app-modernization/quickstart-upgrade)
- [Install for VS Code](https://aka.ms/AM014)
- [Install for IntelliJ IDEA](https://aka.ms/AM015)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/modernizing-spring-boot-applications-with-github-copilot-app/ba-p/4486466)
