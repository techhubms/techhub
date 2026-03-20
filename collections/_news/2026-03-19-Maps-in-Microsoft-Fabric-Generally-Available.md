---
feed_name: Microsoft Fabric Blog
section_names:
- ai
- azure
- ml
external_url: https://blog.fabric.microsoft.com/en-US/blog/maps-in-microsoft-fabric-generally-available/
title: Maps in Microsoft Fabric (Generally Available)
tags:
- Aggregations
- AI
- Azure
- Azure Maps
- Cloud Optimized GeoTIFF
- Entities And Relationships
- Environmental Data
- Eventhouse
- Fabric IQ
- Fabric Ontology
- Geospatial Analytics
- KQL Database
- Lakehouse
- Maps in Fabric
- Materialized Views
- Microsoft Fabric
- Microsoft Planetary Computer Pro
- ML
- News
- OneLake
- Ontology Modeling
- Raster Data
- Real Time Intelligence
- Scheduled Refresh
- Spatial Joins
- Vector Tiles
- Vegetation Index
- WMS
- WMTS
date: 2026-03-19 16:30:00 +00:00
primary_section: ai
author: Microsoft Fabric Blog
---

The Microsoft Fabric Blog announces general availability of Maps in Microsoft Fabric, highlighting Ontology support in Fabric IQ, new data connections (including Microsoft Planetary Computer Pro and WMS/WMTS), and operational features like scheduled vector tile refresh for real-time geospatial scenarios.<!--excerpt_end-->

# Maps in Microsoft Fabric (Generally Available)

*If you haven’t already, check out Arun Ulag’s hero blog “*[FabCon and SQLCon 2026: Unifying databases and Fabric on a single, complete platform](https://aka.ms/FabCon-SQLCon-2026-news)*” for a complete look at all of our FabCon and SQLCon announcements across both Fabric and our database offerings.*

## Everything happens somewhere

Maps in Microsoft Fabric is now **generally available**, after:

- Preview introduction at **FabCon Europe 2025**
- Additional features added at **Ignite 2025**
- GA announcement at **FabCon Atlanta**

The stated goal is to enable “any data citizen” to analyze data in **time and space** without specialized geospatial knowledge, and to create and share map-centric applications.

## Introducing support for Ontology

Maps in Fabric supports:

- **High-velocity data** in **Eventhouse**
- **Large quantities of data** in **Lakehouse**

With **Ontology in Fabric IQ**, Maps and AI agents can reason about a business using **entities and relationships** (not just tables/schemas). These entities can include:

- Locations
- Areas
- Paths (movement)

Maps in Fabric now supports **static and time series entities** defined in **Fabric Ontology**, so you can model real-world concepts once (for example: venues, cities, routes, parking areas, buses) and use them across **agents** and **Maps**.

![2 Screenshots of Maps in Fabric connected to Ontology in Fabric IQ as well as a Graph representation of the Ontology. The Map shows bus routes, reel-time bus locations and landmarks. The Ontology is visualized through it Graph views with entities and relationships.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/2-screenshots-of-maps-in-fabric-connected-to-ontol.png)

*Figure: Maps in Fabric connected to Ontology*

## Connect to your data wherever it lives

Building on support for **Cloud Optimized GeoTIFF (COG)** in **OneLake** (announced at Ignite 2025), Maps in Fabric can now connect to:

- Imagery, environmental data, vegetation index, and more in [Microsoft Planetary Computer Pro](https://azure.microsoft.com/products/planetary-computer-pro)
- Standards-based raster services:
  - **WMS**
  - **WMTS**

![Screenshot of Maps in Fabric connected to real-time weather from Azure Maps through a Web Map Tile Service connection.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-maps-in-fabric-connected-to-real-tim.png)

*Figure: Maps in Fabric connected to WMS, WMTS services and Microsoft Planetary Computer Pro*

### Schedule your data refresh

Maps in Fabric uses **vector tiles** to optimize large datasets for browser consumption.

A new scheduler lets you:

- Define layers once
- Refresh tile sets as often as needed (examples given: daily construction updates, or every five minutes for event operations)

![Screenshot of a dialog to configure scheduled refreshes for tile sets in Maps in Fabric.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-of-a-dialog-to-configure-scheduled-refr.png)

*Figure: Schedule tile set refresh in Maps in Fabric*

## Additional improvements

User experience improvements include:

- **Data-driven styling** to speed up layer design
- **Filter within a layer** to focus on relevant items
- **Built-in and custom icons** for clearer storytelling and faster interpretation

For **Eventhouse** as a data source, Maps in Fabric now supports:

- **Functions**
- **Materialized views**

This enables more complex logic including **spatial joins** and **aggregations**.

## From global to local

Example scenario: a global sports event using Maps in Fabric with:

- A shared **ontology** defining teams, matches, venues, cities, transport routes/vehicles, parking, fan zones, hospitality, and support services
- **Real-time operational signals** streaming into **KQL Databases** (crowd flow, transit status, parking occupancy)
- **Up-to-date imagery** from **Microsoft Planetary Computer Pro** for construction progress
- **Scheduled tile sets** to keep maps fresh under peak load
- **Custom icons and styling** to guide operators to what matters

With Maps in Fabric, the post states that anyone can build and share map-centric applications.

![Animated GIF showing several screenshots of Maps in Fabric scenes.
1. Density of fans in front of a football stadium visualized as heatmap.
2. Overlay of drone imagery from a connection to Microsoft Planetary Computer Pro.
3. Overlay of real-time weather from a Web Map Tile Service Connection connection to Azure Maps.
4. Available parking space represented as extruded polygons.
5. Stadium Seats color coded by availability for the next game.
6. Overlay of color codes public transport routes and real-time bus locations](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/animated-gif-showing-several-screenshots-of-maps-i.gif)

*Figure: Real-Time insights with Maps in Fabric*

## Get started

- [Explore the Maps](https://aka.ms/StartWithFabricMaps) experience in Real-Time Intelligence.
- Model your [spatial properties with Ontology](https://aka.ms/MapsInFabricOntology) in Fabric IQ.
- Bring in your own [imagery](https://aka.ms/MapsInFabricWMTS) and real-time data.
- [Share](https://learn.microsoft.com/fabric/real-time-intelligence/map/share-map-direct) insights across your organization.


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/maps-in-microsoft-fabric-generally-available/)

