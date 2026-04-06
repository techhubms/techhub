---
title: Working with unstructured text in Fabric Data Warehouse with built-in AI functions (Preview)
author: Microsoft Fabric Blog
feed_name: Microsoft Fabric Blog
section_names:
- ai
- ml
tags:
- AI
- AI Functions
- Fabric Data Warehouse
- JSON Output
- Lakehouse
- Microsoft Fabric
- ML
- News
- OPENROWSET
- Preview Features
- Prompt Engineering
- Sentiment Analysis
- T SQL
- Text Classification
- Text Extraction
- Text Summarization
- Translation
- Unstructured Text
external_url: https://blog.fabric.microsoft.com/en-US/blog/working-with-unstructured-text-in-fabric-data-warehouse-with-built-in-ai-functions-preview/
primary_section: ai
date: 2026-04-06 12:00:00 +00:00
---

Microsoft Fabric Blog introduces (preview) built-in AI functions in Microsoft Fabric Data Warehouse to extract, classify, analyze sentiment, and transform unstructured text directly in T-SQL, including a prompt-based function for custom processing.<!--excerpt_end-->

# Working with unstructured text in Fabric Data Warehouse with built-in AI functions (Preview)

Data warehouses traditionally focus on structured and semi-structured data. Free-form text (notes, logs, comments) has typically been harder to process inside a warehouse without adding external components.

Microsoft Fabric Data Warehouse introduces **built-in AI functions (preview)** that let you process unstructured text directly in **T-SQL**, including:

- Extraction of structured information from text
- Sentiment analysis
- Text classification
- Text transformation (summarize, translate, grammar fixing)
- Prompt-based custom processing

## Extract information from text

The **`ai_extract(text, topics…)`** function identifies and extracts information from unstructured text using contextual understanding.

- It returns extracted results as **JSON properties**.
- It’s designed to reduce reliance on complex rule engines or regular expressions.

![SQL query selecting patient's medial notes and extracting symptoms, diagnosis, and treatment data using AI function. The extracted data is returned as JSON-formatted text.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/sql-query-selecting-patients-medial-notes-and-ext.png)

![SQL query selecting patient's medial notes and extracting symptoms, diagnosis, and treatment data using AI function. The extracted data is returned as JSON-formatted text.](//dataplatformblogwebfd-d3h9cbawf0h8ecgf.b01.azurefd.net/wp-content/uploads/2026/04/sql-query-selecting-patients-medial-notes-and-ext-1.png)

*Figures 1-2. Extracting details from medical notes.*

## Analyze sentiment of text

The **`ai_analyze_sentiment(text)`** function determines sentiment for a text column.

Example:

```sql
SELECT CommentId, CommentText,

ai_analyze_sentiment(CommentText) AS Sentiment

FROM Feedback;
```

The function categorizes sentiment as:

- positive
- negative
- neutral
- mixed

## Classify text with ai_classify

The **`ai_classify(text, classes…)`** function assigns a category label to text based on contextual similarity to the provided classes.

Example scenario: classify log entries read from a lakehouse via `OPENROWSET()`, then insert into a table.

```sql
INSERT INTO silver.Logs (LogTime, LogMessage, Category)

SELECT LogTime, LogMessage,

aio_classify(LogMessage, ‘UI’,’wrong result’,’performance’,’timeout’) AS Category

FROM OPENROWSET(‘/Files/logs/*.jsonl’, DATA_SOURCE=’AppLogLakehouse’);
```

Notes:

- `ai_classify()` returns the single class most like the input text.
- The returned value can be ingested into a dedicated classification column (e.g., `Category`) for downstream filtering/reporting.

## Transform text using AI functions

Fabric Data Warehouse includes built-in functions for common text transformations such as grammar correction, summarization, and translation.

Example:

```sql
UPDATE Tickets

SET Summary = ai_summarize(Description),

Description = ai_fix_grammar(Description),

SpanishDescription = ai_translate(Description,’es’)

FROM Tickets;
```

The article’s point: doing this in the warehouse can apply transformations **at scale** across large tables without pushing the work into the application layer and building additional orchestration.

## Apply prompt engineering rules to transform text

When built-in functions aren’t enough, you can use a prompt-based function: **`ai_generate_response(instructions, text)`**.

Use cases mentioned:

- Mask sensitive data
- Customize sentiment analysis rules
- More precise extraction tailored to your scenario

Example: encapsulate a prompt inside a T-SQL function to analyze incident text and return concise JSON.

```sql
CREATE OR ALTER FUNCTION dbo.analyze_incident( @incident_text VARCHAR(8000) )

RETURNS VARCHAR(8000) AS BEGIN

RETURN

ai_generate_response(‘Analyze the incident and return a concise JSON with root_cause, immediate_action, and owner_team. Use short technical explanations to avoid quoting the text. Incident: ‘, @incident_text )

)

END;
```

Best practice called out: **encapsulate prompts in T-SQL functions or stored procedures** so they can be invoked as modules from queries.

## Conclusion

These preview AI features in **Fabric Data Warehouse** make it possible to process unstructured text directly in **T-SQL**, including custom prompt-based transformations. This supports warehouse-native automation for tasks like extraction, sentiment detection, classification, summarization, translation, and grammar fixes.

## Reference

- Documentation: [Use AI functions in Fabric Data Warehouse](https://learn.microsoft.com/fabric/Use%20AI%20functions%20in%20Fabric%20Data%20Warehouse)


[Read the entire article](https://blog.fabric.microsoft.com/en-US/blog/working-with-unstructured-text-in-fabric-data-warehouse-with-built-in-ai-functions-preview/)

