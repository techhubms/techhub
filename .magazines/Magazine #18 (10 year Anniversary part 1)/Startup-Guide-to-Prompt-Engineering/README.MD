
<img src="./images/StartupGuidetoPromptEngineering.jpg" width="900">

# Startup Guide to Prompt Engineering Using GitHub Copilot

* By Randy Pagels

## Introduction
Prompt engineering is an essential skill for developers seeking to maximize the utility of GitHub Copilot. By structuring effective prompts, developers can receive accurate, context-aware suggestions that save time and enhance productivity.

This article explores foundational and advanced techniques to harness GitHub Copilot for writing code, debugging, and testing in C#, a language widely used in software development.

## Core Concepts in Prompt Engineering

<div style="display: flex; align-items: flex-start; margin-bottom: 20px;">
  <img src="./images/CoreConceptsinPromptEngineering.jpg" width="300" style="margin-right: 15px;">
<div style="font-size: 1.1em;">
    <p><strong>Prompt engineering strategies include:</strong></p>
    <ul>
      <li><strong>One-shot prompting.</strong> Providing a single example to guide the model.</li>
      <li><strong>Single-shot prompting.</strong> Similar to one-shot, but often used interchangeably.</li>
      <li><strong>Chain-of-thought prompting.</strong> Breaking down the problem into smaller steps.</li>
    </ul>
  </div>
</div>

### One-Shot Prompting
One-shot prompting involves crafting a single, concise instruction. This method is best for straightforward tasks. It allows GitHub Copilot to generate code based on minimal input, making it efficient for simple functions or commonly known programming patterns.

**Example**: `Reverse a string`.

**Prompt**
_"Create a C# function to reverse a string."_

**Output**
```csharp
using System;

public class StringManipulation
{
    public static string ReverseString(string input)
    {
        char[] charArray = input.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}
```

A clear instruction produces a simple and effective solution.

### Single-Shot Prompting
Here, you provide an example input and desired output to guide Copilot. This technique helps Copilot understand the expected structure and format, making it particularly useful for tasks that require precise output patterns or specific conditions.

**Example**: Filter even numbers from a list.

**Prompt**
_"Given a list of integers, return a new list containing only even numbers. Example: Input: [1, 2, 3, 4] Output: [2, 4]."_

**Output**
```csharp
using System;
using System.Collections.Generic;
using System.Linq;

public class NumberFilter
{
    public static List<int> FilterEvenNumbers(List<int> numbers)
    {
        return numbers.Where(n => n % 2 == 0).ToList();
    }
}

```

This approach ensures Copilot understands your expectations.

### Chain of Thought Prompting
For complex tasks, break the problem into logical steps and guide Copilot progressively. This method helps Copilot generate more structured and accurate responses by incrementally refining the solution. It is particularly effective for algorithms, multi-step workflows, and scenarios that require sequential decision-making.

**Example**: Process user data from a CSV.

**Prompt**
_"Write a C# program to read a CSV file, filter rows based on a condition, and output the results."_

**Output**

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class CSVProcessor
{
    public static List<string[]> ReadCSV(string filePath)
    {
        return File.ReadAllLines(filePath).Select(line => line.Split(',')).ToList();
    }
    
    public static List<string[]> FilterRows(List<string[]> data)
    {
        return data.Where(row => int.Parse(row[1]) > 50).ToList();
    }
}

```
Clearly distinguish the prompt from the output.

### Zero Shot Chain of Thought Prompting
Zero-shot CoT Prompting essentially involves adding "Let's think step by step" to the original prompt.

**Example**: Process user data from a CSV.

**Prompt**
_"Determine whether a number is prime. Let's think step by step."_

**Output**

```csharp
using System;

public class PrimeChecker
{
    public static bool IsPrime(int number)
    {
        if (number < 2) return false;
        for (int i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0) return false;
        }
        return true;
    }
}
```

Breaking the solution into steps improves accuracy.

> [!NOTE]  
> Read more about Chain-of-Thought prompting here: https://www.promptingguide.ai/techniques/cot


## Writing Effective Prompts

<div style="display: flex; align-items: flex-start;">
  <img src="./images/WritingEffectivePrompts.jpg" width="300" style="margin-right: 10px;">
<p style="font-size: 1.2em;">Clear prompts are the foundation of accurate outputs. Ambiguity leads to irrelevant or incomplete code. A well-structured prompt ensures that GitHub Copilot understands the developer's intent, leading to more precise and useful code suggestions.
  The more specific and descriptive the prompt, the better the results, as it reduces the need for unnecessary iterations and refinements.</p>
</div>

### Examples of Effective vs. Ineffective Prompts

**Ineffective**: _"Create a function."_

- **Effective**: _"Write a C# method that accepts a list of integers and returns the sum of even numbers."_


**Ineffective**: "Sort a list."

- **Effective**: "Write a C# method that accepts a list of strings and returns the list sorted in alphabetical order."


**Ineffective**: "Generate a report."

- **Effective**: "Write a C# method that generates a sales report for a given month, including total sales, average sales per day, and the highest sales day."

These examples demonstrate how providing specific details in your prompt can lead to more accurate and useful code suggestions from GitHub Copilot.

### Multi-step Prompts
Progressively guide Copilot with incremental instructions.

**Prompt 1**
_"Create a C# web API project."_

**Output 1**

```csharp
// Create a new ASP.NET Core Web API project using the .NET CLI
// Command: dotnet new webapi -n UserManagementAPI
```

**Prompt 2**
_"Add a UsersController to manage users."_

**Output 2**
```csharp
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private static List<string> users = new List<string> { "Alice", "Bob", "Charlie" };

    [HttpGet]
    public IActionResult GetUsers()
    {
        return Ok(users);
    }
}
```
**Prompt 3**
_"Add a method to add a new user to the list."_

**Output 3**
```csharp
[HttpPost]
public IActionResult AddUser([FromBody] string user)
{
    users.Add(user);
    return Ok();
}
```
**Prompt 4**
_"Add a method to delete a user from the list."_

**Output 4**
```csharp
[HttpDelete("{name}")]
public IActionResult DeleteUser(string name)
{
    users.Remove(name);
    return Ok();
}
```

### Advanced Techniques
Here are more advanced examples using Copilot for performance optimization.

**Prompt**
_"Optimize the following C# method to reduce memory usage and improve performance."_

**Initial Code**

```csharp
public static List<int> GetSquareNumbers(List<int> numbers)
{
    List<int> squares = new List<int>();
    foreach (var num in numbers)
    {
        squares.Add(num * num);
    }
    return squares;
}
```
**Optimized Code Using LINQ**

```csharp
public static List<int> GetSquareNumbers(List<int> numbers) => numbers.Select(n => n * n).ToList();
```

Using LINQ improves performance by reducing iterations.

## Testing and Debugging

<div style="display: flex; align-items: flex-start;">
  <img src="./images/MaximizingGitHubCopilotforTestingandDebugging.jpg" width="300" style="margin-right: 10px;">
  <p>Testing and debugging are crucial in software development, and GitHub Copilot simplifies these tasks. With well-structured prompts, developers can generate unit tests, detect bugs, and refine code efficiently. Copilot automates test creation and suggests improvements, enhancing code reliability while reducing manual effort. It also helps troubleshoot runtime errors by offering fixes and explaining common mistakes. Leveraging Copilot for testing and debugging accelerates development and improves software quality by identifying issues early.</p>
</div>

**Example**
Instead of excessive code comments, use Copilot Chat for a more interactive debugging session.

**Prompt**
_"Why is my C# function returning incorrect results?"_

**Output**
- "Let's check for edge cases. Are you handling empty lists correctly?"
- "Consider adding input validation before processing."

This interactive approach is more practical than embedding lengthy comments inside the code.

### Generating Unit Tests
Copilot’s ability to generate unit tests helps developers validate their code quickly.

**Prompt**
_"Write unit tests for a method that checks if a number is prime."_

**Output**

```csharp
using System;
using Xunit;

public class PrimeCheckerTests
{
    [Fact]
    public void Test_PrimeNumbers()
    {
        Assert.True(PrimeChecker.IsPrime(5));
        Assert.True(PrimeChecker.IsPrime(11));
    }

    [Fact]
    public void Test_NonPrimeNumbers()
    {
        Assert.False(PrimeChecker.IsPrime(4));
        Assert.False(PrimeChecker.IsPrime(10));
    }
}
```

Unit tests ensure correctness and catch regressions early.

### Debugging Assistance
GitHub Copilot can assist in identifying and fixing bugs by analyzing existing code and suggesting improvements.

**Example**
Fixing a concurrency issue in a multi-threaded application.

**Prompt**
_"Fix the race condition in this C# multithreading example."_

**Buggy Code**

```csharp
using System;
using System.Threading;

class Counter
{
    private int count = 0;

    public void Increment()
    {
        count++;
    }

    public int GetCount() => count;
}
```

**Copilot Suggested Fix** (Using lock for thread safety)

```csharp
using System;
using System.Threading;

class Counter
{
    private int count = 0;
    private readonly object lockObject = new object();

    public void Increment()
    {
        lock (lockObject)
        {
            count++;
        }
    }

    public int GetCount() => count;
}
```

Using lock ensures only one thread can modify count at a time, preventing race conditions.

### Real-World Testing Scenarios
Generate mock data for testing APIs or database operations effortlessly. Ensure the prompt is specific about test data.

**Prompt**
_"Generate mock test data for a C# user registration system."_

**Output**

```csharp
using System;
using System.Collections.Generic;

public class MockUserData
{
    public static List<(string name, int age)> GenerateUsers(int count)
    {
        var users = new List<(string, int)>();
        var rand = new Random();
        
        for (int i = 0; i < count; i++)
        {
            users.Add(($"User{i}", rand.Next(18, 70)));
        }
        return users;
    }
}
```

By specifying the user registration system, Copilot provides relevant mock data.

By leveraging these techniques, GitHub Copilot becomes a powerful tool not just for writing code but for ensuring its quality and reliability.

## Summary
I hope you found this article helpful in kick starting prompt engineering with GitHub Copilot. Whether you're refining simple functions or tackling complex workflows, the right prompts can make all the difference. Keep experimenting, iterating, and optimizing your prompts for the best results. Best of luck on your coding journey!

Here are the key takeaways:
- Start with clear, specific, structured prompts for better responses and straightforward tasks.
- Leverage context, including comments and multi-step guidance, for complex workflows.
- Use Chain of Thought prompting and iterative refinement for handling complex challenges.
- Utilize Copilot Chat for debugging instead of excessive inline comments.
- Optimize performance using advanced techniques and structured refactoring.
- Incorporate testing and refactoring into your workflow to maximize Copilot’s utility.