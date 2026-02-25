---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-to-troubleshoot-azure-functions-not-visible-in-azure-portal/ba-p/4495873
title: How to Troubleshoot Azure Functions Not Visible in Azure Portal
author: vikasgupta5
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-02-23 04:33:16 +00:00
tags:
- Application Insights
- Azure
- Azure CLI
- Azure Functions
- Azure Portal
- AzureWebJobsStorage
- Community
- Deployment
- Diagnose And Solve
- Extension Bundles
- Function App
- Function.json
- GitHub Actions
- Host.json
- Kudu
- Networking
- Serverless
- Storage Account
- Troubleshooting
- V2 Programming Model
- WEBSITE RUN FROM PACKAGE
section_names:
- azure
---
vikasgupta5 provides a thorough troubleshooting checklist and detailed solutions for when Azure Functions are not showing in the Azure Portal, helping developers resolve deployment, configuration, and infrastructure issues.<!--excerpt_end-->

# How to Troubleshoot Azure Functions Not Visible in Azure Portal

## Overview

Azure Functions allows you to run event-driven code without managing infrastructure. Sometimes, after deploying your functions, they don't show up in the Azure Portal—even when they're running. This guide covers common causes for this issue and provides actionable troubleshooting steps.

## How Functions Appear in the Portal

When you access your Function App in the Azure Portal, the portal:

1. Checks host status at `/admin/host/status`
2. Enumerates functions from the runtime
3. Gets metadata (trigger type, bindings, configuration)
4. Renders functions in the UI

If any step fails, your functions may not appear. Key files for discovery include `host.json`, `function.json`, compiled DLLs (for .NET), and `extensions.json`.

## Main Issue Categories

- **Deployment**: Incomplete or failed deployments
- **Configuration**: Invalid `function.json` or binding errors
- **Host/Runtime**: Startup failure, runtime errors
- **Storage**: Issues with `AzureWebJobsStorage` connectivity
- **Portal/Sync**: Metadata cache or trigger sync failures
- **Networking**: VNet, private endpoint, or firewall problems

## Common Causes and Solutions

### 1. Function App Host Not Running

- **Symptoms**: No functions visible, host error in portal.
- **How to Check**: Open `/admin/host/status` (see [Admin API](https://github.com/Azure/azure-functions-host/wiki/Admin-API)).
- **Solutions**:
  - Fix invalid/missing Application Settings (like `AzureWebJobsStorage`, `FUNCTIONS_EXTENSION_VERSION`)
  - Check for required extension bundles in `host.json`
  - Use 'Diagnose and solve problems' in Portal

### 2. Deployment Issues

- **Symptoms**: Functions present locally but missing in portal; only some functions visible; old versions remain.
- **How to Check**: Use Kudu's Debug Console to inspect `site/wwwroot` for expected files/folders.
- **Solutions**:
  - Redeploy using preferred method (VS, Azure CLI, GitHub Actions, ZIP deploy)
  - Restart Function App (e.g., via `az functionapp restart`)

### 3. Invalid or Missing `function.json`

- **Symptoms**: Some functions missing or have wrong triggers.
- **How to Check**: Validate `function.json` in Kudu, check build output, ensure correct syntax.
- **Solution**: Fix any syntax or binding errors, rebuild if necessary.

### 4. V2 Programming Model Issues (Python/Node.js)

- **Symptoms**: Functions defined in code but not visible in portal; no `function.json` files.
- **Cause**: Python V2 and Node.js V4 models rely on code decorators, requiring the host to be running for discovery.
- **Solutions**:
  - Verify host is running
  - Check entry configurations and Application Insights for errors
  - Review function directory structure

### 5. Extension Bundle or Dependency Issues

- **Symptoms**: Functions using certain triggers not appearing; extension errors; works locally but not in Azure.
- **Solution**: Update or add proper extension bundle reference in `host.json` and ensure compatible versions.

### 6. Sync Trigger Issues

- **Symptoms**: Portal not updating function list after deployment.
- **Solutions**:
  - Use Refresh in Portal; or Restart the app.
  - Trigger sync via REST API (see [docs](https://learn.microsoft.com/azure/azure-functions/functions-deployment-technologies?tabs=linux#trigger-syncing)).

### 7. Storage Account Connectivity Issues

- **Symptoms**: Host/storage errors, unable to get keys, functions invisible.
- **Solution**: Validate storage account access, check connection strings, update if rotated, confirm correct networking settings.

### 8. WEBSITE_RUN_FROM_PACKAGE Problems

- **Symptoms**: Functions disappear after deployment; read-only errors; missing files.
- **Solution**: Ensure deployment package (.zip) is accessible, not expired, and has correct root structure. Adjust networking/firewall as needed.

### 9. Misconfigured Filtering

- **Symptoms**: Only some functions visible.
- **Solution**: Confirm `host.json` `functions` array instead lists all desired functions, or remove the filter.

### 10. Networking Configuration

- **Symptoms**: Portal can't reach admin APIs; intermittent visibility; timeouts.
- **Solution**: Modify access restrictions, ensure portal can reach app if functions are to be managed via Portal. Recognize expected behavior for private apps.

## Troubleshooting Checklist

- Confirm host is running
- Verify files present via Kudu
- Check `function.json` syntax
- Validate package settings (`WEBSITE_RUN_FROM_PACKAGE`)
- Extension bundle in `host.json`
- Storage connection
- No undesired filters
- Portal metadata sync
- App/network accessibility

## Verification via REST API

- List functions: `curl "https://<app>.azurewebsites.net/admin/functions?code=<master-key>"`
- Get host status: `curl "https://<app>.azurewebsites.net/admin/host/status?code=<master-key>"`
- If API shows functions but portal does not, focus on portal sync/caching.

## Conclusion

By systematically checking configuration, deployment, host, storage, and network, you can resolve nearly all issues where Azure Functions are not displayed in the portal. Use built-in diagnostics, consult official documentation, and consider opening a Microsoft support ticket if all else fails.

## References

- [Azure Functions host.json reference](https://learn.microsoft.com/azure/azure-functions/functions-host-json)
- [Azure Functions deployment technologies](https://learn.microsoft.com/azure/azure-functions/functions-deployment-technologies)
- [Troubleshoot Azure Functions](https://learn.microsoft.com/azure/azure-functions/functions-recover-storage-account)
- [Python V2 programming model](https://learn.microsoft.com/azure/azure-functions/functions-reference-python?tabs=asgi%2Capplication-level&pivots=python-mode-decorators)
- [Node.js V4 programming model](https://learn.microsoft.com/azure/azure-functions/functions-reference-node?tabs=javascript%2Cwindows%2Cazure-cli&pivots=nodejs-model-v4)
- [Azure Functions diagnostics](https://learn.microsoft.com/azure/azure-functions/functions-diagnostics)
- [Azure Functions networking options](https://learn.microsoft.com/azure/azure-functions/functions-networking-options)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/how-to-troubleshoot-azure-functions-not-visible-in-azure-portal/ba-p/4495873)
