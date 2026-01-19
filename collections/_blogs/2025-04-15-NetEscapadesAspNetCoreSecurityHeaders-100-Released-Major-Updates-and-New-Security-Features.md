---
layout: post
title: 'NetEscapades.AspNetCore.SecurityHeaders 1.0.0 Released: Major Updates and New Security Features'
author: Andrew Lock
canonical_url: https://andrewlock.net/netescapades-aspnetcore-securityheaders-1-0-0-released/
viewing_mode: external
feed_name: Andrew Lock's Blog
feed_url: https://andrewlock.net/rss.xml
date: 2025-04-15 09:00:00 +00:00
permalink: /coding/blogs/NetEscapadesAspNetCoreSecurityHeaders-100-Released-Major-Updates-and-New-Security-Features
tags:
- .NET Core
- API Security
- ASP.NET Core
- Content Security Policy
- HTTP Headers
- Middleware
- NetEscapades.AspNetCore.SecurityHeaders
- Nonce Generation
- OWASP
- Permissions Policy
- Security Headers
- Trusted Types
section_names:
- coding
- security
---
Andrew Lock introduces NetEscapades.AspNetCore.SecurityHeaders 1.0.0, outlining extensive new features, updates, and best practices for integrating enhanced security headers in ASP.NET Core applications.<!--excerpt_end-->

## NetEscapades.AspNetCore.SecurityHeaders 1.0.0 Released: Major Updates and New Security Features

**Author:** Andrew Lock

### Introduction

Andrew Lock announces the release of the 1.0.0 version of [NetEscapades.AspNetCore.SecurityHeaders](https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders), a NuGet package designed to simplify the addition and management of HTTP security headers in ASP.NET Core applications. This major release brings longstanding feature requests, updates for target frameworks, the removal or obsoletion of several old behaviors, and enhanced customization options.

---

### Overview & Structure

- What are security headers?
- Adding security headers to your app
- Major changes in `1.0.0`
- Changes to the supported frameworks
- Changes to headers and defaults
- Obsolete and deprecated headers
- New features: PermissionsPolicy, Trusted Types, and API security headers
- Applying different headers to different endpoints
- Advanced customization
- Changes to nonce generation
- Updates to build provenance
- Summary

---

### What are Security Headers?

Security headers are HTTP response headers that instruct browsers to enable or disable certain features to reduce a web application's attack surface. Examples include Strict-Transport-Security, Permissions-Policy, and Content-Security-Policy. Applying these headers helps harden applications and mitigate common security risks. The sheer number and changing nature of these headers, combined with subtle syntax differences, can make reliable manual configuration difficult.

NetEscapades.AspNetCore.SecurityHeaders provides a structured, fluent interface to adopt recommended security headers and easily customize policies within ASP.NET Core.

---

### Adding Security Headers to Your ASP.NET Core App

- Install the package:

  ```bash
  dotnet add package NetEscapades.AspNetCore.SecurityHeaders
  ```

- Optionally update your .csproj:

  ```xml
  <PackageReference Include="NetEscapades.AspNetCore.SecurityHeaders" Version="1.0.0" />
  ```

- Add middleware early in your pipeline:

  ```csharp
  app.UseSecurityHeaders();
  ```

- By default, adds headers such as:
  - X-Content-Type-Options: nosniff
  - X-Frame-Options: Deny
  - Referrer-Policy: strict-origin-when-cross-origin
  - Content-Security-Policy: object-src 'none'; form-action 'self'; frame-ancestors 'none'
  - Strict-Transport-Security: max-age=31536000; includeSubDomains *(HTTPS only)*
  - Cross-Origin-Opener-Policy: same-origin
  - Cross-Origin-Embedder-Policy: credentialless
  - Cross-Origin-Resource-Policy: same-origin

The defaults are chosen for broad compatibility and sensible security based on [OWASP guidance](https://cheatsheetseries.owasp.org/cheatsheets/HTTP_Headers_Cheat_Sheet.html#security-headers). Further customization is possible using `HeaderPolicyCollection` and its fluent API.

---

### Major Changes in Version 1.0.0

#### 1. Supported Frameworks

- Dropped support for .NET Framework and netstandard2.0.
- Supported TFMs: .NET Core 3.1+ and later (.NET 8, etc.).
- Aligns with current best practices and Microsoft recommendations.

#### 2. Changes to Default and Supported Headers

- Default headers updated.
  - Now adds: Cross-Origin-Opener-Policy, Cross-Origin-Embedder-Policy, Cross-Origin-Resource-Policy by default.
  - No longer includes: X-XSS-Protection (no longer recommended per MDN/OWASP).
- Deprecated headers:
  - X-XSS-Protection: can introduce XSS vulnerabilities; marked obsolete.
  - Expect-CT: deprecated in Chromium; marked obsolete.
  - Feature-Policy: replaced with Permissions-Policy; marked obsolete.

#### 3. New Policy Methods and Features

- **PermissionsPolicyBuilder.AddDefaultSecureDirectives**: Convenience method to rapidly add a broad set of disabled browser features as recommended for APIs by OWASP.
- **AddDefaultApiSecurityHeaders**: Designed for JSON or API-only endpoints. Adds a stricter set of headers based on OWASP’s REST recommendations, including Permissions-Policy and more restrictive Content-Security-Policy.
- **Trusted Types**: Content-Security-Policy now supports trusted types protection, helping defend against DOM-based XSS by using the Trusted Types directive available in Chromium browsers.

  Example (trusted types):

  ```csharp
  app.UseSecurityHeaders(new HeaderPolicyCollection()
      .AddContentSecurityPolicy(builder => {
          builder.AddRequireTrustedTypesFor().Script();
          builder.AddTrustedTypes().AllowPolicy("my-policy");
      }));
  ```

#### 4. Endpoint and Request-Specific Policies

- Now supports multiple header policies in a single application.
- Policies can be applied per-endpoint (e.g., API vs. HTML responses).
- Introduces named policy support for more granular control.
- Offers the ability to completely customize/calculate the headers on each request (useful for multi-tenant or advanced needs).

  Example (different policy per endpoint):

  ```csharp
  builder.Services.AddSecurityHeaderPolicies()
      .SetDefaultPolicy(p => p.AddDefaultSecurityHeaders())
      .AddPolicy("API", p => p.AddDefaultApiSecurityHeaders());
  ```

#### 5. Removal of “Document Headers”

- The document headers concept has been removed.
- Security headers are now applied to *all* responses (unless not applicable), in accordance with OWASP defense-in-depth guidance.
- You can recreate previous document-only behavior via custom policy selectors if needed.

#### 6. Changes to Nonce Generation

- Nonce for CSP is now generated lazily using `HttpContext.GetNonce()` when needed.
- This supports scenarios where the header policies are not known upfront due to endpoint-specific behavior.

#### 7. Updates to Build Provenance

- Each NuGet build now includes provenance attestations and an SBOM (CycloneDX) for enhanced supply chain security.
- Verification steps are described in the project README due to post-upload artifact changes by nuget.org.

---

### Summary

NetEscapades.AspNetCore.SecurityHeaders 1.0.0 modernizes middleware-based security header management for ASP.NET Core applications:

- Supports only contemporary .NET versions
- Updates and deprecates default header sets
- Adds rich customization for APIs and advanced scenarios
- Aligns with industry guidance (OWASP, MDN)
- Improves provenance and supply chain trust

Developers are encouraged to update to this release and provide feedback to the author.

---

### Additional Resources

- [NetEscapades.AspNetCore.SecurityHeaders GitHub](https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders)
- [OWASP Secure Headers Project](https://owasp.org/www-project-secure-headers/)
- [MDN HTTP Security Headers Reference](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers)
- [Release Changelog](https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders/compare/v0.24.0...v1.0.0)

---

*For any questions or issues, please [open an issue on GitHub](https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders/issues).*

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/netescapades-aspnetcore-securityheaders-1-0-0-released/)
