---
layout: post
title: Test and Validate Functions with Develop Mode in Fabric User Data Functions
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/test-and-validate-your-functions-with-develop-mode-in-fabric-user-data-functions-preview/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-08-19 11:00:00 +00:00
permalink: /ml/news/Test-and-Validate-Functions-with-Develop-Mode-in-Fabric-User-Data-Functions
tags:
- Code Publishing
- Data Engineering
- Develop Mode
- Fabric Portal
- Function Validation
- Library Management
- Microsoft Fabric
- Preview Features
- Python
- Region Availability
- Testing
- User Data Functions
section_names:
- ml
---
Microsoft Fabric Blog announces major improvements to User Data Functions, with a new Develop mode for testing and validating functions. This update provides developers with enhanced workflows, as explained and demonstrated by the Microsoft Fabric Blog team.<!--excerpt_end-->

# Test and Validate Functions with Develop Mode in Fabric User Data Functions (Preview)

Microsoft Fabric has introduced a major update to User Data Functions, specifically targeting the development and testing experience. The new 'Develop mode' aims to simplify how developers interact with, test, and validate their functions before publishing, based on direct community feedback during the preview.

## Key Features

- **Develop mode:**
  - Accessible to users with Write permissions.
  - Offers a portal editor to view, modify, test, and publish code.
  - The new Test capability allows executing functions in a dedicated, isolated test environment for real-time outputs and logs without publishing changes prematurely.
  - Developers can see side-by-side unpublished and published code differences.

- **Run only mode:**
  - For users with Execute permissions.
  - Intended for running only the published version of functions (read-only access to published code).

- **View only mode:**
  - For users with Read permissions.
  - Grants read-only access to code and its metadata.

## How to Use Develop Mode and Test Capability

1. **Upgrade Required:**
   - Ensure your `fabric-user-data-functions` library is updated to the latest version via Library Management in the Fabric portal.
2. **Access Develop mode:**
   - Open the Functions explorer, switch to Develop mode, and select the Test icon.
3. **Testing Functions:**
   - Open a side panel and start a test session (a dedicated live Python runtime).
   - Make code changes, execute, and immediately view outputs, logs, and errors.
   - Test sessions last 15 minutes (auto-extended with each execution) and use any libraries or connections managed in your workspace.

## Important Considerations

- **Region Availability:** The Test capability is being rolled out across regions; if your Fabric Tenant region isn’t supported yet, you’ll receive an informational message. Publishing and running in 'Run only' mode remains available as before.

## How to Upgrade the Library

1. Go to Library Management in the Functions portal.
2. Locate `fabric-user-data-functions` and click the edit icon.
3. Select the latest version from the dropdown at the top. No need to republish your functions after an upgrade.

## Resources and Support

- [Develop mode documentation](https://go.microsoft.com/fwlink/?linkid=2330551)
- [User Data Functions overview](https://learn.microsoft.com/fabric/data-engineering/user-data-functions/user-data-functions-overview)
- Email: [FabricUserDataFunctionsPreview@service.microsoft.com](mailto:FabricUserDataFunctionsPreview@service.microsoft.com)

## Summary

This update enables a more flexible, developer-friendly experience for developing, testing, and publishing User Data Functions in Microsoft Fabric. Immediate feedback, safer testing, and region-based rollout are core improvements.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/test-and-validate-your-functions-with-develop-mode-in-fabric-user-data-functions-preview/)
