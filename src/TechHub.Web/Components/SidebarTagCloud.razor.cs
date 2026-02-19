using Microsoft.AspNetCore.Components;
using TechHub.Core.Models;
using TechHub.Web.Services;

namespace TechHub.Web.Components;

/// <summary>
/// Defines how the tag cloud handles tag clicks
/// </summary>
public enum TagCloudNavigationMode
{
    /// <summary>
    /// Update URL query params in-place (stays on current page)
    /// Used for Section and Collection pages
    /// </summary>
    Filter,

    /// <summary>
    /// Navigate to a different URL with the tag applied
    /// Used for Homepage (→ /all) and Content Item (→ primarySection)
    /// </summary>
    Navigate
}

/// <summary>
/// Tag cloud component for sidebar filtering.
/// Displays tags with usage counts and size categories.
/// Supports multiple tag selection with OR logic.
/// Emits tag selection changes via callback; parent handles URL state.
/// </summary>
public partial class SidebarTagCloud : ComponentBase, IDisposable
{
    [Inject]
    private ITechHubApiClient ApiClient { get; set; } = default!;

    [Inject]
    private ILogger<SidebarTagCloud> Logger { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private PersistentComponentState ApplicationState { get; set; } = default!;

    /// <summary>
    /// Title for the tag cloud section
    /// </summary>
    [Parameter]
    public string Title { get; set; } = "Tags";

    /// <summary>
    /// Section name - defaults to "all" if not specified
    /// </summary>
    [Parameter]
    public string? SectionName { get; set; }

    /// <summary>
    /// Collection name - defaults to "all" if not specified
    /// </summary>
    [Parameter]
    public string? CollectionName { get; set; }

    /// <summary>
    /// Pre-selected tags (e.g., from URL parameters)
    /// </summary>
    [Parameter]
    public List<string>? SelectedTags { get; init; }

    /// <summary>
    /// Event callback fired when tag selection changes (for Filter mode)
    /// </summary>
    [Parameter]
    public EventCallback<List<string>> OnSelectionChanged { get; set; }

    /// <summary>
    /// Navigation mode: Filter (update URL params in-place) or Navigate (go to different URL)
    /// </summary>
    [Parameter]
    public TagCloudNavigationMode NavigationMode { get; set; } = TagCloudNavigationMode.Filter;

    /// <summary>
    /// Base URL for navigation mode (e.g., "/all" for homepage, "/github-copilot" for content items)
    /// Required when NavigationMode is Navigate
    /// </summary>
    [Parameter]
    public string? NavigationBaseUrl { get; set; }

    /// <summary>
    /// Maximum number of tags to display (default: 20)
    /// </summary>
    [Parameter]
    public int MaxTags { get; set; } = 20;

    /// <summary>
    /// Minimum usage count for tag inclusion (default: 1)
    /// </summary>
    [Parameter]
    public int MinUses { get; set; } = 1;

    /// <summary>
    /// Optional: Provide tags for a content item.
    /// When SectionName is also set, fetches real counts from the API using the default
    /// date range filter (90 days), so counts match what users see when clicking through.
    /// When SectionName is not set, displays tags with count=1 as fallback.
    /// </summary>
    [Parameter]
    public IReadOnlyList<string>? Tags { get; set; }

    /// <summary>
    /// Optional from-date for date range filtering (yyyy-MM-dd format)
    /// </summary>
    [Parameter]
    public string? FromDate { get; set; }

    /// <summary>
    /// Optional to-date for date range filtering (yyyy-MM-dd format)
    /// </summary>
    [Parameter]
    public string? ToDate { get; set; }

    /// <summary>
    /// Optional text search query for filtering tag counts by matching content.
    /// When provided, tag counts reflect only content items matching this search.
    /// </summary>
    [Parameter]
    public string? SearchQuery { get; set; }

    private IReadOnlyList<TagCloudItem>? _tags;
    private HashSet<string> _selectedTagsInternal = [];
    private bool _isLoading = true;
    private bool _hasError;
    private bool _hasInitialized; // Track if we've loaded tags to prevent double-load flicker
    private HashSet<string> _previousSelectedTags = []; // Track previous state to detect changes
    private string? _previousFromDate;
    private string? _previousToDate;
    private string? _previousSearchQuery;
    private string? _previousSectionName;
    private string? _previousCollectionName;
    private PersistingComponentStateSubscription? _persistSubscription;

    protected override async Task OnInitializedAsync()
    {
        // Prevent double initialization when transitioning from SSR to interactive mode
        if (_hasInitialized)
        {
            return;
        }

        _hasInitialized = true;

        _persistSubscription ??= ApplicationState.RegisterOnPersisting(PersistTagCloudState);

        // Initialize all change-tracking fields from current parameter values
        // This prevents false change detection in OnParametersSetAsync
        _previousFromDate = FromDate;
        _previousToDate = ToDate;
        _previousSearchQuery = SearchQuery;
        _previousSectionName = SectionName;
        _previousCollectionName = CollectionName;

        if (SelectedTags != null)
        {
            _selectedTagsInternal = new HashSet<string>(
                SelectedTags.Select(t => t.Trim().ToLowerInvariant()),
                StringComparer.OrdinalIgnoreCase);

            _previousSelectedTags = new HashSet<string>(_selectedTagsInternal, StringComparer.OrdinalIgnoreCase);
        }

        // Try to restore persisted state from prerender (avoids duplicate API call)
        var stateKey = GetPersistedStateKey();
        if (ApplicationState.TryTakeFromJson<PersistedTagCloudData>(stateKey, out var restored) && restored != null)
        {
            _tags = restored.Tags;
            _isLoading = false;
            Logger.LogDebug("Restored tag cloud from persisted state for key {Key}", stateKey);
            return;
        }

        await LoadTagsAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        // Always sync selectedTagsInternal with SelectedTags parameter
        // This is critical for Filter mode where URL changes trigger parameter updates
        SyncSelectedTagsFromParameter();

        // Check if section or collection changed (navigating between collections)
        var scopeChanged = !string.Equals(_previousSectionName, SectionName, StringComparison.OrdinalIgnoreCase)
            || !string.Equals(_previousCollectionName, CollectionName, StringComparison.OrdinalIgnoreCase);
        if (scopeChanged)
        {
            Logger.LogDebug("Section/collection scope changed from {PrevSection}/{PrevCollection} to {Section}/{Collection}",
                _previousSectionName, _previousCollectionName, SectionName, CollectionName);
            _previousSectionName = SectionName;
            _previousCollectionName = CollectionName;
            _previousSelectedTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // Reset change tracking
        }

        // Check if dates changed
        var datesChanged = _previousFromDate != FromDate || _previousToDate != ToDate;
        if (datesChanged)
        {
            Logger.LogDebug("Date range changed. From: {From}, To: {To}", FromDate, ToDate);
            _previousFromDate = FromDate;
            _previousToDate = ToDate;
        }

        // Check if search query changed
        var searchChanged = _previousSearchQuery != SearchQuery;
        if (searchChanged)
        {
            Logger.LogDebug("Search query changed. Query: {Query}", SearchQuery);
            _previousSearchQuery = SearchQuery;
        }

        // Reload tag cloud if selected tags, dates, search, or scope have changed
        if (scopeChanged || datesChanged || searchChanged || !_selectedTagsInternal.SetEquals(_previousSelectedTags))
        {
            Logger.LogDebug("Filters changed, reloading tag cloud. Previous tags: [{Previous}], Current: [{Current}]",
                string.Join(", ", _previousSelectedTags),
                string.Join(", ", _selectedTagsInternal));

            _previousSelectedTags = new HashSet<string>(_selectedTagsInternal, StringComparer.OrdinalIgnoreCase);
            await LoadTagsAsync();
        }
    }

    /// <summary>
    /// Synchronizes the internal selected tags set with the SelectedTags parameter.
    /// This ensures the component's internal state matches the parent component's state
    /// (which is derived from URL parameters) after URL updates.
    /// </summary>
    private void SyncSelectedTagsFromParameter()
    {
        if (SelectedTags != null)
        {
            var normalizedTags = SelectedTags
                .Select(t => t.Trim().ToLowerInvariant())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Only update if different to avoid unnecessary re-renders
            if (!_selectedTagsInternal.SetEquals(normalizedTags))
            {
                _selectedTagsInternal = normalizedTags;
                Logger.LogDebug("Synced selected tags from parameter: {Tags}",
                    string.Join(", ", _selectedTagsInternal));
            }
        }
        else if (_selectedTagsInternal.Count > 0)
        {
            // Parameter is null but we have selections - clear them
            _selectedTagsInternal.Clear();
            Logger.LogDebug("Cleared selected tags (parameter is null)");
        }
    }

    private async Task LoadTagsAsync()
    {
        try
        {
            _isLoading = true;
            _hasError = false;

            // If Tags parameter is provided, handle content item tag display
            if (Tags != null && Tags.Count > 0)
            {
                var distinctTags = Tags
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(t => t, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                // If SectionName is available, fetch real counts from API
                // so users see accurate counts matching what they'd get when clicking through.
                // The API returns these tags with real counts PLUS fills remaining slots
                // (up to MaxTags) with popular tags from the section.
                if (!string.IsNullOrWhiteSpace(SectionName))
                {
                    var sectionForTags = SectionName;
                    var collectionForTags = string.IsNullOrWhiteSpace(CollectionName) ? "all" : CollectionName;

                    Logger.LogDebug("Fetching real counts for {Count} content item tags in section {SectionName}",
                        distinctTags.Count, sectionForTags);

                    var tagCounts = await ApiClient.GetTagCloudAsync(
                        sectionForTags,
                        collectionForTags,
                        maxTags: MaxTags,
                        minUses: null,
                        lastDays: null, // Use default date range (90 days via API defaults)
                        selectedTags: null,
                        tagsToCount: distinctTags,
                        fromDate: null,
                        toDate: null,
                        searchQuery: null);

                    if (tagCounts != null && tagCounts.Count > 0)
                    {
                        _tags = tagCounts;
                        Logger.LogDebug("Loaded {Count} tags for content item (specific + popular fill)", _tags.Count);
                        return;
                    }
                }

                // Fallback: no SectionName or API returned empty — show tags with count=1
                _tags = [.. distinctTags
                    .Select(t => new TagCloudItem { Tag = t, Count = 1, Size = TagSize.Medium })];

                Logger.LogDebug("Using {Count} provided tags for content item (static count=1)", _tags.Count);
                return;
            }

            // Explicit values - "all" is the default for both section and collection
            var effectiveSectionName = string.IsNullOrWhiteSpace(SectionName) ? "all" : SectionName;
            var effectiveCollectionName = string.IsNullOrWhiteSpace(CollectionName) ? "all" : CollectionName;

            Logger.LogDebug("Loading tag cloud for section: {SectionName}, collection: {CollectionName}, with {FilterCount} filter tags",
                effectiveSectionName, effectiveCollectionName, _selectedTagsInternal.Count);

            // Create filter list from selected tags (for content filtering)
            List<string>? filterTags = _selectedTagsInternal.Count > 0
                ? [.. _selectedTagsInternal]
                : null;

            // Pass selected tags as tagsToCount to ensure they always appear in results
            // (even if they're not in the top N popular tags for current filters).
            // The API returns their counts + fills remaining slots with popular tags.
            // lastDays is always null — the API applies its configured default (90 days).
            // Callers that need date filtering pass explicit FromDate/ToDate instead.
            var tags = await ApiClient.GetTagCloudAsync(
                effectiveSectionName,
                effectiveCollectionName,
                MaxTags,
                MinUses,
                lastDays: null,
                selectedTags: filterTags,
                tagsToCount: filterTags, // Include selected tags in results
                fromDate: FromDate,
                toDate: ToDate,
                searchQuery: SearchQuery);

            // Reorder: selected tags first, then remaining popular tags
            if (_selectedTagsInternal.Count > 0 && tags != null && tags.Count > 0)
            {
                var selected = new List<TagCloudItem>();
                var rest = new List<TagCloudItem>();
                foreach (var tag in tags)
                {
                    if (_selectedTagsInternal.Contains(tag.Tag.Trim().ToLowerInvariant()))
                    {
                        selected.Add(tag);
                    }
                    else
                    {
                        rest.Add(tag);
                    }
                }

                _tags = [.. selected, .. rest];
                Logger.LogDebug("Loaded {SelectedCount} selected + {PopularCount} popular tags",
                    selected.Count, rest.Count);
            }
            else
            {
                _tags = tags;
            }
        }
        // Suppress CA1031: Catching all exceptions is appropriate for component error handling
        // We log the error and set hasError flag to show error UI to user
#pragma warning disable CA1031
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load tag cloud for section {SectionName}, collection {CollectionName}",
                SectionName ?? "all", CollectionName ?? "all");
            _hasError = true;
            _tags = null;
        }
#pragma warning restore CA1031
        finally
        {
            _isLoading = false;
        }
    }

    private async Task HandleTagClick(string tag)
    {
        if (NavigationMode == TagCloudNavigationMode.Navigate)
        {
            // Navigate mode: Go to a different URL with the tag
            var baseUrl = NavigationBaseUrl ?? "/all";
            var encodedTag = Uri.EscapeDataString(tag.ToLowerInvariant());
            var targetUrl = $"{baseUrl}?tags={encodedTag}";

            Logger.LogDebug("Navigating to {Url} with tag {Tag}", targetUrl, tag);
            NavigationManager.NavigateTo(targetUrl);
        }
        else
        {
            // Filter mode: Toggle tag selection and update URL in-place
            await ToggleTagAndUpdateUrl(tag);
        }
    }

    private async Task ToggleTagAndUpdateUrl(string tag)
    {
        // Normalize tag for consistent comparison
        var normalizedTag = tag.Trim().ToLowerInvariant();

        // Toggle selection
#pragma warning disable CA1868 // Unnecessary call to 'Contains(item)'
        if (_selectedTagsInternal.Contains(normalizedTag))
        {
            _selectedTagsInternal.Remove(normalizedTag);
            Logger.LogDebug("Deselected tag: {Tag}", normalizedTag);
        }
        else
        {
            _selectedTagsInternal.Add(normalizedTag);
            Logger.LogDebug("Selected tag: {Tag}", normalizedTag);
        }
#pragma warning restore CA1868 // Unnecessary call to 'Contains(item)'

        // Update previous tags for change detection
        _previousSelectedTags = new HashSet<string>(_selectedTagsInternal, StringComparer.OrdinalIgnoreCase);

        // Reload tag cloud immediately with new filter state
        await LoadTagsAsync();

        // Raise event to notify parent component (parent handles URL state)
        await OnSelectionChanged.InvokeAsync([.. _selectedTagsInternal]);
    }

    private string GetPersistedStateKey()
    {
        var section = string.IsNullOrWhiteSpace(SectionName) ? "all" : SectionName;
        var collection = string.IsNullOrWhiteSpace(CollectionName) ? "all" : CollectionName;
        return $"tag-cloud-{section}-{collection}";
    }

    private Task PersistTagCloudState()
    {
        // Only persist if we loaded tags from API (not when Tags parameter is provided directly)
        if (Tags == null || Tags.Count == 0)
        {
            ApplicationState.PersistAsJson(GetPersistedStateKey(), new PersistedTagCloudData
            {
                Tags = _tags?.ToList()
            });
        }
        return Task.CompletedTask;
    }

    private sealed class PersistedTagCloudData
    {
        public List<TagCloudItem>? Tags { get; set; }
    }

    public void Dispose()
    {
        _persistSubscription?.Dispose();
        GC.SuppressFinalize(this);
    }
}
