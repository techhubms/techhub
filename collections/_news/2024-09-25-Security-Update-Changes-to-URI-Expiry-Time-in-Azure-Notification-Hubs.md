---
external_url: https://devblogs.microsoft.com/azure-notification-hubs/security-update-changes-to-uri-expiry-time-in-azure-notification-hubs/
title: 'Security Update: Changes to URI Expiry Time in Azure Notification Hubs'
author: Sreehari R
viewing_mode: external
feed_name: Microsoft DevBlog
date: 2024-09-25 09:35:26 +00:00
tags:
- API Changes
- Azure Notification Hubs
- Backward Compatibility
- Get Notification Message Telemetry API
- Notification Hubs
- Platform Notification Services Feedback
- PnsErrorDetailsUri
- Push Notifications
- Security Update
- URI Expiry
section_names:
- azure
- security
---
Authored by Sreehari R, this update details upcoming changes to Azure Notification Hubs' URI expiry policy, focusing on improved security and guidance for users.<!--excerpt_end-->

## Security Update: Changes to URI Expiry Time in Azure Notification Hubs

**Author:** Sreehari R

### Overview

In an effort to further enhance the security of Azure Notification Hubs, Microsoft is introducing a modification to the expiry period of generated URIs. Beginning in October 2024, the expiry time for the `PnsErrorDetailsUri` will decrease from 10 days to 5 days. This measure aims to reduce the exposure window in which these URIs are valid, aligning with best practices in API security and resource management.

### What Is Changing?

- **Affected URI:** `PnsErrorDetailsUri`
- **Affected API:** Get Notification Message Telemetry API
- **Change:** Expiry time reduced from **10 days** to **5 days**
- **Effective date:** October 2024

After 5 days, any `PnsErrorDetailsUri` provided by the API will expire and become invalid. This adjustment necessitates that consumers of the API review and update their integrations to ensure compatibility with the new expiry window.

### Details on the API

- The `PnsErrorDetailsUri` is returned by the [Get Notification Message Telemetry API](https://learn.microsoft.com/en-us/rest/api/notificationhubs/get-notification-message-telemetry), which provides telemetry and insights on the states of outgoing push notifications.

### Action Required

- **Consumers of the API:**
  - Review any automated processes, scripts, or manual workflows that rely on the `PnsErrorDetailsUri`, ensuring they operate within the new 5-day expiry period.
  - Test for compatibility and reliability before October 2024 to avoid disruptions.

### Unchanged Behavior

- For the **Get Platform Notification Services (PNS) Feedback API**, the container URI will **continue to have an expiry period of 1 day**. No changes are being made to this aspect.

### Feedback

Customers are encouraged to leave feedback or submit feature ideas via the official [feedback portal](https://feedback.azure.com/d365community/forum/405a1b30-8b26-ec11-b6e6-000d3a4f0789).

Thank you for choosing Azure Notification Hubs as your push notification platform.

---

**Related resources:**

- [Get Notification Message Telemetry API Documentation](https://learn.microsoft.com/en-us/rest/api/notificationhubs/get-notification-message-telemetry)

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-notification-hubs/security-update-changes-to-uri-expiry-time-in-azure-notification-hubs/)
