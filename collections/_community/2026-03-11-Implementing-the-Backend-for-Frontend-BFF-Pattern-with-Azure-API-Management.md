---
external_url: https://techcommunity.microsoft.com/t5/microsoft-developer-community/implementing-the-backend-for-frontend-bff-curated-api-pattern/ba-p/4499880
title: Implementing the Backend-for-Frontend (BFF) Pattern with Azure API Management
author: SajalMukherjee
primary_section: dotnet
feed_name: Microsoft Tech Community
date: 2026-03-11 07:00:00 +00:00
tags:
- Aggregation
- API Design
- API Gateway
- API Policy
- APIM
- Authentication
- Azure
- Azure API Management
- Azure Container Apps
- Azure Functions
- Backend For Frontend
- BFF Pattern
- Caching
- Community
- Cross Cutting Concerns
- Curated API
- DevOps
- Distributed Systems
- Frontend Optimization
- Payload Transformation
- Rate Limiting
- Response Aggregation
- .NET
section_names:
- azure
- dotnet
- devops
---
SajalMukherjee explores how Azure API Management enables the Backend-for-Frontend pattern, offering practical API orchestration and aggregation strategies to tailor APIs for diverse frontend needs.<!--excerpt_end-->

# Implementing the Backend-for-Frontend (BFF) / Curated API Pattern Using Azure API Management

**Author:** SajalMukherjee

## Overview

Modern digital applications often support multiple types of clients—web, mobile, partner, and internal tools. Each client may have unique requirements for data shape, performance, and payload size, which poses challenges when exposing backend APIs directly. This article explores how the Backend-for-Frontend (BFF) or 'Curated API' pattern addresses these challenges by introducing a client-specific API layer using Azure API Management (APIM).

## What is the Backend-for-Frontend (BFF) Pattern?

The BFF pattern involves building a backend layer tailored for each frontend application, aggregating and optimizing responses according to the specific needs of its client. It helps:

- Aggregate data from multiple backend services
- Filter and reshape API responses
- Optimize payloads for different clients
- Decouple client experiences from backend complexities

[Read architectural details in Azure Architecture Center](https://learn.microsoft.com/en-us/azure/architecture/patterns/backends-for-frontends)

## Why Use Azure API Management for BFF?

Azure API Management is typically used as an API gateway but its robust policy engine supports deep customization. With APIM policies, you can:

- Orchestrate calls to multiple backend services, either sequentially or in parallel
- Transform request and response payloads per client
- Uniformly enforce authentication, authorization, caching, rate-limiting, and logging
- Achieve the BFF pattern without substantial backend code changes

## Scenarios Suited for APIM-Based BFF

Consider the BFF pattern with APIM when:

- Each frontend requires optimized API responses
- Backend APIs are generic and should stay reusable
- Mobile or constrained clients need compact payloads
- You want to standardize security, caching, rate-limiting
- Avoiding deployment of a separate aggregation/curation microservice is preferable
- API governance and observability are important

## Implementing BFF in Azure API Management

Example architecture with APIM BFF policies:

1. APIM receives a client request
2. APIM issues simultaneous requests to multiple backend services
3. Policy code awaits all responses, possibly aggregating using a pattern like this:

```xml
<wait for="all">
  <send-request mode="copy" response-variable-name="operation1" timeout="{{bff-timeout}}" ignore-error="false">
    <set-url>@("{{bff-baseurl}}/operation1?param1=" + context.Request.Url.Query.GetValueOrDefault("param1", "value1"))</set-url>
  </send-request>
  <send-request mode="copy" response-variable-name="operation2" timeout="{{bff-timeout}}" ignore-error="false">
    <set-url>{{bff-baseurl}}/operation2</set-url>
  </send-request>
  <send-request mode="copy" response-variable-name="operation3" timeout="{{bff-timeout}}" ignore-error="false">
    <set-url>{{bff-baseurl}}/operation3</set-url>
  </send-request>
  <send-request mode="copy" response-variable-name="operation4" timeout="{{bff-timeout}}" ignore-error="false">
    <set-url>{{bff-baseurl}}/operation4</set-url>
  </send-request>
</wait>
```

Some key implementation points:

- `wait for="all"`: Waits for all parallel `send-request` operations
- `response-variable-name`: Collects responses for future transformation
- Timeouts and error handling are customizable per endpoint

After all responses are received, use policy code to merge them, capturing body and status codes:

```xml
<set-variable name="finalResponseData" value="@{ JObject finalResponse = new JObject(); ... /* Aggregation logic as described in the article */ ... return finalResponse; }" />
```

This combines all responses; successful (`200 OK`) if all succeed, otherwise partial/multi-status (`206/207`).

Return the curated result:

```xml
<return-response>
  <set-status code="@((int)((JObject)context.Variables["finalResponseData"]).SelectToken("status"))" reason="@(((JObject)context.Variables["finalResponseData"]).SelectToken("reason").ToString())" />
  <set-body>@(((JObject)context.Variables["finalResponseData"]).SelectToken("body").ToString(Newtonsoft.Json.Formatting.None))</set-body>
</return-response>
```

[See policy samples and code on GitHub](https://github.com/Azure/api-management-policy-snippets/tree/master)

## Limitations and When to Rethink APIM as BFF

- Complex transformations and code maintenance: If API orchestration or transformation logic grows too complex, consider dedicated curation microservices using Azure Functions or Azure Container Apps
- Granular per-operation logic: Significant divergence in how requests for each backend are shaped may be better handled in code, not policies
- Architectural fit: Do not introduce APIM solely for BFF pattern if you do not already have an API gateway need

## Conclusion

Azure API Management provides powerful features for implementing the Backend-for-Frontend pattern, reducing the need for extra backend services and supporting agile frontend evolution. For deep technical patterns, explore [Microsoft's official architectural guidance](https://learn.microsoft.com/en-us/azure/architecture/patterns/backends-for-frontends) and sample [GitHub repositories](https://github.com/Azure/api-management-policy-snippets/tree/master).

---

**References**

- [Azure Architecture Center – Backend-for-Frontends Pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/backends-for-frontends)
- [Azure API Management Policy Snippets (GitHub)](https://github.com/Azure/api-management-policy-snippets/tree/master)
- [Curated APIs Policy Example (GitHub)](https://github.com/Azure/api-management-policy-snippets/blob/master/examples/Backend%20for%20Frontend%20or%20curated%20API%20policy.xml)
- [Send-request Policy Reference](https://learn.microsoft.com/en-us/azure/api-management/send-request-policy)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/implementing-the-backend-for-frontend-bff-curated-api-pattern/ba-p/4499880)
