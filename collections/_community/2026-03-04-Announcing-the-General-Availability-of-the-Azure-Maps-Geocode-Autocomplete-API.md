---
layout: "post"
title: "Announcing the General Availability of the Azure Maps Geocode Autocomplete API"
description: "This post introduces the general availability of the Azure Maps Geocode Autocomplete API, a REST service that provides predictive and structured location suggestions for applications. It covers new features in the GA release, technical capabilities for developers, usage workflows, sample API requests, and resources to help migrate from Bing Maps Autosuggest."
author: "sinnypan"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/azure-maps-blog/announcing-the-general-availability-of-the-azure-maps-geocode/ba-p/4499242"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2026-03-04 17:22:55 +00:00
permalink: "/2026-03-04-Announcing-the-General-Availability-of-the-Azure-Maps-Geocode-Autocomplete-API.html"
categories: ["Azure", "Coding"]
tags: ["Address Search", "API", "Autocomplete", "Azure", "Azure Maps", "Bing Maps Migration", "Coding", "Community", "Developer Workflow", "Entity Suggestions", "GA Release", "Geocoding", "Location Services", "Multilingual", "REST", "Result Ranking", "Typeahead", "Web Application Integration"]
tags_normalized: ["address search", "api", "autocomplete", "azure", "azure maps", "bing maps migration", "coding", "community", "developer workflow", "entity suggestions", "ga release", "geocoding", "location services", "multilingual", "rest", "result ranking", "typeahead", "web application integration"]
---

sinnypan announces the GA release of Azure Maps Geocode Autocomplete API, detailing its new capabilities, API usage, and how developers can create fast, structured location search in their applications.<!--excerpt_end-->

# Announcing the General Availability of the Azure Maps Geocode Autocomplete API

**Author: sinnypan**

The Azure Maps Geocode Autocomplete API is now generally available as a production-ready REST service for intelligent and structured location suggestions. After successful public preview and community feedback, this release delivers stability, scalability, and developer-friendly enhancements.

## Why This API Matters

Modern applications increasingly require real-time, relevant location suggestions for use cases like store locators, delivery routing, address entry, and dynamic map UIs.

**API Highlights**:

- Instant, context-aware typeahead suggestions
- Flexible result filtering by category (Place, Address) and subtypes
- Structured outputs for easy application integration
- Multilingual and localized ranking support
- Proximity and popularity-aware responses

The API offers a successor path from the Bing Maps Autosuggest REST API, supporting both modernization and greenfield apps.

## What's New in GA

- **Stable API Version:** Now available at `2026-01-01`, supporting reliable production workloads
- **Improved Ranking & Language Handling:** Enhanced geo-awareness and accurate, multilingual suggestions
- **Refreshed Docs & Samples:** Updated [REST API documentation](https://learn.microsoft.com/en-us/rest/api/maps/search/get-geocode-autocomplete?view=rest-maps-2026-01-01&tabs=HTTP), [developer samples](https://samples.azuremaps.com/?search=autocomplete), and [how-to guides](https://learn.microsoft.com/en-us/azure/azure-maps/how-to-search-for-address?pivots=search-latest)

## Core Capabilities

- **Entity suggestions:** Places (e.g., admin divisions, cities, landmarks, postcodes) and Addresses (e.g., roads, street addresses)
- **Consistent structure:** Output with components suitable for downstream logic
- **Relevance-based ranking:** Uses input, popularity, proximity, and map bounding boxes
- **Multilingual support:** Honors Accept-Language for user localization
- **Targeted filtering:** Restrict by country, type group, or entity type

## How to Use the Geocode Autocomplete API

**Production Endpoint:**

```
https://atlas.microsoft.com/search/geocode:autocomplete?api-version=2026-01-01&query={query}
```

**Key parameters:**

- `coordinates`: Bias suggestions based on user/device location
- `bbox`: Prefer results within a visible map area
- `top`: Limit returned suggestions
- `resultTypeGroups` / `resultTypes`: Filter to Places, Addresses, and subtypes
- `countryRegion`: Restrict results to a region

**Basic developer workflow:**

1. Call the autocomplete API as user types input
2. Use the selected suggestion with the [Azure Maps geocoding service](https://learn.microsoft.com/en-us/rest/api/maps/search/get-geocoding?view=rest-maps-2026-01-01&tabs=HTTP) to retrieve complete address and coordinates

**Example: Place Entity Autocomplete**

```
GET https://atlas.microsoft.com/search/geocode:autocomplete?api-version=2026-01-01&subscription-key={YourAzureMapsKey}&coordinates={coordinates}&query=new yo&top=3
```

*Returns street/region suggestions like New York City or New York State based on partial input and proximity.*

**Example: Address Entity Autocomplete**

```
GET https://atlas.microsoft.com/search/geocode:autocomplete?api-version=2026-01-01&subscription-key={YourAzureMapsKey}&bbox={bbox}&query=One Micro&top=3&countryRegion=US
```

*Returns address suggestions such as NE One Microsoft Way for US-based queries.*

## Integrating With Web Applications

Developers can integrate the autocomplete API into search forms, address validation, or routing experiences. The API delivers structured location objects for downstream map display or geocoding workflows.

## Migration & Next Steps

- **Migration support:** Updated guides streamline the path from Bing Maps Autosuggest REST to Azure Maps.
- **Resource links:**
  - [Geocode Autocomplete REST API Documentation](https://learn.microsoft.com/en-us/rest/api/maps/search/get-geocode-autocomplete?view=rest-maps-2026-01-01&tabs=HTTP)
  - [How-to-Guide for Azure Maps Search Service](https://learn.microsoft.com/en-us/azure/azure-maps/how-to-search-for-address?pivots=search-latest)
  - [Sample repository](https://samples.azuremaps.com/?search=autocomplete)
  - [Bing Maps to Azure Maps migration](https://learn.microsoft.com/en-us/azure/azure-maps/migrate-bing-maps-overview)
  - [Managing Azure Maps API Keys](https://learn.microsoft.com/en-us/azure/azure-maps/how-to-manage-account-keys)

## Conclusion

The GA release of Azure Maps Geocode Autocomplete API equips developers with a reliable, performant, and flexible way to add typeahead and address suggestion experiences to their apps, with robust support for localization and filtering.

If you've used the API or migrated from Bing Maps, share your feedback and continue building high-quality location experiences with Azure Maps.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-maps-blog/announcing-the-general-availability-of-the-azure-maps-geocode/ba-p/4499242)
