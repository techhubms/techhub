---
layout: "post"
title: "Challenges with Application Insights Diagnostic Settings and Stream Analytics File Updates"
description: "This community post discusses issues with exporting Application Insights pageview data to a database via Azure Stream Analytics. The main challenge is that diagnostic settings write logs for an entire hour to a single file in Azure Storage, but Stream Analytics processes the file as soon as it's created, missing subsequent appended data. The author seeks solutions for file write timing and improved integration approaches."
author: "GarseBo"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/AZURE/comments/1mjx1ga/application_insights_diagnostic_setting_to/"
viewing_mode: "external"
feed_name: "Reddit Azure"
feed_url: "https://www.reddit.com/r/azure/.rss"
date: 2025-08-07 11:02:14 +00:00
permalink: "/2025-08-07-Challenges-with-Application-Insights-Diagnostic-Settings-and-Stream-Analytics-File-Updates.html"
categories: ["Azure", "ML"]
tags: ["Application Insights", "Azure", "Azure Storage", "Azure Stream Analytics", "Community", "Data Export", "Data Pipeline", "Diagnostic Settings", "Log Export", "Log File Appending", "ML", "Monitoring", "Pageviews", "ResourceID Path", "Storage Account", "Streaming", "Telemetry"]
tags_normalized: ["application insights", "azure", "azure storage", "azure stream analytics", "community", "data export", "data pipeline", "diagnostic settings", "log export", "log file appending", "ml", "monitoring", "pageviews", "resourceid path", "storage account", "streaming", "telemetry"]
---

GarseBo describes challenges with Azure Application Insights diagnostic settings writing logs to a single file per hour, causing Stream Analytics to process incomplete data. The post focuses on possible workarounds and improvements.<!--excerpt_end-->

# Challenges with Application Insights Diagnostic Settings and Stream Analytics File Updates

**Posted by GarseBo**

## Context

- **Goal:** Export pageviews from Azure Application Insights to a database.
- **Method:** Previously used Azure Stream Analytics to process data exported to a Storage Account via Application Insights diagnostic settings.

## Issue Details

- **Diagnostic Setting Behavior:**
  - Logs are written to a path like `y={datetime:yyyy}/m={datetime:MM}/d={datetime:dd}/h={datetime:HH}/m={datetime:mm}`.
  - The minute directory is always `00`, meaning one file is used for the whole hour.
  - New pageviews for the hour are appended to this same file.
- **Stream Analytics Challenge:**
  - The Stream Analytics job processes this file as soon as it appears, typically capturing only initial entries.
  - Subsequent updates to the file are ignored, so most data is unprocessed.

## Questions Raised

- *Can I change the diagnostic setting to only write a file after the hour is complete, or have it create a new file every minute?*
- *Is there a way to configure the log file creation behavior for more granular ingestion?*

## Observations / Frustrations

- Application Insights stores 6 months of data by default.
- It's possible to retrieve pageview events directly via API at export time as an alternative.
- The log export workflow seems complex for a simple pageview export scenario.

## Possible Approaches

- Review diagnostic setting configuration for possible settings (as of recent Azure documentation, customization of file interval is limited).
- Consider an alternative export pipeline using the Application Insights REST API or continuous export.
- For Stream Analytics, investigate options for reprocessing files or using other triggers.

---

**Key Takeaway:**
The author highlights a practical integration challenge between telemetry log generation and downstream streaming analytics on Azure, triggering a discussion on improving data pipeline reliability and configuration.

This post appeared first on "Reddit Azure". [Read the entire article here](https://www.reddit.com/r/AZURE/comments/1mjx1ga/application_insights_diagnostic_setting_to/)
