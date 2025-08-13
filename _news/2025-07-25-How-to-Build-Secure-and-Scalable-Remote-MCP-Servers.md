---
layout: "post"
title: "How to Build Secure and Scalable Remote MCP Servers"
description: "This guide by Den Delimarsky explores security and scalability best practices for implementing Model Context Protocol (MCP) servers, emphasizing authorization with OAuth 2.1, secrets management, multi-user scenarios, observability, and use of AI/API gateways. Learn how to build robust MCP integrations for AI agent connectivity."
author: "Den Delimarsky"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://github.blog/ai-and-ml/generative-ai/how-to-build-secure-and-scalable-remote-mcp-servers/"
viewing_mode: "external"
feed_name: "The GitHub Blog"
feed_url: "https://github.blog/feed/"
date: 2025-07-25 17:12:02 +00:00
permalink: "/2025-07-25-How-to-Build-Secure-and-Scalable-Remote-MCP-Servers.html"
categories: ["AI", "Security", "Azure"]
tags: ["AI", "AI & ML", "AI Agents", "AI Gateway", "APIs", "Authorization", "Azure", "Azure Key Vault", "Distributed Tracing", "Generative AI", "MCP", "Model Context Protocol", "Multi Tenancy", "News", "OAuth 2.1", "OpenTelemetry", "Secrets Management", "Security", "Security Best Practices", "Token Validation"]
tags_normalized: ["ai", "ai ml", "ai agents", "ai gateway", "apis", "authorization", "azure", "azure key vault", "distributed tracing", "generative ai", "mcp", "model context protocol", "multi tenancy", "news", "oauth 2 dot 1", "opentelemetry", "secrets management", "security", "security best practices", "token validation"]
---

Authored by Den Delimarsky, this article offers a thorough walkthrough for developers on building secure, scalable MCP servers. It focuses on robust security, authorization, secrets management, and architectural practices needed to safely enable AI agent connectivity and operations.<!--excerpt_end-->

## Introduction

Model Context Protocol (MCP) is a standardized protocol enabling AI agents to connect with external tools and data sources, removing the need to build API-specific connectors. In applications such as extracting data from invoices, summarizing tickets, or code search, MCP facilitates secure, structured access to required contextual data. This guide addresses the critical security considerations and best practices necessary when implementing MCP servers, especially in light of the latest protocol specifications.

## Why Security is Critical for MCP

MCP servers function as bridges between AI agents and potentially sensitive enterprise resources, not just serving known clients but exposing a broader attack surface. If compromised, such servers could enable attackers to manipulate AI behaviors or gain access to downstream systems. The MCP specification has expanded to include [security guidelines](https://modelcontextprotocol.io/specification/2025-06-18/basic/security_best_practices) addressing common risks such as confused deputy problems, token passthrough, and session hijacking. Following these recommendations from the outset helps safeguard sensitive tools and data.

## Authorization in MCP

MCP leverages OAuth 2.1, allowing implementers to use mature industry standards and libraries for secure authorization. This ensures:

- **Authorization Server Discovery:** Implementation of OAuth 2.0 Protected Resource Metadata (PRM; [RFC 9728](https://datatracker.ietf.org/doc/html/rfc9728/)) allows servers to declare their supported authorization mechanisms. Clients that encounter a protected MCP server receive an HTTP 401 with a `WWW-Authenticate` header pointing to the metadata endpoint.
- **Dynamic Client Registration:** OAuth 2.0 Dynamic Client Registration ([RFC 7591](https://datatracker.ietf.org/doc/html/rfc7591/)) automates client setup, allowing AI agents to connect dynamically without manual configuration.
- **Resource Indicators:** Tokens are bound to specific MCP servers via [RFC 8707](https://datatracker.ietf.org/doc/html/rfc8707), limiting misuse and enforcing strict security boundaries.

The 2025-06-18 MCP specification clarifies the division of responsibilities between authorization and resource servers, while keeping compliance with OAuth 2.1 foundational. Developers can use existing open-source or commercial OAuth solutions without custom implementations.

### MCP Authorization Flow

1. **Discovery:** MCP client attempts to access a server without credentials.
2. **Server Response:** Returns HTTP 401 with metadata reference.
3. **Metadata Retrieval:** Client fetches PRM, extracts authorization details.
4. **Client Registration:** Automatic (or pre-registered) registration with the authorization server.
5. **Authorization Request:** Initiates OAuth flow (using PKCE and `resource` parameter).
6. **User Consent:** User authorizes via the authorization server.
7. **Token Exchange:** Client receives access token upon authorization code exchange.
8. **Authenticated Requests:** Subsequent client requests include a `Bearer` token.

This standard flow permits leveraging robust OAuth infrastructure rather than writing custom authorization logic.

## Implementing Authorization in MCP Servers

- **PRM Endpoint:** MCP servers must expose a `/.well-known/oauth-protected-resource` endpoint to communicate supported authorization scopes. Tools such as the [MCP TypeScript SDK](https://github.com/modelcontextprotocol/typescript-sdk) include native support for this feature.
- **Token Validation Middleware:** Open-source solutions like [PyJWT](https://github.com/jpadilla/pyjwt) help ensure that only valid tokens are accepted:
  - Extract bearer tokens
  - Validate signatures using JWKS endpoints
  - Check expiration and claims
  - Confirm tokens are audience-specific
- **Error Handling:** Servers should return precise HTTP status codes (`401` for missing/invalid, `403` for insufficient permissions), with appropriate headers.

Integration of these capabilities is being driven by the wider MCP community to streamline secure server implementation.

### Multi-Tenancy Security

When serving multiple users, MCP servers face challenges with identity scoping and data isolation. Risks like the confused deputy problem increase with complexity. Secure token validation and mapping of user IDs (`sub` claim) to internal permissions are essential. Each actionable API call or data query should enforce the principle of least privilege, and use trusted, mature libraries for session and access management.

## Scaling with AI Gateways

Scalability introduces new challenges: traffic spikes, version compatibility, and collective enforcement of security policies. AI gateways (similar to API gateways) serve as a central control point for:

- Rate limiting
- JWT validation before server access
- Adding security headers and managing CORS

Such gateways support operations like request/response transformation, caching, and circuit breaking—centralizing cross-cutting security and scalability concerns for easier maintenance.

### Essential AI Gateway Policies for MCP

- Automated rate limiting
- Protocol transformation for backward/forward compatibility
- Centralized JWT/token validation
- Unified error handling and diagnostics

## Production-Ready Security and Scalability Patterns

### Secrets Management

Secrets such as API keys, database credentials, and OAuth tokens should not be managed with environment variables in production, as this poses rotation and exposure risks. Instead, use dedicated secrets management tools:

- [Azure Key Vault](https://learn.microsoft.com/azure/key-vault/general/basic-concepts)
- [AWS Secrets Manager](https://docs.aws.amazon.com/secretsmanager/latest/userguide/intro.html)
- [HashiCorp Vault](https://developer.hashicorp.com/vault/docs/about-vault/what-is-vault)

For maximum security, implement **workload identities** where the app is assigned a secure identity tied to the cloud provider, enabling fine-grained, short-lived access with no static credentials. Each MCP server instance should have only the minimal required permissions/secrets.

### Observability and Monitoring

Operational transparency is vital:

- **Structured Logging:** Consistent log format with correlation IDs across calls
- **Distributed Tracing:** Use standards like [OpenTelemetry](https://opentelemetry.io/) for hop-by-hop breakdowns
- **Security Event Logging:** Capture all auth attempts, failures, anomalies
- **Metrics Collection:** Track latency, error rates, resource utilization
- **Alerting and Dashboards:** Automated notification of critical events, summary health and security dashboards

Establishing strong observability enables proactive detection and diagnosis of issues at scale.

## Conclusion

Building secure and scalable MCP servers demands thoughtful application of standardized authentication, authorization, secrets management, and operational patterns. The evolving MCP specification provides solid security building blocks, while modern cloud platforms and open-source tooling fill the gaps for robust production systems. Start with security at the foundation to build reliable infrastructure supporting sensitive AI-driven tasks.

**References:**

- [MCP Authorization Specification](https://modelcontextprotocol.io/specification/2025-06-18/basic/authorization)
- [Security Best Practices for MCP](https://modelcontextprotocol.io/specification/2025-06-18/basic/security_best_practices)
- [Azure Key Vault Concepts](https://learn.microsoft.com/azure/key-vault/general/basic-concepts)

**Author:** Den Delimarsky

This post appeared first on "The GitHub Blog". [Read the entire article here](https://github.blog/ai-and-ml/generative-ai/how-to-build-secure-and-scalable-remote-mcp-servers/)
