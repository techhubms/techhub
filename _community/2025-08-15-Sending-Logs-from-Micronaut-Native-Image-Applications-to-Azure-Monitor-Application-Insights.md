---
layout: "post"
title: "Sending Logs from Micronaut Native Image Applications to Azure Monitor Application Insights"
description: "This guide by Logico_jp details how to configure a Java Micronaut application (with native image support) to send logs and traces directly to Azure Monitor’s Application Insights. It covers the setup of Azure resources, required Maven dependencies, native compilation steps with GraalVM, and full tracing/logging pipeline integration using OpenTelemetry and Application Insights log appenders."
author: "Logico_jp"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-logs-from-micronaut-native-image-applications-to-azure/ba-p/4443867"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-15 04:55:41 +00:00
permalink: "/2025-08-15-Sending-Logs-from-Micronaut-Native-Image-Applications-to-Azure-Monitor-Application-Insights.html"
categories: ["Azure", "Coding"]
tags: ["Application Configuration", "Application Insights", "Azure", "Azure Container Apps", "Azure Monitor", "Cloud Logging", "Coding", "Community", "GraalVM", "Instrumentation", "Java", "Log Analytics Workspace", "Log Appender", "Log Collection", "Logback", "Maven", "Micronaut", "Micronaut CLI", "Microsoft Azure", "Native Image", "OpenTelemetry", "Tracing"]
tags_normalized: ["application configuration", "application insights", "azure", "azure container apps", "azure monitor", "cloud logging", "coding", "community", "graalvm", "instrumentation", "java", "log analytics workspace", "log appender", "log collection", "logback", "maven", "micronaut", "micronaut cli", "microsoft azure", "native image", "opentelemetry", "tracing"]
---

Logico_jp explains step-by-step how to connect Micronaut native image applications to Azure Monitor’s Application Insights, demonstrating the full configuration workflow from resource creation to log/tracing integration and verification.<!--excerpt_end-->

# Sending Logs from Micronaut Native Image Applications to Azure Monitor Application Insights

Author: Logico_jp

## Overview

This guide covers how to send logs and traces from a Micronaut application (including when compiled as a GraalVM native image) to Azure Monitor Application Insights. The steps walk through Azure resource setup, project creation, integration of necessary dependencies, application configuration, and native compilation guidance. The approach ensures comprehensive logging and tracing visibility in Azure Monitor.

---

## 1. Choosing Log Destinations in Azure

- **Default log destinations may differ for Azure managed services** (App Service, Container Apps, etc.).
- For **Azure Container Apps**, options include:
  - Console output to `ContainerAppConsoleLogs_CL`
  - Custom tables in Log Analytics Workspace via Data Collection Endpoint (DCE)
  - **traces** table in Application Insights using a Log Appender (focus of this guide)

More info:

- [Publish Application Logs to Azure Monitor Logs (Micronaut)](https://guides.micronaut.io/latest/micronaut-azure-logging-maven-java.html)
- [Log storage and monitoring options in Azure Container Apps](https://learn.microsoft.com/azure/container-apps/log-options)

---

## 2. Prerequisites

- **Maven:** 3.9.10
- **JDK:** 21
- **Micronaut:** 4.9.0 or later
- Supported log libraries: Log4j2, Logback, JBoss Logging, java.util.logging (Logback used in this example)

---

## 3. Azure Resource Setup

- Create a resource group and an Application Insights resource
- Refer to [Application Insights resource creation docs](https://learn.microsoft.com/en-us/azure/azure-monitor/app/create-workspace-resource?tabs=portal)

---

## 4. Project Generation

- Use Micronaut CLI or [Micronaut Launch](https://micronaut.io/launch/) to generate a new application skeleton supporting GraalVM and Azure tracing.
- Example CLI:

  ```bash
  mn create-app \
    --build=maven \
    --jdk=21 \
    --lang=java \
    --test=junit \
    --features=graalvm,azure-tracing,yaml \
    dev.logicojp.micronaut.azuremonitor-log
  ```

- When using Micronaut Launch, pick: `graalvm`, `azure-tracing`, and `yaml` features.

---

## 5. Maven Dependencies and Plugins

Add the following to `pom.xml`:

**Dependencies:**

```xml
<dependency>
  <groupId>io.opentelemetry.instrumentation</groupId>
  <artifactId>opentelemetry-logback-appender-1.0</artifactId>
</dependency>
<dependency>
  <groupId>com.microsoft.azure</groupId>
  <artifactId>applicationinsights-logging-logback</artifactId>
</dependency>
<dependency>
  <groupId>io.micronaut.tracing</groupId>
  <artifactId>micronaut-tracing-opentelemetry-http</artifactId>
</dependency>
<!-- If not already included -->
<dependency>
  <groupId>com.azure</groupId>
  <artifactId>azure-monitor-opentelemetry-autoconfigure</artifactId>
</dependency>
<dependency>
  <groupId>org.graalvm.buildtools</groupId>
  <artifactId>graalvm-reachability-metadata</artifactId>
  <version>0.11.0</version>
  <classifier>repository</classifier>
  <type>zip</type>
</dependency>
```

**Plugin for GraalVM Native Build:**

```xml
<plugin>
  <groupId>org.graalvm.buildtools</groupId>
  <artifactId>native-maven-plugin</artifactId>
  <configuration>
    <metadataRepository>
      <enabled>true</enabled>
    </metadataRepository>
    <buildArgs combine.children="append">
      <buildArg>-Ob</buildArg>
    </buildArgs>
    <quickBuild>true</quickBuild>
  </configuration>
</plugin>
```

---

## 6. Application Configuration

- Integrate both Application Insights and Azure tracing settings in your `application.yml`.
- Set connection strings via environment variables or directly in the config file.

**Example `application.yml`:**

```yaml
applicationinsights:
  connection:
    string: ${AZURE_MONITOR_CONNECTION_STRING}
  sampling:
    percentage: 100
  instrumentation:
    logging:
      level: "INFO"
    preview:
      captureLogbackMarker: true
      captureControllerSpans: true
azure:
  tracing:
    connection-string: ${AZURE_MONITOR_CONNECTION_STRING}
```

See docs for [Java Application Insights standalone config](https://learn.microsoft.com/en-us/azure/azure-monitor/app/java-standalone-config).

---

## 7. Code Integration

### Enabling Application Insights

- Instantiate OpenTelemetry SDK in your application:

```java
AutoConfiguredOpenTelemetrySdkBuilder sdkBuilder = AutoConfiguredOpenTelemetrySdk.builder();
OpenTelemetry openTelemetry = sdkBuilder.build().getOpenTelemetrySdk();
AzureMonitorAutoConfigure.customize(sdkBuilder, "connectionString");
```

### Logback Appender

- Modify `src/main/resources/logback.xml` to add the OpenTelemetry appender.
- Example snippet:

```xml
<configuration>
  <appender name="STDOUT" class="ch.qos.logback.core.ConsoleAppender">
    <encoder>
      <pattern>%cyan(%d{HH:mm:ss.SSS}) %gray([%thread]) %highlight(%-5level) %magenta(%logger{36}) - %msg%n</pattern>
    </encoder>
  </appender>
  <appender name="OpenTelemetry" class="io.opentelemetry.instrumentation.logback.appender.v1_0.OpenTelemetryAppender">
    <captureExperimentalAttributes>true</captureExperimentalAttributes>
    <captureCodeAttributes>true</captureCodeAttributes>
    <captureMarkerAttribute>true</captureMarkerAttribute>
    <captureKeyValuePairAttributes>true</captureKeyValuePairAttributes>
    <captureMdcAttributes>*</captureMdcAttributes>
  </appender>
  <root level="info">
    <appender-ref ref="STDOUT"/>
    <appender-ref ref="OpenTelemetry"/>
  </root>
</configuration>
```

- Register the OpenTelemetry appender with your OpenTelemetry instance:

```java
OpenTelemetryAppender.install(openTelemetry);
```

---

## 8. Example Controller

Sample controller for creating and logging using the integrated logger:

```java
package dev.logicojp.micronaut;

import io.micronaut.http.HttpStatus;
import io.micronaut.http.MediaType;
import io.micronaut.http.annotation.*;
import io.micronaut.http.exceptions.HttpStatusException;
import io.micronaut.scheduling.TaskExecutors;
import io.micronaut.scheduling.annotation.ExecuteOn;
import io.opentelemetry.api.OpenTelemetry;
import io.opentelemetry.instrumentation.logback.appender.v1_0.OpenTelemetryAppender;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

@Controller("/api/hello")
@ExecuteOn(TaskExecutors.IO)
public class HelloController {
  private static final Logger logger = LoggerFactory.getLogger(HelloController.class);
  public HelloController(OpenTelemetry _openTelemetry) {
    OpenTelemetryAppender.install(_openTelemetry);
    logger.info("OpenTelemetry is configured and ready to use.");
  }
  @Get
  @Produces(MediaType.APPLICATION_JSON)
  public GreetingResponse hello(@QueryValue(value = "name", defaultValue = "World") String name) {
    logger.info("Hello endpoint was called with query parameter: {}", name);
    HelloService helloService = new HelloService();
    GreetingResponse greetingResponse = helloService.greet(name);
    logger.info("Processing complete, returning response");
    return greetingResponse;
  }
  @Post
  @Consumes(MediaType.APPLICATION_JSON)
  @Produces(MediaType.APPLICATION_JSON)
  @Status(HttpStatus.ACCEPTED)
  public void setGreetingPrefix(@Body GreetingPrefix greetingPrefix) {
    String prefix = greetingPrefix.prefix();
    if (prefix == null || prefix.isBlank()) {
      logger.error("Received request to set an empty or null greeting prefix.");
      throw new HttpStatusException(HttpStatus.BAD_REQUEST, "Prefix cannot be null or empty");
    }
    HelloService helloService = new HelloService();
    helloService.setGreetingPrefix(prefix);
    logger.info("Greeting prefix set to: {}", prefix);
  }
}
```

---

## 9. Native Image Build & Configuration

- Build Java application: `mvn clean package`
- Run using GraalVM native-image agent to generate config files and reachability metadata.
- Relevant files: `jni-config.json`, `reflect-config.json`, `proxy-config.json`, `resource-config.json`, `reachability-metadata.json` (stored under `src/main/resources/META-INF/native-image/groupId/artifactId`)
- Build native image: `mvn package -Dpackaging=native-image`
- Tune build/optimization options in `native-image.properties` or Maven config

More info:

- [GraalVM maven plugin options](https://graalvm.github.io/native-build-tools/latest/maven-plugin.html#native-image-options)
- [Collect Metadata with the Tracing Agent](https://www.graalvm.org/latest/reference-manual/native-image/metadata/AutomaticMetadataCollection/)

---

## 10. Verification

- Test REST endpoints (`GET /api/hello?name=...`, `POST /api/hello`) in both JVM and native modes.
- Check Application Insights (traces table) to confirm log entries appear as expected.

---

## 11. Summary and Notes

- Manual log appender configuration is required, so "zero code" integration is not currently possible
- The amount of additional configuration is minimal
- Approach supports both JVM and native-image deployments
- Advanced options available for performance tuning and operational logging

---

> For further reference, review the [related blog post](https://techcommunity.microsoft.com/blog/appsonazureblog/send-signals-from-micronaut-native-image-applications-to-azure-monitor/4443735) and the [Micronaut logging documentation](https://guides.micronaut.io/latest/micronaut-azure-logging-maven-java.html).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/send-logs-from-micronaut-native-image-applications-to-azure/ba-p/4443867)
