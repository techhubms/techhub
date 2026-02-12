using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.WebUtilities;

namespace TechHub.Web.Components;

/// <summary>
/// Search box component for sidebar content filtering.
/// Implements debounced text search with clear functionality.
/// Updates URL directly since parent pages use static SSR.
/// </summary>
public sealed partial class SidebarSearch : ComponentBase, IDisposable
{
    private const int DebounceDelayMs = 300;
    
    [Inject]
    private NavigationManager Navigation { get; set; } = default!;
    
    /// <summary>
    /// Current search query from URL parameter.
    /// </summary>
    [Parameter]
    public string? SearchQuery { get; set; }

    /// <summary>
    /// Event callback when search query changes after debounce delay.
    /// This is still invoked for backward compatibility, but the component
    /// handles URL navigation directly.
    /// </summary>
    [Parameter]
    public EventCallback<string> OnSearchQueryChanged { get; set; }

    private string _searchQueryInternal = string.Empty;
    private Timer? _debounceTimer;

    protected override void OnParametersSet()
    {
        // Initialize internal state from parameter
        _searchQueryInternal = SearchQuery ?? string.Empty;
    }

    private void HandleInput(ChangeEventArgs e)
    {
        var newQuery = e.Value?.ToString() ?? string.Empty;
        _searchQueryInternal = newQuery;

        // Cancel previous timer
        _debounceTimer?.Dispose();
        _debounceTimer = null;

        // Create new debounce timer
        _debounceTimer = new Timer(async _ =>
        {
            await InvokeAsync(() =>
            {
                // Update URL directly from this interactive component.
                // The parent page (Section/SectionCollection) is static SSR and cannot
                // call NavigateTo during the interactive phase.
                UpdateUrl(_searchQueryInternal);
                
                // Also invoke callback for backward compatibility
                OnSearchQueryChanged.InvokeAsync(_searchQueryInternal);
            });
        }, null, DebounceDelayMs, Timeout.Infinite);
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Escape")
        {
            await ClearSearch();
        }
    }

    private async Task ClearSearch()
    {
        _searchQueryInternal = string.Empty;
        
        // Cancel pending debounce timer
        if (_debounceTimer is not null)
        {
            await _debounceTimer.DisposeAsync();
            _debounceTimer = null;
        }
        
        // Update URL immediately
        UpdateUrl(string.Empty);
        
        // Also invoke callback for backward compatibility
        await OnSearchQueryChanged.InvokeAsync(string.Empty);
    }

    /// <summary>
    /// Updates the browser URL with search query parameter while preserving
    /// other query parameters (e.g., tags, from, to).
    /// </summary>
    private void UpdateUrl(string searchQuery)
    {
        var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
        var basePath = uri.AbsolutePath;

        // Parse existing query parameters so we can preserve them
        var existingParams = QueryHelpers.ParseQuery(uri.Query);
        var queryParams = new Dictionary<string, string?>();

        foreach (var kvp in existingParams)
        {
            // Skip search parameter - we'll add our own if needed
            if (!string.Equals(kvp.Key, "search", StringComparison.OrdinalIgnoreCase))
            {
                queryParams[kvp.Key] = kvp.Value.ToString();
            }
        }

        // Only add search parameter if there's a query
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            queryParams["search"] = searchQuery;
        }

        var newUrl = QueryHelpers.AddQueryString(basePath, queryParams);
        Navigation.NavigateTo(newUrl, new NavigationOptions 
        { 
            ReplaceHistoryEntry = true,
            ForceLoad = false  // Use enhanced navigation to preserve focus
        });
    }

    public void Dispose()
    {
        _debounceTimer?.Dispose();
        GC.SuppressFinalize(this);
    }
}
