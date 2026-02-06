---
external_url: https://techcommunity.microsoft.com/t5/azure-networking-blog/integrating-azure-application-gateway-v2-with-azure-api/ba-p/4470804
title: Integrating Azure Application Gateway v2 with Azure API Management for Secure and Scalable API Delivery
author: ranjsharma
feed_name: Microsoft Tech Community
date: 2025-11-18 17:33:43 +00:00
tags:
- API Security
- Azure API Management
- Azure Application Gateway
- Azure DevOps
- Certificates
- CI/CD
- Key Vault
- Layer 7 Routing
- Log Analytics
- Mtls
- Networking
- OWASP
- Private Endpoint
- Rate Limiting
- Terraform
- VNet
- WAF
- Zero Trust
- Azure
- DevOps
- Security
- Community
- .NET
section_names:
- azure
- dotnet
- devops
- security
primary_section: dotnet
---
ranjsharma presents a step-by-step guide to integrating Azure Application Gateway v2 and Azure API Management for secure, scalable API solutions, including automation with Terraform and Azure DevOps.<!--excerpt_end-->

# Integrating Azure Application Gateway v2 with Azure API Management for Secure and Scalable API Delivery

This guide demonstrates how to combine Azure Application Gateway v2 (with WAF) and Azure API Management to provide an enterprise-grade, scalable, and secure API front door. The integration leverages network architectures, automation, certificates, private endpoints, and CI/CD pipelines with Terraform and Azure DevOps.

## Why Application Gateway v2 + Azure API Management?

- **Layer-7 routing**: Path-based rules, host headers, URL rewrites, WAF protection via the OWASP Core Rule Set.
- **API Gateway capabilities**: Abstraction, versioning, throttling, caching, JWT validation, and per-API policies.
- **Combined scenario**: Application Gateway acts as the secure, internet-facing entry point while Azure API Management provides centralized API governance.

### Reference Architecture Scenarios

#### Scenario 1: Public Azure API Management

```
Internet → App Gateway (WAF) → Azure API Management (External) → Backends
```

- Simple and fast to implement, supports CDN/Front Door chaining
- Azure API Management public; add IP allow-lists/mTLS as needed

#### Scenario 2: Private Azure API Management Endpoint (Zero Trust)

```
Internet → App Gateway (WAF) → Azure API Management (Internal) via Private Endpoint
```

- Only App Gateway is public; Azure API Management is internal, enabled for zero-trust
- Requires vNet, DNS, and Private Link configuration

#### Scenario 3: API Management as Public Front Door

```
Azure API Management (External) → App Gateway (Internal) → Private Backends
```

- API Management is the public edge; App Gateway does L7 routing
- More API Management policy work required

## Network & DNS Design Checklist

- Dedicated subnets for Application Gateway and API Management
- Private Link/Endpoints for internal connectivity
- Custom and Private DNS zones (e.g., privatelink.azure-api.net)
- Firewall/NSG rules to tightly control access
- Hybrid connectivity: VPN/ExpressRoute for on-prem integration

## Certificates & TLS

- Use custom domains (e.g., api.contoso.com)
- Store certs in Azure Key Vault; grant managed identity access to App Gateway & API Management
- Enforce TLS 1.2+/disable weak ciphers
- mTLS available for mutual authentication

## WAF on App Gateway v2

- Modes: Detection vs prevention (enable prevention after tuning)
- Start with CRS 3.2, set exclusions needed for APIs (e.g., JSON payloads)
- Enable bot protection, custom anomaly scores, and log to Log Analytics

## Azure API Management Policies

- **Inbound policies**: `validate-jwt`, `rate-limit`, `ip-filter`, `check-header`
- **Backend**: `retry`, `forward-request` (optionally with mTLS)
- **Outbound**: `set-header`, `cache`
- Use named values and Key Vault for parameterization

**Example: JWT Validation and Rate-Limit Policy**

```xml
<policies>
  <inbound>
    <base />
    <validate-jwt header-name="Authorization" failed-validation-httpcode="401">
      <openid-config url="https://login.microsoftonline.com/<tenant-id>/v2.0/.well-known/openid-configuration" />
      <audiences>
        <audience>api://contoso-app-id</audience>
      </audiences>
      <issuers>
        <issuer>https://sts.windows.net/<tenant-id>/</issuer>
      </issuers>
    </validate-jwt>
    <rate-limit calls="100" renewal-period="60" />
  </inbound>
  <backend>
    <forward-request />
  </backend>
  <outbound>
    <base />
  </outbound>
</policies>
```

## Terraform: Core Resources (Simplified)

- Parameterize for production use
- Integrate with Azure Key Vault for certificate storage and rotation
- Diagnostic settings for monitoring
- Example resources include:
  - Resource group
  - Virtual network and subnets
  - Application Gateway v2 (WAF)
  - API Management (Developer/Premium SKU)

## Azure DevOps - CI/CD Pipeline (YAML)

- Stages: Validate, Plan, Apply
- Tasks: Install Terraform, init and validate, plan, apply
- Leverages Azure Service Connection and managed identity

## Observability & Diagnostics

- Collect access logs from App Gateway/WAF
- Enable diagnostic metrics/logs on API Management
- Correlate trace IDs across all components
- Build alerts for error spikes, throttling, and certificate expiration

## Security Hardening

- Enforce network restrictions (NSGs, IP allow-lists)
- WAF exclusions kept minimal and regularly reviewed
- Use managed identities and RBAC for admin endpoints
- DDoS Protection and Azure Front Door where appropriate
- mTLS from App Gateway or clients to API Management
- Deny public network access where possible

## Cost & Performance Considerations

- Right-size Application Gateway
- Use Premium SKU for vNet, multi-region, and zone redundancy
- Leverage caching and response compression
- Tune health probes for optimal backend performance

## Troubleshooting Tips

- Diagnose errors like 502/504 and authentication issues (401/403)
- Validate DNS and certificate configurations
- Review NSG rules and backend health

## Production Checklist

- Custom domains and automated cert rotation
- WAF in Prevention, tuned exclusions
- Strict policies for API security and governance
- Private endpoint and DNS validation
- Monitoring, autoscaling, and CI/CD safeguards
- Secure Terraform state and establish failover runbooks

_Last updated: Nov 18, 2025. Author: ranjsharma_

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-networking-blog/integrating-azure-application-gateway-v2-with-azure-api/ba-p/4470804)
