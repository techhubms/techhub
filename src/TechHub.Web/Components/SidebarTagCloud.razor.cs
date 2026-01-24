using Microsoft.AspNetCore.Components;
using TechHub.Core.DTOs;
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
/// Tag cloud component for sidebar filtering
/// Displays tags with usage counts and size categories
/// Supports multiple tag selection with OR logic
/// </summary>
public partial class SidebarTagCloud : ComponentBase
{
    [Inject]
    private ITechHubApiClient ApiClient { get; set; } = default!;

    [Inject]
    private ILogger<SidebarTagCloud> Logger { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Title for the tag cloud section
    /// </summary>
    [Parameter]
    public string Title { get; set; } = "Tags";

    /// <summary>
    /// Scope for tag cloud (Homepage, Section, Collection, Content)
    /// </summary>
    [Parameter]
    public TagCloudScope Scope { get; set; }

    /// <summary>
    /// Section name (required for Section/Collection/Content scopes)
    /// </summary>
    [Parameter]
    public string? SectionName { get; set; }

    /// <summary>
    /// Collection name (required for Collection scope)
    /// </summary>
    [Parameter]
    public string? CollectionName { get; set; }

    /// <summary>
    /// Content item slug (required for Content scope)
    /// </summary>
    [Parameter]
    public string? Slug { get; set; }

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
    /// Only include tags from content published within this many days (default: 90)
    /// </summary>
    [Parameter]
    public int LastDays { get; set; } = 90;

    private IReadOnlyList<TagCloudItem>? tags;
    private HashSet<string> selectedTagsInternal = [];
    private bool isLoading = true;
    private bool hasError;
    private bool hasInitialized; // Track if we've loaded tags to prevent double-load flicker

    protected override async Task OnInitializedAsync()
    {
        // Prevent double initialization when transitioning from SSR to interactive mode
        if (hasInitialized)
        {
            return;
        }

        hasInitialized = true;

        // Initialize selected tags from parameter (deduplicate and normalize)
        if (SelectedTags != null)
        {
            selectedTagsInternal = new HashSet<string>(
                SelectedTags.Select(t => t.Trim().ToLowerInvariant()),
                StringComparer.OrdinalIgnoreCase);
        }

        await LoadTagsAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        // Always sync selectedTagsInternal with SelectedTags parameter
        // This is critical for Filter mode where URL changes trigger parameter updates
        SyncSelectedTagsFromParameter();
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
            if (!selectedTagsInternal.SetEquals(normalizedTags))
            {
                selectedTagsInternal = normalizedTags;
                Logger.LogDebug("Synced selected tags from parameter: {Tags}",
                    string.Join(", ", selectedTagsInternal));
            }
        }
        else if (selectedTagsInternal.Count > 0)
        {
            // Parameter is null but we have selections - clear them
            selectedTagsInternal.Clear();
            Logger.LogDebug("Cleared selected tags (parameter is null)");
        }
    }

    private async Task LoadTagsAsync()
    {
        try
        {
            isLoading = true;
            hasError = false;

            Logger.LogInformation("Loading tag cloud for scope: {Scope}, section: {SectionName}, collection: {CollectionName}",
                Scope, SectionName ?? "(none)", CollectionName ?? "(none)");

            tags = await ApiClient.GetTagCloudAsync(
                Scope,
                SectionName,
                CollectionName,
                Slug,
                MaxTags,
                MinUses,
                LastDays);

            Logger.LogInformation("Successfully loaded {Count} tags", tags?.Count ?? 0);
        }
        // Suppress CA1031: Catching all exceptions is appropriate for component error handling
        // We log the error and set hasError flag to show error UI to user
#pragma warning disable CA1031
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load tag cloud for scope {Scope}", Scope);
            hasError = true;
            tags = null;
        }
#pragma warning restore CA1031
        finally
        {
            isLoading = false;
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
        if (selectedTagsInternal.Contains(normalizedTag))
        {
            selectedTagsInternal.Remove(normalizedTag);
            Logger.LogDebug("Deselected tag: {Tag}", normalizedTag);
        }
        else
        {
            selectedTagsInternal.Add(normalizedTag);
            Logger.LogDebug("Selected tag: {Tag}", normalizedTag);
        }
#pragma warning restore CA1868 // Unnecessary call to 'Contains(item)'

        // Update URL with new tag selection
        UpdateUrlWithTags();

        // Raise event with current selection
        await OnSelectionChanged.InvokeAsync([.. selectedTagsInternal]);
    }

    private void UpdateUrlWithTags()
    {
        var currentUri = new Uri(NavigationManager.Uri);
        var basePath = currentUri.GetLeftPart(UriPartial.Path);

        if (selectedTagsInternal.Count == 0)
        {
            // No tags selected - navigate to URL without tags parameter
            NavigationManager.NavigateTo(basePath, replace: true);
        }
        else
        {
            // Build URL with tags parameter
            var tagsParam = string.Join(",", selectedTagsInternal.Select(t => Uri.EscapeDataString(t.ToLowerInvariant())));
            var targetUrl = $"{basePath}?tags={tagsParam}";
            NavigationManager.NavigateTo(targetUrl, replace: true);
        }
    }
}
