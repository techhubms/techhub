---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/modernizing-legacy-java-project-using-github-copilot-app/ba-p/4440777
title: Modernizing Legacy Java Applications with GitHub Copilot App Modernization Upgrade
author: jorgebalderas
feed_name: Microsoft Tech Community
date: 2025-08-12 04:15:00 +00:00
tags:
- AI Powered Refactoring
- CI/CD
- Claude Sonnet 4
- Code Automation
- CVE Security Validation
- GitHub Copilot App Modernization
- GitHub Copilot Chat
- Gradle
- Jakarta EE Upgrade
- Java 21
- Java 8
- Java Application Modernization
- Legacy Code Upgrade
- Maven
- Maven Wrapper
- OpenRewrite
- PetClinic Example
- Spring Boot Upgrade
- Unit Test Generation
- Upgrade For Java
- Visual Studio Code Extension
- AI
- Coding
- GitHub Copilot
- Community
section_names:
- ai
- coding
- github-copilot
primary_section: github-copilot
---
Jorge Balderas presents a practical guide to upgrading legacy Java applications using the GitHub Copilot App Modernization – Upgrade for Java extension, highlighting AI-driven automation and hands-on steps for developers.<!--excerpt_end-->

# Modernizing Legacy Java Applications with GitHub Copilot App Modernization Upgrade

**Author:** Jorge Balderas

## Overview

The [GitHub Copilot App Modernization – Upgrade for Java](https://learn.microsoft.com/java/upgrade/overview) extension is designed to simplify and automate the process of modernizing legacy Java applications. Leveraging AI through GitHub Copilot and integration with Visual Studio Code, this toolkit enables developers to analyze, upgrade, validate, and secure Java projects with minimal manual intervention.

## Key Features

- **Automated Analysis:** Scans project dependencies and codebase to recommend an upgrade path.
- **Upgrade Execution:** Applies upgrades—including dependency and framework version bumps—via automated steps.
- **Issue Resolution:** Detects and auto-fixes common code issues during migration.
- **Reporting:** Provides detailed output, including commits, logs, and summaries of what changed.
- **Security Validation:** Performs CVE checks post-upgrade to flag and address vulnerabilities.
- **Unit Test Generation:** Automatically creates new test cases to help catch regressions.

## Prerequisites

Before beginning, ensure the following are installed:

1. [Visual Studio Code](https://code.visualstudio.com/download)
2. [GitHub Copilot account](https://github.com/features/copilot/plans?cft=copilot_li.features_copilot)
3. [GitHub Copilot chat](https://marketplace.visualstudio.com/items?itemName=GitHub.copilot-chat) extension
4. [GitHub Copilot app modernization - upgrade for Java](https://marketplace.visualstudio.com/items?itemName=vscjava.vscode-java-upgrade) extension
5. JDKs for the source and target platforms ([JDK 8](https://adoptium.net/temurin/releases/?version=8) and [JDK 21](https://adoptium.net/temurin/releases/?version=21))
6. [Node.js and npm](https://nodejs.org/download) for frontend app builds

## Step-by-Step Upgrade Process

### 1. Project Setup

- **Clone Example Repository:**

  ```
  git clone https://github.com/yortch/spring-petclinic-angularjs
  ```

- **Open in VS Code & Verify Build:**

  ```
  ./mvnw clean install
  cd spring-petclinic-server
  ../mvnw spring-boot:run
  ```

- Browse to [http://localhost:8080/](http://localhost:8080/) to confirm the project runs.

### 2. Exploring the Upgrade Tools

Within VS Code, the extension offers several tools, invoked via Copilot chat, such as:

- **Plan Generation:** `generate_upgrade_plan_for_java_project`
- **Plan Review:** `confirm_upgrade_plan_for_java_project`
- **Automated Changes:** `upgrade_java_project_using_openrewrite`
- **Build Validation:** `build_java_project`
- **Testing:** `run_tests_for_java`
- **Behavior Validation:** `validate_behavior_changes_for_java`
- **Security Check:** `validate_cves_for_java`
- **Documentation:** `summarize_upgrade`

Frameworks and libraries supported for upgrades include Spring Boot (2.7.x through 3.5.x), Jakarta EE 10, and custom Maven/Gradle dependencies. The underlying upgrade engine uses [OpenRewrite](https://github.com/openrewrite) (version 1.1.0).

### 3. Running the Upgrade

- **Generate the Upgrade Plan:**
  - Use Copilot chat with a prompt similar to:

    ```
    #generate_upgrade_plan_for_java_project for spring-petclinic-server project into Java 21 and latest SpringBoot 3.x
    ```

  - Specify JDK paths as needed for your environment.
- **Review and Confirm the Plan** once the preview is shown.
- **Run Each Step:**
  - The extension applies each upgrade phase, shows output, and iteratively resolves build or test errors.
  - Occasionally, manual intervention (e.g., clicking **Continue**) may be necessary.

### 4. Fixing Issues

- **Maven Wrapper Incompatibility:**
  - If the Maven wrapper (`mvnw`) fails due to version mismatch, use Copilot chat:

    ```
    Upgrade maven wrapper to at least version 3.6.3
    ```

  - Accept and commit the automatically suggested changes.
  - Rebuild and rerun the project to verify successful migration.

### 5. Finalizing and Version Control

- **Verify Project Runs:** Navigate to [http://localhost:8080](http://localhost:8080) post-upgrade.
- **Commit Changes:** Use a descriptive branch name like `java-upgrade-<unique id>`.
- **Push and Open a Pull Request:** Document and review all automated upgrades and fixes before merging.

## Additional Notes

- **AI Model:** Copilot agent mode with Claude Sonnet 4 can be used for additional natural language upgrade queries.
- **Test Coverage:** Use the test generation features to ensure functional equivalence and stability of upgrades.
- **Security Validation:** Automated CVE scanning is integrated into the workflow to maintain code safety post-upgrade.

## Summary

The GitHub Copilot App Modernization – Upgrade for Java extension enables developers to perform complex Java upgrades with speed and confidence, reducing manual effort and risk. By following this workflow, teams can keep their codebases current, secure, and well-tested with minimal disruption.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/modernizing-legacy-java-project-using-github-copilot-app/ba-p/4440777)
