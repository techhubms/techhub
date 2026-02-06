---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-unit-test-agent-profiles-for-logic-apps-data-maps/ba-p/4490216
title: Introducing Unit Test Agent Profiles for Logic Apps & Data Maps
author: WSilveira
primary_section: ai
feed_name: Microsoft Tech Community
date: 2026-01-28 23:33:34 +00:00
tags:
- AI
- Automated Test SDK
- Azure
- Azure DevOps
- Azure Logic Apps
- Community
- Continuous Integration
- Custom Agents
- Data Map Testing
- DevOps
- Integration Testing
- LML
- Mock Data
- MSTest
- Scenario Design
- SDK Constraints
- Spec First Testing
- Speckit
- Test Automation
- Unit Testing
- Workflow Automation
- Workflow Discovery
- XSLT
- .NET
section_names:
- ai
- azure
- dotnet
- devops
---
WSilveira shares how GitHub Copilot custom agents can automate and standardize unit testing for Azure Logic Apps and Data Maps, emphasizing reusable specs, automated mock generation, and robust MSTest implementations.<!--excerpt_end-->

# Introducing Unit Test Agent Profiles for Logic Apps & Data Maps

**Author: WSilveira**

Unit testing is often an afterthought in integration projects, especially given the complexity of Azure Logic Apps Standard workflows and custom Data Maps. This article introduces a set of agent profiles—built with GitHub Copilot custom agents—that aim to make robust, maintainable, and consistent testing part of the standard workflow for cloud integration developers.

## Why Unit Test Agent Profiles?

The primary goal of these agent profiles is to:

- Simplify and automate the cumbersome steps of discovering workflows/maps
- Encourage spec-first development for clarity and maintainability
- Generate repeatable test cases, mock data, and code with minimal manual tweaking
- Foster collaboration across teams by adhering to structure and best practices

All agents leverage GitHub Copilot's extensibility to make automated test authoring accessible and reliable for Azure-based integration workloads.

## Key Features

- **Workflow Discovery:** Enumerates Logic Apps Standard workflows, actions, triggers, and dependencies to generate a testability inventory.
- **Spec-First Testing:** Support for Speckit-style reusable specs capturing intent, scenario data, and expected outcomes, enabling reliable code and data regeneration from the same contract.
- **Scenario-Driven Design:** Builds catalogs for common scenario categories (Happy Path, Error Handling, Edge Cases, etc.).
- **Mock and Test Data Generation:** Produces typed mocks for workflows and data files for Data Maps, improving test repeatability.
- **Automated MSTest Implementation:** Enforces code structure and reliability via the Automated Test SDK and MSTest on net8.0.
- **Batch Project Execution:** Supports batch operations for testing all workflows/maps with verification and reporting.

## Agent Profiles Available

### Logic Apps Workflow Unit Test Author

- **Purpose:** Author unit tests for Logic Apps Standard workflows using MSTest (net8.0) and the Automated Test SDK.
- **Capabilities:**
  - Discover workflow definitions (workflow.json)
  - Generate mock inventories
  - Create and maintain scenario-based test specs
  - Implement MSTest code adhering to SDK constraints
  - Batch testing and validation across all workflows
- **Example Skills**:
  - Workflow Discovery
  - Test Case Creation
  - Speckit Spec Authoring
  - MSTest Implementation
  - Test Data Generation
  - Project Batch Operations

### Logic Apps Data Map Unit Test Author

- **Purpose:** Build and manage unit tests for LML or XSLT Data Maps with MSTest (net8.0), supporting schema transformations and data validation.
- **Workflow:**
  - Discover all data maps (*.lml, *.xslt)
  - Capture transformation specs
  - Generate comprehensive test scenario catalogs
  - Produce input/output sample files to match mapping logic
  - Implement MSTest code for automated validation
  - Execute tests across all maps as a batch

## How the Agent Profiles Work

1. **Discover:** Scan project structure for workflows (workflow.json) or maps (*.lml, *.xslt) and summarize triggers, actions, and transformations.
2. **Spec-First:** Use reusable, human-readable Speckit-style specs to define intent, scenarios, mock plans, and expected outputs.
3. **Create Cases:** Build catalogs for different scenario types (Happy Path, Error Handling, Boundary Values, etc.)
4. **Generate Test Data:** Automate mock generation for Logic Apps or input/output pairs for Data Maps.
5. **Implement Tests:** Use MSTest and the Automated Test SDK to create test classes that follow required patterns and constraints.
6. **Validate:** Build and run the suite, with feedback to improve specs/data if needed.
7. **Batch Operations:** Apply above steps to all workflows/maps once project scaffolding is verified.

## Example Repository

You can find a full sample project here: [https://github.com/wsilveiranz/logicapps-unittest-custom-agent](https://github.com/wsilveiranz/logicapps-unittest-custom-agent). Fork it or adjust as suits your needs.

## FAQ

**Why Speckit-style specs?**
> Speckit specs act as single-source-of-truth contracts: they separate scenario design from code generation, helping scale automated test creation while preventing drift between intent and implementation.

**Can batch operations be run?**
> Yes, both Logic Apps and Data Maps agents support project-level orchestration—ideal for verifying or updating tests across an entire solution.

**What if scaffolding is missing?**
> The agents will report exactly which folders/files you need to create before batch testing (compatible with Logic Apps Designer's Create Unit Test commands or manual steps).

## Conclusion

With these agent profiles, teams can integrate scalable, maintainable unit testing as a first-class capability in Azure Logic Apps and Data Maps projects. The modular approach—anchored in discovery, reusable specs, and robust automation via GitHub Copilot—caters to real-world, multi-scenario integration challenges while adhering to industry best practices.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/introducing-unit-test-agent-profiles-for-logic-apps-data-maps/ba-p/4490216)
