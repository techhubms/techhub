---
layout: "post"
title: "Exploring Passkey Support in ASP.NET Core Identity with .NET 10 Preview 6"
description: "Andrew Lock explores the addition of passkey support in ASP.NET Core Identity and the Blazor Web App template in .NET 10 preview 6, outlining user-facing changes, implementation details, and underlying code. The post highlights template updates, WebAuthn browser integration, and changes in the Microsoft Identity system."
author: "Andrew Lock"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://andrewlock.net/exploring-dotnet-10-preview-features-6-passkey-support-for-aspnetcore-identity/"
viewing_mode: "external"
feed_name: "Andrew Lock's Blog"
feed_url: "https://andrewlock.net/rss.xml"
date: 2025-08-05 10:00:00 +00:00
permalink: "/blogs/2025-08-05-Exploring-Passkey-Support-in-ASPNET-Core-Identity-with-NET-10-Preview-6.html"
categories: ["Coding", "Security"]
tags: [".NET 10", "ASP.NET Core", "ASP.NET Core Identity", "Blazor", "Coding", "EF Core", "FIDO2", "JavaScript Integration", "Microsoft Identity", "Passkey", "Passwordless Authentication", "Blogs", "Security", "Template Changes", "User Authentication", "WebAuthn"]
tags_normalized: ["dotnet 10", "aspdotnet core", "aspdotnet core identity", "blazor", "coding", "ef core", "fido2", "javascript integration", "microsoft identity", "passkey", "passwordless authentication", "blogs", "security", "template changes", "user authentication", "webauthn"]
---

In this comprehensive post, Andrew Lock examines the new passkey support introduced in ASP.NET Core Identity and the Blazor Web App template as part of .NET 10 preview 6, explaining both user and implementation perspectives.<!--excerpt_end-->

# Exploring Passkey Support in ASP.NET Core Identity with .NET 10 Preview 6

**Author:** Andrew Lock

## Introduction

This post reviews the recently introduced passkey support in ASP.NET Core Identity with .NET 10 preview 6. The focus is on how the Blazor Web App template changes to support passkeys, what user and developer experiences are like, and the implementation steps, especially those relating to WebAuthn interactions with browsers.

> **Note:** The features discussed are based on .NET 10 preview 6 and may change prior to the final .NET 10 release.

## What Are Passkeys?

[Passkeys](https://fidoalliance.org/passkeys/) offer a secure, passwordless method to authenticate with websites and apps. Relying on [FIDO (Fast IDentity Online)](https://fidoalliance.org/) standards, passkeys let users sign in with biometrics or a PIN, similar to unlocking a laptop or phone. Passkeys are more secure than passwords, but usability can be a challenge, especially when sharing across devices ([reference](https://arstechnica.com/security/2024/12/passkey-technology-is-elegant-but-its-most-definitely-not-usable-security/)).

In .NET 10 preview 6, ASP.NET Core introduces passkey support as an alternative login path in apps using ASP.NET Core Identity. However, passwords are not completely replaced: users must initially register with a password but can later add a passkey for easier login.

> _Personal note:_ Mandatory passwords somewhat defeat the purpose of true passwordless security, as the persistent presence of a password still exposes users to phishing risks.

## Trying Out the New Template

The new passkey feature was added via [this PR](https://github.com/dotnet/aspnetcore/pull/62112), enhancing ASP.NET Core Identity with passkey abstractions and the Blazor Web App template with passkey UI and logic. The PR notes this is not a full WebAuthn/FIDO2 library—its API is intentionally limited for long-term stability, though extensibility is possible, especially around attestation statement validation. Community feedback might further evolve these APIs.

For a more full-featured alternative, consider [the Fido2 library](https://github.com/passwordless-lib/fido2-net-lib), compatible with .NET 8+ and integrable with ASP.NET Core’s built-in functionality for additional features.

To experiment with passkeys:

1. Ensure you’re using .NET 10 preview 6 or later.
2. Create a new Blazor Web App with individual authentication:

   ```bash
   dotnet new blazor -au Individual
   ```

3. Run the application with `dotnet run` or directly from your IDE.
4. Register a new user via the **Register** page. No visible changes appear at this stage.
5. Confirm your account and navigate to the login page.

> In the default template, users still create a password for their account, even if passkeys are used later. This is a template decision, not a strict ASP.NET Core Identity requirement.

1. Once logged in, visit the account page where a new **Passkeys** section is available for passkey registration.

   - Clicking **Add a new passkey** triggers a browser dialog to initiate registration, possibly integrating with password managers (e.g., 1Password) or native browser options (such as Windows Hello).
   - There are options for cross-device sign-in, e.g., using a nearby phone’s biometrics, described further [here](https://www.passkeycentral.org/design-guidelines/optional-patterns/cross-device-sign-in).

2. After registering, name your passkey (default name is “Unnamed passkey”) and continue.

3. The management page now allows adding, renaming, or removing passkeys.

4. Test the login flow:
   - Use **Log in with a passkey** on the login page—browser prompts allow selecting a saved passkey.
   - Device authentication is required (e.g., Windows Hello face recognition), after which the user is logged in passwordlessly.

> To delete passkeys saved on Windows, go to Windows Settings > Accounts > Passkeys, or use
> `ms-settings:savedpasskeys`.

**Current Limitations:**

- Passkeys currently cannot be used as a second factor (MFA) or as a full password replacement in the template.

## Code Changes Overview

All code references are from the Blazor Web App template in .NET 10 preview 6. Some features have changed in later releases ([see updates](https://github.com/dotnet/aspnetcore/pull/62530)).

### New/Modified UI Components

- **Components/Account/Shared/PasskeySubmit.razor & PasskeySubmit.razor.js**: Provides the UI and browser interactions for passkey submit/registration.
- **Login.razor**: Adds the "log in with a passkey" link.
- **ManageNavMenu.razor**: Navigation update for passkeys menu item.
- **Manage/Passkeys.razor**: Passkey management page.
- **Manage/RenamePasskey.razor**: Handles passkey renaming.

### Backend Changes

- New endpoints in `IdentityComponentsEndpointRouteBuilderExtensions`—used by Blazor components for Identity integration.
- New EF Core migration for a database table `AspNetUserPasskeys` to store passkey credentials.

### PasskeySubmit Component & JavaScript

The `PasskeySubmit` Razor component is a form submit button with a custom element (`passkey-submit`).

Key JavaScript functions:

- Registers the custom element and attaches to the form.
- Handles submission (creation/request of passkey), using `navigator.credentials` API for WebAuthn communication.
- Functions for creating (`createCredential()`) and requesting (`requestCredential()`) passkeys integrate with Identity API endpoints.
- Autofill support and clean abort handling improve UX.

#### Example: Creating a Passkey

```js
async function createCredential(signal) {
    const optionsResponse = await fetchWithErrorHandling('/Account/PasskeyCreationOptions', { method: 'POST', signal });
    const optionsJson = await optionsResponse.json();
    const options = PublicKeyCredential.parseCreationOptionsFromJSON(optionsJson);
    return await navigator.credentials.create({ publicKey: options, signal });
}
```

#### Example: Requesting Passkey for Login

```js
async function requestCredential(email, mediation, signal) {
    const optionsResponse = await fetchWithErrorHandling(`/Account/PasskeyRequestOptions?username=${email}`, { method: 'POST', signal });
    const optionsJson = await optionsResponse.json();
    const options = PublicKeyCredential.parseRequestOptionsFromJSON(optionsJson);
    return await navigator.credentials.get({ publicKey: options, mediation, signal });
}
```

#### Supporting Helper

```js
async function fetchWithErrorHandling(url, options = {}) {
    const response = await fetch(url, { credentials: 'include', ...options });
    if (!response.ok) {
        const text = await response.text();
        console.error(text);
        throw new Error(`The server responded with status ${response.status}.`);
    }
    return response;
}
```

### API Endpoints

**Passkey Creation:**

```csharp
accountGroup.MapPost("/PasskeyCreationOptions", async (...)
```

- Creates and returns passkey challenge/options based on the logged-in user.
- Utilizes `SignInManager.ConfigurePasskeyCreationOptionsAsync()` (renamed in later previews) to prepare and return a JSON payload for the client.

Sample JSON (abbreviated):

```json
{
  "rp": { "name": "localhost", "id": "localhost" },
  "user": { "id": "...", "name": "[email protected]", "displayName": "[email protected]" },
  ...
}
```

**Passkey Request (Login):**

```csharp
accountGroup.MapPost("/PasskeyRequestOptions", async (...)
```

- Returns login challenge/options for a user (username optional).

### Database Schema Change

Stores registered passkeys in `AspNetUserPasskeys` with columns: `CredentialId (BLOB)`, `UserId (TEXT)`, and `Data (TEXT)`.

### Server-Side Integration: Handling Passkey Addition

1. Retrieve passkey creation options via `SignInManager`.
2. Validate credentials using `PerformPasskeyAttestationAsync()`.
3. Persist the passkey with `UserManager.SetPasskeyAsync()`.
4. Redirect user to name the credential.

Example method outline:

```csharp
private async Task AddPasskey() {
    var options = await SignInManager.RetrievePasskeyCreationOptionsAsync();
    var attestationResult = await SignInManager.PerformPasskeyAttestationAsync(Input.CredentialJson, options);
    if (!attestationResult.Succeeded) { /* handle error */ }
    var setPasskeyResult = await UserManager.SetPasskeyAsync(user, attestationResult.Passkey);
    if (!setPasskeyResult.Succeeded) { /* handle error */ }
    // Redirect to rename page
}
```

## Summary

- Passkey support in .NET 10 preview 6 modernizes ASP.NET Core Identity.
- The Blazor Web App template includes new UI and backend logic for registering, managing, and logging in with passkeys.
- Full passwordless scenarios are not yet realized, but the foundation is solid.
- The actual implementation is in flux as .NET 10 nears final release.

---

**About the Author:**
Andrew Lock writes detailed explorations of new features in the Microsoft stack on his blog, .Net Escapades.

Stay updated with his latest posts by subscribing to notifications.

This post appeared first on "Andrew Lock's Blog". [Read the entire article here](https://andrewlock.net/exploring-dotnet-10-preview-features-6-passkey-support-for-aspnetcore-identity/)
