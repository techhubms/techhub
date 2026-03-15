---
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/transforming-retry-after-headers-in-azure-apim-a-step-by-step/ba-p/4489762
title: 'Transforming Retry-After Headers in Azure APIM: A Step-by-Step Guide'
author: pratikpanda
primary_section: azure
feed_name: Microsoft Tech Community
date: 2026-01-27 14:18:10 +00:00
tags:
- API Gateway
- API Throttling
- APIM
- Azure
- Azure API Management
- Cloud Architecture
- Community
- DevOps Best Practices
- Error Handling
- Header Manipulation
- Microsoft Azure
- On Error Policy
- Policy Customization
- Rate Limit Policy
- Rate Limiting
- REST API
- Retry After Header
section_names:
- azure
---
pratikpanda shares how to override the Retry-After response header in Azure API Management's rate-limiting policies, giving developers practical steps to improve client experience and meet custom requirements.<!--excerpt_end-->

# Transforming Retry-After Headers in Azure APIM: A Step-by-Step Guide

Customizing response headers in Azure API Management (APIM) gives API providers increased flexibility and a better way to communicate rate limit details to clients. This post focuses on how to return a timestamp rather than a numeric interval in the `Retry-After` header when using APIM rate-limiting policies.

## Why Rate Limiting Matters

Rate limiting helps ensure your APIs are not overused or abused, maintaining fair access for all users and protecting backend services from denial-of-service (DoS) attacks and spikes in usage. In Azure API Management, common approaches include:

- `rate-limit`
- `rate-limit-by-key`

These policies control how many times a client or a defined key can access an API within a set period.

## Default Retry-After Behavior

By default, when a client exceeds the allowed call volume, APIM responds with a `Retry-After` header containing the wait interval in seconds (e.g., `Retry-After: 60`). In some scenarios, you may instead need to provide an absolute timestamp, helping clients better schedule retries.

**Example:**

- Interval header: `Retry-After: 60`
- Timestamp header: `Retry-After: 2020-05-04T12:23:41.6181792Z`

## How to Customize the Retry-After Header

Since the built-in `rate-limit` policy doesn’t natively support timestamp values for this header, you'll use the APIM on-error policy scope in conjunction with error handling for rate limit events.

**Sample Policy Configuration:**

```xml
<inbound>
  <base />
  <rate-limit-by-key calls="1000" renewal-period="60"
    counter-key="@(context.Request.IpAddress)"
    increment-condition="@(context.Response.StatusCode == 200)"
    remaining-calls-variable-name="remainingCallsPerIP"
    retry-after-header-name="Retry-After"
    remaining-calls-header-name="Requests-Remaining"
    retry-after-variable-name="retryAfter">
  </rate-limit-by-key>
</inbound>
```

**On-Error Handler:**

```xml
<on-error>
  <choose>
    <when condition="@(context.LastError.Reason == \"ratelimitexceeded\")">
      <set-header name="Retry-After" exists-action="override">
        <value>@(DateTime.UtcNow.AddSeconds(context.Variables.GetValueOrDefault<int>("retryAfter")).ToString("o"))</value>
      </set-header>
    </when>
  </choose>
  <base />
</on-error>
```

- The `on-error` policy scope captures rate-limiting errors (specifically `ratelimitexceeded`).
- It then overrides the `Retry-After` header to provide a timestamp (`DateTime.UtcNow` plus the `retryAfter` seconds).
- This approach ensures only throttled responses include the modified header value.
- See the [API Management policy error documentation](https://learn.microsoft.com/en-us/azure/api-management/api-management-error-handling-policies#predefined-errors-for-policies) for more on APIM error handling.

## Benefits

- Enhanced transparency and flexibility for API consumers.
- Ability to meet advanced or custom client requirements.
- Demonstrates how APIM policies can be tailored beyond default settings.

## Conclusion

Customizing the `Retry-After` header in APIM helps elevate the developer and client experience. By handling errors using on-error scopes and rate-limit details, you can provide clear, timestamped instructions on when clients can safely retry. This flexibility helps your API remain robust, compliant with customer needs, and user-centric.

---

## References

- [Azure API Management policy reference - rate-limit | Microsoft Learn](https://learn.microsoft.com/en-us/azure/api-management/rate-limit-policy)
- [Error handling in Azure API Management policies | Microsoft Learn](https://learn.microsoft.com/en-us/azure/api-management/api-management-error-handling-policies#predefined-errors-for-policies)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/transforming-retry-after-headers-in-azure-apim-a-step-by-step/ba-p/4489762)
