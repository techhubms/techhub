---
external_url: https://dellenny.com/boosting-productivity-with-github-copilot-real-world-net-coding-examples/
title: 'Boosting Productivity with GitHub Copilot: Real-World .NET Coding Examples'
author: Dellenny
feed_name: Dellenny's Blog
date: 2025-08-20 07:58:07 +00:00
tags:
- .NET
- AI Pair Programmer
- ASP.NET Core
- Boilerplate Reduction
- C#
- Code Automation
- Controller Scaffolding
- Copilot Best Practices
- EF
- LINQ
- Microsoft
- Productivity Tools
- Software Development
- Unit Testing
- VS
- VS Code
- XML Documentation
- xUnit
- AI
- GitHub Copilot
- Blogs
section_names:
- ai
- dotnet
- github-copilot
primary_section: github-copilot
---
Dellenny demonstrates how developers can leverage GitHub Copilot as an AI coding assistant in the .NET ecosystem, showcasing real-world examples to boost productivity and reduce repetitive tasks.<!--excerpt_end-->

# Boosting Productivity with GitHub Copilot: Real-World .NET Coding Examples

Author: Dellenny

Software development in the .NET ecosystem is powerful, but it's easy to get bogged down with repetitive boilerplate code and tedious tasks. GitHub Copilot steps in as an AI pair programmer within Visual Studio or VS Code, offering context-aware suggestions and automation across your workflow.

Below are practical examples of how Copilot can streamline your .NET development:

## 1. Quickly Generating ASP.NET Core Controllers

Building REST APIs in ASP.NET Core involves repetitive patterns, such as defining controllers, routes, and handling response logic. Copilot can scaffold these controllers efficiently.

**Example – Basic Products Controller:**

```csharp
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase {
    private readonly IProductService _productService;

    public ProductsController(IProductService productService) {
        _productService = productService;
    }

    [HttpGet]
    public IActionResult GetAll() {
        return Ok(_productService.GetAllProducts());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id) {
        var product = _productService.GetProductById(id);
        if (product == null) return NotFound();
        return Ok(product);
    }
}
```

Copilot speeds up controller creation, handling route attributes, parameter parsing, and response patterns for you.

## 2. Entity Framework Data Models

Defining classes for database tables in Entity Framework means writing many similar properties. Copilot suggests entire model structures based on naming conventions.

**Example – Product Entity:**

```csharp
public class Product {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

Typing just the class name helps Copilot propose common fields, accelerating model development.

## 3. Writing Unit Tests Faster with xUnit

Test writing can be time-consuming. Copilot generates test scaffolds and common assertions, especially in xUnit projects.

**Example – ProductServiceTests using xUnit:**

```csharp
using Xunit;

public class ProductServiceTests {
    [Fact]
    public void AddProduct_ShouldIncreaseCount() {
        var service = new ProductService();
        var initialCount = service.GetAllProducts().Count;
        service.AddProduct(new Product { Name = "Test", Price = 10.0m });
        Assert.Equal(initialCount + 1, service.GetAllProducts().Count);
    }

    [Fact]
    public void GetProductById_ShouldReturnNull_WhenNotFound() {
        var service = new ProductService();
        var result = service.GetProductById(999);
        Assert.Null(result);
    }
}
```

Arrange-Act-Assert patterns appear more readily when using Copilot for test case generation.

## 4. Auto-Generating LINQ Queries

Copilot assists with LINQ syntax, helping engineers draft queries, filters, and orderings quickly.

**Example – Filtering Products:**

```csharp
var expensiveProducts = products
    .Where(p => p.Price > 100)
    .OrderByDescending(p => p.Price)
    .ToList();
```

Start a query and Copilot can recommend appropriate filtering and ordering expressions.

## 5. Documentation and XML Comments

Excellent documentation improves maintainability. Copilot generates well-structured XML doc comments for methods and classes.

**Example – Doc Comment:**

```csharp
/// <summary>
/// Calculates the discount price for a product.
/// </summary>
/// <param name="price">The original price.</param>
/// <param name="percentage">The discount percentage.</param>
/// <returns>The price after discount.</returns>
public decimal CalculateDiscount(decimal price, decimal percentage) {
    return price - (price * (percentage / 100));
}
```

Starting a doc comment prompts Copilot to suggest summaries and parameter descriptions in standard XML format.

## Best Practices for Using Copilot in .NET

- **Always validate suggestions** to ensure correctness and performance
- **Use Copilot for scaffolding** (controllers, tests, boilerplate); refine business logic manually
- **Combine Copilot with .NET and Entity Framework tooling** for maximum productivity

GitHub Copilot is most valuable for eliminating repetitive work, letting you focus on domain-specific challenges in .NET development.

---

If you haven’t tried Copilot in your .NET workflow yet, it could become your favorite coding assistant.

---

This post appeared first on "Dellenny's Blog". [Read the entire article here](https://dellenny.com/boosting-productivity-with-github-copilot-real-world-net-coding-examples/)
