namespace TechHub.Core.Configuration;

/// <summary>
/// Configuration options for filtering functionality
/// </summary>
public class FilteringOptions
{
    public TagCloudOptions TagCloud { get; set; } = new();
    public DateRangeOptions DateRange { get; set; } = new();
    public TagDropdownOptions TagDropdown { get; set; } = new();
}

public class TagCloudOptions
{
    public int DefaultMaxTags { get; set; } = 20;
    public int MinimumTagUses { get; set; } = 5;
    public int DefaultDateRangeDays { get; set; } = 90;
    public QuantilePercentilesOptions QuantilePercentiles { get; set; } = new();
}

public class QuantilePercentilesOptions
{
    public double SmallToMedium { get; set; } = 0.25;
    public double MediumToLarge { get; set; } = 0.75;
}

public class DateRangeOptions
{
    public int DefaultDays { get; set; } = 90;
    public PresetsOptions Presets { get; set; } = new();
}

public class PresetsOptions
{
    public int Last7Days { get; set; } = 7;
    public int Last30Days { get; set; } = 30;
    public int Last90Days { get; set; } = 90;
}

public class TagDropdownOptions
{
    public int VirtualScrollThreshold { get; set; } = 50;
    public bool EnableVirtualScroll { get; set; } = true;
}
