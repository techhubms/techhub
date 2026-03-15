---
external_url: https://techcommunity.microsoft.com/t5/azure-maps-blog/azure-maps-understanding-view-vs-routing-coordinates/ba-p/4483532
title: 'Azure Maps: Understanding View vs. Routing Coordinates'
author: IoTGirl
feed_name: Microsoft Tech Community
date: 2026-01-06 20:27:42 +00:00
tags:
- Access Points
- API Usage
- Azure Maps
- Display Coordinates
- Geocoding
- Location Intelligence
- Map UI
- Navigation
- Routing
- Spatial Data
- Azure
- Community
section_names:
- azure
primary_section: azure
---
IoTGirl from Microsoft breaks down how Azure Maps provides both view and routing coordinates, detailing why each matters for developers and the pitfalls of misusing them in applications.<!--excerpt_end-->

# Azure Maps: Understanding View vs. Routing Coordinates

When geocoding with Azure Maps, you might assume that every address yields a single coordinate. However, Azure Maps intentionally produces two different coordinates for each location: one intended for map display (view/display coordinate) and another for navigation (routing/access coordinate).

If you've tried to plan a driving route to a large venue or complex, you may have noticed odd behavior, like being routed to the center of a mall or the middle of a national park—places vehicles can't really access. That’s because the coordinate used didn’t represent the real-world entrance or access point.

## Why Two Coordinates? Visual Context vs. Real-world Access

Azure Maps geocoding APIs are designed to serve two fundamental geospatial needs:

1. **Display Accuracy:** Place a map pin or center a map at the most visually representative spot—often the geometric center or main point-of-interest (POI).
2. **Routing Accuracy:** Guide vehicles or pedestrians to the actual, legal access point—like a driveway, main entrance, or other point on the road network.

For many large or complex locations, the ideal center for display may be far from a road or a viable arrival spot. Using this for navigation leads to confusing or even non-functional routes.

## The View Coordinate: Map Display Precision

The "view coordinate" is the default map anchor: the centermost point of a building, parcel, or POI. Azure Maps uses this for tasks like:

- Dropping pins on the map
- Centering on search results
- Spatial analysis (e.g., clustering, proximity searches)

**Limitations:** View coordinates aren’t guaranteed near a drivable road. They work visually but are unreliable for trip endpoints.

## The Routing Coordinate: Getting There Safely

The "routing coordinate" represents where a person or vehicle can actually approach the POI via streets or pathways. It’s snapped to a legal road segment, considering access, entrances, and orientation—vital for accurate ETAs and route instructions.

Use cases include:

- Route calculations
- Travel time and distance analysis
- Delivery, pickup, and fleet management
- Multi-stop itinerary planning

If a view coordinate is given to a routing API, results may be inaccurate or unusable—like routing to a mall’s center instead of a parking lot.

## How Azure Maps Returns These Coordinates

In Azure Maps API search and geocode responses, you’ll see these properties:

- `"usageTypes": [ "Display" ]` or `position/displayPosition` → **View Coordinate**
- `"usageTypes": [ "Route" ]` or `routePosition/accessPoint/entryPoints` → **Routing Coordinate**

Both types are returned so developers can pick the right one based on use case.

## Consequences of Mixing Them Up

- **Routing with a View Coordinate:** May place users inside the wrong part of a building, on the wrong street, or in an unreachable zone.
- **Displaying a Routing Coordinate:** Pins might appear off-center or detached from landmark geometry.

**Mental Model:**

- For anything UI or visualization-related, use the *view* coordinate.
- For routing, navigation, or driveway/entry access, use the *routing* coordinate.

## Practical Advice and Further Reading

Whenever your solution involves travel time, movement, routing, or logistics, always feed the routing coordinate into mapping engines. For simple display, stick with the view coordinate.

Learn more:

- [Azure Maps Geocoding Documentation](https://learn.microsoft.com/en-us/azure/azure-maps/how-to-search-for-address?pivots=search-latest)
- [Azure Maps Geocoder API Reference](https://learn.microsoft.com/en-us/rest/api/maps/search/get-geocoding)

By leveraging both coordinate types as intended, you can build applications that are both visually clear and operationally reliable for users.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-maps-blog/azure-maps-understanding-view-vs-routing-coordinates/ba-p/4483532)
