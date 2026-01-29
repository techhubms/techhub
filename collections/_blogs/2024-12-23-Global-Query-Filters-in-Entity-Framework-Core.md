---
external_url: https://code-maze.com/efcore-global-query-filters/
title: Global Query Filters in Entity Framework Core
author: Gergő Vándor
feed_name: Code Maze Blog
date: 2024-12-23 04:47:32 +00:00
tags:
- .NET
- C#
- Database Filtering
- DbContext
- Dependency Injection
- EF Core
- Filtering
- Global Query Filter
- Global Query Filters
- in Memory Database
- LINQ
- Model Configuration
- Multitenancy
- Navigation Properties
- Row Level Security
- Soft Delete
- Coding
- Blogs
section_names:
- coding
primary_section: coding
---
Gergő Vándor presents a comprehensive guide to Entity Framework Core's global query filters, detailing their purpose, implementation, and key considerations for .NET developers.<!--excerpt_end-->

# Global Query Filters in Entity Framework Core

*By Gergő Vándor*

---

## Introduction

In this article, we discuss Entity Framework Core’s (EF Core) global query filters and their practical applications. We'll cover scenarios where query filters are beneficial, how to implement them (using soft delete as an example), and important considerations such as their impact on navigation properties.

[Source code for this article is available on GitHub.](https://github.com/CodeMazeBlog/CodeMazeGuides/tree/main/dotnet-efcore/GlobalQueryFilters)

---

## What Is a Global Query Filter?

Global query filters in EF Core allow you to apply a `WHERE` condition to all queries for a given entity type. Defined at the model level (e.g., within `OnModelCreating`), these filters are enforced on every query unless explicitly disabled. This is useful for ensuring certain constraints are always respected, such as limiting data access to specific users or filtering out 'soft deleted' records.

### Common Use Cases

1. **Row-Level Security**: Restrict each user to records they've created by applying a global filter tied to the user's ID.
2. **Multitenancy**: Apply a filter based on tenant ID, ensuring users only view data within their tenancy.
3. **Soft Delete** (most common): Rather than deleting records from the database, flag them as deleted (`IsDeleted = true`) and use a global filter to exclude deleted entities by default.

---

## Implementing a Global Query Filter – Soft Delete Example

### Setting Up the Project

- Create a new console project:

  ```shell
  mkdir GlobalQueryFilters
  cd GlobalQueryFilters
  dotnet new console
  ```

- Install EF Core and InMemory provider:

  ```shell
  dotnet add package Microsoft.EntityFrameworkCore
  dotnet add package Microsoft.EntityFrameworkCore.InMemory
  ```

### Creating the DbContext

```csharp
public sealed class SoftDeleteDbContext(DbContextOptions<SoftDeleteDbContext> options) : DbContext(options)
{
}
```

Register the `DbContext`:

```csharp
var services = new ServiceCollection();
services.AddDbContext<SoftDeleteDbContext>(options =>
    options.UseInMemoryDatabase("GlobalQueryFilters"));
var serviceProvider = services.BuildServiceProvider();
```

### Defining the Entity and Query Filter

Create an entity with an `IsDeleted` flag:

```csharp
public sealed class SoftDeleteEntity
{
    public Guid Id { get; private set; }
    public bool IsDeleted { get; set; }
}
```

Apply the global query filter in `OnModelCreating`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<SoftDeleteEntity>().HasKey(s => s.Id);
    modelBuilder.Entity<SoftDeleteEntity>().Property(s => s.IsDeleted).IsRequired();
    modelBuilder.Entity<SoftDeleteEntity>().HasQueryFilter(entity => !entity.IsDeleted);
}
```

This filter will exclude entities where `IsDeleted == true` from all queries by default.

---

## Testing the Global Query Filter

Add and soft delete an entity:

```csharp
var dbContext = serviceProvider.GetRequiredService<SoftDeleteDbContext>();

var entity = new SoftDeleteEntity();
dbContext.SoftDeleteEntities.Add(entity);
await dbContext.SaveChangesAsync();
Console.WriteLine($"New entity added with id: {entity.Id}");

var entityToDelete = await dbContext.SoftDeleteEntities.FirstOrDefaultAsync(e => e.Id == entity.Id);
Console.WriteLine($"Entity queried from DB: {entityToDelete!.Id}");

entityToDelete.IsDeleted = true;
await dbContext.SaveChangesAsync();
Console.WriteLine("Entity soft deleted");

var entityAfterDelete = await dbContext.SoftDeleteEntities.FirstOrDefaultAsync(e => e.Id == entity.Id);
Console.WriteLine($"Entity queried from DB after soft delete: {entityAfterDelete?.Id.ToString() ?? "null"}");
```

**Output Example:**

```
New entity added with id: b56e23a7-3333-4397-3acf-08dcfbf3bcbf
Entity queried from DB: b56e23a7-3333-4397-3acf-08dcfbf3bcbf
Entity soft deleted
Entity queried from DB after soft delete: null
```

This output demonstrates that after soft deleting, the entity is no longer returned by queries due to the active filter.

---

## Disabling Global Query Filters

To query all records, including those normally filtered (e.g., to restore deleted items), use EF Core’s `IgnoreQueryFilters()`:

```csharp
var entityAfterDeleteWithDisabledQueryFilter = await dbContext.SoftDeleteEntities
    .IgnoreQueryFilters()
    .FirstOrDefaultAsync(e => e.Id == entity.Id);
Console.WriteLine($"Entity queried from DB after soft delete with disabled query filter: {entityAfterDeleteWithDisabledQueryFilter?.Id.ToString() ?? "null"}");
```

**Output Example:**

```
Entity queried from DB after soft delete with disabled query filter: 04a64c96-48a2-41f8-0a53-08dcfd79062d
```

---

## Pitfall: Query Filters and Required Navigation Properties

A common issue arises if you use required navigation properties with global query filters. If a required navigation property is filtered out but not its parent, the parent is also excluded due to the underlying `INNER JOIN`.

### Example

- Define child and parent entities:

```csharp
public sealed class ChildEntity
{
    public Guid Id { get; private set; }
    public ParentEntity Parent { get; private set; } = null!;
}

public sealed class ParentEntity(IEnumerable<ChildEntity> children)
{
    public Guid Id { get; private set; }
    public bool IsDeleted { get; set; }
    public IEnumerable<ChildEntity> Children { get; private set; } = children;
    private ParentEntity() : this(null!){}
}
```

- DbContext configuration (simplified):

```csharp
modelBuilder.Entity<ParentEntity>().HasKey(s => s.Id);
modelBuilder.Entity<ParentEntity>().Property(s => s.IsDeleted).IsRequired();
modelBuilder.Entity<ParentEntity>().HasQueryFilter(entity => !entity.IsDeleted);
modelBuilder.Entity<ParentEntity>().HasMany(s => s.Children).WithOne(c => c.Parent).IsRequired();
modelBuilder.Entity<ChildEntity>().HasKey(c => c.Id);
```

- Testing

```csharp
var navPropDbContext = serviceProvider.GetRequiredService<NavPropDbContext>();
var child = new ChildEntity();
var parent = new ParentEntity([child]);
navPropDbContext.ParentEntities.Add(parent);
await navPropDbContext.SaveChangesAsync();

parent.IsDeleted = true;
await navPropDbContext.SaveChangesAsync();

var childAfterParentDelete = await navPropDbContext.Children
    .Include(c => c.Parent)
    .FirstOrDefaultAsync(c => c.Id == child.Id);
Console.WriteLine($"Child queried after parent is soft deleted: {childAfterParentDelete?.Id.ToString() ?? "null"}");
```

If the parent is soft deleted, querying the children (with a required parent relationship and a global filter on the parent) will return `null` as a result. This is because the required navigation property translates to an `INNER JOIN` in SQL.

#### Mitigation Strategies

- Apply corresponding filters to child entities:

  ```csharp
  modelBuilder.Entity<ChildEntity>().HasQueryFilter(c => !c.Parent.IsDeleted);
  ```

- If you want children to be returned regardless of their parent, make the relationship optional (`LEFT JOIN`).

---

## Conclusion

Entity Framework Core’s global query filters are a powerful tool for enforcing application-wide query constraints such as soft delete, multi-tenancy, or row-level security. They are easy to implement but require careful consideration with required navigation properties to avoid unintended data omission. Use `IgnoreQueryFilters()` when you need to override default filter behavior in specific queries.

---

*For more about EF Core and .NET development, check out additional resources and courses at [Code Maze](https://code-maze.com).*

This post appeared first on "Code Maze Blog". [Read the entire article here](https://code-maze.com/efcore-global-query-filters/)
