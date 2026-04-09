---
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/designing-reliable-health-check-endpoints-for-iis-behind-azure/ba-p/4507938
author: AjaySingh_
feed_name: Microsoft Tech Community
primary_section: azure
section_names:
- azure
- devops
date: 2026-04-08 23:18:05 +00:00
tags:
- /health Endpoint
- Anonymous Authentication
- Azure
- Azure Application Gateway
- Backend Health
- Certificate CN Mismatch
- Community
- Custom Health Probe
- DevOps
- Health Check Endpoint
- Health Probes
- Host Header
- HTTP 502 Bad Gateway
- HTTP 504 Gateway Timeout
- HTTPS
- IIS
- IIS Site Bindings
- Invoke WebRequest
- Migration To Azure IaaS
- Production Troubleshooting
- SNI
- TLS
- Web.config
- Windows Authentication
title: Designing Reliable Health Check Endpoints for IIS Behind Azure Application Gateway
---

AjaySingh_ explains how to avoid false “backend unhealthy” states and resulting 502/504 errors by designing a lightweight, unauthenticated IIS /health endpoint and configuring Azure Application Gateway health probes (including correct host header/SNI handling).<!--excerpt_end-->

# Designing Reliable Health Check Endpoints for IIS Behind Azure Application Gateway

When hosting IIS-based applications behind Azure Application Gateway, a common production issue is unexpected **502 / 504** errors even when the application seems healthy. Often the root cause is a **misconfigured or unreliable health probe**, not an app failure.

This guidance is based on lessons learned during an enterprise migration of IIS applications to **Azure IaaS**, where default probe behavior wasn’t sufficient once authentication, redirects, and certificates were involved.

## Why health probes matter in Azure Application Gateway

[Azure Application Gateway](https://learn.microsoft.com/en-us/azure/application-gateway/) relies on [health probes](https://learn.microsoft.com/en-us/azure/application-gateway/application-gateway-probe-overview) to decide whether backend instances should receive traffic.

If a probe:

- receives a non-200 response
- times out
- gets redirected
- requires authentication

…the backend is marked **Unhealthy** and traffic stops, leading to user-facing errors.

A healthy IIS application does **not automatically** mean a healthy Application Gateway backend.

## Failure flow: how a misconfigured health probe leads to 502 errors

A confusing scenario: IIS is running fine, but users intermittently see **502 Bad Gateway**.

What’s happening:

- Application Gateway periodically sends health probes to backend IIS instances.
- If the probe endpoint:
  - redirects to `/login`
  - requires authentication
  - returns `401` / `403` / `302`
  - times out
  
  the probe is considered **failed**.
- After consecutive failures, the backend instance is marked **Unhealthy**.
- Application Gateway stops forwarding traffic to unhealthy backends.
- If **all** backend instances are unhealthy, every client request results in **502 Bad Gateway**, even though IIS may still be running.

**Key takeaway:** Many 502s behind Application Gateway are health probe failures, not application failures.

## Common health probe pitfalls with IIS

### 1) Probing the root path (`/`)

Many IIS apps:

- redirect `/` → `/login`
- require authentication
- return `401` / `302` / `403`

Application Gateway expects a clean **200 OK**, not redirects or auth challenges.

### 2) Authentication-enabled endpoints

Health probes do not support authentication headers. If your app enforces:

- Windows Authentication
- OAuth / JWT
- client certificates

…the probe will fail.

### 3) Slow or heavy endpoints

If the probe hits a controller that:

- calls a database
- performs startup checks
- loads configuration

…you can get intermittent probe failures, especially under load.

### 4) Certificate and host header mismatch

TLS-enabled backends can fail probes due to:

- missing Host header
- incorrect SNI configuration
- certificate CN mismatch

## Design principles for a reliable IIS health endpoint

A good health check endpoint should be:

- lightweight
- anonymous
- fast (< 100 ms)
- always return HTTP 200
- independent of business logic

## Step 1: Create a dedicated health endpoint

### Recommended path

Use a dedicated endpoint like:

- `/health`

It should:

- bypass authentication
- avoid redirects
- avoid database calls

### Example: simple IIS health page

Create a static file:

- `C:\inetpub\wwwroot\website\health\index.html`

Benefits:

- static
- fast
- zero dependencies

## Step 2: Exclude the health endpoint from authentication

If the site uses authentication, explicitly allow anonymous access to `/health`.

### `web.config` example

```xml
<location path="health">
  <system.webServer>
    <security>
      <authentication>
        <anonymousAuthentication enabled="true" />
        <windowsAuthentication enabled="false" />
      </authentication>
    </security>
  </system.webServer>
</location>
```

This ensures probes succeed even if the rest of the site is secured.

## Step 3: Configure Azure Application Gateway health probe

### Recommended probe settings

| Setting | Value |
| --- | --- |
| Protocol | HTTPS |
| Path | /health |
| Interval | 30 seconds |
| Timeout | 30 seconds |
| Unhealthy threshold | 3 |
| Pick host name from backend | Enabled |

### Why “Pick host name from backend” matters

It helps ensure:

- correct Host header
- proper certificate validation
- fewer TLS handshake failures

## Step 4: Validate health probe behavior

### From Application Gateway

- Go to **Backend health**
- Verify the backend shows **Healthy**
- Confirm response code is `200`

### From the IIS VM

```powershell
Invoke-WebRequest https://your-app-domain/health
```

Expected:

```text
StatusCode : 200
```

## Troubleshooting common failures

### Probe shows Unhealthy but app works

- Check authentication rules
- Verify `/health` does not redirect
- Confirm it returns HTTP 200

### TLS or certificate errors

- Ensure certificate CN matches the backend domain
- Enable “Pick host name from backend”
- Validate the certificate is bound in IIS

### Intermittent failures

- Reduce probe complexity
- Avoid DB or service calls
- Prefer static content

## Production best practices

- Use separate health endpoints per application
- Never reuse business endpoints for probes
- Monitor probe failures as early warning signs
- Test probes after every deployment
- Keep health endpoints simple and boring

## Final thoughts

A reliable health check endpoint isn’t optional when running IIS behind Azure Application Gateway; it’s a core part of availability. A dedicated, authentication-free, lightweight endpoint can eliminate false outages and improve stability.


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-architecture-blog/designing-reliable-health-check-endpoints-for-iis-behind-azure/ba-p/4507938)

