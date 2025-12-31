---
layout: "post"
title: "Disabling Recording of an Activity (span) in .NET OpenTelemetry Instrumentation"
description: "Steve Gordon explores how to programmatically disable the recording and export of Activities (spans) in .NET when instrumenting code with OpenTelemetry. He details the reasoning, mechanics, and implementation behind selectively avoiding trace recording for unneeded or invalid requests, including sample code and design considerations."
author: "Steve Gordon"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.stevejgordon.co.uk/disabling-recording-of-an-activity-span-in-dotnet-opentelemetry-instrumentation"
viewing_mode: "external"
feed_name: "Steve Gordon's Blog"
feed_url: "https://www.stevejgordon.co.uk/feed"
date: 2024-09-18 11:30:07 +00:00
permalink: "/blogs/2024-09-18-Disabling-Recording-of-an-Activity-span-in-NET-OpenTelemetry-Instrumentation.html"
categories: ["Coding", "DevOps"]
tags: [".NET", "Activity", "ActivityTraceFlags", "ASP.NET Core", "Coding", "DevOps", "Instrumentation", "IsAllDataRequested", "Middleware", "Observability", "OpenTelemetry", "Blogs", "System.Diagnostics"]
tags_normalized: ["dotnet", "activity", "activitytraceflags", "aspdotnet core", "coding", "devops", "instrumentation", "isalldatarequested", "middleware", "observability", "opentelemetry", "blogs", "systemdotdiagnostics"]
---

In this technical post, Steve Gordon shares practical guidance on disabling the recording of Activities (spans) in .NET OpenTelemetry instrumentation, highlighting both the motivation and code implementation.<!--excerpt_end-->

## Disabling Recording of an Activity (span) in .NET OpenTelemetry Instrumentation

**Author: Steve Gordon**

![OpenTelemetry Activity Header](https://www.stevejgordon.co.uk/wp-content/uploads/2024/09/Disabling-Recording-of-an-Activity-span-in-.NET-OpenTelemetry-Instrumentation-750x410.png)

### Introduction

Steve Gordon, a .NET engineer at Elastic, details his experience building hobby projects to evaluate Elastic's observability tooling, focusing on pain points related to the .NET instrumentation libraries (System.Diagnostics) for OpenTelemetry compatibility.

### Recording Activities in .NET

In .NET, the `Activity` class represents a span within an OpenTelemetry trace. Typically, the recording and exporting of these spans is determined early, for example when handling incoming HTTP requests in ASP.NET Core. Head sampling is commonly used—such as ratio-based tracing—which controls what percentage of traces are sampled to balance cost and completeness.

If a trace is sampled (the default behavior in the OpenTelemetry SDK), an `Activity` is created per incoming request. Child activities may be created for sub-operations, e.g., HTTP calls or database activity. However, nuances arise:

- Tracing decisions can be overridden by upstream services via W3C headers.
- There are differences between recording a trace and sampling it for export.

### Requirement and Use Case

Steve encountered a scenario where he needed to control programmatically if a span (`Activity`) should be exported. In an ASP.NET Core middleware designed for a callback endpoint, he validated requests against query parameters and an IP allowlist. Invalid requests were either mistakes or potentially malicious, and he wanted to avoid recording or incurring costs for such traces.

### Implementation

To prevent recording/exporting an `Activity` after an initial positive sampling decision, only a couple of code lines are needed. Key technical points:

- The `Activity` object has an `ActivityTraceFlags` property (a [Flags] enum), with a `Recorded` bit indicating export eligibility.
- The framework and libraries set trace flags based on sampling.
- The `IsAllDataRequested` property signals whether enrichment (tags, links, events) should be captured.

**Sample Implementation**:

```csharp
var activity = Activity.Current;
if (activity is not null)
{
    activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
    activity.IsAllDataRequested = false;
}
Activity.Current = null;
```

This snippet ensures the current activity is not recorded:

- The `Recorded` flag is unset using a bitwise operation.
- Data enrichment is skipped by setting `IsAllDataRequested` to false.
- Setting `Activity.Current` to null ensures no child spans will attach to the ignored parent activity.

### Alternative Designs

An alternative is to use a filtering `Processor` in the OpenTelemetry SDK (e.g., `MyFilteringProcessor`). With this, a custom property can be checked during processing to decide export eligibility.

Advantages and disadvantages:

- *Processor approach:* Disables recording at the end of an activity, not as early. May result in unnecessary enrichment or overhead.
- *Direct approach:* Early check avoids all overhead, better for scenarios with potentially malicious or frequent invalid requests.

### Conclusion

The required code to prevent an `Activity` from being recorded is simple, but the underlying mechanics may not be obvious. This approach is useful for .NET applications using OpenTelemetry where granular control is needed over trace exporting, especially for cost or security reasons.

> "Hopefully, this post helps if you find yourself with a similar requirement." — Steve Gordon

---

**About the Author**

Steve Gordon is a Pluralsight author, 7-time Microsoft MVP, and a .NET engineer at Elastic. He maintains the .NET APM agent, contributes to .NET open-source projects, and leads community initiatives, including the .NET South East Meetup group.

---

If you found this post helpful, consider supporting Steve via [Buy Me a Coffee](https://www.buymeacoffee.com/stevejgordon) or [PayPal](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=WV4JPPV9FS34L&source=url).

This post appeared first on "Steve Gordon's Blog". [Read the entire article here](https://www.stevejgordon.co.uk/disabling-recording-of-an-activity-span-in-dotnet-opentelemetry-instrumentation)
