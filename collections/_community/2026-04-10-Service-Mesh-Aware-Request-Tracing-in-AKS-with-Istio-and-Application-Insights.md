---
feed_name: Microsoft Tech Community
author: Siddhi_Singh
date: 2026-04-10 16:37:58 +00:00
external_url: https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/service-mesh-aware-request-tracing-in-aks-with-istio-and/ba-p/4509928
primary_section: azure
tags:
- Access Logs
- AKS
- Azure
- Azure Application Insights
- Azure Monitor
- Community
- ContainerLogV2
- DevOps
- Envoy
- EnvoyFilter
- Istio
- Istio Telemetry API
- JSON Logs
- KQL
- Kubectl
- Log Analytics
- Managed Istio
- Observability
- Operation Id
- PowerShell Automation
- Request Correlation
- Service Mesh
- Structured Logging
- Traceparent
- Tracestate
- W3C Trace Context
section_names:
- azure
- devops
title: Service Mesh-Aware Request Tracing in AKS with Istio and Application Insights
---

Siddhi_Singh shows how to enable Istio/Envoy access logging in Azure Kubernetes Service (AKS) and correlate those mesh logs with Azure Application Insights telemetry using W3C trace context and Log Analytics (KQL) for end-to-end request troubleshooting.<!--excerpt_end-->

## Introduction

As platforms evolve toward microservice-based architectures, observability becomes more complex. In **Azure Kubernetes Service (AKS)**, teams often use **Istio** for service-to-service communication and **Azure Application Insights** for application-level telemetry.

Because these operate at different layers, correlating a single request across the service mesh and the application layer requires deliberate configuration.

This guide walks through a production-oriented approach to:

- Enable **Istio (Envoy) access logging** in AKS
- Ingest logs into **Azure Monitor / Log Analytics**
- Correlate Envoy access logs with **Application Insights** telemetry using a shared trace context

## Platform observability context

Environment components:

- AKS with **managed Istio** enabled
- **Envoy sidecars** injected into application pods
- **Application Insights SDK** running inside workloads
- **Log Analytics** as the centralized log store

Goal:

- Align service mesh and application telemetry using a **common trace context**
- Avoid adding extra tracing systems or custom agents

## Enabling Istio access logging at the mesh level

Istio’s **Telemetry API** can enable access logging centrally (without changing individual workloads).

Apply a `Telemetry` resource in the Istio system namespace to enable Envoy access logging:

```yaml
apiVersion: telemetry.istio.io/v1
kind: Telemetry
metadata:
  name: mesh-access-logs
  namespace: aks-istio-system
spec:
  accessLogging:
    - providers:
        - name: envoy
```

This ensures:

- All Envoy sidecars emit access logs
- Logging is consistent across the mesh
- The setup remains compatible with **AKS managed Istio**

## Standardizing Envoy logs using EnvoyFilter

At scale, access logs need to be structured. In AKS managed Istio, direct Envoy configuration is restricted, so **EnvoyFilter** is used to customize logging behavior.

EnvoyFilters are configured to:

- Emit **structured JSON** logs
- Write logs to `/dev/stdout`
- Include trace and request correlation headers

To cover the full request path, apply separate EnvoyFilters for **inbound** and **outbound** traffic.

### Inbound sidecar access logs

```yaml
apiVersion: networking.istio.io/v1alpha3
kind: EnvoyFilter
metadata:
  name: json-access-logs
  namespace: aks-istio-system
spec:
  configPatches:
    - applyTo: NETWORK_FILTER
      match:
        context: SIDECAR_INBOUND
        listener:
          filterChain:
            filter:
              name: envoy.filters.network.http_connection_manager
      patch:
        operation: MERGE
        value:
          typed_config:
            "@type": type.googleapis.com/envoy.extensions.filters.network.http_connection_manager.v3.HttpConnectionManager
            access_log:
              - name: envoy.access_loggers.file
                typed_config:
                  "@type": type.googleapis.com/envoy.extensions.access_loggers.file.v3.FileAccessLog
                  path: /dev/stdout
                  log_format:
                    json_format:
                      timestamp: "%START_TIME%"
                      method: "%REQ(:METHOD)%"
                      path: "%REQ(X-ENVOY-ORIGINAL-PATH?:PATH)%"
                      response_code: "%RESPONSE_CODE%"
                      response_flags: "%RESPONSE_FLAGS%"
                      duration_ms: "%DURATION%"
                      downstream_remote_address: "%DOWNSTREAM_REMOTE_ADDRESS%"
                      x_request_id: "%REQ(X-REQUEST-ID)%"
                      traceparent: "%REQ(TRACEPARENT)%"
                      tracestate: "%REQ(TRACESTATE)%"
                      x_b3_traceid: "%REQ(X-B3-TRACEID)%"
```

This ensures inbound traffic logs include both request metadata and correlation identifiers.

### Outbound sidecar access logs

```yaml
apiVersion: networking.istio.io/v1alpha3
kind: EnvoyFilter
metadata:
  name: json-access-logs-outbound
  namespace: aks-istio-system
spec:
  configPatches:
    - applyTo: NETWORK_FILTER
      match:
        context: SIDECAR_OUTBOUND
        listener:
          filterChain:
            filter:
              name: envoy.filters.network.http_connection_manager
      patch:
        operation: MERGE
        value:
          typed_config:
            "@type": type.googleapis.com/envoy.extensions.filters.network.http_connection_manager.v3.HttpConnectionManager
            access_log:
              - name: envoy.access_loggers.file
                typed_config:
                  "@type": type.googleapis.com/envoy.extensions.access_loggers.file.v3.FileAccessLog
                  path: /dev/stdout
                  log_format:
                    json_format:
                      timestamp: "%START_TIME%"
                      method: "%REQ(:METHOD)%"
                      path: "%REQ(X-ENVOY-ORIGINAL-PATH?:PATH)%"
                      response_code: "%RESPONSE_CODE%"
                      response_flags: "%RESPONSE_FLAGS%"
                      duration_ms: "%DURATION%"
                      downstream_remote_address: "%DOWNSTREAM_REMOTE_ADDRESS%"
                      x_request_id: "%REQ(X-REQUEST-ID)%"
                      traceparent: "%REQ(TRACEPARENT)%"
                      tracestate: "%REQ(TRACESTATE)%"
                      x_b3_traceid: "%REQ(X-B3-TRACEID)%"
```

With both inbound and outbound filters, logs follow the same schema and are easier to query consistently.

## Automating the configuration with PowerShell

To repeat the setup across environments, wrap it in a script that:

- Validates the Istio system namespace
- Applies the `Telemetry` resource
- Applies inbound and outbound `EnvoyFilter` resources

Example (as described):

```powershell
$MeshRootNamespace = "aks-istio-system"
$TelemetryName = "mesh-access-logs"
$EnvoyFilterName = "json-access-logs"

kubectl get ns $MeshRootNamespace --ignore-not-found

$telemetryYaml | kubectl apply -f -
$envoyFilterYaml | kubectl apply -f -
$envoyFilterOutboundYaml | kubectl apply -f -
```

## Log ingestion into Azure Monitor

Because Envoy access logs are written to standard output:

- AKS automatically collects them
- Logs are ingested into **Log Analytics**
- Data appears in the `ContainerLogV2` table

No additional agents or custom log pipelines are required.

## Aligning with Application Insights telemetry

Application Insights uses **W3C Trace Context**, where `operation_Id` represents the trace identifier.

Because Envoy access logs capture the `traceparent` header, both layers expose the same trace ID. This enables correlation between:

- Istio/Envoy access logs in `ContainerLogV2`
- Application request telemetry in Application Insights

without changing application code.

## Correlating requests using KQL

Approach:

- Parse JSON Envoy access logs from `ContainerLogV2`
- Extract the trace ID from `traceparent`
- Filter on a known Application Insights `operation_Id`

Example query (filter Istio access logs using an Application Insights `operation_Id`):

```kusto
let operationId = "<OperationID>"; // Replace with your actual operation_Id
ContainerLogV2
| where TimeGenerated >= ago(24h)
| where ContainerName == "istio-proxy"
| where LogSource == "stdout"
| where LogMessage startswith "{"
| extend AccessLog = parse_json(LogMessage)
| extend ExtractedOperationId = extract(@"00-([a-f0-9]{32})-", 1, tostring(AccessLog.traceparent))
| where ExtractedOperationId == operationId
| project
    TimeGenerated,
    PodName,
    Method = tostring(AccessLog.method),
    Path = tostring(AccessLog.path),
    ResponseCode = toint(AccessLog.response_code),
    RequestId = tostring(AccessLog.x_request_id),
    TraceParent = tostring(AccessLog.traceparent),
    TraceState = tostring(AccessLog.tracestate),
    Authority = tostring(AccessLog.authority),
    RawLogMessage = LogMessage
| order by TimeGenerated asc
```

## Closing thoughts

End-to-end request tracing in AKS can be achieved by aligning **service mesh logging** and **application telemetry** around shared standards.

By enabling structured Istio access logs and correlating them with Application Insights through W3C trace context, you get request flow visibility across networking and application layers using Azure-native tooling, without adding extra platform components.

[Read the entire article](https://techcommunity.microsoft.com/t5/azure-infrastructure-blog/service-mesh-aware-request-tracing-in-aks-with-istio-and/ba-p/4509928)

