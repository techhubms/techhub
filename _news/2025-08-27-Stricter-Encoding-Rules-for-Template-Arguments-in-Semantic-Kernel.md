---
layout: "post"
title: "Stricter Encoding Rules for Template Arguments in Semantic Kernel"
description: "This news post details a significant update to Semantic Kernel, introducing stricter encoding rules for template arguments. Developers must now manually encode properties of complex types and adjust configuration to ensure secure template rendering when working with .NET and Python Semantic Kernel SDKs, thus minimizing security risks."
author: "Dmytro Struk"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/semantic-kernel/encoding-changes-for-template-arguments-in-semantic-kernel/"
viewing_mode: "external"
feed_name: "Microsoft Semantic Kernel Blog"
feed_url: "https://devblogs.microsoft.com/semantic-kernel/feed/"
date: 2025-08-27 05:17:18 +00:00
permalink: "/2025-08-27-Stricter-Encoding-Rules-for-Template-Arguments-in-Semantic-Kernel.html"
categories: ["AI", "Coding"]
tags: [".NET", "AI", "AllowDangerouslySetContent", "Automatic Encoding", "Coding", "Exception Handling", "Handlebars", "HttpUtility.HtmlEncode", "Liquid", "Manual Encoding", "News", "Prompt Templates", "Python", "SDK Update", "Semantic Kernel", "Template Encoding", "Template Injection"]
tags_normalized: ["dotnet", "ai", "allowdangerouslysetcontent", "automatic encoding", "coding", "exception handling", "handlebars", "httputilitydothtmlencode", "liquid", "manual encoding", "news", "prompt templates", "python", "sdk update", "semantic kernel", "template encoding", "template injection"]
---

Dmytro Struk explains recent encoding changes in Semantic Kernel, showing developers how to securely handle template arguments in .NET and Python. The article includes practical guidance and code examples.<!--excerpt_end-->

# Stricter Encoding Rules for Template Arguments in Semantic Kernel

Semantic Kernel's latest release changes how template argument encoding works. Previously, if a template argument was a simple string, encoding was performed automatically. However, for complex or custom types (objects, collections, anonymous types), encoding was skipped, which could potentially introduce security vulnerabilities.

## What Changed?

- **Automatic encoding** is now stricter: Passing complex types with automatic encoding enabled (the default) throws an exception.
- This enforces secure template usage—developers must manually encode complex type properties and explicitly allow 'dangerous' content when they have sufficiently sanitized input.
- The update applies to both the .NET and Python SDKs for Semantic Kernel, impacting scenarios where templating engines like Handlebars and Liquid are used.

## Why This Matters

- **Security:** Automatic encoding for strings helps prevent template injection. For complex types, automatic processing could have missed nested properties, leading to risks. Now, the SDK strictly requires the developer to handle encoding manually for these cases.

## Upgrading Your Code

### .NET Example (Before)

```csharp
var arguments = new KernelArguments()
{
    { "customer", new { firstName = userInput.FirstName, lastName = userInput.LastName } }
};

var templateFactory = new LiquidPromptTemplateFactory();
var promptTemplateConfig = new PromptTemplateConfig() { TemplateFormat = "liquid" };
var promptTemplate = templateFactory.Create(promptTemplateConfig);
var renderedPrompt = await promptTemplate.RenderAsync(kernel, arguments); // Now throws exception
```

### .NET Example (After)

```csharp
var arguments = new KernelArguments()
{
    { "customer", new {
        firstName = HttpUtility.HtmlEncode(userInput.FirstName),
        lastName = HttpUtility.HtmlEncode(userInput.LastName),
    } }
};

var promptTemplateConfig = new PromptTemplateConfig()
{
    TemplateFormat = "liquid",
    InputVariables = new() {
        new() { Name = "customer", AllowDangerouslySetContent = true },
    }
};
var templateFactory = new LiquidPromptTemplateFactory();
var promptTemplate = templateFactory.Create(promptTemplateConfig);
var renderedPrompt = await promptTemplate.RenderAsync(kernel, arguments); // No exception
```

### Python Example (Before)

```python
arguments = {
    "customer": {
        "first_name": user_input.first_name,
        "last_name": user_input.last_name,
    }
}
prompt_template_config = PromptTemplateConfig(template_format="handlebars")
prompt_template = HandlebarsPromptTemplate(prompt_template_config=prompt_template_config)
rendered_prompt = await prompt_template.render(kernel, arguments) # Now throws exception
```

### Python Example (After)

```python
arguments = {
    "customer": {
        "first_name": escape(user_input.first_name),
        "last_name": escape(user_input.last_name),
    }
}
prompt_template_config = PromptTemplateConfig(
    template=template,
    template_format="handlebars",
    input_variables=[InputVariable(name="customer", allow_dangerously_set_content=True)],
)
prompt_template = HandlebarsPromptTemplate(prompt_template_config=prompt_template_config)
rendered_prompt = await prompt_template.render(kernel, arguments) # No exception
```

**Key steps:**

1. Manually encode user input or property values before passing them as arguments.
2. Set `AllowDangerouslySetContent = true` (or equivalent) for any variable where you handle encoding yourself.
3. Test template outputs to confirm correct encoding and behavior with your data.

## Summary

The Semantic Kernel SDK now enforces manual encoding for complex template arguments, raising runtime exceptions when unsafe patterns are detected. Developers must explicitly sanitize input and configure template variables to permit this, greatly improving the security posture of applications leveraging Semantic Kernel.

Find more guidance in the [official documentation](https://learn.microsoft.com/en-us/semantic-kernel/concepts/prompts/) or join the [GitHub discussions](https://github.com/microsoft/semantic-kernel/discussions) for community support and feedback.

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/semantic-kernel/encoding-changes-for-template-arguments-in-semantic-kernel/)
