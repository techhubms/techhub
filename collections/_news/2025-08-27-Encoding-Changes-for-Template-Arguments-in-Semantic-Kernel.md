---
layout: "post"
title: "Encoding Changes for Template Arguments in Semantic Kernel"
description: "This article details recent updates to Microsoft Semantic Kernel's template argument encoding, explaining stricter rules that now require developers to manually encode complex argument types. It covers the security rationale, provides .NET and Python code examples showing before-and-after handling, and guides developers on updating code for compliance with the new SDK behaviors."
author: "Dmytro Struk"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://devblogs.microsoft.com/agent-framework/encoding-changes-for-template-arguments-in-semantic-kernel/"
viewing_mode: "external"
feed_name: "Microsoft Semantic Kernel Blog"
feed_url: "https://devblogs.microsoft.com/semantic-kernel/feed/"
date: 2025-08-27 05:17:18 +00:00
permalink: "/2025-08-27-Encoding-Changes-for-Template-Arguments-in-Semantic-Kernel.html"
categories: ["AI", "Coding"]
tags: [".NET", "AI", "AllowDangerouslySetContent", "Argument Encoding", "Coding", "Escaping", "Handlebars", "Liquid", "Manual Encoding", "Microsoft", "News", "PromptTemplateConfig", "Python", "SDK Update", "Security Best Practices", "Semantic Kernel", "Template Engine", "Template Injection Prevention", "Template Rendering"]
tags_normalized: ["dotnet", "ai", "allowdangerouslysetcontent", "argument encoding", "coding", "escaping", "handlebars", "liquid", "manual encoding", "microsoft", "news", "prompttemplateconfig", "python", "sdk update", "security best practices", "semantic kernel", "template engine", "template injection prevention", "template rendering"]
---

Dmytro Struk explains recent changes to Semantic Kernel’s template argument encoding, illustrating with .NET and Python updates. Learn why manual encoding of complex types is now enforced for security.<!--excerpt_end-->

# Encoding Changes for Template Arguments in Semantic Kernel

Recent versions of Semantic Kernel—Microsoft's SDK for integrating AI capabilities—have updated how template arguments are processed when rendering prompts via templating engines like Handlebars or Liquid.

## Stricter Encoding Enforcement

Previously, template argument encoding was automatic for `string` types, but not for custom or complex types (anonymous objects, collections, etc.). The new update introduces stricter enforcement:

- **Automatic encoding** remains the default, but will now throw an exception if you pass a complex type as an argument.
- This change is aimed at preventing potential template injection vulnerabilities, since the SDK cannot safely determine how to encode nested or structured data.

## Why This Change Was Made

Automatic encoding provides basic protection against template injection attacks, escaping characters in user input. However, complex arguments introduce uncertainty because the SDK can't encode nested properties reliably. To avoid silent security holes, developers are now required to:

1. **Manually encode** any properties within complex types
2. **Explicitly disable automatic encoding** for these arguments by setting `AllowDangerouslySetContent` to `true` in the template configuration

## Updating Your Code

### .NET Example

Before the update:

```csharp
var arguments = new KernelArguments() {
    { "customer", new { firstName = userInput.FirstName, lastName = userInput.LastName } }
};
var templateFactory = new LiquidPromptTemplateFactory();
var promptTemplateConfig = new PromptTemplateConfig() { TemplateFormat = "liquid" };
var promptTemplate = templateFactory.Create(promptTemplateConfig);
// Now throws exception because complex type is used with automatic encoding
await promptTemplate.RenderAsync(kernel, arguments);
```

After the update, manually encode properties and disable auto-encoding:

```csharp
var arguments = new KernelArguments() {
    {
        "customer", new {
            firstName = HttpUtility.HtmlEncode(userInput.FirstName),
            lastName = HttpUtility.HtmlEncode(userInput.LastName),
        }
    }
};
var templateFactory = new LiquidPromptTemplateFactory();
var promptTemplateConfig = new PromptTemplateConfig() {
    TemplateFormat = "liquid",
    InputVariables = new() {
        new() { Name = "customer", AllowDangerouslySetContent = true }
    }
};
var promptTemplate = templateFactory.Create(promptTemplateConfig);
// No exception; encoding is now controlled manually
await promptTemplate.RenderAsync(kernel, arguments);
```

### Python Example

Before:

```python
arguments = {
    "customer": { "first_name": user_input.first_name, "last_name": user_input.last_name }
}
prompt_template_config = PromptTemplateConfig(template_format="handlebars")
prompt_template = HandlebarsPromptTemplate(prompt_template_config=prompt_template_config)

# Throws exception with complex arguments

await prompt_template.render(kernel, arguments)
```

After:

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
    input_variables=[InputVariable(name="customer", allow_dangerously_set_content=True)]
)
prompt_template = HandlebarsPromptTemplate(prompt_template_config=prompt_template_config)

# No exception; encoding is manual

await prompt_template.render(kernel, arguments)
```

## Takeaways for Developers

- **Always manually encode complex arguments** before rendering templates.
- **Explicitly flag variables** for which you’ve handled encoding by setting `AllowDangerouslySetContent` (or equivalent).
- Test your outputs to ensure encoded data renders as intended and isn’t over-escaped or left unprotected.

For more information or to join discussions, see [the Semantic Kernel documentation](https://learn.microsoft.com/en-us/semantic-kernel/concepts/prompts/) or visit the [GitHub discussions](https://github.com/microsoft/semantic-kernel/discussions).

This post appeared first on "Microsoft Semantic Kernel Blog". [Read the entire article here](https://devblogs.microsoft.com/agent-framework/encoding-changes-for-template-arguments-in-semantic-kernel/)
