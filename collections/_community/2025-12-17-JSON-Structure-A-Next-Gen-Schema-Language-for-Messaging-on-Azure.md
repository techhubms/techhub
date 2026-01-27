---
external_url: https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/json-structure-a-json-schema-language-you-ll-love/ba-p/4476852
title: 'JSON Structure: A Next-Gen Schema Language for Messaging on Azure'
author: clemensv
feed_name: Microsoft Tech Community
date: 2025-12-17 08:03:09 +00:00
tags:
- .NET
- Avro
- Azure Event Grid
- Azure Event Hubs
- Azure Service Bus
- Code Generation
- Data Definition
- Data Modeling
- JSON Schema
- JSON Structure
- Messaging
- Microsoft Fabric
- Protobuf
- Python
- Schema Language
- SQL Server
- Structurize
- TypeScript
- VS Code Extension
section_names:
- azure
- coding
primary_section: coding
---
clemensv presents JSON Structure, a modern schema language designed for robust data definitions in distributed messaging systems on Azure. This article reviews its features, type system, and integrations, spotlighting practical advantages for developers.<!--excerpt_end-->

# JSON Structure: A Next-Gen Schema Language for Messaging on Azure

**Author: clemensv**  
*Published: Dec 12, 2025*

## Introduction

Developers moving structured data through Azure queues, event streams, and topics demand reliable, efficient, and maintainable data contracts. Traditional approaches relying on JSON Schema or message schemas often fall short; they're either too unstructured for real code generation or too tightly bound to specific frameworks like Avro or Protobuf.

## The Problem with Current Schema Tools

- **JSON Schema** is approachable for basic validation, but quickly gets unmanageable as constructs become complex. Most industry usage sits on "Draft 7" with little adoption of newer specs. Attempting code generation or data mapping with JSON Schema is often frustrating and brittle, as it was designed for validation rather than data definition.
- **Avro and Protobuf** improve on code generation but are deeply tied to their serialization formats, limiting flexibility and integration.
- In large platforms like **Microsoft Fabric** and general Azure messaging (Event Hubs, Service Bus, Event Grid), teams need more robust, language-neutral schemas for interoperability—not just validation.

## Introducing JSON Structure

JSON Structure is a vendor-neutral, standards-track schema language submitted to the IETF, aiming to become an RFC. It's a modern, strictly typed data definition system for JSON-encoded data. The schema syntax remains close to what developers know, but with important enhancements:

- **Precise primitive types**: Support for `int32`, `int64`, `decimal` with precision/scale, `float`, `double`, and more.
- **Rich date and time types**: Built-in types for `date`, `time`, `datetime`, `duration`.
- **Compound types**: In addition to objects and arrays, use `set`, `map`, `tuple`, and `choice` for discriminated unions/enums-with-data.
- **Modularity**: Namespaces and imports for manageable, scalable schemas.
- **Currency and unit annotations**: Explicit metadata for business and scientific data.

### Example: Order Event Schema

```json
{
  "$schema": "https://json-structure.org/meta/extended/v0/#",
  "$id": "https://example.com/schemas/OrderEvent.json",
  "name": "OrderEvent",
  "type": "object",
  "properties": {
    "orderId": { "type": "uuid" },
    "customerId": { "type": "uuid" },
    "timestamp": { "type": "datetime" },
    "status": {
      "type": "choice",
      "choices": {
        "pending": { "type": "null" },
        "shipped": {
          "type": "object",
          "name": "ShippedInfo",
          "properties": {
            "carrier": { "type": "string" },
            "trackingId": { "type": "string" }
          }
        },
        "delivered": {
          "type": "object",
          "name": "DeliveredInfo",
          "properties": {
            "signedBy": { "type": "string" }
          }
        }
      }
    },
    "total": { "type": "decimal", "precision": 12, "scale": 2 },
    "currency": { "type": "string", "maxLength": 3 },
    "items": {
      "type": "array",
      "items": {
        "type": "tuple",
        "properties": {
          "sku": { "type": "string" },
          "quantity": { "type": "int32" },
          "unitPrice": { "type": "decimal", "precision": 10, "scale": 2 }
        },
        "tuple": ["sku", "quantity", "unitPrice"],
        "required": ["sku", "quantity", "unitPrice"]
      }
    },
    "tags": { "type": "set", "items": { "type": "string" } },
    "metadata": { "type": "map", "values": { "type": "string" } }
  },
  "required": ["orderId", "customerId", "timestamp", "status", "total", "currency", "items"]
}
```

### Features

- **Choice/Union Types**: Flexible status values (pending, shipped, delivered) with associated data, mapping cleanly to enums or sealed types in .NET, Python, Java, etc.
- **Precise Mapping**: Native types like `uuid` and `datetime` match .NET's `Guid` and `DateTimeOffset`, Python's `uuid` and `datetime`, JavaScript's `Date`, and others.
- **Decimal Handling**: Monetary fields as precise decimals, avoiding floating-point errors.
- **Line Items and Tuples**: Strong typing for arrays where order and type matter, as in order items.
- **Metadata**: Extensible maps and sets for labels, tags, and arbitrary key-value properties.

## Why It Matters for Azure Messaging

- Messaging systems like **Service Bus**, **Event Hubs**, and **Event Grid** depend on strict, interoperable contract definitions across teams and tech stacks.
- JSON Structure schemas can drive code generation for C#, Python, TypeScript, and more, producing first-class types and validation from a single source file.
- Reduces friction between producer and consumer teams using different tech stacks.
- Supports critical workflows for Microsoft Fabric and other cloud data flows.

## SDKs and Tooling

- Official SDKs for [TypeScript](https://json-structure.org/sdks/typescript), [Python](https://json-structure.org/sdks/python), [.NET](https://json-structure.org/sdks/dotnet), [Java](https://json-structure.org/sdks/java), [Go](https://json-structure.org/sdks/go), [Rust](https://json-structure.org/sdks/rust), and others.
- [VS Code extension](https://marketplace.visualstudio.com/items?itemName=json-structure.json-structure-sdk) for schema authoring, validation, and IntelliSense.
- **Structurize**: A [tool](https://clemensv.github.io/avrotize/) for generating SQL DDL and self-serializing classes from JSON Structure schemas, supporting multiple database dialects.

### Example: Generating a SQL Server Table

Given a postal address schema, Structurize outputs DDL for SQL Server, mapping types like `uuid` to `UNIQUEIDENTIFIER`, `datetime` to `DATETIME2`, and so on. Extended descriptions are preserved as SQL Server extended properties.

## Status and Community

- JSON Structure is under IETF review and open for community input
- The spec, SDKs, and tools are in draft state; contributors are welcome via [GitHub](https://github.com/json-structure/)

## Further Reading

- [json-structure.org](https://json-structure.org)
- [Primer](https://json-structure.org/json-structure-primer.html)
- [IETF Drafts](https://datatracker.ietf.org/doc/search?name=draft-vasters-json-structure&rfcs=on&activedrafts=on&olddrafts=on)
- [Structurize Tool & Docs](https://clemensv.github.io/avrotize/)

---
*For questions and feedback, visit the official [GitHub repository](https://github.com/json-structure/).*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/messaging-on-azure-blog/json-structure-a-json-schema-language-you-ll-love/ba-p/4476852)
