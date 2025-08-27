---
layout: "post"
title: "Using Entra ID Authentication with Arc-Enabled SQL Server in a .NET Windows Forms Application"
description: "This guide by jpigott demonstrates how to implement Microsoft Entra ID (formerly Azure AD) authentication in a .NET Framework Windows Forms application connecting directly to an Arc-enabled SQL Server. Covering prerequisites, MSAL integration, token management, secure connection practices, and relevant security considerations, the article provides detailed setup instructions, sample code, and references for production-ready configurations."
author: "jpigott"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/using-entra-id-authentication-with-arc-enabled-sql-server-in-a/ba-p/4435168"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Community"
date: 2025-08-15 14:31:39 +00:00
permalink: "/2025-08-15-Using-Entra-ID-Authentication-with-Arc-Enabled-SQL-Server-in-a-NET-Windows-Forms-Application.html"
categories: ["Azure", "Coding", "Security"]
tags: [".NET Framework", "Access Tokens", "API Permissions", "App Registration", "Arc Enabled SQL Server", "Authentication", "Azure", "Azure Active Directory", "Azure SQL Database", "Client App Registration", "Coding", "Community", "Managed Identity", "Microsoft Entra ID", "Microsoft.Data.SqlClient", "Microsoft.Identity.Client", "MSAL", "Security", "SQL Server", "SSMS", "Token Cache", "Windows Forms"]
tags_normalized: ["dotnet framework", "access tokens", "api permissions", "app registration", "arc enabled sql server", "authentication", "azure", "azure active directory", "azure sql database", "client app registration", "coding", "community", "managed identity", "microsoft entra id", "microsoftdotdatadotsqlclient", "microsoftdotidentitydotclient", "msal", "security", "sql server", "ssms", "token cache", "windows forms"]
---

jpigott details how developers can use Microsoft Entra ID authentication with Arc-enabled SQL Server in a .NET Windows Forms application, focusing on token management, secure connections, and MSAL integration.<!--excerpt_end-->

# Using Entra ID Authentication with Arc-Enabled SQL Server in a .NET Windows Forms Application

## Introduction

This guide demonstrates how to securely connect a .NET Framework Windows Forms application directly to an Arc-enabled SQL Server 2022 or 2025 instance using Entra ID (formerly Azure AD) authentication. The article covers user authentication, token management, and best practices for secure connections, including full code samples and detailed explanations.

## Why Direct Connection?

Most modern applications use a web service to mediate SQL Server access for improved security and scalability. However, certain scenarios—like administrative tools or specific client applications (e.g., SQL Server Management Studio)—require direct SQL Server connections. This guide covers secure strategies for such cases, leveraging Entra ID authentication for robust credential management.

## Prerequisites

- Arc-enabled SQL Server 2022/2025 configured for Entra ID authentication
- Microsoft Entra ID (Azure AD) tenant and registered application
- .NET Framework 4.6.2 Windows Forms application
- Microsoft.Identity.Client (MSAL) and Microsoft.Data.SqlClient NuGet packages

## Application Overview

- Authenticate users through Entra ID
- Safely acquire a token and use it for SQL Server connections
- Optional: Cache tokens to memory or disk for session persistence
- Fetch and display data in a DataGridView
- Similar approach can be used with SSMS and Entra ID

## User Interface & Workflow

- **Check User**: Display current user's authentication status
- **Connect to Entra ID at Login**: Verifies login and connects to SQL Server
- **Entra ID Authentication**: If not logged in, prompts for Entra ID credentials
- **Post Login**: Shows a connection success message if authentication and connection succeed
- **Load Data**: Queries the Adventure Works database to populate the data grid
- **Token Cache Option**: Choose between in-memory (session only) or disk-cached tokens (persisted between app launches; location: C:\Users\[user]\AppData\Local). For production, encrypt disk cache files for security.

## Authentication Logic (MSAL Integration)

MSAL (Microsoft Authentication Library) enables Entra ID authentication in .NET:

```csharp
private static IPublicClientApplication app = PublicClientApplicationBuilder.Create("YourApplicationClientID")
    .WithAuthority(AzureCloudInstance.AzurePublic, "YourTenantID")
    .WithRedirectUri("http://localhost")
    .WithLogging((level, message, containsPii) => Debug.WriteLine($"MSAL: {message}"), LogLevel.Verbose, true, true)
    .Build();
```

- **Login Handling:**

```csharp
private async Task<AuthenticationResult> LoginAsync(bool persistCache)
{
    if (persistCache)
        TokenCacheHelper.EnablePersistence(app.UserTokenCache);
    else
    {
        app.UserTokenCache.SetBeforeAccess(args => { });
        app.UserTokenCache.SetAfterAccess(args => { });
    }
    string[] scopes = new[] { "https://database.windows.net//.default" };
    var accounts = await app.GetAccountsAsync();
    if (accounts == null || !accounts.Any())
        return await app.AcquireTokenInteractive(scopes).ExecuteAsync();
    var account = accounts.FirstOrDefault();
    return await app.AcquireTokenSilent(scopes, account).ExecuteAsync();
}
```

## Secure SQL Server Connection with Access Token

After token acquisition, use it to connect securely:

```csharp
string connectionString = $"Server={txtSqlServer.Text};Database=AdventureWorks2019;Encrypt=True;TrustServerCertificate=True;";
var result = await LoginAsync(checkBox1.Checked);
using (var conn = new SqlConnection(connectionString))
{
    conn.AccessToken = result.AccessToken;
    conn.Open();
    // ... operate on the connection ...
}
```

**Note:**

- For production, don't use self-signed certificates. Use a CA-issued certificate and set `TrustServerCertificate=False`.
- Reference: [Configure Client Computer and Application for Encryption - SQL Server](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/special-cases-for-encrypting-connections-sql-server?view=sql-server-ver17)

## Fetch Data Example

Load data into DataGridView from SQL Server after secure authentication:

```csharp
private async Task<DataTable> FetchDataAsync()
{
    var dataTable = new DataTable();
    var result = await LoginAsync(checkBox1.Checked);
    using (var conn = new SqlConnection(connectionString))
    {
        conn.AccessToken = result.AccessToken;
        await conn.OpenAsync();
        using (var cmd = new SqlCommand("SELECT TOP (1000) [FirstName], [MiddleName], [LastName] FROM [AdventureWorks2019].[Person].[Person]", conn))
        using (var reader = await cmd.ExecuteReaderAsync())
        {
            dataTable.Load(reader);
        }
    }
    return dataTable;
}
```

## Azure Arc SQL Server Configuration for Entra ID

- SQL Server 2022: Requires configuring a Key Vault and certificates.
- SQL Server 2025: Setup is easier with managed identity for authentication, no Key Vault/certificates needed.
- Use [Set up Microsoft Entra authentication for SQL Server](https://learn.microsoft.com/en-us/sql/relational-databases/security/authentication-access/azure-ad-authentication-sql-server-setup-tutorial?view=sql-server-ver16#create-logins-and-users) for detailed instructions (covers SSMS too).

## Entra ID App Registration Steps

1. Register a new app in Azure AD.
2. Add "http://localhost" as a Redirect URI.
3. Add API permissions:
    - https://database.windows.net/.default
    - Microsoft Graph: User.Read.All, Application.Read.All, Group.Read.All
    - Azure SQL Database: user_impersonation (delegated)
    - Verify Microsoft.Sql Resource Provider is registered
4. Grant admin consent for all permissions.

## Security Best Practices

- Never store client secrets in client apps.
- Prefer in-memory token caching for security; encrypt disk cache if persisting tokens.
- Leverage user tokens for access auditing and least-privilege enforcement.

## References

- [Azure AD Authentication for SQL Server](https://learn.microsoft.com/en-us/azure/azure-sql/database/authentication-aad-overview)
- [MSAL.NET Documentation](https://learn.microsoft.com/en-us/azure/active-directory/develop/msal-overview)
- [Arc-enabled SQL Server Documentation](https://learn.microsoft.com/en-us/azure/azure-arc/data/)

## Conclusion

Configuring Entra ID authentication for Arc-enabled SQL Server in .NET client applications increases security and simplifies user credential management in modern development environments. By following this guide, developers can protect their data access while enabling efficient user sign-in and robust auditing.

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/core-infrastructure-and-security/using-entra-id-authentication-with-arc-enabled-sql-server-in-a/ba-p/4435168)
