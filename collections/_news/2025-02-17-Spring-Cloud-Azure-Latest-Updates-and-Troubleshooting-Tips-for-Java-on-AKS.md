---
layout: "post"
title: "Spring Cloud Azure: Latest Updates and Troubleshooting Tips for Java on AKS"
description: "This article by Moary Chen highlights new features and fixes in Spring Cloud Azure versions 5.16.0 to 5.19.0, provides troubleshooting guidance for Java on Azure Kubernetes Service (AKS), details enhancements for authentication, diagnostics, passwordless connections, and discusses known issues and solutions."
author: "Moary Chen"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/azure-sdk/spring-cloud-azure-updates-and-troubleshooting-tips-for-java-on-aks/"
viewing_mode: "external"
feed_name: "Microsoft DevBlog"
feed_url: "https://devblogs.microsoft.com/azure-sdk/feed/"
date: 2025-02-17 17:05:08 +00:00
permalink: "/news/2025-02-17-Spring-Cloud-Azure-Latest-Updates-and-Troubleshooting-Tips-for-Java-on-AKS.html"
categories: ["Azure", "Coding", "DevOps"]
tags: ["AKS", "Authentication", "Azure", "Azure Identity Extensions", "Azure SDK", "Coding", "Database", "DevOps", "Diagnostics", "Java", "JCA", "Key Vault", "Monitoring", "News", "Passwordless Connections", "Service Bus", "Spring", "Spring Boot", "Spring Cloud Azure", "TokenCredential"]
tags_normalized: ["aks", "authentication", "azure", "azure identity extensions", "azure sdk", "coding", "database", "devops", "diagnostics", "java", "jca", "key vault", "monitoring", "news", "passwordless connections", "service bus", "spring", "spring boot", "spring cloud azure", "tokencredential"]
---

Moary Chen explores the latest advancements in Spring Cloud Azure with a special focus on Java applications deployed in Azure Kubernetes Service (AKS), offering comprehensive updates and troubleshooting tips.<!--excerpt_end-->

# Spring Cloud Azure: Latest Updates and Troubleshooting Tips for Java on AKS

*By Moary Chen*

This post explores the newest features, improvements, and bug fixes introduced in Spring Cloud Azure for Java developers, specifically covering versions 5.16.0 to 5.19.0. It also provides troubleshooting guidance for Java processes running on Azure Kubernetes Service (AKS) and highlights practical features such as advanced authentication, diagnostics tooling, passwordless connections, and known issues to be mindful of after upgrades.

---

## Fresh Capabilities to Explore

### Full Support for Spring Boot 3.4

Spring Cloud Azure 5.19.0 now fully supports Spring Boot 3.4.0 and later. Developers can adopt the newest Spring Boot features within their Azure-connected applications.

See the [release notes](https://github.com/Azure/azure-sdk-for-java/blob/main/sdk/spring/CHANGELOG.md) for details.

### Enhanced Configuration of Authentication Mechanism

Spring Cloud Azure lets you flexibly configure authentication properties, using the `spring.cloud.azure.credential.` prefix for credentials like service principals and managed identities. The new `spring.cloud.azure.credential.token-credential-bean-name` property allows further customization and control over authentication beans, improving the security of your applications.

#### Example: Custom TokenCredential Bean

**Java Bean:**

```java
@Bean
TokenCredential myTokenCredential() {
   // Your concrete TokenCredential instance
}
```

**YAML Property:**

```yaml
spring:
  cloud:
    azure:
      credential:
        token-credential-bean-name: myTokenCredential
```

#### Client-specific Configuration

You may configure the `token-credential-bean-name` for individual Azure SDK clients, such as Blob Storage:

```yaml
spring:
  cloud:
    azure:
      storage:
        blob:
          account-name: <your-azure-storage-account-name>
          container-name: <your-blob-container-name>
          credential:
            token-credential-bean-name: myTokenCredential
```

#### Passwordless Connections

Configure the token credential for various Azure services, e.g., Azure Database for MySQL, PostgreSQL, Redis, and Service Bus.

**Azure Redis Example:**

```yaml
spring:
  data:
    redis:
      host: <your-azure-redis-name>.redis.cache.windows.net
      port: 6380
      ssl:
        enabled: true
      azure:
        passwordless-enabled: true
        credential:
          token-credential-bean-name: myTokenCredential
```

**Service Bus JMS Example:**

```yaml
spring:
  jms:
    servicebus:
      pricing-tier: <your-service-bus-pricing-tier>
      namespace: <your-service-bus-namespace>
      topic-client-id: <topic-client-id>
      passwordless-enabled: true
      credential:
        token-credential-bean-name: myTokenCredential
```

#### Key Vault Property Source Authentication

For Key Vault property sources, `token-credential-bean-name` does not apply as authentication occurs earlier. Instead, a custom TokenCredential can be set during application bootstrap:

```java
public static void main(String[] args) {
  SpringApplication application = new SpringApplication(YourSpringApplication.class);
  application.addBootstrapRegistryInitializer(registry ->
    registry.register(TokenCredential.class, context -> {
      // Your TokenCredential instance
    })
  );
  application.run(args);
}
```

---

## New Java Diagnostic Tool on AKS

The Java Diagnostic Tool (`diag4j`) is a lightweight, nonintrusive diagnostic and monitoring solution for Java applications running on AKS.

**Key features:**

- **Lightweight Integration:** Utilizes Spring Boot Admin and Java Attach Agent for minimal resource impact.
- **Automatic Kubernetes Integration:** Detects pods with actuator endpoints and lists them in the SBA dashboard.
- **Real-time Metrics & Diagnostics:** Presents live app metrics, GC status, environment variables, and allows dynamic log level adjustments.
- **Advanced Diagnostics:** View stack traces, local variables, perform heap/thread dumps, dynamically inject logs.
- **IDE Compatibility:** Integrates with JetBrains IntelliJ for streamlined, live debugging.

- [Get started with Java Diagnostic Tool](https://learn.microsoft.com/azure/developer/java/fundamentals/java-diagnostic-tools-on-aks-overview)
- [YouTube video: Diag4j in action](https://youtu.be/srysxWp2tak)
- [GitHub repository for feedback/issues](https://github.com/microsoft/diag4j)

---

## Common Scenarios

### Dead-lettering a Message Using Service Bus JMS

Consume from the Service Bus Dead Letter Queue (DLQ) like any standard queue using `@JmsListener`.

**Configuration:**

```yaml
spring:
  jms:
    listener:
      session:
        acknowledge-mode: CLIENT
      transacted: false
    servicebus:
      pricing-tier: <your-service-bus-pricing-tier>
      namespace: <your-service-bus-namespace>
      passwordless-enabled: true
```

**Standard Queue Listener:**

```java
@Component
public class QueueReceiveService {
  private final Logger LOGGER = LoggerFactory.getLogger(QueueReceiveService.class);
  private static final String QUEUE_NAME = "<your-queue-name>";

  @JmsListener(destination = QUEUE_NAME, containerFactory = "jmsListenerContainerFactory")
  public void receiveMessage(JmsObjectMessage message) throws JMSException {
    User user = (User) message.getObject();
    LOGGER.info("Received message from queue: {}.", user);
    if (user.getName().toLowerCase().contains("invalid")) {
      message.setIntProperty(JmsMessageSupport.JMS_AMQP_ACK_TYPE, REJECTED);
      message.acknowledge();
      LOGGER.info("Move message into dead letter queue: {}.", user);
    }
  }
}
```

**DLQ Listener:**

```java
@Component
public class DeadLetterQueueReceiveService {
  private final Logger LOGGER = LoggerFactory.getLogger(DeadLetterQueueReceiveService.class);
  private static final String QUEUE_NAME = "<your-queue-name>";
  private static final String DEAD_LETTER_QUEUE_NAME_SUFFIX = "/$deadletterqueue";
  private static final String DEAD_LETTER_QUEUE_NAME = QUEUE_NAME + DEAD_LETTER_QUEUE_NAME_SUFFIX;

  @JmsListener(destination = DEAD_LETTER_QUEUE_NAME, containerFactory = "jmsListenerContainerFactory")
  public void receiveDeadLetterMessage(JmsObjectMessage message) throws JMSException {
    User user = (User) message.getObject();
    LOGGER.info("Received message from dead letter queue: {}.", user);
  }
}
```

---

### Refresh Key Vault Property Sources

Key Vault property sources can now auto-refresh at configurable intervals for each source and property.

```yaml
spring:
  cloud:
    azure:
      keyvault:
        secret:
          property-source-enabled: true
          property-sources:
            - endpoint: ${KEY_VAULT_ENDPOINT_1}
              name: KeyVault1
              refresh-interval: 10s
              secret-keys:
                - one-minutes-available-username
                - one-minutes-available-password
            - endpoint: ${KEY_VAULT_ENDPOINT_2}
              name: KeyVault2
              refresh-interval: 30s
              secret-keys: game-rules
```

---

### Configure Passwordless Connections with Multiple Data Sources

Spring Data can now manage multiple data sources using passwordless authentication. From version 5.18.0, Spring Cloud Azure JDBC Starter supports this scenario:

```yaml
spring:
  datasource:
    read:
      url: <your-azure-database-jdbc-url>
      username: <your-database-user-name-for-read>
      azure:
        passwordless-enabled: true
    write:
      url: <your-azure-database-jdbc-url>
      username: <your-database-user-name-for-write>
      azure:
        passwordless-enabled: true
        credential:
          token-credential-bean-name: myTokenCredential
```

- The read data source uses `DefaultAzureCredential` (e.g., Microsoft Entra ID)
- The write data source uses a custom token credential bean

---

## Protecting Applications with Efficiency

### Key Vault JCA Support for Intermediate Certificates

From version 2.10.0, Key Vault Java Cryptography Architecture (JCA) can read intermediate certificates in certificate chains:

- [Enable HTTPS in Spring Boot](https://learn.microsoft.com/azure/developer/java/spring-framework/configure-spring-boot-starter-java-app-with-azure-key-vault-certificates)
- [Sign and verify JAR files](https://techcommunity.microsoft.com/blog/appsonazureblog/seamlessly-integrating-azure-keyvault-with-jarsigner-for-enhanced-security/4125770)
- [TLS/SSL via Key Vault to JVM](https://learn.microsoft.com/azure/developer/java/fundamentals/java-azure-keyvault-ssl-integration-jvm)
- [TLS/SSL via Key Vault to Apache Tomcat](https://learn.microsoft.com/azure/developer/java/fundamentals/java-azure-keyvault-tomcat-integration?tabs=windows)

### Azure Identity Extensions: TokenCredential Caching

From version 1.2.0, Azure Identity Extensions supports caching of `TokenCredential` objects across authentication attempts. This reduces Microsoft Entra access token request load.

**Disable Caching Example:**

```java
Properties properties = new Properties();
properties.setProperty("user", "<your-database-username>");
properties.setProperty("authenticationPluginClassName", "com.azure.identity.extensions.jdbc.postgresql.AzurePostgresqlAuthenticationPlugin");
properties.setProperty("azure.tokenCredentialCacheEnabled", "false");
String url = "<your-database-jdbc-url>";
Connection connection = DriverManager.getConnection(url, properties);
// use connection as needed
```

---

## Known Issues

A defect regarding merging of global credential properties with each SDK properties bean was fixed in version 5.18.0. Upgrading to 5.19.0 may result in regression test failures for users of global managed identity credentials. Relevant issues:

- [GitHub Issue #42979](https://github.com/Azure/azure-sdk-for-java/issues/42979)
- [GitHub Issue #43787](https://github.com/Azure/azure-sdk-for-java/issues/43787)

---

## Resources

- [Reference Documentation](https://aka.ms/spring/docs)
- [Conceptual Documentation](https://aka.ms/spring/msdocs)
- [Code Samples](https://aka.ms/spring/samples)
- [Spring Version Mapping](https://aka.ms/spring/versions)
- [CHANGELOGS of Spring Cloud Azure](https://github.com/Azure/azure-sdk-for-java/blob/main/sdk/spring/CHANGELOG.md)

Feedback and contributions welcome on [Stack Overflow](https://stackoverflow.com/questions/tagged/spring-cloud-azure) and [GitHub](https://github.com/Azure/azure-sdk-for-java/issues?q=is%3Aissue+is%3Aopen+label%3Aazure-spring).

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/spring-cloud-azure-updates-and-troubleshooting-tips-for-java-on-aks/)
