---
external_url: https://blog.fabric.microsoft.com/en-US/blog/enrich-power-bi-reports-with-machine-learning-in-microsoft-fabric/
title: Integrating Machine Learning with Power BI Reports in Microsoft Fabric
author: Microsoft Fabric Blog
primary_section: ai
feed_name: Microsoft Fabric Blog
date: 2026-02-12 12:00:00 +00:00
tags:
- AI
- Azure
- Banking Analytics
- Churn Prediction
- Classification
- Dataflow Gen2
- Delta Lake
- Feature Engineering
- LightGBM
- Microsoft Fabric
- ML
- ML Model Deployment
- MLflow
- News
- OneLake
- Pandas
- Power BI
- Prediction API
- PySpark
- Python
- Real Time Scoring
- Semantic Link
- SMOTE
- Spark SQL
section_names:
- ai
- azure
- ml
---
Microsoft Fabric Blog demonstrates how to infuse Power BI reports with predictive insights by building and operationalizing machine learning models within Microsoft Fabric’s unified analytics stack.<!--excerpt_end-->

# Integrating Machine Learning with Power BI Reports in Microsoft Fabric

**Author:** Microsoft Fabric Blog

## Overview

Many organizations now aim to move beyond descriptive reporting in Power BI and leverage machine learning to predict future outcomes, identify at-risk accounts, and prioritize actions for maximum business impact. While this typically requires complex data movement and logic duplication, Microsoft Fabric streamlines the process by enabling unified data science, engineering, and business intelligence workflows on its platform.

This article details an end-to-end pattern to enrich a Power BI report with machine learning in Fabric, specifically through a bank customer churn prediction scenario. The steps include exploring the semantic model, data preparation, feature engineering, model training and validation, batch and real-time scoring, and surfacing the results directly in Power BI.

---

## Contents

- Scenario: Predicting Bank Customer Churn
- Architecture Overview
- Step 1: Explore Semantic Model with Semantic Link
- Step 2: Feature Engineering and Data Preparation
- Step 3: Model Training with LightGBM and SMOTE
- Step 4: Batch and Real-Time Scoring in Fabric
- Step 5: Data Enrichment During Ingestion (Dataflow Gen2)
- Step 6: Visualizing Predictions in Power BI
- Closing Thoughts

---

## Scenario: Predicting Bank Customer Churn

The case study examines a financial institution using Power BI to analyze customer data. Historically, about 20% of customers churn. Instead of simply reporting churn after it happens, the organization seeks to proactively identify customers who are likely to churn, allowing for timely intervention.

---

## Architecture Overview

The machine learning enrichment workflow in Fabric includes:

1. **Semantic model**: Centralized business logic and metrics.
2. **Semantic Link**: Direct access to the semantic model from notebooks.
3. **Fabric ML experiment**: Model training and evaluation.
4. **Batch scoring**: Large-scale batch inference, saving results to OneLake.
5. **Real-time scoring endpoints**: Low-latency inference capabilities.
6. **Dataflow Gen2**: Ingestion-time enrichment with ML predictions.
7. **Power BI Report**: Actionable dashboards using the enriched data.

All process components natively integrate within Fabric and share governance, storage, and security.

---

## Step 1: Explore Semantic Model Using Semantic Link

- Business logic is centralized in the Power BI semantic model.
- **Semantic Link** enables direct querying of the model from Fabric notebooks—no data export or duplication needed.
- Example Python code demonstrates exploring datasets, tables, columns, and measures:

```python
import sempy.fabric as fabric
df_datasets = fabric.list_datasets()
df_tables = fabric.list_tables("Bank Customer Churn Analysis")
df_columns = fabric.list_columns("Bank Customer Churn Analysis")
df_measures = fabric.list_measures("Bank Customer Churn Analysis")
df_raw = fabric.read_table(dataset="Bank Customer Churn Analysis", table="churn")
```

- Data can be directly analyzed and prepared using pandas, seaborn, and other familiar Python libraries, preserving business definitions.

---

## Step 2: Feature Engineering and Data Preparation

- Cleaning data by dropping duplicates and columns not needed for modeling.
- Exploratory analysis includes:
  - Statistical summaries
  - Box plots and histograms for numerical features
  - Count plots for categorical features
- Feature engineering:
  - Calculating new tenure, credit score, and salary-based features using `pd.qcut` bins
  - One-hot encoding categorical variables (e.g., Geography, Gender)
- Data is saved in Delta tables (via Spark) for scalable processing.

---

## Step 3: Model Training with LightGBM and SMOTE

- **LightGBM classifier** is chosen for its efficiency with tabular data.
- **SMOTE** is used to balance the imbalanced dataset, synthesizing samples of the minority class (churned customers).
- Data is split into training, validation, and test sets; test data is saved as a Delta table.
- Model training and experiment tracking use MLflow:

```python
from lightgbm import LGBMClassifier
from imblearn.over_sampling import SMOTE
sm = SMOTE(random_state=1234)
X_res, y_res = sm.fit_resample(X_train, y_train)
lgbm_model = LGBMClassifier(...)
lgbm_model.fit(X_res, y_res)
```

- Model evaluation includes accuracy, F1, ROC AUC, confusion matrix, and classification report, all tracked in MLflow. The trained model is registered in the Fabric Model Registry.

---

## Step 4: Batch and Real-Time Scoring in Fabric

- Batch scoring applies the registered model at Spark scale to active customers; predictions are written to OneLake for downstream consumption.
- Real-time scoring endpoints (activatable from the Fabric UI) allow on-demand, low-latency inference for time-sensitive actions (e.g., triggering outreach when risk status updates).
- Example Spark code and MLFlowTransformer/PySpark UDF usage are shown for flexible prediction workflows.

---

## Step 5: Data Enrichment During Ingestion (Dataflow Gen2)

- Real-time scoring can be embedded directly within Dataflow Gen2 pipelines so that new or updated records are scored immediately upon arrival.
- An example M script is provided for calling the ML REST endpoint, handling authentication with a service principal and enriching the data with predictions.
- The entire prediction and enrichment process is governed and managed within Fabric, ensuring data lineage and consistent logic.

---

## Step 6: Visualizing Predictions in Power BI

- Predictions stored in OneLake are integrated back into the semantic model, joined with customer dimensions, and visualized in Power BI reports.
- Teams can build dashboards that filter, segment, and act on high-risk customers based on the machine learning output.

---

## Closing Thoughts

The recommended pattern demonstrates operationalizing machine learning inside Fabric for predictive analytics. Machine learning becomes a core part of the analytics experience, enabling data-driven, proactive engagement without additional operational overhead or fragmented logic. The approach is extensible to other scenarios such as anomaly detection, forecasting, and propensity modeling.

**Resources:**

- [Sample Repository](https://aka.ms/enrich-pbi-with-ML)
- [Fabric Data Science Documentation](https://aka.ms/fabric-ds-docs)
- [Book Time with the Author](https://outlook.office.com/bookwithme/user/45cc8a043ef64c5a8847e619c77d811d@microsoft.com/meetingtype/hcyCyWWlyEyidZZw1oLfZg2?anonymous&ep=mlink)

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/enrich-power-bi-reports-with-machine-learning-in-microsoft-fabric/)
