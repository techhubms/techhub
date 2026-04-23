As part of our ongoing efforts to enhance the security of Azure Notification Hubs, we are making an important change to the expiry time of the generated URIs. Starting from October 2024, the expiry time of **PnsErrorDetailsUri** will be reduced from 10 days to 5 days. This means that the **PnsErrorDetailsUri** returned will now be valid for only 5 days and will expire thereafter.

The **PnsErrorDetailsUri** is returned from the Get Notification Message Telemetry API, which provides additional telemetry on the finished states of outgoing push notifications. Learn more about Get Notification Message Telemetry API [here](https://learn.microsoft.com/en-us/rest/api/notificationhubs/get-notification-message-telemetry).

Consumers of this API are requested to take note of this change and make the necessary adjustments for backward compatibility.

Please note that for the Get Platform Notification Services (PNS) Feedback API, the container URI returned will continue to have an expiry time of 1 day, as before.

Thank you for choosing Azure Notification Hubs for your push notification needs. Be sure to leave your feedback in the comments below, and feel free to [submit your feature ideas](https://feedback.azure.com/d365community/forum/405a1b30-8b26-ec11-b6e6-000d3a4f0789).