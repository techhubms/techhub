---
layout: "post"
title: "Execute Power Query Programmatically in Microsoft Fabric"
description: "This news update from the Microsoft Fabric Blog announces the public preview of a new Execute Query API that allows Power Query M scripts to run programmatically across Microsoft Fabric surfaces. Developers and data engineers can now automate, integrate, and scale data transformations using REST APIs, with capabilities for orchestration, hybrid access, and integration with Spark, SQL, and pipelines."
author: "Microsoft Fabric Blog"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://blog.fabric.microsoft.com/en-US/blog/execute-power-query-programmatically-in-microsoft-fabric/"
viewing_mode: "external"
feed_name: "Microsoft Fabric Blog"
feed_url: "https://blog.fabric.microsoft.com/en-us/blog/feed/"
date: 2026-03-02 08:00:00 +00:00
permalink: "/2026-03-02-Execute-Power-Query-Programmatically-in-Microsoft-Fabric.html"
categories: ["Azure", "Coding", "ML"]
tags: ["Apache Arrow", "Automation", "Azure", "CI/CD", "Coding", "Data Engineering", "Data Pipelines", "Data Transformation", "Dataflow Gen2", "Hybrid Connectivity", "M Language", "Microsoft Fabric", "ML", "News", "Orchestration", "Power Query", "Python", "REST API", "Spark Notebooks"]
tags_normalized: ["apache arrow", "automation", "azure", "cislashcd", "coding", "data engineering", "data pipelines", "data transformation", "dataflow gen2", "hybrid connectivity", "m language", "microsoft fabric", "ml", "news", "orchestration", "power query", "python", "rest api", "spark notebooks"]
---

Microsoft Fabric Blog introduces a significant update allowing developers to execute Power Query transformations programmatically using a REST API. This article details automation, integration, and best practices for working with Power Query in Fabric.<!--excerpt_end-->

# Execute Power Query Programmatically in Microsoft Fabric

Microsoft Fabric Blog announces the public preview of the Execute Query API for Power Query, enabling programmatic execution of Power Query M scripts via REST. This evolution transforms Power Query into a programmable data transformation engine accessible from notebooks, pipelines, and applications.

## Why This Matters

Power Query now supports:

- **Automation**: Run M transformations from pipelines or applications
- **Integration**: Combine with Spark, SQL, and orchestration flows
- **Reuse**: Standardize M scripts across execution surfaces
- **Scale**: Utilize Fabric's distributed compute engine
- **Connectivity**: Access 100+ data sources and reach on-premises or private network data via gateway

Power Query is positioned as a first-class compute engine within Microsoft Fabric.

## Execution Surfaces

- **Spark notebooks**: Receive results as Spark or Pandas DataFrames
- **REST API (Execute Query)**: Trigger transformations from any HTTP client
- **Fabric pipelines & notebook jobs**: Integrate Power Query within orchestration workflows
- **Gateway & Live Query**: Run scripts against on-premises data sources

## Quick Start: Execute Query API (REST)

1. **Prerequisites**: Have a Dataflow Gen2 artifact and configured connections in the Fabric workspace.
2. **Acquire an Access Token**:
   - Using Azure CLI or Fabric notebook code
3. **Construct the Endpoint**:
   - `POST https://api.fabric.microsoft.com/v1/workspaces/{workspaceId}/dataflows/{dataflowId}/executeQuery`
   - Authorization required in HTTP header
4. **Provide M Script or Query Reference**:
   - Specify `queryName` and optionally `customMashupDocument` (M script)
5. **Parse Arrow Result Stream**:
   - Use `pyarrow` to read results as a pandas DataFrame

### Example Python (Fabric Notebook)

```python
import requests
import pyarrow as pa
workspace_id = "00000000-0000-0000-0000-000000000000"
artifact_id = "11111111-1111-1111-1111-111111111111"
fabric_token = notebookutils.credentials.getToken("https://analysis.windows.net/powerbi/api/")
headers = {
    "Authorization": f"Bearer {fabric_token}",
    "Content-Type": "application/json",
}
url = f"https://api.fabric.microsoft.com/v1/workspaces/{workspace_id}/dataflows/{artifact_id}/executeQuery"
request_body = {
    "queryName": "Monthly2020Trends",
    "customMashupDocument": """
        section Section1;
        shared Monthly2020Trends = let
          Source = Lakehouse.Contents(null),
          ...
        in Sorted;
    """
}
response = requests.post(url, headers=headers, json=request_body, stream=True)
if response.status_code == 200:
    with pa.ipc.open_stream(response.raw) as reader:
        data_frame = reader.read_pandas()
        display(data_frame)
else:
    print(response.content)
```

## Capabilities

- Automate Power Query across systems
- Pass custom M scripts dynamically at runtime
- Integrate with Spark, Python, SQL, and orchestration flows
- Secure execution respecting roles and gateway policies
- Fast, columnar Arrow streams for result delivery

## Limitations

- 90-second execution timeout per query
- Read-only: no write operations
- No native queries in custom mashup documents
- Connector support for headless execution may vary
- Performance varies by script complexity and data folding
- Proper gateway configuration required for on-premises data

## Pricing

API usage is billed under “Dataflows Gen2 Run Query API” operation. For details, see the [Dataflow Gen2 pricing and billing documentation](https://learn.microsoft.com/fabric/data-factory/pricing-dataflows-gen2).

## Best Practices

- Store and version M scripts in Git
- Test transformations independently before integration
- Monitor responses and performance
- Document parameters and output schemas clearly

## Learn More

- [Execute Query API Reference](https://learn.microsoft.com/en-us/rest/api/fabric/dataflow/query-execution/execute-query)
- [Microsoft Fabric documentation](https://learn.microsoft.com/fabric/)
- [Power Query M language reference](https://learn.microsoft.com/powerquery-m/)

This capability positions Power Query as a versatile engine for automated, scalable data transformation within the Microsoft ecosystem.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/execute-power-query-programmatically-in-microsoft-fabric/)
