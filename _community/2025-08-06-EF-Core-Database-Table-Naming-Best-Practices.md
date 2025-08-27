---
layout: "post"
title: "EF Core Database Table Naming Best Practices"
description: "A community discussion on recommended strategies for naming database tables in Entity Framework Core (EF Core) projects. Covers default behaviors, using attributes or Fluent API for customization, and leveraging naming conventions such as snake_case versus PascalCase. Also addresses when explicit configuration is needed, and best practices for infrastructure versus domain modeling."
author: "jemofcourse"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://www.reddit.com/r/dotnet/comments/1mje247/ef_core_table_naming/"
viewing_mode: "external"
feed_name: "Reddit DotNet"
feed_url: "https://www.reddit.com/r/dotnet/.rss"
date: 2025-08-06 19:24:20 +00:00
permalink: "/2025-08-06-EF-Core-Database-Table-Naming-Best-Practices.html"
categories: ["Coding"]
tags: [".NET", "Attributes", "C#", "Coding", "Community", "Custom Conventions", "Database Design", "DbContext", "EF Core", "Fluent API", "Infrastructure Code", "Naming Conventions", "PascalCase", "Snake Case", "Table Naming"]
tags_normalized: ["dotnet", "attributes", "csharp", "coding", "community", "custom conventions", "database design", "dbcontext", "ef core", "fluent api", "infrastructure code", "naming conventions", "pascalcase", "snake case", "table naming"]
---

jemofcourse shares community insights and best practices on naming tables with Entity Framework Core, exploring default behaviors, Fluent API, and naming conventions for database clarity.<!--excerpt_end-->

# EF Core Database Table Naming Best Practices

When using Entity Framework Core (EF Core), the default table naming convention uses your C# class names as the table names in your database. This community discussion explores approaches and opinions on whether and how to configure table naming in EF Core, with contributions from experienced developers:

## Options for Table Naming

1. **Default Naming (Class Names):**
   - EF Core by default uses class names for table names, e.g., `User` class becomes `User` table.
   - Simple and works out of the box, but may not follow preferred database conventions.

2. **Explicit Naming (Attributes / Fluent API):**
   - You can customize table names using Data Annotations or the Fluent API.
   - Useful to enforce database team conventions or map to legacy/existing databases.
   - Example using Fluent API:

     ```csharp
     modelBuilder.Entity<User>().ToTable("users");
     ```

3. **Custom Conventions:**
   - Leverage naming conventions for consistency. For example, using `optionsBuilder.UseSnakeCaseNamingConvention()` for snake_case.
   - Remove or add conventions as needed:

     ```csharp
     configurationBuilder.Conventions.Remove<TableNameFromDbSetConvention>();
     ```

   - These help automate naming and reduce manual config.
   - Reference: [Custom Conventions in EF](https://learn.microsoft.com/en-us/ef/ef6/modeling/code-first/conventions/custom)

## Considerations and Best Practices

- For code-first projects, think like a DB admin: name tables, keys, indexes, and relationships intentionally. The structure should optimize for database clarity and maintainability.
- Be careful about mixing infrastructure, domain, and UI models (avoid using one model for all layers).
- Explicit table naming is most useful when mapping to existing databases or enforcing strict naming conventions.
- Many recommend avoiding outdated practices like prefixing tables with `TBL_` or columns with `FLD_`.
- Preferences vary: some like PascalCase in the DB, others prefer snake_case for readability.
- Generally, using Fluent API for mapping is recommended for flexibility and clarity.

## Sample DbSet in Context

```csharp
public virtual DbSet<User> Users { get; set; }
```

You can use the FluentAPI to further control how this maps to your schema. Alternatively, naming your model classes to match your intended entity names helps, but explicit configuration is often still valuable.

## Community Insights

- *No point giving them custom names, IMO. It's just another bit of config for something you will never use.*
- *It’s useful for mapping to an existing DB you didn’t name yourself.*
- *Whatever you do, don't prefix tables with TBL_ and fields with FLD_ like its the 1980s.*
- *Usually bad idea to leave everything to EF Core.. Use FluentAPI within context class.*

## Conclusion

Choose the table naming strategy that aligns with your project's requirements and team preferences. Use conventions or explicit mapping as needed, and prioritize clarity and maintainability for long-term success.

This post appeared first on "Reddit DotNet". [Read the entire article here](https://www.reddit.com/r/dotnet/comments/1mje247/ef_core_table_naming/)
