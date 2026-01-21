---
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/how-to-modernise-a-microsoft-access-database-forms-vba-to-node/ba-p/4473504
title: 'Modernizing Microsoft Access: Migrating to Node.js, OpenAPI, SQL Server, and MongoDB with GitHub Copilot'
author: anthkernan
feed_name: Microsoft Tech Community
date: 2025-12-08 19:59:25 +00:00
tags:
- Accessibility
- API First
- Business Logic Migration
- Constraint Refactoring
- Database Modernization
- Liquibase
- Microsoft Access
- MongoDB
- N Tier Architecture
- Node.js
- OpenAPI
- Repository Pattern
- REST API
- SQL Server
- SQL Server Migration Assistant
- Synthetic Data
- Test Automation
- VBA
- VS Code
- WCAG
section_names:
- ai
- azure
- coding
- devops
- github-copilot
---
anthkernan presents a detailed guide on modernizing a Microsoft Access solution using Node.js, OpenAPI, SQL Server, and GitHub Copilot, with a focus on code migration, automation, and architectural best practices.<!--excerpt_end-->

# Modernizing Microsoft Access: Migrating to Node.js, OpenAPI, SQL Server, and MongoDB with GitHub Copilot

## Overview

This post documents a real-world modernization project converting a Microsoft Access database (with Forms and VBA) to a modern architecture leveraging Node.js, OpenAPI, SQL Server, and, for demonstration, MongoDB. The approach utilizes GitHub Copilot and Visual Studio Code to accelerate each phase, highlighting example prompts, migration techniques, and the architectural decisions underpinning the solution.

## Background and Motivation

Microsoft Access has long been a backbone for rapid application development in enterprises, but its limitations around scalability, data fragmentation, and integrations often necessitate modernization. Modern equivalents typically require rethinking business logic, database design, APIs, and UI frameworks.

## Solution Architecture: The N-Tier Approach

- **Data Management Layer:** Migration from Access to SQL Server using Microsoft SSMA.
- **Data Access/API Layer:** Definition and scaffolding of REST APIs with OpenAPI (Swagger) specifications.
- **Business Logic Layer:** Automated translation of VBA logic to Node.js services using Copilot.
- **Presentation Layer:** Redesign of GUIs referencing legacy layouts, accessibility requirements, and Copilot’s image-based prompt capabilities.

## Database Migration with Microsoft SSMA and Copilot

- Utilized **SQL Server Migration Assistant (SSMA)** to automate migration of Access tables, constraints, and data.
- Addressed quirks from SSMA, such as non-descriptive constraint names, by scripting constraint renaming via Copilot prompts.
- Copilot batch-generated SQL scripts for more maintainable naming conventions and bulk modifications.
- [Example Copilot Prompt]: Provided multi-phase prompts guiding constraint renaming and generation of SQL migration summaries.

## Automated Change Management with Liquibase

- Introduced **Liquibase** to manage database schema changes and deployments.
- Used Copilot to refactor manually generated changelog files for consistency, traceability, and cross-DB compatibility.
- Sample prompts guided Copilot to convert vendor-specific types to Liquibase-agnostic types.
- Enforced clear summary documentation of changes and rationale for data type decisions.

## Generating Synthetic Data

- Leveraged Copilot to create seed data sets for test environments, ensuring data privacy and fictitious placeholders.
- This improved developer productivity by decoupling development from sensitive production data.

## API Development with OpenAPI & Node.js

- Defined API contracts for all entities with OpenAPI specifications.
- Copilot generated CRUD endpoints mapped to both SQL Server and (demonstration) MongoDB backends.
- Employed repository patterns and self-documenting code practices for maintainability and transparency.
- Implemented validation and error handling up front to meet modern organizational development standards.

## Business Logic Migration

- Translated VBA routines to Node.js services using Copilot guidance and prompt engineering.
- Wrote corresponding automated tests for new business logic, raising confidence versus the legacy codebase.

## User Interface Modernization

- Recreated Access form layouts using Copilot's support for image-based prompts.
- Maintained interface familiarity while introducing accessibility improvements (WCAG compliance, labeling, etc).
- Prompted Copilot for iterative UI generation followed by manual UX/accessibility refinement.

## User Story Generation

- Used Copilot to translate UI and legacy requirements into user stories for specification-driven development.
- Aided business analysts in documenting formal requirements where none existed before.

## Introducing MongoDB (Bonus Demonstration)

- Demonstrated migration/design of selected entities from SQL Server to MongoDB.
- Used Copilot to analyze relational structures and advise on denormalized, access-pattern-optimized MongoDB schemas with updated ERDs.
- Generated Node.js data access code capable of swapping between SQL Server and MongoDB.

## Key Outcomes

- **Speed**: Modernization achieved in 2 weeks instead of many months.
- **Quality**: Automated prompt-driven code generation facilitated robust, standards-compliant, and testable outputs.
- **Documentation**: Copilot aided in generating migration summaries, usage documentation, and entity relationship diagrams.
- **Blueprint**: Serves as a practical playbook for organizations upgrading legacy Access-based solutions leveraging Microsoft and open-source technologies.

---

*Author: anthkernan*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-architecture-blog/how-to-modernise-a-microsoft-access-database-forms-vba-to-node/ba-p/4473504)
