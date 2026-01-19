---
layout: post
title: 'Enhancing Fabric Notebooks: Native Pandas DataFrame Support in User Data Functions'
author: Microsoft Fabric Blog
canonical_url: https://blog.fabric.microsoft.com/en-US/blog/now-in-fabric-notebook-udf-integration-with-native-support-for-pandas-dataframes-and-series-via-apache-arrow/
viewing_mode: external
feed_name: Microsoft Fabric Blog
feed_url: https://blog.fabric.microsoft.com/en-us/blog/feed/
date: 2025-08-26 11:45:35 +00:00
permalink: /ml/news/Enhancing-Fabric-Notebooks-Native-Pandas-DataFrame-Support-in-User-Data-Functions
tags:
- Apache Arrow
- Cross Language Support
- Data Analytics
- Data Engineering
- Data Processing
- DataFrames
- Microsoft Fabric
- Notebook
- Pandas
- Performance Optimization
- PySpark
- Python
- Python Notebooks
- Scala
- UDF
- User Data Functions
section_names:
- azure
- ml
---
Microsoft Fabric Blog introduces a major update by enabling native Pandas DataFrame and Series support in User Data Functions within Fabric Notebooks. This enhancement, powered by Apache Arrow, streamlines large-scale data analysis and enables high-efficiency workflows for data engineers and scientists.<!--excerpt_end-->

# Enhancing Fabric Notebooks: Native Pandas DataFrame Support in User Data Functions

Microsoft Fabric has introduced a significant enhancement to its Notebook integration: User Data Functions (UDFs) now natively support Pandas DataFrames and Series both as input and output types, thanks to deep integration with Apache Arrow.

## Key Highlights

- **Pandas Integration**: UDFs in Fabric Notebooks now accept and return Pandas DataFrames and Series, greatly simplifying analytics workflows.
- **Powered by Apache Arrow**: Adoption of Arrow's efficient columnar memory format enables high-performance serialization, zero-copy data sharing, and improved scalability for large datasets.
- **Cross-language Compatibility**: The update allows seamless use of UDFs in Python, PySpark, Scala, and R.

## Benefits

- **Performance**: Significant improvements in data transfer efficiency and reduced memory overhead.
- **Developer Experience**: No need to manually serialize large datasets to JSON; DataFrames can be passed directly for computation.
- **Scalability**: Effortlessly handle millions of rows in real-time analytics or feature engineering tasks.
- **Code Reuse and Collaboration**: Teams can modularize business logic and share tested functions, promoting consistent implementation across projects.

## Sample Usage

### PySpark / Python

```python
# Retrieve the function

agg_func = notebookutils.udf.getFunctions("AggregateRevenueByDriver")

# Example Pandas DataFrame input

import pandas as pd
df = pd.DataFrame({"driver_id": [1, 2, 1], "revenue": [100.0, 150.0, 200.0]})

# Call the UDF

esult_df = agg_func.aggregate(df)
print(result_df)
```

### Scala

```scala
val aggFunc = notebookutils.udf.getFunctions("AggregateRevenueByDriver")
val input = Seq((1, 100.0), (2, 150.0), (1, 200.0)).toDF("driver_id", "revenue")
val result = aggFunc.aggregate(input)
result.show()
```

### R

```r
agg_func <- notebookutils.udf.getFunctions("AggregateRevenueByDriver")
df <- data.frame(driver_id = c(1, 2, 1), revenue = c(100.0, 150.0, 200.0))
result <- agg_func$aggregate(df)
print(result)
```

## Impact

This update empowers data professionals to:

- Accelerate interactive analytics on massive datasets
- Standardize and reuse robust function logic across projects
- Speed up workflows for real-time metrics, aggregation, and feature engineering

## Getting Started

Register a Pandas-compatible UDF in your Fabric Notebook and start leveraging these performance benefits today. See [NotebookUtils for Fabric documentation](https://learn.microsoft.com/fabric/data-engineering/notebook-utilities#user-data-function-udf-utilities) for setup instructions.

## Further Reading

- [User Data Functions now support async functions and pandas DataFrame, Series types](https://blog.fabric.microsoft.com/blog/user-data-functions-now-support-async-functions-and-pandas-dataframe-series-types)

---
*Article originally published by Microsoft Fabric Blog.*

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/now-in-fabric-notebook-udf-integration-with-native-support-for-pandas-dataframes-and-series-via-apache-arrow/)
