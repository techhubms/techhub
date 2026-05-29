using FluentAssertions;
using TechHub.Infrastructure.Services.Newsletter;

namespace TechHub.Infrastructure.Tests.Services;

public class NewsletterTemplateResourceTests
{
    [Fact]
    public void NewsletterRoundupTemplate_ShouldContainResponsiveLayoutMarkers()
    {
        const string ResourceName = "TechHub.Infrastructure.Data.Resources.newsletter-roundup-template.html";

        using var stream = typeof(NewsletterService).Assembly.GetManifestResourceStream(ResourceName);
        stream.Should().NotBeNull();

        using var reader = new StreamReader(stream!);
        var template = reader.ReadToEnd();

        template.Should().Contain("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />");
        template.Should().Contain("width=\"100%\"");
        template.Should().Contain("max-width:900px");
    }
}
