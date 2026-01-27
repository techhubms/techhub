---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/node-js-24-is-now-available-on-azure-app-service-for-linux/ba-p/4468801
title: Node.js 24 Now Available on Azure App Service for Linux
author: TulikaC
feed_name: Microsoft Tech Community
date: 2025-11-11 04:10:13 +00:00
tags:
- App Service Deployment
- ARM Templates
- Azure App Service
- Bicep Templates
- CI Workflows
- Continuous Integration
- JavaScript
- Linux
- Node:test
- Node.js 24
- npm 11
- Testing
- V8 Engine
section_names:
- azure
- coding
primary_section: coding
---
TulikaC announces that Node.js 24 LTS is now supported on Azure App Service for Linux, enabling developers to deploy faster, modern JavaScript apps with improved testing and deployment capabilities.<!--excerpt_end-->

# Node.js 24 Now Available on Azure App Service for Linux

Node.js 24 LTS is now generally available for use on Azure App Service for Linux. Developers can create new Node.js 24 applications directly through the Azure portal, automate deployments using the Azure CLI, or leverage ARM/Bicep templates for integration into their existing workflows.

## Key Improvements in Node.js 24

- **Modern JavaScript and Performance**
  - Ships with the V8 13.6 engine and npm 11
  - Enhanced capabilities such as `RegExp.escape`, `Float16Array`, improved async context handling, support for global `URLPattern`, and extended WebAssembly memory support
  - Results in cleaner, faster code without extra polyfills
  - Designated as Long Term Support (LTS), making it a stable option for production applications

- **Better Built-in Testing Workflows**
  - The `node:test` runner now automatically waits on nested subtests
  - Fewer flaky test executions and errors related to incomplete tests
  - Eliminates the need for additional third-party test frameworks or manual await logic

## Deployment and Automation Options

- **Azure Portal**: Provision Node.js 24 apps with a graphical interface
- **Azure CLI**: Script and automate deployments and management operations
- **ARM/Bicep Templates**: Infrastructure-as-code for repeatable and scalable deployments

## Migration and Scalability

- Easily migrate your existing Node.js applications to Node.js 24 LTS on Azure App Service for Linux
- Scale and monitor your workloads with the same App Service management features
- Take full advantage of performance improvements and latest runtime support

For detailed release notes about Node.js 24, visit the [official Node.js 24 release notes](https://nodejs.org/blog/release/v24.0.0).

---

_Updated Nov 11, 2025_

**Author:** TulikaC

For additional deployment documentation and support, refer to the Azure App Service documentation.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/node-js-24-is-now-available-on-azure-app-service-for-linux/ba-p/4468801)
