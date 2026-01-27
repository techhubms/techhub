---
external_url: https://khalidabuhakmeh.com/how-to-pick-the-right-constructor-when-using-activatorutilities-in-dotnet
title: How To Pick The Right Constructor When Using ActivatorUtilities In .NET
author: Khalid Abuhakmeh
feed_name: Khalid Abuhakmeh's Blog
date: 2024-08-13 00:00:00 +00:00
tags:
- .NET
- ActivatorUtilities
- ActivatorUtilitiesConstructorAttribute
- C#
- Constructor Selection
- Dependency Injection
- Microsoft.Extensions.DependencyInjection
- Reflection
- Service Provider
- ServiceCollection
section_names:
- coding
primary_section: coding
---
In this detailed guide, Khalid Abuhakmeh demonstrates how to select the appropriate constructor when using ActivatorUtilities for dependency injection in .NET. The author shares practical code examples and explains the impact of using attributes for constructor selection.<!--excerpt_end-->

![How To Pick The Right Constructor When Using ActivatorUtilities In .NET](https://res.cloudinary.com/abuhakmeh/image/fetch/c_limit,f_auto,q_auto,w_800/https://khalidabuhakmeh.com/assets/images/posts/misc/activatorutilities-dotnet-pick-constructor-attributes.jpg)

*Photo by [Bon Vivant](https://unsplash.com/@bonvivant)*

# How To Pick The Right Constructor When Using ActivatorUtilities In .NET

If you’ve worked with reflection in .NET, you’ve probably used `Activator` to create objects. With .NET 6, [`ActivatorUtilities`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.activatorutilities?view=net-6.0) was introduced to make class instantiation easier, especially for types with dependencies. Constructor dependency injection is standard in .NET, leading to many types needing constructor parameters. While `Activator` suits simple cases, `ActivatorUtilities` aligns better with today’s .NET development.

This article explores, with practical code, how to guide `ActivatorUtilities` to use a specific constructor when a type has multiple constructors.

## Registering a Type and the Dependencies

Consider the following `Person` class with three constructors:

```csharp
public class Person(string name, int age = 0) {
    public Person(string name) : this(name, 0) {
        Console.WriteLine("Person(string)");
    }

    public Person(object name) : this(name.ToString()!, 41) {
        Console.WriteLine("Person(object)");
    }

    public string Name { get; } = name;
    public int Age { get; } = age;
}
```

**Constructors:**

1. Primary constructor: takes a string `name` and optional `age`.
2. Constructor with a `string` dependency.
3. Constructor with an `object` dependency.

Now, set up a new `ServiceCollection` and register dependencies (make sure `Microsoft.Extensions.DependencyInjection` NuGet package is installed):

```csharp
using Microsoft.Extensions.DependencyInjection;

var collection = new ServiceCollection();
var value = "Khalid";

collection.AddTransient<Person>();           // Registers Person
collection.AddSingleton<object>(value);      // Registers object instance
collection.AddSingleton<string>(value);      // Registers string instance

var serviceProvider = collection.BuildServiceProvider();
var person = ActivatorUtilities.CreateInstance<Person>(serviceProvider);

Console.WriteLine($"{person.Name} is {person.Age} years old.");
```

Which constructor gets called? Given the dependencies are satisfied, let’s check the result:

```text
Khalid is 0 years old.
```

The primary constructor gets called by default.

## Choosing a Different Constructor with ActivatorUtilitiesConstructor Attribute

Want to use a different constructor? The `Microsoft.Extensions.DependencyInjection` package provides the `ActivatorUtilitiesConstructorAttribute`. Apply it to your preferred constructor, for example, the one accepting `object`:

```csharp
public class Person(string name, int age = 0)
{
    public Person(string name) : this(name, 0)
    {
        Console.WriteLine("Person(string)");
    }

    [ActivatorUtilitiesConstructor]
    public Person(object name) : this(name.ToString()!, 41)
    {
        Console.WriteLine("Person(object)");
    }

    public string Name { get; } = name;
    public int Age { get; } = age;
}
```

Rerunning the previous code now outputs:

```text
Person(object)
Khalid is 41 years old.
```

You didn't change any executing logic—just decorated a constructor with an attribute. This allows you to have as many constructors as needed while specifying which one `ActivatorUtilities` will use.

---

*With this approach, .NET developers get explicit control over dependency injection and constructor selection, reducing ambiguity and making behavior clear.*

---

*Author: Khalid Abuhakmeh, Developer Advocate at JetBrains focusing on .NET technologies and tooling.*

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/how-to-pick-the-right-constructor-when-using-activatorutilities-in-dotnet)
