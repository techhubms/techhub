---
author: Shikhaghildiyal
section_names:
- ai
- azure
- devops
- github-copilot
title: Building a VS Code extension to scaffold Terraform with guardrails and GitHub Copilot
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/vs-code-extension/ba-p/4500803
tags:
- AI
- Azure
- Azure Infrastructure
- CI/CD Workflow
- Community
- Deterministic Guardrails
- DevOps
- Generator Code
- GitHub Copilot
- GitLab
- IaC
- JFrog
- Module Templates
- Node.js
- Prompt Engineering
- Repository APIs
- Terraform
- Terraform Modules
- TypeScript
- Visual Studio Marketplace
- VS Code
- VS Code Extension API
- VS Code Extensions
- Vsce
- Yeoman
feed_name: Microsoft Tech Community
primary_section: github-copilot
date: 2026-03-31 03:00:06 +00:00
---

Shikhaghildiyal walks through what VS Code extensions are and how to build one, then describes an enterprise use case: a custom extension that scaffolds Terraform from centrally managed module templates and uses GitHub Copilot to speed up authoring while keeping deterministic compliance guardrails in place.<!--excerpt_end-->

## Overview

Managing Azure infrastructure efficiently often requires bringing infrastructure tasks into the same place developers already work. This post explains VS Code extensions and outlines a practical use case: a custom extension that scaffolds Terraform from centrally maintained module templates, with deterministic validation and optional AI-assisted generation using GitHub Copilot.

## What is a VS Code extension?

A VS Code extension is a small program that adds features to Visual Studio Code.

Typical extension capabilities include:

- Language support (syntax highlighting, IntelliSense)
- Custom commands in the Command Palette
- Automating repetitive development tasks
- Integrations with external tools and services
- Formatting, linting, and debugging support

Distribution is typically done via the Visual Studio Marketplace.

High-level characteristics:

- Runs on Node.js
- Written in JavaScript or TypeScript
- Uses the VS Code Extension API to interact with the editor

## How to create a VS Code extension (step-by-step)

### Step 1: Prerequisites

Install:

- Node.js
- npm
- Git (recommended)
- Visual Studio Code

### Step 2: Install Yeoman and the VS Code extension generator

Microsoft provides an official Yeoman generator.

```bash
npm install -g yo generator-code
```

This installs:

- Yeoman (`yo`) for scaffolding
- `generator-code` for VS Code extension templates

### Step 3: Scaffold a new extension

```bash
yo code
```

You’ll be prompted for:

- Extension type (TypeScript or JavaScript)
- Name, identifier, description
- Package manager (npm recommended)

### Step 4: Understand the generated project structure

Common files/folders:

- `package.json` (metadata, commands, contributions)
- `src/extension.ts` (or `extension.js`) for activation and core logic
- `.vscode/launch.json` for debugging
- `README.md` for documentation

`package.json` defines what your extension contributes and which VS Code versions it supports.

### Step 5: Run and test the extension

1. Open the extension project in VS Code
2. Press **F5** (Start Debugging)
3. A new **Extension Development Host** window opens
4. Open Command Palette (**Ctrl+Shift+P**)
5. Run your command (for example, *Hello World*)

### Step 6: Modify the extension

Edit `extension.ts` to:

- Change user-visible messages
- Register new commands
- Use VS Code APIs (notifications, inputs, file access)

After changes:

- Reload the Extension Development Host
- Re-run your command to validate behavior

### Step 7: Package and publish (optional)

Install the VS Code Extension CLI:

```bash
npm install -g vsce
```

Package and publish:

```bash
vsce package
vsce publish
```

## Use case: Terraform scaffolding for Azure infrastructure with deterministic guardrails

In an enterprise infrastructure initiative, engineers needed to generate Terraform code for CPF modules in strict alignment with reference guidelines and approved module templates.

Problem:

- Modules were centrally maintained in a repository, but manual consumption required repeated searching, interpretation, and boilerplate assembly.

Approach:

- Build a custom VS Code extension to automate Terraform scaffolding.
- Keep deterministic control points for auditability.
- Integrate with repository APIs to fetch the latest templates and extract module metadata (variables, outputs, structural requirements).

Constraint:

- MCP (or any managed AI orchestration platform) could not be used, so the design stayed entirely within the VS Code extension boundary.

### Where GitHub Copilot fits

For the Terraform code generator portion, GitHub Copilot is used to assist with producing the final Terraform configuration content.

The extension remains the governing layer by:

- Constraining and validating generated output
- Enforcing module selection rules
- Enforcing naming conventions
- Enforcing approved file structure

This is presented as a pattern: Copilot improves speed inside the IDE while deterministic guardrails preserve compliance.

## End-to-end flow (as described)

1. **Start**
   - User launches the VS Code extension.
2. **Resource & source selection**
   - User selects resource types and a source (GitLab or JFrog).
3. **Source choice branching**
   - **GitLab path**: fetch projects, filter and rank modules.
   - **JFrog path**: fetch artifacts, filter and rank modules.
4. **Ranked module list**
   - Show consolidated ranked modules.
5. **User selection**
   - User selects modules to deploy.
6. **Download/clone modules**
   - Clone Git repositories or download artifacts; extract to workspace.
7. **Terraform parser**
   - Parse `.tf` files for:
     - `locals`
     - module calls
     - `required_providers`
     - provider blocks
     - backend configuration
8. **Metadata assembly**
   - Aggregate:
     - `module_info`
     - `example_values` (locals, module calls, providers, backend)
9. **Output generation**
   - Save module JSON (separate files for GitLab and JFrog).
   - Generate prompt file:
     - Initialize `prompt-for-Iac.md`
     - Append module metadata
10. **AI-assisted IaC generation**
   - Use the prompt to generate:
     - `main.tf`
     - `providers.tf`
     - `variables.tf`
     - `outputs.tf`
11. **Deployable Terraform code**
   - Ready for:
     - `terraform init`
     - `terraform plan`
     - `terraform apply`
12. **End**
   - User reviews output and replaces values if required.

## Metadata from source

- Updated: Mar 29, 2026
- Version: 1.0
- Author: Shikhaghildiyal

[Read the entire article](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/vs-code-extension/ba-p/4500803)

