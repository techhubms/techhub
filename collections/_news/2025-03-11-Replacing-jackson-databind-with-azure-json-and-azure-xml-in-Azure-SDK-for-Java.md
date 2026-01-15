---
layout: post
title: Replacing jackson-databind with azure-json and azure-xml in Azure SDK for Java
author: Alan Zimmer
canonical_url: https://devblogs.microsoft.com/azure-sdk/replacing-jackson-databind-with-azure-json-and-azure-xml/
viewing_mode: external
feed_name: Microsoft DevBlog
feed_url: https://devblogs.microsoft.com/azure-sdk/feed/
date: 2025-03-11 20:38:45 +00:00
permalink: /coding/news/Replacing-jackson-databind-with-azure-json-and-azure-xml-in-Azure-SDK-for-Java
tags:
- Azure
- Azure Core
- Azure JSON
- Azure SDK
- Azure XML
- Coding
- Core
- Dependency Management
- Gson
- Jackson Databind
- Java
- JsonSerializable
- Module System
- News
- Reflection
- Serialization
- XmlSerializable
section_names:
- azure
- coding
---
In this post, Alan Zimmer explains how the Azure SDK for Java transitioned from jackson-databind to in-house azure-json and azure-xml libraries, exploring the technical motivations, migration details, and integration points with common serialization frameworks.<!--excerpt_end-->

## Overview

[`jackson-databind`](https://github.com/FasterXML/jackson-databind) (Jackson) has been a foundation for JSON serialization in the Java ecosystem. However, maintaining reliability in the Azure SDK for Java became challenging due to dependency version conflicts and the Java module system's stricter requirements. To address these, the Azure SDK for Java team developed and adopted `azure-json` and `azure-xml` libraries.

---

## Why Replace jackson-databind?

- **Dependency Conflicts**: Different Java libraries pull in unique versions of Jackson, which can lead to runtime issues when dependencies clash.
- **Reflection Issues**: Jackson’s heavy reliance on reflection makes it less compatible with the Java module system and can impact security policies.
- **SDK Compatibility**: Reducing dependency issues improves the Azure SDK's flexibility and reliability for diverse Java environments.

---

## Introducing azure-json and azure-xml

To minimize reliance on Jackson, Azure SDK for Java now provides:

- [`azure-json`](https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/serialization/azure-json): Focused on JSON serialization.
- [`azure-xml`](https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/serialization/azure-xml): Focused on XML serialization.

### Key Features

- Both libraries support streaming readers and writers.
- Offer interfaces (`JsonSerializable` and `XmlSerializable`) for custom serialization behavior.
- No runtime dependencies, mitigating further dependency conflicts.

#### azure-json

- Includes `JsonReader`, `JsonWriter`, and `JsonSerializable`.
- Inspired by the [JSON specification](https://www.json.org/), `jackson-core`, and Gson.
- Uses a `JsonProvider` SPI, with the default implementation leveraging shaded `jackson-core`.
- Offers a model-based JSON structure in `com.azure.json.models`.

#### azure-xml

- Provides `XmlReader`, `XmlWriter`, and `XmlSerializable`.
- Modeled after Java’s `javax.xml.stream` streaming APIs.
- Uses and wraps Java's `XMLStreamReader` and `XMLStreamWriter`.
- For performance, resorts to shaded `aalto-xml` if possible.

*Result: Reduced dependency on Jackson and reflection, and improved module-system compatibility.*

---

## Changes in Azure SDK for Java Model Serialization

- **Before:** Models used `jackson-annotations`.
- **Now:** Models implement `JsonSerializable` and `XmlSerializable`.

*Benefits:*

- Reduces the need for reflection.
- Eases the support for dual JSON and XML serialization.

---

## Integrating with Jackson and Gson

To facilitate interoperability, the Azure SDK team provides helpers for integrating with Jackson and Gson:

- **Jackson Integration:**
  - Use `JacksonJsonProvider.getJsonSerializableDatabindModule()` to register with Jackson's `ObjectMapper`:
  
    ```java
    ObjectMapper objectMapper = JsonMapper.builder()
        .addModule(JacksonJsonProvider.getJsonSerializableDatabindModule())
        .build();
    ```

- **Gson Integration:**
  - Use `GsonJsonProvider.getJsonSerializableTypeAdapterFactory()` to register with Gson's `GsonBuilder`:

    ```java
    Gson gson = new GsonBuilder()
        .registerTypeAdapterFactory(GsonJsonProvider.getJsonSerializableTypeAdapterFactory())
        .create();
    ```

These providers ensure `JsonSerializable` types in the Azure SDK work seamlessly with these popular serialization frameworks.

---

## Status of Jackson Dependencies

- **Current State:**
  - Azure SDK for Java client libraries still depend on `jackson-annotations`, `jackson-core`, and `jackson-databind` within `azure-core` where streaming APIs or other features can't yet be replaced.
  - `jackson-dataformat-xml` has been removed.
  - Most libraries outside of `azure-core*` removed direct Jackson dependencies, relying on the Azure libraries instead.

- **Future Direction:**
  - The team is researching the possibility of completely removing Jackson from `azure-core`.

---

## Feedback

For questions or concerns, developers are encouraged to [contact the Azure SDK for Java team on GitHub](https://github.com/Azure/azure-sdk-for-java/issues/new?template=bug_report.md).

---

*Post by Alan Zimmer for the [Azure SDK Blog](https://devblogs.microsoft.com/azure-sdk).*

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/replacing-jackson-databind-with-azure-json-and-azure-xml/)
