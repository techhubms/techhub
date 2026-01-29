---
external_url: https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/from-bronze-to-gold-data-quality-strategies-for-etl-in-microsoft/ba-p/4476303
title: 'From Bronze to Gold: Data Quality Strategies for ETL in Microsoft Fabric'
author: Sally_Dabbah
feed_name: Microsoft Tech Community
date: 2025-12-09 08:12:15 +00:00
tags:
- Automated Testing
- Bronze Layer
- Data Drift
- Data Quality
- Data Validation
- ETL
- Fabric Pipeline
- Gold Layer
- Great Expectations
- Lakehouse
- Medallion Architecture
- Microsoft Fabric
- NYC Taxi Dataset
- Observability
- Power BI
- Primary Key Uniqueness
- PySpark
- Schema Compliance
- Silver Layer
- Time Series Validation
- Azure
- Machine Learning
- Community
section_names:
- azure
- ml
primary_section: ml
---
Sally Dabbah provides a technical walkthrough on implementing enterprise-grade data quality checks in Microsoft Fabric ETL pipelines, showcasing Great Expectations integration at every architecture layer.<!--excerpt_end-->

# From Bronze to Gold: Data Quality Strategies for ETL in Microsoft Fabric

## Introduction

Trustworthy data underpins reliable analytics, machine learning, and AI. Inconsistent schemas, nulls, data drift, and upstream changes often break business logic and dashboards. Microsoft Fabric, combined with Great Expectations (GX), an open-source data validation framework, enables engineers to build scalable, reusable, and automated data quality solutions.

This guide shows how to use Fabric notebooks and pipelines together with Great Expectations for comprehensive data quality checks—including row-count drift, schema validation, primary key uniqueness, and batch-based time-series anomalies—especially focusing on the Bronze layer of the Medallion Architecture with the NYC Taxi dataset.

## Why Data Quality Matters

Many analytics projects stumble because of poor data, not model shortcomings. In Fabric, organizations seek:

- Reliable validation as data lands in Bronze
- Early detection of schema changes before breaking pipelines
- Prevention of silent failures and drift
- Standardized checks across diverse tables and workflows

Great Expectations answers these needs with declarative, automation-ready rule suites.

## The Medallion Architecture and Data Quality

Microsoft Fabric implements Medallion Architecture, moving data through Bronze, Silver, and Gold layers; each step benefits from distinct validation:

### Bronze Layer

- Ingest raw data without transformations
- Run foundational checks (row count, schema, required columns, key uniqueness)

### Silver Layer

- Clean and standardize data
- Enforce business logic and domain/reference values
- Detect anomalies, outliers, duplicate records

### Gold Layer

- Curate analytics-ready datasets
- Validate aggregates, KPIs, and cross-system consistency
- Monitor feature/data drift for ML readiness

### Recommended Checks per Layer

**Bronze:**

- Row-count and volume drift detection
- Schema and datatype compliance
- Null/empty column checks
- Unique primary key validation

**Silver:**

- Reference/domain value checks
- Business rule enforcement
- Anomaly detection
- Post-standardization deduplication

**Gold:**

- Aggregation and metric validation
- KPI monitoring
- Feature drift analysis (for ML)
- Cross-system metric alignment

## Practical Implementation: Great Expectations in Fabric Notebooks

### Step 1: Read Data from Lakehouse

```python
lakehouse_name = "Bronze"
table_name = "NYC Taxi - Green"
query = f"SELECT * FROM {lakehouse_name}.`{table_name}`"
df = spark.sql(query)
```

### Step 2: Create and Register Suite

```python
context = gx.get_context()
suite = context.suites.add(
    gx.ExpectationSuite(name="nyc_bronze_suite")
)
```

### Step 3: Add Bronze Expectations (Reusable Function)

```python
import great_expectations as gx

def add_bronze_expectations(
    suite: gx.ExpectationSuite,
    primary_key_columns: list[str],
    required_columns: list[str],
    expected_schema: list[str],
    expected_row_count: int | None = None,
    max_row_drift_pct: float = 0.2,
) -> gx.ExpectationSuite:
    # Row Count & Drift
    if expected_row_count is not None:
        min_rows = int(expected_row_count * (1 - max_row_drift_pct))
        max_rows = int(expected_row_count * (1 + max_row_drift_pct))
        row_count_expectation = gx.expectations.ExpectTableRowCountToBeBetween(
            min_value=min_rows, max_value=max_rows)
        suite.add_expectation(expectation=row_count_expectation)
    # Schema Compliance
    schema_expectation = gx.expectations.ExpectTableColumnsToMatchSet(
        column_set=expected_schema, exact_match=True)
    suite.add_expectation(expectation=schema_expectation)
    # Required Columns
    for col in required_columns:
        not_null_expectation = gx.expectations.ExpectColumnValuesToNotBeNull(column=col)
        suite.add_expectation(expectation=not_null_expectation)
    # Primary Key Uniqueness
    if primary_key_columns:
        unique_pk_expectation = gx.expectations.ExpectCompoundColumnsToBeUnique(column_list=primary_key_columns)
        suite.add_expectation(expectation=unique_pk_expectation)
    return suite
```

### Step 4: Attach Data Asset & Batch Definition

```python
data_source = context.data_sources.add_spark(name="bronze_datasource")
data_asset = data_source.add_dataframe_asset(name="nyc_bronze_data")
batch_definition = data_asset.add_batch_definition_whole_dataframe("full_bronze_batch")
```

### Step 5: Run Validation

```python
validation_definition = gx.ValidationDefinition(
    data=batch_definition,
    suite=suite,
    name="Bronze_DQ_Validation"
)
results = validation_definition.run(batch_parameters={"dataframe": df})
print(results)
```

#### Optional: Daily Time-Series Validation

Fabric currently requires custom logic for daily batch validation:

```python
dates_df = df.select(F.to_date("lpepPickupDatetime").alias("dt")).distinct()
for d in dates:
    df_day = df.filter(F.to_date("lpepPickupDatetime") == d)
    results = validation_definition.run(batch_parameters={"dataframe": df_day})
```

Allows for per-day anomaly detection, completeness checks, and schema drift early warning.

## Automating Data Quality with Fabric Pipelines

- Trigger notebooks post-ingestion
- Pass pipeline parameters (table, layer)
- Persist results to Lakehouse or Log Analytics
- Configure alerts with Fabric Monitor
- Automate incident raising and engineer notifications on failure

**Workflow:**

1. Run notebook
2. Check results
3. If failures: raise incident, fail pipeline, notify on-call

## Enterprise Benefits

Standardized, reusable DQ rules across domains create consistent practices and improved observability. Teams can detect and resolve failures early, ensuring downstream analytics and Power BI operate on reliable data. High-quality data directly boosts outcomes in AI, analytics, and reporting.

## Conclusion

Great Expectations and Microsoft Fabric together allow organizations to modularize and scale data quality enforcement throughout the Medallion Architecture. With unified compute and monitoring, DQ becomes an integrated, automated workflow, not an afterthought.

**Further links:**

- [Implement Medallion Lakehouse Architecture in Fabric – Microsoft Learn](https://learn.microsoft.com/en-us/fabric/onelake/onelake-medallion-lakehouse-architecture)
- [GX Expectations Gallery](https://greatexpectations.io/expectations/)
- [Github Example: Data Quality Checks in Microsoft Fabric](https://github.com/sallydabbahmsft/Data-Quality-Checks-in-Microsoft-Fabric)

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/analytics-on-azure-blog/from-bronze-to-gold-data-quality-strategies-for-etl-in-microsoft/ba-p/4476303)
