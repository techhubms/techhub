---
external_url: https://techcommunity.microsoft.com/t5/azure-maps-blog/introducing-the-azure-maps-geocode-autocomplete-api/ba-p/4455784
title: Introducing the Azure Maps Geocode Autocomplete API
author: sinnypan
feed_name: Microsoft Tech Community
date: 2025-09-23 09:26:24 +00:00
tags:
- Address Autocomplete
- API Integration
- Autocomplete
- Azure Maps
- Billing
- Developer Tools
- Endpoint
- Entity Suggestions
- Geocode Autocomplete API
- Location Intelligence
- Mapping
- Microsoft Azure
- Multilingual Support
- Place Suggestions
- REST API
- Web Application Development
section_names:
- azure
- coding
---
sinnypan details the launch and features of the Azure Maps Geocode Autocomplete API, showing developers how to integrate real-time, structured address and place suggestions into mapping platforms and web applications.<!--excerpt_end-->

# Introducing the Azure Maps Geocode Autocomplete API

The Azure Maps Geocode Autocomplete API is now available as a public preview, offering developers a new REST-based solution for implementing advanced autocomplete capabilities in Microsoft’s mapping ecosystem.

## Why Autocomplete Matters

Autocomplete transforms user experience by delivering instant, relevant suggestions for locations and addresses as users type. Reliable autocomplete improves workflows across:

- **Store locators** (quickly suggest and center on relevant branches)
- **Rideshare and dispatching platforms** (accurate, fast pickup/drop-off address entry)
- **Delivery services** (validate deliverable addresses within target regions)
- **Any web UI requiring structured location input** (form autofill, real estate, etc.)

## API Features & Capabilities

- **Supports both Place and Address entities**: Suggests populated places, administrative districts, landmarks, roads, and point addresses.
- **Relevance ranking**: Results can be tailored by entity popularity, user coordinates, or a bounding box for contextual suggestions.
- **Structured output**: Returns detailed address components for direct integration.
- **Multilingual support**: Responses can be provided in different languages using Accept-Language.
- **Flexible filtering**: Limit results by country/region or entity subtype (e.g., only show postal codes).

## How It Works

Developers call the API via:

```
https://atlas.microsoft.com/search/geocode:autocomplete?api-version=2025-06-01-preview
```

**Example usage:**

- Place suggestion for "new yo":
  - Returns "New York City" and other matches with structured metadata.
- Address suggestion for "One Micro" in the US:
  - Returns full address "NE One Microsoft Way, Redmond, WA 98052, United States" with all associated metadata.

Integration is as simple as providing a subscription key, user query, and optional filters in the request parameters. Returned data can be plugged directly into applications or used for mapping/disptach.

## Pricing

The Geocode Autocomplete API uses the same metering model as the Azure Maps Search service. Ten autocomplete requests count as one billable transaction, providing transparent and predictable billing for developers familiar with Azure Maps.

## Real-World Scenarios

- Enhance address forms with dynamic suggestions
- Improve speed and accuracy for ride and delivery dispatch systems
- Add intelligent autocomplete to any mapping-based web application

## Resources

- [Geocode Autocomplete REST API Documentation](https://aka.ms/azmgeocodecompleteapidoc)
- [Geocode Autocomplete Samples](https://aka.ms/azmsamplegeocodeautocomplete)
- [Migrate from Bing Maps to Azure Maps](https://learn.microsoft.com/en-us/azure/azure-maps/migrate-bing-maps-overview)
- [How to use Azure Maps APIs](https://learn.microsoft.com/en-us/azure/azure-maps/how-to-manage-account-keys)

## Summary

The Azure Maps Geocode Autocomplete API simplifies delivering high-performance, relevant, and structured location suggestions in modern applications. Developers can now build smarter, more interactive mapping experiences quickly and reliably.

---

*Authored by sinnypan*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-maps-blog/introducing-the-azure-maps-geocode-autocomplete-api/ba-p/4455784)
