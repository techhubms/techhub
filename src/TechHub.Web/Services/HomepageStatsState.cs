namespace TechHub.Web.Services;

/// <summary>
/// Circuit-scoped state shared between Home.razor and SubNav.razor.
/// Home.razor populates stats after loading; SubNav.razor reads them to display in the subnav bar.
/// </summary>
internal sealed class HomepageStatsState
{
    public long? RecentCount { get; private set; }
    public long? TotalCount { get; private set; }
    public string? WeekAgoDate { get; private set; }
    public int SectionsCount { get; private set; }

    public event Action? OnChanged;

    public void Update(long? recentCount, long? totalCount, string weekAgoDate, int sectionsCount)
    {
        RecentCount = recentCount;
        TotalCount = totalCount;
        WeekAgoDate = weekAgoDate;
        SectionsCount = sectionsCount;
        OnChanged?.Invoke();
    }
}
