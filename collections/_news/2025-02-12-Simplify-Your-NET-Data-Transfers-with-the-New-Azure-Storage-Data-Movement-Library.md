---
external_url: https://devblogs.microsoft.com/azure-sdk/simplify-your-net-data-transfers-with-the-new-azure-storage-data-movement-library/
title: Simplify Your .NET Data Transfers with the New Azure Storage Data Movement Library
author: Charles Barnett
feed_name: Microsoft DevBlog
date: 2025-02-12 17:34:48 +00:00
tags:
- .NET
- Azure Files
- Azure Identity
- Azure SDK
- Azure Storage
- Blob Storage
- C#
- Data Movement
- Data Transfer
- Library Migration
- Microsoft Entra
- NuGet Packages
- Storage
- TransferManager
section_names:
- azure
- coding
primary_section: coding
---
Charles Barnett introduces the new Azure Storage Data Movement library for .NET, highlighting its advanced features and modernization, and offering practical guidance for developers.<!--excerpt_end-->

# Simplify Your .NET Data Transfers with the New Azure Storage Data Movement Library

_Authored by Charles Barnett_

We’re excited to announce the release of the modern Azure Storage Data Movement library, designed to simplify your data transfer experience when using Azure Blob Storage and Azure Files. This new library, along with extension libraries for Azure Storage and Azure Files, is now available as of February 11, 2025. This post explores its benefits and guides you on how to get started.

## Why Use the New and Improved Data Movement Library?

### Convenience with High Performance

The Azure Storage Data Movement library is optimal for uploading, downloading, and copying blobs and files to, from, and between storage accounts. Notable features include:

- Setting the number of parallel operations per transfer
- Tracking transfer progress
- Pause/resume functionality
- Checkpointing support

Compared to standard Azure Storage client libraries, the Data Movement library is tailored for advanced transfer scenarios such as directory-level movement, resource control, and large-scale data movement. Unlike standalone tools like AzCopy or PowerShell scripts, this .NET package integrates directly into your applications, whether on-premises or in the cloud.

### Modernization

The new Data Movement libraries utilize the same infrastructure as modern, v12 Azure Storage libraries, which simplifies transfer and copy operations. They also integrate seamlessly with the Azure Identity libraries, ensuring up-to-date security practices.

The legacy Microsoft.Azure.Storage.DataMovement library will retire in March 2026. The new release adds support for future Azure Storage features and services, aiding a smooth migration.

## Getting Started with the Azure Storage Data Movement Library

The modern suite includes:

- The [common client library](https://www.nuget.org/packages/Azure.Storage.DataMovement)
- Extension libraries for [Azure Blob Storage](https://www.nuget.org/packages/Azure.Storage.DataMovement.Blobs) and [Azure Files](https://www.nuget.org/packages/Azure.Storage.DataMovement.Files.Shares)

The common library provides core data transfer functionality, while each extension adds features for specific storage types.

### Authentication

Microsoft Entra token-based authentication, via the Azure Identity library, is recommended for secure access. Assign the "Azure Storage Data Contributor" or a higher role for the operations in your app.

### Installation

Install these NuGet packages:

```shell
dotnet add package Azure.Storage.DataMovement.Blobs
dotnet add package Azure.Identity
```

### Using Directives

Add these at the top of your code file:

```csharp
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Storage.DataMovement;
using Azure.Storage.DataMovement.Blobs;
```

### The `TransferManager` Object

`TransferManager` is the main class for managing uploads, downloads, and copies. To create an instance:

```csharp
TransferManager transferManager = new(new TransferManagerOptions());
```

### Creating a `StorageResource` for Azure Blob Storage

The `StorageResource` class represents storage resources, such as blobs or files. For Blob Storage:

```csharp
// Create a token credential
DefaultAzureCredential tokenCredential = new();
BlobsStorageResourceProvider blobsProvider = new(tokenCredential);
// Get container resource
StorageResource container = await blobsProvider.FromContainerAsync(
    new Uri("http://<storage-account-name>.blob.core.windows.net/sample-container")
);
// Get a block blob resource
StorageResource blockBlob = await blobsProvider.FromBlobAsync(
    new Uri("http://<storage-account-name>.blob.core.windows.net/sample-container/sample-block-blob"),
    new BlockBlobStorageResourceOptions()
);
```

## Example: Upload a Local File to a Blob

To upload, use `TransferManager.StartTransferAsync()`:

```csharp
DefaultAzureCredential tokenCredential = new();
TransferManager transferManager = new(new TransferManagerOptions());
BlobsStorageResourceProvider blobsProvider = new(tokenCredential);
string localFilePath = "C:/path/to/file.txt";
string blobUri = "https://<storage-account-name>.blob.core.windows.net/sample-container/sample-blob";
TransferOperation transferOperation = await transferManager.StartTransferAsync(
    sourceResource: LocalFilesStorageResourceProvider.FromFile(localFilePath),
    destinationResource: await blobsProvider.FromBlobAsync(new Uri(blobUri))
);
await transferOperation.WaitForCompletionAsync();
```

This code initiates a transfer, using unique transfer IDs to support resuming and pausing operations.

## Conclusion

The new Azure Storage Data Movement library offers .NET developers a powerful, convenient way to manage data transfers involving Azure Blob Storage and Azure Files. With significant enhancements, users of the legacy library are encouraged to migrate.

## Resources

- [Transfer data with the Data Movement library](https://aka.ms/DataMovementLibrary)
- [Azure Storage Data Movement Common library on GitHub](https://aka.ms/DMLCommon)
- [Azure.Storage.DataMovement.Blobs Documentation](https://aka.ms/DMLBlobDoc)
- [Azure.Storage.DataMovement.Files.Shares documentation](https://aka.ms/DMLFilesDoc)
- [Migration guide: v1 to v2](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/storage/Azure.Storage.DataMovement/MigrationGuide.md)

For feature requests or bug reports, [open an issue](https://github.com/Azure/azure-sdk-for-net/issues) in the GitHub repository.

This post appeared first on "Microsoft DevBlog". [Read the entire article here](https://devblogs.microsoft.com/azure-sdk/simplify-your-net-data-transfers-with-the-new-azure-storage-data-movement-library/)
