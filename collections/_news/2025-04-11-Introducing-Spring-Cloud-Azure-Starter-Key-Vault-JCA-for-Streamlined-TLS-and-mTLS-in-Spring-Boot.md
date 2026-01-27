---
external_url: https://devblogs.microsoft.com/azure-sdk/introducing-spring-cloud-azure-starter-key-vault-jca-streamlined-tls-and-mtls-for-spring-boot/
title: Introducing Spring Cloud Azure Starter Key Vault JCA for Streamlined TLS and mTLS in Spring Boot
author: Moary Chen
feed_name: Microsoft DevBlog
date: 2025-04-11 21:11:34 +00:00
tags:
- Azure Key Vault
- Azure SDK
- Certificate Management
- Java
- JCA
- Mtls
- RestTemplate
- Service Principal
- Spring
- Spring Boot
- Spring Cloud Azure
- SSL Bundles
- TLS
- WebClient
section_names:
- azure
- security
- coding
primary_section: coding
---
Moary Chen introduces the Spring Cloud Azure Starter Key Vault JCA, showing how to streamline TLS and mTLS for Spring Boot using Azure Key Vault for secure certificate management.<!--excerpt_end-->

# Introducing Spring Cloud Azure Starter Key Vault JCA for Streamlined TLS and mTLS in Spring Boot

*Author: Moary Chen*

---

## Overview

Microsoft has released the **Spring Cloud Azure Starter Key Vault Java Crypto Architecture (JCA)**, extending the Spring Cloud Azure ecosystem with new secure communication features for Java developers. This starter package simplifies configuring TLS and mutual TLS (mTLS) in Spring Boot applications by integrating Azure Key Vault’s certificate management through the Java Crypto Architecture and Spring SSL Bundles. The solution is designed for Spring Boot 3.1+ and is available in version 5.21.0 and above.

## What is Spring Cloud Azure Starter Key Vault JCA?

Spring Cloud Azure Starter Key Vault JCA bridges Spring Boot’s [SSL Bundles](https://docs.spring.io/spring-boot/reference/features/ssl.html) abstraction with the Azure Key Vault JCA provider. This integration allows developers to use certificates securely stored in Azure Key Vault directly in their Spring applications. This new approach reduces the complexity associated with traditional Java keystore management, offering a modern, secure, and maintainable alternative.

- **SSL Bundles** (Spring Boot 3.1+): Abstraction layer in Spring Boot for managing SSL credentials.
- **Azure Key Vault JCA Provider**: Enables reading certificates and keys securely from Azure Key Vault.

**Reference:** [Legacy approach and comparison](https://learn.microsoft.com/azure/developer/java/spring-framework/configure-spring-boot-starter-java-app-with-azure-key-vault-certificates)

## Getting Started

To begin using the starter:

### 1. Add the Maven Dependency

```xml
<dependency>
  <groupId>com.azure.spring</groupId>
  <artifactId>spring-cloud-azure-starter-keyvault-jca</artifactId>
  <version>5.21.0</version>
</dependency>
```

### 2. Prepare Azure Resources

- **Create two self-signed certificates** in separate Azure Key Vault resources: one named `server` (in `keyvault1`), and another named `client` (in `keyvault2`). [Instructions](https://learn.microsoft.com/azure/key-vault/certificates/quick-create-portal#add-a-certificate-to-key-vault)
- **Create a Service Principal** with appropriate permissions using [this guide](https://learn.microsoft.com/entra/identity-platform/howto-create-service-principal-portal).
- **Assign Key Vault Certificate User Role** to the Service Principal for both vaults ([role assignment guide](https://learn.microsoft.com/azure/key-vault/general/rbac-guide)).
- **Set environment variables**: Variables prefixed with `KEY_VAULT_SSL_BUNDLES_` store connection and credential information for Key Vault access.

## Use Cases and Example Configurations

### Enabling Embedded Server TLS

Secure inbound HTTP traffic with server-side TLS using Key Vault certificate:

**application.yml Example:**

```yaml
spring:
  application:
    name: ssl-bundles-server
  ssl:
    bundle:
      keyvault:
        tlsServerBundle:
          key:
            alias: server
          keystore:
            keyvault-ref: keyvault1
  cloud:
    azure:
      keyvault:
        jca:
          vaults:
            keyvault1:
              endpoint: ${KEY_VAULT_SSL_BUNDLES_KEYVAULT_URI_01}
              profile:
                tenant-id: ${KEY_VAULT_SSL_BUNDLES_TENANT_ID}
              credential:
                client-id: ${KEY_VAULT_SSL_BUNDLES_CLIENT_ID}
                client-secret: ${KEY_VAULT_SSL_BUNDLES_CLIENT_SECRET}
server:
  ssl:
    bundle: tlsServerBundle
```

### Securing RestTemplate for Outbound TLS

Configure Spring’s `RestTemplate` to use a Key Vault certificate for secure outbound HTTPS:

**application.yml adjustment:**

```yaml
spring:
  ssl:
    bundle:
      keyvault:
        tlsClientBundle:
          truststore:
            keyvault-ref: keyvault1
  cloud:
    azure:
      keyvault:
        jca:
          vaults:
            keyvault1:
              endpoint: ${KEY_VAULT_SSL_BUNDLES_KEYVAULT_URI_01}
              profile:
                tenant-id: ${KEY_VAULT_SSL_BUNDLES_TENANT_ID}
              credential:
                client-id: ${KEY_VAULT_SSL_BUNDLES_CLIENT_ID}
                client-secret: ${KEY_VAULT_SSL_BUNDLES_CLIENT_SECRET}
```

**Java configuration:**

```java
@Bean
RestTemplate restTemplateWithTLS(RestTemplateBuilder restTemplateBuilder, SslBundles sslBundles) {
    return restTemplateBuilder.sslBundle(sslBundles.getBundle("tlsClientBundle")).build();
}
```

### Securing WebClient for Outbound TLS

Follow the same `application.yml` as in the RestTemplate scenario, then adjust the WebClient registration:

```java
@Bean
WebClient webClientWithTLS(WebClientSsl ssl) {
    return WebClient.builder().apply(ssl.fromBundle("tlsClientBundle")).build();
}
```

### mTLS (Mutual TLS) Communication Setup

#### Server Side Configuration

Trust client certificates in `keyvault2` and enable client authentication:

```yaml
spring:
  application:
    name: ssl-bundles-server
  ssl:
    bundle:
      keyvault:
        tlsServerBundle:
          key:
            alias: server
          keystore:
            keyvault-ref: keyvault1
          truststore:
            keyvault-ref: keyvault2
  cloud:
    azure:
      keyvault:
        jca:
          vaults:
            keyvault1: ...  # as above
            keyvault2: ...
server:
  ssl:
    bundle: tlsServerBundle
    client-auth: NEED
```

#### Client Side Configuration

Configure `application.yml` to use `keyvault2` for the client’s certificate and `keyvault1` for the trust store:

```yaml
spring:
  ssl:
    bundle:
      keyvault:
        mtlsClientBundle:
          key:
            alias: client
            for-client-auth: true
          keystore:
            keyvault-ref: keyvault2
          truststore:
            keyvault-ref: keyvault1
  cloud:
    azure:
      keyvault:
        jca:
          vaults:
            keyvault1: ...
            keyvault2: ...
```

**Registering TLS/mTLS Java Beans:**

```java
@Bean
RestTemplate restTemplateWithMTLS(RestTemplateBuilder restTemplateBuilder, SslBundles sslBundles) {
    return restTemplateBuilder.sslBundle(sslBundles.getBundle("mtlsClientBundle")).build();
}

@Bean
WebClient webClientWithMTLS(WebClientSsl ssl) {
    return WebClient.builder().apply(ssl.fromBundle("mtlsClientBundle")).build();
}
```

## Feedback and Contributions

You can share feedback or contribute via [StackOverflow](https://stackoverflow.com/questions/tagged/spring-cloud-azure) or the [Azure SDK for Java GitHub repository](https://github.com/Azure/azure-sdk-for-java/issues?q=is%3Aissue+is%3Aopen+label%3Aazure-spring).

## Further Resources

- [Enable HTTPS in Spring Boot application (Legacy)](https://learn.microsoft.com/azure/developer/java/spring-framework/configure-spring-boot-starter-java-app-with-azure-key-vault-certificates)
- [Sign and verify jar files with Azure Key Vault](https://techcommunity.microsoft.com/blog/appsonazureblog/seamlessly-integrating-azure-keyvault-with-jarsigner-for-enhanced-security/4125770)
- [Using Azure Key Vault for JVM SSL certificates](https://learn.microsoft.com/azure/developer/java/fundamentals/java-azure-keyvault-ssl-integration-jvm)
- [Using Azure Key Vault with Apache Tomcat](https://learn.microsoft.com/azure/developer/java/fundamentals/java-azure-keyvault-tomcat-integration?tabs=windows)
- [Spring Cloud Azure Documentation](https://aka.ms/spring/docs)
- [Spring Cloud Azure Code Samples](https://aka.ms/spring/samples)
- [Changelog](https://github.com/Azure/azure-sdk-for-java/blob/main/sdk/spring/CHANGELOG.md)

---

This starter streamlines the integration of secure, certificate-based communication between Spring Boot applications and Azure Key Vault, making it easier for Java developers to adopt robust security practices on Azure.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/introducing-spring-cloud-azure-starter-key-vault-jca-streamlined-tls-and-mtls-for-spring-boot/)
