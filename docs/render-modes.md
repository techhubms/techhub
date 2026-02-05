# Render Mode Strategy

This document defines when to use Blazor's different render modes: Static SSR vs Interactive Server.

## Background

Blazor supports multiple render modes:

| Mode | Description | SignalR Required | Use Case |
|------|-------------|------------------|----------|
| **Static SSR** | Renders on server, sends HTML | No | Display + simple JS interactions |
| **Interactive Server** | Renders interactively via SignalR | Yes | Server logic or additional data needed |
| **Interactive Server (prerender: true)** | Prerenders during SSR, then hydrates | Yes | Interactive + SEO (use PersistentComponentState) |
| **Interactive Server (prerender: false)** | Only renders after SignalR connects | Yes | JS interop on render |

## When to Use Each Mode

### Static SSR (No Render Mode Attribute)

Use for components that **don't require server-side logic or additional data** on interaction:

- Navigation links, headers, footers
- Content display (article body, headings, paragraphs)
- Static images and videos
- Breadcrumbs, metadata display
- **Simple interactive elements** (toggles, accordions, dropdowns) - handle with JavaScript

```razor
@* No @rendermode attribute - static SSR *@
<NavHeader />
<PageHeader />
<Footer />
<Accordion />  @* Uses JavaScript for toggle behavior *@
```

**When to use JavaScript for interactivity:**

If the interaction is purely client-side (show/hide, toggle classes, DOM manipulation), use Static SSR with JavaScript:

```html
<!-- Static SSR component with JS interaction -->
<button onclick="document.getElementById('details').classList.toggle('hidden')">
    Toggle Details
</button>
<div id="details" class="hidden">...</div>
```

This avoids SignalR overhead and provides instant user feedback.

### Interactive Server with Prerender

Use **only** when the component requires **server-side logic or additional data** on interaction:

- Tag cloud filters that trigger server-side data queries
- Form submissions that validate/process on the server
- Components that fetch additional data based on user actions
- Content grids that require server-side filtering/sorting

```razor
@rendermode @(new InteractiveServerRenderMode(prerender: true))
```

**Why prerender: true?**

- Component HTML is included in the initial SSR response
- Content is visible immediately (better perceived performance, SEO)
- Hydrates to become interactive when SignalR connects

**CRITICAL: Avoid Double Data Loading**

When using `prerender: true`, data is loaded during SSR AND during hydration. Use `PersistentComponentState` to load data once:

```csharp
@inject PersistentComponentState ApplicationState

private List<Item> items = new();

protected override async Task OnInitializedAsync()
{
    // Try to restore data from SSR
    if (!ApplicationState.TryTakeFromJson<List<Item>>("items", out var restored))
    {
        // Data not in state - load it (during SSR)
        items = await LoadItemsAsync();
        
        // Persist for hydration
        ApplicationState.RegisterOnPersisting(() =>
        {
            ApplicationState.PersistAsJson("items", items);
            return Task.CompletedTask;
        });
    }
    else
    {
        // Data restored from SSR - reuse it (during hydration)
        items = restored;
    }
}
```

This ensures data is loaded **once** during SSR and reused during hydration.

### Interactive Server without Prerender (Rare)

Use only when the component **must not render until SignalR is established**:

- Components that immediately call JavaScript interop on render
- Components with side effects that shouldn't run twice

```razor
@rendermode @(new InteractiveServerRenderMode(prerender: false))
```

**Warning**: Components with `prerender: false` will show nothing during SSR. This causes layout shift and poor user experience.

## JavaScript Interop Disposal

Components using JavaScript interop (via `IJSObjectReference`) must handle disposal safely to avoid errors when Blazor Server circuits disconnect.

**Problem**: When users navigate away, close the browser, or experience network issues, the SignalR circuit disconnects. If `DisposeAsync()` is called on a JS module reference after disconnection, it throws `JSDisconnectedException`, which gets logged as an unhandled error.

**Solution**: Wrap ALL JavaScript interop disposal calls in try-catch blocks:

```csharp
public async ValueTask DisposeAsync()
{
    if (jsModule != null)
    {
        try
        {
            // Both cleanup calls and disposal can fail after circuit disconnect
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

**Key Points**:

- Catch exceptions from BOTH `InvokeVoidAsync()` AND `DisposeAsync()`
- Circuit disconnections are normal (navigation, browser close, network issues)
- Swallowing these specific disposal exceptions is safe and expected
- See [ContentItemsGrid.razor](../src/TechHub.Web/Components/ContentItemsGrid.razor) for a complete example

## SignalR Message Size Considerations

When using `prerender: true`, the component's state is serialized and sent over SignalR during hydration. Large prerendered content can exceed SignalR's default 32KB message limit.

**Symptom**: Console error "The maximum message size of 32768B was exceeded"

**Solution**: Increase the SignalR message size limit in Program.cs:

```csharp
builder.Services.AddSignalR(options =>
{
    options.MaximumReceiveMessageSize = 256 * 1024; // 256KB
});
```

## Current Component Configuration

| Component | Render Mode | Reason |
|-----------|-------------|--------|
| `SidebarTagCloud` | InteractiveServer (prerender: true) | Buttons need click handlers; must show in SSR for SEO |
| `ContentItemsGrid` | InteractiveServer (prerender: true) | Responds to tag filter changes; must show initial content |
| `NavHeader` | Static SSR | Links only, no interactivity needed |
| `PageHeader` | Static SSR | Display only |
| `SidebarToc` | Static SSR | Links only, no interactivity needed |
| `Footer` | Static SSR | Display only |

## Testing Interactive Components

When testing components with `PersistentComponentState` (used for SSR → Interactive hydration), use bUnit's built-in support:

```csharp
// In your test setup
this.AddBunitPersistentComponentState();
```

This ensures the `PersistentComponentState` service is properly registered for testing prerendered components.

## Decision Flowchart

```text
Does the interaction require server-side logic or additional data?
├── No → Use Static SSR + JavaScript (no @rendermode attribute)
│         Examples: toggles, accordions, show/hide, DOM manipulation
│
└── Yes → Does the component need to be visible in initial page load?
    ├── Yes → Use InteractiveServer with prerender: true
    │         ⚠️  MUST use PersistentComponentState to avoid double data loading
    │
    └── No → Does it call JS interop immediately on render?
        ├── Yes → Use InteractiveServer with prerender: false
        └── No → Use InteractiveServer with prerender: true
                  ⚠️  MUST use PersistentComponentState to avoid double data loading
```

## Best Practices

1. **Default to Static SSR + JavaScript** - Use for simple interactions (toggles, show/hide)
2. **Use Interactive Server only when needed** - Server logic or additional data required
3. **Always use PersistentComponentState with prerender: true** - Prevents double data loading
4. **Prefer prerender: true over false** - Better UX and SEO
5. **Watch message sizes** - Monitor SignalR payload sizes for interactive components
6. **Test hydration** - Ensure components load data once and work correctly after hydration

## Implementation Reference

- **Blazor component patterns**: [src/TechHub.Web/AGENTS.md](../src/TechHub.Web/AGENTS.md)
- **Component tests**: [tests/TechHub.Web.Tests/AGENTS.md](../tests/TechHub.Web.Tests/AGENTS.md)
