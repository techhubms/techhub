---
external_url: https://www.stevejgordon.co.uk/receiving-github-webhooks-when-using-the-aspnetcore-developer-certificate
title: Receiving GitHub Webhooks When Using the ASP.NET Core Developer Certificate
author: Steve Gordon
feed_name: Steve Gordon's Blog
date: 2024-09-24 09:46:37 +00:00
tags:
- .NET
- ASP.NET Core
- Certificate Management
- Developer Certificate
- GitHub
- GitHub App
- HTTPS
- Integration Testing
- Middleware
- Node.js
- OpenTelemetry
- Smee.io
- Webhooks
- DevOps
- Blogs
section_names:
- dotnet
- devops
primary_section: dotnet
---
In this detailed post, Steve Gordon shares practical steps for developing and testing GitHub webhooks locally within an ASP.NET Core app, detailing webhook setup, certificate management, and smee.io usage.<!--excerpt_end-->

# Receiving GitHub Webhooks When Using the ASP.NET Core Developer Certificate

_Authored by Steve Gordon_

## Overview

This article is part of the series "A Guide to Developing GitHub Apps on .NET." Steve Gordon details his experience and methods while experimenting with building a GitHub App using .NET. The focus of this post is on enabling the receipt and local testing of GitHub webhooks in an ASP.NET Core application, specifically how to navigate HTTPS challenges using the self-signed ASP.NET Core developer certificate.

## Introduction

For several weeks, I have been working on creating a GitHub App with .NET to realize an integration idea and keep my skills sharp. This real-world scenario utilizes ASP.NET Core features and allows me to "dogfood" my work on OpenTelemetry and .NET app instrumentation at Elastic, discover documentation gaps, and develop helpful guidance.

A primary early goal is supporting GitHub webhook receipt in my ASP.NET Core app. In the next post(s) in this series, I will cover the code for handling webhook requests efficiently and securely via middleware. This post lays essential groundwork, focusing on how to enable local testing of webhook delivery over HTTPS when developing with the ASP.NET Core developer certificate.

---

## GitHub Webhooks Basics

GitHub webhooks notify integrations by sending predetermined payloads to your app's endpoint whenever specific events occur on GitHub. A GitHub App can subscribe to a wide variety of events. For a full list, see [GitHub's documentation](https://docs.github.com/en/webhooks/webhook-events-and-payloads#pull_request).

Testing webhook delivery in a local, unexposed development environment is challenging. While you can test with fake (mock) payloads, true integration testing requires allowing GitHub to send real requests to your locally running app.

GitHub's documentation on [testing webhooks](https://docs.github.com/en/webhooks/testing-and-troubleshooting-webhooks/testing-webhooks) is recommended for deeper insights. While most steps are detailed there, this post highlights one hurdle: ASP.NET Core’s use of a self-signed HTTPS certificate in development, which can block testing if not addressed.

> **Note:** This guide is written for the Windows OS, though similar steps can be replicated on other operating systems.

---

## Prerequisites

### GitHub App Registration

- Register your GitHub App in [your account settings](https://github.com/settings/apps).
- Enter your unique smee.io URL as the webhook endpoint.
- Smee.io will forward payloads from GitHub to your localhost application.

> **Note:** For simplicity, the webhook secret is not configured here but should never be omitted in real applications—future posts will cover webhook payload validation.

### ASP.NET Core HTTPS Developer Certificate

- By default, the .NET SDK sets up a developer certificate for HTTPS.
- If not, use [`dotnet dev-certs`](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-dev-certs) to generate one.

### Smee.io URL & smee-client

- Create a new channel on [smee.io](https://smee.io), generating a unique URL.
- Install the [smee client](https://github.com/probot/smee-client) (requires Node.js and NPM).
- The smee client tunnels webhook traffic from GitHub (via smee.io) to your localhost endpoint.

### ASP.NET Core Application Scaffold

Start your app and prepare a basic middleware to receive POST requests at your webhook endpoint:

```csharp
namespace GitHubAppSample;

public class GitHubWebHookMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private const string WebHookPath = "/github/webhook";

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments(WebHookPath, StringComparison.Ordinal) &&
            context.Request.Method == "POST")
        {
            // TODO: Handle webhook
        }
        await _next(context);
    }
}
```

Register the middleware in your pipeline:

```csharp
app.UseMiddleware<GitHubWebHookMiddleware>();
```

---

## The HTTPS Challenge

Launch your app and inspect the HTTPS port (from launchSettings.json or console logs). For this example, it's `https://localhost:7192`.

Run the smee client to forward payloads:

```shell
smee --url https://smee.io/YOUR_CHANNEL --target https://localhost:7192/github/webhook
```

At webhook creation, GitHub sends a `ping` event. You can view and redeliver events in your smee.io channel.

If your breakpoint isn't hit and the Smee client logs a Node.js self-signed certificate error (`DEPTH_ZERO_SELF_SIGNED_CERT`), it's because the Node runtime cannot verify your ASP.NET Core developer certificate.

---

## Trusting the ASP.NET Core Developer Certificate for Node.js

1. **Export the Developer Certificate**:
    - Open Certificate Manager (`certmgr.msc`).
    - Find the certificate issued to `localhost` under _Personal → Certificates_.
    - Right-click → _All Tasks_ → _Export_, and follow the wizard. Export as **Base-64 encoded X.509 (.CER)**, _without_ the private key.
2. **Configure Node.js to Trust the Certificate**:
    - Temporarily set the environment variable for your terminal session:

    ```powershell
    $Env:NODE_EXTRA_CA_CERTS = "c:\certs\aspnetcore.cer"
    ```

3. **Restart the smee client**:

    ```shell
    smee --url https://smee.io/YOUR_CHANNEL --target https://localhost:7192/github/webhook
    ```

4. **Redeliver the Webhook**
    - In your smee.io UI, redeliver the ping event. Now, your breakpoint should be triggered, and the Smee client should show successful forwarding. No certificate errors will appear.

Example successful log:

```
Forwarding https://smee.io/CHANNEL to https://localhost:7192/github/webhook
Connected https://smee.io/CHANNEL
POST https://localhost:7192/github/webhook - 200
```

---

## Summary

You can deliver GitHub webhook events to your ASP.NET Core app locally via HTTPS using smee.io as a tunnel. The critical step is to export and trust the development certificate for Node.js using the `NODE_EXTRA_CA_CERTS` environment variable. This setup enables robust local integration testing before deploying your GitHub App.

Future posts will demonstrate securing and validating webhook endpoints in depth.

---

## Resources

- [Testing webhooks from GitHub](https://docs.github.com/en/webhooks/testing-and-troubleshooting-webhooks/testing-webhooks)
- [ASP.NET Core developer certificates](https://learn.microsoft.com/en-us/dotnet/core/additional-tools/self-signed-certificates-guide)
- [Authenticating a .NET GitHub App using a JSON Web Token (JWT)](https://www.stevejgordon.co.uk/authenticating-a-github-app-using-a-json-web-token-in-dotnet)
- [smee.io and smee-client](https://github.com/probot/smee-client)

---

## About the Author

**Steve Gordon** is a Pluralsight author, 7x Microsoft MVP, and .NET engineer at Elastic (maintaining the .NET APM agent and libraries). He is an active community member, writer, speaker, and OSS contributor, based in Brighton and founder of .NET South East Meetup. Follow Steve on [Twitter (X)](https://twitter.com/stevejgordon) and via [his blog](https://www.stevejgordon.co.uk).

This post appeared first on "Steve Gordon's Blog". [Read the entire article here](https://www.stevejgordon.co.uk/receiving-github-webhooks-when-using-the-aspnetcore-developer-certificate)
