---
layout: "post"
title: "AspNetCore.SecurityKey: API Key Authentication for ASP.NET Core Applications"
description: "AspNetCore.SecurityKey is a lightweight library offering flexible API key authentication for ASP.NET Core. It supports various input sources, integration patterns, and OpenAPI docs, and allows extensive custom validation. This article explores its features, installation, usage patterns, and advanced customization capabilities."
author: "pwelter34"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/csharp/comments/1mex14a/aspnetcoresecuritykey_security_api_key/"
viewing_mode: "external"
feed_name: "Reddit CSharp"
feed_url: "https://www.reddit.com/r/csharp/.rss"
date: 2025-08-01 13:50:31 +00:00
permalink: "/2025-08-01-AspNetCoreSecurityKey-API-Key-Authentication-for-ASPNET-Core-Applications.html"
categories: ["Coding", "Security"]
tags: ["API Key Authentication", "ASP.NET Core", "Attribute Based Security", "C#", "Coding", "Community", "Custom Validation", "ISecurityKeyValidator", "Middleware", "Minimal APIs", "NuGet Package", "OpenAPI", "Security", "Swagger"]
tags_normalized: ["api key authentication", "asp dot net core", "attribute based security", "c", "coding", "community", "custom validation", "isecuritykeyvalidator", "middleware", "minimal apis", "nuget package", "openapi", "security", "swagger"]
---

Authored by pwelter34, this article introduces AspNetCore.SecurityKey, a robust API key authentication library for ASP.NET Core. It examines installation steps, configuration options, and advanced customization for securing .NET applications.<!--excerpt_end-->

## Overview

**AspNetCore.SecurityKey** is an open-source library designed to provide a complete and flexible API key authentication solution for ASP.NET Core applications. The library integrates with the existing ASP.NET Core authentication and authorization system and supports multiple input methods, making it adaptable for various scenarios.

### Key Features

- **Multiple Input Sources:** API keys via headers, query parameters, or cookies.
- **Flexible Authentication:** Integrates with ASP.NET Core authentication or works as standalone middleware.
- **Extensible Design:** Supports custom validation and extraction logic.
- **Rich Integration:** Use controller attributes, middleware, or minimal API support.
- **OpenAPI Support:** Automatically generates Swagger/OpenAPI documentation (.NET 9+).
- **Minimal Overhead:** High performance with optional caching.
- **Multiple Deployment Patterns:** Choose from attribute-based, middleware, or endpoint filters.

## Getting Started

1. **Package Installation:**
   - Install via NuGet:

     ```sh
     dotnet add package AspNetCore.SecurityKey
     ```

2. **Configure the API Key:**
   - In `appsettings.json`:

     ```json
     { "SecurityKey": "your-secret-api-key-here" }
     ```

3. **Register Services & Secure Endpoints:**

   ```csharp
   builder.Services.AddSecurityKey();
   app.UseSecurityKey();
   ```

4. **Call the API:**

   ```sh
   curl -H "X-API-KEY: your-secret-api-key-here" https://yourapi.com/endpoint
   ```

### NuGet & CLI Options

- [NuGet package](https://www.nuget.org/packages/AspNetCore.SecurityKey/)
- Install using Package Manager Console or .NET CLI.

## Passing API Keys

AspNetCore.SecurityKey allows clients to pass API keys in different ways:

- **Headers (Recommended):** Secure and common for API-to-API communication.
- **Query Parameters:** Useful for simple scenarios; less secure.
- **Cookies:** Suitable for browser-based applications or persistent usage.

**Security Note:** Query params can leak keys via logs and browser history; headers are preferred for production.

## Configuration Details

- Configure one or more valid API keys in `appsettings.json`. To support multiple keys, separate them with semicolons.

  ```json
  { "SecurityKey": "key1;key2;key3" }
  ```

## Usage Patterns

AspNetCore.SecurityKey supports a variety of integration methods:

1. **Middleware Pattern:** Apply globally to all endpoints for comprehensive security.
2. **Attribute Pattern:** Secure specific controllers or actions using `[SecurityKey]` attributes.
3. **Endpoint Filter Pattern:** For minimal APIs, attach directly to chosen endpoints or groups.
4. **Authentication Scheme Pattern:** Integrate with ASP.NET Core's authentication and use with roles or policies.

### Example: Attribute-Based Security

```csharp
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase {
    [SecurityKey]
    [HttpGet]
    public IEnumerable GetUsers() => UserService.GetUsers();

    [HttpGet("public")]
    public IEnumerable GetPublicUsers() => UserService.GetPublicUsers();
}
```

## Advanced Customization

Users can implement custom validation logic by providing their own `ISecurityKeyValidator`. For example, one could verify API keys against a database, check expiration, log usage, and populate claims for authenticated users.

### Sample Custom Validator

```csharp
public class DatabaseSecurityKeyValidator : ISecurityKeyValidator {
    private readonly IApiKeyRepository _repository;
    private readonly ILogger _logger;
    // ... Constructor and implementation ...
}
```

Register your custom implementation with dependency injection for full control.

## License

AspNetCore.SecurityKey is distributed under the MIT License.

This post appeared first on "Reddit CSharp". [Read the entire article here](https://www.reddit.com/r/csharp/comments/1mex14a/aspnetcoresecuritykey_security_api_key/)
