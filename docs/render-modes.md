# Render Mode Strategy

This document defines TechHub's Blazor render mode architecture: global InteractiveServer with prerendering.

## Architecture

TechHub uses **global InteractiveServer** render mode with prerendering enabled. This means:

- All components run in InteractiveServer mode (set at the router level in `Routes.razor`)
- Prerendering is enabled by default, so initial page loads render complete HTML on the server
- After prerendering, the SignalR circuit connects and the app becomes interactive
- No per-component `@rendermode` directives are needed

```razor
@* Routes.razor - global render mode *@
<Router AppAssembly="typeof(Program).Assembly"
        @rendermode="InteractiveServer">
```

```razor
@* App.razor - HeadOutlet also interactive *@
<HeadOutlet @rendermode="InteractiveServer" />
```

## Why Global InteractiveServer?

| Benefit | Description |
|---------|-------------|
| **Simplified architecture** | No per-component render mode decisions |
| **Consistent interactivity** | All components are interactive by default |
| **SEO preserved** | Prerendering ensures search engines see complete HTML |
| **No mixed-mode complexity** | Eliminates SSR boundary issues, parameter serialization problems |

## Component Lifecycle: `OnInitializedAsync` vs `OnParametersSetAsync`

Blazor components have two key lifecycle methods for loading data:

| Method | When It Runs | Use For |
|--------|--------------|---------|
| `OnInitializedAsync` | **Once** per component instance | Initial data loading, one-time setup |
| `OnParametersSetAsync` | On **every** parameter set (initial + every re-render) | Reacting to actual parameter changes |

### Rule of Thumb

> **Put data loading in `OnInitializedAsync`.** Only use `OnParametersSetAsync` when the component must react to parameter changes (e.g., route parameters changing while the component stays mounted).

### Why This Matters

With global InteractiveServer and prerendering, `OnParametersSetAsync` runs on every render cycle. While `PersistentComponentState.TryTakeFromJson` guards the hydration re-render (since it's one-shot), any subsequent re-render would bypass that guard and trigger duplicate API calls. Using `OnInitializedAsync` avoids this entirely because it runs only once.

### The `previous*` Change-Tracking Pattern

When a component **must** react to parameter changes (e.g., route parameters that change while the component stays mounted), use `previous*` tracking fields:

1. **Declare tracking fields** for each parameter that triggers data reloading
2. **Initialize them in `OnInitializedAsync`** from the current parameter values
3. **Compare in `OnParametersSetAsync`** and only reload when values actually change
4. **Update tracking fields** after loading new data

```csharp
// Step 1: Declare tracking fields
private string? _previousSectionName;
private string? _previousSlug;

protected override async Task OnInitializedAsync()
{
    // Step 2: Initialize tracking fields from current parameters
    _previousSectionName = SectionName;
    _previousSlug = Slug;

    // Initial data load
    await LoadData();
}

protected override async Task OnParametersSetAsync()
{
    // Step 3: Only reload when parameters actually changed
    if (SectionName != _previousSectionName || Slug != _previousSlug)
    {
        // Step 4: Update tracking fields
        _previousSectionName = SectionName;
        _previousSlug = Slug;

        await LoadData();
    }
}
```

**Why initialize tracking fields?** Without initialization, tracking fields default to `null`. If a parameter arrives as `""` (empty string), the comparison `"" != null` evaluates to `true`, causing a false change detection and a spurious data reload.

### Components Using This Pattern

| Component | Tracking Fields | Triggers Reload On |
|-----------|----------------|-------------------|
| `ContentItemsGrid.razor` | `previousCollectionName`, `previousFilterTags`, `previousSearchQuery`, `previousFromDate`, `previousToDate` | Collection, filter, search, or date change |
| `ContentItem.razor` | `_previousSectionName`, `_previousCollectionName`, `_previousSlug` | Route parameter change |
| `GenAI.razor` | `_previousPageType` | Route change between genai-basics/applied/advanced |
| `SidebarTagCloud.razor` | `_previousFromDate`, `_previousToDate` | Date range filter change |
| `DateRangeSlider.razor` | `_previousFromDate`, `_previousToDate` | External date parameter change |

### Pages That Only Use `OnInitializedAsync`

Pages without changing parameters put all data loading in `OnInitializedAsync` and do not need `OnParametersSetAsync` at all:

- `Home.razor`
- `GitHubCopilotLevels.razor`
- `GitHubCopilotFeatures.razor`
- `GitHubCopilotHandbook.razor`
- `AISDLC.razor`
- `DXSpace.razor`

## PersistentComponentState (Critical Pattern)

When prerendering is enabled, components render **twice**: once during SSR and once when the SignalR circuit connects. Use `PersistentComponentState` to persist data from SSR and restore it during hydration, preventing duplicate API calls.

**Pattern**:

```csharp
@implements IDisposable
@inject PersistentComponentState ApplicationState

private MyData? data;
private PersistingComponentStateSubscription? _persistSubscription;

protected override async Task OnInitializedAsync()
{
    _persistSubscription = ApplicationState.RegisterOnPersisting(PersistState);

    if (ApplicationState.TryTakeFromJson<MyData>("state-key", out var restored) && restored != null)
    {
        data = restored;
        return;
    }

    // Only executes during SSR prerender - data persisted for hydration
    data = await ApiClient.LoadDataAsync();
}

private Task PersistState()
{
    if (data != null)
    {
        ApplicationState.PersistAsJson("state-key", data);
    }
    return Task.CompletedTask;
}

public void Dispose()
{
    _persistSubscription?.Dispose();
    GC.SuppressFinalize(this);
}
```

**State key conventions**:

- Use descriptive, unique keys: `"home-data"`, `"content-item-{Section}-{Collection}-{Slug}"`
- For pages with route parameters, include them in the key to avoid stale data
- Use a sealed `PersistedXxxData` class when persisting multiple fields

**Pages using PersistentComponentState**:

| Page | State Key Pattern | Data Persisted |
|------|-------------------|----------------|
| `Home.razor` | `home-data` | Sections list |
| `ContentItem.razor` | `content-item-{Section}-{Collection}-{Slug}` | Content detail |
| `ContentItemsGrid.razor` | `grid-state-{Section}-{Collection}` | Grid items |
| `DXSpace.razor` | `dxspace-data` | Section + page data |
| `GitHubCopilotFeatures.razor` | `copilot-features-data` | Page + video items |
| `GitHubCopilotHandbook.razor` | `copilot-handbook-data` | Page data |
| `GitHubCopilotLevels.razor` | `copilot-levels-data` | Page data |
| `GitHubCopilotVSCodeUpdates.razor` | `vscode-updates-{VideoSlug}` | Video items + detail |
| `GenAI.razor` | `genai-{PageType}` | Section + page data |
| `AISDLC.razor` | `ai-sdlc-data` | Section + page data |

## RendererInfo.IsInteractive Guard

During prerendering, JavaScript interop is not available. Use `RendererInfo.IsInteractive` to guard JS calls in `OnAfterRenderAsync`:

```csharp
protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (!RendererInfo.IsInteractive) return;

    if (firstRender)
    {
        // Safe to call JS interop here
        await JSRuntime.InvokeVoidAsync("initComponent");
    }
}
```

## HttpContext During Prerendering

`HttpContext` is only available during prerendering (SSR pass), not after hydration. For components that need HTTP context (setting status codes, reading headers):

```csharp
@inject IHttpContextAccessor HttpContextAccessor

// Available during prerender, null during interactive
var httpContext = HttpContextAccessor.HttpContext;
if (httpContext is { Response.StatusCode: 200 })
{
    httpContext.Response.StatusCode = 404;
}
```

## JavaScript Interop Disposal

Components using JavaScript interop must handle disposal safely to avoid errors when SignalR circuits disconnect:

```csharp
public async ValueTask DisposeAsync()
{
    if (jsModule != null)
    {
        try
        {
            await jsModule.InvokeVoidAsync("dispose");
            await jsModule.DisposeAsync();
        }
        catch
        {
            // Ignore disposal errors - circuit may be disconnected
        }
    }

    dotNetRef?.Dispose();
    GC.SuppressFinalize(this);
}
```

## SignalR Message Size

Large prerendered content can exceed SignalR's default 32KB message limit. The limit is configured in `Program.cs`:

```csharp
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 256 * 1024; // 256KB
});
```

## URL State Management

Parent pages manage URL state through `NavigationManager.GetUriWithQueryParameters`:

```csharp
private void UpdateUrl()
{
    var queryParams = new Dictionary<string, object?> { ... };
    var url = Navigation.GetUriWithQueryParameters(queryParams);
    Navigation.NavigateTo(url, new NavigationOptions { ReplaceHistoryEntry = true });
}
```

Child filter components (SidebarSearch, SidebarTagCloud, DateRangeSlider) emit `EventCallback` events. Parent pages handle these events and update the URL.

## Testing Interactive Components

For bUnit tests with `PersistentComponentState`:

```csharp
this.AddBunitPersistentComponentState();
```

## Implementation Reference

- **Component patterns**: [src/TechHub.Web/AGENTS.md](../src/TechHub.Web/AGENTS.md)
- **Component tests**: [tests/TechHub.Web.Tests/AGENTS.md](../tests/TechHub.Web.Tests/AGENTS.md)
- **Reference implementation**: [ContentItemsGrid.razor](../src/TechHub.Web/Components/ContentItemsGrid.razor)
