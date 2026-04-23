*

C# / .NET Principal Content developer

 

Union types have been frequently requested for C#, and they’re here. Starting with .NET 11 Preview 2, C# 15 introduces the `union` keyword. The `union` keyword declares that a value is exactly one of a fixed set of types with compiler-enforced exhaustive pattern matching. If you’ve used discriminated unions in F# or similar features in other languages, you’ll feel right at home. But C# unions are designed for a C#-native experience: they’re type unions that compose existing types, integrate with the pattern matching you already know, and work seamlessly with the rest of the language.

## What are union types?

Before C# 15, when a method needs to return one of several possible types, you had imperfect options. Using `object` placed no constraints on what types are actually stored — any type could end up there, and the caller had to write defensive logic for unexpected values. Marker interfaces and abstract base classes were better because they restrict the set of types, but they can’t be “closed” — anyone can implement the interface or derive from the base class, so the compiler can never consider the set complete. And both approaches require the types to share a common ancestor, which doesn’t work when you wanted a union of unrelated types like `string` and `Exception`, or `int` and `IEnumerable`.

Union types solve these problems. A union declares a closed set of case types — they don’t need to be related to each other, and no other types can be added. The compiler guarantees that `switch` expressions handling the union are exhaustive, covering every case type without needing a discard `_` or default branch. But it’s more than exhaustiveness: unions enable designs that traditional hierarchies can’t express, composing any combination of existing types into a single, compiler-verified contract.

Here’s the simplest declaration:

```csharp
public record class Cat(string Name);
public record class Dog(string Name);
public record class Bird(string Name);

public union Pet(Cat, Dog, Bird);
```

This single line declares `Pet` as a new type whose variables can hold a `Cat`, a `Dog`, or a `Bird`. The compiler provides implicit conversions from each case type, so you can assign any of them directly:

```csharp
Pet pet = new Dog("Rex");
Console.WriteLine(pet.Value); // Dog { Name = Rex }

Pet pet2 = new Cat("Whiskers");
Console.WriteLine(pet2.Value); // Cat { Name = Whiskers }
```

The compiler issues an error if you assign an instance of a type that isn’t one of the case types to a `Pet` object.

The When you use an instance of a `union` type known to be not null, the compiler knows the complete set of case types, so a `switch` expression that covers all of them is exhaustive— no discard needed:

```csharp
string name = pet switch
{
 Dog d => d.Name,
 Cat c => c.Name,
 Bird b => b.Name,
};
```

The types `Dog`, `Cat`, and `Bird` are all non-nullable types. The `pet` variable is known to be non-null, it was set in the earlier snippet. Therefore, this `switch` expression isn’t required to check for `null`. If any of the types are nullable, for example `int?` or `Bird?`, all `switch` expressions for a `Pet` instance would need a `null` arm for exhaustiveness. If you later add a fourth case type to `Pet`, every `switch` expression that doesn’t handle it produces a compiler warning. That’s one core value: the compiler catches missing cases at build time, not at runtime.

Patterns apply to the union’s `Value` property, not the union struct itself. This “unwrapping” is automatic — you write `Dog d` and the compiler checks `Value` for you. The two exceptions are `var` and `_`, which apply to the union value itself so you can capture or ignore the whole union.

For `union` types, the `null` pattern checks whether `Value` is null. The `default` value of a union struct has a null `Value`:

```csharp
Pet pet = default;

var description = pet switch
{
 Dog d => d.Name,
 Cat c => c.Name,
 Bird b => b.Name,
 null => "no pet",
};
// description is "no pet"
```

The `Pet` example illustrates the syntax. Now, let’s explore real world scenarios for union types.

### OneOrMore — single value or collection

APIs sometimes accept either a single item or a collection. A union with a body lets you add helper members alongside the case types. The `OneOrMore` declaration includes an `AsEnumerable()` method directly in the union body — just like you’d add methods to any type declaration:

```csharp
public union OneOrMore(T, IEnumerable)
{
 public IEnumerable AsEnumerable() => Value switch
 {
 T single => [single],
 IEnumerable multiple => multiple,
 null => []
 };
}
```

Notice that the `AsEnumerable` method must handle the case where `Value` is `null`. That’s because the default null-state of the `Value` property is maybe-null*. This rule is necessary to provide proper warnings for arrays of a union type, or instances of the default value for the `union` struct.

Callers pass whichever form is convenient, and `AsEnumerable()` normalizes it:

```csharp
OneOrMore tags = "dotnet";
OneOrMore moreTags = new[] { "csharp", "unions", "preview" };

foreach (var tag in tags.AsEnumerable())
 Console.Write($"[{tag}] ");
// [dotnet]

foreach (var tag in moreTags.AsEnumerable())
 Console.Write($"[{tag}] ");
// [csharp] [unions] [preview]
```

## Custom unions for existing libraries

The `union` declaration is an opinionated shorthand. The compiler generates a struct with a constructor for each case type and a `Value` property of type `object?` that holds the underlying value. The constructors enable implicit conversions from any of the case types to the union type. The union instance always stores its contents as a single `object?` reference and boxes value types. That covers the majority of use cases cleanly.

But several community libraries already provide union-like types with their own storage strategies. Those libraries don’t need to switch to the `union` syntax to benefit from C# 15. Any class or struct with a `[System.Runtime.CompilerServices.Union]` attribute is recognized as a union type, as long as it follows the basic union pattern: one or more public single-parameter constructors (defining the case types) and a public `Value` property.

For performance-sensitive scenarios where case types include value types, libraries can also implement the non-boxing access pattern by adding a `HasValue` property and `TryGetValue` methods. This lets the compiler implement pattern matching without boxing.

For full details on creating custom union types and the non-boxing access pattern, see the [union types language reference](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/union#custom-union-types).

## Related proposals

Union types give you a type that contains one of a closed set of types. Two proposed features provide related functionality for type hierarchies and enumerations. You can learn about both proposals and how they relate to unions by reading the feature specifications:

- [Closed hierarchies](https://github.com/dotnet/csharplang/blob/main/proposals/closed-hierarchies.md): The `closed` modifier on a class prevents derived classes from being declared outside the defining assembly.

- [Closed enums](https://github.com/dotnet/csharplang/blob/main/proposals/closed-enums.md): A `closed` enum prevents creation of values other than the declared members.

Together, these three features give C# a comprehensive exhaustiveness story:

- **Union types** — exhaustive matching over a closed set of types

- **Closed hierarchies** — exhaustive matching over a sealed class hierarchy

- **Closed enums** — exhaustive matching over a fixed set of enum values

Union types are available now in preview. When evaluating them, keep this broader roadmap in mind. These proposals are active, but aren’t yet committed to a release. Join the discussion as we continue the design and implementation of them.

## Try it yourself

Union types are available starting with .NET 11 Preview 2. To get started:

- Install the [.NET 11 Preview SDK](https://dotnet.microsoft.com/download/dotnet).

- Create or update a project targeting `net11.0`.

- Set `preview` in your project file.

IDE support in Visual Studio will be available in the next Visual Studio Insiders build. It is included in the latest C# DevKit Insiders build.

****Early preview: declare runtime types yourself**

In .NET 11 Preview 2, the `UnionAttribute` and `IUnion` interface aren’t included in the runtime yet. You must declare them in your project. Later preview versions will include these types in the runtime.

Add the following to your project (or grab [RuntimePolyfill.cs](https://github.com/dotnet/docs/blob/e68b5dd1e557b53c45ca43e61b013bc919619fb9/docs/csharp/language-reference/builtin-types/snippets/unions/RuntimePolyfill.cs) from the docs repo):

```csharp
namespace System.Runtime.CompilerServices
{
 [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct,
 AllowMultiple = false)]
 public sealed class UnionAttribute : Attribute;

 public interface IUnion
 {
 object? Value { get; }
 }
}
```

Once those are in place, you can declare and use union types:

```csharp
public record class Cat(string Name);
public record class Dog(string Name);

public union Pet(Cat, Dog);

Pet pet = new Cat("Whiskers");
Console.WriteLine(pet switch
{
 Cat c => $"Cat: {c.Name}",
 Dog d => $"Dog: {d.Name}",
});
```

Some features from the full [proposal specification](https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/unions) aren’t yet implemented, including union member providers. Those are coming in future previews.

## Share your feedback

Union types are in preview, and your feedback directly shapes the final design. Try them in your projects, explore edge cases, and tell us what works and what doesn’t.

To learn more:

- [Union types — language reference](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/union)

- [Unions — feature specification](https://learn.microsoft.com/dotnet/csharp/language-reference/proposals/unions)

- [What’s new in C# 15](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-15)

 
 

Category

Topics

## Author

![Bill Wagner](https://secure.gravatar.com/avatar/00bb9d2d7b9025659d5d3ed66cd2ebf9?s=96&r=g)

C# / .NET Principal Content developer

Bill Wagner writes the docs for https://docs.microsoft.com/dotnet/csharp. His team is responsible for all the .NET content on docs.microsoft.com. He's also a member of the C# standardization committee.