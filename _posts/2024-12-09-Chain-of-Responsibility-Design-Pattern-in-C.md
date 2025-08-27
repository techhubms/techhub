---
layout: "post"
title: "Chain of Responsibility Design Pattern in C#"
description: "This article by Ahsan Ullah explores the Chain of Responsibility design pattern, illustrating its principles with practical implementation in C#. It covers the motivation for using the pattern, detailed code examples, typical use cases, benefits, drawbacks, and best practices, including an example of refactoring for improved maintainability."
author: "Ahsan Ullah"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://code-maze.com/csharp-chain-of-responsibility-design-pattern/"
viewing_mode: "external"
feed_name: "Code Maze Blog"
feed_url: "https://code-maze.com/feed/"
date: 2024-12-09 06:56:56 +00:00
permalink: "/2024-12-09-Chain-of-Responsibility-Design-Pattern-in-C.html"
categories: ["Coding"]
tags: [".NET", "C#", "Chain Of Responsibility", "Coding", "Design Pattern", "Design Patterns", "Loose Coupling", "Middleware", "Posts", "Refactoring", "Request Handling", "Single Responsibility Principle", "Software Architecture"]
tags_normalized: ["dotnet", "csharp", "chain of responsibility", "coding", "design pattern", "design patterns", "loose coupling", "middleware", "posts", "refactoring", "request handling", "single responsibility principle", "software architecture"]
---

Ahsan Ullah explores the Chain of Responsibility design pattern in this in-depth article, demonstrating how to implement and leverage it for flexible, maintainable C# applications.<!--excerpt_end-->

# Chain of Responsibility Design Pattern in C#

_Article by Ahsan Ullah, featured on Code Maze._

---

## Introduction

In this article, we will talk about the Chain of Responsibility (CoR) Design Pattern. We will see how to implement this pattern in C# and how it can help resolve specific design challenges, especially concerning extensibility and maintainability.

[Download Source Code: CodeMaze Blog GitHub Repository](https://github.com/CodeMazeBlog/CodeMazeGuides/tree/main/csharp-design-patterns/ChainOfResponsibilityPattern)

---

## What is The Chain of Responsibility Design Pattern?

Chain of Responsibility is a behavioral pattern aiming to build a sequence of independent processing components (handlers). Each handler has a distinct responsibility, and handlers are chained together to form a pipeline for request handling. This approach enhances adherence to the Single Responsibility Principle and achieves loose coupling.

**Key Concepts:**

- **Sender:** The initiator of the request.
- **Handlers:** Components that process the request (or pass it along).
- **Chain Formation:** Handlers are sequenced using interfaces, typically `IHandler` with methods for setting the next handler (e.g., `SetNext`) and handling requests (e.g., `Handle`).

**ASP.NET Core middleware** is a practical example of this pattern, where an incoming request is passed through independently configurable middleware components.

**Workflow Types:**

- **Multi-Handler Workflow:** Multiple handlers operate on a request in sequence.
- **Single-Handler Workflow:** The appropriate handler processes the request, and the chain stops.

## Problems Solved by the Chain of Responsibility

The pattern addresses several challenges, such as:

- Avoiding monolithic, tightly coupled classes where business logic is intertwined.
- Decoupling request logic from processing steps.

**Example Problem:**
A library rental API processes a `RentalRequest`, with steps like checking availability, eligibility, balance, and finally issuance. Traditionally, this results in procedural code, which is hard to extend or modify:

```csharp
public class RentalAssistService {
    public RentalResponse ProcessRentRequest(RentalRequest request) {
        var result = CheckForBookAvailability(request.BookName);
        if (result != RentalResponse.BookAvailable) return result;
        result = CheckForMemberAccessibility(request.BookName, request.UserName);
        if (result != RentalResponse.AccessibleToUser) return result;
        result = CheckForAvailableBalance(request.UserName);
        if (result != RentalResponse.RentalApproved) return result;
        return IssueBook(request.BookName, request.UserName);
    }
    // Step implementations omitted for brevity
}
```

This approach violates the Single Responsibility Principle and increases coupling.

## Chain of Responsibility Implementation in C#

A better approach is to encapsulate each operation in an independent handler class.

### Handler Abstraction

```csharp
public abstract class HandlerBase : IHandler {
    protected IHandler? _nextHandler;
    public IHandler SetNext(IHandler nextHandler) {
        _nextHandler = nextHandler;
        return _nextHandler;
    }
    public abstract RentalResponse Handle(RentalRequest request);
}
```

### Concrete Handlers

Example: Book Availability Check

```csharp
public class BookAvailabilityCheckHandler : HandlerBase {
    public override RentalResponse Handle(RentalRequest request) {
        if (DataStore.FindBook(request.BookName) is not { } book || book.IssuedTo is not null)
            return RentalResponse.BookUnavailable;
        if (_nextHandler is null) return RentalResponse.BookAvailable;
        return _nextHandler.Handle(request);
    }
}
```

Similarly, you'd define `MemberEligibilityCheckHandler`, `UserBalanceCheckHandler`, and `RentalIssuanceHandler`.

### Orchestrating the Chain

```csharp
public RentalResponse ProcessRentRequest(RentalRequest request) {
    var handler = new BookAvailabilityCheckHandler();
    handler.SetNext(new MemberAccessibilityCheckHandler())
           .SetNext(new UserBalanceCheckHandler())
           .SetNext(new RentalIssuanceHandler());
    return handler.Handle(request);
}
```

The primary responsibility of the client is to arrange the handlers in order and invoke the chain.

### Enabling Flexible Workflows

The chain can be adjusted depending on requirements, such as skipping the book issuance step for a "validation only" operation:

```csharp
public RentalResponse AssessRentRequest(RentalRequest request) {
    var handler = new BookAvailabilityCheckHandler();
    handler.SetNext(new MemberAccessibilityCheckHandler())
           .SetNext(new UserBalanceCheckHandler());
    return handler.Handle(request);
}
```

This flexibility allows easy extension or customization without altering existing classes.

## Drawbacks and Considerations

- **Handler Order Matters:** Incorrect sequencing (e.g., validation after action) can cause logic errors.
- **Potential Complexity:** Managing state between handlers or ensuring handler independence can increase architectural complexity.
- **Unhandled Requests:** If no handler processes a request, it may remain unhandled—careful design is necessary.

## Conclusion

The Chain of Responsibility pattern streamlines request processing by delegating each step to independent handlers—improving maintainability and extensibility. The pattern is especially useful for business workflows, middleware, and cases needing dynamic handler chains. However, implementers must carefully manage dependencies, handler sequence, and request handling guarantees.

---

For further practice and to download the source code, visit the [Code Maze GitHub repository](https://github.com/CodeMazeBlog/CodeMazeGuides/tree/main/csharp-design-patterns/ChainOfResponsibilityPattern).

---

_Check out Code Maze courses for more hands-on .NET and C# architecture guidance._

This post appeared first on "Code Maze Blog". [Read the entire article here](https://code-maze.com/csharp-chain-of-responsibility-design-pattern/)
