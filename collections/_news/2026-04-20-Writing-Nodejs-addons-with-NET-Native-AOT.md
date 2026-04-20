---
author: Drew Noakes
title: Writing Node.js addons with .NET Native AOT
date: 2026-04-20 20:00:00 +00:00
tags:
- .NET
- .NET Publish
- AI
- ArrayPool
- C#
- C# Dev Kit
- Developer Stories
- DevOps
- Function Pointers
- GitHub Copilot
- Interop
- LibraryImport
- Microsoft.Win32.Registry
- N API
- Native Aot
- NativeAOT
- NativeLibrary.SetDllImportResolver
- News
- Node API
- Node Gyp
- Node.js
- Node.js Addons
- P/Invoke Source Generation
- Python Dependency
- Span<T>
- Stackalloc
- String Marshalling
- TypeScript
- UnmanagedCallersOnly
- UTF 8
- VS Code
- VS Code Extension
- Windows Registry
feed_name: Microsoft .NET Blog
external_url: https://devblogs.microsoft.com/dotnet/writing-nodejs-addons-with-dotnet-native-aot/
section_names:
- ai
- devops
- dotnet
- github-copilot
primary_section: github-copilot
---

Drew Noakes explains how the C# Dev Kit team replaced C++ Node.js addons with a .NET Native AOT shared library, showing how to build a Node-API (N-API) addon in C# with exported entry points, P/Invoke via LibraryImport, and practical string marshalling between JavaScript and .NET.<!--excerpt_end-->

# Writing Node.js addons with .NET Native AOT

[C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) is a VS Code extension. Like all VS Code extensions, its front end is TypeScript running in Node.js.

For platform-specific tasks (for example, reading the Windows Registry), the team historically used native Node.js addons written in C++ and compiled via [node-gyp](https://github.com/nodejs/node-gyp) during installation.

That approach worked, but it created friction:

- Building the addons required an old Python version on every developer machine.
- New contributors had to install tooling they would never touch directly.
- CI pipelines had to provision and maintain extra dependencies.

Since the C# Dev Kit team already has the .NET SDK installed, the post explores using C# plus **Native AOT** to produce a native library that Node.js can load.

## How Node.js addons work

A Node.js native addon is a shared library:

- Windows: `.dll`
- Linux: `.so`
- macOS: `.dylib`

When Node.js loads the library, it calls the exported function `napi_register_module_v1`. The addon registers exported functions, and JavaScript can then call them like any other module.

This is enabled by **N-API** (also called Node-API), a stable ABI-compatible C API:

- N-API doesn’t care what language built the shared library.
- It only requires the right exported symbols and correct calls.
- Native AOT can produce shared libraries with arbitrary native entry points, which fits N-API’s needs.

The rest of the post walks through a small Native AOT addon that reads a string value from the Windows Registry.

## The project file

Minimal project configuration:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <PublishAot>true</PublishAot>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
  </PropertyGroup>
</Project>
```

Key properties:

- [`PublishAot`](https://learn.microsoft.com/dotnet/core/deploying/native-aot/) produces a native shared library on publish.
- `AllowUnsafeBlocks` is required due to function pointers and fixed buffers used for N-API interop.

## The module entry point

Node expects `napi_register_module_v1` to be exported. In C#, this can be done using [`[UnmanagedCallersOnly]`](https://learn.microsoft.com/dotnet/api/system.runtime.interopservices.unmanagedcallersonlyattribute):

```csharp
public static unsafe partial class RegistryAddon
{
    [UnmanagedCallersOnly(
        EntryPoint = "napi_register_module_v1",
        CallConvs = [typeof(CallConvCdecl)])]
    public static nint Init(nint env, nint exports)
    {
        Initialize();

        RegisterFunction(
            env, exports,
            "readStringValue"u8,
            &ReadStringValue);

        // Register additional functions...

        return exports;
    }
}
```

Notable C# features in use:

- [`nint`](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/nint-nuint) as a native-sized integer for N-API handles.
- The [`u8` suffix](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/reference-types#utf-8-string-literals) to produce UTF-8 bytes without extra encoding/allocation.
- `[UnmanagedCallersOnly]` to export the method with a specific entry point name and calling convention.

`RegisterFunction` maps a native function pointer to a property on the JavaScript `exports` object so that calling `addon.readStringValue(...)` invokes the C# method in-process.

## Calling N-API from .NET

N-API functions are exported by the host process (`node.exe`), so the addon needs to resolve imports against the host.

The post uses [`[LibraryImport]`](https://learn.microsoft.com/dotnet/standard/native-interop/pinvoke-source-generation) with library name `"node"`, plus a resolver that redirects `"node"` imports to the host process via [`NativeLibrary.SetDllImportResolver`](https://learn.microsoft.com/dotnet/api/system.runtime.interopservices.nativelibrary.setdllimportresolver):

```csharp
private static void Initialize()
{
    NativeLibrary.SetDllImportResolver(
        System.Reflection.Assembly.GetExecutingAssembly(),
        ResolveDllImport);

    static nint ResolveDllImport(
        string libraryName,
        Assembly assembly,
        DllImportSearchPath? searchPath)
    {
        if (libraryName is not "node") return 0;

        return NativeLibrary.GetMainProgramHandle();
    }
}
```

With that in place, N-API methods can be declared using source-generated P/Invoke:

```csharp
private static partial class NativeMethods
{
    [LibraryImport("node", EntryPoint = "napi_create_string_utf8")]
    internal static partial Status CreateStringUtf8(
        nint env,
        ReadOnlySpan<byte> str,
        nuint length,
        out nint result);

    [LibraryImport("node", EntryPoint = "napi_create_function")]
    internal static unsafe partial Status CreateFunction(
        nint env,
        ReadOnlySpan<byte> utf8name,
        nuint length,
        delegate* unmanaged[Cdecl]<nint, nint, nint> cb,
        nint data,
        out nint result);

    [LibraryImport("node", EntryPoint = "napi_get_cb_info")]
    internal static unsafe partial Status GetCallbackInfo(
        nint env,
        nint cbinfo,
        ref nuint argc,
        Span<nint> argv,
        nint* thisArg,
        nint* data);

    // ... other N-API functions as needed
}
```

Registering a function maps a named JavaScript export to a native callback:

```csharp
private static unsafe void RegisterFunction(
    nint env,
    nint exports,
    ReadOnlySpan<byte> name,
    delegate* unmanaged[Cdecl]<nint, nint, nint> callback)
{
    NativeMethods.CreateFunction(env, name, (nuint)name.Length, callback, 0, out nint fn);
    NativeMethods.SetNamedProperty(env, exports, name, fn);
}
```

The post highlights that `[LibraryImport]` handles marshalling (for example, `ReadOnlySpan<byte>` mapping cleanly to `const char*`) and that the generated code is trimming-compatible.

## Marshalling strings

Much of the interop is moving strings between JavaScript (UTF-8 via N-API) and .NET.

### Reading a string argument from JavaScript

```csharp
private static unsafe string? GetStringArg(nint env, nint cbinfo, int index)
{
    nuint argc = (nuint)(index + 1);
    Span<nint> argv = stackalloc nint[index + 1];
    NativeMethods.GetCallbackInfo(env, cbinfo, ref argc, argv, null, null);

    if ((int)argc <= index) return null;

    // Ask N-API for the UTF-8 byte length
    NativeMethods.GetValueStringUtf8(env, argv[index], null, 0, out nuint len);

    // Allocate a buffer
    int bufLen = (int)len + 1;
    byte[]? rented = null;
    Span<byte> buf = bufLen <= 512
        ? stackalloc byte[bufLen]
        : (rented = ArrayPool<byte>.Shared.Rent(bufLen));

    try
    {
        fixed (byte* pBuf = buf)
            NativeMethods.GetValueStringUtf8(env, argv[index], pBuf, len + 1, out _);

        return Encoding.UTF8.GetString(buf[..(int)len]);
    }
    finally
    {
        if (rented is not null) ArrayPool<byte>.Shared.Return(rented);
    }
}
```

Approach:

- Ask N-API for the byte length
- Allocate on stack for small strings; rent from `ArrayPool<byte>` for larger ones
- Decode UTF-8 bytes to a .NET `string`

### Returning a string to JavaScript

```csharp
private static nint CreateString(nint env, string value)
{
    int byteCount = Encoding.UTF8.GetByteCount(value);

    byte[]? rented = null;
    Span<byte> buf = byteCount <= 512
        ? stackalloc byte[byteCount]
        : (rented = ArrayPool<byte>.Shared.Rent(byteCount));

    try
    {
        Encoding.UTF8.GetBytes(value, buf);
        NativeMethods.CreateStringUtf8(
            env,
            buf[..byteCount],
            (nuint)byteCount,
            out nint result);

        return result;
    }
    finally
    {
        if (rented is not null) ArrayPool<byte>.Shared.Return(rented);
    }
}
```

The post emphasizes using `Span<T>`, `stackalloc`, and `ArrayPool` to avoid heap allocations for typical string sizes.

## Implementing an exported function

Example exported function to read a string value from the Windows Registry:

```csharp
[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
private static nint ReadStringValue(nint env, nint info)
{
    try
    {
        var keyPath = GetStringArg(env, info, 0);
        var valueName = GetStringArg(env, info, 1);

        if (keyPath is null || valueName is null)
        {
            ThrowError(env, "Expected two string arguments: keyPath, valueName");
            return 0;
        }

        using var key = Registry.CurrentUser.OpenSubKey(keyPath, writable: false);

        return key?.GetValue(valueName) is string value
            ? CreateString(env, value)
            : GetUndefined(env);
    }
    catch (Exception ex)
    {
        ThrowError(env, $"Registry read failed: {ex.Message}");
        return 0;
    }
}
```

Important note: an unhandled exception inside an `[UnmanagedCallersOnly]` method can crash the host process, so exceptions are caught and forwarded to JavaScript via `ThrowError`.

## Calling the addon from TypeScript

1. Build the platform-specific shared library via `dotnet publish`.
2. Rename output to end with `.node` (Node treats `.node` paths as native addons).

TypeScript interface example:

```typescript
interface RegistryAddon {
  readStringValue(keyPath: string, valueName: string): string | undefined;

  // Declare additional functions...
}
```

Loading and calling from TypeScript:

```typescript
// Load our native module
const registry = require('./native/win32-x64/RegistryAddon.node') as RegistryAddon

// Call our native function
const sdkPath = registry.readStringValue(
  'SOFTWARE\\dotnet\\Setup\\InstalledVersions\\x64\\sdk',
  'InstallLocation')
```

While the example registry addon is Windows-only, the same Native AOT + N-API technique works on Windows, Linux, and macOS.

## Existing libraries

The post points to [node-api-dotnet](https://github.com/microsoft/node-api-dotnet) as a higher-level option for .NET/JavaScript interop.

Why the post uses a thin wrapper instead:

- Only needed a handful of functions
- Wanted full control over the interop layer
- Avoided extra dependencies

If you need to expose entire .NET classes or handle callbacks from JavaScript into .NET, the library is suggested as a good fit.

## What the team gained

- Removed the need for a specific Python version during install.
- Simplified onboarding and CI dependency management.
- Performance comparable to the C++ implementation for the discussed workload (string marshalling + registry access).

Trade-offs mentioned:

- .NET brings a GC and somewhat larger memory footprint.
- In a long-running VS Code extension process, that overhead was described as negligible.

The post also notes a longer-term possibility: hosting some .NET workloads in-process (as shared libraries) rather than out-of-process via IPC, potentially reducing serialization and process-management overhead.

## Footnote: GitHub Copilot

The author notes that GitHub Copilot helped the team quickly build a proof-of-concept for integrating with N-API calling conventions and interop wiring.

## Summary

Native AOT expands where .NET code can run. In this case it allowed the team to consolidate tooling, reduce dependencies, and streamline contributor experience by building a Node.js native addon in C# and shipping it as a Native AOT shared library.


[Read the entire article](https://devblogs.microsoft.com/dotnet/writing-nodejs-addons-with-dotnet-native-aot/)

