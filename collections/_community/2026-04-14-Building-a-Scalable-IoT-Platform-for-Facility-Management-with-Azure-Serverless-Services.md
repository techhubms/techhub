---
tags:
- Azure
- Azure API Management
- Azure App Configuration
- Azure Container Apps
- Azure Cosmos DB
- Azure Data Lake Storage Gen2
- Azure Event Grid
- Azure Functions V4
- Azure Key Vault
- Azure Redis Cache
- Azure Storage Queues
- Blob Storage Leases
- Chakra UI
- Circuit Breaker
- CloudEvents
- Community
- CORS
- DevOps
- Distributed Locking
- Event Driven Architecture
- JWT
- Microservices
- Microsoft Entra ID
- MSAL
- OpenAPI V3
- Opossum
- Rate Limiting
- RBAC
- React 18
- ReDoS Mitigation
- Security
- Telemetry Ingestion
- TypeScript
- TypeSpec
- Vite
- Zod
external_url: https://techcommunity.microsoft.com/t5/azure-architecture-blog/building-a-scalable-iot-platform-for-facility-management-with/ba-p/4495263
date: 2026-04-14 20:44:41 +00:00
feed_name: Microsoft Tech Community
author: nishantmv
primary_section: azure
section_names:
- azure
- devops
- security
title: Building a Scalable IoT Platform for Facility Management with Azure Serverless Services
---

In this community post, nishantmv breaks down a production-grade Azure serverless architecture for an enterprise facility-management IoT platform, covering a multi-provider telemetry pipeline, template-driven device modeling, an event-driven rule engine, and the security/resilience hardening that made it ready for production.<!--excerpt_end-->

# Building a Scalable IoT Platform for Facility Management with Azure Serverless Services

Facility management at scale requires real-time visibility into thousands of connected devices across geographically distributed sites. HVAC systems, generators, occupancy sensors, and independent monitoring devices each produce continuous telemetry streams that must be ingested, processed, stored, and acted upon—often within seconds.

The platform needed to:

- Ingest telemetry from multiple device providers
- Apply configurable business rules
- Send automated notifications
- Provide a modern operations portal
- Support multi-tenant RBAC
- Use template-driven device management
- Support historical analytics

This write-up walks through an architecture built on Azure Functions, Event Grid, Cosmos DB, Redis, and Data Lake Storage Gen2, and includes security/resilience fixes found during code review.

## High-level architecture

The platform uses a microservices approach with six independently deployable services. All services run on **Azure Functions v4 with TypeScript**, are **containerized**, and deployed to **Azure Container Apps**.

Main building blocks:

- **IoT Portal UI** — React 18 frontend with Chakra UI, TanStack Query, and MSAL for Azure AD authentication
- **IoT Portal API** — Backend for asset management, locations, templates, rules, and telemetry queries
- **IoT Profile API** — User profile management and RBAC with custom JWT token generation
- **HTTP Ingestion** — Provider-agnostic telemetry ingestion endpoints
- **Telemetry Processor** — Event-driven standardization, state management, and historical archival
- **Rule Engine** — Configurable rule evaluation with automated notifications via email and SMS

## Telemetry ingestion pipeline

A two-stage pipeline is used to keep providers isolated and enable horizontal scaling.

### Stage 1: HTTP reception (edge)

Telemetry arrives via authenticated HTTP POST endpoints.

- **Azure API Management (APIM)** handles:
  - Authentication via subscription keys
  - Rate limiting
  - Request validation at the gateway
- Each provider gets a dedicated route/controller, but all converge into a shared publishing pipeline.

Key design decision:

- Onboarding a new device provider requires adding:
  - A route
  - A controller
  - An APIM configuration
- No changes to the core pipeline.

### Stage 2: Event forwarding

- Validated payloads are converted to **CloudEvents**:
  - `Telemetry.Http.Ingested.{Provider}`
- Published to **Azure Event Grid**
- Routed to **Azure Storage Queues** for downstream processing

## Telemetry processing and standardization

The Telemetry Processor consumes events from Storage Queues and performs:

| Stage | Function | Technology |
| --- | --- | --- |
| Standardize | Transform provider-specific payloads into a unified `StandardizedEvent` schema using JSONPath-based mappings | Capability templates + Redis-cached metadata |
| Update State | Upsert current asset state with monotonic update logic | Azure Cosmos DB (hierarchical partition keys) |
| Archive | Write historical telemetry as JSON files organized by date hierarchy | Azure Data Lake Storage Gen2 |
| Trigger Rules | Publish trigger events for rule evaluation | Azure Event Grid → Storage Queues |

Monotonic update logic:

- Only newer timestamps overwrite existing state
- Protects against out-of-order delivery in distributed systems

## Template-driven device management

A three-tier template hierarchy decouples device definition from device instances:

| Tier | Purpose | Examples |
| --- | --- | --- |
| Capability Templates | Define individual data points (telemetry, commands, parameters) with types/validation/units | Temperature, Humidity, ON/OFF Command |
| Asset Templates | Define device types by combining capabilities + physical specs | Sensor, Gateway, Genset Controller |
| Location Templates | Define physical spaces and required assets (optional strict enforcement) | Building, Floor, Room, Equipment Area |

Template rules:

- 7 data types: String, Integer, Double, Boolean, DateTime, JSON, Binary
- 3 validation rule types:
  - Value limits
  - Allowed value lists
  - Regex patterns
- Semantic versioning starting at **1.0.0**
- Deletion protection prevents removing templates referenced by higher tiers/instances

Operational benefit:

- New device models can be onboarded by defining templates (operations-driven), not writing code.
- Telemetry Processor looks up templates via Redis (5-minute TTL) to standardize data.

## Rule engine: event-driven automation

The Rule Engine evaluates configurable rules against device state and triggers actions. It uses a **template-to-implementation** approach:

- Rule templates are reusable
- Implementations bind templates to specific assets or locations

### Rule trigger types

| Trigger Type | Mechanism | Use Case |
| --- | --- | --- |
| State Change | Storage Queue from Telemetry Processor | Alert when temperature exceeds 30°C |
| Time-Based | CRON patterns evaluated every minute | Check if equipment has been idle for 4 hours |
| HTTP | Direct HTTP call (debug only) | Development/testing |

### Condition evaluation

Conditions support:

- Comparison operators
- Temporal operators: `olderThan`, `newerThan`
- Composite logic: `all` / `any` with short-circuit evaluation
- History-based conditions:
  - `totalDuration`
  - `stateChangeCount`

### Notification actions

Notification flow:

- Rule Engine publishes notification events to **Event Grid**
- Routed to **Storage Queues**
- Notification Service batches and sends Email/SMS via a third-party messaging API

`Rule Triggered → Event Grid → Storage Queue → Notification Service → Messaging Provider (Email/SMS)`

## Role-based access control and identity

Security combines **Microsoft Entra ID (Azure AD)** with a custom platform token.

### Authentication flow

1. User authenticates with Microsoft Entra ID
2. APIM validates the AAD token at the gateway
3. APIM calls Profile API to generate a custom **platform token** (JWT)
4. JWT is signed with RSA certs from **Azure Key Vault**
5. APIM caches token (60-second TTL) and forwards it as a custom header to backend APIs

### Four scope types

| Scope | Description |
| --- | --- |
| platform | Global administrative access |
| site | Access limited to specific sites |
| client | Access limited to specific clients |
| siteAndClient | Combined scope for fine-grained access |

- Each scope supports a `GLOBAL` target.
- Cosmos DB entities:
  - Permissions (atomic rights)
  - Roles (permission groups)
  - User Profiles (role assignments + scoping)

## Hardening: what the code review found (and fixes)

### Race conditions in distributed rule processing

Problem:

- Multiple Azure Function instances can run timer triggers concurrently
- This caused duplicate rule executions and duplicate notifications

Fix:

- Distributed locking using **Azure Blob Storage leases**

```typescript
const lockKey = `timer-execute-${myTimer.scheduleStatus.next}`;
const lock = await acquireDistributedLock(lockKey, 60000);
if (!lock) {
  context.log("Another instance is processing, skipping");
  return;
}
```

### Circuit breaker for external APIs

Problem:

- Third-party messaging API calls lacked circuit breaker protection
- Failures caused 30+ second timeouts and cascading failures

Fix:

- Wrap external calls with a circuit breaker (example uses `opossum`)
- Opens after 50% failure rate; tests recovery after 60 seconds

```typescript
import CircuitBreaker from "opossum";

this.emailCircuitBreaker = new CircuitBreaker(
  this.sendNotification.bind(this),
  {
    timeout: 30000,
    errorThresholdPercentage: 50,
    resetTimeout: 60000
  }
);
```

### Authentication bypass risk

Problem:

- A configuration flag could disable auth checks entirely

Fix:

- Remove bypass for production
- For dev, tie bypass to environment detection with multiple safeguards

### Weak token validation

Problem:

- JWT validation used `decodeToken()` (base64 decode) instead of signature verification

Fix:

- Use `verifyToken()` with Azure AD public signing keys from the OpenID config endpoint
- Validate issuer, audience, expiration, algorithm

### Additional hardening items

| Issue | Severity | Resolution |
| --- | --- | --- |
| Missing rate limiting for notifications | High | Per-notification-group limits with Redis counters + TTL |
| ReDoS vulnerability in multipart parsing | Medium | Non-backtracking regex + character classes + length limits |
| No request size limits | Medium | `maxRequestBodySize` in Azure Functions `host.json` |
| Array splitting without length limits | Medium | Explicit max count (100) before processing comma-separated inputs |
| CORS wildcard in production | High | Replace with explicit allowed origins per environment |
| Anonymous function authorization | Medium | Change `authLevel` to `function` for defense in depth |

## Technology decisions and trade-offs

| Aspect | Choice | Rationale |
| --- | --- | --- |
| Compute | Azure Functions v4 (TypeScript) | Serverless scaling, pay-per-execution, minimal infra |
| Messaging | Event Grid + Storage Queues (CloudEvents) | Reliable at-least-once delivery, dead-letter support |
| Primary store | Azure Cosmos DB (hierarchical partition keys) | Global distribution, sub-10ms reads, flexible schema |
| Historical store | Azure Data Lake Storage Gen2 | Cost-effective long-term storage, analytics-friendly layout |
| Caching | Azure Redis Cache | Read-through caching + explicit invalidation |
| API gateway | Azure API Management | Centralized auth, rate limiting, CORS, subscription keys |
| Configuration | Azure App Configuration | Centralized configuration + feature flags |
| Secrets | Azure Key Vault | RSA signing certs, API keys, connection strings |
| Frontend | React 18 + Vite + Chakra UI | Modern DX and accessible UI components |
| API spec | TypeSpec → OpenAPI v3 | Type-safe definitions + generated OpenAPI |

Trade-off noted:

- **Storage Queues** were chosen over **Service Bus** for simplicity and cost.
- If ordering guarantees/sessions are needed later, migrating to Service Bus is considered straightforward.

## Lessons learned

1. Provider isolation makes onboarding new vendors mostly configuration work.
2. Template-driven data models reduce code churn when devices evolve.
3. Serverless timers require distributed locking to avoid duplicate processing.
4. Code review is a production readiness gate (auth bypass, ReDoS, missing circuit breakers).
5. Edge schema validation (Zod) prevents bad inputs from cascading downstream.

## Conclusion and recommended next steps

Key takeaways:

1. Event-driven decoupling (Event Grid + Storage Queues) creates clean scaling boundaries
2. Template-driven device management avoids code changes for onboarding
3. Layered security (Entra ID + APIM + custom JWT + function auth) provides defense in depth
4. Resilience patterns (circuit breakers, locking, rate limiting) are required for production IoT

If building something similar:

1. Start with ingestion + processing (the backbone)
2. Design templates early
3. Add distributed locking for timer-triggered functions from day one
4. Add circuit breakers to all external calls before production
5. Run a security-focused review (bypass flags, injection, DoS)

References:

- Azure IoT reference architecture: https://learn.microsoft.com/en-us/azure/architecture/reference-architectures/iot
- Azure Well-Architected Framework: https://learn.microsoft.com/en-us/azure/well-architected/


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-architecture-blog/building-a-scalable-iot-platform-for-facility-management-with/ba-p/4495263)

