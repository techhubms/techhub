---
external_url: https://devblogs.microsoft.com/dotnet/mongodb-efcore-provider-queryable-encryption-vector-search/
title: 'Secure and Intelligent: Queryable Encryption and Vector Search in MongoDB EF Core Provider'
author: Rishit Bhatia, Luce Carter
viewing_mode: external
feed_name: Microsoft .NET Blog
date: 2026-01-07 18:05:00 +00:00
tags:
- .NET
- C#
- Data Security
- Database Development
- EF
- EF Core
- Encryption
- LINQ
- Mongodb
- Queryable Encryption
- Rag
- Semantic Search
- Vector Embeddings
- Vector Search
- Vectorsearch
section_names:
- ai
- coding
---
Rishit Bhatia and Luce Carter discuss how developers can secure data and enable AI-powered search using Queryable Encryption and Vector Search in the MongoDB EF Core provider, with hands-on guidance and code samples.<!--excerpt_end-->

# Secure and Intelligent: Queryable Encryption and Vector Search in MongoDB EF Core Provider

**Authors: Rishit Bhatia (Senior Product Manager, MongoDB) and Luce Carter (Senior Developer Advocate, MongoDB, Microsoft MVP)**

The MongoDB Entity Framework (EF) Core provider has generally been available since May 2024, allowing developers to integrate MongoDB into their .NET projects using modern EF Core patterns such as LINQ queries, change tracking, and optimistic concurrency. The provider continues to evolve, adding features and improving support for MongoDB-specific operations.

## What's New: Queryable Encryption and Vector Search

The latest version introduces two major features for .NET developers building with MongoDB:

### Queryable Encryption

- Encrypt sensitive data in MongoDB while retaining the ability to run queries on encrypted fields.
- Meets the needs of industries with strict data governance (e.g. healthcare, finance).
- Easily define encrypted properties in your EF Core data models using `OnModelCreating`.

**Sample Configuration:**

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder) {
  modelBuilder.Entity<Employee>(entity => {
    entity.Property(e => e.TaxPayerId)
      .IsEncryptedForEquality(<Your Data Encryption Key GUID>);
    entity.Property(e => e.Salary)
      .HasBsonRepresentation(BsonType.Decimal128)
      .IsEncryptedForRange(0m, 10000000m, 0, <Your Data Encryption Key GUID>);
  });
}
```

**Query Encrypted Fields:**

```csharp
// Encrypted Equality Query
var specificEmployee = db.Employees.Where(e => e.TaxPayerId == "45678");
// Encrypted Range Query
var seniorEmployees = db.Employees.Where(e => e.Salary >= 100000m && e.Salary < 200000m);
```

Read the [full tutorial](https://dev.to/mongodb/getting-started-with-queryable-encryption-with-the-mongodb-ef-core-provider-5238) for more examples.

### Vector Search

- Enables semantic or similarity searching over unstructured data using vector embeddings.
- Ideal for AI-powered applications like recommendation engines, semantic search, or anomaly detection.
- Configure vector embedding fields in your data models and run LINQ-based vector queries.

**Sample Model Definition:**

```csharp
// Inside OnModelCreating
b.Property(e => e.PlotEmbedding)
  .HasElementName("plot_embedding_voyage_3_large")
  .HasBinaryVectorDataType(BinaryVectorDataType.Float32);

// OR, in model
[BinaryVector(BinaryVectorDataType.Float32)]
public float[]? PlotEmbedding { get; set; }
```

**Running a Vector Search:**

```csharp
var similarMovies = await db.Movies.VectorSearch(
    e => e.PlotEmbedding, myCustom.PlotEmbedding, limit: 10)
  .ToListAsync();
```

Refer to the [vector search tutorial](https://dev.to/mongodb/mongodb-vector-search-with-ef-core-3dbh) for extended use cases.

## Getting Started

- Set up a .NET console app and connect to [MongoDB Atlas](https://www.mongodb.com/products/platform/atlas-database/), MongoDB's fully managed cloud database service.
- Explore these features with the [MongoDB EF Core provider documentation](https://www.mongodb.com/docs/entity-framework/current/quick-start/).

## Additional Resources

- [EF Core documentation](https://learn.microsoft.com/ef/core/)
- [MongoDB documentation](https://www.mongodb.com/docs/)
- [Queryable Encryption official docs](https://www.mongodb.com/docs/manual/core/queryable-encryption/)
- [MongoDB Vector Search tutorial](https://www.mongodb.com/docs/atlas/atlas-vector-search/tutorials/vector-search-quick-start/)
- Boost your skills with the [Vector Search badge course](https://learn.mongodb.com/courses/vector-search-fundamentals?team=devrel-content)

Give these new features a try in your next .NET project and discover new possibilities for secure and intelligent data-driven applications.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/mongodb-efcore-provider-queryable-encryption-vector-search/)
