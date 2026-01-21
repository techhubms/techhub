---
external_url: https://devblogs.microsoft.com/azure-sdk/easily-connect-ai-workloads-to-azure-blob-storage-with-adlfs/
title: Easily Connect AI Workloads to Azure Blob Storage with adlfs
author: Vishnu Charan TJ
feed_name: Microsoft Azure SDK Blog
date: 2025-10-15 17:17:17 +00:00
tags:
- Adlfs
- AI Workloads
- Azure Blob Storage
- Azure Data Lake Storage
- Azure SDK
- Blob Storage
- Cloud Storage
- Concurrent Uploads
- Dask
- Data Engineering
- Data Lake
- Data Pipelines
- ETL
- Fsspec
- Pandas
- Performance Optimization
- Python
- PyTorch
- Ray
- Storage
- Storage Authentication
section_names:
- ai
- azure
- ml
---
Vishnu Charan TJ explains the latest enhancements in adlfs, empowering data professionals to efficiently connect Python-based AI and ML workloads to Azure Blob and Data Lake Storage, with real-world framework integrations and best practices.<!--excerpt_end-->

# Easily Connect AI Workloads to Azure Blob Storage with adlfs

**Author:** Vishnu Charan TJ

adlf's newest release bridges the gap between Python AI/ML workflows and Azure's cloud storage, focusing on high performance and robust authentication. Microsoft continues its collaboration with the open-source fsspec community, ensuring adlfs remains optimized for modern data engineering needs.

## Who Is This For?

This update is designed for data professionals and developers building with Python who need seamless Azure storage integration—particularly those leveraging frameworks like Dask, Pandas, Ray, PyTorch, and PyIceberg. adlfs stands out for Azure-centric ML, data science, and ETL workloads, allowing easy dataset loading and model checkpoint storage using familiar file path conventions such as `az://`.

## What's New in adlfs 2025.8.0?

- **2–5× Faster Large File Writes**: Thanks to parallel (concurrent) block uploads
- **Improved Reliability**: Default block size reduced from 1 GiB to 50 MiB, reducing timeouts and supporting geo-redundant storage scenarios
- **Broader Framework Support**: Native support ensures nearly any fsspec-compatible library instantly benefits

## Installation

adlf is available on PyPI. To install the latest version:

```shell
pip install adlfs==2025.8.0
```

## Example Usage in Ray

Here's how you can connect Ray data pipelines to Azure Blob Storage using adlfs:

```python
import ray
from adlfs import AzureBlobFileSystem

ray.init()

# Configure authentication - set anon=False to use credentials

abfs = AzureBlobFileSystem(account_name="myaccount", anon=False)
ds = ray.data.read_parquet("az://mycontainer/data/", filesystem=abfs)
print(ds.count())
ray.shutdown()
```

This integration enables distributed data processing across your Ray cluster with support for standard Azure authentication via CLI, environment variables, managed identity, or explicit credentials.

## Why Use adlfs for AI/ML and ETL?

- **Native integration** with Python frameworks (Dask, Pandas, PyTorch, Ray, PyIceberg)
- **Performance improvements** benefit all compatible tools without requiring code changes
- **Easy transition** from local or other cloud file adapters—just modify the storage path or settings
- **Efficient large file operations** for cloud storage tasks in training or data pipelines
- **Flexible authentication**, accommodating various Azure identities and workflows

## Looking Ahead

Microsoft actively supports adlfs development and encourages data engineers and AI practitioners to upgrade. Improvements in this release are automatically applied to applications using compatible frameworks. Feedback and feature requests are welcome via the [adlfs GitHub repository](https://github.com/fsspec/adlfs).

## Summary

The 2025.8.0 release of adlfs delivers significant performance and reliability gains for Azure storage integration in Python AI/ML scenarios. Data professionals working with modern frameworks can expect easier, faster, and more robust data access for their cloud-based workloads.

This post appeared first on "Microsoft Azure SDK Blog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/easily-connect-ai-workloads-to-azure-blob-storage-with-adlfs/)
