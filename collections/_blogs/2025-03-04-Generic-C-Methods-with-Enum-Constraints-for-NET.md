---
external_url: https://khalidabuhakmeh.com/generic-csharp-methods-with-enum-constraints-for-dotnet
title: Generic C# Methods with Enum Constraints for .NET
author: Khalid Abuhakmeh
feed_name: Khalid Abuhakmeh's Blog
date: 2025-03-04 00:00:00 +00:00
tags:
- .NET
- C#
- Enum
- Generics
- Helper Methods
- Metadata
- Reflection
- Tuples
- Type Constraints
- Web Applications
section_names:
- coding
primary_section: coding
---
In this post, Khalid Abuhakmeh illustrates how to leverage modern C# features to create type-safe, generic helper methods for extracting enum metadata, offering practical insight for .NET developers.<!--excerpt_end-->

# Generic C# Methods with Enum Constraints for .NET

*By Khalid Abuhakmeh*

![Generic C# Methods with Enum Constraints for .NET](https://res.cloudinary.com/abuhakmeh/image/fetch/c_limit,f_auto,q_auto,w_800/https://khalidabuhakmeh.com/assets/images/posts/misc/generic-csharp-methods-enums-dotnet.jpg)

> Photo by [Stephanie Klepacki](https://unsplash.com/@sklepacki)

Every couple of years, I tend to write the same variation of an `enum` helper that reads metadata from an enumeration using reflection. Almost any .NET developer with a few years of experience has done the same. The implementation uses Generics to work for any `enum` defined in my solution, as there is typically more than one described in a typical .NET solution. Historically, generics and enums didn’t work well because of the limitations of generics when dealing with this particular type, but to my surprise, they do now!

In this post, we’ll explore how to write the helper method I’m describing while ensuring the generic constraints stop us from passing in arguments other than enums.

## Straight To The Implementation

Since this post will be short, I’ll start with a sample you can play with:

```csharp
using System.ComponentModel;
using System.Reflection;

// get all descriptions
{
    Console.WriteLine("Groceries:\n");
    var descriptions = Enums.GetDescriptions<Groceries>();
    foreach (var (value, description) in descriptions)
    {
        Console.WriteLine($"{value} - {description}");
    }
    Console.WriteLine();
}

// Get a description for a single value
{
    var (value, description) = Enums.GetDescription(Groceries.Fruit);
    Console.WriteLine($"Single value:\n{value} - {description}");
}

public enum Groceries
{
    [Description("Apples, Oranges, Bananas")]
    Fruit,
    [Description("Spinach, Kale, Broccoli, Cabbage")]
    Vegetables,
    [Description("Cheese, Milk, Yogurt")]
    Dairy,
    [Description("Chicken, Beef, Pork, Lamb, Turkey")]
    Meat,
    [Description("Anything not listed above")]
    Other
}

public static class Enums
{
    public static IEnumerable<(TEnum Value, string Description)> GetDescriptions<TEnum>() where TEnum : struct, Enum
    {
        var values = Enum.GetValues<TEnum>();
        foreach (var value in values)
        {
            yield return GetDescription(value);
        }
    }

    public static (TEnum Value, string Description) GetDescription<TEnum>(TEnum value) where TEnum : struct, Enum
    {
        var type = typeof(TEnum);
        var name = value.ToString();
        var info = type.GetMember(name)[0];
        var description = info.GetCustomAttribute<DescriptionAttribute>() is { } attr ? attr.Description : name;
        return (value, description);
    }
}
```

The essential part of our generic methods is our method declarations’ `where` clause:

```csharp
where TEnum: struct, Enum
```

This line adds two crucial elements to our generic method implementation:

- It ensures that `TEnum` is an `Enum` type.
- The `struct` constraint ensures that `TEnum` is non-nullable.

The non-nullability of an `Enum` is essential since we are using the `Enum.GetValues<TEnum>` method to get known values. You could remove this constraint, but your implementation would require more boxing and defensive programming in the form of null checks. It’s more straightforward to enforce the `struct` requirement.

## Conclusion

There you go—a constrained generic method implementation that can get metadata from an Enum value. If you’re still wondering, “where would I use this?” I find these methods helpful in web applications for creating a `SelectListItem`, but I stuck with Tuples for this post. What a time to be alive. Cheers!

---

![Khalid Abuhakmeh's Picture](/assets/images/authorimage.jpg)

### About Khalid Abuhakmeh

Khalid is a developer advocate at JetBrains focusing on .NET technologies and tooling.

---

### Read Next

[Strongly-Typed Markdown for ASP.NET Core Content Apps](/strongly-typed-markdown-for-aspnet-core-content-apps)

[Server-Sent Events in ASP.NET Core and .NET 10](/server-sent-events-in-aspnet-core-and-dotnet-10)

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/generic-csharp-methods-with-enum-constraints-for-dotnet)
