---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/upgrade-your-java-jdk-8-11-17-21-or-25-with-github-copilot-app/ba-p/4486468
title: Upgrade Your Java JDK (8, 11, 17, 21, or 25) with GitHub Copilot App Modernization
author: ayangupta
feed_name: Microsoft Tech Community
date: 2026-01-20 02:12:24 +00:00
tags:
- App Modernization
- Automated Refactoring
- Cloud Development
- Code Upgrade
- Continuous Integration
- Dependency Management
- Gradle
- IDE Extension
- IntelliJ IDEA
- Jakarta EE
- Java
- Java EE
- JDK Upgrade
- JUnit
- Maven
- OpenRewrite
- Spring Boot
- Spring Framework
- VS Code
section_names:
- ai
- coding
- github-copilot
---
ayangupta explains how GitHub Copilot app modernization automates Java JDK and framework upgrades, helping developers update dependencies and migrate build systems with minimal manual effort.<!--excerpt_end-->

# Upgrade Your Java JDK (8, 11, 17, 21, or 25) with GitHub Copilot App Modernization

Developers looking to modernize Java applications often face the challenge of upgrading JDK versions, frameworks, dependencies, and build tools. GitHub Copilot app modernization streamlines this process by analyzing your project, identifying upgrade hurdles, and applying targeted, automated changes.

## Supported Upgrade Scenarios

GitHub Copilot app modernization automates upgrades for:

- Java Development Kit (JDK) to versions 8, 11, 17, 21, or 25
- Spring Boot (up to 3.5)
- Spring Framework (up to 6.2+)
- Java EE to Jakarta EE (up to Jakarta EE 10)
- JUnit updates
- Third-party dependency version alignment
- Ant to Maven build migrations

For a full capabilities list, refer to the [Microsoft Learn quickstart](https://learn.microsoft.com/en-us/azure/developer/github-copilot-app-modernization/quickstart-upgrade).

## Prerequisites

To use Copilot app modernization, you need:

- A GitHub account (Free, Pro, Pro+, Business, or Enterprise)
- Visual Studio Code (v1.101+) with Copilot and the app modernization extension; restart after installation
- OR IntelliJ IDEA (2023.3+) with Copilot plugin (1.5.59+); restart after installation
- Java project managed with Maven or Gradle under Git
- Connection to Maven Central (if Maven), Gradle wrapper v5+, and optional Kotlin DSL support
- “Tools enabled” set to true in VS Code (corporate orgs may control this)

## Selecting and Preparing a Java Project

Open your Java project in VS Code or IntelliJ IDEA. Sample projects include Maven-based uportal-messaging or Gradle-based docraptor-java. Once open, launch Copilot app modernization using Agent Mode.

## Running an Automated Upgrade Example (Java 8 → Java 21)

Initiate the upgrade by opening GitHub Copilot Chat, switching to Agent Mode, and issuing a prompt like:

> **Upgrade this project to Java 21**

Copilot app modernization will deliver:

1. **Upgrade Plan**: JDK updates, build file changes, dependency adjustments, and framework upgrade paths
2. **Automated Transformations**: Using OpenRewrite-based transformations, the tool applies code and build changes
3. **Dynamic Build/Fix Loop**:
    - Build
    - Detect failures
    - Apply fixes
    - Retry until success
4. **Security & Behavioral Checks**:
    - Detect CVEs in dependencies
    - Flag potential behavioral changes
    - Suggest fixes
5. **Final Upgrade Summary**: A markdown report listing updated JDK, dependency changes, code edits, and any remaining warnings or CVEs

## Typical Upgrade Outcomes

Expectations when upgrading from Java 8 to 21:

- Updated build settings (e.g., `maven.compiler.release` set to 21)
- Removal/replacement of deprecated APIs
- Library version adjustments for compatibility
- Surface warnings for manual review
- Building with modern JDK settings

This automated process accelerates modernization but leaves space for developers to review runtime or architectural impacts.

## Learn More

For the full upgrade workflow and deep-dive on capabilities, consult the [Microsoft Learn Copilot app modernization quickstart](https://learn.microsoft.com/en-us/azure/developer/github-copilot-app-modernization/quickstart-upgrade).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/upgrade-your-java-jdk-8-11-17-21-or-25-with-github-copilot-app/ba-p/4486468)
