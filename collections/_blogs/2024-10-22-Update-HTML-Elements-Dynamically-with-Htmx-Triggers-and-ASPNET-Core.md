---
external_url: https://khalidabuhakmeh.com/update-html-elements-with-htmx-triggers-and-aspnet-core
title: Update HTML Elements Dynamically with Htmx Triggers and ASP.NET Core
author: Khalid Abuhakmeh
viewing_mode: external
feed_name: Khalid Abuhakmeh's Blog
date: 2024-10-22 00:00:00 +00:00
tags:
- .NET
- ASP.NET
- ASP.NET Core
- Avatar Update
- C#
- Dynamic UI
- Htmx
- HX Trigger
- Partial Views
- Razor Pages
- User Profile
- Web Development
section_names:
- coding
---
In this post, Khalid Abuhakmeh walks through using Htmx's HX-Trigger headers with ASP.NET Core Razor Pages to seamlessly update UI elements like avatars upon profile changes, backed by practical code examples and integration tips.<!--excerpt_end-->

# Update HTML Elements Dynamically with Htmx Triggers and ASP.NET Core

*Photo by [Julian Hochgesang](https://unsplash.com/@julianhochgesang)*

## Introduction

Khalid Abuhakmeh shares how combining [Htmx](https://htmx.org) with ASP.NET Core opens up powerful possibilities for .NET web developers, specifically around making UI elements update in response to user actions without requiring a full page reload. The article demonstrates updating a user's avatar image—using Htmx's `HX-Trigger` header and ASP.NET Core Razor Pages—as a concrete example.

## Overview and Motivation

Khalid was inspired by a conversation with Illya Busigin on Mastodon, discussing the `hx-trigger` and `HX-Trigger` header technique in ASP.NET Core and how it can be used to update UI parts such as avatars dynamically when the user updates their profile. The post explores this pattern step-by-step with practical code.

## Components Involved

To achieve the desired dynamic UI updates, several application components are outlined:

- **User Profile store:** Holds user information, like name and avatar URL.
- **Profile Settings endpoints:** For display and profile update handling.
- **Avatar refresh endpoint:** Enables the page to re-render only the avatar section upon updates.

The demonstration uses ASP.NET Core Razor Pages and Razor Views to create and manage HTML snippets that Htmx swaps in/out as needed.

### Note on Dependencies

- NuGet packages required: **Htmx**, and optionally **Htmx.TagHelpers**.

## Implementing the User Service

A simple C# class named `UserService` is used for this demo:

```csharp
public class UserService {
    public static readonly string[] AvatarUrls = [
        "~/img/avatar_one.png",
        "~/img/avatar_two.png",
        "~/img/avatar_three.png",
    ];

    public string Name { get; set; } = "Khalid Abuhakmeh";
    public string AvatarUrl { get; set; } = AvatarUrls[0];
}
```

Three avatars are available in `wwwroot/img` for simplicity (more could be added in a full system). The service is registered as a Singleton in the demo for illustrative purposes:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<UserService>();

var app = builder.Build();
```

With this setup, other components (such as pages and partials) can inject the service and access the current user's state.

## Integrating into Razor Pages

Injection is simple in Razor views, e.g., in `Layout.cshtml`:

```razor
@inject UserService UserService
@await Html.PartialAsync("_Avatar", UserService)
```

## Profile Endpoints

A new `Index` Razor page serves three endpoints:

- Displaying the profile form
- Accepting / processing updates
- Rendering just the avatar as a partial for dynamic updates

Here is the class walkthrough:

```csharp
using System.Diagnostics.CodeAnalysis;
using Htmx;
using HtmxAvatarChange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HtmxAvatarChange.Pages;

public class IndexModel(UserService userService, ILogger<IndexModel> logger) : PageModel {
    [BindProperty]
    public string? Name { get; set; }

    [BindProperty]
    public string? AvatarUrl { get; set; }

    [TempData]
    public string? Message { get; set; }
    [TempData]
    public string? MessageCssClass { get; set; }

    [MemberNotNullWhen(true, nameof(Message))]
    public bool HasMessage => Message != null;

    public List<SelectListItem> Avatars => UserService
        .AvatarUrls
        .Select((x, i) => new SelectListItem($"avatar-{i:00}", x))
        .ToList();

    public void OnGet() {
        Name = userService.Name;
        AvatarUrl = userService.AvatarUrl;
    }

    public IActionResult OnPost() {
        if (ModelState.IsValid) {
            Message = "Successfully saved account settings";
            MessageCssClass = "alert-success";
            userService.Name = Name!;
            userService.AvatarUrl = AvatarUrl!;
            Response.Htmx(h => h.WithTrigger("avatar"));
        } else {
            Message = "Failed to save account settings";
            MessageCssClass = "alert-danger";
        }

        if (Request.IsHtmx()) {
            return Partial("_Form", this);
        }
        return RedirectToPage("Index");
    }

    public IActionResult OnGetAvatar() {
        return Partial("_Avatar", userService);
    }

    public string? IsCurrentAvatar(string avatarValue) => avatarValue == AvatarUrl ? "checked" : null;
}
```

### Endpoint Highlights

- **OnGet:** Populates Razor properties from the service.
- **OnPost:** Validates form input, updates the profile, triggers Htmx's client-side "avatar" event (via header), and returns appropriate partials for Htmx-aware requests.
- **OnGetAvatar:** Returns the partial view for the avatar, enabling dynamic refresh upon `avatar` event.

## Razor Form with Htmx Integration

The account settings form uses Htmx attributes to take over form submission and manage UI updates:

```razor
@model IndexModel

<fieldset id="account-settings">
    <legend>Account Settings</legend>

    @if (Model.HasMessage) {
        <div class="alert @(Model.MessageCssClass ?? "alert-info")" role="alert">
            @Model.Message
        </div>
    }
    <form method="post" hx-post hx-target="#account-settings" hx-swap="outerHTML">
        <div class="form-group row">
            <label asp-for="Name" class="col-sm-2 col-form-label">Name</label>
            <div class="col-sm-10">
                <input class="form-control" asp-for="Name">
            </div>
        </div>
        <fieldset class="form-group mt-3">
            <div class="row">
                <legend class="col-form-label col-sm-2 pt-0">Avatar</legend>
                <div class="col-sm-10">
                    @foreach (var avatar in Model.Avatars) {
                        <div class="form-check">
                            <input id="@avatar.Text" asp-for="AvatarUrl" class="form-check-inline" type="radio" value="@avatar.Value" checked="@Model.IsCurrentAvatar(avatar.Value)">
                            <label class="form-check-label" for="@avatar.Text">
                                <img src="@Url.Content(avatar.Value)" class="profile-pic" alt="@avatar.Text"/>
                            </label>
                        </div>
                    }
                </div>
            </div>
        </fieldset>
        <div class="form-group row mt-3">
            <button type="submit" class="btn btn-primary">Save Profile</button>
        </div>
    </form>
</fieldset>
```

- `hx-post`, `hx-target`, and `hx-swap` allow the form submission to be handled by Htmx, enabling partial updates.

## Partial View for Avatar with Htmx Trigger

The `_Avatar` partial contains the magic Htmx attribute:

```razor
@model HtmxAvatarChange.Models.UserService

<div id="profile-avatar" class="mx-2 smooth" hx-get="@Url.Page("Index", "Avatar")" hx-trigger="avatar from:body">
    <div class="profile-pic">
        <img src="@Url.Content(Model.AvatarUrl)" alt="Profile Picture">
    </div>
    <span class="navbar-text">
        @Model.Name
    </span>
</div>
```

- The key is `hx-trigger="avatar from:body"`: it listens for the `avatar` event (triggered by the response header) broadcast to `body`, which then pulls the updated avatar from the server.

## How it Works

1. User submits the profile form.
2. On valid input, the backend sets the `HX-Trigger` header with value `avatar`.
3. Htmx on the client listens for this event and automatically fetches updated data for any element subscribed with `hx-trigger="avatar from:body"`.
4. Only the avatar element is refreshed, without a full page reload.

## Result in Action

![](https://github.com/khalidabuhakmeh/HtmxAvatarChange/raw/main/misc/htmx-hx-trigger-sample.gif)

## Conclusion and Considerations

Using `HX-Trigger` headers and Htmx, you can create modular, decoupled UI components in ASP.NET Core that refresh independently—improving interactivity without the complexity of a full SPA. However, each trigger results in a server request, with potential performance implications to consider.

For code samples and a runnable demo, visit [the GitHub repository](https://github.com/khalidabuhakmeh/HtmxAvatarChange).

---

**About the Author**

Khalid Abuhakmeh is a developer advocate at JetBrains, specializing in .NET technologies and tooling.

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/update-html-elements-with-htmx-triggers-and-aspnet-core)
