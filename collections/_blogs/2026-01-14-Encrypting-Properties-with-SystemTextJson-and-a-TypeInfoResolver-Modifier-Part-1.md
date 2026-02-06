---
external_url: https://www.stevejgordon.co.uk/encrypting-properties-with-system-text-json-and-a-typeinforesolver-modifier-part-1
title: Encrypting Properties with System.Text.Json and a TypeInfoResolver Modifier (Part 1)
author: Steve Gordon
feed_name: Steve Gordon's Blog
date: 2026-01-14 15:09:54 +00:00
tags:
- .NET
- Attribute Based Encryption
- Azure Key Vault
- C#
- Custom Serialization
- Data Protection
- Dependency Injection
- Elasticsearch
- Encryption
- GDPR
- GitHub OAuth
- JSON
- JsonSerializer
- Minimal API
- PII Compliance
- POCO
- Serialization
- System.Text.Json
- Typeinforesolver
- Azure
- Security
- Blogs
section_names:
- azure
- dotnet
- security
primary_section: dotnet
---
Steve Gordon explains how to use System.Text.Json's TypeInfoResolver modifier for property-level encryption in C#, outlining a practical approach to securing sensitive data that paves the way for future integration with Azure Key Vault.<!--excerpt_end-->

# Encrypting Properties with System.Text.Json and a TypeInfoResolver Modifier (Part 1)

*By Steve Gordon*

In this blog post, you'll learn how to build the groundwork for encrypting and decrypting specific properties of .NET types when serializing and deserializing with System.Text.Json. The series begins with fundamental concepts and a working code sample, and sets the stage for integrating Azure Key Vault as the ultimate storage for encryption keys in future installments.

## Why Encrypt Individual Properties?

There are two primary reasons for encrypting certain fields in serialized objects:

- **Protection of Sensitive Data in Datastores**: Prevent exposure of fields like credit card numbers or OAuth tokens stored in databases (e.g., Elasticsearch) by encrypting them at rest.
- **Crypto Shredding for Compliance**: Enabling deletion of user-specific keys to make all their related PII data unreadable, useful for supporting GDPR or deleting event-sourced data.

In this example, Steve focuses on securely storing OAuth access/refresh tokens after GitHub authentication, with Elasticsearch as the JSON data store.

## Solution Design

The proposed approach uses an attribute to mark properties requiring encryption. During serialization, a custom TypeInfoResolver modifier triggers encryption for these properties; during deserialization, it handles decryption. The modifier is designed to be dependency-injectable, anticipating future use of real key management like Azure Key Vault.

### Tools and Technologies Used

- **.NET (C#)**
- **System.Text.Json** (contract customization via TypeInfoResolver)
- **ASP.NET Core minimal API**
- **Attribute-driven field marking**

## Code Walkthrough

### Defining the Attribute

```csharp
internal sealed class EncryptedData {
  [EncryptedData("AccessTokenKey")]
  public required string AccessToken { get; init; }
}
```

Currently, the key name is not used but will help select different encryption keys in later parts.

### Minimal API Setup Example

```csharp
var builder = WebApplication.CreateBuilder(args);
// Service registration omitted for brevity
var app = builder.Build();

app.MapPost("/", static ([FromServices] JsonSerializerOptions jsonOptions, [FromServices] ILogger<Program> logger, [FromBody] PlainTextDto data) => {
  var encryptedData = new EncryptedData { AccessToken = data.AccessToken };
  var encryptedDataJson = JsonSerializer.Serialize(encryptedData, jsonOptions);
  logger.LogInformation("Encrypted JSON: {Json}", encryptedDataJson);

  var decryptedData = JsonSerializer.Deserialize<EncryptedData>(encryptedDataJson, jsonOptions);
  logger.LogInformation("Decrypted access token: {AccessToken}", decryptedData.AccessToken);

  return Results.Json(new {
    EncryptedJson = encryptedDataJson,
    DecryptedAccessToken = decryptedData.AccessToken
  });
});
```

### Building the TypeInfoResolver Modifier

The `EncryptedJsonTypeInfoResolverModifier` class includes logic to:

- Detect string properties marked with the custom attribute
- Apply delegate-based encryption (append a suffix for this demo) on serialization
- Apply delegate-based decryption (remove suffix) on deserialization

```csharp
public sealed class EncryptedJsonTypeInfoResolverModifier {
    private const string EncryptionSuffix = "_encrypted";
    public void EncryptStringsModifier(JsonTypeInfo jsonTypeInfo) {
        if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object) return;
        foreach (JsonPropertyInfo jsonPropertyInfo in jsonTypeInfo.Properties) {
            if (jsonPropertyInfo.PropertyType != typeof(string)) continue;

            var attribute = jsonPropertyInfo.AttributeProvider?
                .GetCustomAttributes(typeof(EncryptedDataAttribute), false)
                .OfType<EncryptedDataAttribute>()
                .SingleOrDefault();
            if (string.IsNullOrEmpty(attribute?.KeyName)) continue;
            var propertyGet = jsonPropertyInfo.Get;
            var propertySet = jsonPropertyInfo.Set;
            // Encrypt on serialization
            jsonPropertyInfo.Get = obj => {
                var value = propertyGet(obj);
                if (value is not string plainTextValue) return string.Empty;
                return plainTextValue + EncryptionSuffix;
            };
            // Decrypt on deserialization
            jsonPropertyInfo.Set = (obj, value) => {
                if (value is string cipherText && cipherText.EndsWith(EncryptionSuffix))
                    propertySet(obj, cipherText[..^EncryptionSuffix.Length]);
                else
                    propertySet(obj, value);
            };
        }
    }
}
```

### Service Registration

```csharp
builder.Services.AddSingleton<EncryptedJsonTypeInfoResolverModifier>();
builder.Services.AddSingleton(sp => {
    var modifier = sp.GetRequiredService<EncryptedJsonTypeInfoResolverModifier>();
    return new JsonSerializerOptions {
        TypeInfoResolver = new DefaultJsonTypeInfoResolver {
            Modifiers = { modifier.EncryptStringsModifier }
        }
    };
});
```

## Result and Output Example

A POST with:

```json
{
  "accessToken": "AccessTokenValue"
}
```

Produces logs like:

```
Encrypted JSON: {"AccessToken":"AccessTokenValue_encrypted"}
Decrypted access token: AccessTokenValue
```

## Next Steps

The post concludes by indicating future posts will use Azure Key Vault to securely store and manage encryption keys, upgrading from the current placeholder encryption. This pattern is relevant for anyone interested in property-level data protection, compliance, and scalable key management in .NET applications.

This post appeared first on "Steve Gordon's Blog". [Read the entire article here](https://www.stevejgordon.co.uk/encrypting-properties-with-system-text-json-and-a-typeinforesolver-modifier-part-1)
