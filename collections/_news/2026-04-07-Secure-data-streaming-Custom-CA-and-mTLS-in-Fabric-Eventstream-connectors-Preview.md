---
feed_name: Microsoft Fabric Blog
date: 2026-04-07 09:00:00 +00:00
author: Microsoft Fabric Blog
tags:
- Amazon MSK
- Apache Kafka
- Azure
- Azure Key Vault
- Certificate Rotation
- Confluent Cloud
- Confluent Schema Registry
- Custom Certificate Authority
- Eventstream
- Microsoft Fabric
- ML
- Mtls
- News
- PEM Certificates
- Private Endpoint
- Private Network
- Real Time Intelligence
- Security
- Streaming Connectors
- TLS
- Virtual Network
- Vnet Injection
- X.509 Certificates
section_names:
- azure
- ml
- security
external_url: https://blog.fabric.microsoft.com/en-US/blog/secure-data-streaming-custom-ca-and-mtls-in-fabric-eventstream-connectors-preview/
primary_section: ml
title: 'Secure data streaming: Custom CA and mTLS in Fabric Eventstream connectors (Preview)'
---

Microsoft Fabric Blog announces a preview feature for Fabric Eventstream connectors that adds support for custom Certificate Authorities and mutual TLS, using Azure Key Vault to store and rotate certificates for Kafka-based streaming sources.<!--excerpt_end-->

# Secure data streaming: Custom CA and mTLS in Fabric Eventstream connectors (Preview)

Security is non-negotiable for real-time data streaming—especially in regulated industries (banking, healthcare, telecommunications) where every data connection must be encrypted and mutually authenticated.

Microsoft Fabric Eventstream (part of Real-Time Intelligence / RTI) provides nearly 20 streaming connectors for ingesting real-time data from sources like Apache Kafka, Amazon Managed Streaming for Apache Kafka (Amazon MSK), and Confluent Cloud for Apache Kafka.

Historically, these connectors only supported TLS encryption via system-predefined CA certificates from a trusted CA list. That meant you could not connect if:

- Your source systems used certificates signed by a custom/internal CA, or
- Your environment required mutual TLS (mTLS) for two-way authentication.

## What’s being announced (Preview)

Fabric Eventstream now supports **Custom CA and mTLS** for **Kafka-based sources** (preview), including:

- Apache Kafka
- Amazon Managed Streaming for Apache Kafka (Amazon MSK)
- Confluent Cloud for Apache Kafka
- Confluent Schema Registry

This enables Eventstream connectors to establish encrypted connections that can also be mutually authenticated when required.

## Bring your own certificates with Azure Key Vault

Eventstream integrates with **Azure Key Vault** so you can reference certificates stored centrally rather than managing files per connector.

### How it works

1. Import your certificates into **Azure Key Vault** in **.pem** format:
   - For TLS with a custom CA: import the **CA certificate**.
   - For mTLS: import the **CA certificate** plus **client certificate and private key**.
2. In the Eventstream wizard, configure a Kafka-based source and **reference the certificates from Key Vault**.
3. The Eventstream connector service **fetches the certificates at runtime** and uses them to establish an encrypted and (for mTLS) mutually authenticated connection.

### Why Key Vault (vs file-based cert handling)

- Centralized and secure certificate storage
- Multiple engineers can reuse the same certificates without file handoffs
- Certificate rotation/update happens once in Key Vault
- Connectors referencing the certificate automatically pick up the new version

![The architecture diagram for the Eventstream connector custom CA and mTLS support feature. It leverages Azure Key Vault to bring customers' certificates for the connector data encryption.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/the-architecture-diagram-for-the-eventstream-conne.png)

Figure 1 Architecture custom CA and mTLS in Eventstream.

## Essentials and getting started

Before configuring custom CA or mTLS in Eventstream, ensure these prerequisites are in place:

- **Source system readiness**
  - Confirm the source server accepts connections using your custom CA certificates.
  - If using mTLS, confirm it is configured to validate client certificates.
- **Azure Key Vault with certificates**
  - Import required certificates into Key Vault in **.pem** format.
  - Ensure you have an appropriate role assignment (examples given):
    - `Key Vault Administrator`
    - `Key Vault Certificate User`
- **Private network setup (if applicable)**
  - If the source is in a private network, use Eventstream’s **vNet injection**.
  - Provide an **Azure virtual network** connected to the source’s private network.
  - Create a **private endpoint** from Azure Key Vault to the virtual network so the vNet-injected connector can securely access certificates.

After prerequisites are met, configure the connector in the Eventstream wizard to use the Key Vault certificates.

![Animation to show the end to end flow to configure the custom CA or mTLS support for Eventstream sources.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/animation-to-show-the-end-to-end-flow-to-configure.gif)

Figure: Custom CA and mTLS configuration end to end flow.

For a detailed guide, see the documentation:

- [Add and Manage Eventstream Sources](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/add-manage-eventstream-sources)

It references source-specific configuration docs for Apache Kafka, Confluent Cloud for Apache Kafka, and Amazon Managed Streaming for Apache Kafka.

## Next steps and resources

- Learn more about Eventstream:
  - [Eventstream overview](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/overview?tabs=enhancedcapabilities)
- Try the feature via Fabric (trial links provided in the original post):
  - [Sign up for Power BI with a new Microsoft 365 trial](https://learn.microsoft.com/power-bi/enterprise/service-admin-signing-up-for-power-bi-with-a-new-office-365-trial)
  - [Fabric trial capacity](https://learn.microsoft.com/fabric/fundamentals/fabric-trial)

Feedback channels:

- [Community forum](https://aka.ms/realtimecommunity)
- [Idea submission](https://aka.ms/realtimeideas)
- Email: [askeventstreams@microsoft.com](mailto:askeventstreams@microsoft.com)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/secure-data-streaming-custom-ca-and-mtls-in-fabric-eventstream-connectors-preview/)

