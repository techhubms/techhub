---
layout: post
title: General Availability of XML Parse and Compose Actions for Azure Logic Apps
author: hcamposu
canonical_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-the-general-availability-of-the-xml-parse-and-compose/ba-p/4470825
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure
date: 2025-11-25 03:45:25 +00:00
permalink: /coding/community/General-Availability-of-XML-Parse-and-Compose-Actions-for-Azure-Logic-Apps
tags:
- Array Input
- Azure Logic Apps
- BizTalk Migration
- Cloud Workflow
- Compose XML
- Content Type
- Data Integration
- Encoding
- HTTP Request
- Integration Account
- Parse XML
- Rules Engine
- Token Picker
- Workflow Automation
- XML Operations Connector
- XML Schema
- XSD Schema
- XSLT
section_names:
- azure
- coding
---
hcamposu announces the general availability of XML Parse and Compose actions in Azure Logic Apps, offering enhanced XML workflow capabilities for cloud developers.<!--excerpt_end-->

# General Availability: XML Parse and Compose Actions in Azure Logic Apps

Azure Logic Apps now natively supports both XML and JSON data formats, with the introduction of the XML Operations connector. Two key actions—**Parse XML with schema** and **Compose XML with schema**—are now generally available, streamlining how developers use schema-driven XML workflows in the cloud.

## XML Operations Connector Overview

- **Parse XML with schema**: Use XSD files to validate and parse XML data in your workflow. Simply upload your schema into Logic App artifacts or an Integration account, then reference it to extract structured data from XML content. The Logic App Designer's token picker reflects properties defined in your schema for easier mapping and manipulation.
- **Compose XML with schema**: Generate valid XML output from JSON inputs guided by an XSD schema. Schema-based definitions dynamically produce the required JSON structure, allowing Logic Apps to create new XML documents tailored to your specifications.

## Key Recommendations & Considerations (GA Release)

Based on customer feedback, here are best practices for reliable, flexible, and international-ready XML workflows:

### Handling Array Inputs in XML

- Use **array input mode** in the Logic App Designer when mapping elements with `maxOccurs > 1`. Assigning entire arrays directly streamlines process logic and avoids excess looping.

### Managing Non-UTF-8 Encoded XML

- Set the encoding parameter in XML Compose to output in character sets like `iso-2022-jp` (Japanese) for broader internationalization.
- Configure the '.NET XML writer settings' property to control encoding.

### Transport of Binary & Diverse Encoding Content

- XML Compose output is wrapped in a content envelope to safely carry non-UTF-8 and even binary content over JSON-based payloads. Downstream actions (e.g., HTTP Request) can consume the envelope as raw payload, maintaining fidelity and correct encoding.

### Content-Type Header Management

- Logic Apps now automatically sets the exact `Content-Type` header (e.g., `application/xml; charset=iso-2022-jp`) to inform downstream systems of the correct character set for your XML payloads.

### Using XML Output in HTTP Actions

- Reference the XML output property in downstream HTTP actions; headers and body will be correctly configured for fidelity and compatibility.

### Documentation & Learning Resources

- For advanced scenarios and troubleshooting, consult:
  - [Support non-Unicode character encoding in Azure Logic Apps](https://learn.microsoft.com/azure/logic-apps/support-non-unicode-character-encoding)
  - [Content-Type](https://developer.mozilla.org/docs/Web/HTTP/Headers/Content-Type)
  - [Content-Encoding](https://developer.mozilla.org/docs/Web/HTTP/Headers/Content-Encoding)
- Avoid confusion: `Content-Type` specifies encoding, while `Content-Encoding` is related to compression (e.g., gzip).

## Practical Use Cases

- Migrate BizTalk XML workflows to Azure Logic Apps
- Build enterprise-scale integrations needing schema-driven XML validation
- Enable flexible data exchange with diverse encoding requirements

## Author & Community

Announced by **hcamposu** on the Azure Integration Services Blog. For ongoing updates and discussion, follow the blog board and review contributor profiles for best practice sharing.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-the-general-availability-of-the-xml-parse-and-compose/ba-p/4470825)
