---
external_url: https://devblogs.microsoft.com/dotnet/dotnet-10-networking-improvements/
title: .NET 10 Networking Improvements
author: Máňa
feed_name: Microsoft .NET Blog
date: 2025-12-08 18:05:00 +00:00
tags:
- .NET
- .NET 10
- Cookies
- HTTP
- IP Address
- MediaTypeNames.Yaml
- NegotiatedCipherSuite
- Net Security
- Networking
- Performance Optimization
- Query Verb
- QuicConnection
- Server Sent Events
- ServerCertificateValidationCallback
- SslStream
- TLS 1.3
- URI
- Web Sockets
- WebSockets
- WebSocketStream
- WinHttpHandler
- Security
primary_section: dotnet
section_names:
- dotnet
- security
---
Máňa outlines the significant networking improvements in .NET 10, including HTTP, WebSocket API upgrades, and security features relevant to developer scenarios.<!--excerpt_end-->

# .NET 10 Networking Improvements

Máňa presents an annual roundup of networking changes and additions introduced in .NET 10. This release focuses on HTTP and WebSocket updates, several security advancements, plus enhancements to other networking primitives.

## HTTP Enhancements

### WinHttpHandler Certificate Caching

- **Performance Optimization:** Caching validated certificates by server IP reduces redundant validation, especially for repeated requests to the same server. The opt-in via `AppContext.SetSwitch("System.Net.Http.UseWinHttpCertificateCaching", true);` delivers cumulative time savings for applications making frequent HTTP calls.
- **Code Example:**

```csharp
AppContext.SetSwitch("System.Net.Http.UseWinHttpCertificateCaching", true);
using var client = new HttpClient(new WinHttpHandler
{
    ServerCertificateValidationCallback = static (req, cert, chain, errors) => {
        Console.WriteLine("Server certificate validation invoked");
        return errors == SslPolicyErrors.None;
    }
});
Console.WriteLine((await client.GetAsync("https://github.com")).StatusCode);
```

### New HTTP Verb: QUERY

- **Purpose:** Enables safe, idempotent querying with body contents for cases where query data exceeds URI length limits. QUERY verb is set for future expansion as standardization progresses.
- **Usage Example:**

```csharp
using var client = new HttpClient();
var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Query, "https://api.example.com/resource"));
```

### Public CookieException Constructor

- **Change:** Developers can now manually throw `CookieException` for custom error handling.
- **Example:**

```csharp
throw new CookieException("🍪");
```

## WebSockets API Improvements

### WebSocketStream Abstraction

- **Benefit:** Simplifies data reads/writes via Streams, removing manual buffering and message boundary logic.
- **Common Patterns:**
  - Read JSON messages: `CreateReadableMessageStream` with `JsonSerializer.DeserializeAsync`
  - Stream text protocols: `Create`, layer with `StreamReader`
  - Write binary messages: `CreateWritableMessageStream`, chunk writes, automatic EOM signaling
- **Before vs After Example:**
  - **Previously:** Manual buffering and EOM checks
  - **Now:** Stream-based read with clear API boundaries

## Security Updates

### TLS 1.3 Support on OSX

- **Client-Side Only:** Enabled via `AppContext` switch or environment variable
- **Code Example:**

```csharp
AppContext.SetSwitch("System.Net.Security.UseNetworkFramework", true);
```

- **Limitation:** Only TLS 1.2 and 1.3 supported in this mode; opt-in for client operations

### Negotiated Cipher Suite API Unified

- **Update:** Legacy key exchange and hash properties on `SslStream` obsolete; only `NegotiatedCipherSuite` (with full TLS cipher suite info) remains as the authoritative source.
- **Extended To:** `QuicConnection.NegotiatedCipherSuite` for QUIC connections

## Networking Primitives

### Server-Sent Events Formatter

- **New API:** Helpers for formatting SSEs on the server, dual overloads for string and generic payloads, plus control over Event ID and reconnection interval. Supports full round-trip data between formatter and parser.

### IPAddress Validity & Parsing

- **Methods Added:** `IPAddress.IsValid`, `IPAddress.IsValidUtf8`
- **Interface Implementation:** `IUtf8SpanParsable<T>` for easier and faster parsing of IP addresses

### Miscellaneous Changes

- **Uri Length Limit Removed:** Data URIs (RFC 2397) can now be longer, supporting embedded resources.
- **MediaTypesName.Yaml Added:** Constant for YAML files in media type names

## Resources and Further Reading

- [Announcing .NET 10](https://devblogs.microsoft.com/dotnet/announcing-dotnet-10/)
- [Performance Improvements in .NET 10](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-10/#networking)
- [GitHub: dotnet/runtime](https://github.com/dotnet/runtime/issues)

## Acknowledgements

Máňa thanks co-author [@CarnaViire](https://github.com/CarnaViire) for contributions to the WebSockets section and the wider .NET networking community for ongoing feedback and feature requests.

This post appeared first on "Microsoft .NET Blog". [Read the entire article here](https://devblogs.microsoft.com/dotnet/dotnet-10-networking-improvements/)
