namespace TechHub.Core.Configuration;

/// <summary>
/// Configuration options for tag cloud generation
/// </summary>
public class TagCloudOptions
{
    public int DefaultMaxTags { get; set; } = 20;
    public int MinimumTagUses { get; set; } = 1;
    public int DefaultDateRangeDays { get; set; } = 90;
    public QuantilePercentilesOptions QuantilePercentiles { get; set; } = new();
}

public class QuantilePercentilesOptions
{
    public double SmallToMedium { get; set; } = 0.25;
    public double MediumToLarge { get; set; } = 0.75;
}
