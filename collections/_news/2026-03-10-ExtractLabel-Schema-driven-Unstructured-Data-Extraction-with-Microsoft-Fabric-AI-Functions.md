---
external_url: https://blog.fabric.microsoft.com/en-US/blog/extractlabel-schema-driven-unstructured-data-extraction-with-fabric-ai-functions/
title: 'ExtractLabel: Schema-driven Unstructured Data Extraction with Microsoft Fabric AI Functions'
author: Microsoft Fabric Blog
primary_section: ai
feed_name: Microsoft Fabric Blog
date: 2026-03-10 10:30:00 +00:00
tags:
- AI
- AI Functions
- Azure
- Data Engineering
- Data Enrichment
- Data Pipeline
- Data Validation
- ExtractLabel
- JSON Schema
- LLM
- Microsoft Fabric
- ML
- News
- Pandas
- Pydantic
- PySpark
- Schema Extraction
- Structured Data
- Text Analytics
- Unstructured Data
section_names:
- ai
- azure
- ml
---
Microsoft Fabric Blog details how ExtractLabel in Fabric AI Functions lets data engineers and scientists extract structured information from unstructured text using AI, JSON schema contracts, and scalable data platforms.<!--excerpt_end-->

# ExtractLabel: Schema-driven Unstructured Data Extraction with Microsoft Fabric AI Functions

**Author:** Microsoft Fabric Blog

Enterprise data frequently resides in unstructured formats—tickets, feedback, contracts, and more—making it challenging to extract usable insights. Traditional solutions involve brittle rule-based parsers or custom NLP models, but LLMs alone can be inconsistent without output enforcement. Microsoft Fabric AI Functions address these challenges by providing out-of-the-box functions like `ai.extract`, `ai.summarize`, and most notably, `ExtractLabel`, for reliable structured data extraction within the data platforms teams already use (pandas and PySpark).

## Why Structured Extraction is Challenging

- Free text contains critical info but lacks the structure pipelines expect.
- LLM outputs can be inconsistent, lacking type or schema guarantees.
- Reliable enterprise solutions demand type enforcement, constrained outputs, and dependable downstream integration.

## Microsoft Fabric AI Functions: Overview

- **ai.extract:** Basic pattern-based label extraction from free text.
    - Example: `df["text"].ai.extract("name", "profession", "city")` returns columns of extracted values.
    - Good for simple, unconstrained fields, but lacks structure for more complex workflows.
- **ExtractLabel:** Schema-driven extraction.
    - Lets you define strict schema (via JSON Schema or Pydantic) for types, constraints, required fields, and value enums.
    - Guarantees output structure, reduces post-processing, and supports rich, nested, or enumerated outputs for downstream systems.

## Practical Example: Warranty Claims

- Scenario: Ingesting unstructured warranty claims and producing structured outputs required by operations (e.g., product, issue, attempted troubleshooting, resolution).
- Solution: Define schema using ExtractLabel, such as:

```python
from synapse.ml.aifunc import ExtractLabel
claim_schema = ExtractLabel(
  label="claim",
  max_items=1,
  type="object",
  description="Extract structured warranty claim information",
  properties={
    "product_name": {"type": "string"},
    "problem_category": {"type": "string", "enum": ["defect", "damage_in_transit", "missing_part", "other"]},
    "problem_summary": {"type": "string"},
    "time_owned": {"type": "string"},
    "troubleshooting_tried": {"type": "array", "items": {"type": "string"}},
    "requested_resolution": {"type": "string"}
  },
  required=["product_name", "problem_category", "problem_summary", "time_owned", "troubleshooting_tried", "requested_resolution"],
  additionalProperties=False
)
```

- Usage: `df[["claim"]] = df["text"].ai.extract(claim_schema)`
- Output is reliably shaped—a dict containing fields exactly as specified, with proper typing and constraints (e.g., enums, arrays, nullable fields).

## JSON Schema and Pydantic Integration

- **JSON Schema:** Offers explicit contract for field names, types, allowed values, and requirements.
    - Descriptions help clarify choices for model (e.g., exemplars for ambiguous enums).
- **Pydantic:** Allows defining schema as Python classes with type hints and constraints, then converting to JSON Schema via `.model_json_schema()`.
    - Easier to maintain, validate, and reuse.

## Scaling Across Pandas & PySpark

- `synapse.ml.aifunc` for pandas; `synapse.ml.spark.aifunc` for PySpark.
- Fabric distributes extraction across clusters for large data volumes in PySpark—no code change required.

## Best Practices

- Always validate extraction outputs against labeled samples.
- Refine descriptions and schema to handle edge cases and maximize consistency.
- Consider required vs. optional fields and enumerate valid options for critical fields.

## Resources & Next Steps

- [Microsoft Fabric AI Functions documentation](https://learn.microsoft.com/fabric/data-science/ai-functions/overview?tabs=pandas-pyspark%2Cpandas)
- [ExtractLabel parameter reference](https://learn.microsoft.com/fabric/data-science/ai-functions/pandas/extract?tabs=labels#extractlabel-parameters)
- [Example Notebooks](https://github.com/microsoft/fabric-toolbox/blob/main/samples/notebook_aifunctions/aifunc_extractlabel_extraction.ipynb)

Microsoft Fabric AI Functions democratize the transformation of unstructured text into high-quality, schema-compliant, structured data, unlocking enterprise value for downstream analytics, operations, and machine learning.

This post appeared first on "Microsoft Fabric Blog". [Read the entire article here](https://blog.fabric.microsoft.com/en-US/blog/extractlabel-schema-driven-unstructured-data-extraction-with-fabric-ai-functions/)
