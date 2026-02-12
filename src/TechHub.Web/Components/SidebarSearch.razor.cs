using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace TechHub.Web.Components;

/// <summary>
/// Search box component for sidebar content filtering.
/// Implements debounced text search with clear functionality.
/// Emits search query changes via callback; parent handles URL state.
/// </summary>
public sealed partial class SidebarSearch : ComponentBase, IDisposable
{
    private const int DebounceDelayMs = 300;

    /// <summary>
    /// Current search query from URL parameter.
    /// </summary>
    [Parameter]
    public string? SearchQuery { get; set; }

    /// <summary>
    /// Event callback when search query changes after debounce delay.
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
            await InvokeAsync(async () =>
            {
                await OnSearchQueryChanged.InvokeAsync(_searchQueryInternal);
                StateHasChanged();
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
        
        await OnSearchQueryChanged.InvokeAsync(string.Empty);
    }

    public void Dispose()
    {
        _debounceTimer?.Dispose();
        GC.SuppressFinalize(this);
    }
}
