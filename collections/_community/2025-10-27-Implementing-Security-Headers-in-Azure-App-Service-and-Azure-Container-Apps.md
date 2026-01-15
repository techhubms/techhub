---
layout: post
title: Implementing Security Headers in Azure App Service and Azure Container Apps
author: AmritpalSinghNaroo
canonical_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/implementing-security-headers-in-azure-app-service-and-azure/ba-p/4464250
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-10-27 07:52:59 +00:00
permalink: /azure/community/Implementing-Security-Headers-in-Azure-App-Service-and-Azure-Container-Apps
tags:
- .NET
- App Security
- Application Gateway
- Azure
- Azure App Service
- Azure Container Apps
- Azure Front Door
- Community
- Configuration
- Content Security Policy
- Express
- HTTP Headers
- Middleware
- Nginx
- Node.js
- Permissions Policy
- Referrer Policy
- Security
- Security Headers
- Strict Transport Security
- Web.config
- X Content Type Options
- X Frame Options
section_names:
- azure
- security
---
AmritpalSinghNaroo presents a hands-on guide to implementing vital security headers in Azure App Service and Azure Container Apps, including code examples and best practices for securing web applications.<!--excerpt_end-->

# Implementing Security Headers in Azure App Service and Azure Container Apps

Security headers play a pivotal role in strengthening your web application's defense against threats like cross-site scripting (XSS), clickjacking, and unintended data exposure. While Azure provides a robust hosting environment, it's up to developers to apply and configure these headers for enhanced security.

## Why Security Headers Are Important

- **Prevent vulnerabilities**: Security headers guide browsers in ways to interact with your site safely, blocking malicious behaviors.
- **Protection examples**:
  - XSS: Blocked by Content-Security-Policy
  - Clickjacking: Blocked by X-Frame-Options
  - Protocol security: Enforced via Strict-Transport-Security

## Core Security Headers to Implement

| Header                      | Purpose                                   | Example Value                                |
|-----------------------------|-------------------------------------------|----------------------------------------------|
| Content-Security-Policy     | Mitigate XSS attacks                      | `default-src 'self';`                        |
| X-Content-Type-Options      | Prevent MIME type sniffing                 | `nosniff`                                    |
| X-Frame-Options             | Control iframe embedding (clickjacking)    | `DENY`                                       |
| Strict-Transport-Security   | Enforce HTTPS                             | `max-age=31536000; includeSubDomains`        |
| Referrer-Policy             | Limit referrer info                        | `no-referrer`                                |
| Permissions-Policy          | Restrict browser features                  | `geolocation=(), camera=()`                  |

## Approaches to Adding Security Headers

### 1. Web.config (for .NET / Windows-based App Service)

Modify your `web.config` file to add custom headers:

```xml
<system.webServer>
  <httpProtocol>
    <customHeaders>
      <add name="X-Content-Type-Options" value="nosniff" />
      <add name="X-Frame-Options" value="DENY" />
      <add name="Strict-Transport-Security" value="max-age=31536000; includeSubDomains" />
      <add name="Content-Security-Policy" value="default-src 'self'" />
    </customHeaders>
  </httpProtocol>
</system.webServer>
```

### 2. Application-Level Headers (Linux, Node.js, Python, Java, PHP)

- For **Node.js (Express)**:

  ```js
  const helmet = require('helmet');
  app.use(helmet());
  ```

- For other languages: Use corresponding security middleware or framework support.

### 3. Reverse Proxy Headers (Nginx, Apache inside container)

Configure security headers in your proxy configuration.

- **Nginx example:**

  ```nginx
  add_header X-Content-Type-Options "nosniff";
  add_header X-Frame-Options "DENY";
  add_header Strict-Transport-Security "max-age=31536000; includeSubDomains";
  add_header Content-Security-Policy "default-src 'self'";
  ```

For more: [How to set Nginx headers](https://azureossd.github.io/2023/02/24/how-to-modify-nginx-headers/)

### 4. Azure Front Door / Application Gateway (Edge Services)

Use Azure's edge services to inject headers via the Rules Engine.

- [Add HSTS header with Azure Application Gateway](https://learn.microsoft.com/en-us/azure/application-gateway/hsts-http-headers-portal)
- [Add security headers using Azure Front Door Rules Engine](https://learn.microsoft.com/en-us/azure/frontdoor/front-door-security-headers)

## Summary

Adding security headers is a simple but crucial step to safeguard your Azure-hosted applications. Whether you choose configuration files, middleware, container settings, or edge rules, make it a routine practice for modern web development.

## References & Further Reading

- [App Service Linux security FAQs](https://azureossd.github.io/2023/02/28/security-faqs-app-service-linux/)
- [Editing Response Headers on Linux App Service](https://azureossd.github.io/2022/05/25/Editing-Response-Headers-on-Linux-App-Service/)
- [Configure a PHP App - Azure App Service (Microsoft Learn)](https://learn.microsoft.com/en-us/azure/app-service/configure-language-php?pivots=platform-linux)
- [Configure Node.js Apps - Azure App Service (Microsoft Learn)](https://learn.microsoft.com/en-us/azure/app-service/configure-language-nodejs?pivots=platform-linux)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/implementing-security-headers-in-azure-app-service-and-azure/ba-p/4464250)
