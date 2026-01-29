---
external_url: https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-the-hl7-connector-for-azure-logic-apps-standard-and/ba-p/4470690
title: 'Announcing Public Preview: HL7 Connector for Azure Logic Apps (Standard & Hybrid)'
author: hcamposu
feed_name: Microsoft Tech Community
date: 2025-11-24 12:13:17 +00:00
tags:
- Automated Data Exchange
- Azure Integration Services
- Azure Logic Apps
- BizTalk Migration
- Clinical Data Exchange
- Healthcare Integration
- HL7
- HL7 Connector
- Hybrid Deployment
- Integration Account
- Message Orchestration
- MLLP Adapter
- Schema Management
- Standard Workflow
- Azure
- Community
section_names:
- azure
primary_section: azure
---
hcamposu announces the public preview of the HL7 connector for Azure Logic Apps, highlighting seamless clinical application integration and robust data exchange capabilities for healthcare organizations.<!--excerpt_end-->

# Announcing the HL7 Connector for Azure Logic Apps (Standard and Hybrid) – Public Preview

hcamposu presents the new HL7 connector for Azure Logic Apps (Standard and Hybrid), designed to help healthcare organizations with reliable, automated integration across clinical applications, using HL7 protocols.

## Why HL7 Integration is Important

Healthcare environments generate complex critical patient data across admissions, laboratory, billing, and clinical departments. Standardized, secure data exchange is key to improving operational efficiency and patient care.

## Connector Features

- **HL7-Specific Adapters & Schemas**: Simplifies integration between medical applications using industry HL7 standards.
- **Automated Communication**: Reduces manual intervention in healthcare workflows.
- **End-to-End Process Support**: Enables scenarios like admissions, discharge, and transfer through publisher/subscriber messaging patterns.

## Supported Scenarios

1. **End-to-End Integration**: For example, Admissions Discharge and Transfer systems send messages to Hospital Information Systems or Pharmacy. Logic Apps handle validation, transformation, routing, and acknowledgments.
2. **Query/Response**: Applications can request specific information (such as lab results), with Logic Apps managing message routing and responses.

## Encode/Decode Actions

- **Decode**: Converts flat file HL7 messages to XML, including header extraction.
  - Input: Flat file HL7
  - Output: XML message/content and header
- **Encode**: Converts XML message and header into HL7 flat file format.
  - Input: XML message/header
  - Output: Encoded HL7 file

## Technical Highlights

- **Schema Support**: HL7 v2.X flat-file and XML schemas supported.
- **Schema Management**: Direct BizTalk schema uploads into Integration Accounts for easy migration.
- **Hybrid Deployment**: Integration accounts connect to Azure for flexible hybrid workflows.
- **Message Processing**: Single message processing currently, batching capability planned.
- **Multipart Handling**: Logic Apps exposes message segments for simplified orchestration compared to BizTalk.
- **Connector Availability**: Available for hybrid and Logic App Standard.

### MLLP Private Preview

Minimal Lower Layer Protocol (MLLP) adapter is now in private preview, available only in hybrid deployments due to port restrictions. Interested users can [request access](https://aka.ms/privatemllp).

## Get Started

Explore full documentation at [Integrate Healthcare Systems with HL7 in Standard Workflows | Microsoft Learn](https://learn.microsoft.com/en-us/azure/logic-apps/connectors/integrate-healthcare-systems).

## Additional Resources

- Video walkthrough available explaining HL7 connector features.
- Example scenarios include patient admissions integrations, lab result queries, and automated clinical data flows.

---
*Author: hcamposu | Updated Nov 24, 2025 | Version 1.0*

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-integration-services-blog/announcing-the-hl7-connector-for-azure-logic-apps-standard-and/ba-p/4470690)
