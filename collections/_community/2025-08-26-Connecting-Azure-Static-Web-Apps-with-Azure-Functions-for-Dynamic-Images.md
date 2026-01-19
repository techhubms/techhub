---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure/what-s-the-secret-sauce-for-getting-functions-api-to-work-with/m-p/4448430#M1359
title: Connecting Azure Static Web Apps with Azure Functions for Dynamic Images
author: fcc_archivist
viewing_mode: external
feed_name: Microsoft Tech Community
date: 2025-08-26 21:18:41 +00:00
tags:
- .NET
- API Integration
- Azure Functions
- Azure Static Web Apps
- Backend Integration
- C#
- CORS
- Function Apps
- HTTP Triggers
- Image Generation
- Local Development
- SkiaSharp
- Staticwebapp.config.json
section_names:
- azure
- coding
---
fcc_archivist describes the challenge of connecting an Azure Static Web App with an Azure Functions API that generates images dynamically in C#. The guide covers typical pitfalls and configuration requirements.<!--excerpt_end-->

# Connecting Azure Static Web Apps with Azure Functions for Dynamic Images

**Author: fcc_archivist**

Many new Azure developers run into the challenge of integrating serverless APIs (like Azure Functions) with a static front-end web app, especially when trying to access dynamically generated assets. This guide explains the problem and walks through the critical configuration steps to make it work.

## Scenario

- **Frontend:** Azure Static Web Apps (localhost:7154)
- **Backend/API:** Azure Functions (localhost:7071) generates PNG images asynchronously using SkiaSharp
- **Goal:** Embed a dynamic image (e.g., `<img src="/api/image/123">`) in your site hosted on the static web app, with the image content generated live by your Azure Function

## The Issue

Your static web page tries to access `/api/image/123`, expecting the backend to serve it. When running locally, you get a **404 Not Found** because the frontend at port 7154 doesn't forward requests to port 7071 where your Azure Functions are running. This is a common issue during local development.

## Backend: Example Azure Function Code

```csharp
[Function("GenerateImage")]
public HttpResponseData Run(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "image/{id}")] HttpRequestData req, string id)
{
    // ...generate image using SkiaSharp...
}
```

## Frontend: Static Web App

Your site contains:

```html
<img src="/api/image/123" alt="Generated Image">
```

## Configuration: staticwebapp.config.json

```json
{
  "routes": [
    { "route": "/api/*", "allowedRoles": ["anonymous"] }
  ],
  "navigationFallback": {
    "rewrite": "/index.html",
    "exclude": ["/api/*"]
  }
}
```

This tells Azure Static Web Apps to pass `/api/*` requests straight to the Functions backend, not to the static site's HTML.

## Local Development: The Missing Link

**Problem:** When running locally, your static site and Azure Functions are on different ports, so browser requests don't reach the backend automatically.

### Solution: Use the Azure Static Web Apps CLI

The Azure Static Web Apps CLI (`swa`) can proxy API requests to the Functions backend for you. This way, the front-end site and API share a single origin, and CORS/routing issues are handled automatically.

#### Steps

1. **Install the CLI:**

   ```sh
   npm install -g @azure/static-web-apps-cli
   ```

2. **Start the Functions API locally:**

   ```sh
   func start
   ```

3. **Run the Static Web App with the CLI, linking to your API:**

   ```sh
   swa start http://localhost:YOUR-STATIC-PORT --api-location http://localhost:7071
   ```

   Replace `YOUR-STATIC-PORT` with your frontend port (e.g., 3000 for React). The CLI proxies `/api` requests to Functions.

4. **Visit:**
   - `http://localhost:4280` (default) to see your app working with dynamic image endpoints.
   - The `<img src="/api/image/123">` tag should now work as expected.

### Without the CLI: Manually Use Full URLs

If you're not using the CLI, hardcode the full backend URL (not recommended for production):

```html
<img src="http://localhost:7071/api/image/123" alt="Generated Image">
```

But you'll hit [CORS](https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS) issues that the CLI helps avoid.

## Deployment to Azure: It Just Works

When you deploy both static and Functions code together with Azure Static Web Apps, Azure automatically glues them together under a single https://<SWA_SITE>.azurestaticapps.net location. Your `/api/*` paths will just work with the configuration you showed.

## Troubleshooting Checklist

- Double check all backend endpoints work directly (e.g., http://localhost:7071/api/image/123).
- Confirm your staticwebapp.config.json matches your deployed folders and routes.
- During local development, use the SWA CLI for a seamless dev experience.

## References

- [Azure Static Web Apps Documentation](https://learn.microsoft.com/en-us/azure/static-web-apps/overview)
- [Azure Functions Documentation](https://learn.microsoft.com/en-us/azure/azure-functions/)
- [SWA CLI Docs](https://learn.microsoft.com/en-us/azure/static-web-apps/cli)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure/what-s-the-secret-sauce-for-getting-functions-api-to-work-with/m-p/4448430#M1359)
