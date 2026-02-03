---
external_url: https://blog.fabric.microsoft.com/en-US/blog/bringing-together-fabric-real-time-intelligence-notebook-and-spark-structured-streaming-preview/
title: Integrating Real-Time Eventstreams with Spark Notebooks in Microsoft Fabric
author: Microsoft Fabric Blog
primary_section: ai
feed_name: Microsoft Fabric Blog
date: 2026-02-02 14:00:00 +00:00
tags:
- AI
- Apache Kafka
- Automatic Code Generation
- Azure
- Data Engineering
- Data Pipelines
- Entra ID
- Eventstreams
- Microsoft Fabric
- ML
- News
- Notebook Integration
- PySpark
- Real Time Intelligence
- Real Time Processing
- Spark Notebooks
- Spark Structured Streaming
- Streaming Analytics
- Streaming Data
- Machine Learning
section_names:
- ai
- azure
- ml
---
Microsoft Fabric Blog, coauthored by QiXiao Wang, presents the Preview integration of Fabric Eventstreams and Spark Notebooks. This advancement enables developers to build robust, real-time analytics and AI pipelines in Microsoft Fabric with less manual coding.<!--excerpt_end-->

# Integrating Real-Time Eventstreams with Spark Notebooks in Microsoft Fabric

**By Microsoft Fabric Blog, coauthored by QiXiao Wang**

Microsoft Fabric now offers a preview integration that unifies the power of real-time stream processing via Fabric Eventstreams with the open-source capabilities of Spark Structured Streaming, all accessible from within Spark Notebooks. This new feature is designed to make it significantly simpler for developers and data engineers to create end-to-end, event-driven analytic and AI solutions.

## Key Features and Capabilities

- **Seamless Eventstream Access:**
  - Instantly discover and connect to real-time Eventstreams using the Real-Time Hub inside Spark Notebooks.
  - Quickly set up new Eventstreams, with support for ~30 streaming sources including CDC-enabled databases, brokers, and feeds.

- **Rapid Pipeline Creation with Code Generation:**
  - Kick-start streaming workflows in minutes via auto-generated PySpark code snippets.
  - Minimal configuration required; generated code handles connecting and reading Eventstreams as well as outputting to the console.

- **Notebook Reuse and Collaboration:**
  - Teams can import existing Spark Notebooks into Eventstreams, turning prototypes into production streaming processors with limited refactoring.
  - Notebooks can now be assigned as operational processors within Eventstreams.

- **Secure Connectivity:**
  - Connection and authentication is managed through an enhanced Kafka-based adapter, using Entra ID and secure token validation.
  - No manual handling of secrets or connection strings in code.

## Example Usage Scenarios

- **Fraud Detection:**
  - Use notebooks to analyze financial transactions in real-time and build advanced anomaly detection pipelines.
- **Predictive Maintenance:**
  - Enrich IoT sensor streams with historical data using Eventstreams and Spark Structured Streaming for equipment monitoring.
- **ML-Driven Analytics:**
  - Extend existing notebooks with advanced machine learning models to derive insights from live data.

## Code Example: Auto-Generated PySpark Snippet

```python
from pyspark.sql import SparkSession
from pyspark.sql.functions import col
from pyspark.sql.types import StringType
from pyspark.sql.dataframe import DataFrame

eventstream_options = {
    "eventstream.itemid": '<ENTER ITEMID FOR YOUR EVENTSTREAM>',
    "eventstream.datasourceid": '<ENTER DATASOURCEID FOR THE NOTEBOOK DESTINATION>'
}

# Read from Kafka using the config map

df_raw = spark.readStream.format("kafka").options(**eventstream_options).load()

decoded_df = df_raw.select(
    col("key").cast(StringType()).alias("key"),
    col("value").cast(StringType()).alias("value"),
    col("partition"),
    col("offset")
)

def showDf(x: DataFrame, y: int):
    x.show()

# Print messages to the console

query = decoded_df.writeStream.foreachBatch(showDf).outputMode("append").start()
query.awaitTermination()
```

## Advantages

- **End-to-End Simplicity:** Fast, code-light setup of real-time pipelines.
- **Enterprise Security:** Secure connections use existing user identity; secrets are not embedded in code.
- **Extensibility:** Reuse of prior notebook investments accelerates analytics and ML initiatives.

## Resources

- [Microsoft Fabric Eventstreams Overview](https://learn.microsoft.com/fabric/real-time-intelligence/event-streams/overview?tabs=enhancedcapabilities)
- [How to use notebooks – Microsoft Fabric](https://learn.microsoft.com/fabric/data-engineering/how-to-use-notebook)
- [Real-Time Intelligence in Microsoft Fabric documentation](https://learn.microsoft.com/fabric/real-time-intelligence/)

## Feedback

The Fabric team welcomes ideas, feedback, and use cases as developers explore the new Eventstream and Notebook integration in preview.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/bringing-together-fabric-real-time-intelligence-notebook-and-spark-structured-streaming-preview/)
