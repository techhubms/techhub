---
external_url: https://www.reddit.com/r/dotnet/comments/1mhjloq/deployment_to_iis/
title: Troubleshooting IIS Deployment for Angular 11 and .NET Framework 4.7 Applications
author: Defiant_Priority_801
feed_name: Reddit DotNet
date: 2025-08-04 17:20:05 +00:00
tags:
- .NET
- .NET Framework 4.7
- 404 Error
- Angular 11
- Backend Deployment
- Frontend Deployment
- IIS Deployment
- MSBuild
- Production Build
- Site Configuration
- Web.config
section_names:
- coding
- devops
primary_section: coding
---
Defiant_Priority_801 outlines challenges encountered while deploying an Angular 11 and .NET Framework 4.7 application stack to IIS, particularly struggling with 404 errors after following standard deployment procedures.<!--excerpt_end-->

Defiant_Priority_801 is seeking assistance with deploying a full-stack application—with an Angular 11 frontend and a .NET Framework 4.7 backend—to Internet Information Services (IIS). While both parts of the application run as expected locally, deployment to IIS results in a StatusCode 404 (Not Found) when attempting to access the site.

## Stack Details

- **Frontend:** Angular 11
- **Backend:** .NET Framework 4.7

## Deployment Steps

1. **IIS Site Setup:**
   - Separate IIS sites are configured for the frontend and backend.
2. **Angular Frontend:**
   - Built for production using `yarn build --prod`.
   - The `/dist` directory produced by the build is copied to `c:\inetpub\wwwroot\frontend`.
3. **Backend:**
   - Published using `msbuild publish`, targeting the backend site folder.
4. **Configuration:**
   - Each IIS site contains its own `web.config` file.

## Problem Encountered

When visiting the assigned server IP (e.g., <http://SERVERIP>), a 404 Not Found error is shown instead of the expected application pages.

## Possible Issues and Best Practices

- **Site Bindings:** Ensure IIS bindings are set correctly, and that the hostname or IP addresses match requests.
- **Physical Path:** Verify the physical paths for both frontend and backend directories point to the deployed code.
- **web.config Files:** Confirm that the `web.config` files are correctly set up for Angular (potential URL rewrite for client-side routing) and for .NET backend configuration.
- **App Pool Configuration:** Double check app pool settings, ensuring .NET CLR version compatibility for the backend site.
- **Static File Serving:** Make sure IIS is configured to serve static files for Angular and proper handlers are installed.
- **Firewall/Permissions:** Validate that there aren’t firewall or NTFS permission issues preventing file access.
- **Port and URL Matching:** Confirm that access URLs correspond to the correct site bindings and content roots.
- **Network Access:** Check that the server’s firewall is not blocking inbound HTTP requests to the sites.

## Seeking Community Advice

The author invites advice and recommendations from the community for optimal deployment practices tailored to Angular and .NET Framework applications running on IIS, as well as troubleshooting tips for resolving the persistent 404 errors.

[Reddit Post Link](https://www.reddit.com/r/dotnet/comments/1mhjloq/deployment_to_iis/)

This post appeared first on "Reddit DotNet". [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1mhjloq/deployment_to_iis/)
