---
layout: post
title: Introducing the Azure Maps Geocode Autocomplete API
author: sinnypan
canonical_url: https://techcommunity.microsoft.com/t5/azure-maps-blog/introducing-the-azure-maps-geocode-autocomplete-api/ba-p/4455780
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-09-22 03:33:28 +00:00
permalink: /coding/community/Introducing-the-Azure-Maps-Geocode-Autocomplete-API
tags:
- Address Lookup
- API Integration
- Autocomplete
- Azure Maps
- Bing Maps Migration
- Entity Suggestions
- Form Autofill
- Geocode Autocomplete API
- Location Intelligence
- Mapping
- Microsoft Azure
- Multilingual Support
- REST API
- Rideshare
- Store Locator
- Web Application
section_names:
- azure
- coding
---
sinnypan introduces the Azure Maps Geocode Autocomplete API, detailing its enhanced autocomplete capabilities, technical features, and integration scenarios for developers working with location data.<!--excerpt_end-->

# Introducing the Azure Maps Geocode Autocomplete API

The Azure Maps team has unveiled the Geocode Autocomplete API—a REST service crafted to modernize and upgrade autocomplete capabilities across Microsoft’s mapping platforms.

## Why This API Matters

The Geocode Autocomplete API steps in as the next-generation solution replacing the Bing Maps Autosuggest REST API, following the deprecation of Bing Maps Enterprise. It's engineered for use cases requiring fast, intelligent location suggestions, such as:

- Store locators
- Rideshare and dispatching platforms
- Form autofill and address entry
- Web UIs needing location input
- Real estate search and delivery applications

## Key Capabilities

- **Entity Suggestions**: Supports identification of both Place entities (geographical regions, administrative districts, populated places, landmarks, postal codes) and Address entities (roads, point addresses).
- **Ranking Options**: Results can be prioritized by entity popularity, user location (coordinates), and a bounding box.
- **Structured Output**: Suggestions returned with structured address formats for seamless integration.
- **Multilingual Support**: Choose query language preferences via the Accept-Language parameter.
- **Flexible Filtering**: Further refine suggestions by country, region, or result type (e.g., restrict results to postal codes, addresses, or other entity subtypes as required for your application's UX).

## How to Use the API

The service is accessed via the endpoint:

```
https://atlas.microsoft.com/search/geocode:autocomplete
```

### Example: Place Entity Autocomplete

```http
GET https://atlas.microsoft.com/search/geocode:autocomplete?api-version=2025-06-01-preview&subscription-key={YourAzureMapsKey}&query=new yo&countryRegion=US
```

This returns structured suggestions (such as "New York City") matching the partial input.

### Example: Address Entity Autocomplete

```http
GET https://atlas.microsoft.com/search/geocode:autocomplete?api-version=2025-06-01-preview&subscription-key={YourAzureMapsKey}&query=10 Downing st&countryRegion=GB
```

Returns address suggestions (e.g., "10 Downing Street" as an Address entity).

### Integrating With Web Applications

Developers can wire the API to autocomplete fields, providing real-time, location-based suggestions in form entries and UIs. The API’s structured and filterable output ensures robust address and place selection experiences, suitable for global applications.

## Why Now?

The new Geocode Autocomplete API offers precision, flexibility, and performance for a variety of real-world mapping scenarios. Whether building a dynamic dispatch system, a store locator, or enhancing address entry forms, this API can unlock richer, more interactive mapping experiences.

## Get Started

- [Geocode Autocomplete REST API Documentation](https://aka.ms/azmgeocodecompleteapidoc)
- [API Usage Samples](https://aka.ms/azmsamplegeocodeautocomplete)
- [Managing Azure Maps API Keys](https://learn.microsoft.com/en-us/azure/azure-maps/how-to-manage-account-keys)

_Last updated: September 22, 2025_

---

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-maps-blog/introducing-the-azure-maps-geocode-autocomplete-api/ba-p/4455780)
