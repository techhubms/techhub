---
layout: post
title: 'Partial Updates in MongoDB via Azure Data Factory Data Flow: Nested Field Modification'
author: leopoldinoex
canonical_url: https://techcommunity.microsoft.com/t5/azure-data-factory/help-with-partial-mongodb-update-via-azure-data-factory-data/m-p/4443596#M937
viewing_mode: external
feed_name: Microsoft Tech Community
feed_url: https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community
date: 2025-08-14 16:29:48 +00:00
permalink: /azure/community/Partial-Updates-in-MongoDB-via-Azure-Data-Factory-Data-Flow-Nested-Field-Modification
tags:
- Array Manipulation
- Azure Data Factory
- Data Flow
- Derived Column
- ETL
- Id Key
- JSON Processing
- MongoDB
- Nested Fields
- NoSQL
- Partial Update
- Sink Configuration
section_names:
- azure
---
leopoldinoex seeks detailed advice on performing partial, in-place updates to nested fields in MongoDB using Data Flow in Azure Data Factory—focusing on transformation expressions and preserving document structure.<!--excerpt_end-->

# Partial Updates in MongoDB via Azure Data Factory Data Flow

**Posted by: leopoldinoex**

## Scenario

You're using Azure Data Factory (ADF) Data Flow to process JSON documents for a MongoDB collection. The document structure looks like this:

```json
{
  "_id": { "$oid": "1xp3232to" },
  "root_field": "root_value",
  "main_array": [
    {
      "array_id": "id001",
      "status": "PENDING",
      "nested_array": []
    }
  ],
  "numeric_value": { "$numberDecimal": "10.99" }
}
```

## Update Requirements

1. Change the `status` field within each object in `main_array` from "PENDING" to "SENT".
2. Add a new object to the `nested_array` with these fields:
   - `event`: "SENT"
   - `description`: "FILE GENERATED"
   - `timestamp`: (current date/time)
   - `system`: "Sis Test"

## Expression in Data Flow's Derived Column

You want both changes to occur in a single transformation.

**Example Data Flow Expression** (assuming `main_array` is of array type):

```csharp
main_array =
  array(
    main_array,
    x -> x.status == 'PENDING' ? {
      ...x,
      status: 'SENT',
      nested_array: arrayUnion(
        x.nested_array,
        array({
          event: 'SENT',
          description: 'FILE GENERATED',
          timestamp: currentUTC(),
          system: 'Sis Test'
        })
      )
    } : x
  )
```

- `...x` copies all current object properties.
- The `status` field is overwritten to "SENT" for items where status is "PENDING".
- The `nested_array` is appended with the new event.
- For non-PENDING items, the object is kept as-is.

*Note: The exact syntax might require adaptation depending on Data Flow's expression language version. `arrayUnion` and direct destructuring with `{ ...x, ...}` are representative; check Data Flow's [expression documentation](https://learn.microsoft.com/en-us/azure/data-factory/data-flow-expression-functions) for up-to-date functions.*

## MongoDB Sink Configuration for Partial Update

- Set the **Update Method** in MongoDB Sink to "Update" (instead of Insert/Replace).
- Specify `_id` as the **key column** to match existing documents.
- In the update settings, enable "Update Method = Partial Update" or equivalent (sometimes called 'update mode: upsert/no upsert').
- Map only the fields you wish to update (in your sink mapping), NOT the entire document. This preserves fields like `root_field` and `numeric_value` during the partial update.
- Use the **'Update Filter'** on `_id` to target the correct document.

## Recommendations

- Test the Derived Column transformation logic with sample data and use Data Preview.
- Carefully review Sink mapping, ensuring only the intended sub-fields are updated.
- Use partial update mode in the MongoDB Sink to avoid overwriting non-updated fields.
- Review ADF documentation for any changes in expression support or MongoDB sink behavior.

## References

- [Data Flow expressions in ADF](https://learn.microsoft.com/en-us/azure/data-factory/data-flow-expression-functions)
- [Mapping data flows: MongoDB connector](https://learn.microsoft.com/en-us/azure/data-factory/connector-mongodb#mapping-data-flows)

## Summary

This approach allows you to efficiently update only the needed nested fields in your MongoDB documents via Azure Data Factory Data Flows, ensuring data integrity for untouched fields.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-data-factory/help-with-partial-mongodb-update-via-azure-data-factory-data/m-p/4443596#M937)
