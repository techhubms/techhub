using FluentAssertions;
using TechHub.Infrastructure.Services.RoundupGeneration;

namespace TechHub.Infrastructure.Tests.Services;

public class RoundupContentBuilderTests
{
    [Fact]
    public void BuildTableOfContents_WithSimpleHeading_BuildsExpectedAnchor()
    {
        var content = """
            ## AI
            """;

        var toc = RoundupContentBuilder.BuildTableOfContents(content);

        toc.Should().Be("- [AI](#ai)");
    }

    [Fact]
    public void BuildTableOfContents_WithSpecialCharacters_StripsPunctuationFromAnchor()
    {
        var content = """
            ## .NET & MAUI testing updates

            ### What's new in C#?
            """;

        var toc = RoundupContentBuilder.BuildTableOfContents(content);

        toc.Should().Contain("- [.NET & MAUI testing updates](#net-maui-testing-updates)");
        toc.Should().Contain("  - [What's new in C#?](#whats-new-in-c)");
    }
}
