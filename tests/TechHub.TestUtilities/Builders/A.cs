namespace TechHub.TestUtilities.Builders;

/// <summary>
/// Static entry point for all test builders using the "A" pattern.
/// Usage: A.ContentItem.Build() or A.ContentItem.WithTitle("test").Build()
/// </summary>
public static class A
{
    public static ContentItemBuilder ContentItem => new();
    public static SectionBuilder Section => new();
    public static CollectionBuilder Collection => new();
}
