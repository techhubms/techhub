---
feed_name: Microsoft Tech Community
author: AnaviNahar
section_names:
- azure
- ml
primary_section: ml
title: Near–Real-Time CDC to Delta Lake for BI and ML with Lakeflow on Azure Databricks
date: 2026-03-18 16:00:00 +00:00
tags:
- AI/BI Dashboards
- Azure
- Azure Databricks
- Azure ExpressRoute
- Bronze Silver Gold
- Change Data Capture (cdc)
- Community
- Data Governance
- Data Lineage
- Data Quality Expectations
- Databricks Jobs
- Delta Lake
- ELT
- ETL
- Genie
- gRPC
- Lakeflow
- Lakeflow Connect
- Materialized Views
- ML
- Natural Language To SQL
- Orchestration
- SCD Type 1
- SCD Type 2
- Service Principals
- SQL Server
- Streaming Tables
- System Tables
- Unity Catalog
- Zerobus Ingest
external_url: https://techcommunity.microsoft.com/t5/azure-databricks/near-real-time-cdc-to-delta-lake-for-bi-and-ml-with-lakeflow-on/ba-p/4502750
---

AnaviNahar walks through a near-real-time ingestion and transformation setup on Azure Databricks using Lakeflow (Connect, Spark Declarative Pipelines, and Jobs), covering CDC from SQL Server, streaming telemetry ingestion, Bronze/Silver/Gold modeling, Unity Catalog governance, and monitoring via system tables.<!--excerpt_end-->

# Near–Real-Time CDC to Delta Lake for BI and ML with Lakeflow on Azure Databricks

## The challenge: too many tools, not enough clarity

Modern data teams on Azure often stitch together separate orchestrators, custom streaming consumers, hand-rolled transformation notebooks, and third-party connectors — each with its own monitoring UI, credential system, and failure modes. The result is observability gaps, weeks of work per new data source, disconnected lineage, and governance bolted on as an afterthought.

[Lakeflow](https://www.databricks.com/product/data-engineering), Databricks’ unified data engineering solution, consolidates ingestion, transformation, and orchestration natively inside Azure Databricks — governed end-to-end by [Unity Catalog](https://www.databricks.com/product/unity-catalog).

| Component | What it does |
| --- | --- |
| [Lakeflow Connect](https://learn.microsoft.com/en-us/azure/databricks/ingestion/lakeflow-connect/) | Point-and-click connectors for databases (using CDC), SaaS apps, files, streaming, and Zerobus for direct telemetry |
| [Lakeflow Spark Declarative Pipelines](https://learn.microsoft.com/en-us/azure/databricks/ldp/concepts) | Declarative ETL with AutoCDC, data quality enforcement, and automatic incremental processing |
| [Lakeflow Jobs](https://learn.microsoft.com/en-us/azure/databricks/jobs/) | Managed orchestration with 99.95% uptime, a visual task DAG, and repair-and-rerun |

## Architecture

### Step 1: Stream application telemetry with Zerobus Ingest

[Zerobus Ingest](https://learn.microsoft.com/en-us/azure/databricks/ingestion/zerobus-overview) (part of Lakeflow Connect) lets your application push events directly to a Delta table over gRPC — no message bus, no Structured Streaming job. It targets sub-5-second latency and up to 100 MB/sec per connection, with data immediately queryable in Unity Catalog.

#### Prerequisites

- Azure Databricks workspace with Unity Catalog enabled and serverless compute on
- A service principal with write access to the target table

#### Setup

Create the target table in a SQL notebook:

```sql
CREATE CATALOG IF NOT EXISTS prod;
CREATE SCHEMA IF NOT EXISTS prod.bronze;

CREATE TABLE IF NOT EXISTS prod.bronze.telemetry_events (
  event_id     STRING,
  user_id      STRING,
  event_type   STRING,
  session_id   STRING,
  ts           BIGINT,
  page         STRING,
  duration_ms  INT
);
```

Create and authorize a service principal:

1. Go to **Settings → Identity and Access → Service Principals → Add service principal**
2. Open the service principal → **Secrets** tab → **Generate secret**. Save the **Client ID** and secret.
3. In a SQL notebook, grant access:

```sql
GRANT USE CATALOG ON CATALOG prod TO `<client-id>`;
GRANT USE SCHEMA ON SCHEMA prod.bronze TO `<client-id>`;
GRANT MODIFY, SELECT ON TABLE prod.bronze.telemetry_events TO `<client-id>`;
```

Derive the Zerobus endpoint from your workspace URL:

- `<workspace-id>.zerobus.<region>.azuredatabricks.net`
- The workspace ID is the number in your workspace URL (for example: `adb-**1234567890**.12.azuredatabricks.net`).

Install the SDK:

```bash
pip install databricks-zerobus-ingest-sdk
```

In your application, open a stream and push records:

```python
from zerobus.sdk.sync import ZerobusSdk
from zerobus.sdk.shared import RecordType, StreamConfigurationOptions, TableProperties

sdk = ZerobusSdk(
  "<workspace-id>.zerobus.<region>.azuredatabricks.net",
  "https://<workspace-url>"
)

stream = sdk.create_stream(
  "<client-id>",
  "<client-secret>",
  TableProperties("prod.bronze.telemetry_events"),
  StreamConfigurationOptions(record_type=RecordType.JSON)
)

stream.ingest_record({
  "event_id": "e1",
  "user_id": "u42",
  "event_type": "page_view",
  "ts": 1700000000000
})

stream.close()
```

Verify in **Catalog → prod → bronze → telemetry_events → Sample Data**.

### Step 2: Ingest from on-premises SQL Server via CDC

Lakeflow Connect reads SQL Server’s transaction log incrementally (CDC) rather than full table scans. The example assumes connectivity from Azure Databricks to on-prem SQL Server over Azure ExpressRoute.

#### Prerequisites

- SQL Server reachable from Databricks over ExpressRoute (TCP port 1433)
- CDC enabled on the source database and tables
- A SQL login with CDC read permissions on the source database
- Databricks privileges: `CREATE CONNECTION` on the metastore; `USE CATALOG`, `CREATE TABLE` on the destination catalog

#### Setup

Enable CDC on SQL Server:

```sql
USE YourDatabase;
EXEC sys.sp_cdc_enable_db;

EXEC sys.sp_cdc_enable_table
  @source_schema = N'dbo',
  @source_name = N'orders',
  @role_name = NULL;

EXEC sys.sp_cdc_enable_table
  @source_schema = N'dbo',
  @source_name = N'customers',
  @role_name = NULL;
```

Configure the connector in Databricks:

1. Click **Data Ingestion** in the sidebar (or **+ New → Add Data**)
2. Select **SQL Server** from the connector list
3. **Ingestion Gateway**: enter a gateway name, select staging catalog/schema, click **Next**
4. **Ingestion Pipeline**: name the pipeline, click **Create connection**
5. Host: your on-prem IP (for example `10.0.1.50`) · Port: `1433` · Database: `YourDatabase`
6. Enter credentials, click **Create**, then **Create pipeline and continue**
7. **Source**: select `dbo.orders` and `dbo.customers`; optionally enable **History tracking (SCD Type 2)** per table; set destination tables to `orders_raw` and `customers_raw`
8. **Destination**: catalog `prod`, schema `bronze`, click **Save and continue**
9. **Settings**: set a schedule (for example every 5 minutes), click **Save and run pipeline**

### Step 3: Transform with Spark Declarative Pipelines

The [Lakeflow Pipelines Editor](https://learn.microsoft.com/en-us/azure/databricks/ldp/multi-file-editor) is used to define Bronze → Silver → Gold in SQL. The platform handles incremental execution, schema evolution, and lineage.

#### Prerequisites

- Bronze tables populated (from Steps 1 and 2)
- `CREATE TABLE` and `USE SCHEMA` privileges on `prod.silver` and `prod.gold`

#### Setup

1. In the sidebar, click **Jobs & Pipelines → ETL pipeline → Start with an empty file → SQL**
2. Rename the pipeline to `lakeflow-demo-pipeline`
3. Paste the following SQL:

```sql
-- Silver: latest order state (SCD Type 1)
CREATE OR REFRESH STREAMING TABLE prod.silver.orders;

APPLY CHANGES INTO prod.silver.orders
  FROM STREAM(prod.bronze.orders_raw)
  KEYS (order_id)
  SEQUENCE BY updated_at
  STORED AS SCD TYPE 1;

-- Silver: full customer history (SCD Type 2)
CREATE OR REFRESH STREAMING TABLE prod.silver.customers;

APPLY CHANGES INTO prod.silver.customers
  FROM STREAM(prod.bronze.customers_raw)
  KEYS (customer_id)
  SEQUENCE BY updated_at
  STORED AS SCD TYPE 2;

-- Silver: telemetry with data quality check
CREATE OR REFRESH STREAMING TABLE prod.silver.telemetry_events (
  CONSTRAINT valid_event_type
    EXPECT (event_type IN ('page_view', 'add_to_cart', 'purchase'))
    ON VIOLATION DROP ROW
)
AS
SELECT *
FROM STREAM(prod.bronze.telemetry_events);

-- Gold: materialized view joining all three Silver tables
CREATE OR REFRESH MATERIALIZED VIEW prod.gold.customer_activity AS
SELECT
  o.order_id,
  o.customer_id,
  c.customer_name,
  c.email,
  o.order_amount,
  o.order_status,
  COUNT(e.event_id) AS total_events,
  SUM(CASE WHEN e.event_type = 'purchase' THEN 1 ELSE 0 END) AS purchase_events
FROM prod.silver.orders o
LEFT JOIN prod.silver.customers c
  ON o.customer_id = c.customer_id
LEFT JOIN prod.silver.telemetry_events e
  ON CAST(o.customer_id AS STRING) = e.user_id -- user_id in telemetry is string
GROUP BY
  o.order_id,
  o.customer_id,
  c.customer_name,
  c.email,
  o.order_amount,
  o.order_status;
```

4. Click **Settings** → set **Pipeline mode: Continuous** → set **Target catalog: prod** → **Save**
5. Click **Start** (the editor switches to the live Graph view)

### Step 4: Govern with Unity Catalog

All tables created in Steps 1–3 are automatically registered in [Unity Catalog](https://learn.microsoft.com/en-us/azure/databricks/data-governance/unity-catalog/) with lineage.

#### View lineage

1. Go to **Catalog → prod → gold → customer_activity**
2. Click the **Lineage** tab → **See Lineage Graph**
3. Expand upstream nodes to follow Bronze sources → Silver → Gold

#### Set permissions

```sql
-- Grant analysts read access to the Gold layer only
GRANT SELECT ON TABLE prod.gold.customer_activity TO `analysts@contoso.com`;

-- Mask PII for non-privileged users
CREATE FUNCTION prod.security.mask_email(email STRING)
  RETURNS STRING
  RETURN CASE WHEN is_account_group_member('data-engineers') THEN email
              ELSE CONCAT(LEFT(email, 2), '***@***.com')
         END;

ALTER TABLE prod.silver.customers
  ALTER COLUMN email
  SET MASK prod.security.mask_email;
```

### Step 5: Orchestrate and monitor with Lakeflow Jobs

Create a job that runs the ingestion pipeline and then the transformation pipeline with dependencies, scheduling, and alerting.

#### Prerequisites

- Pipelines from Steps 2 and 3 saved in the workspace

#### Setup

1. Go to **Jobs & Pipelines → Create → Job**
2. Task 1: **Pipeline** → name `ingest_sql_server_cdc` → select your Lakeflow Connect pipeline → **Create task**
3. Task 2: **+ Add task → Pipeline** → name `transform_bronze_to_gold` → select `lakeflow-demo-pipeline` → set **Depends on** `ingest_sql_server_cdc` → **Create task**
4. In the Job details panel: **Add schedule** → set frequency → add email notification on failure → **Save**
5. Click **Run now**, then open the Run details from the run ID

For health monitoring across all jobs, query system tables:

```sql
SELECT
  job_name,
  result_state,
  DATEDIFF(second, start_time, end_time) AS duration_sec
FROM system.lakeflow.job_run_timeline
WHERE start_time >= CURRENT_TIMESTAMP - INTERVAL 24 HOURS
ORDER BY start_time DESC;
```

### Step 6: Visualize with AI/BI Dashboards and Genie

[AI/BI Dashboard](https://learn.microsoft.com/en-us/azure/databricks/ai-bi/) is presented as a low-code dashboarding option.

1. Click **+ New → Dashboard**
2. Add a visualization against `prod.gold.customer_activity`
3. Click **Publish** (viewers see data under their Unity Catalog permissions)

[Genie](https://learn.microsoft.com/en-us/azure/databricks/genie/) provides natural-language interaction with the dataset.

1. Click **Genie → New**
2. Choose data sources: `prod.gold.customer_activity` → **Create**
3. Add context in **Instructions** (relationships, business definitions)
4. Ask a question in chat, for example:

> "Which customers have the highest total events and what were their order amounts?"

5. Genie generates and executes SQL; you can click **View SQL** to inspect it.

## Everything in one platform (summary table)

| Capability | Lakeflow | Previously required |
| --- | --- | --- |
| Telemetry ingestion | Zerobus Ingest | Message bus + custom consumer |
| Database CDC | Lakeflow Connect | Custom scripts or 3rd-party tools |
| Transformation + AutoCDC | Spark Declarative Pipelines | Hand-rolled MERGE logic |
| Data quality | SDP Expectations | Separate validation tooling |
| Orchestration | Lakeflow Jobs | External schedulers (Airflow, etc.) |
| Governance | Unity Catalog | Disconnected ACLs and lineage |
| Monitoring | Job UI + System Tables | Separate APM tools |
| BI + NL query | AI/BI Dashboards + Genie | External BI tools |

## Reported customer results

- Ahold Delhaize — 4.5x faster deployment and 50% cost reduction running 1,000+ ingestion jobs daily
- Porsche Holding — 85% faster ingestion pipeline development vs. a custom-built solution

## Next steps

- [Lakeflow product page](https://www.databricks.com/product/data-engineering)
- [Lakeflow Connect documentation](https://docs.databricks.com/ingestion/lakeflow-connect/index.html)
- [Live demos on Demo Center](https://databricks.com/demos)
- [Get started with Azure Databricks](https://azure.microsoft.com/products/databricks)

Updated Mar 16, 2026

Version 1.0


[Read the entire article](https://techcommunity.microsoft.com/t5/azure-databricks/near-real-time-cdc-to-delta-lake-for-bi-and-ml-with-lakeflow-on/ba-p/4502750)

