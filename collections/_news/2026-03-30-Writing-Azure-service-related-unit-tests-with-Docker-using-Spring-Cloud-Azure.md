---
section_names:
- azure
- devops
date: 2026-03-30 15:00:31 +00:00
feed_name: Microsoft Azure SDK Blog
primary_section: azure
author: Rujun Chen
title: Writing Azure service-related unit tests with Docker using Spring Cloud Azure
tags:
- Azure
- Azure Blob Storage
- Azure SDK
- Azure Service Bus
- Azure Storage
- Azurite
- DevOps
- Docker
- Docker Compose
- Emulators
- Java
- JUnit 5
- Local Integration Testing
- Maven
- News
- Pom.xml
- Service Bus Emulator
- ServiceBusTemplate
- Spring
- Spring Boot
- Spring Boot Test
- Spring Cloud Azure
- Spring Cloud Stream
- Spring Messaging
- Testcontainers
- Unit Test
external_url: https://devblogs.microsoft.com/azure-sdk/writing-azure-service-related-unit-tests-with-docker-using-spring-cloud-azure/
---

Rujun Chen shows how to run Azure-service unit/integration-style tests locally for Spring Boot by spinning up emulators with Docker Compose or Testcontainers, covering Azure Storage (Azurite) and Azure Service Bus (Service Bus emulator) setups and sample JUnit tests.<!--excerpt_end-->

# Writing Azure service-related unit tests with Docker using Spring Cloud Azure

In this post, we explore how to write Azure service-related unit tests for Spring Boot projects using Docker and Spring Cloud Azure.

## What is Spring Cloud Azure?

Spring Cloud Azure is an open-source project that makes it easier to use Azure services in Spring applications.

- User guide: https://learn.microsoft.com/azure/developer/java/spring-framework/spring-cloud-azure-overview

## Why use Docker for unit testing?

Using Docker for unit testing provides an easy way to test Azure-related features without provisioning real Azure resources.

- Verify code changes locally
- Gain confidence before deploying to Azure

## Examples

## Azure Storage Blob

### Using Docker Compose

#### 1) Add dependencies

Add the following dependency to your `pom.xml` file:

```xml
<properties>
  <version.spring.cloud.azure>7.1.0</version.spring.cloud.azure>
</properties>

<dependencyManagement>
  <dependencies>
    <dependency>
      <groupId>com.azure.spring</groupId>
      <artifactId>spring-cloud-azure-dependencies</artifactId>
      <version>${version.spring.cloud.azure}</version>
      <type>pom</type>
      <scope>import</scope>
    </dependency>
  </dependencies>
</dependencyManagement>

<dependencies>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-cloud-azure-starter-storage-blob</artifactId>
  </dependency>
  <dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-test</artifactId>
    <scope>test</scope>
  </dependency>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-cloud-azure-docker-compose</artifactId>
    <scope>test</scope>
  </dependency>
</dependencies>
```

#### 2) Create `storage-compose.yaml`

Create a `storage-compose.yaml` file in the `src/test/resources` directory with the following content:

```yaml
services:
  storage:
    image: mcr.microsoft.com/azure-storage/azurite:latest
    ports:
      - '10000'
      - '10001'
      - '10002'
    command: azurite -l /data --blobHost 0.0.0.0 --queueHost 0.0.0.0 --tableHost 0.0.0.0 --skipApiVersionCheck
```

#### 3) Write a unit test

Write a unit test that uses the Docker container:

```java
import com.azure.spring.cloud.autoconfigure.implementation.context.AzureGlobalPropertiesAutoConfiguration;
import com.azure.spring.cloud.autoconfigure.implementation.storage.blob.AzureStorageBlobAutoConfiguration;
import com.azure.spring.cloud.autoconfigure.implementation.storage.blob.AzureStorageBlobResourceAutoConfiguration;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.autoconfigure.ImportAutoConfiguration;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.io.Resource;
import org.springframework.core.io.WritableResource;
import org.springframework.util.StreamUtils;

import java.io.IOException;
import java.io.OutputStream;
import java.nio.charset.Charset;

import static org.assertj.core.api.Assertions.assertThat;

@SpringBootTest(properties = {
  "spring.docker.compose.skip.in-tests=false",
  "spring.docker.compose.file=classpath:storage-compose.yaml",
  "spring.docker.compose.stop.command=down"
})
public class AzureBlobResourceDockerComposeTest {

  @Value("azure-blob://testcontainers/message.txt")
  private Resource blobFile;

  @Test
  void test() throws IOException {
    String originalContent = "Hello World!";

    try (OutputStream os = ((WritableResource) this.blobFile).getOutputStream()) {
      os.write(originalContent.getBytes());
    }

    String resultContent = StreamUtils.copyToString(this.blobFile.getInputStream(), Charset.defaultCharset());
    assertThat(resultContent).isEqualTo(originalContent);
  }

  @Configuration(proxyBeanMethods = false)
  @ImportAutoConfiguration(classes = {
    AzureGlobalPropertiesAutoConfiguration.class,
    AzureStorageBlobAutoConfiguration.class,
    AzureStorageBlobResourceAutoConfiguration.class
  })
  static class Config {
  }
}
```

In this example, you start a Docker container running **Azurite** (an open-source Azure Storage emulator). The test writes a string to a blob and reads it back to verify the content.

### Using Testcontainers

#### 1) Add dependencies

Add the following dependency to your `pom.xml` file:

```xml
<properties>
  <version.spring.cloud.azure>7.1.0</version.spring.cloud.azure>
</properties>

<dependencyManagement>
  <dependencies>
    <dependency>
      <groupId>com.azure.spring</groupId>
      <artifactId>spring-cloud-azure-dependencies</artifactId>
      <version>${version.spring.cloud.azure}</version>
      <type>pom</type>
      <scope>import</scope>
    </dependency>
  </dependencies>
</dependencyManagement>

<dependencies>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-cloud-azure-starter-storage-blob</artifactId>
  </dependency>
  <dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-test</artifactId>
    <scope>test</scope>
  </dependency>
  <dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-testcontainers</artifactId>
    <scope>test</scope>
  </dependency>
  <dependency>
    <groupId>org.testcontainers</groupId>
    <artifactId>testcontainers-junit-jupiter</artifactId>
    <scope>test</scope>
  </dependency>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-cloud-azure-testcontainers</artifactId>
    <scope>test</scope>
  </dependency>
</dependencies>
```

#### 2) Write a unit test

```java
import com.azure.spring.cloud.autoconfigure.implementation.context.AzureGlobalPropertiesAutoConfiguration;
import com.azure.spring.cloud.autoconfigure.implementation.storage.blob.AzureStorageBlobAutoConfiguration;
import com.azure.spring.cloud.autoconfigure.implementation.storage.blob.AzureStorageBlobResourceAutoConfiguration;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.autoconfigure.ImportAutoConfiguration;
import org.springframework.boot.testcontainers.service.connection.ServiceConnection;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.io.Resource;
import org.springframework.core.io.WritableResource;
import org.springframework.test.context.junit.jupiter.SpringJUnitConfig;
import org.springframework.util.StreamUtils;
import org.testcontainers.containers.GenericContainer;
import org.testcontainers.junit.jupiter.Container;
import org.testcontainers.junit.jupiter.Testcontainers;

import java.io.IOException;
import java.io.OutputStream;
import java.nio.charset.Charset;

import static org.assertj.core.api.Assertions.assertThat;

@SpringJUnitConfig
@Testcontainers
class AzureBlobResourceTestContainerTest {

  @Container
  @ServiceConnection
  private static final GenericContainer<?> AZURITE_CONTAINER = new GenericContainer<>(
    "mcr.microsoft.com/azure-storage/azurite:latest")
    .withExposedPorts(10000)
    .withCommand("azurite --skipApiVersionCheck && azurite -l /data --blobHost 0.0.0.0 --queueHost 0.0.0.0 --tableHost 0.0.0.0");

  @Value("azure-blob://testcontainers/message.txt")
  private Resource blobFile;

  @Test
  void test() throws IOException {
    String originalContent = "Hello World!";

    try (OutputStream os = ((WritableResource) this.blobFile).getOutputStream()) {
      os.write(originalContent.getBytes());
    }

    String resultContent = StreamUtils.copyToString(this.blobFile.getInputStream(), Charset.defaultCharset());
    assertThat(resultContent).isEqualTo(originalContent);
  }

  @Configuration(proxyBeanMethods = false)
  @ImportAutoConfiguration(classes = {
    AzureGlobalPropertiesAutoConfiguration.class,
    AzureStorageBlobAutoConfiguration.class,
    AzureStorageBlobResourceAutoConfiguration.class
  })
  static class Config {
  }
}
```

This starts Azurite via **Testcontainers** and performs the same write/read verification.

## Azure Service Bus

## Spring messaging with Service Bus

### Using Docker Compose

#### 1) Add dependencies

```xml
<properties>
  <version.spring.cloud.azure>7.1.0</version.spring.cloud.azure>
</properties>

<dependencyManagement>
  <dependencies>
    <dependency>
      <groupId>com.azure.spring</groupId>
      <artifactId>spring-cloud-azure-dependencies</artifactId>
      <version>${version.spring.cloud.azure}</version>
      <type>pom</type>
      <scope>import</scope>
    </dependency>
  </dependencies>
</dependencyManagement>

<dependencies>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-messaging-azure-servicebus</artifactId>
  </dependency>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-cloud-azure-starter</artifactId>
  </dependency>
  <dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-test</artifactId>
    <scope>test</scope>
  </dependency>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-cloud-azure-docker-compose</artifactId>
    <scope>test</scope>
  </dependency>
</dependencies>
```

#### 2) Create `servicebus-compose.yaml`

Create `servicebus-compose.yaml` in `src/test/resources`:

```yaml
services:
  servicebus:
    image: mcr.microsoft.com/azure-messaging/servicebus-emulator:latest
    pull_policy: always
    volumes:
      - "./Config.json:/ServiceBus_Emulator/ConfigFiles/Config.json"
    ports:
      - "5672"
    environment:
      SQL_SERVER: sqledge
      MSSQL_SA_PASSWORD: A_Str0ng_Required_Password
      ACCEPT_EULA: Y
    depends_on:
      - sqledge
    networks:
      sb-emulator:
        aliases:
          - "sb-emulator"

  sqledge:
    image: "mcr.microsoft.com/azure-sql-edge:latest"
    networks:
      sb-emulator:
        aliases:
          - "sqledge"
    environment:
      ACCEPT_EULA: Y
      MSSQL_SA_PASSWORD: A_Str0ng_Required_Password

networks:
  sb-emulator:
```

#### 3) Create `Config.json`

Create `Config.json` in `src/test/resources` (example config for namespaces, queues, topics, subscriptions, and rules):

```json
{
  "UserConfig": {
    "Namespaces": [
      {
        "Name": "sbemulatorns",
        "Queues": [
          {
            "Name": "queue.1",
            "Properties": {
              "DeadLetteringOnMessageExpiration": false,
              "DefaultMessageTimeToLive": "PT1H",
              "DuplicateDetectionHistoryTimeWindow": "PT20S",
              "ForwardDeadLetteredMessagesTo": "",
              "ForwardTo": "",
              "LockDuration": "PT1M",
              "MaxDeliveryCount": 10,
              "RequiresDuplicateDetection": false,
              "RequiresSession": false
            }
          }
        ],
        "Topics": [
          {
            "Name": "topic.1",
            "Properties": {
              "DefaultMessageTimeToLive": "PT1H",
              "DuplicateDetectionHistoryTimeWindow": "PT20S",
              "RequiresDuplicateDetection": false
            },
            "Subscriptions": [
              {
                "Name": "subscription.1",
                "Properties": {
                  "DeadLetteringOnMessageExpiration": false,
                  "DefaultMessageTimeToLive": "PT1H",
                  "LockDuration": "PT1M",
                  "MaxDeliveryCount": 10,
                  "ForwardDeadLetteredMessagesTo": "",
                  "ForwardTo": "",
                  "RequiresSession": false
                },
                "Rules": [
                  {
                    "Name": "app-prop-filter-1",
                    "Properties": {
                      "FilterType": "Correlation",
                      "CorrelationFilter": {
                        "ContentType": "application/text",
                        "CorrelationId": "id1",
                        "Label": "subject1",
                        "MessageId": "msgid1",
                        "ReplyTo": "someQueue",
                        "ReplyToSessionId": "sessionId",
                        "SessionId": "session1",
                        "To": "xyz"
                      }
                    }
                  }
                ]
              },
              {
                "Name": "subscription.2",
                "Properties": {
                  "DeadLetteringOnMessageExpiration": false,
                  "DefaultMessageTimeToLive": "PT1H",
                  "LockDuration": "PT1M",
                  "MaxDeliveryCount": 10,
                  "ForwardDeadLetteredMessagesTo": "",
                  "ForwardTo": "",
                  "RequiresSession": false
                },
                "Rules": [
                  {
                    "Name": "user-prop-filter-1",
                    "Properties": {
                      "FilterType": "Correlation",
                      "CorrelationFilter": {
                        "Properties": {
                          "prop3": "value3"
                        }
                      }
                    }
                  }
                ]
              },
              {
                "Name": "subscription.3",
                "Properties": {
                  "DeadLetteringOnMessageExpiration": false,
                  "DefaultMessageTimeToLive": "PT1H",
                  "LockDuration": "PT1M",
                  "MaxDeliveryCount": 10,
                  "ForwardDeadLetteredMessagesTo": "",
                  "ForwardTo": "",
                  "RequiresSession": false
                }
              }
            ]
          }
        ]
      }
    ],
    "Logging": {
      "Type": "File"
    }
  }
}
```

#### 4) Write a unit test

```java
import com.azure.messaging.servicebus.ServiceBusMessage;
import com.azure.messaging.servicebus.ServiceBusSenderClient;
import com.azure.spring.cloud.autoconfigure.implementation.context.AzureGlobalPropertiesAutoConfiguration;
import com.azure.spring.cloud.autoconfigure.implementation.servicebus.AzureServiceBusAutoConfiguration;
import com.azure.spring.cloud.autoconfigure.implementation.servicebus.AzureServiceBusMessagingAutoConfiguration;
import com.azure.spring.cloud.autoconfigure.implementation.servicebus.properties.AzureServiceBusConnectionDetails;
import com.azure.spring.cloud.service.servicebus.consumer.ServiceBusErrorHandler;
import com.azure.spring.cloud.service.servicebus.consumer.ServiceBusRecordMessageListener;
import com.azure.spring.messaging.servicebus.core.ServiceBusTemplate;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.autoconfigure.ImportAutoConfiguration;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.messaging.support.MessageBuilder;

import java.time.Duration;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;

import static org.assertj.core.api.Assertions.assertThat;
import static org.awaitility.Awaitility.waitAtMost;

@SpringBootTest(properties = {
  "spring.docker.compose.skip.in-tests=false",
  "spring.docker.compose.file=classpath:servicebus-compose.yaml",
  "spring.docker.compose.stop.command=down",
  "spring.docker.compose.readiness.timeout=PT5M",
  "spring.cloud.azure.servicebus.namespace=sbemulatorns",
  "spring.cloud.azure.servicebus.entity-name=queue.1",
  "spring.cloud.azure.servicebus.entity-type=queue",
  "spring.cloud.azure.servicebus.producer.entity-name=queue.1",
  "spring.cloud.azure.servicebus.producer.entity-type=queue",
  "spring.cloud.azure.servicebus.processor.entity-name=queue.1",
  "spring.cloud.azure.servicebus.processor.entity-type=queue"
})
class ServiceBusDockerComposeTest {

  @Autowired
  private AzureServiceBusConnectionDetails connectionDetails;

  @Autowired
  private ServiceBusSenderClient senderClient;

  @Autowired
  private ServiceBusTemplate serviceBusTemplate;

  @Test
  void connectionDetailsShouldBeProvidedByFactory() {
    assertThat(connectionDetails).isNotNull();
    assertThat(connectionDetails.getConnectionString())
      .isNotBlank()
      .startsWith("Endpoint=sb://");
  }

  @Test
  void senderClientCanSendMessage() {
    // Wait for Service Bus emulator to be fully ready and queue entity to be available
    // The emulator depends on SQL Edge and needs time to initialize the messaging entities
    waitAtMost(Duration.ofSeconds(120)).pollInterval(Duration.ofSeconds(2)).untilAsserted(() -> {
      this.senderClient.sendMessage(new ServiceBusMessage("Hello World!"));
    });

    waitAtMost(Duration.ofSeconds(30)).pollDelay(Duration.ofSeconds(5)).untilAsserted(() -> {
      assertThat(Config.MESSAGES).contains("Hello World!");
    });
  }

  @Test
  void serviceBusTemplateCanSendMessage() {
    // Wait for Service Bus emulator to be fully ready and queue entity to be available
    // The emulator depends on SQL Edge and needs time to initialize the messaging entities
    waitAtMost(Duration.ofSeconds(120)).pollInterval(Duration.ofSeconds(2)).untilAsserted(() -> {
      this.serviceBusTemplate
        .sendAsync("queue.1", MessageBuilder.withPayload("Hello from ServiceBusTemplate!").build())
        .block(Duration.ofSeconds(10));
    });

    waitAtMost(Duration.ofSeconds(30)).pollDelay(Duration.ofSeconds(5)).untilAsserted(() -> {
      assertThat(Config.MESSAGES).contains("Hello from ServiceBusTemplate!");
    });
  }

  @Configuration(proxyBeanMethods = false)
  @ImportAutoConfiguration(classes = {
    AzureGlobalPropertiesAutoConfiguration.class,
    AzureServiceBusAutoConfiguration.class,
    AzureServiceBusMessagingAutoConfiguration.class
  })
  static class Config {

    private static final Set<String> MESSAGES = ConcurrentHashMap.newKeySet();

    @Bean
    ServiceBusRecordMessageListener processMessage() {
      return context -> {
        MESSAGES.add(context.getMessage().getBody().toString());
      };
    }

    @Bean
    ServiceBusErrorHandler errorHandler() {
      // No-op error handler for tests: acknowledge errors without affecting test execution.
      return (context) -> {
      };
    }
  }
}
```

### Using Testcontainers

#### 1) Add dependencies

```xml
<properties>
  <version.spring.cloud.azure>7.1.0</version.spring.cloud.azure>
</properties>

<dependencyManagement>
  <dependencies>
    <dependency>
      <groupId>com.azure.spring</groupId>
      <artifactId>spring-cloud-azure-dependencies</artifactId>
      <version>${version.spring.cloud.azure}</version>
      <type>pom</type>
      <scope>import</scope>
    </dependency>
  </dependencies>
</dependencyManagement>

<dependencies>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-messaging-azure-servicebus</artifactId>
  </dependency>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-cloud-azure-starter</artifactId>
  </dependency>
  <dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-test</artifactId>
    <scope>test</scope>
  </dependency>
  <dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-testcontainers</artifactId>
    <scope>test</scope>
  </dependency>
  <dependency>
    <groupId>org.testcontainers</groupId>
    <artifactId>testcontainers-junit-jupiter</artifactId>
    <scope>test</scope>
  </dependency>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-cloud-azure-testcontainers</artifactId>
    <scope>test</scope>
  </dependency>
  <dependency>
    <groupId>com.microsoft.sqlserver</groupId>
    <artifactId>mssql-jdbc</artifactId>
    <scope>test</scope>
  </dependency>
</dependencies>
```

#### 2) Create `Config.json`

Use the same `Config.json` structure as in the Docker Compose example.

#### 3) Write a unit test

```java
import com.azure.messaging.servicebus.ServiceBusMessage;
import com.azure.messaging.servicebus.ServiceBusSenderClient;
import com.azure.spring.cloud.autoconfigure.implementation.context.AzureGlobalPropertiesAutoConfiguration;
import com.azure.spring.cloud.autoconfigure.implementation.servicebus.AzureServiceBusAutoConfiguration;
import com.azure.spring.cloud.autoconfigure.implementation.servicebus.AzureServiceBusMessagingAutoConfiguration;
import com.azure.spring.cloud.autoconfigure.implementation.servicebus.properties.AzureServiceBusConnectionDetails;
import com.azure.spring.cloud.service.servicebus.consumer.ServiceBusErrorHandler;
import com.azure.spring.cloud.service.servicebus.consumer.ServiceBusRecordMessageListener;
import com.azure.spring.messaging.servicebus.core.ServiceBusTemplate;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.autoconfigure.ImportAutoConfiguration;
import org.springframework.boot.testcontainers.service.connection.ServiceConnection;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.messaging.support.MessageBuilder;
import org.springframework.test.context.TestPropertySource;
import org.springframework.test.context.junit.jupiter.SpringJUnitConfig;
import org.testcontainers.azure.ServiceBusEmulatorContainer;
import org.testcontainers.containers.MSSQLServerContainer;
import org.testcontainers.containers.Network;
import org.testcontainers.junit.jupiter.Container;
import org.testcontainers.junit.jupiter.Testcontainers;
import org.testcontainers.utility.MountableFile;

import java.time.Duration;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;

import static org.assertj.core.api.Assertions.assertThat;
import static org.awaitility.Awaitility.waitAtMost;

@SpringJUnitConfig
@TestPropertySource(properties = {
  "spring.cloud.azure.servicebus.entity-name=queue.1",
  "spring.cloud.azure.servicebus.entity-type=queue"
})
@Testcontainers
class ServiceBusTestContainerTest {

  private static final Network NETWORK = Network.newNetwork();

  private static final MSSQLServerContainer<?> SQLSERVER = new MSSQLServerContainer<>(
    "mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
    .acceptLicense()
    .withNetwork(NETWORK)
    .withNetworkAliases("sqlserver");

  @Container
  @ServiceConnection
  private static final ServiceBusEmulatorContainer SERVICE_BUS = new ServiceBusEmulatorContainer(
    "mcr.microsoft.com/azure-messaging/servicebus-emulator:latest")
    .acceptLicense()
    .withCopyFileToContainer(
      MountableFile.forClasspathResource("Config.json"),
      "/ServiceBus_Emulator/ConfigFiles/Config.json")
    .withNetwork(NETWORK)
    .withMsSqlServerContainer(SQLSERVER);

  @Autowired
  private AzureServiceBusConnectionDetails connectionDetails;

  @Autowired
  private ServiceBusSenderClient senderClient;

  @Autowired
  private ServiceBusTemplate serviceBusTemplate;

  @Test
  void connectionDetailsShouldBeProvidedByFactory() {
    assertThat(connectionDetails).isNotNull();
    assertThat(connectionDetails.getConnectionString())
      .isNotBlank()
      .startsWith("Endpoint=sb://");
  }

  @Test
  void senderClientCanSendMessage() {
    // Wait for Service Bus emulator to be fully ready and queue entity to be available
    waitAtMost(Duration.ofSeconds(120)).pollInterval(Duration.ofSeconds(2)).untilAsserted(() -> {
      this.senderClient.sendMessage(new ServiceBusMessage("Hello World!"));
    });

    waitAtMost(Duration.ofSeconds(30)).untilAsserted(() -> {
      assertThat(Config.MESSAGES).contains("Hello World!");
    });
  }

  @Test
  void serviceBusTemplateCanSendMessage() {
    // Wait for Service Bus emulator to be fully ready and queue entity to be available
    waitAtMost(Duration.ofSeconds(120)).pollInterval(Duration.ofSeconds(2)).untilAsserted(() -> {
      this.serviceBusTemplate
        .sendAsync("queue.1", MessageBuilder.withPayload("Hello from ServiceBusTemplate!").build())
        .block(Duration.ofSeconds(10));
    });

    waitAtMost(Duration.ofSeconds(30)).untilAsserted(() -> {
      assertThat(Config.MESSAGES).contains("Hello from ServiceBusTemplate!");
    });
  }

  @Configuration(proxyBeanMethods = false)
  @ImportAutoConfiguration(classes = {
    AzureGlobalPropertiesAutoConfiguration.class,
    AzureServiceBusAutoConfiguration.class,
    AzureServiceBusMessagingAutoConfiguration.class
  })
  static class Config {

    private static final Set<String> MESSAGES = ConcurrentHashMap.newKeySet();

    @Bean
    ServiceBusRecordMessageListener processMessage() {
      return context -> {
        MESSAGES.add(context.getMessage().getBody().toString());
      };
    }

    @Bean
    ServiceBusErrorHandler errorHandler() {
      // No-op error handler for tests: acknowledge errors without affecting test execution.
      return (context) -> {
      };
    }
  }
}
```

## Spring Cloud Stream binders with Service Bus

### Using Docker Compose

#### 1) Add dependencies

```xml
<properties>
  <version.spring.cloud.azure>7.1.0</version.spring.cloud.azure>
</properties>

<dependencyManagement>
  <dependencies>
    <dependency>
      <groupId>com.azure.spring</groupId>
      <artifactId>spring-cloud-azure-dependencies</artifactId>
      <version>${version.spring.cloud.azure}</version>
      <type>pom</type>
      <scope>import</scope>
    </dependency>
  </dependencies>
</dependencyManagement>

<dependencies>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-cloud-azure-stream-binder-servicebus</artifactId>
  </dependency>
  <dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-test</artifactId>
    <scope>test</scope>
  </dependency>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-cloud-azure-autoconfigure</artifactId>
    <scope>test</scope>
  </dependency>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-cloud-azure-docker-compose</artifactId>
    <scope>test</scope>
  </dependency>
</dependencies>
```

#### 2) Create `servicebus-compose.yaml` and `Config.json`

Use the same `servicebus-compose.yaml` and `Config.json` approach described earlier.

#### 3) Write a unit test

```java
import com.azure.spring.cloud.autoconfigure.implementation.context.AzureGlobalPropertiesAutoConfiguration;
import com.azure.spring.cloud.autoconfigure.implementation.servicebus.AzureServiceBusAutoConfiguration;
import com.azure.spring.cloud.autoconfigure.implementation.servicebus.AzureServiceBusMessagingAutoConfiguration;
import com.azure.spring.messaging.checkpoint.Checkpointer;
import org.junit.jupiter.api.Test;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.boot.autoconfigure.EnableAutoConfiguration;
import org.springframework.boot.autoconfigure.ImportAutoConfiguration;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.messaging.Message;
import org.springframework.messaging.support.MessageBuilder;

import java.time.Duration;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.function.Consumer;
import java.util.function.Supplier;

import static com.azure.spring.messaging.AzureHeaders.CHECKPOINTER;
import static org.assertj.core.api.Assertions.assertThat;
import static org.awaitility.Awaitility.waitAtMost;

@SpringBootTest(properties = {
  "spring.docker.compose.skip.in-tests=false",
  "spring.docker.compose.file=classpath:servicebus-compose.yaml",
  "spring.docker.compose.stop.command=down",
  "spring.cloud.function.definition=consume;supply",
  "spring.cloud.stream.bindings.consume-in-0.destination=queue.1",
  "spring.cloud.stream.bindings.supply-out-0.destination=queue.1",
  "spring.cloud.stream.servicebus.bindings.consume-in-0.consumer.auto-complete=false",
  "spring.cloud.stream.servicebus.bindings.supply-out-0.producer.entity-type=queue",
  "spring.cloud.stream.poller.fixed-delay=1000",
  "spring.cloud.stream.poller.initial-delay=0"
})
class ServiceBusDockerComposeTest {

  private static final Logger LOGGER = LoggerFactory.getLogger(ServiceBusDockerComposeTest.class);
  private static final Set<String> RECEIVED_MESSAGES = ConcurrentHashMap.newKeySet();
  private static final AtomicInteger MESSAGE_SEQUENCE = new AtomicInteger(0);

  @Test
  void supplierAndConsumerShouldWorkThroughServiceBusQueue() {
    waitAtMost(Duration.ofSeconds(60))
      .pollDelay(Duration.ofSeconds(2))
      .untilAsserted(() -> {
        assertThat(RECEIVED_MESSAGES).isNotEmpty();
        LOGGER.info("✓ Test passed - Consumer received {} message(s)", RECEIVED_MESSAGES.size());
      });
  }

  @Configuration(proxyBeanMethods = false)
  @EnableAutoConfiguration
  @ImportAutoConfiguration(classes = {
    AzureGlobalPropertiesAutoConfiguration.class,
    AzureServiceBusAutoConfiguration.class,
    AzureServiceBusMessagingAutoConfiguration.class
  })
  static class Config {

    @Bean
    public Supplier<Message<String>> supply() {
      return () -> {
        int sequence = MESSAGE_SEQUENCE.getAndIncrement();
        String payload = "Hello world, " + sequence;
        LOGGER.info("[Supplier] Invoked - message sequence: {}", sequence);
        return MessageBuilder.withPayload(payload).build();
      };
    }

    @Bean
    public Consumer<Message<String>> consume() {
      return message -> {
        String payload = message.getPayload();
        RECEIVED_MESSAGES.add(payload);
        LOGGER.info("[Consumer] Received message: {}", payload);

        Checkpointer checkpointer = (Checkpointer) message.getHeaders().get(CHECKPOINTER);
        if (checkpointer != null) {
          checkpointer.success()
            .doOnSuccess(s -> LOGGER.info("[Consumer] Message checkpointed"))
            .doOnError(e -> LOGGER.error("[Consumer] Checkpoint failed", e))
            .block();
        }
      };
    }
  }
}
```

### Using Testcontainers

#### 1) Add dependencies

```xml
<properties>
  <version.spring.cloud.azure>7.1.0</version.spring.cloud.azure>
</properties>

<dependencyManagement>
  <dependencies>
    <dependency>
      <groupId>com.azure.spring</groupId>
      <artifactId>spring-cloud-azure-dependencies</artifactId>
      <version>${version.spring.cloud.azure}</version>
      <type>pom</type>
      <scope>import</scope>
    </dependency>
  </dependencies>
</dependencyManagement>

<dependencies>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-cloud-azure-stream-binder-servicebus</artifactId>
  </dependency>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-cloud-azure-starter</artifactId>
  </dependency>
  <dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-starter-test</artifactId>
    <scope>test</scope>
  </dependency>
  <dependency>
    <groupId>org.springframework.boot</groupId>
    <artifactId>spring-boot-testcontainers</artifactId>
    <scope>test</scope>
  </dependency>
  <dependency>
    <groupId>org.testcontainers</groupId>
    <artifactId>testcontainers-junit-jupiter</artifactId>
    <scope>test</scope>
  </dependency>
  <dependency>
    <groupId>com.azure.spring</groupId>
    <artifactId>spring-cloud-azure-testcontainers</artifactId>
    <scope>test</scope>
  </dependency>
  <dependency>
    <groupId>com.microsoft.sqlserver</groupId>
    <artifactId>mssql-jdbc</artifactId>
    <scope>test</scope>
  </dependency>
</dependencies>
```

#### 2) Create `Config.json`

Use the same `Config.json` structure as in the Docker Compose example.

#### 3) Write a unit test

```java
import com.azure.spring.cloud.autoconfigure.implementation.context.AzureGlobalPropertiesAutoConfiguration;
import com.azure.spring.cloud.autoconfigure.implementation.servicebus.AzureServiceBusAutoConfiguration;
import com.azure.spring.cloud.autoconfigure.implementation.servicebus.AzureServiceBusMessagingAutoConfiguration;
import com.azure.spring.messaging.checkpoint.Checkpointer;
import org.junit.jupiter.api.Test;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.boot.autoconfigure.EnableAutoConfiguration;
import org.springframework.boot.autoconfigure.ImportAutoConfiguration;
import org.springframework.boot.testcontainers.service.connection.ServiceConnection;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.messaging.Message;
import org.springframework.messaging.support.MessageBuilder;
import org.springframework.test.context.TestPropertySource;
import org.springframework.test.context.junit.jupiter.SpringJUnitConfig;
import org.testcontainers.azure.ServiceBusEmulatorContainer;
import org.testcontainers.containers.MSSQLServerContainer;
import org.testcontainers.containers.Network;
import org.testcontainers.junit.jupiter.Container;
import org.testcontainers.junit.jupiter.Testcontainers;
import org.testcontainers.utility.MountableFile;

import java.time.Duration;
import java.util.Set;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.function.Consumer;
import java.util.function.Supplier;

import static com.azure.spring.messaging.AzureHeaders.CHECKPOINTER;
import static org.assertj.core.api.Assertions.assertThat;
import static org.awaitility.Awaitility.waitAtMost;

@SpringJUnitConfig
@TestPropertySource(properties = {
  "spring.cloud.function.definition=consume;supply",
  "spring.cloud.stream.bindings.consume-in-0.destination=queue.1",
  "spring.cloud.stream.bindings.supply-out-0.destination=queue.1",
  "spring.cloud.stream.servicebus.bindings.consume-in-0.consumer.auto-complete=false",
  "spring.cloud.stream.servicebus.bindings.supply-out-0.producer.entity-type=queue",
  "spring.cloud.stream.poller.fixed-delay=1000",
  "spring.cloud.stream.poller.initial-delay=0"
})
@Testcontainers
class ServiceBusTestContainerTest {

  private static final Network NETWORK = Network.newNetwork();

  private static final MSSQLServerContainer<?> SQLSERVER = new MSSQLServerContainer<>(
    "mcr.microsoft.com/mssql/server:2022-CU14-ubuntu-22.04")
    .acceptLicense()
    .withNetwork(NETWORK)
    .withNetworkAliases("sqlserver");

  @Container
  @ServiceConnection
  private static final ServiceBusEmulatorContainer SERVICE_BUS = new ServiceBusEmulatorContainer(
    "mcr.microsoft.com/azure-messaging/servicebus-emulator:latest")
    .acceptLicense()
    .withCopyFileToContainer(
      MountableFile.forClasspathResource("Config.json"),
      "/ServiceBus_Emulator/ConfigFiles/Config.json")
    .withNetwork(NETWORK)
    .withMsSqlServerContainer(SQLSERVER);

  private static final Logger LOGGER = LoggerFactory.getLogger(ServiceBusTestContainerTest.class);
  private static final Set<String> RECEIVED_MESSAGES = ConcurrentHashMap.newKeySet();
  private static final AtomicInteger MESSAGE_SEQUENCE = new AtomicInteger(0);

  @Test
  void supplierAndConsumerShouldWorkThroughServiceBusQueue() {
    waitAtMost(Duration.ofSeconds(60))
      .pollDelay(Duration.ofSeconds(2))
      .untilAsserted(() -> {
        assertThat(RECEIVED_MESSAGES).isNotEmpty();
        LOGGER.info("✓ Test passed - Consumer received {} message(s)", RECEIVED_MESSAGES.size());
      });
  }

  @Configuration(proxyBeanMethods = false)
  @EnableAutoConfiguration
  @ImportAutoConfiguration(classes = {
    AzureGlobalPropertiesAutoConfiguration.class,
    AzureServiceBusAutoConfiguration.class,
    AzureServiceBusMessagingAutoConfiguration.class
  })
  static class Config {

    @Bean
    public Supplier<Message<String>> supply() {
      return () -> {
        int sequence = MESSAGE_SEQUENCE.getAndIncrement();
        String payload = "Hello world, " + sequence;
        LOGGER.info("[Supplier] Invoked - message sequence: {}", sequence);
        return MessageBuilder.withPayload(payload).build();
      };
    }

    @Bean
    public Consumer<Message<String>> consume() {
      return message -> {
        String payload = message.getPayload();
        RECEIVED_MESSAGES.add(payload);
        LOGGER.info("[Consumer] Received message: {}", payload);

        Checkpointer checkpointer = (Checkpointer) message.getHeaders().get(CHECKPOINTER);
        if (checkpointer != null) {
          checkpointer.success()
            .doOnSuccess(s -> LOGGER.info("[Consumer] Message checkpointed"))
            .doOnError(e -> LOGGER.error("[Consumer] Checkpoint failed", e))
            .block();
        }
      };
    }
  }
}
```

## More examples

- Docker Compose examples: https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/spring-cloud-azure-docker-compose
- Testcontainers examples: https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/spring-cloud-azure-testcontainers

## Summary

This post walks through writing unit tests with Docker for Spring Cloud Azure applications, including examples for Azure Storage and Azure Service Bus using Docker Compose and Testcontainers. The goal is to test Azure integrations locally without provisioning real Azure resources, while still validating that the application behaves correctly before deployment.

[Read the entire article](https://devblogs.microsoft.com/azure-sdk/writing-azure-service-related-unit-tests-with-docker-using-spring-cloud-azure/)

