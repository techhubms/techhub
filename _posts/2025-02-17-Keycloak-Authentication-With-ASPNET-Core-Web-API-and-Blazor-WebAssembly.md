---
layout: "post"
title: "Keycloak Authentication with ASP.NET Core Web API and Blazor WebAssembly"
description: "This article by Marinko Spasojević provides a comprehensive guide on integrating Keycloak authentication with a Blazor WebAssembly frontend and ASP.NET Core Web API backend, covering setup, configuration, secure user authentication, and role-based authorization using .NET technologies."
author: "Marinko Spasojević"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code-maze.com/keycloak-authentication-with-asp-net-core-web-api-and-blazor-webassembly/"
viewing_mode: "external"
feed_name: "Code Maze Blog"
feed_url: "https://code-maze.com/feed/"
date: 2025-02-17 07:59:06 +00:00
permalink: "/2025-02-17-Keycloak-Authentication-with-ASPNET-Core-Web-API-and-Blazor-WebAssembly.html"
categories: ["Coding", "Security"]
tags: [".NET", "ASP.NET Core", "Asp.net Core Web API", "Authentication", "Authorization", "Blazor WebAssembly", "Coding", "Docker", "HttpClientFactory", "Identity And Access Management", "JWT", "Keycloak", "Microsoft.AspNetCore.Authentication.JwtBearer", "Microsoft.AspNetCore.Components.WebAssembly.Authentication", "OAuth2", "OpenID Connect", "Posts", "Security", "Web API"]
tags_normalized: ["dotnet", "aspdotnet core", "aspdotnet core web api", "authentication", "authorization", "blazor webassembly", "coding", "docker", "httpclientfactory", "identity and access management", "jwt", "keycloak", "microsoftdotaspnetcoredotauthenticationdotjwtbearer", "microsoftdotaspnetcoredotcomponentsdotwebassemblydotauthentication", "oauth2", "openid connect", "posts", "security", "web api"]
---

In this article, Marinko Spasojević details how to integrate Keycloak authentication with both a Blazor WebAssembly client application and an ASP.NET Core Web API backend, providing step-by-step guidance and sample configurations.<!--excerpt_end-->

# Keycloak Authentication with ASP.NET Core Web API and Blazor WebAssembly

**Author:** Marinko Spasojević  
**Published on Code Maze**

---

## Overview

This article explores how to integrate Keycloak—a popular open-source identity and access management solution—with a Blazor WebAssembly (WASM) frontend and an ASP.NET Core Web API backend. The goal is to establish secure authentication and authorization for users across both applications, leveraging industry-standard protocols such as OpenID Connect and OAuth2.

## Table of Contents

1. [Introduction](#introduction)
2. [Installing Keycloak with Docker](#installing-keycloak-with-docker)
3. [Setting up Keycloak Realms, Clients, and Users](#setting-up-keycloak-realms-clients-and-users)
4. [Configuring the ASP.NET Core Web API](#configuring-the-aspnet-core-web-api)
5. [Blazor WebAssembly Configuration](#blazor-webassembly-configuration)
6. [Testing the Integration](#testing-the-integration)
7. [Inspecting JWT Tokens](#inspecting-jwt-tokens)
8. [Conclusion](#conclusion)

---

## Introduction

Integrating Keycloak into your applications offers secure user authentication and authorization. In this scenario, a Blazor WebAssembly client will interact with a .NET Web API, both protected using Keycloak IAM.

Keycloak supports OpenID Connect and OAuth2, automating authentication, authorization, and user management.

## Installing Keycloak with Docker

Ensure Docker Desktop is installed. To run Keycloak in a container, execute:

```shell
docker run -p 8080:8080 -e KC_BOOTSTRAP_ADMIN_USERNAME=admin -e KC_BOOTSTRAP_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:26.1.1 start-dev
```

- This pulls and runs the Keycloak image, exposing port 8080.
- The Keycloak admin console will be available at `http://localhost:8080/admin` (login with admin/admin).

## Setting up Keycloak Realms, Clients, and Users

### 1. Creating a Realm

- In Keycloak, a _realm_ manages users, roles, and clients in an isolated space.
- After logging in, click the _master_ realm, then select **Create realm**.
- Name it, e.g., `BlazorWebApiRealm` and confirm.

### 2. Registering Clients

#### a) Web API Client

- Go to **Clients** > **Create client**.
- Use these settings:
  - Client type: _OpenID Connect_
  - Client ID: `web-api`
  - Client Authentication: _Off_
  - Authorization: _Off_
  - Standard Flow Enabled: _Off_
  - Direct Access Grants Enabled: _Off_
  - URLs: leave blank
- Click **Save**.

#### b) Audience Mapping for API

- Go to **Client scopes** > **Create client scope**.
- Name: `blazor_api_scope`, provide a description.
- Enable **Include in token scope**.
- Save.
- In the new scope, open **Mappers**, click **Configure new mapper**.
- Mapper type: _Audience_; name it (e.g., `blazor_api audience`).
- Select `web-api` for _Included Client Audience_.
- Save.

#### c) Blazor WASM Client

- Back in **Clients** > **Create client**:
  - Client type: _OpenID Connect_
  - Client ID: `blazor-client`
  - Client Type: _Public_
  - Standard Flow only
  - Valid Redirect URIs: `https://localhost:5000/authentication/login-callback`
  - Logout Redirect: `https://localhost:5000/authentication/logout-callback`
  - Save.
  - On the scope tab, add the previously created `blazor_api_scope` as optional.

### 3. Creating a User

- Go to **Users** > **Create new user**.
- Fill in username, email, first and last name; save.
- Go to **Credentials**, set password (toggle _Temporary_ to off).

## Configuring the ASP.NET Core Web API

1. **Install Package:**
   - `Microsoft.AspNetCore.Authentication.JwtBearer`
2. **In `Program.cs`:**

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(policy => {
    policy.AddPolicy("CorsPolicy", opt => opt
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.Authority = "http://localhost:8080/realms/BlazorWebApiRealm";
        options.Audience = "web-api";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

**Key Points:**

- JWT Bearer is used to validate tokens from Keycloak.
- `options.Authority` and `options.Audience` must match Keycloak setup.
- For local development, HTTPS is not required, but it should be in production.

## Blazor WebAssembly Configuration

1. **Ensure Package Installed:**
   - `Microsoft.AspNetCore.Components.WebAssembly.Authentication`

2. **In `Program.cs`:**

```csharp
builder.Services.AddOidcAuthentication(options => {
    options.ProviderOptions.Authority = "http://localhost:8080/realms/BlazorWebApiRealm";
    options.ProviderOptions.ClientId = "blazor-client";
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.DefaultScopes.Add("blazor_api_scope");
});
```

1. **Configure HTTP Client:**

```csharp
builder.Services.AddHttpClient("WeatherAPI", client =>
    client.BaseAddress = new Uri("https://localhost:5001/"))
    .AddHttpMessageHandler(sp => {
        var handler = sp.GetRequiredService<AuthorizationMessageHandler>()
            .ConfigureHandler(authorizedUrls: ["https://localhost:5001"]);
        return handler;
    });
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("WeatherAPI"));
```

- This ensures secure calls to the API, with tokens added automatically.

1. **Securing a Page:**

In the Weather page:

```razor
@page "/weather"
@inject HttpClient Http
@using Microsoft.AspNetCore.Authorization
@attribute [Authorize]
```

- Only authorized users can access this page.

## Testing the Integration

- Run both the API and Blazor WASM applications.
- On the Blazor app, click the Login link—authenticate using your Keycloak user.
- Successfully authenticated users will see protected content (such as weather data).
- Log out to end the session.

## Authorizing the Web API

- Add the `[Authorize]` attribute to your controller methods, e.g.:

```csharp
[HttpGet]
[Authorize]
public IEnumerable<WeatherForecast> Get() { ... }
```

- Now, only authenticated requests will work; otherwise, they will be denied.

## Inspecting JWT Tokens

- Open browser Dev Tools → Applications → Session Storage.
- Find the "oidc" key and copy the access token value.
- Paste it at [jwt.io](https://jwt.io/) to inspect claims such as `iss` (issuer) and `aud` (audience)—these should match your Keycloak configuration.

**Example Token Claims:**

```json
{
  "exp": 1739446851,
  "iat": 1739446551,
  "auth_time": 1739446549,
  "jti": "c1e7e9a7-8bf0-4766-b694-7105c6894a02",
  "iss": "http://localhost:8080/realms/BlazorWebApiRealm",
  "aud": ["web-api", "account"],
  ...
}
```

## Conclusion

Integrating Keycloak with Blazor WebAssembly and ASP.NET Core Web API empowers developers to deliver scalable, secure, and standards-based authentication and authorization for their applications. The approach leverages JWT validation, OpenID Connect flows, and .NET's authentication middleware.

For extensions such as role-based authorization, refer to [how to use roles with Keycloak, Web API, and Blazor WASM](https://code-maze.com/keycloak-roles-blazor-webassembly-web-api/).

---

*This article is adapted for technical reference and is authored by Marinko Spasojević on Code Maze.*

This post appeared first on "Code Maze Blog". [Read the entire article here](https://code-maze.com/keycloak-authentication-with-asp-net-core-web-api-and-blazor-webassembly/)
