---
section_names:
- ai
- ml
feed_name: Microsoft Fabric Blog
primary_section: ai
date: 2026-03-19 15:00:00 +00:00
author: Microsoft Fabric Blog
external_url: https://blog.fabric.microsoft.com/en-US/blog/unlock-insights-from-images-and-pdfs-with-multimodal-support-in-fabric-ai-functions-preview/
tags:
- AI
- Capacity Units (cu)
- Cost Calculator
- Dataflows Gen2
- Document Extraction
- Eval Notebooks
- F1 Score
- Fabric AI Functions
- Fabric Capacity Metrics App
- Image Classification
- LLM as A Judge
- Microsoft Fabric
- ML
- Multimodal AI
- News
- PDF Processing
- Precision And Recall
- Preview
- Prompting
- Schema Inference
- Sentiment Analysis
- Spark Notebooks
- Token Estimation
title: 'Multimodal support in Fabric AI functions (Preview): process images and PDFs, plus cost and eval updates'
---

Microsoft Fabric Blog announces multimodal support (Preview) for Fabric AI functions, enabling images and PDFs to be processed directly from notebooks and dataflows. The post also covers new cost transparency features (token/CU estimation and capacity metrics), plus GitHub-hosted eval and starter notebooks to help validate and adopt AI functions faster.<!--excerpt_end-->

# Multimodal support in Fabric AI functions (Preview)

Fabric AI functions are adding **multimodal support (Preview)** so you can run AI functions directly over **images, PDFs, and text files** (not just text). The update also introduces **cost visibility improvements**, plus **evaluation notebooks** and **starter notebooks** to help teams adopt AI functions and validate output quality.

![GIF of a Fabric notebook using multimodal AI functions to load files into a table, classify and analyze sentiment from photos, and summarize and extract details from PDFs.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/gif-of-a-fabric-notebook-using-multimodal-ai-funct.gif)

## What’s new

- **Multimodal AI functions (Preview)** — process **images, PDFs, and text files** directly.
- **Improved cost transparency** — estimate consumption with a **cost calculator** and track usage with a dedicated **operation name** in the **Fabric Capacity Metrics** app.
- **Eval notebooks** — structured notebooks to evaluate AI function output quality.
- **Starter notebooks** — ready-made notebooks to get started end-to-end.

## Multimodal AI functions (Preview)

Previously, Fabric AI functions worked only on **text input**. With multimodal support, you can point functions at **files** and extract structured outputs.

### Supported file types

- **Images:** JPG, JPEG, PNG, static GIF, WebP
- **Documents:** PDF
- **Text files:** MD, TXT, CSV, TSV, JSON, XML, PY, and other text files

### New multimodal functions

Multimodal support introduces new functions and extends existing AI functions to accept file inputs.

- **`aifunc.load()`**
  - Reads all files in a folder and generates a structured table.
  - Optional parameters:
    - `prompt`: guide what to extract
    - `schema`: provide a schema for more reproducible results

- **`aifunc.list_file_paths(folder_path)`**
  - Fetches all valid files from a folder (or glob pattern).
  - Returns a series of URLs you can feed into AI functions.

- **`ai.infer_schema()`**
  - Infers a common schema across your files (optionally guided by a prompt).
  - Returns a list of **ExtractLabel** objects usable directly with `ai.extract()`.

### Using existing AI functions on files (examples)

The post shows that many existing AI functions now work with multimodal inputs, including:

- **Classify** insurance claim photos by category

![Code snippet showing an AI classify function that labels damage severity from image file paths.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/code-snippet-showing-an-ai-classify-function-that.png)

- **Summarize** PDF earnings reports

![Python code using ai.summarize to generate summaries from files, focusing on revenue, profit margins, and year‑over‑year growth.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/python-code-using-ai-summarize-to-generate-summari.png)

- **Extract** invoice date, vendor name, and total amount from invoices

![Python code using ai.extract to extract invoice date, vendor name, and total amount from files.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/python-code-using-ai-extract-to-extract-invoice-da.png)

- **Analyze sentiment** of product review photos

![Code snippet showing an AI sentiment analysis function applied to product review files.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/code-snippet-showing-an-ai-sentiment-analysis-func.png)

- **Use a custom prompt** to flag risky clauses in contracts

![Code snippet showing an AI generate response function identifying red flag clauses in contracts.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/code-snippet-showing-an-ai-generate-response-funct.png)

**Note:** Passing `column_type="path"` indicates a column contains **file paths** rather than plain text.

## Improved cost transparency

The update adds mechanisms to better understand and track consumption.

### Progress bar with cost calculator

AI functions now include a configurable progress bar with real-time job progress and estimated token usage. Configure using `use_progress_bar`:

| Mode | What it shows |
| --- | --- |
| `basic` (default) | Rows processed and percent completion |
| `stats` | Everything in `basic`, plus estimated total input tokens, predicted output tokens, and a running **total CU prediction** updated in real time (with final CU count at completion) |
| `disable` | Turns the progress bar off |

In **`stats`** mode, you can see a running **Capacity Units (CU)** prediction while the job is running.

Documentation: [custom configurations article](https://learn.microsoft.com/fabric/data-science/ai-functions/pandas/configuration)

![Screenshot showing an example of the progress bar with the "basic", "stats", and "disable" modes.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/03/screenshot-showing-an-example-of-the-progress-bar.png)

### Dedicated operation name in Fabric Capacity Metrics

AI functions now show up as a dedicated **AI Functions** operation in the **Fabric Capacity Metrics App**. Previously, usage could appear under **Spark** or **Dataflows Gen2**, making it harder to isolate AI function consumption.

Related post: [Billing updates: new operations for Fabric AI Functions and AI Services](https://blog.fabric.microsoft.com/blog/billing-updates-new-operations-for-fabric-ai-functions-and-ai-services/)

## Eval notebooks

The post introduces **evaluation notebooks** (hosted in the Fabric samples GitHub repo) to help validate AI function output quality using **LLM-as-a-Judge**.

Link: [evaluation notebooks](https://aka.ms/fabric-aifunctions-eval-notebooks)

### Workflow described

1. **Run** an AI function using an executor model (example: `gpt-4.1-mini`).
2. **Evaluate** output using a judge model (example: `gpt-5 with reasoning`) to produce ground truth labels or quality scores.
3. **Calculate** metrics such as accuracy, precision, recall, F1 score, coherence, consistency, relevance, and more.
4. **Identify** outputs needing review and visualize results.
5. **Refine** labels/prompts/model configuration and re-evaluate.

The notebooks are intended as templates you can adapt to your data, prompts, and quality criteria.

## Starter notebooks

Starter notebooks provide an end-to-end walkthrough for AI functions: setup, configuration, and running AI functions on your data.

Link: [starter notebooks](https://aka.ms/fabric-aifunctions-starter-notebooks)

## Next steps (from the post)

- Try multimodal AI functions on images, PDFs, and text files.
- Read docs: [AI functions](https://aka.ms/ai-functions) and [multimodal capabilities](https://aka.ms/fabric-aifunctions-multimodal).
- Download: [starter notebooks](https://aka.ms/fabric-aifunctions-starter-notebooks) and [eval notebooks](https://aka.ms/fabric-aifunctions-eval-notebooks).
- Share feedback: [Fabric Ideas](https://aka.ms/FabricBlog/ideas) and the [Fabric Community](https://aka.ms/FabricBlog/Community).


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/unlock-insights-from-images-and-pdfs-with-multimodal-support-in-fabric-ai-functions-preview/)

