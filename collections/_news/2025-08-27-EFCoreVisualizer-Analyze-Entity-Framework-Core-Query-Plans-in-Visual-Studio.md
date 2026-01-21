---
external_url: https://devblogs.microsoft.com/dotnet/ef-core-visualizer-view-entity-framework-core-query-plan-inside-visual-studio/
title: 'EFCore.Visualizer: Analyze Entity Framework Core Query Plans in Visual Studio'
author: Giorgi Dalakishvili
feed_name: Microsoft .NET Blog
date: 2025-08-27 17:05:00 +00:00
tags:
- .NET
- .NET Data
- Database Indexes
- Debugger Visualizer
- EF
- EF Core
- EFCore.Visualizer
- LINQ
- MySQL
- Oracle
- ORM
- Performance Tuning
- PostgreSQL
- Query Execution Plan
- RDBMS
- SQL Server
- SQLite
- Visual Studio Extension
- VS
section_names:
- coding
---
Giorgi Dalakishvili details how the EFCore.Visualizer extension for Visual Studio helps developers analyze and optimize Entity Framework Core query execution plans directly from the IDE.<!--excerpt_end-->

# EFCore.Visualizer: Analyze Entity Framework Core Query Plans in Visual Studio

**Author:** Giorgi Dalakishvili

Entity Framework Core (EF Core) is widely used as an ORM in .NET applications, translating LINQ queries into SQL for a variety of relational databases. However, as applications and schemas grow more complex, generated SQL queries can become inefficient—often due to missing indexes or suboptimal query shapes. While EF Core supports basic query logging and slow query detection, truly understanding database performance issues requires examining the actual execution plan from the database engine.

## Visualizing Query Plans in the IDE

**EFCore.Visualizer** addresses this gap through a Visual Studio extension that lets developers inspect generated SQL and its execution plan directly within the IDE. Once installed, the extension adds a debugger visualizer for `IQueryable<>` variables. When debugging, you can hover over any EF Core query, open the visualizer, and immediately see both the SQL and a graphical representation of the query plan—no need to switch to external database management tools.

### Supported Databases

- SQL Server
- PostgreSQL
- MySQL
- SQLite
- Oracle

EFCore.Visualizer automatically detects the connected provider and uses different visualization libraries for each database engine.

## Installation Steps

1. Open Visual Studio Extension Manager.
2. Search for “EFCore.Visualizer” and install, or download from the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=GiorgiDalakishvili.EFCoreVisualizer).

## Usage Example

Suppose you have the following EF Core model:

```csharp
public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Post>().HasIndex(p => p.PublishedAt);
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
    public List<Post> Posts { get; } = new();
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}
```

You debug a LINQ query such as:

```csharp
var postsQuery = bloggingContext.Posts.Where(post => post.PublishedAt.Year == 2010);
```

Upon visualizing the query plan, you may discover that SQL Server does not use the index on `PublishedAt` because the query extracts the year, making it **non-sargable**. Refactoring the query to use a date range instead enables the database to utilize the index:

```csharp
var fromDate = new DateTime(2010, 1, 1);
var toDate = new DateTime(2011, 1, 1);
postsQuery = bloggingContext.Posts.Where(post => post.PublishedAt >= fromDate && post.PublishedAt < toDate);
```

This optimization allows SQL Server to perform an efficient index seek.

## Under the Hood

The visualizer executes the LINQ query as an ADO.NET command, then retrieves its execution plan from the database engine. It uses libraries like [html-query-plan](https://github.com/JustinPealing/html-query-plan) for SQL Server or [pev2](https://github.com/dalibo/pev2/) for PostgreSQL to render the plan visually.

## Limitations

- The visualizer does **not** support queries with reducing operators like `Count()`, `Min()`, or `First()`.
- Network latency or highly complex queries might cause the visualizer to exceed its five-second timeout.
- Timeout settings cannot be extended, but you can upvote the [Visual Studio issue](https://github.com/microsoft/VSExtensibility/issues/325).

## Conclusion

EFCore.Visualizer shortens the developer loop by making execution plan analysis available directly in Visual Studio. If you frequently troubleshoot EF Core performance issues, this extension may save significant time.

- Try EFCore.Visualizer from the [Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=GiorgiDalakishvili.EFCoreVisualizer)
- Source code is available on [GitHub](https://github.com/Giorgi/EFCore.Visualizer) for those interested in contributing or exploring the implementation.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/ef-core-visualizer-view-entity-framework-core-query-plan-inside-visual-studio/)
