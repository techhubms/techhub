---
external_url: https://techcommunity.microsoft.com/t5/azure-maps/could-not-load-image-because-of-out-of-range-source-coordinates/m-p/4441495#M153
title: Intermittent Image Load Errors with Azure Maps Control in Angular
author: JeroenBooij
feed_name: Microsoft Tech Community
date: 2025-08-08 10:20:30 +00:00
tags:
- Angular
- Angular Modules
- AuthenticationType.subscriptionKey
- Azure Maps
- Azure Maps Control
- Image Load Error
- JavaScript
- JPEG
- Map Initialization
- Ngafterviewinit
- PNG
- SDK 2.3.7
- SVG Not Supported
- TypeScript
- Azure
- Coding
- Community
section_names:
- azure
- coding
primary_section: coding
---
JeroenBooij highlights an intermittent error encountered when initializing Azure Maps Control in an Angular application. This discussion offers firsthand insights into troubleshooting and handling SDK image loading issues.<!--excerpt_end-->

# Troubleshooting Image Load Errors in Azure Maps with Angular

**Author: JeroenBooij**

When working with the [`azure-maps-control`](https://github.com/Azure/azure-maps-control-js) SDK (version 2.3.7) in an Angular application, you may encounter an intermittent error:

> *"Could not load image because of out of range source coordinates for image copy. Please make sure to use a supported image type such as PNG or JPEG. Note that SVGs are not supported."*

## Context

- **Framework**: Angular
- **Map SDK**: Azure Maps Control 2.3.7
- **Lifecycle Event**: `ngAfterViewInit` used to initialize the map.
- **SDK Initialization**:

  ```typescript
  ngAfterViewInit(): void {
    this._initMap();
  }

  private _initMap() {
    this._map = new atlas.Map(this.mapId, {
      language: 'en-us',
      style: 'satellite',
      showFeedbackLink: false
    });

    this._map.events.add('ready', (event) => {
      this._map$.next(event.map);
      this.map.emit(this._map$.asObservable());
    });

    this._map.events.add('error', (e) => {
      console.error(e);
      this._sharedService.showMessage(
        'Error occurred while loading the map',
        SnackBarClasses.error,
      );
    });

    this._map$.next(this._map);
  }
  ```

- **Module Configuration**:

  ```typescript
  AzureMapsModule.forRoot({
    authOptions: {
      authType: AuthenticationType.subscriptionKey,
      subscriptionKey: environment.azureMapsSubscriptionKey,
    },
  }),
  ```

## Observations

- **Intermittency**: The error appears inconsistently; sometimes map loads fine, other times it does not.
- **Isolation**: Removing all code that adds markers does not prevent the error; it occurs during plain map initialization.
- **Error Source**: The error comes before any user images or overlays are introduced.
- **Error Event Hook**: The error is captured via the `.on('error', ...)` map event.

## Possible Causes

- **Image Type Issue**: The SDK expects images to be in PNG or JPEG format. SVGs are explicitly not supported, but in this code, no custom images are loaded before the error.
- **Map Style/Tile Loading**: The intermittent nature suggests a possible issue with the internal loading of map tile images or sprites, potentially related to the selected map style ('satellite') or SDK internal requests.
- **Timing**: There might be a race condition or timing issue in how the map is loaded within the Angular lifecycle.
- **SDK Bug**: If upgrading/downgrading the SDK solves the issue, it may be a bug in v2.3.7.

## Troubleshooting Steps & Recommendations

- **SDK Version**: Try testing with a newer or older version of `azure-maps-control` to determine if the error persists.
- **Custom Images/Sprites**: Ensure you are not inadvertently referencing unsupported image resources elsewhere in the codebase.
- **Network Issues**: Verify network connectivity and check for failed requests in the browser's developer console; the map's underlying tiles or sprite sheets may be failing to load.
- **Initialization Timing**: Delay map initialization slightly after `ngAfterViewInit` to ensure DOM elements are fully available.
- **Official Support**: Consider contacting [Microsoft Azure Maps support](https://docs.microsoft.com/en-us/azure/azure-maps/) if the issue remains unresolved after troubleshooting.

## References

- [Azure Maps Documentation](https://docs.microsoft.com/en-us/azure/azure-maps/)
- [Supported Image Formats](https://docs.microsoft.com/en-us/azure/azure-maps/map-add-image)

---
**Summary:**
This post provides an insight into a real-world issue with Azure Maps Control when used within Angular. Even without adding custom markers or overlays, you may encounter intermittent image loading errors, possibly stemming from internal SDK handling, network, or initialization order. Review the SDK version and initialization sequence to mitigate such issues.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-maps/could-not-load-image-because-of-out-of-range-source-coordinates/m-p/4441495#M153)
