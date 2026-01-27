namespace TechHub.Core.Models;

/// <summary>
/// Specific data structure for the Developer Experience Space page
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "DevEx is the standard abbreviation for Developer Experience")]
public record DXSpacePageData
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required DXIntroSection Intro { get; init; }
    public required DXDoraSection Dora { get; init; }
    public required DXSpaceSection Space { get; init; }
    public required DXDevExSection DevEx { get; init; }
    public required DXRelationshipsSection Relationships { get; init; }
    public required DXGettingStartedSection GettingStarted { get; init; }
    public required DXToolsSection Tools { get; init; }
    public required DXBestPracticesSection BestPractices { get; init; }
}

// ============================================================================
// Common/Reusable Types
// ============================================================================

/// <summary>
/// Reusable insight box that appears in multiple sections
/// </summary>
public record DXInsightBox
{
    public required string Heading { get; init; }
    public required string Content { get; init; }
}

/// <summary>
/// Reusable quote with author attribution
/// </summary>
public record DXQuote
{
    public required string Text { get; init; }
    public required string Author { get; init; }
}

/// <summary>
/// Generic name/description pair used throughout
/// </summary>
public record DXNamedItem
{
    public required string Name { get; init; }
    public required string Description { get; init; }
}

/// <summary>
/// Generic titled item with description
/// </summary>
public record DXTitledItem
{
    public required string Title { get; init; }
    public required string Description { get; init; }
}

/// <summary>
/// Generic item with icon, name, and description
/// </summary>
public record DXIconItem
{
    public required string Icon { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
}

/// <summary>
/// Generic titled item with icon
/// </summary>
public record DXIconTitledItem
{
    public required string Icon { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
}

/// <summary>
/// Generic card with title and list of points
/// </summary>
public record DXCard
{
    public required string Title { get; init; }
    public required List<string> Points { get; init; }
}

/// <summary>
/// Stat with number and description
/// </summary>
public record DXStat
{
    public required string Number { get; init; }
    public required string Description { get; init; }
}

// ============================================================================
// Section Types
// ============================================================================

public record DXIntroSection
{
    public required string Content { get; init; }
    public required DXQuote Quote { get; init; }
}

public record DXDoraSection
{
    public required string Title { get; init; }
    public required string IntroHeading { get; init; }
    public required List<string> IntroContent { get; init; }
    public required string MetricsHeading { get; init; }
    public required List<DXDoraMetric> Metrics { get; init; }
    public required DXInsightBox Insight { get; init; }
}

public record DXDoraMetric
{
    public required string Icon { get; init; }
    public required string Name { get; init; }
    public required string Question { get; init; }
    public required string WhyItMatters { get; init; }
    public required string ElitePerformance { get; init; }
    public required List<string> Improvements { get; init; }
}

public record DXSpaceSection
{
    public required string Title { get; init; }
    public required string IntroHeading { get; init; }
    public required List<string> IntroContent { get; init; }
    public required string DimensionsHeading { get; init; }
    public required List<DXSpaceDimension> Dimensions { get; init; }
    public required DXInsightBox Insight { get; init; }
}

public record DXSpaceDimension
{
    public required string Letter { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string WhyItMatters { get; init; }
    public required List<string> ExampleMetrics { get; init; }
    public required string AdditionalInfo { get; init; }
}

public record DXDevExSection
{
    public required string Title { get; init; }
    public required string IntroHeading { get; init; }
    public required List<string> IntroContent { get; init; }
    public required List<DXStat> Stats { get; init; }
    public required string DimensionsHeading { get; init; }
    public required List<DXDevExDimension> Dimensions { get; init; }
    public required DXInsightBox Insight { get; init; }
}

public record DXDevExDimension
{
    public required string Icon { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string FactorsHeading { get; init; }
    public required List<string> Factors { get; init; }
    public required string ImprovementHeading { get; init; }
    public required List<string> Improvements { get; init; }
}

public record DXRelationshipsSection
{
    public required string Title { get; init; }
    public required string IntroHeading { get; init; }
    public required string IntroContent { get; init; }
    public required List<DXFrameworkComparison> Frameworks { get; init; }
    public required string ChoiceHeading { get; init; }
    public required List<DXCard> Choices { get; init; }
    public required DXInsightBox Insight { get; init; }
}

public record DXFrameworkComparison
{
    public required string Icon { get; init; }
    public required string Name { get; init; }
    public required string Focus { get; init; }
    public required string Scope { get; init; }
    public required string BestFor { get; init; }
    public required string AdditionalInfo { get; init; }
}

public record DXGettingStartedSection
{
    public required string Title { get; init; }
    public required string IntroHeading { get; init; }
    public required string IntroContent { get; init; }
    public required string StepsHeading { get; init; }
    public required List<DXStep> Steps { get; init; }
}

public record DXStep
{
    public required int Number { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string TipsHeading { get; init; }
    public required List<string> Tips { get; init; }
}

public record DXToolsSection
{
    public required string Title { get; init; }
    public required string IntroHeading { get; init; }
    public required string IntroContent { get; init; }
    public required string ToolsHeading { get; init; }
    public required List<DXTool> Tools { get; init; }
    public required string ResourcesHeading { get; init; }
    public required List<DXIconTitledItem> Resources { get; init; }
}

public record DXTool
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string Description { get; init; }
}

public record DXBestPracticesSection
{
    public required string Title { get; init; }
    public required string IntroHeading { get; init; }
    public required string IntroContent { get; init; }
    public required string DoHeading { get; init; }
    public required List<string> DoPractices { get; init; }
    public required string DontHeading { get; init; }
    public required List<string> DontPractices { get; init; }
    public required DXInsightBox Insight { get; init; }
}
